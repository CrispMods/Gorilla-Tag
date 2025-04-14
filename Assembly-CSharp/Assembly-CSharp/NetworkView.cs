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
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0004D4DD File Offset: 0x0004B6DD
	public bool IsMine
	{
		get
		{
			return this.punView != null && this.punView.IsMine;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0004D4FA File Offset: 0x0004B6FA
	public bool IsValid
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0004D4FA File Offset: 0x0004B6FA
	public bool HasView
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0004D508 File Offset: 0x0004B708
	public bool IsRoomView
	{
		get
		{
			return this.punView.IsRoomView;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0004D515 File Offset: 0x0004B715
	public PhotonView GetView
	{
		get
		{
			return this.punView;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000FED RID: 4077 RVA: 0x0004D51D File Offset: 0x0004B71D
	public NetPlayer Owner
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.punView.Owner);
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0004D534 File Offset: 0x0004B734
	public int ViewID
	{
		get
		{
			return this.punView.ViewID;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0004D541 File Offset: 0x0004B741
	// (set) Token: 0x06000FF0 RID: 4080 RVA: 0x0004D54E File Offset: 0x0004B74E
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
	// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0004D576 File Offset: 0x0004B776
	// (set) Token: 0x06000FF2 RID: 4082 RVA: 0x0004D583 File Offset: 0x0004B783
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
	// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0004D5AB File Offset: 0x0004B7AB
	// (set) Token: 0x06000FF4 RID: 4084 RVA: 0x0004D5B8 File Offset: 0x0004B7B8
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

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0004D5E0 File Offset: 0x0004B7E0
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

	// Token: 0x06000FF6 RID: 4086 RVA: 0x0004D675 File Offset: 0x0004B875
	protected virtual void Awake()
	{
		this.GetViews();
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x0004D67D File Offset: 0x0004B87D
	protected virtual void Start()
	{
		if (this._sceneObject)
		{
			NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
		}
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x0004D698 File Offset: 0x0004B898
	public void SendRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
		Player playerRef = (targetPlayer as PunNetPlayer).PlayerRef;
		this.punView.RPC(method, playerRef, parameters);
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x0004D6BF File Offset: 0x0004B8BF
	public void SendRPC(string method, RpcTarget target, params object[] parameters)
	{
		this.punView.RPC(method, target, parameters);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0004D6D0 File Offset: 0x0004B8D0
	public void SendRPC(string method, int target, params object[] parameters)
	{
		Room currentRoom = PhotonNetwork.CurrentRoom;
		if (currentRoom == null || !currentRoom.Players.ContainsKey(target))
		{
			return;
		}
		this.punView.RPC(method, currentRoom.Players[target], parameters);
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0004D70E File Offset: 0x0004B90E
	public override void Spawned()
	{
		base.Spawned();
		this._spawned = true;
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0004D71D File Offset: 0x0004B91D
	public void RequestOwnership()
	{
		this.GetView.RequestOwnership();
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0004D72A File Offset: 0x0004B92A
	public void ReleaseOwnership()
	{
		this.changingStatAuth = true;
		base.Object.ReleaseStateAuthority();
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0004D73E File Offset: 0x0004B93E
	public virtual void StateAuthorityChanged()
	{
		if (this.changingStatAuth)
		{
			this.changingStatAuth = false;
		}
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x0400120D RID: 4621
	[SerializeField]
	private PhotonView punView;

	// Token: 0x0400120E RID: 4622
	[SerializeField]
	private PhotonView reliableView;

	// Token: 0x0400120F RID: 4623
	[SerializeField]
	internal NetworkObject fusionView;

	// Token: 0x04001210 RID: 4624
	[SerializeField]
	protected bool _sceneObject;

	// Token: 0x04001211 RID: 4625
	private bool _spawned;

	// Token: 0x04001212 RID: 4626
	private bool changingStatAuth;
}
