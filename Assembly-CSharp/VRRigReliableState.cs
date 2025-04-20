using System;
using System.Collections.Generic;
using Fusion;
using GorillaNetworking;
using GorillaTagScripts;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class VRRigReliableState : MonoBehaviour, IWrappedSerializable, INetworkStruct
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06001764 RID: 5988 RVA: 0x0003FDEC File Offset: 0x0003DFEC
	public bool HasBracelet
	{
		get
		{
			return this.braceletBeadColors.Count > 0;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06001765 RID: 5989 RVA: 0x0003FDFC File Offset: 0x0003DFFC
	// (set) Token: 0x06001766 RID: 5990 RVA: 0x0003FE04 File Offset: 0x0003E004
	public bool isDirty { get; private set; } = true;

	// Token: 0x06001767 RID: 5991 RVA: 0x000C7E5C File Offset: 0x000C605C
	private void Awake()
	{
		VRRig.newPlayerJoined = (Action)Delegate.Combine(VRRig.newPlayerJoined, new Action(this.SetIsDirty));
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.SetIsDirty));
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x0003FE0D File Offset: 0x0003E00D
	private void OnDestroy()
	{
		VRRig.newPlayerJoined = (Action)Delegate.Remove(VRRig.newPlayerJoined, new Action(this.SetIsDirty));
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x0003FE2F File Offset: 0x0003E02F
	public void SetIsDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x0003FE38 File Offset: 0x0003E038
	public void SetIsNotDirty()
	{
		this.isDirty = false;
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000C7EAC File Offset: 0x000C60AC
	public void SharedStart(bool isOfflineVRRig_, BodyDockPositions bDock_)
	{
		this.isOfflineVRRig = isOfflineVRRig_;
		this.bDock = bDock_;
		this.activeTransferrableObjectIndex = new int[5];
		for (int i = 0; i < this.activeTransferrableObjectIndex.Length; i++)
		{
			this.activeTransferrableObjectIndex[i] = -1;
		}
		this.transferrablePosStates = new TransferrableObject.PositionState[5];
		this.transferrableItemStates = new TransferrableObject.ItemStates[5];
		this.transferableDockPositions = new BodyDockPositions.DropPositions[5];
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000C7F14 File Offset: 0x000C6114
	void IWrappedSerializable.OnSerializeRead(object newData)
	{
		this.Data = (ReliableStateData)newData;
		long header = this.Data.Header;
		int num;
		this.SetHeader(header, out num);
		for (int i = 0; i < this.activeTransferrableObjectIndex.Length; i++)
		{
			if ((header & 1L << (i & 31)) != 0L)
			{
				long num2 = this.Data.TransferrableStates[i];
				this.activeTransferrableObjectIndex[i] = (int)num2;
				this.transferrablePosStates[i] = (TransferrableObject.PositionState)(num2 >> 32 & 255L);
				this.transferrableItemStates[i] = (TransferrableObject.ItemStates)(num2 >> 40 & 255L);
				this.transferableDockPositions[i] = (BodyDockPositions.DropPositions)(num2 >> 48 & 255L);
			}
			else
			{
				this.activeTransferrableObjectIndex[i] = -1;
				this.transferrablePosStates[i] = TransferrableObject.PositionState.None;
				this.transferrableItemStates[i] = (TransferrableObject.ItemStates)0;
				this.transferableDockPositions[i] = BodyDockPositions.DropPositions.None;
			}
		}
		this.wearablesPackedStates = this.Data.WearablesPackedState;
		this.lThrowableProjectileIndex = this.Data.LThrowableProjectileIndex;
		this.rThrowableProjectileIndex = this.Data.RThrowableProjectileIndex;
		this.sizeLayerMask = this.Data.SizeLayerMask;
		this.randomThrowableIndex = this.Data.RandomThrowableIndex;
		this.braceletBeadColors.Clear();
		if (num > 0)
		{
			if (num <= 3)
			{
				int num3 = (int)this.Data.PackedBeads;
				this.braceletSelfIndex = num3 >> 30;
				VRRigReliableState.UnpackBeadColors((long)num3, 0, num, this.braceletBeadColors);
			}
			else
			{
				long packedBeads = this.Data.PackedBeads;
				this.braceletSelfIndex = (int)(packedBeads >> 60);
				if (num <= 6)
				{
					VRRigReliableState.UnpackBeadColors(packedBeads, 0, num, this.braceletBeadColors);
				}
				else
				{
					VRRigReliableState.UnpackBeadColors(packedBeads, 0, 6, this.braceletBeadColors);
					VRRigReliableState.UnpackBeadColors(this.Data.PackedBeadsMoreThan6, 6, num, this.braceletBeadColors);
				}
			}
		}
		this.bDock.RefreshTransferrableItems();
		this.bDock.myRig.UpdateFriendshipBracelet();
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000C80F0 File Offset: 0x000C62F0
	object IWrappedSerializable.OnSerializeWrite()
	{
		this.isDirty = false;
		ReliableStateData reliableStateData = default(ReliableStateData);
		long header = this.GetHeader();
		reliableStateData.Header = header;
		long[] array = this.GetTransferrableStates(header).ToArray();
		reliableStateData.TransferrableStates.CopyFrom(array, 0, array.Length);
		reliableStateData.WearablesPackedState = this.wearablesPackedStates;
		reliableStateData.LThrowableProjectileIndex = this.lThrowableProjectileIndex;
		reliableStateData.RThrowableProjectileIndex = this.rThrowableProjectileIndex;
		reliableStateData.SizeLayerMask = this.sizeLayerMask;
		reliableStateData.RandomThrowableIndex = this.randomThrowableIndex;
		if (this.braceletBeadColors.Count > 0)
		{
			long num = VRRigReliableState.PackBeadColors(this.braceletBeadColors, 0);
			if (this.braceletBeadColors.Count <= 3)
			{
				num |= (long)this.braceletSelfIndex << 30;
				reliableStateData.PackedBeads = num;
			}
			else
			{
				num |= (long)this.braceletSelfIndex << 60;
				reliableStateData.PackedBeads = num;
				if (this.braceletBeadColors.Count > 6)
				{
					reliableStateData.PackedBeadsMoreThan6 = VRRigReliableState.PackBeadColors(this.braceletBeadColors, 6);
				}
			}
		}
		this.Data = reliableStateData;
		return reliableStateData;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000C8208 File Offset: 0x000C6408
	void IWrappedSerializable.OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.isDirty)
		{
			return;
		}
		this.isDirty = false;
		long header = this.GetHeader();
		stream.SendNext(header);
		foreach (long num in this.GetTransferrableStates(header))
		{
			stream.SendNext(num);
		}
		stream.SendNext(this.wearablesPackedStates);
		stream.SendNext(this.lThrowableProjectileIndex);
		stream.SendNext(this.rThrowableProjectileIndex);
		stream.SendNext(this.sizeLayerMask);
		stream.SendNext(this.randomThrowableIndex);
		if (this.braceletBeadColors.Count > 0)
		{
			long num2 = VRRigReliableState.PackBeadColors(this.braceletBeadColors, 0);
			if (this.braceletBeadColors.Count <= 3)
			{
				num2 |= (long)this.braceletSelfIndex << 30;
				stream.SendNext((int)num2);
				return;
			}
			num2 |= (long)this.braceletSelfIndex << 60;
			stream.SendNext(num2);
			if (this.braceletBeadColors.Count > 6)
			{
				stream.SendNext(VRRigReliableState.PackBeadColors(this.braceletBeadColors, 6));
			}
		}
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000C835C File Offset: 0x000C655C
	void IWrappedSerializable.OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
		long num = (long)stream.ReceiveNext();
		this.isMicEnabled = ((num & 32L) != 0L);
		this.isBraceletLeftHanded = ((num & 64L) != 0L);
		this.isBuilderWatchEnabled = ((num & 128L) != 0L);
		int num2 = (int)(num >> 12) & 15;
		this.lThrowableProjectileColor.r = (byte)(num >> 16);
		this.lThrowableProjectileColor.g = (byte)(num >> 24);
		this.lThrowableProjectileColor.b = (byte)(num >> 32);
		this.rThrowableProjectileColor.r = (byte)(num >> 40);
		this.rThrowableProjectileColor.g = (byte)(num >> 48);
		this.rThrowableProjectileColor.b = (byte)(num >> 56);
		for (int i = 0; i < this.activeTransferrableObjectIndex.Length; i++)
		{
			if ((num & 1L << (i & 31)) != 0L)
			{
				long num3 = (long)stream.ReceiveNext();
				this.activeTransferrableObjectIndex[i] = (int)num3;
				this.transferrablePosStates[i] = (TransferrableObject.PositionState)(num3 >> 32 & 255L);
				this.transferrableItemStates[i] = (TransferrableObject.ItemStates)(num3 >> 40 & 255L);
				this.transferableDockPositions[i] = (BodyDockPositions.DropPositions)(num3 >> 48 & 255L);
			}
			else
			{
				this.activeTransferrableObjectIndex[i] = -1;
				this.transferrablePosStates[i] = TransferrableObject.PositionState.None;
				this.transferrableItemStates[i] = (TransferrableObject.ItemStates)0;
				this.transferableDockPositions[i] = BodyDockPositions.DropPositions.None;
			}
		}
		this.wearablesPackedStates = (int)stream.ReceiveNext();
		this.lThrowableProjectileIndex = (int)stream.ReceiveNext();
		this.rThrowableProjectileIndex = (int)stream.ReceiveNext();
		this.sizeLayerMask = (int)stream.ReceiveNext();
		this.randomThrowableIndex = (int)stream.ReceiveNext();
		this.braceletBeadColors.Clear();
		if (num2 > 0)
		{
			if (num2 <= 3)
			{
				int num4 = (int)stream.ReceiveNext();
				this.braceletSelfIndex = num4 >> 30;
				VRRigReliableState.UnpackBeadColors((long)num4, 0, num2, this.braceletBeadColors);
			}
			else
			{
				long num5 = (long)stream.ReceiveNext();
				this.braceletSelfIndex = (int)(num5 >> 60);
				if (num2 <= 6)
				{
					VRRigReliableState.UnpackBeadColors(num5, 0, num2, this.braceletBeadColors);
				}
				else
				{
					VRRigReliableState.UnpackBeadColors(num5, 0, 6, this.braceletBeadColors);
					VRRigReliableState.UnpackBeadColors((long)stream.ReceiveNext(), 6, num2, this.braceletBeadColors);
				}
			}
		}
		if (CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			this.bDock.RefreshTransferrableItems();
		}
		this.bDock.myRig.UpdateFriendshipBracelet();
		this.bDock.myRig.EnableBuilderResizeWatch(this.isBuilderWatchEnabled);
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x000C85CC File Offset: 0x000C67CC
	private long GetHeader()
	{
		long num = 0L;
		if (CosmeticsController.instance.isHidingCosmeticsFromRemotePlayers)
		{
			for (int i = 0; i < this.activeTransferrableObjectIndex.Length; i++)
			{
				if (this.activeTransferrableObjectIndex[i] != -1 && (this.transferrablePosStates[i] == TransferrableObject.PositionState.InLeftHand || this.transferrablePosStates[i] == TransferrableObject.PositionState.InRightHand))
				{
					num |= (long)((ulong)((byte)(1 << i)));
				}
			}
		}
		else
		{
			for (int j = 0; j < this.activeTransferrableObjectIndex.Length; j++)
			{
				if (this.activeTransferrableObjectIndex[j] != -1)
				{
					num |= (long)((ulong)((byte)(1 << j)));
				}
			}
		}
		if (this.isBraceletLeftHanded)
		{
			num |= 64L;
		}
		if (this.isMicEnabled)
		{
			num |= 32L;
		}
		if (this.isBuilderWatchEnabled && !CosmeticsController.instance.isHidingCosmeticsFromRemotePlayers)
		{
			num |= 128L;
		}
		num |= ((long)this.braceletBeadColors.Count & 15L) << 12;
		num |= (long)((long)((ulong)this.lThrowableProjectileColor.r) << 16);
		num |= (long)((long)((ulong)this.lThrowableProjectileColor.g) << 24);
		num |= (long)((long)((ulong)this.lThrowableProjectileColor.b) << 32);
		num |= (long)((long)((ulong)this.rThrowableProjectileColor.r) << 40);
		num |= (long)((long)((ulong)this.rThrowableProjectileColor.g) << 48);
		return num | (long)((long)((ulong)this.rThrowableProjectileColor.b) << 56);
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x000C8714 File Offset: 0x000C6914
	private void SetHeader(long header, out int numBeadsToRead)
	{
		this.isMicEnabled = ((header & 32L) != 0L);
		this.isBraceletLeftHanded = ((header & 64L) != 0L);
		numBeadsToRead = ((int)(header >> 12) & 15);
		this.lThrowableProjectileColor.r = (byte)(header >> 16);
		this.lThrowableProjectileColor.g = (byte)(header >> 24);
		this.lThrowableProjectileColor.b = (byte)(header >> 32);
		this.rThrowableProjectileColor.r = (byte)(header >> 40);
		this.rThrowableProjectileColor.g = (byte)(header >> 48);
		this.rThrowableProjectileColor.b = (byte)(header >> 56);
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x000C87AC File Offset: 0x000C69AC
	private List<long> GetTransferrableStates(long header)
	{
		List<long> list = new List<long>();
		for (int i = 0; i < this.activeTransferrableObjectIndex.Length; i++)
		{
			if ((header & 1L << (i & 31)) != 0L && this.activeTransferrableObjectIndex[i] != -1)
			{
				long num = (long)((ulong)this.activeTransferrableObjectIndex[i]);
				num |= (long)this.transferrablePosStates[i] << 32;
				num |= (long)this.transferrableItemStates[i] << 40;
				num |= (long)this.transferableDockPositions[i] << 48;
				list.Add(num);
			}
		}
		return list;
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000C8828 File Offset: 0x000C6A28
	private static long PackBeadColors(List<Color> beadColors, int fromIndex)
	{
		long num = 0L;
		int num2 = Mathf.Min(fromIndex + 6, beadColors.Count);
		int num3 = 0;
		for (int i = fromIndex; i < num2; i++)
		{
			long num4 = (long)FriendshipGroupDetection.PackColor(beadColors[i]);
			num |= num4 << num3;
			num3 += 10;
		}
		return num;
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000C8874 File Offset: 0x000C6A74
	private static void UnpackBeadColors(long packed, int startIndex, int endIndex, List<Color> beadColorsResult)
	{
		int num = Mathf.Min(startIndex + 6, endIndex);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			short data = (short)(packed >> num2 & 1023L);
			beadColorsResult.Add(FriendshipGroupDetection.UnpackColor(data));
			num2 += 10;
		}
	}

	// Token: 0x04001A03 RID: 6659
	[NonSerialized]
	public int[] activeTransferrableObjectIndex;

	// Token: 0x04001A04 RID: 6660
	[NonSerialized]
	public TransferrableObject.PositionState[] transferrablePosStates;

	// Token: 0x04001A05 RID: 6661
	[NonSerialized]
	public TransferrableObject.ItemStates[] transferrableItemStates;

	// Token: 0x04001A06 RID: 6662
	[NonSerialized]
	public BodyDockPositions.DropPositions[] transferableDockPositions;

	// Token: 0x04001A07 RID: 6663
	[NonSerialized]
	public int wearablesPackedStates;

	// Token: 0x04001A08 RID: 6664
	[NonSerialized]
	public int lThrowableProjectileIndex = -1;

	// Token: 0x04001A09 RID: 6665
	[NonSerialized]
	public int rThrowableProjectileIndex = -1;

	// Token: 0x04001A0A RID: 6666
	[NonSerialized]
	public Color32 lThrowableProjectileColor = Color.white;

	// Token: 0x04001A0B RID: 6667
	[NonSerialized]
	public Color32 rThrowableProjectileColor = Color.white;

	// Token: 0x04001A0C RID: 6668
	[NonSerialized]
	public int randomThrowableIndex;

	// Token: 0x04001A0D RID: 6669
	[NonSerialized]
	public bool isMicEnabled;

	// Token: 0x04001A0E RID: 6670
	private bool isOfflineVRRig;

	// Token: 0x04001A0F RID: 6671
	private BodyDockPositions bDock;

	// Token: 0x04001A10 RID: 6672
	[NonSerialized]
	public int sizeLayerMask = 1;

	// Token: 0x04001A11 RID: 6673
	private const long IS_MIC_ENABLED_BIT = 32L;

	// Token: 0x04001A12 RID: 6674
	private const long BRACELET_LEFTHAND_BIT = 64L;

	// Token: 0x04001A13 RID: 6675
	private const long BUILDER_WATCH_ENABLED_BIT = 128L;

	// Token: 0x04001A14 RID: 6676
	private const int BRACELET_NUM_BEADS_SHIFT = 12;

	// Token: 0x04001A15 RID: 6677
	private const int LPROJECTILECOLOR_R_SHIFT = 16;

	// Token: 0x04001A16 RID: 6678
	private const int LPROJECTILECOLOR_G_SHIFT = 24;

	// Token: 0x04001A17 RID: 6679
	private const int LPROJECTILECOLOR_B_SHIFT = 32;

	// Token: 0x04001A18 RID: 6680
	private const int RPROJECTILECOLOR_R_SHIFT = 40;

	// Token: 0x04001A19 RID: 6681
	private const int RPROJECTILECOLOR_G_SHIFT = 48;

	// Token: 0x04001A1A RID: 6682
	private const int RPROJECTILECOLOR_B_SHIFT = 56;

	// Token: 0x04001A1B RID: 6683
	private const int POS_STATES_SHIFT = 32;

	// Token: 0x04001A1C RID: 6684
	private const int ITEM_STATES_SHIFT = 40;

	// Token: 0x04001A1D RID: 6685
	private const int DOCK_POSITIONS_SHIFT = 48;

	// Token: 0x04001A1E RID: 6686
	private const int BRACELET_SELF_INDEX_SHIFT = 60;

	// Token: 0x04001A1F RID: 6687
	[NonSerialized]
	public bool isBraceletLeftHanded;

	// Token: 0x04001A20 RID: 6688
	[NonSerialized]
	public int braceletSelfIndex;

	// Token: 0x04001A21 RID: 6689
	[NonSerialized]
	public List<Color> braceletBeadColors = new List<Color>(10);

	// Token: 0x04001A22 RID: 6690
	[NonSerialized]
	public bool isBuilderWatchEnabled;

	// Token: 0x04001A24 RID: 6692
	private ReliableStateData Data;
}
