using System;
using System.Collections.Generic;
using System.Timers;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTagScripts;
using KID.Model;
using Photon.Pun;
using TagEffects;
using UnityEngine;

// Token: 0x02000815 RID: 2069
internal class RoomSystem : MonoBehaviour
{
	// Token: 0x0600330D RID: 13069 RVA: 0x001399B0 File Offset: 0x00137BB0
	internal static void DeserializeLaunchProjectile(object[] projectileData, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		GorillaNot.IncrementRPCCall(info, "LaunchSlingshotProjectile");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer))
		{
			return;
		}
		byte b = Convert.ToByte(projectileData[5]);
		byte b2 = Convert.ToByte(projectileData[6]);
		byte b3 = Convert.ToByte(projectileData[7]);
		byte b4 = Convert.ToByte(projectileData[8]);
		Color32 c = new Color32(b, b2, b3, b4);
		Vector3 position = (Vector3)projectileData[0];
		Vector3 velocity = (Vector3)projectileData[1];
		float num = 10000f;
		if (position.IsValid(num))
		{
			float num2 = 10000f;
			if (velocity.IsValid(num2) && float.IsFinite((float)b) && float.IsFinite((float)b2) && float.IsFinite((float)b3) && float.IsFinite((float)b4))
			{
				RoomSystem.ProjectileSource projectileSource = (RoomSystem.ProjectileSource)Convert.ToInt32(projectileData[2]);
				int projectileIndex = Convert.ToInt32(projectileData[3]);
				bool overridecolour = Convert.ToBoolean(projectileData[4]);
				VRRig rig = rigContainer.Rig;
				if (rig.isOfflineVRRig || rig.IsPositionInRange(position, 4f))
				{
					RoomSystem.launchProjectile.targetRig = rig;
					RoomSystem.launchProjectile.position = position;
					RoomSystem.launchProjectile.velocity = velocity;
					RoomSystem.launchProjectile.overridecolour = overridecolour;
					RoomSystem.launchProjectile.colour = c;
					RoomSystem.launchProjectile.projectileIndex = projectileIndex;
					RoomSystem.launchProjectile.projectileSource = projectileSource;
					RoomSystem.launchProjectile.messageInfo = info;
					FXSystem.PlayFXForRig(FXType.Projectile, RoomSystem.launchProjectile, info);
				}
				return;
			}
		}
		GorillaNot.instance.SendReport("invalid projectile state", player.UserId, player.NickName);
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x00139B48 File Offset: 0x00137D48
	internal static void SendLaunchProjectile(Vector3 position, Vector3 velocity, RoomSystem.ProjectileSource projectileSource, int projectileCount, bool randomColour, byte r, byte g, byte b, byte a)
	{
		if (!RoomSystem.JoinedRoom)
		{
			return;
		}
		RoomSystem.projectileSendData[0] = position;
		RoomSystem.projectileSendData[1] = velocity;
		RoomSystem.projectileSendData[2] = projectileSource;
		RoomSystem.projectileSendData[3] = projectileCount;
		RoomSystem.projectileSendData[4] = randomColour;
		RoomSystem.projectileSendData[5] = r;
		RoomSystem.projectileSendData[6] = g;
		RoomSystem.projectileSendData[7] = b;
		RoomSystem.projectileSendData[8] = a;
		byte b2 = 0;
		object obj = RoomSystem.projectileSendData;
		RoomSystem.SendEvent(b2, obj, NetworkSystemRaiseEvent.neoOthers, false);
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x00139BF0 File Offset: 0x00137DF0
	internal static void ImpactEffect(VRRig targetRig, Vector3 position, float r, float g, float b, float a, int projectileCount, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		RoomSystem.impactEffect.targetRig = targetRig;
		RoomSystem.impactEffect.position = position;
		RoomSystem.impactEffect.colour = new Color(r, g, b, a);
		RoomSystem.impactEffect.projectileIndex = projectileCount;
		FXSystem.PlayFXForRig(FXType.Impact, RoomSystem.impactEffect, info);
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x00139C44 File Offset: 0x00137E44
	internal static void DeserializeImpactEffect(object[] impactData, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		GorillaNot.IncrementRPCCall(info, "SpawnSlingshotPlayerImpactEffect");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || rigContainer.Rig.projectileWeapon.IsNull())
		{
			return;
		}
		float num = Convert.ToSingle(impactData[1]);
		float num2 = Convert.ToSingle(impactData[2]);
		float num3 = Convert.ToSingle(impactData[3]);
		float num4 = Convert.ToSingle(impactData[4]);
		Vector3 position = (Vector3)impactData[0];
		float num5 = 10000f;
		if (!position.IsValid(num5) || !float.IsFinite(num) || !float.IsFinite(num2) || !float.IsFinite(num3) || !float.IsFinite(num4))
		{
			GorillaNot.instance.SendReport("invalid impact state", player.UserId, player.NickName);
			return;
		}
		int projectileCount = Convert.ToInt32(impactData[5]);
		RoomSystem.ImpactEffect(rigContainer.Rig, position, num, num2, num3, num4, projectileCount, info);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x00139D34 File Offset: 0x00137F34
	internal static void SendImpactEffect(Vector3 position, float r, float g, float b, float a, int projectileCount)
	{
		RoomSystem.ImpactEffect(VRRigCache.Instance.localRig.Rig, position, r, g, b, a, projectileCount, default(PhotonMessageInfoWrapped));
		if (RoomSystem.joinedRoom)
		{
			RoomSystem.impactSendData[0] = position;
			RoomSystem.impactSendData[1] = r;
			RoomSystem.impactSendData[2] = g;
			RoomSystem.impactSendData[3] = b;
			RoomSystem.impactSendData[4] = a;
			RoomSystem.impactSendData[5] = projectileCount;
			byte b2 = 1;
			object obj = RoomSystem.impactSendData;
			RoomSystem.SendEvent(b2, obj, NetworkSystemRaiseEvent.neoOthers, false);
		}
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x00139DD4 File Offset: 0x00137FD4
	private void Awake()
	{
		base.transform.SetParent(null, true);
		UnityEngine.Object.DontDestroyOnLoad(this);
		RoomSystem.playerImpactEffectPrefab = this.roomSettings.PlayerImpactEffect;
		RoomSystem.callbackInstance = this;
		RoomSystem.disconnectTimer.Interval = (double)(this.roomSettings.PausedDCTimer * 1000);
		RoomSystem.playerEffectDictionary.Clear();
		foreach (RoomSystem.PlayerEffectConfig playerEffectConfig in this.roomSettings.PlayerEffects)
		{
			RoomSystem.playerEffectDictionary.Add(playerEffectConfig.type, playerEffectConfig);
		}
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x00139E88 File Offset: 0x00138088
	private void Start()
	{
		List<PhotonView> list = new List<PhotonView>(20);
		foreach (PhotonView photonView in PhotonNetwork.PhotonViewCollection)
		{
			if (photonView.IsRoomView)
			{
				list.Add(photonView);
			}
		}
		RoomSystem.sceneViews = list.ToArray();
		NetworkSystem.Instance.OnRaiseEvent += RoomSystem.OnEvent;
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeftRoom;
		NetworkSystem.Instance.OnPlayerJoined += this.OnPlayerEnteredRoom;
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
		NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnLeftRoom;
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x00051B63 File Offset: 0x0004FD63
	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			RoomSystem.disconnectTimer.Stop();
			return;
		}
		if (RoomSystem.JoinedRoom)
		{
			RoomSystem.disconnectTimer.Start();
		}
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x00139F68 File Offset: 0x00138168
	private void OnJoinedRoom()
	{
		RoomSystem.joinedRoom = true;
		foreach (NetPlayer item in NetworkSystem.Instance.AllNetPlayers)
		{
			RoomSystem.netPlayersInRoom.Add(item);
		}
		PlayerCosmeticsSystem.UpdatePlayerCosmetics(RoomSystem.netPlayersInRoom);
		RoomSystem.roomGameMode = NetworkSystem.Instance.GameModeString;
		if (NetworkSystem.Instance.IsMasterClient)
		{
			for (int j = 0; j < this.prefabsToInstantiateByPath.Length; j++)
			{
				this.prefabsInstantiated.Add(NetworkSystem.Instance.NetInstantiate(this.prefabsToInstantiate[j], Vector3.zero, Quaternion.identity, true));
			}
		}
		try
		{
			this.roomSettings.ExpectedUsersTimer.Start();
			Action joinedRoomEvent = RoomSystem.JoinedRoomEvent;
			if (joinedRoomEvent != null)
			{
				joinedRoomEvent();
			}
		}
		catch (Exception)
		{
			Debug.LogError("RoomSystem failed invoking event");
		}
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x0013A044 File Offset: 0x00138244
	private void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		if (newPlayer.IsLocal)
		{
			return;
		}
		if (!RoomSystem.netPlayersInRoom.Contains(newPlayer))
		{
			RoomSystem.netPlayersInRoom.Add(newPlayer);
		}
		PlayerCosmeticsSystem.UpdatePlayerCosmetics(newPlayer);
		try
		{
			Action<NetPlayer> playerJoinedEvent = RoomSystem.PlayerJoinedEvent;
			if (playerJoinedEvent != null)
			{
				playerJoinedEvent(newPlayer);
			}
			Action playersChangedEvent = RoomSystem.PlayersChangedEvent;
			if (playersChangedEvent != null)
			{
				playersChangedEvent();
			}
		}
		catch (Exception)
		{
			Debug.LogError("RoomSystem failed invoking event");
		}
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x0013A0B8 File Offset: 0x001382B8
	private void OnLeftRoom()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		RoomSystem.joinedRoom = false;
		RoomSystem.netPlayersInRoom.Clear();
		RoomSystem.roomGameMode = "";
		PlayerCosmeticsSystem.StaticReset();
		int actorNumber = NetworkSystem.Instance.LocalPlayer.ActorNumber;
		for (int i = 0; i < RoomSystem.sceneViews.Length; i++)
		{
			RoomSystem.sceneViews[i].ControllerActorNr = actorNumber;
			RoomSystem.sceneViews[i].OwnerActorNr = actorNumber;
		}
		this.roomSettings.StatusEffectLimiter.Reset();
		this.roomSettings.SoundEffectLimiter.Reset();
		this.roomSettings.SoundEffectOtherLimiter.Reset();
		this.roomSettings.PlayerEffectLimiter.Reset();
		try
		{
			this.roomSettings.ExpectedUsersTimer.Stop();
			Action leftRoomEvent = RoomSystem.LeftRoomEvent;
			if (leftRoomEvent != null)
			{
				leftRoomEvent();
			}
		}
		catch (Exception)
		{
			Debug.LogError("RoomSystem failed invoking event");
		}
		GC.Collect(0);
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x0013A1B0 File Offset: 0x001383B0
	private void OnPlayerLeftRoom(NetPlayer netPlayer)
	{
		if (netPlayer == null)
		{
			Debug.LogError("Player how left doesnt have a reference somehow");
		}
		foreach (NetPlayer netPlayer2 in RoomSystem.netPlayersInRoom)
		{
			if (netPlayer2 == netPlayer)
			{
				RoomSystem.netPlayersInRoom.Remove(netPlayer2);
				break;
			}
		}
		RoomSystem.netPlayersInRoom.Remove(netPlayer);
		try
		{
			Action<NetPlayer> playerLeftEvent = RoomSystem.PlayerLeftEvent;
			if (playerLeftEvent != null)
			{
				playerLeftEvent(netPlayer);
			}
			Action playersChangedEvent = RoomSystem.PlayersChangedEvent;
			if (playersChangedEvent != null)
			{
				playersChangedEvent();
			}
		}
		catch (Exception)
		{
			Debug.LogError("RoomSystem failed invoking event");
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06003319 RID: 13081 RVA: 0x00051B84 File Offset: 0x0004FD84
	public static List<NetPlayer> PlayersInRoom
	{
		get
		{
			return RoomSystem.netPlayersInRoom;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x0600331A RID: 13082 RVA: 0x00051B8B File Offset: 0x0004FD8B
	public static string RoomGameMode
	{
		get
		{
			return RoomSystem.roomGameMode;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x0600331B RID: 13083 RVA: 0x00051B92 File Offset: 0x0004FD92
	public static bool JoinedRoom
	{
		get
		{
			return NetworkSystem.Instance.InRoom && RoomSystem.joinedRoom;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x0600331C RID: 13084 RVA: 0x00051BA7 File Offset: 0x0004FDA7
	public static bool AmITheHost
	{
		get
		{
			return NetworkSystem.Instance.IsMasterClient || !NetworkSystem.Instance.InRoom;
		}
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x0013A264 File Offset: 0x00138464
	static RoomSystem()
	{
		RoomSystem.disconnectTimer.Elapsed += RoomSystem.TimerDC;
		RoomSystem.disconnectTimer.AutoReset = false;
		RoomSystem.StaticLoad();
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x0013A374 File Offset: 0x00138574
	[OnEnterPlay_Run]
	private static void StaticLoad()
	{
		RoomSystem.netEventCallbacks[0] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializeLaunchProjectile);
		RoomSystem.netEventCallbacks[1] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializeImpactEffect);
		RoomSystem.netEventCallbacks[4] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.SearchForNearby);
		RoomSystem.netEventCallbacks[7] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.SearchForParty);
		RoomSystem.netEventCallbacks[2] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializeStatusEffect);
		RoomSystem.netEventCallbacks[3] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializeSoundEffect);
		RoomSystem.netEventCallbacks[5] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializeReportTouch);
		RoomSystem.netEventCallbacks[8] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializePlayerLaunched);
		RoomSystem.netEventCallbacks[6] = new Action<object[], PhotonMessageInfoWrapped>(RoomSystem.DeserializePlayerEffect);
		RoomSystem.soundEffectCallback = new Action<RoomSystem.SoundEffect, NetPlayer>(RoomSystem.OnPlaySoundEffect);
		RoomSystem.statusEffectCallback = new Action<RoomSystem.StatusEffects>(RoomSystem.OnStatusEffect);
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x00051BC4 File Offset: 0x0004FDC4
	private static void TimerDC(object sender, ElapsedEventArgs args)
	{
		RoomSystem.disconnectTimer.Stop();
		if (!RoomSystem.joinedRoom)
		{
			return;
		}
		PhotonNetwork.Disconnect();
		PhotonNetwork.SendAllOutgoingCommands();
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x00051BE2 File Offset: 0x0004FDE2
	internal static void SendEvent(in byte code, in object evData, in NetPlayer target, bool reliable)
	{
		NetworkSystemRaiseEvent.neoTarget.TargetActors[0] = target.ActorNumber;
		RoomSystem.SendEvent(code, evData, NetworkSystemRaiseEvent.neoTarget, reliable);
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x00051C04 File Offset: 0x0004FE04
	internal static void SendEvent(in byte code, in object evData, in NetEventOptions neo, bool reliable)
	{
		RoomSystem.sendEventData[0] = NetworkSystem.Instance.ServerTimestamp;
		RoomSystem.sendEventData[1] = code;
		RoomSystem.sendEventData[2] = evData;
		NetworkSystemRaiseEvent.RaiseEvent(3, RoomSystem.sendEventData, neo, reliable);
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x00051C41 File Offset: 0x0004FE41
	private static void OnEvent(EventData data)
	{
		RoomSystem.OnEvent(data.Code, data.CustomData, data.Sender);
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x0013A474 File Offset: 0x00138674
	private static void OnEvent(byte code, object data, int source)
	{
		NetPlayer netPlayer;
		if (code != 3 || !Utils.PlayerInRoom(source, out netPlayer))
		{
			return;
		}
		try
		{
			object[] array = (object[])data;
			int tick = Convert.ToInt32(array[0]);
			byte key = Convert.ToByte(array[1]);
			object[] arg = null;
			if (array.Length > 2)
			{
				object obj = array[2];
				arg = ((obj == null) ? null : ((object[])obj));
			}
			PhotonMessageInfoWrapped arg2 = new PhotonMessageInfoWrapped(netPlayer.ActorNumber, tick);
			Action<object[], PhotonMessageInfoWrapped> action;
			if (RoomSystem.netEventCallbacks.TryGetValue(key, out action))
			{
				action(arg, arg2);
			}
		}
		catch
		{
		}
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x0013A508 File Offset: 0x00138708
	internal static void SearchForNearby(object[] shuffleData, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		GorillaNot.IncrementRPCCall(info, "JoinPubWithNearby");
		string shufflerStr = (string)shuffleData[0];
		string newKeyStr = (string)shuffleData[1];
		ValueTuple<bool, Permission.ManagedByEnum> privateRoomPermissionStatus = KIDManager.Instance.GetPrivateRoomPermissionStatus();
		bool flag = (privateRoomPermissionStatus.Item1 || privateRoomPermissionStatus.Item2 == Permission.ManagedByEnum.PLAYER) && privateRoomPermissionStatus.Item2 != Permission.ManagedByEnum.PROHIBITED;
		if (!GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(NetworkSystem.Instance.LocalPlayer.UserId))
		{
			GorillaNot.instance.SendReport("possible kick attempt", player.UserId, player.NickName);
			return;
		}
		if (!flag || !NetworkSystem.Instance.SessionIsPrivate)
		{
			return;
		}
		PhotonNetworkController.Instance.AttemptToFollowIntoPub(player.UserId, player.ActorNumber, newKeyStr, shufflerStr, JoinType.FollowingNearby);
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x0013A5E4 File Offset: 0x001387E4
	internal static void SearchForParty(object[] shuffleData, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "PARTY_JOIN");
		string shufflerStr = (string)shuffleData[0];
		string newKeyStr = (string)shuffleData[1];
		if (!FriendshipGroupDetection.Instance.IsInMyGroup(info.Sender.UserId))
		{
			GorillaNot.instance.SendReport("possible kick attempt", info.Sender.UserId, info.Sender.NickName);
			return;
		}
		if (PlayFabAuthenticator.instance.GetSafety())
		{
			return;
		}
		PhotonNetworkController.Instance.AttemptToFollowIntoPub(info.Sender.UserId, info.Sender.ActorNumber, newKeyStr, shufflerStr, JoinType.FollowingParty);
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x0013A688 File Offset: 0x00138888
	internal static void SendNearbyFollowCommand(GorillaFriendCollider friendCollider, string shuffler, string keyStr)
	{
		RoomSystem.groupJoinSendData[0] = shuffler;
		RoomSystem.groupJoinSendData[1] = keyStr;
		NetEventOptions netEventOptions = new NetEventOptions
		{
			TargetActors = new int[1]
		};
		foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
		{
			if (friendCollider.playerIDsCurrentlyTouching.Contains(netPlayer.UserId) && netPlayer != NetworkSystem.Instance.LocalPlayer)
			{
				netEventOptions.TargetActors[0] = netPlayer.ActorNumber;
				byte b = 4;
				object obj = RoomSystem.groupJoinSendData;
				RoomSystem.SendEvent(b, obj, netEventOptions, false);
			}
		}
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x0013A738 File Offset: 0x00138938
	internal static void SendPartyFollowCommand(string shuffler, string keyStr)
	{
		RoomSystem.groupJoinSendData[0] = shuffler;
		RoomSystem.groupJoinSendData[1] = keyStr;
		NetEventOptions netEventOptions = new NetEventOptions
		{
			TargetActors = new int[1]
		};
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			if (vrrig.IsLocalPartyMember && vrrig.creator != NetworkSystem.Instance.LocalPlayer)
			{
				netEventOptions.TargetActors[0] = vrrig.creator.ActorNumber;
				Debug.Log(string.Format("SendGroupFollowCommand - sendEvent to {0} from {1}, shuffler {2} key {3}", new object[]
				{
					vrrig.creator.NickName,
					NetworkSystem.Instance.LocalPlayer.UserId,
					RoomSystem.groupJoinSendData[0],
					RoomSystem.groupJoinSendData[1]
				}));
				byte b = 7;
				object obj = RoomSystem.groupJoinSendData;
				RoomSystem.SendEvent(b, obj, netEventOptions, false);
			}
		}
	}

	// Token: 0x06003328 RID: 13096 RVA: 0x0013A840 File Offset: 0x00138A40
	private static void DeserializeReportTouch(object[] data, PhotonMessageInfoWrapped info)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		NetPlayer arg = (NetPlayer)data[0];
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		Action<NetPlayer, NetPlayer> action = RoomSystem.playerTouchedCallback;
		if (action == null)
		{
			return;
		}
		action(arg, player);
	}

	// Token: 0x06003329 RID: 13097 RVA: 0x0013A888 File Offset: 0x00138A88
	internal static void SendReportTouch(NetPlayer touchedNetPlayer)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			RoomSystem.reportTouchSendData[0] = touchedNetPlayer;
			byte b = 5;
			object obj = RoomSystem.reportTouchSendData;
			RoomSystem.SendEvent(b, obj, NetworkSystemRaiseEvent.neoMaster, false);
			return;
		}
		Action<NetPlayer, NetPlayer> action = RoomSystem.playerTouchedCallback;
		if (action == null)
		{
			return;
		}
		action(touchedNetPlayer, NetworkSystem.Instance.LocalPlayer);
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x0013A8DC File Offset: 0x00138ADC
	internal static void LaunchPlayer(NetPlayer player, Vector3 velocity)
	{
		RoomSystem.reportTouchSendData[0] = velocity;
		byte b = 8;
		object obj = RoomSystem.reportTouchSendData;
		RoomSystem.SendEvent(b, obj, player, false);
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x0013A90C File Offset: 0x00138B0C
	private static void DeserializePlayerLaunched(object[] data, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "DeserializePlayerLaunched");
		GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
		if (activeGameMode != null && activeGameMode.GameType() == GameModeType.Guardian && info.Sender == NetworkSystem.Instance.MasterClient)
		{
			object obj = data[0];
			if (obj is Vector3)
			{
				Vector3 velocity = (Vector3)obj;
				float num = 10000f;
				if (velocity.IsValid(num) && velocity.magnitude <= 20f && RoomSystem.playerLaunchedCallLimiter.CheckCallTime(Time.time))
				{
					GTPlayer.Instance.DoLaunch(velocity);
					return;
				}
			}
		}
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x0013A9A0 File Offset: 0x00138BA0
	private static void SetSlowedTime()
	{
		if (GorillaTagger.Instance.currentStatus != GorillaTagger.StatusEffect.Slowed)
		{
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
			GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		}
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Slowed, GorillaTagger.Instance.slowCooldown);
		GorillaTagger.Instance.offlineVRRig.PlayTaggedEffect();
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x0013AA1C File Offset: 0x00138C1C
	private static void SetTaggedTime()
	{
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, GorillaTagger.Instance.tagCooldown);
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.offlineVRRig.PlayTaggedEffect();
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x0013AA8C File Offset: 0x00138C8C
	private static void SetFrozenTime()
	{
		GorillaFreezeTagManager gorillaFreezeTagManager = GameMode.ActiveGameMode as GorillaFreezeTagManager;
		if (gorillaFreezeTagManager != null)
		{
			GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Slowed, gorillaFreezeTagManager.freezeDuration);
			GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
			GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
			GorillaTagger.Instance.offlineVRRig.PlayTaggedEffect();
		}
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x00051C5A File Offset: 0x0004FE5A
	private static void SetJoinedTaggedTime()
	{
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x0013AB08 File Offset: 0x00138D08
	private static void SetUntaggedTime()
	{
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.None, 0f);
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x00051C9A File Offset: 0x0004FE9A
	private static void OnStatusEffect(RoomSystem.StatusEffects status)
	{
		switch (status)
		{
		case RoomSystem.StatusEffects.TaggedTime:
			RoomSystem.SetTaggedTime();
			return;
		case RoomSystem.StatusEffects.JoinedTaggedTime:
			RoomSystem.SetJoinedTaggedTime();
			return;
		case RoomSystem.StatusEffects.SetSlowedTime:
			RoomSystem.SetSlowedTime();
			return;
		case RoomSystem.StatusEffects.UnTagged:
			RoomSystem.SetUntaggedTime();
			return;
		case RoomSystem.StatusEffects.FrozenTime:
			RoomSystem.SetFrozenTime();
			return;
		default:
			return;
		}
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x0013AB64 File Offset: 0x00138D64
	private static void DeserializeStatusEffect(object[] data, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		GorillaNot.IncrementRPCCall(info, "DeserializeStatusEffect");
		if (!player.IsMasterClient)
		{
			GorillaNot.instance.SendReport("invalid status", player.UserId, player.NickName);
			return;
		}
		if (!RoomSystem.callbackInstance.roomSettings.StatusEffectLimiter.CheckCallServerTime(info.SentServerTime))
		{
			return;
		}
		RoomSystem.StatusEffects obj = (RoomSystem.StatusEffects)Convert.ToInt32(data[0]);
		Action<RoomSystem.StatusEffects> action = RoomSystem.statusEffectCallback;
		if (action == null)
		{
			return;
		}
		action(obj);
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x0013ABEC File Offset: 0x00138DEC
	internal static void SendStatusEffectAll(RoomSystem.StatusEffects status)
	{
		Action<RoomSystem.StatusEffects> action = RoomSystem.statusEffectCallback;
		if (action != null)
		{
			action(status);
		}
		if (!RoomSystem.joinedRoom)
		{
			return;
		}
		RoomSystem.statusSendData[0] = (int)status;
		byte b = 2;
		object obj = RoomSystem.statusSendData;
		RoomSystem.SendEvent(b, obj, NetworkSystemRaiseEvent.neoOthers, false);
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x0013AC38 File Offset: 0x00138E38
	internal static void SendStatusEffectToPlayer(RoomSystem.StatusEffects status, NetPlayer target)
	{
		if (!target.IsLocal)
		{
			RoomSystem.statusSendData[0] = (int)status;
			byte b = 2;
			object obj = RoomSystem.statusSendData;
			RoomSystem.SendEvent(b, obj, target, false);
			return;
		}
		Action<RoomSystem.StatusEffects> action = RoomSystem.statusEffectCallback;
		if (action == null)
		{
			return;
		}
		action(status);
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x00051CD4 File Offset: 0x0004FED4
	internal static void PlaySoundEffect(int soundIndex, float soundVolume, bool stopCurrentAudio)
	{
		VRRigCache.Instance.localRig.Rig.PlayTagSoundLocal(soundIndex, soundVolume, stopCurrentAudio);
	}

	// Token: 0x06003336 RID: 13110 RVA: 0x0013AC80 File Offset: 0x00138E80
	internal static void PlaySoundEffect(int soundIndex, float soundVolume, bool stopCurrentAudio, NetPlayer target)
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(target, out rigContainer))
		{
			rigContainer.Rig.PlayTagSoundLocal(soundIndex, soundVolume, stopCurrentAudio);
		}
	}

	// Token: 0x06003337 RID: 13111 RVA: 0x00051CED File Offset: 0x0004FEED
	private static void OnPlaySoundEffect(RoomSystem.SoundEffect sound, NetPlayer target)
	{
		if (target.IsLocal)
		{
			RoomSystem.PlaySoundEffect(sound.id, sound.volume, sound.stopCurrentAudio);
			return;
		}
		RoomSystem.PlaySoundEffect(sound.id, sound.volume, sound.stopCurrentAudio, target);
	}

	// Token: 0x06003338 RID: 13112 RVA: 0x0013ACAC File Offset: 0x00138EAC
	private static void DeserializeSoundEffect(object[] data, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		GorillaNot.IncrementRPCCall(info, "DeserializeSoundEffect");
		if (!player.Equals(NetworkSystem.Instance.MasterClient))
		{
			GorillaNot.instance.SendReport("invalid sound effect", player.UserId, player.NickName);
			return;
		}
		RoomSystem.SoundEffect soundEffect;
		soundEffect.id = Convert.ToInt32(data[0]);
		soundEffect.volume = Convert.ToSingle(data[1]);
		soundEffect.stopCurrentAudio = Convert.ToBoolean(data[2]);
		if (!float.IsFinite(soundEffect.volume))
		{
			return;
		}
		NetPlayer arg;
		if (data.Length > 3)
		{
			if (!RoomSystem.callbackInstance.roomSettings.SoundEffectOtherLimiter.CheckCallServerTime(info.SentServerTime))
			{
				return;
			}
			int playerID = Convert.ToInt32(data[3]);
			arg = NetworkSystem.Instance.GetPlayer(playerID);
		}
		else
		{
			if (!RoomSystem.callbackInstance.roomSettings.SoundEffectLimiter.CheckCallServerTime(info.SentServerTime))
			{
				return;
			}
			arg = NetworkSystem.Instance.LocalPlayer;
		}
		RoomSystem.soundEffectCallback(soundEffect, arg);
	}

	// Token: 0x06003339 RID: 13113 RVA: 0x00051D27 File Offset: 0x0004FF27
	internal static void SendSoundEffectAll(int soundIndex, float soundVolume, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectAll(new RoomSystem.SoundEffect(soundIndex, soundVolume, stopCurrentAudio));
	}

	// Token: 0x0600333A RID: 13114 RVA: 0x0013ADB4 File Offset: 0x00138FB4
	internal static void SendSoundEffectAll(RoomSystem.SoundEffect sound)
	{
		Action<RoomSystem.SoundEffect, NetPlayer> action = RoomSystem.soundEffectCallback;
		if (action != null)
		{
			action(sound, NetworkSystem.Instance.LocalPlayer);
		}
		if (!RoomSystem.joinedRoom)
		{
			return;
		}
		RoomSystem.soundSendData[0] = sound.id;
		RoomSystem.soundSendData[1] = sound.volume;
		RoomSystem.soundSendData[2] = sound.stopCurrentAudio;
		byte b = 3;
		object obj = RoomSystem.soundSendData;
		RoomSystem.SendEvent(b, obj, NetworkSystemRaiseEvent.neoOthers, false);
	}

	// Token: 0x0600333B RID: 13115 RVA: 0x00051D36 File Offset: 0x0004FF36
	internal static void SendSoundEffectToPlayer(int soundIndex, float soundVolume, NetPlayer player, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectToPlayer(new RoomSystem.SoundEffect(soundIndex, soundVolume, stopCurrentAudio), player);
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0013AE34 File Offset: 0x00139034
	internal static void SendSoundEffectToPlayer(RoomSystem.SoundEffect sound, NetPlayer player)
	{
		if (player.IsLocal)
		{
			Action<RoomSystem.SoundEffect, NetPlayer> action = RoomSystem.soundEffectCallback;
			if (action == null)
			{
				return;
			}
			action(sound, player);
			return;
		}
		else
		{
			if (!RoomSystem.joinedRoom)
			{
				return;
			}
			RoomSystem.soundSendData[0] = sound.id;
			RoomSystem.soundSendData[1] = sound.volume;
			RoomSystem.soundSendData[2] = sound.stopCurrentAudio;
			byte b = 3;
			object obj = RoomSystem.soundSendData;
			RoomSystem.SendEvent(b, obj, player, false);
			return;
		}
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x00051D46 File Offset: 0x0004FF46
	internal static void SendSoundEffectOnOther(int soundIndex, float soundvolume, NetPlayer target, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectOnOther(new RoomSystem.SoundEffect(soundIndex, soundvolume, stopCurrentAudio), target);
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0013AEB0 File Offset: 0x001390B0
	internal static void SendSoundEffectOnOther(RoomSystem.SoundEffect sound, NetPlayer target)
	{
		Action<RoomSystem.SoundEffect, NetPlayer> action = RoomSystem.soundEffectCallback;
		if (action != null)
		{
			action(sound, target);
		}
		if (!RoomSystem.joinedRoom)
		{
			return;
		}
		RoomSystem.sendSoundDataOther[0] = sound.id;
		RoomSystem.sendSoundDataOther[1] = sound.volume;
		RoomSystem.sendSoundDataOther[2] = sound.stopCurrentAudio;
		RoomSystem.sendSoundDataOther[3] = target.ActorNumber;
		byte b = 3;
		object obj = RoomSystem.sendSoundDataOther;
		RoomSystem.SendEvent(b, obj, NetworkSystemRaiseEvent.neoOthers, false);
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0013AF38 File Offset: 0x00139138
	internal static void OnPlayerEffect(PlayerEffect effect, NetPlayer target)
	{
		if (target == null)
		{
			return;
		}
		RoomSystem.PlayerEffectConfig playerEffectConfig;
		RigContainer rigContainer;
		if (RoomSystem.playerEffectDictionary.TryGetValue(effect, out playerEffectConfig) && VRRigCache.Instance.TryGetVrrig(target, out rigContainer) && rigContainer != null && rigContainer.Rig != null && playerEffectConfig.tagEffectPack != null)
		{
			TagEffectsLibrary.PlayEffect(rigContainer.Rig.transform, false, rigContainer.Rig.scaleFactor, target.IsLocal ? TagEffectsLibrary.EffectType.FIRST_PERSON : TagEffectsLibrary.EffectType.THIRD_PERSON, playerEffectConfig.tagEffectPack, playerEffectConfig.tagEffectPack, rigContainer.Rig.transform.rotation);
		}
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0013AFD0 File Offset: 0x001391D0
	private static void DeserializePlayerEffect(object[] data, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "DeserializePlayerEffect");
		if (!RoomSystem.callbackInstance.roomSettings.PlayerEffectLimiter.CheckCallServerTime(info.SentServerTime))
		{
			return;
		}
		int playerID = Convert.ToInt32(data[0]);
		PlayerEffect effect = (PlayerEffect)Convert.ToInt32(data[1]);
		NetPlayer player = NetworkSystem.Instance.GetPlayer(playerID);
		RoomSystem.OnPlayerEffect(effect, player);
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0013B02C File Offset: 0x0013922C
	internal static void SendPlayerEffect(PlayerEffect effect, NetPlayer target)
	{
		RoomSystem.OnPlayerEffect(effect, target);
		if (!RoomSystem.joinedRoom)
		{
			return;
		}
		RoomSystem.playerEffectData[0] = target.ActorNumber;
		RoomSystem.playerEffectData[1] = effect;
		byte b = 6;
		object obj = RoomSystem.playerEffectData;
		RoomSystem.SendEvent(b, obj, NetworkSystemRaiseEvent.neoOthers, false);
	}

	// Token: 0x0400368D RID: 13965
	private static RoomSystem.ImpactFxContainer impactEffect = new RoomSystem.ImpactFxContainer();

	// Token: 0x0400368E RID: 13966
	private static RoomSystem.LaunchProjectileContainer launchProjectile = new RoomSystem.LaunchProjectileContainer();

	// Token: 0x0400368F RID: 13967
	public static GameObject playerImpactEffectPrefab = null;

	// Token: 0x04003690 RID: 13968
	private static readonly object[] projectileSendData = new object[9];

	// Token: 0x04003691 RID: 13969
	private static readonly object[] impactSendData = new object[6];

	// Token: 0x04003692 RID: 13970
	private static readonly List<int> hashValues = new List<int>(2);

	// Token: 0x04003693 RID: 13971
	[SerializeField]
	private RoomSystemSettings roomSettings;

	// Token: 0x04003694 RID: 13972
	[SerializeField]
	private string[] prefabsToInstantiateByPath;

	// Token: 0x04003695 RID: 13973
	[SerializeField]
	private GameObject[] prefabsToInstantiate;

	// Token: 0x04003696 RID: 13974
	private List<GameObject> prefabsInstantiated = new List<GameObject>();

	// Token: 0x04003697 RID: 13975
	public static Dictionary<PlayerEffect, RoomSystem.PlayerEffectConfig> playerEffectDictionary = new Dictionary<PlayerEffect, RoomSystem.PlayerEffectConfig>();

	// Token: 0x04003698 RID: 13976
	[OnEnterPlay_SetNull]
	private static RoomSystem callbackInstance;

	// Token: 0x04003699 RID: 13977
	[OnEnterPlay_Clear]
	private static List<NetPlayer> netPlayersInRoom = new List<NetPlayer>(10);

	// Token: 0x0400369A RID: 13978
	[OnEnterPlay_Set("")]
	private static string roomGameMode = "";

	// Token: 0x0400369B RID: 13979
	[OnEnterPlay_Set(false)]
	private static bool joinedRoom = false;

	// Token: 0x0400369C RID: 13980
	[OnEnterPlay_SetNull]
	private static PhotonView[] sceneViews;

	// Token: 0x0400369D RID: 13981
	[OnExitPlay_SetNull]
	public static Action LeftRoomEvent;

	// Token: 0x0400369E RID: 13982
	[OnExitPlay_SetNull]
	public static Action JoinedRoomEvent;

	// Token: 0x0400369F RID: 13983
	[OnExitPlay_SetNull]
	public static Action<NetPlayer> PlayerJoinedEvent;

	// Token: 0x040036A0 RID: 13984
	[OnExitPlay_SetNull]
	public static Action<NetPlayer> PlayerLeftEvent;

	// Token: 0x040036A1 RID: 13985
	[OnExitPlay_SetNull]
	public static Action PlayersChangedEvent;

	// Token: 0x040036A2 RID: 13986
	private static Timer disconnectTimer = new Timer();

	// Token: 0x040036A3 RID: 13987
	[OnExitPlay_Clear]
	internal static readonly Dictionary<byte, Action<object[], PhotonMessageInfoWrapped>> netEventCallbacks = new Dictionary<byte, Action<object[], PhotonMessageInfoWrapped>>(10);

	// Token: 0x040036A4 RID: 13988
	private static readonly object[] sendEventData = new object[3];

	// Token: 0x040036A5 RID: 13989
	private static readonly object[] groupJoinSendData = new object[2];

	// Token: 0x040036A6 RID: 13990
	private static readonly object[] reportTouchSendData = new object[1];

	// Token: 0x040036A7 RID: 13991
	[OnExitPlay_SetNull]
	public static Action<NetPlayer, NetPlayer> playerTouchedCallback;

	// Token: 0x040036A8 RID: 13992
	private static CallLimiter playerLaunchedCallLimiter = new CallLimiter(3, 15f, 0.5f);

	// Token: 0x040036A9 RID: 13993
	private static object[] statusSendData = new object[1];

	// Token: 0x040036AA RID: 13994
	public static Action<RoomSystem.StatusEffects> statusEffectCallback;

	// Token: 0x040036AB RID: 13995
	private static object[] soundSendData = new object[3];

	// Token: 0x040036AC RID: 13996
	private static object[] sendSoundDataOther = new object[4];

	// Token: 0x040036AD RID: 13997
	public static Action<RoomSystem.SoundEffect, NetPlayer> soundEffectCallback;

	// Token: 0x040036AE RID: 13998
	private static object[] playerEffectData = new object[2];

	// Token: 0x02000816 RID: 2070
	private class ImpactFxContainer : IFXContext
	{
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x00051D69 File Offset: 0x0004FF69
		public FXSystemSettings settings
		{
			get
			{
				return this.targetRig.fxSettings;
			}
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0013B080 File Offset: 0x00139280
		public virtual void OnPlayFX()
		{
			NetPlayer creator = this.targetRig.creator;
			ProjectileTracker.ProjectileInfo projectileInfo;
			if (this.targetRig.isOfflineVRRig)
			{
				projectileInfo = ProjectileTracker.GetLocalProjectile(this.projectileIndex);
			}
			else
			{
				ValueTuple<bool, ProjectileTracker.ProjectileInfo> remotePlayerProjectile = ProjectileTracker.GetRemotePlayerProjectile(creator, this.projectileIndex);
				if (!remotePlayerProjectile.Item1)
				{
					return;
				}
				projectileInfo = remotePlayerProjectile.Item2;
			}
			SlingshotProjectile projectileInstance = projectileInfo.projectileInstance;
			GameObject obj = projectileInfo.hasImpactOverride ? projectileInstance.playerImpactEffectPrefab : RoomSystem.playerImpactEffectPrefab;
			GameObject gameObject = ObjectPools.instance.Instantiate(obj, this.position);
			gameObject.transform.localScale = Vector3.one * this.targetRig.scaleFactor;
			GorillaColorizableBase gorillaColorizableBase;
			if (gameObject.TryGetComponent<GorillaColorizableBase>(out gorillaColorizableBase))
			{
				gorillaColorizableBase.SetColor(this.colour);
			}
			SurfaceImpactFX component = gameObject.GetComponent<SurfaceImpactFX>();
			if (component != null)
			{
				component.SetScale(projectileInstance.transform.localScale.x * projectileInstance.impactEffectScaleMultiplier);
			}
			SoundBankPlayer component2 = gameObject.GetComponent<SoundBankPlayer>();
			if (component2 != null && !component2.playOnEnable)
			{
				component2.Play(projectileInstance.impactSoundVolumeOverride, projectileInstance.impactSoundPitchOverride);
			}
			if (projectileInstance.gameObject.activeSelf && projectileInstance.projectileOwner == creator)
			{
				projectileInstance.Deactivate();
			}
		}

		// Token: 0x040036AF RID: 13999
		public VRRig targetRig;

		// Token: 0x040036B0 RID: 14000
		public Vector3 position;

		// Token: 0x040036B1 RID: 14001
		public Color colour;

		// Token: 0x040036B2 RID: 14002
		public int projectileIndex;
	}

	// Token: 0x02000817 RID: 2071
	private class LaunchProjectileContainer : RoomSystem.ImpactFxContainer
	{
		// Token: 0x06003346 RID: 13126 RVA: 0x0013B1B4 File Offset: 0x001393B4
		public override void OnPlayFX()
		{
			GameObject gameObject = null;
			SlingshotProjectile slingshotProjectile = null;
			try
			{
				switch (this.projectileSource)
				{
				case RoomSystem.ProjectileSource.ProjectileWeapon:
					if (this.targetRig.projectileWeapon.IsNotNull() && this.targetRig.projectileWeapon.IsNotNull())
					{
						SlingshotProjectile slingshotProjectile2 = this.targetRig.projectileWeapon.LaunchNetworkedProjectile(this.position, this.velocity, this.projectileSource, this.projectileIndex, this.targetRig.scaleFactor, this.overridecolour, this.colour, this.messageInfo);
						if (slingshotProjectile2.IsNotNull())
						{
							ProjectileTracker.AddRemotePlayerProjectile(this.messageInfo.Sender, slingshotProjectile2, this.projectileIndex, this.messageInfo.SentServerTime, this.velocity, this.position, this.targetRig.scaleFactor);
						}
					}
					return;
				case RoomSystem.ProjectileSource.LeftHand:
					this.tempThrowableGO = this.targetRig.myBodyDockPositions.GetLeftHandThrowable();
					break;
				case RoomSystem.ProjectileSource.RightHand:
					this.tempThrowableGO = this.targetRig.myBodyDockPositions.GetRightHandThrowable();
					break;
				default:
					return;
				}
				if (!this.tempThrowableGO.IsNull() && this.tempThrowableGO.TryGetComponent<SnowballThrowable>(out this.tempThrowableRef))
				{
					int projectileHash = this.tempThrowableRef.ProjectileHash;
					gameObject = ObjectPools.instance.Instantiate(projectileHash);
					slingshotProjectile = gameObject.GetComponent<SlingshotProjectile>();
					ProjectileTracker.AddRemotePlayerProjectile(this.targetRig.creator, slingshotProjectile, this.projectileIndex, this.messageInfo.SentServerTime, this.velocity, this.position, this.targetRig.scaleFactor);
					slingshotProjectile.Launch(this.position, this.velocity, this.messageInfo.Sender, false, false, this.projectileIndex, this.targetRig.scaleFactor, this.overridecolour, this.colour);
				}
			}
			catch
			{
				GorillaNot.instance.SendReport("throwable error", this.messageInfo.Sender.UserId, this.messageInfo.Sender.NickName);
				if (slingshotProjectile != null && slingshotProjectile)
				{
					slingshotProjectile.transform.position = Vector3.zero;
					slingshotProjectile.Deactivate();
				}
				else if (gameObject.IsNotNull())
				{
					ObjectPools.instance.Destroy(gameObject);
				}
			}
		}

		// Token: 0x040036B3 RID: 14003
		public Vector3 velocity;

		// Token: 0x040036B4 RID: 14004
		public RoomSystem.ProjectileSource projectileSource;

		// Token: 0x040036B5 RID: 14005
		public bool overridecolour;

		// Token: 0x040036B6 RID: 14006
		public PhotonMessageInfoWrapped messageInfo;

		// Token: 0x040036B7 RID: 14007
		private GameObject tempThrowableGO;

		// Token: 0x040036B8 RID: 14008
		private SnowballThrowable tempThrowableRef;
	}

	// Token: 0x02000818 RID: 2072
	internal enum ProjectileSource
	{
		// Token: 0x040036BA RID: 14010
		ProjectileWeapon,
		// Token: 0x040036BB RID: 14011
		LeftHand,
		// Token: 0x040036BC RID: 14012
		RightHand
	}

	// Token: 0x02000819 RID: 2073
	private struct Events
	{
		// Token: 0x040036BD RID: 14013
		public const byte PROJECTILE = 0;

		// Token: 0x040036BE RID: 14014
		public const byte IMPACT = 1;

		// Token: 0x040036BF RID: 14015
		public const byte STATUS_EFFECT = 2;

		// Token: 0x040036C0 RID: 14016
		public const byte SOUND_EFFECT = 3;

		// Token: 0x040036C1 RID: 14017
		public const byte NEARBY_JOIN = 4;

		// Token: 0x040036C2 RID: 14018
		public const byte PLAYER_TOUCHED = 5;

		// Token: 0x040036C3 RID: 14019
		public const byte PLAYER_EFFECT = 6;

		// Token: 0x040036C4 RID: 14020
		public const byte PARTY_JOIN = 7;

		// Token: 0x040036C5 RID: 14021
		public const byte PLAYER_LAUNCHED = 8;
	}

	// Token: 0x0200081A RID: 2074
	public enum StatusEffects
	{
		// Token: 0x040036C7 RID: 14023
		TaggedTime,
		// Token: 0x040036C8 RID: 14024
		JoinedTaggedTime,
		// Token: 0x040036C9 RID: 14025
		SetSlowedTime,
		// Token: 0x040036CA RID: 14026
		UnTagged,
		// Token: 0x040036CB RID: 14027
		FrozenTime
	}

	// Token: 0x0200081B RID: 2075
	public struct SoundEffect
	{
		// Token: 0x06003348 RID: 13128 RVA: 0x0013B414 File Offset: 0x00139614
		public SoundEffect(int soundID, float soundVolume, bool _stopCurrentAudio)
		{
			this.id = soundID;
			this.volume = soundVolume;
			this.volume = soundVolume;
			this.stopCurrentAudio = _stopCurrentAudio;
		}

		// Token: 0x040036CC RID: 14028
		public int id;

		// Token: 0x040036CD RID: 14029
		public float volume;

		// Token: 0x040036CE RID: 14030
		public bool stopCurrentAudio;
	}

	// Token: 0x0200081C RID: 2076
	[Serializable]
	public struct PlayerEffectConfig
	{
		// Token: 0x040036CF RID: 14031
		public PlayerEffect type;

		// Token: 0x040036D0 RID: 14032
		public TagEffectPack tagEffectPack;
	}
}
