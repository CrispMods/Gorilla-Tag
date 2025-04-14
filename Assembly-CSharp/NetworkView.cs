using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000294 RID: 660
[RequireComponent(typeof(PhotonView), typeof(NetworkObject))]
[NetworkBehaviourWeaved(0)]
public class NetworkView : NetworkBehaviour, IStateAuthorityChanged, IPunOwnershipCallbacks
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0004D199 File Offset: 0x0004B399
	public bool IsMine
	{
		get
		{
			return this.punView != null && this.punView.IsMine;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0004D1B6 File Offset: 0x0004B3B6
	public bool IsValid
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0004D1B6 File Offset: 0x0004B3B6
	public bool HasView
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0004D1C4 File Offset: 0x0004B3C4
	public bool IsRoomView
	{
		get
		{
			return this.punView.IsRoomView;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0004D1D1 File Offset: 0x0004B3D1
	public PhotonView GetView
	{
		get
		{
			return this.punView;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0004D1D9 File Offset: 0x0004B3D9
	public NetPlayer Owner
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.punView.Owner);
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0004D1F0 File Offset: 0x0004B3F0
	public int ViewID
	{
		get
		{
			return this.punView.ViewID;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000FED RID: 4077 RVA: 0x0004D1FD File Offset: 0x0004B3FD
	// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0004D20A File Offset: 0x0004B40A
	internal OwnershipOption OwnershipTransfer
	{
		get
		{
			return this.punView.OwnershipTransfer;
		}
		set
		{
			this.punView.OwnershipTransfer = value;
			if (this.reliableView != null)
			{
				this.reliableView.OwnershipTransfer = value;
			}
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0004D232 File Offset: 0x0004B432
	// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0004D23F File Offset: 0x0004B43F
	public int OwnerActorNr
	{
		get
		{
			return this.punView.OwnerActorNr;
		}
		set
		{
			this.punView.OwnerActorNr = value;
			if (this.reliableView != null)
			{
				this.reliableView.OwnerActorNr = value;
			}
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0004D267 File Offset: 0x0004B467
	// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0004D274 File Offset: 0x0004B474
	public int ControllerActorNr
	{
		get
		{
			return this.punView.ControllerActorNr;
		}
		set
		{
			this.punView.ControllerActorNr = value;
			if (this.reliableView != null)
			{
				this.reliableView.ControllerActorNr = value;
			}
		}
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0004D29C File Offset: 0x0004B49C
	private void GetViews()
	{
		PhotonView[] components = base.GetComponents<PhotonView>();
		if (components.Length > 1)
		{
			if (components[0].Synchronization == ViewSynchronization.UnreliableOnChange)
			{
				this.punView = components[0];
				this.reliableView = components[1];
			}
			else if (components[0].Synchronization == ViewSynchronization.ReliableDeltaCompressed)
			{
				this.reliableView = components[0];
				this.punView = components[1];
			}
		}
		else
		{
			this.punView = components[0];
		}
		if (this.punView == null)
		{
			this.punView = base.GetComponent<PhotonView>();
		}
		if (this.fusionView == null)
		{
			this.fusionView = base.GetComponent<NetworkObject>();
		}
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0004D331 File Offset: 0x0004B531
	protected virtual void Awake()
	{
		this.GetViews();
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0004D339 File Offset: 0x0004B539
	protected virtual void Start()
	{
		if (this._sceneObject)
		{
			NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
		}
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0004D354 File Offset: 0x0004B554
	public void SendRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
		Player playerRef = (targetPlayer as PunNetPlayer).PlayerRef;
		this.punView.RPC(method, playerRef, parameters);
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0004D37B File Offset: 0x0004B57B
	public void SendRPC(string method, RpcTarget target, params object[] parameters)
	{
		this.punView.RPC(method, target, parameters);
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0004D38B File Offset: 0x0004B58B
	public override void Spawned()
	{
		base.Spawned();
		this._spawned = true;
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0004D39A File Offset: 0x0004B59A
	public void RequestOwnership()
	{
		this.GetView.RequestOwnership();
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0004D3A7 File Offset: 0x0004B5A7
	public void ReleaseOwnership()
	{
		this.changingStatAuth = true;
		base.Object.ReleaseStateAuthority();
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0004D3BB File Offset: 0x0004B5BB
	public virtual void StateAuthorityChanged()
	{
		if (this.changingStatAuth)
		{
			this.changingStatAuth = false;
		}
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x0400120C RID: 4620
	[SerializeField]
	private PhotonView punView;

	// Token: 0x0400120D RID: 4621
	[SerializeField]
	private PhotonView reliableView;

	// Token: 0x0400120E RID: 4622
	[SerializeField]
	internal NetworkObject fusionView;

	// Token: 0x0400120F RID: 4623
	[SerializeField]
	protected bool _sceneObject;

	// Token: 0x04001210 RID: 4624
	private bool _spawned;

	// Token: 0x04001211 RID: 4625
	private bool changingStatAuth;
}
