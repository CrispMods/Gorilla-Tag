using System;
using System.Collections;
using System.Collections.Generic;
using GorillaExtensions;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020001F9 RID: 505
[RequireComponent(typeof(NetworkView))]
public class RequestableOwnershipGuard : MonoBehaviourPunCallbacks, ISelfValidator
{
	// Token: 0x06000BD8 RID: 3032 RVA: 0x0003855B File Offset: 0x0003675B
	private void SetViewToRequest()
	{
		base.GetComponent<NetworkView>().OwnershipTransfer = OwnershipOption.Request;
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00038569 File Offset: 0x00036769
	private NetworkView netView
	{
		get
		{
			if (this.netViews == null)
			{
				return null;
			}
			if (this.netViews.Length == 0)
			{
				return null;
			}
			return this.netViews[0];
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000BDA RID: 3034 RVA: 0x00038588 File Offset: 0x00036788
	[DevInspectorShow]
	public bool isTrulyMine
	{
		get
		{
			return object.Equals(this.actualOwner, NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0003859F File Offset: 0x0003679F
	public bool isMine
	{
		get
		{
			return object.Equals(this.currentOwner, NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x000385B6 File Offset: 0x000367B6
	private void BindNetworkViews()
	{
		this.netViews = base.GetComponents<NetworkView>();
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0009C030 File Offset: 0x0009A230
	public override void OnDisable()
	{
		base.OnDisable();
		RequestableOwnershipGaurdHandler.RemoveViews(this.netViews, this);
		NetworkSystem.Instance.OnPlayerJoined -= this.PlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft -= this.PlayerLeftRoom;
		NetworkSystem.Instance.OnJoinedRoomEvent -= this.JoinedRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent -= this.MasterClientSwitch;
		this.currentMasterClient = null;
		this.currentOwner = null;
		this.actualOwner = null;
		this.creator = NetworkSystem.Instance.LocalPlayer;
		this.currentState = NetworkingState.IsOwner;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0009C0D4 File Offset: 0x0009A2D4
	public override void OnEnable()
	{
		base.OnEnable();
		if (this.autoRegister)
		{
			this.BindNetworkViews();
		}
		if (this.netViews == null)
		{
			return;
		}
		RequestableOwnershipGaurdHandler.RegisterViews(this.netViews, this);
		NetworkSystem.Instance.OnPlayerJoined += this.PlayerEnteredRoom;
		NetworkSystem.Instance.OnPlayerLeft += this.PlayerLeftRoom;
		NetworkSystem.Instance.OnJoinedRoomEvent += this.JoinedRoom;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent += this.MasterClientSwitch;
		NetworkSystem instance = NetworkSystem.Instance;
		if (instance == null || !instance.InRoom)
		{
			GorillaTagger.OnPlayerSpawned(delegate
			{
				this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			});
			return;
		}
		this.currentMasterClient = NetworkSystem.Instance.MasterClient;
		int creatorActorNr = this.netView.GetView.CreatorActorNr;
		NetPlayer netPlayer = this.currentMasterClient;
		int? num = (netPlayer != null) ? new int?(netPlayer.ActorNumber) : null;
		if (!(creatorActorNr == num.GetValueOrDefault() & num != null))
		{
			this.SetOwnership(NetworkSystem.Instance.GetPlayer(this.netView.GetView.CreatorActorNr), false, false);
			return;
		}
		if (this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			this.currentState = NetworkingState.IsOwner;
			return;
		}
		this.currentState = NetworkingState.IsBlindClient;
		this.SetOwnership(null, false, false);
		this.RequestTheCurrentOwnerFromAuthority();
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0009C244 File Offset: 0x0009A444
	private void PlayerEnteredRoom(NetPlayer player)
	{
		if (player.IsLocal)
		{
			return;
		}
		if (NetworkSystem.Instance.InRoom && this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			this.netView.SendRPC("SetOwnershipFromMasterClient", player, new object[]
			{
				this.currentOwner.GetPlayerRef()
			});
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0009C2A0 File Offset: 0x0009A4A0
	public override void OnPreLeavingRoom()
	{
		if (!PhotonNetwork.InRoom)
		{
			return;
		}
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.IsBlindClient:
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			break;
		case NetworkingState.IsClient:
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks callback)
			{
				callback.OnMyOwnerLeft();
			});
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0009C324 File Offset: 0x0009A524
	private void JoinedRoom()
	{
		this.currentMasterClient = NetworkSystem.Instance.MasterClient;
		if (this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			this.currentState = NetworkingState.IsOwner;
			return;
		}
		this.currentState = NetworkingState.IsBlindClient;
		this.SetOwnership(null, false, false);
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0009C380 File Offset: 0x0009A580
	private void PlayerLeftRoom(NetPlayer otherPlayer)
	{
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			break;
		case NetworkingState.IsBlindClient:
			if (this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
			{
				this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
				return;
			}
			this.RequestTheCurrentOwnerFromAuthority();
			return;
		case NetworkingState.IsClient:
			if (this.creator != null && object.Equals(this.creator, otherPlayer))
			{
				this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks callback)
				{
					callback.OnMyCreatorLeft();
				});
			}
			if (object.Equals(this.actualOwner, otherPlayer))
			{
				this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks callback)
				{
					callback.OnMyOwnerLeft();
				});
				if (this.fallbackOwner != null)
				{
					this.SetOwnership(this.fallbackOwner, false, false);
					return;
				}
				this.SetOwnership(this.currentMasterClient, false, false);
				return;
			}
			break;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			if (this.creator != null && object.Equals(this.creator, otherPlayer))
			{
				this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks callback)
				{
					callback.OnMyCreatorLeft();
				});
			}
			if (this.currentState == NetworkingState.ForcefullyTakingOver && object.Equals(this.currentOwner, otherPlayer))
			{
				this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks callback)
				{
					callback.OnMyOwnerLeft();
				});
			}
			if (object.Equals(this.actualOwner, otherPlayer))
			{
				if (this.fallbackOwner != null)
				{
					this.SetOwnership(this.fallbackOwner, false, false);
					if (object.Equals(this.fallbackOwner, PhotonNetwork.LocalPlayer))
					{
						Action action = this.ownershipRequestAccepted;
						if (action == null)
						{
							return;
						}
						action();
						return;
					}
					else
					{
						Action action2 = this.ownershipDenied;
						if (action2 == null)
						{
							return;
						}
						action2();
						return;
					}
				}
				else if (object.Equals(this.currentMasterClient, PhotonNetwork.LocalPlayer))
				{
					Action action3 = this.ownershipRequestAccepted;
					if (action3 == null)
					{
						return;
					}
					action3();
					return;
				}
				else
				{
					Action action4 = this.ownershipDenied;
					if (action4 == null)
					{
						return;
					}
					action4();
					return;
				}
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0009C5A0 File Offset: 0x0009A7A0
	private void MasterClientSwitch(NetPlayer newMaster)
	{
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.IsClient:
			if (this.actualOwner == null && this.currentMasterClient == null)
			{
				this.SetOwnership(newMaster, false, false);
			}
			break;
		case NetworkingState.IsBlindClient:
			if (object.Equals(newMaster, NetworkSystem.Instance.LocalPlayer))
			{
				this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			}
			else
			{
				this.RequestTheCurrentOwnerFromAuthority();
			}
			break;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.currentMasterClient = newMaster;
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0009C630 File Offset: 0x0009A830
	[PunRPC]
	public void RequestCurrentOwnerFromAuthorityRPC(PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		GorillaNot.IncrementRPCCall(info, "RequestCurrentOwnerFromAuthorityRPC");
		if (!this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			return;
		}
		this.netView.SendRPC("SetOwnershipFromMasterClient", player, new object[]
		{
			this.actualOwner.GetPlayerRef()
		});
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0009C694 File Offset: 0x0009A894
	[PunRPC]
	public void TransferOwnershipFromToRPC([CanBeNull] Player nextplayer, string nonce, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "TransferOwnershipFromToRPC");
		if (nextplayer == null)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(nextplayer);
		NetworkSystem.Instance.GetPlayer(info.Sender);
		if (!this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer) && base.photonView.OwnerActorNr != info.Sender.ActorNumber)
		{
			NetPlayer netPlayer = this.currentOwner;
			int? num = (netPlayer != null) ? new int?(netPlayer.ActorNumber) : null;
			int actorNumber = info.Sender.ActorNumber;
			if (!(num.GetValueOrDefault() == actorNumber & num != null))
			{
				NetPlayer netPlayer2 = this.actualOwner;
				num = ((netPlayer2 != null) ? new int?(netPlayer2.ActorNumber) : null);
				actorNumber = info.Sender.ActorNumber;
				if (!(num.GetValueOrDefault() == actorNumber & num != null))
				{
					return;
				}
			}
		}
		if (this.currentOwner == null)
		{
			this.RequestTheCurrentOwnerFromAuthority();
			return;
		}
		if (this.currentOwner.ActorNumber != base.photonView.OwnerActorNr)
		{
			return;
		}
		if (this.actualOwner.ActorNumber == player.ActorNumber)
		{
			return;
		}
		switch (this.currentState)
		{
		case NetworkingState.IsClient:
			this.SetOwnership(player, false, false);
			return;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			if (this.ownershipRequestNonce == nonce)
			{
				this.ownershipRequestNonce = "";
				this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
				return;
			}
			this.actualOwner = player;
			return;
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			this.RequestTheCurrentOwnerFromAuthority();
			return;
		}
		throw new ArgumentOutOfRangeException();
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0009C834 File Offset: 0x0009AA34
	[PunRPC]
	public void SetOwnershipFromMasterClient([CanBeNull] Player nextMaster, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "SetOwnershipFromMasterClient");
		if (nextMaster == null)
		{
			return;
		}
		NetPlayer player = NetworkSystem.Instance.GetPlayer(nextMaster);
		NetPlayer player2 = NetworkSystem.Instance.GetPlayer(info.Sender);
		this.SetOwnershipFromMasterClient(player, player2);
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0009C878 File Offset: 0x0009AA78
	public void SetOwnershipFromMasterClient([CanBeNull] NetPlayer nextMaster, NetPlayer sender)
	{
		if (nextMaster == null)
		{
			return;
		}
		if (!this.PlayerHasAuthority(sender))
		{
			GorillaNot.instance.SendReport("Sent an SetOwnershipFromMasterClient when they weren't the master client", sender.UserId, sender.NickName);
			return;
		}
		NetworkingState networkingState;
		if (this.currentOwner == null)
		{
			networkingState = this.currentState;
			if (networkingState != NetworkingState.IsBlindClient)
			{
				int num = networkingState - NetworkingState.RequestingOwnershipWaitingForSight;
			}
		}
		networkingState = this.currentState;
		if (networkingState - NetworkingState.ForcefullyTakingOver <= 3 && object.Equals(nextMaster, PhotonNetwork.LocalPlayer))
		{
			Action action = this.ownershipRequestAccepted;
			if (action != null)
			{
				action();
			}
			this.SetOwnership(nextMaster, false, false);
			return;
		}
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.IsBlindClient:
		case NetworkingState.IsClient:
			this.SetOwnership(nextMaster, false, false);
			return;
		case NetworkingState.ForcefullyTakingOver:
			this.actualOwner = nextMaster;
			this.currentState = NetworkingState.ForcefullyTakingOver;
			return;
		case NetworkingState.RequestingOwnership:
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			this.currentState = NetworkingState.RequestingOwnership;
			return;
		case NetworkingState.RequestingOwnershipWaitingForSight:
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, false, false);
			this.currentState = NetworkingState.RequestingOwnership;
			this.ownershipRequestNonce = Guid.NewGuid().ToString();
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			return;
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			this.actualOwner = nextMaster;
			this.currentState = NetworkingState.ForcefullyTakingOver;
			this.ownershipRequestNonce = Guid.NewGuid().ToString();
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0009CA0C File Offset: 0x0009AC0C
	[PunRPC]
	public void OwnershipRequested(string nonce, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		GorillaNot.IncrementRPCCall(info, "OwnershipRequested");
		if (nonce != null && nonce.Length > 68)
		{
			return;
		}
		if (info.Sender == PhotonNetwork.LocalPlayer)
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[8].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		bool flag = true;
		using (List<IRequestableOwnershipGuardCallbacks>.Enumerator enumerator = this.callbacksList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.OnOwnershipRequest(player))
				{
					flag = false;
				}
			}
		}
		if (!flag)
		{
			this.netView.SendRPC("OwnershipRequestDenied", player, new object[]
			{
				nonce
			});
			return;
		}
		this.TransferOwnership(player, nonce);
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x000385C4 File Offset: 0x000367C4
	private void TransferOwnershipWithID(int id)
	{
		this.TransferOwnership(NetworkSystem.Instance.GetPlayer(id), "");
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0009CAF8 File Offset: 0x0009ACF8
	public void TransferOwnership(NetPlayer player, string Nonce = "")
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			this.SetOwnership(player, false, false);
			return;
		}
		if (base.photonView.IsMine)
		{
			this.SetOwnership(player, false, false);
			this.netView.SendRPC("TransferOwnershipFromToRPC", RpcTarget.Others, new object[]
			{
				player.GetPlayerRef(),
				Nonce
			});
			return;
		}
		if (this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			this.SetOwnership(player, false, false);
			this.netView.SendRPC("SetOwnershipFromMasterClient", RpcTarget.Others, new object[]
			{
				player.GetPlayerRef()
			});
			return;
		}
		Debug.LogError("Tried to transfer ownership when im not the owner or a master client");
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x000385DC File Offset: 0x000367DC
	public void RequestTheCurrentOwnerFromAuthority()
	{
		this.netView.SendRPC("RequestCurrentOwnerFromAuthorityRPC", this.GetAuthoritativePlayer(), Array.Empty<object>());
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0009CBA0 File Offset: 0x0009ADA0
	protected void SetCurrentOwner(NetPlayer player)
	{
		if (player == null)
		{
			this.currentOwner = null;
		}
		else
		{
			this.currentOwner = player;
		}
		foreach (NetworkView networkView in this.netViews)
		{
			if (player == null)
			{
				networkView.OwnerActorNr = -1;
				networkView.ControllerActorNr = -1;
			}
			else
			{
				networkView.OwnerActorNr = player.ActorNumber;
				networkView.ControllerActorNr = player.ActorNumber;
			}
		}
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0009CC04 File Offset: 0x0009AE04
	protected internal void SetOwnership(NetPlayer player, bool isLocalOnly = false, bool dontPropigate = false)
	{
		if (!object.Equals(player, this.currentOwner) && !dontPropigate)
		{
			this.callbacksList.ForEachBackwards(delegate(IRequestableOwnershipGuardCallbacks actualOwner)
			{
				actualOwner.OnOwnershipTransferred(player, this.currentOwner);
			});
		}
		this.SetCurrentOwner(player);
		if (isLocalOnly)
		{
			return;
		}
		this.actualOwner = player;
		if (player == null)
		{
			return;
		}
		if (player.ActorNumber == NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.currentState = NetworkingState.IsOwner;
			return;
		}
		this.currentState = NetworkingState.IsClient;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x000385F9 File Offset: 0x000367F9
	public NetPlayer GetAuthoritativePlayer()
	{
		if (this.giveCreatorAbsoluteAuthority)
		{
			return this.creator;
		}
		return NetworkSystem.Instance.MasterClient;
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x0009CCA4 File Offset: 0x0009AEA4
	[PunRPC]
	public void OwnershipRequestDenied(string nonce, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		GorillaNot.IncrementRPCCall(info, "OwnershipRequestDenied");
		int actorNumber = info.Sender.ActorNumber;
		NetPlayer netPlayer = this.actualOwner;
		int? num = (netPlayer != null) ? new int?(netPlayer.ActorNumber) : null;
		if (!(actorNumber == num.GetValueOrDefault() & num != null) && !this.PlayerHasAuthority(player))
		{
			return;
		}
		Action action = this.ownershipDenied;
		if (action != null)
		{
			action();
		}
		this.ownershipDenied = null;
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.IsBlindClient:
		case NetworkingState.IsClient:
			return;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			this.currentState = NetworkingState.IsClient;
			this.SetOwnership(this.actualOwner, false, false);
			return;
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00038614 File Offset: 0x00036814
	public IEnumerator RequestTimeout()
	{
		Debug.Log(string.Format("Timeout request started...  {0} ", this.currentState));
		yield return new WaitForSecondsRealtime(2f);
		Debug.Log(string.Format("Timeout request ended! {0} ", this.currentState));
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		case NetworkingState.IsBlindClient:
		case NetworkingState.IsClient:
			break;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			this.currentState = NetworkingState.IsClient;
			this.SetOwnership(this.actualOwner, false, false);
			break;
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		yield break;
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x0009CD9C File Offset: 0x0009AF9C
	public void RequestOwnership(Action onRequestSuccess, Action onRequestFailed)
	{
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
			return;
		case NetworkingState.IsBlindClient:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			this.currentState = NetworkingState.RequestingOwnershipWaitingForSight;
			base.StartCoroutine("RequestTimeout");
			return;
		case NetworkingState.IsClient:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			this.ownershipRequestNonce = Guid.NewGuid().ToString();
			this.currentState = NetworkingState.RequestingOwnership;
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			base.StartCoroutine("RequestTimeout");
			return;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
		case NetworkingState.RequestingOwnershipWaitingForSight:
		case NetworkingState.ForcefullyTakingOverWaitingForSight:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			base.StartCoroutine("RequestTimeout");
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x0009CE98 File Offset: 0x0009B098
	public void RequestOwnershipImmediately(Action onRequestFailed)
	{
		Debug.Log("WorldShareable RequestOwnershipImmediately");
		if (this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			this.RequestOwnershipImmediatelyWithGuaranteedAuthority();
			return;
		}
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
		{
			bool inRoom = NetworkSystem.Instance.InRoom;
			return;
		}
		case NetworkingState.IsBlindClient:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			this.currentState = NetworkingState.ForcefullyTakingOverWaitingForSight;
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, true, false);
			this.RequestTheCurrentOwnerFromAuthority();
			return;
		case NetworkingState.IsClient:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			this.ownershipRequestNonce = Guid.NewGuid().ToString();
			this.currentState = NetworkingState.ForcefullyTakingOver;
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, true, false);
			this.netView.SendRPC("OwnershipRequested", this.actualOwner, new object[]
			{
				this.ownershipRequestNonce
			});
			base.StartCoroutine("RequestTimeout");
			return;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			this.ownershipDenied = (Action)Delegate.Combine(this.ownershipDenied, onRequestFailed);
			this.currentState = NetworkingState.ForcefullyTakingOver;
			base.StartCoroutine("RequestTimeout");
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0009CFDC File Offset: 0x0009B1DC
	public void RequestOwnershipImmediatelyWithGuaranteedAuthority()
	{
		Debug.Log("WorldShareable RequestOwnershipImmediatelyWithGuaranteedAuthority");
		if (!this.PlayerHasAuthority(NetworkSystem.Instance.LocalPlayer))
		{
			Debug.LogError("Tried to request ownership immediately with guaranteed authority without acutely having authority ");
		}
		switch (this.currentState)
		{
		case NetworkingState.IsOwner:
			return;
		case NetworkingState.IsBlindClient:
			this.currentState = NetworkingState.ForcefullyTakingOverWaitingForSight;
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, true, false);
			this.RequestTheCurrentOwnerFromAuthority();
			return;
		case NetworkingState.IsClient:
			this.currentState = NetworkingState.ForcefullyTakingOver;
			this.SetOwnership(NetworkSystem.Instance.LocalPlayer, true, false);
			this.netView.SendRPC("SetOwnershipFromMasterClient", RpcTarget.All, new object[]
			{
				PhotonNetwork.LocalPlayer
			});
			base.StartCoroutine("RequestTimeout");
			return;
		case NetworkingState.ForcefullyTakingOver:
		case NetworkingState.RequestingOwnership:
			this.currentState = NetworkingState.ForcefullyTakingOver;
			base.StartCoroutine("RequestTimeout");
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x00038623 File Offset: 0x00036823
	public void AddCallbackTarget(IRequestableOwnershipGuardCallbacks callbackObject)
	{
		if (!this.callbacksList.Contains(callbackObject))
		{
			this.callbacksList.Add(callbackObject);
			if (this.currentOwner != null)
			{
				callbackObject.OnOwnershipTransferred(this.currentOwner, null);
			}
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00038654 File Offset: 0x00036854
	public void RemoveCallbackTarget(IRequestableOwnershipGuardCallbacks callbackObject)
	{
		if (this.callbacksList.Contains(callbackObject))
		{
			this.callbacksList.Remove(callbackObject);
			if (this.currentOwner != null)
			{
				callbackObject.OnOwnershipTransferred(null, this.currentOwner);
			}
		}
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00038686 File Offset: 0x00036886
	public void SetCreator(NetPlayer player)
	{
		this.creator = player;
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x0003868F File Offset: 0x0003688F
	private NetworkingState EdCurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00030607 File Offset: 0x0002E807
	public void Validate(SelfValidationResult result)
	{
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00038697 File Offset: 0x00036897
	public bool PlayerHasAuthority(NetPlayer player)
	{
		return object.Equals(this.GetAuthoritativePlayer(), player);
	}

	// Token: 0x04000E52 RID: 3666
	[DevInspectorShow]
	[DevInspectorColor("#ff5")]
	public NetworkingState currentState;

	// Token: 0x04000E53 RID: 3667
	[FormerlySerializedAs("NetworkView")]
	[SerializeField]
	private NetworkView[] netViews;

	// Token: 0x04000E54 RID: 3668
	[DevInspectorHide]
	[SerializeField]
	private bool autoRegister = true;

	// Token: 0x04000E55 RID: 3669
	[DevInspectorShow]
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	public NetPlayer currentOwner;

	// Token: 0x04000E56 RID: 3670
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	private NetPlayer currentMasterClient;

	// Token: 0x04000E57 RID: 3671
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	private NetPlayer fallbackOwner;

	// Token: 0x04000E58 RID: 3672
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	public NetPlayer creator;

	// Token: 0x04000E59 RID: 3673
	public bool giveCreatorAbsoluteAuthority;

	// Token: 0x04000E5A RID: 3674
	public bool attemptMasterAssistedTakeoverOnDeny;

	// Token: 0x04000E5B RID: 3675
	private Action ownershipDenied;

	// Token: 0x04000E5C RID: 3676
	private Action ownershipRequestAccepted;

	// Token: 0x04000E5D RID: 3677
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	[DevInspectorShow]
	public NetPlayer actualOwner;

	// Token: 0x04000E5E RID: 3678
	public string ownershipRequestNonce;

	// Token: 0x04000E5F RID: 3679
	public List<IRequestableOwnershipGuardCallbacks> callbacksList = new List<IRequestableOwnershipGuardCallbacks>();
}
