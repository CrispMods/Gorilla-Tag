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

// Token: 0x020007FB RID: 2043
internal class RoomSystem : MonoBehaviour
{
	// Token: 0x06003257 RID: 12887 RVA: 0x000F1980 File Offset: 0x000EFB80
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
				int projectileCount = Convert.ToInt32(projectileData[3]);
				bool overridecolour = Convert.ToBoolean(projectileData[4]);
				VRRig rig = rigContainer.Rig;
				if (rig.isOfflineVRRig || rig.IsPositionInRange(position, 4f))
				{
					RoomSystem.launchProjectile.targetRig = rig;
					RoomSystem.launchProjectile.position = position;
					RoomSystem.launchProjectile.velocity = velocity;
					RoomSystem.launchProjectile.overridecolour = overridecolour;
					RoomSystem.launchProjectile.colour = c;
					RoomSystem.launchProjectile.projectileCount = projectileCount;
					RoomSystem.launchProjectile.projectileSource = projectileSource;
					RoomSystem.launchProjectile.messageInfo = info;
					FXSystem.PlayFXForRig(FXType.Projectile, RoomSystem.launchProjectile, info);
				}
				return;
			}
		}
		GorillaNot.instance.SendReport("invalid projectile state", player.UserId, player.NickName);
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x000F1B18 File Offset: 0x000EFD18
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

	// Token: 0x06003259 RID: 12889 RVA: 0x000F1BC0 File Offset: 0x000EFDC0
	internal static void ImpactEffect(VRRig targetRig, Vector3 position, float r, float g, float b, float a, int projectileCount, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		RoomSystem.impactEffect.targetRig = targetRig;
		RoomSystem.impactEffect.position = position;
		RoomSystem.impactEffect.colour = new Color(r, g, b, a);
		RoomSystem.impactEffect.projectileCount = projectileCount;
		FXSystem.PlayFXForRig(FXType.Impact, RoomSystem.impactEffect, info);
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000F1C14 File Offset: 0x000EFE14
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

	// Token: 0x0600325B RID: 12891 RVA: 0x000F1D04 File Offset: 0x000EFF04
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

	// Token: 0x0600325C RID: 12892 RVA: 0x000F1DA4 File Offset: 0x000EFFA4
	private void Awake()
	{
		base.transform.SetParent(null, true);
		Object.DontDestroyOnLoad(this);
		RoomSystem.playerImpactEffectPrefab = this.roomSettings.PlayerImpactEffect;
		RoomSystem.callbackInstance = this;
		RoomSystem.disconnectTimer.Interval = (double)(this.roomSettings.PausedDCTimer * 1000);
		RoomSystem.playerEffectDictionary.Clear();
		foreach (RoomSystem.PlayerEffectConfig playerEffectConfig in this.roomSettings.PlayerEffects)
		{
			RoomSystem.playerEffectDictionary.Add(playerEffectConfig.type, playerEffectConfig);
		}
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x000F1E58 File Offset: 0x000F0058
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

	// Token: 0x0600325E RID: 12894 RVA: 0x000F1F38 File Offset: 0x000F0138
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

	// Token: 0x0600325F RID: 12895 RVA: 0x000F1F5C File Offset: 0x000F015C
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

	// Token: 0x06003260 RID: 12896 RVA: 0x000F2038 File Offset: 0x000F0238
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

	// Token: 0x06003261 RID: 12897 RVA: 0x000F20AC File Offset: 0x000F02AC
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

	// Token: 0x06003262 RID: 12898 RVA: 0x000F21A4 File Offset: 0x000F03A4
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

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06003263 RID: 12899 RVA: 0x000F2258 File Offset: 0x000F0458
	public static List<NetPlayer> PlayersInRoom
	{
		get
		{
			return RoomSystem.netPlayersInRoom;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06003264 RID: 12900 RVA: 0x000F225F File Offset: 0x000F045F
	public static string RoomGameMode
	{
		get
		{
			return RoomSystem.roomGameMode;
		}
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x06003265 RID: 12901 RVA: 0x000F2266 File Offset: 0x000F0466
	public static bool JoinedRoom
	{
		get
		{
			return NetworkSystem.Instance.InRoom && RoomSystem.joinedRoom;
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06003266 RID: 12902 RVA: 0x000F227B File Offset: 0x000F047B
	public static bool AmITheHost
	{
		get
		{
			return NetworkSystem.Instance.IsMasterClient || !NetworkSystem.Instance.InRoom;
		}
	}

	// Token: 0x06003267 RID: 12903 RVA: 0x000F2298 File Offset: 0x000F0498
	static RoomSystem()
	{
		RoomSystem.disconnectTimer.Elapsed += RoomSystem.TimerDC;
		RoomSystem.disconnectTimer.AutoReset = false;
		RoomSystem.StaticLoad();
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x000F23A8 File Offset: 0x000F05A8
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

	// Token: 0x06003269 RID: 12905 RVA: 0x000F24A6 File Offset: 0x000F06A6
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

	// Token: 0x0600326A RID: 12906 RVA: 0x000F24C4 File Offset: 0x000F06C4
	internal static void SendEvent(in byte code, in object evData, in NetPlayer target, bool reliable)
	{
		NetworkSystemRaiseEvent.neoTarget.TargetActors[0] = target.ActorNumber;
		RoomSystem.SendEvent(code, evData, NetworkSystemRaiseEvent.neoTarget, reliable);
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x000F24E6 File Offset: 0x000F06E6
	internal static void SendEvent(in byte code, in object evData, in NetEventOptions neo, bool reliable)
	{
		RoomSystem.sendEventData[0] = NetworkSystem.Instance.ServerTimestamp;
		RoomSystem.sendEventData[1] = code;
		RoomSystem.sendEventData[2] = evData;
		NetworkSystemRaiseEvent.RaiseEvent(3, RoomSystem.sendEventData, neo, reliable);
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x000F2523 File Offset: 0x000F0723
	private static void OnEvent(EventData data)
	{
		RoomSystem.OnEvent(data.Code, data.CustomData, data.Sender);
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x000F253C File Offset: 0x000F073C
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

	// Token: 0x0600326E RID: 12910 RVA: 0x000F25D0 File Offset: 0x000F07D0
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

	// Token: 0x0600326F RID: 12911 RVA: 0x000F26AC File Offset: 0x000F08AC
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

	// Token: 0x06003270 RID: 12912 RVA: 0x000F2750 File Offset: 0x000F0950
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

	// Token: 0x06003271 RID: 12913 RVA: 0x000F2800 File Offset: 0x000F0A00
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

	// Token: 0x06003272 RID: 12914 RVA: 0x000F2908 File Offset: 0x000F0B08
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

	// Token: 0x06003273 RID: 12915 RVA: 0x000F2950 File Offset: 0x000F0B50
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

	// Token: 0x06003274 RID: 12916 RVA: 0x000F29A4 File Offset: 0x000F0BA4
	internal static void LaunchPlayer(NetPlayer player, Vector3 velocity)
	{
		RoomSystem.reportTouchSendData[0] = velocity;
		byte b = 8;
		object obj = RoomSystem.reportTouchSendData;
		RoomSystem.SendEvent(b, obj, player, false);
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x000F29D4 File Offset: 0x000F0BD4
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

	// Token: 0x06003276 RID: 12918 RVA: 0x000F2A68 File Offset: 0x000F0C68
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

	// Token: 0x06003277 RID: 12919 RVA: 0x000F2AE4 File Offset: 0x000F0CE4
	private static void SetTaggedTime()
	{
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.Frozen, GorillaTagger.Instance.tagCooldown);
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.offlineVRRig.PlayTaggedEffect();
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x000F2B54 File Offset: 0x000F0D54
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

	// Token: 0x06003279 RID: 12921 RVA: 0x000F2BCD File Offset: 0x000F0DCD
	private static void SetJoinedTaggedTime()
	{
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000F2C10 File Offset: 0x000F0E10
	private static void SetUntaggedTime()
	{
		GorillaTagger.Instance.ApplyStatusEffect(GorillaTagger.StatusEffect.None, 0f);
		GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
		GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.taggedHapticStrength, GorillaTagger.Instance.taggedHapticDuration);
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x000F2C6B File Offset: 0x000F0E6B
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

	// Token: 0x0600327C RID: 12924 RVA: 0x000F2CA8 File Offset: 0x000F0EA8
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

	// Token: 0x0600327D RID: 12925 RVA: 0x000F2D30 File Offset: 0x000F0F30
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

	// Token: 0x0600327E RID: 12926 RVA: 0x000F2D7C File Offset: 0x000F0F7C
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

	// Token: 0x0600327F RID: 12927 RVA: 0x000F2DC3 File Offset: 0x000F0FC3
	internal static void PlaySoundEffect(int soundIndex, float soundVolume, bool stopCurrentAudio)
	{
		VRRigCache.Instance.localRig.Rig.PlayTagSoundLocal(soundIndex, soundVolume, stopCurrentAudio);
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x000F2DDC File Offset: 0x000F0FDC
	internal static void PlaySoundEffect(int soundIndex, float soundVolume, bool stopCurrentAudio, NetPlayer target)
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(target, out rigContainer))
		{
			rigContainer.Rig.PlayTagSoundLocal(soundIndex, soundVolume, stopCurrentAudio);
		}
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000F2E06 File Offset: 0x000F1006
	private static void OnPlaySoundEffect(RoomSystem.SoundEffect sound, NetPlayer target)
	{
		if (target.IsLocal)
		{
			RoomSystem.PlaySoundEffect(sound.id, sound.volume, sound.stopCurrentAudio);
			return;
		}
		RoomSystem.PlaySoundEffect(sound.id, sound.volume, sound.stopCurrentAudio, target);
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000F2E40 File Offset: 0x000F1040
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

	// Token: 0x06003283 RID: 12931 RVA: 0x000F2F46 File Offset: 0x000F1146
	internal static void SendSoundEffectAll(int soundIndex, float soundVolume, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectAll(new RoomSystem.SoundEffect(soundIndex, soundVolume, stopCurrentAudio));
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000F2F58 File Offset: 0x000F1158
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

	// Token: 0x06003285 RID: 12933 RVA: 0x000F2FD5 File Offset: 0x000F11D5
	internal static void SendSoundEffectToPlayer(int soundIndex, float soundVolume, NetPlayer player, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectToPlayer(new RoomSystem.SoundEffect(soundIndex, soundVolume, stopCurrentAudio), player);
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x000F2FE8 File Offset: 0x000F11E8
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

	// Token: 0x06003287 RID: 12935 RVA: 0x000F3061 File Offset: 0x000F1261
	internal static void SendSoundEffectOnOther(int soundIndex, float soundvolume, NetPlayer target, bool stopCurrentAudio = false)
	{
		RoomSystem.SendSoundEffectOnOther(new RoomSystem.SoundEffect(soundIndex, soundvolume, stopCurrentAudio), target);
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000F3074 File Offset: 0x000F1274
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

	// Token: 0x06003289 RID: 12937 RVA: 0x000F30FC File Offset: 0x000F12FC
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

	// Token: 0x0600328A RID: 12938 RVA: 0x000F3194 File Offset: 0x000F1394
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

	// Token: 0x0600328B RID: 12939 RVA: 0x000F31F0 File Offset: 0x000F13F0
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

	// Token: 0x040035D2 RID: 13778
	private static RoomSystem.ImpactFxContainer impactEffect = new RoomSystem.ImpactFxContainer();

	// Token: 0x040035D3 RID: 13779
	private static RoomSystem.LaunchProjectileContainer launchProjectile = new RoomSystem.LaunchProjectileContainer();

	// Token: 0x040035D4 RID: 13780
	public static GameObject playerImpactEffectPrefab = null;

	// Token: 0x040035D5 RID: 13781
	private static readonly object[] projectileSendData = new object[9];

	// Token: 0x040035D6 RID: 13782
	private static readonly object[] impactSendData = new object[6];

	// Token: 0x040035D7 RID: 13783
	private static readonly List<int> hashValues = new List<int>(2);

	// Token: 0x040035D8 RID: 13784
	[SerializeField]
	private RoomSystemSettings roomSettings;

	// Token: 0x040035D9 RID: 13785
	[SerializeField]
	private string[] prefabsToInstantiateByPath;

	// Token: 0x040035DA RID: 13786
	[SerializeField]
	private GameObject[] prefabsToInstantiate;

	// Token: 0x040035DB RID: 13787
	private List<GameObject> prefabsInstantiated = new List<GameObject>();

	// Token: 0x040035DC RID: 13788
	public static Dictionary<PlayerEffect, RoomSystem.PlayerEffectConfig> playerEffectDictionary = new Dictionary<PlayerEffect, RoomSystem.PlayerEffectConfig>();

	// Token: 0x040035DD RID: 13789
	[OnEnterPlay_SetNull]
	private static RoomSystem callbackInstance;

	// Token: 0x040035DE RID: 13790
	[OnEnterPlay_Clear]
	private static List<NetPlayer> netPlayersInRoom = new List<NetPlayer>(10);

	// Token: 0x040035DF RID: 13791
	[OnEnterPlay_Set("")]
	private static string roomGameMode = "";

	// Token: 0x040035E0 RID: 13792
	[OnEnterPlay_Set(false)]
	private static bool joinedRoom = false;

	// Token: 0x040035E1 RID: 13793
	[OnEnterPlay_SetNull]
	private static PhotonView[] sceneViews;

	// Token: 0x040035E2 RID: 13794
	[OnExitPlay_SetNull]
	public static Action LeftRoomEvent;

	// Token: 0x040035E3 RID: 13795
	[OnExitPlay_SetNull]
	public static Action JoinedRoomEvent;

	// Token: 0x040035E4 RID: 13796
	[OnExitPlay_SetNull]
	public static Action<NetPlayer> PlayerJoinedEvent;

	// Token: 0x040035E5 RID: 13797
	[OnExitPlay_SetNull]
	public static Action<NetPlayer> PlayerLeftEvent;

	// Token: 0x040035E6 RID: 13798
	[OnExitPlay_SetNull]
	public static Action PlayersChangedEvent;

	// Token: 0x040035E7 RID: 13799
	private static Timer disconnectTimer = new Timer();

	// Token: 0x040035E8 RID: 13800
	[OnExitPlay_Clear]
	internal static readonly Dictionary<byte, Action<object[], PhotonMessageInfoWrapped>> netEventCallbacks = new Dictionary<byte, Action<object[], PhotonMessageInfoWrapped>>(10);

	// Token: 0x040035E9 RID: 13801
	private static readonly object[] sendEventData = new object[3];

	// Token: 0x040035EA RID: 13802
	private static readonly object[] groupJoinSendData = new object[2];

	// Token: 0x040035EB RID: 13803
	private static readonly object[] reportTouchSendData = new object[1];

	// Token: 0x040035EC RID: 13804
	[OnExitPlay_SetNull]
	public static Action<NetPlayer, NetPlayer> playerTouchedCallback;

	// Token: 0x040035ED RID: 13805
	private static CallLimiter playerLaunchedCallLimiter = new CallLimiter(3, 15f, 0.5f);

	// Token: 0x040035EE RID: 13806
	private static object[] statusSendData = new object[1];

	// Token: 0x040035EF RID: 13807
	public static Action<RoomSystem.StatusEffects> statusEffectCallback;

	// Token: 0x040035F0 RID: 13808
	private static object[] soundSendData = new object[3];

	// Token: 0x040035F1 RID: 13809
	private static object[] sendSoundDataOther = new object[4];

	// Token: 0x040035F2 RID: 13810
	public static Action<RoomSystem.SoundEffect, NetPlayer> soundEffectCallback;

	// Token: 0x040035F3 RID: 13811
	private static object[] playerEffectData = new object[2];

	// Token: 0x020007FC RID: 2044
	private class ImpactFxContainer : IFXContext
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x000F3255 File Offset: 0x000F1455
		public FXSystemSettings settings
		{
			get
			{
				return this.targetRig.fxSettings;
			}
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x000F3264 File Offset: 0x000F1464
		public virtual void OnPlayFX()
		{
			if (this.targetRig.projectileWeapon != null)
			{
				this.targetRig.projectileWeapon.DestroyProjectile(this.projectileCount, this.position);
			}
			GameObject gameObject = ObjectPools.instance.Instantiate(RoomSystem.playerImpactEffectPrefab, this.position);
			gameObject.transform.localScale = Vector3.one * this.targetRig.scaleFactor;
			GorillaColorizableBase gorillaColorizableBase;
			if (gameObject.TryGetComponent<GorillaColorizableBase>(out gorillaColorizableBase))
			{
				gorillaColorizableBase.SetColor(this.colour);
			}
		}

		// Token: 0x040035F4 RID: 13812
		public VRRig targetRig;

		// Token: 0x040035F5 RID: 13813
		public Vector3 position;

		// Token: 0x040035F6 RID: 13814
		public Color colour;

		// Token: 0x040035F7 RID: 13815
		public int projectileCount;
	}

	// Token: 0x020007FD RID: 2045
	private class LaunchProjectileContainer : RoomSystem.ImpactFxContainer
	{
		// Token: 0x06003290 RID: 12944 RVA: 0x000F32E4 File Offset: 0x000F14E4
		public override void OnPlayFX()
		{
			GameObject gameObject = null;
			SlingshotProjectile slingshotProjectile = null;
			try
			{
				switch (this.projectileSource)
				{
				case RoomSystem.ProjectileSource.ProjectileWeapon:
					if (this.targetRig.projectileWeapon.IsNotNull())
					{
						this.targetRig.projectileWeapon.LaunchNetworkedProjectile(this.position, this.velocity, this.projectileSource, this.projectileCount, this.targetRig.scaleFactor, this.overridecolour, this.colour, this.messageInfo);
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
					slingshotProjectile.Launch(this.position, this.velocity, this.messageInfo.Sender, false, false, this.projectileCount, this.targetRig.scaleFactor, this.overridecolour, this.colour);
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

		// Token: 0x040035F8 RID: 13816
		public Vector3 velocity;

		// Token: 0x040035F9 RID: 13817
		public RoomSystem.ProjectileSource projectileSource;

		// Token: 0x040035FA RID: 13818
		public bool overridecolour;

		// Token: 0x040035FB RID: 13819
		public PhotonMessageInfoWrapped messageInfo;

		// Token: 0x040035FC RID: 13820
		private GameObject tempThrowableGO;

		// Token: 0x040035FD RID: 13821
		private SnowballThrowable tempThrowableRef;
	}

	// Token: 0x020007FE RID: 2046
	internal enum ProjectileSource
	{
		// Token: 0x040035FF RID: 13823
		ProjectileWeapon,
		// Token: 0x04003600 RID: 13824
		LeftHand,
		// Token: 0x04003601 RID: 13825
		RightHand
	}

	// Token: 0x020007FF RID: 2047
	private struct Events
	{
		// Token: 0x04003602 RID: 13826
		public const byte PROJECTILE = 0;

		// Token: 0x04003603 RID: 13827
		public const byte IMPACT = 1;

		// Token: 0x04003604 RID: 13828
		public const byte STATUS_EFFECT = 2;

		// Token: 0x04003605 RID: 13829
		public const byte SOUND_EFFECT = 3;

		// Token: 0x04003606 RID: 13830
		public const byte NEARBY_JOIN = 4;

		// Token: 0x04003607 RID: 13831
		public const byte PLAYER_TOUCHED = 5;

		// Token: 0x04003608 RID: 13832
		public const byte PLAYER_EFFECT = 6;

		// Token: 0x04003609 RID: 13833
		public const byte PARTY_JOIN = 7;

		// Token: 0x0400360A RID: 13834
		public const byte PLAYER_LAUNCHED = 8;
	}

	// Token: 0x02000800 RID: 2048
	public enum StatusEffects
	{
		// Token: 0x0400360C RID: 13836
		TaggedTime,
		// Token: 0x0400360D RID: 13837
		JoinedTaggedTime,
		// Token: 0x0400360E RID: 13838
		SetSlowedTime,
		// Token: 0x0400360F RID: 13839
		UnTagged,
		// Token: 0x04003610 RID: 13840
		FrozenTime
	}

	// Token: 0x02000801 RID: 2049
	public struct SoundEffect
	{
		// Token: 0x06003292 RID: 12946 RVA: 0x000F34B8 File Offset: 0x000F16B8
		public SoundEffect(int soundID, float soundVolume, bool _stopCurrentAudio)
		{
			this.id = soundID;
			this.volume = soundVolume;
			this.volume = soundVolume;
			this.stopCurrentAudio = _stopCurrentAudio;
		}

		// Token: 0x04003611 RID: 13841
		public int id;

		// Token: 0x04003612 RID: 13842
		public float volume;

		// Token: 0x04003613 RID: 13843
		public bool stopCurrentAudio;
	}

	// Token: 0x02000802 RID: 2050
	[Serializable]
	public struct PlayerEffectConfig
	{
		// Token: 0x04003614 RID: 13844
		public PlayerEffect type;

		// Token: 0x04003615 RID: 13845
		public TagEffectPack tagEffectPack;
	}
}
