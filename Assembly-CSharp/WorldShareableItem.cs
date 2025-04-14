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

// Token: 0x0200039E RID: 926
[NetworkBehaviourWeaved(0)]
public class WorldShareableItem : NetworkComponent, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600158D RID: 5517 RVA: 0x00069088 File Offset: 0x00067288
	// (set) Token: 0x0600158E RID: 5518 RVA: 0x00069090 File Offset: 0x00067290
	[DevInspectorShow]
	public TransferrableObject.PositionState transferableObjectState { get; set; }

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600158F RID: 5519 RVA: 0x00069099 File Offset: 0x00067299
	// (set) Token: 0x06001590 RID: 5520 RVA: 0x000690A1 File Offset: 0x000672A1
	public TransferrableObject.ItemStates transferableObjectItemState { get; set; }

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001591 RID: 5521 RVA: 0x000690AA File Offset: 0x000672AA
	// (set) Token: 0x06001592 RID: 5522 RVA: 0x000690B2 File Offset: 0x000672B2
	public TransferrableObject.PositionState transferableObjectStateNetworked { get; set; }

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001593 RID: 5523 RVA: 0x000690BB File Offset: 0x000672BB
	// (set) Token: 0x06001594 RID: 5524 RVA: 0x000690C3 File Offset: 0x000672C3
	public TransferrableObject.ItemStates transferableObjectItemStateNetworked { get; set; }

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x000690CC File Offset: 0x000672CC
	// (set) Token: 0x06001596 RID: 5526 RVA: 0x000690D4 File Offset: 0x000672D4
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

	// Token: 0x06001597 RID: 5527 RVA: 0x000690DD File Offset: 0x000672DD
	protected override void Awake()
	{
		base.Awake();
		this.guard = base.GetComponent<RequestableOwnershipGuard>();
		this.teleportSerializer = base.GetComponent<TransformViewTeleportSerializer>();
		NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0006910D File Offset: 0x0006730D
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

	// Token: 0x06001599 RID: 5529 RVA: 0x00069148 File Offset: 0x00067348
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

	// Token: 0x0600159A RID: 5530 RVA: 0x000691C8 File Offset: 0x000673C8
	public void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		WorldShareableItemManager.Unregister(this);
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000691D8 File Offset: 0x000673D8
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

	// Token: 0x0600159C RID: 5532 RVA: 0x00069264 File Offset: 0x00067464
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

	// Token: 0x0600159D RID: 5533 RVA: 0x00069294 File Offset: 0x00067494
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

	// Token: 0x0600159E RID: 5534 RVA: 0x00069385 File Offset: 0x00067585
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		base.OnPhotonInstantiate(info);
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x00069390 File Offset: 0x00067590
	public override void OnOwnerChange(Player newOwner, Player previousOwner)
	{
		if (this.onOwnerChangeCb != null)
		{
			NetPlayer player = NetworkSystem.Instance.GetPlayer(newOwner);
			NetPlayer player2 = NetworkSystem.Instance.GetPlayer(previousOwner);
			this.onOwnerChangeCb(player, player2);
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x060015A0 RID: 5536 RVA: 0x000693CA File Offset: 0x000675CA
	// (set) Token: 0x060015A1 RID: 5537 RVA: 0x000693D2 File Offset: 0x000675D2
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

	// Token: 0x060015A2 RID: 5538 RVA: 0x000693DC File Offset: 0x000675DC
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

	// Token: 0x060015A3 RID: 5539 RVA: 0x000694A5 File Offset: 0x000676A5
	public void SyncToSceneObject(TransferrableObject transferrableObject)
	{
		this.target = WorldTargetItem.GenerateTargetFromWorldSharableItem(null, -2, transferrableObject.transform);
		base.transform.parent = null;
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x000694C7 File Offset: 0x000676C7
	public void SetupSceneObjectOnNetwork(NetPlayer owner)
	{
		this.guard.SetOwnership(owner, false, false);
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x000694D7 File Offset: 0x000676D7
	public bool IsTargetValid()
	{
		return this.target != null;
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x000694E2 File Offset: 0x000676E2
	public void Invalidate()
	{
		this.target = null;
		this.transferableObjectState = TransferrableObject.PositionState.None;
		this.transferableObjectItemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x000694FC File Offset: 0x000676FC
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

	// Token: 0x060015A8 RID: 5544 RVA: 0x00069542 File Offset: 0x00067742
	public override void WriteDataFusion()
	{
		this.transferableObjectItemStateNetworked = this.transferableObjectItemState;
		this.transferableObjectStateNetworked = this.transferableObjectState;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x0006955C File Offset: 0x0006775C
	public override void ReadDataFusion()
	{
		this.transferableObjectItemState = this.transferableObjectItemStateNetworked;
		this.transferableObjectState = this.transferableObjectStateNetworked;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x00069576 File Offset: 0x00067776
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.transferableObjectState);
		stream.SendNext(this.transferableObjectItemState);
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0006959C File Offset: 0x0006779C
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

	// Token: 0x060015AC RID: 5548 RVA: 0x0006962E File Offset: 0x0006782E
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

	// Token: 0x060015AD RID: 5549 RVA: 0x000444E2 File Offset: 0x000426E2
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return true;
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x000444E2 File Offset: 0x000426E2
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return true;
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x00069660 File Offset: 0x00067860
	public void SetWillTeleport()
	{
		this.teleportSerializer.SetWillTeleport();
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x00002655 File Offset: 0x00000855
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x00002661 File Offset: 0x00000861
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x040017E7 RID: 6119
	private bool validShareable = true;

	// Token: 0x040017E8 RID: 6120
	public RequestableOwnershipGuard guard;

	// Token: 0x040017E9 RID: 6121
	private TransformViewTeleportSerializer teleportSerializer;

	// Token: 0x040017EA RID: 6122
	[DevInspectorShow]
	[CanBeNull]
	private WorldTargetItem _target;

	// Token: 0x040017EB RID: 6123
	public WorldShareableItem.OnOwnerChangeDelegate onOwnerChangeCb;

	// Token: 0x040017EC RID: 6124
	public Action rpcCallBack;

	// Token: 0x040017ED RID: 6125
	private bool enableRemoteSync = true;

	// Token: 0x040017EE RID: 6126
	public Dictionary<NetPlayer, WorldShareableItem.CachedData> cachedDatas = new Dictionary<NetPlayer, WorldShareableItem.CachedData>();

	// Token: 0x0200039F RID: 927
	// (Invoke) Token: 0x060015B6 RID: 5558
	public delegate void Delegate();

	// Token: 0x020003A0 RID: 928
	// (Invoke) Token: 0x060015BA RID: 5562
	public delegate void OnOwnerChangeDelegate(NetPlayer newOwner, NetPlayer prevOwner);

	// Token: 0x020003A1 RID: 929
	public struct CachedData
	{
		// Token: 0x040017EF RID: 6127
		public TransferrableObject.PositionState cachedTransferableObjectState;

		// Token: 0x040017F0 RID: 6128
		public TransferrableObject.ItemStates cachedTransferableObjectItemState;
	}
}
