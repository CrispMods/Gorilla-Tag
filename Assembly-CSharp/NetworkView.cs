using System;
using Fusion;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200029F RID: 671
[RequireComponent(typeof(PhotonView), typeof(NetworkObject))]
[NetworkBehaviourWeaved(0)]
public class NetworkView : NetworkBehaviour, IStateAuthorityChanged, IPunOwnershipCallbacks
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x0003B1D5 File Offset: 0x000393D5
	public bool IsMine
	{
		get
		{
			return this.punView != null && this.punView.IsMine;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06001032 RID: 4146 RVA: 0x0003B1F2 File Offset: 0x000393F2
	public bool IsValid
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06001033 RID: 4147 RVA: 0x0003B1F2 File Offset: 0x000393F2
	public bool HasView
	{
		get
		{
			return this.punView != null;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003B200 File Offset: 0x00039400
	public bool IsRoomView
	{
		get
		{
			return this.punView.IsRoomView;
		}
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06001035 RID: 4149 RVA: 0x0003B20D File Offset: 0x0003940D
	public PhotonView GetView
	{
		get
		{
			return this.punView;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06001036 RID: 4150 RVA: 0x0003B215 File Offset: 0x00039415
	public NetPlayer Owner
	{
		get
		{
			return NetworkSystem.Instance.GetPlayer(this.punView.Owner);
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06001037 RID: 4151 RVA: 0x0003B22C File Offset: 0x0003942C
	public int ViewID
	{
		get
		{
			return this.punView.ViewID;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06001038 RID: 4152 RVA: 0x0003B239 File Offset: 0x00039439
	// (set) Token: 0x06001039 RID: 4153 RVA: 0x0003B246 File Offset: 0x00039446
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

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600103A RID: 4154 RVA: 0x0003B26E File Offset: 0x0003946E
	// (set) Token: 0x0600103B RID: 4155 RVA: 0x0003B27B File Offset: 0x0003947B
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

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600103C RID: 4156 RVA: 0x0003B2A3 File Offset: 0x000394A3
	// (set) Token: 0x0600103D RID: 4157 RVA: 0x0003B2B0 File Offset: 0x000394B0
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

	// Token: 0x0600103E RID: 4158 RVA: 0x000A9120 File Offset: 0x000A7320
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

	// Token: 0x0600103F RID: 4159 RVA: 0x0003B2D8 File Offset: 0x000394D8
	protected virtual void Awake()
	{
		this.GetViews();
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x0003B2E0 File Offset: 0x000394E0
	protected virtual void Start()
	{
		if (this._sceneObject)
		{
			NetworkSystem.Instance.RegisterSceneNetworkItem(base.gameObject);
		}
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x000A91B8 File Offset: 0x000A73B8
	public void SendRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
		Player playerRef = (targetPlayer as PunNetPlayer).PlayerRef;
		this.punView.RPC(method, playerRef, parameters);
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0003B2FA File Offset: 0x000394FA
	public void SendRPC(string method, RpcTarget target, params object[] parameters)
	{
		this.punView.RPC(method, target, parameters);
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x000A91E0 File Offset: 0x000A73E0
	public void SendRPC(string method, int target, params object[] parameters)
	{
		Room currentRoom = PhotonNetwork.CurrentRoom;
		if (currentRoom == null || !currentRoom.Players.ContainsKey(target))
		{
			return;
		}
		this.punView.RPC(method, currentRoom.Players[target], parameters);
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0003B30A File Offset: 0x0003950A
	public override void Spawned()
	{
		base.Spawned();
		this._spawned = true;
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x0003B319 File Offset: 0x00039519
	public void RequestOwnership()
	{
		this.GetView.RequestOwnership();
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x0003B326 File Offset: 0x00039526
	public void ReleaseOwnership()
	{
		this.changingStatAuth = true;
		base.Object.ReleaseStateAuthority();
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x0003B33A File Offset: 0x0003953A
	public virtual void StateAuthorityChanged()
	{
		if (this.changingStatAuth)
		{
			this.changingStatAuth = false;
		}
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
	{
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
	{
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
	{
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	private PhotonView punView;

	// Token: 0x04001255 RID: 4693
	[SerializeField]
	private PhotonView reliableView;

	// Token: 0x04001256 RID: 4694
	[SerializeField]
	internal NetworkObject fusionView;

	// Token: 0x04001257 RID: 4695
	[SerializeField]
	protected bool _sceneObject;

	// Token: 0x04001258 RID: 4696
	private bool _spawned;

	// Token: 0x04001259 RID: 4697
	private bool changingStatAuth;
}
