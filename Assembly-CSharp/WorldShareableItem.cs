using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using GorillaNetworking;
using GorillaTag;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020003A9 RID: 937
[NetworkBehaviourWeaved(0)]
public class WorldShareableItem : NetworkComponent, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060015D9 RID: 5593 RVA: 0x0003EBC0 File Offset: 0x0003CDC0
	// (set) Token: 0x060015DA RID: 5594 RVA: 0x0003EBC8 File Offset: 0x0003CDC8
	[DevInspectorShow]
	public TransferrableObject.PositionState transferableObjectState { get; set; }

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060015DB RID: 5595 RVA: 0x0003EBD1 File Offset: 0x0003CDD1
	// (set) Token: 0x060015DC RID: 5596 RVA: 0x0003EBD9 File Offset: 0x0003CDD9
	public TransferrableObject.ItemStates transferableObjectItemState { get; set; }

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x060015DD RID: 5597 RVA: 0x0003EBE2 File Offset: 0x0003CDE2
	// (set) Token: 0x060015DE RID: 5598 RVA: 0x0003EBEA File Offset: 0x0003CDEA
	public TransferrableObject.PositionState transferableObjectStateNetworked { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060015DF RID: 5599 RVA: 0x0003EBF3 File Offset: 0x0003CDF3
	// (set) Token: 0x060015E0 RID: 5600 RVA: 0x0003EBFB File Offset: 0x0003CDFB
	public TransferrableObject.ItemStates transferableObjectItemStateNetworked { get; set; }

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0003EC04 File Offset: 0x0003CE04
	// (set) Token: 0x060015E2 RID: 5602 RVA: 0x0003EC0C File Offset: 0x0003CE0C
	[DevInspectorShow]
	public WorldTargetItem target
	{
		get
		{
			return this._target;
		}
		set
		{
			this._target = value;
		}
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x0003EC15 File Offset: 0x0003CE15
	protected override void Awake()
	{
		base.Awake();
		this.guard = base.GetComponent<RequestableOwnershipGuard>();
		this.teleportSerializer = base.GetComponent<TransformViewTeleportSerializer>();
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x0003EC45 File Offset: 0x0003CE45
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		if (GTAppState.isQuitting)
		{
			return;
		}
		base.OnEnable();
		this.guard.AddCallbackTarget(this);
		WorldShareableItemManager.Register(this);
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x000C1560 File Offset: 0x000BF760
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
		if (this.target == null || !this.target.transferrableObject.isSceneObject)
		{
			return;
		}
		PhotonView[] components = base.GetComponents<PhotonView>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].ViewID = 0;
		}
		this.transferableObjectState = TransferrableObject.PositionState.None;
		this.transferableObjectItemState = TransferrableObject.ItemStates.State0;
		this.guard.RemoveCallbackTarget(this);
		this.rpcCallBack = null;
		this.onOwnerChangeCb = null;
		WorldShareableItemManager.Unregister(this);
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x0003EC7D File Offset: 0x0003CE7D
	public void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		WorldShareableItemManager.Unregister(this);
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x000C15E0 File Offset: 0x000BF7E0
	public void SetupSharableViewIDs(NetPlayer player, int slotID)
	{
		PhotonView[] components = base.GetComponents<PhotonView>();
		PhotonView photonView = components[0];
		PhotonView photonView2 = components[1];
		int num = player.ActorNumber * 1000 + 990 + slotID * 2;
		this.guard.giveCreatorAbsoluteAuthority = true;
		if (num != photonView.ViewID)
		{
			photonView.ViewID = player.ActorNumber * 1000 + 990 + slotID * 2;
			photonView2.ViewID = player.ActorNumber * 1000 + 990 + slotID * 2 + 1;
			this.guard.SetCreator(player);
		}
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x000C166C File Offset: 0x000BF86C
	public void ResetViews()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		PhotonView[] components = base.GetComponents<PhotonView>();
		PhotonView photonView = components[0];
		PhotonView photonView2 = components[1];
		photonView.ViewID = 0;
		photonView2.ViewID = 0;
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x000C169C File Offset: 0x000BF89C
	public void SetupSharableObject(int itemIDx, NetPlayer owner, Transform targetXform)
	{
		this.target = WorldTargetItem.GenerateTargetFromPlayerAndID(owner, itemIDx);
		if (this.target.targetObject != targetXform)
		{
			Debug.LogError(string.Format("The target object found a transform that does not match the target transform, this should never happen. owner: {0} itemIDx: {1} targetXformPath: {2}, target.targetObject: {3}", new object[]
			{
				owner,
				itemIDx,
				targetXform.GetPath(),
				this.target.targetObject.GetPath()
			}));
		}
		TransferrableObject component = this.target.targetObject.GetComponent<TransferrableObject>();
		this.validShareable = (component.canDrop || component.shareable || component.allowWorldSharableInstance);
		if (!this.validShareable)
		{
			Debug.LogError(string.Format("tried to setup an invalid shareable {0} {1} {2}", owner, itemIDx, targetXform.GetPath()));
			base.gameObject.SetActive(false);
			this.Invalidate();
			return;
		}
		this.guard.AddCallbackTarget(component);
		this.guard.giveCreatorAbsoluteAuthority = true;
		component.SetWorldShareableItem(this);
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x0003EC8B File Offset: 0x0003CE8B
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		base.OnPhotonInstantiate(info);
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x000C1790 File Offset: 0x000BF990
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		if (this.onOwnerChangeCb != null)
		{
			NetPlayer player = NetworkSystem.Instance.GetPlayer(newOwner);
			NetPlayer player2 = NetworkSystem.Instance.GetPlayer(previousOwner);
			this.onOwnerChangeCb(player, player2);
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060015EC RID: 5612 RVA: 0x0003EC94 File Offset: 0x0003CE94
	// (set) Token: 0x060015ED RID: 5613 RVA: 0x0003EC9C File Offset: 0x0003CE9C
	[DevInspectorShow]
	public bool EnableRemoteSync
	{
		get
		{
			return this.enableRemoteSync;
		}
		set
		{
			this.enableRemoteSync = value;
		}
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000C17CC File Offset: 0x000BF9CC
	public void TriggeredUpdate()
	{
		if (!this.IsTargetValid())
		{
			return;
		}
		if (this.guard.isTrulyMine)
		{
			if (this.target.transferrableObject)
			{
				this.target.transferrableObject.worldShareableInstance != this;
			}
			base.transform.position = this.target.targetObject.position;
			base.transform.rotation = this.target.targetObject.rotation;
			return;
		}
		if (!base.IsMine && this.EnableRemoteSync)
		{
			this.target.targetObject.position = base.transform.position;
			this.target.targetObject.rotation = base.transform.rotation;
		}
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x0003ECA5 File Offset: 0x0003CEA5
	public void SyncToSceneObject(TransferrableObject transferrableObject)
	{
		this.target = WorldTargetItem.GenerateTargetFromWorldSharableItem(null, -2, transferrableObject.transform);
		base.transform.parent = null;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x0003ECC7 File Offset: 0x0003CEC7
	public void SetupSceneObjectOnNetwork(NetPlayer owner)
	{
		this.guard.SetOwnership(owner, false, false);
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x0003ECD7 File Offset: 0x0003CED7
	public bool IsTargetValid()
	{
		return this.target != null;
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x0003ECE2 File Offset: 0x0003CEE2
	public void Invalidate()
	{
		this.target = null;
		this.transferableObjectState = TransferrableObject.PositionState.None;
		this.transferableObjectItemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x000C1898 File Offset: 0x000BFA98
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		if (toPlayer == null)
		{
			return;
		}
		WorldShareableItem.CachedData cachedData;
		if (this.cachedDatas.TryGetValue(toPlayer, out cachedData))
		{
			this.transferableObjectState = cachedData.cachedTransferableObjectState;
			this.transferableObjectItemState = cachedData.cachedTransferableObjectItemState;
			this.cachedDatas.Remove(toPlayer);
		}
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x0003ECF9 File Offset: 0x0003CEF9
	public override void WriteDataFusion()
	{
		this.transferableObjectItemStateNetworked = this.transferableObjectItemState;
		this.transferableObjectStateNetworked = this.transferableObjectState;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x0003ED13 File Offset: 0x0003CF13
	public override void ReadDataFusion()
	{
		this.transferableObjectItemState = this.transferableObjectItemStateNetworked;
		this.transferableObjectState = this.transferableObjectStateNetworked;
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x0003ED2D File Offset: 0x0003CF2D
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.transferableObjectState);
		stream.SendNext(this.transferableObjectItemState);
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x000C18E0 File Offset: 0x000BFAE0
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (player != this.guard.actualOwner)
		{
			Debug.Log("Blocking info from non owner");
			this.cachedDatas.AddOrUpdate(player, new WorldShareableItem.CachedData
			{
				cachedTransferableObjectState = (TransferrableObject.PositionState)stream.ReceiveNext(),
				cachedTransferableObjectItemState = (TransferrableObject.ItemStates)stream.ReceiveNext()
			});
			return;
		}
		this.transferableObjectState = (TransferrableObject.PositionState)stream.ReceiveNext();
		this.transferableObjectItemState = (TransferrableObject.ItemStates)stream.ReceiveNext();
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x0003ED51 File Offset: 0x0003CF51
	[PunRPC]
	internal void RPCWorldShareable(PhotonMessageInfo info)
	{
		NetworkSystem.Instance.GetPlayer(info.Sender);
		GorillaNot.IncrementRPCCall(info, "RPCWorldShareable");
		if (this.rpcCallBack == null)
		{
			return;
		}
		this.rpcCallBack();
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x00039846 File Offset: 0x00037A46
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return true;
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x00039846 File Offset: 0x00037A46
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return true;
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x0003ED83 File Offset: 0x0003CF83
	public void SetWillTeleport()
	{
		this.teleportSerializer.SetWillTeleport();
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x00030709 File Offset: 0x0002E909
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x00030715 File Offset: 0x0002E915
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x0400182E RID: 6190
	private bool validShareable = true;

	// Token: 0x0400182F RID: 6191
	public RequestableOwnershipGuard guard;

	// Token: 0x04001830 RID: 6192
	private TransformViewTeleportSerializer teleportSerializer;

	// Token: 0x04001831 RID: 6193
	[DevInspectorShow]
	[CanBeNull]
	private WorldTargetItem _target;

	// Token: 0x04001832 RID: 6194
	public WorldShareableItem.OnOwnerChangeDelegate onOwnerChangeCb;

	// Token: 0x04001833 RID: 6195
	public Action rpcCallBack;

	// Token: 0x04001834 RID: 6196
	private bool enableRemoteSync = true;

	// Token: 0x04001835 RID: 6197
	public Dictionary<NetPlayer, WorldShareableItem.CachedData> cachedDatas = new Dictionary<NetPlayer, WorldShareableItem.CachedData>();

	// Token: 0x020003AA RID: 938
	// (Invoke) Token: 0x06001602 RID: 5634
	public delegate void Delegate();

	// Token: 0x020003AB RID: 939
	// (Invoke) Token: 0x06001606 RID: 5638
	public delegate void OnOwnerChangeDelegate(NetPlayer newOwner, NetPlayer prevOwner);

	// Token: 0x020003AC RID: 940
	public struct CachedData
	{
		// Token: 0x04001836 RID: 6198
		public TransferrableObject.PositionState cachedTransferableObjectState;

		// Token: 0x04001837 RID: 6199
		public TransferrableObject.ItemStates cachedTransferableObjectItemState;
	}
}
