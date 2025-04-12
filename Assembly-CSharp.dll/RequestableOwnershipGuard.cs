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

// Token: 0x020001EE RID: 494
[RequireComponent(typeof(NetworkView))]
public class RequestableOwnershipGuard : MonoBehaviourPunCallbacks, ISelfValidator
{
	// Token: 0x06000B8F RID: 2959 RVA: 0x0003729B File Offset: 0x0003549B
	private void SetViewToRequest()
	{
		base.GetComponent<NetworkView>().OwnershipTransfer = OwnershipOption.Request;
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000B90 RID: 2960 RVA: 0x000372A9 File Offset: 0x000354A9
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

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000B91 RID: 2961 RVA: 0x000372C8 File Offset: 0x000354C8
	[DevInspectorShow]
	public bool isTrulyMine
	{
		get
		{
			return object.Equals(this.actualOwner, NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000B92 RID: 2962 RVA: 0x000372DF File Offset: 0x000354DF
	public bool isMine
	{
		get
		{
			return object.Equals(this.currentOwner, NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x000372F6 File Offset: 0x000354F6
	private void BindNetworkViews()
	{
		this.netViews = base.GetComponents<NetworkView>();
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x000997A4 File Offset: 0x000979A4
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

	// Token: 0x06000B95 RID: 2965 RVA: 0x00099848 File Offset: 0x00097A48
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

	// Token: 0x06000B96 RID: 2966 RVA: 0x000999B8 File Offset: 0x00097BB8
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

	// Token: 0x06000B97 RID: 2967 RVA: 0x00099A14 File Offset: 0x00097C14
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

	// Token: 0x06000B98 RID: 2968 RVA: 0x00099A98 File Offset: 0x00097C98
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

	// Token: 0x06000B99 RID: 2969 RVA: 0x00099AF4 File Offset: 0x00097CF4
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

	// Token: 0x06000B9A RID: 2970 RVA: 0x00099D14 File Offset: 0x00097F14
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

	// Token: 0x06000B9B RID: 2971 RVA: 0x00099DA4 File Offset: 0x00097FA4
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

	// Token: 0x06000B9C RID: 2972 RVA: 0x00099E08 File Offset: 0x00098008
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

	// Token: 0x06000B9D RID: 2973 RVA: 0x00099FA8 File Offset: 0x000981A8
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

	// Token: 0x06000B9E RID: 2974 RVA: 0x00099FEC File Offset: 0x000981EC
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

	// Token: 0x06000B9F RID: 2975 RVA: 0x0009A180 File Offset: 0x00098380
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

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00037304 File Offset: 0x00035504
	private void TransferOwnershipWithID(int id)
	{
		this.TransferOwnership(NetworkSystem.Instance.GetPlayer(id), "");
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0009A26C File Offset: 0x0009846C
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

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0003731C File Offset: 0x0003551C
	public void RequestTheCurrentOwnerFromAuthority()
	{
		this.netView.SendRPC("RequestCurrentOwnerFromAuthorityRPC", this.GetAuthoritativePlayer(), Array.Empty<object>());
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0009A314 File Offset: 0x00098514
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

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0009A378 File Offset: 0x00098578
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

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00037339 File Offset: 0x00035539
	public NetPlayer GetAuthoritativePlayer()
	{
		if (this.giveCreatorAbsoluteAuthority)
		{
			return this.creator;
		}
		return NetworkSystem.Instance.MasterClient;
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0009A418 File Offset: 0x00098618
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

	// Token: 0x06000BA7 RID: 2983 RVA: 0x00037354 File Offset: 0x00035554
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

	// Token: 0x06000BA8 RID: 2984 RVA: 0x0009A510 File Offset: 0x00098710
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

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0009A60C File Offset: 0x0009880C
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

	// Token: 0x06000BAA RID: 2986 RVA: 0x0009A750 File Offset: 0x00098950
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

	// Token: 0x06000BAB RID: 2987 RVA: 0x00037363 File Offset: 0x00035563
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

	// Token: 0x06000BAC RID: 2988 RVA: 0x00037394 File Offset: 0x00035594
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

	// Token: 0x06000BAD RID: 2989 RVA: 0x000373C6 File Offset: 0x000355C6
	public void SetCreator(NetPlayer player)
	{
		this.creator = player;
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000BAE RID: 2990 RVA: 0x000373CF File Offset: 0x000355CF
	private NetworkingState EdCurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void Validate(SelfValidationResult result)
	{
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x000373D7 File Offset: 0x000355D7
	public bool PlayerHasAuthority(NetPlayer player)
	{
		return object.Equals(this.GetAuthoritativePlayer(), player);
	}

	// Token: 0x04000E0D RID: 3597
	[DevInspectorShow]
	[DevInspectorColor("#ff5")]
	public NetworkingState currentState;

	// Token: 0x04000E0E RID: 3598
	[FormerlySerializedAs("NetworkView")]
	[SerializeField]
	private NetworkView[] netViews;

	// Token: 0x04000E0F RID: 3599
	[DevInspectorHide]
	[SerializeField]
	private bool autoRegister = true;

	// Token: 0x04000E10 RID: 3600
	[DevInspectorShow]
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	public NetPlayer currentOwner;

	// Token: 0x04000E11 RID: 3601
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	private NetPlayer currentMasterClient;

	// Token: 0x04000E12 RID: 3602
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	private NetPlayer fallbackOwner;

	// Token: 0x04000E13 RID: 3603
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	public NetPlayer creator;

	// Token: 0x04000E14 RID: 3604
	public bool giveCreatorAbsoluteAuthority;

	// Token: 0x04000E15 RID: 3605
	public bool attemptMasterAssistedTakeoverOnDeny;

	// Token: 0x04000E16 RID: 3606
	private Action ownershipDenied;

	// Token: 0x04000E17 RID: 3607
	private Action ownershipRequestAccepted;

	// Token: 0x04000E18 RID: 3608
	[CanBeNull]
	[SerializeField]
	[SerializeReference]
	[DevInspectorShow]
	public NetPlayer actualOwner;

	// Token: 0x04000E19 RID: 3609
	public string ownershipRequestNonce;

	// Token: 0x04000E1A RID: 3610
	public List<IRequestableOwnershipGuardCallbacks> callbacksList = new List<IRequestableOwnershipGuardCallbacks>();
}
