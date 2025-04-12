using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GorillaLocomotion;
using GorillaNetworking;
using GorillaTag.Rendering;
using GorillaTagScripts.CustomMapSupport;
using GorillaTagScripts.UI.ModIO;
using KID.Model;
using ModIO;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x020009E7 RID: 2535
	public class CustomMapManager : MonoBehaviour, IBuildValidation
	{
		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x00058670 File Offset: 0x00056870
		public static bool WaitingForRoomJoin
		{
			get
			{
				return CustomMapManager.waitingForRoomJoin;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06003F22 RID: 16162 RVA: 0x00058677 File Offset: 0x00056877
		public static bool WaitingForDisconnect
		{
			get
			{
				return CustomMapManager.waitingForDisconnect;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06003F23 RID: 16163 RVA: 0x0005867E File Offset: 0x0005687E
		public static long LoadingMapId
		{
			get
			{
				return CustomMapManager.loadingMapId;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06003F24 RID: 16164 RVA: 0x0005868A File Offset: 0x0005688A
		public static long UnloadingMapId
		{
			get
			{
				return CustomMapManager.unloadingMapId;
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00058696 File Offset: 0x00056896
		public bool BuildValidationCheck()
		{
			if (this.virtualStumpEjectLocations.Count != this.virtualStumpEjectLocationZones.Count)
			{
				Debug.LogError("List of ejection locations is not equal to list of ejection zones", base.gameObject);
				return false;
			}
			return true;
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x000586C3 File Offset: 0x000568C3
		private void Awake()
		{
			if (CustomMapManager.instance == null)
			{
				CustomMapManager.instance = this;
				CustomMapManager.hasInstance = true;
				return;
			}
			if (CustomMapManager.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00165690 File Offset: 0x00163890
		public void OnEnable()
		{
			CustomMapManager.gamemodeButtonLayout = UnityEngine.Object.FindObjectOfType<GameModeSelectorButtonLayout>();
			CustomMapManager.disabled = false;
			bool flag = false;
			if (GorillaServer.Instance == null || !GorillaServer.Instance.FeatureFlagsReady)
			{
				return;
			}
			if (!GorillaServer.Instance.UseKID())
			{
				flag = PlayFabAuthenticator.instance.GetSafety();
			}
			else if (KIDIntegration.IsReady)
			{
				AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
				if (activeAccountStatus != AgeStatusType.LEGALADULT)
				{
					if (activeAccountStatus == (AgeStatusType)0)
					{
						bool safety = PlayFabAuthenticator.instance.GetSafety();
						int userAge = KIDAgeGate.UserAge;
						if (safety || userAge < 13)
						{
							flag = true;
						}
					}
					else if (KIDManager.KIDIntegration.GetAgeFromDateOfBirth() < 13)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				CustomMapManager.disabled = true;
			}
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0016573C File Offset: 0x0016393C
		private void Start()
		{
			for (int i = this.virtualStumpTeleportLocations.Count - 1; i >= 0; i--)
			{
				if (this.virtualStumpTeleportLocations[i] == null)
				{
					this.virtualStumpTeleportLocations.RemoveAt(i);
				}
			}
			for (int j = this.virtualStumpEjectLocations.Count - 1; j >= 0; j--)
			{
				if (this.virtualStumpEjectLocations[j] == null)
				{
					this.virtualStumpEjectLocations.RemoveAt(j);
				}
			}
			if (this.virtualStumpEjectLocations.Count != this.virtualStumpEjectLocationZones.Count)
			{
				GTDev.LogError<string>("Custom maps list length for ejection locations and ejection zones don't match!", base.gameObject, null);
			}
			GameEvents.ModIOModManagementEvent.AddListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
			NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnDisconnected;
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x00165850 File Offset: 0x00163A50
		private void OnDestroy()
		{
			if (CustomMapManager.instance == this)
			{
				CustomMapManager.instance = null;
				CustomMapManager.hasInstance = false;
			}
			GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnected;
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x001658E8 File Offset: 0x00163AE8
		private void HandleModManagementEvent(ModManagementEventType eventType, ModId modId, Result result)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (CustomMapManager.waitingForModInstall && CustomMapManager.waitingForModInstallId == modId)
			{
				switch (eventType)
				{
				case ModManagementEventType.Installed:
					goto IL_7E;
				case ModManagementEventType.InstallFailed:
					CustomMapManager.instance.HandleMapLoadFailed("FAILED TO INSTALL MAP: " + result.message);
					return;
				case ModManagementEventType.DownloadStarted:
				case ModManagementEventType.Downloaded:
					return;
				case ModManagementEventType.DownloadFailed:
					break;
				default:
					if (eventType == ModManagementEventType.Updated)
					{
						goto IL_7E;
					}
					if (eventType != ModManagementEventType.UpdateFailed)
					{
						return;
					}
					break;
				}
				CustomMapManager.instance.HandleMapLoadFailed("FAILED TO DOWNLOAD MAP: " + result.message);
				return;
				IL_7E:
				this.LoadInstalledMod(modId);
			}
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x0016597C File Offset: 0x00163B7C
		private void OnModIOLoggedOut()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			ModId @null = ModId.Null;
			if (ModIOMapLoader.IsModLoaded(0L) && NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
			{
				@null = new ModId(ModIOMapLoader.LoadedMapModId);
			}
			CustomMapManager.UnloadMod(true);
			CustomMapManager.mapIdToLoadOnLogin = @null;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x000586FD File Offset: 0x000568FD
		private void OnModIOLoggedIn()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (CustomMapManager.mapIdToLoadOnLogin != ModId.Null)
			{
				CustomMapManager.LoadMod(CustomMapManager.mapIdToLoadOnLogin);
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00058722 File Offset: 0x00056922
		internal static IEnumerator TeleportToVirtualStump(short teleporterIdx, Action<bool> callback, GTZone playerEntranceZone, VRTeleporterSerializer vRTeleporterSerializer)
		{
			if (CustomMapManager.disabled)
			{
				yield break;
			}
			if (!CustomMapManager.hasInstance)
			{
				if (callback != null)
				{
					callback(false);
				}
				yield break;
			}
			CustomMapManager.entranceZone = playerEntranceZone;
			CustomMapManager.instance.arcadeTeleporterNetworkObj = vRTeleporterSerializer;
			GreyZoneManager greyZoneManager = GreyZoneManager.Instance;
			if (greyZoneManager != null)
			{
				greyZoneManager.ForceStopGreyZone();
			}
			if (CustomMapManager.instance.virtualStumpTeleportLocations.Count > 0)
			{
				Transform randTeleportTarget = CustomMapManager.instance.virtualStumpTeleportLocations[UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpTeleportLocations.Count)];
				GTPlayer localPlayer = GTPlayer.Instance;
				if (localPlayer != null)
				{
					CustomMapManager.instance.arcadeTeleporterNetworkObj.PlayTeleportingSFX(teleporterIdx, CustomMapManager.instance.localTeleportSFXSource);
					CustomMapManager.instance.EnableTeleportHUD(true);
					if (CustomMapManager.instance.arcadeTeleporterNetworkObj != null)
					{
						CustomMapManager.instance.arcadeTeleporterNetworkObj.NotifyPlayerTeleporting(teleporterIdx);
					}
					yield return new WaitForSeconds(0.75f);
					localPlayer.TeleportTo(randTeleportTarget, false);
					GorillaComputer.instance.SetInVirtualStump(true);
				}
				else if (callback != null)
				{
					callback(false);
				}
				CustomMapManager.pendingNewPrivateRoomName = "";
				CustomMapManager.currentTeleportCallback = callback;
				bool flag = false;
				if (NetworkSystem.Instance.InRoom)
				{
					if (NetworkSystem.Instance.SessionIsPrivate)
					{
						flag = true;
						CustomMapManager.waitingForRoomJoin = true;
						CustomMapManager.pendingNewPrivateRoomName = GorillaComputer.instance.VStumpRoomPrepend + NetworkSystem.Instance.RoomName;
						PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResult));
					}
					else
					{
						CustomMapManager.waitingForDisconnect = true;
						NetworkSystem.Instance.ReturnToSinglePlayer();
					}
				}
				ZoneManagement.SetActiveZone(GTZone.customMaps);
				foreach (GameObject gameObject in CustomMapManager.instance.rootObjectsToDeactivateAfterTeleport)
				{
					if (gameObject != null)
					{
						gameObject.gameObject.SetActive(false);
					}
				}
				ZoneShaderSettings.ActivateDefaultSettings();
				if (!flag && !CustomMapManager.waitingForDisconnect)
				{
					if (callback != null)
					{
						callback(true);
					}
				}
				randTeleportTarget = null;
				localPlayer = null;
			}
			yield break;
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x001659D4 File Offset: 0x00163BD4
		public static void ExitVirtualStump(Action<bool> callback)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (CustomMapManager.instance.virtualStumpEjectLocations.Count == 0)
			{
				if (callback != null)
				{
					callback(false);
				}
				return;
			}
			CustomMapManager.instance.EnableTeleportHUD(false);
			CustomMapManager.currentTeleportCallback = callback;
			CustomMapManager.exitVirtualStumpPending = true;
			if (!CustomMapManager.UnloadMod(false))
			{
				CustomMapManager.FinalizeExitVirtualStump();
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00165A38 File Offset: 0x00163C38
		private static void FinalizeExitVirtualStump()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.exitVirtualStumpPending = false;
			foreach (GameObject gameObject in CustomMapManager.instance.rootObjectsToDeactivateAfterTeleport)
			{
				if (gameObject != null)
				{
					gameObject.gameObject.SetActive(true);
				}
			}
			GTZone gtzone = CustomMapManager.entranceZone;
			if (gtzone != GTZone.forest)
			{
				if (gtzone == GTZone.arcade)
				{
					ZoneManagement.SetActiveZone(GTZone.arcade);
				}
			}
			else
			{
				ZoneManagement.SetActiveZone(GTZone.forest);
			}
			int num = 0;
			CustomMapManager.pendingTeleportVFXIdx = (short)UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpEjectLocations.Count);
			while (num < 100 && CustomMapManager.instance.virtualStumpEjectLocationZones[(int)CustomMapManager.pendingTeleportVFXIdx] != CustomMapManager.entranceZone)
			{
				CustomMapManager.pendingTeleportVFXIdx = (short)UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpEjectLocations.Count);
				num++;
			}
			Transform destination = CustomMapManager.instance.virtualStumpEjectLocations[(int)CustomMapManager.pendingTeleportVFXIdx];
			GTPlayer gtplayer = GTPlayer.Instance;
			if (gtplayer != null)
			{
				gtplayer.TeleportTo(destination, false);
				GorillaComputer.instance.SetInVirtualStump(false);
			}
			ZoneShaderSettings.ActivateDefaultSettings();
			CustomMapManager.pendingNewPrivateRoomName = "";
			if (!NetworkSystem.Instance.InRoom)
			{
				GorillaComputer.instance.allowedMapsToJoin = CustomMapManager.instance.exitVirtualStumpJoinTrigger.myCollider.myAllowedMapsToJoin;
				CustomMapManager.waitingForRoomJoin = true;
				PhotonNetworkController.Instance.AttemptToJoinPublicRoom(CustomMapManager.instance.exitVirtualStumpJoinTrigger, JoinType.Solo);
				return;
			}
			if (NetworkSystem.Instance.SessionIsPrivate)
			{
				CustomMapManager.waitingForRoomJoin = true;
				CustomMapManager.pendingNewPrivateRoomName = NetworkSystem.Instance.RoomName.RemoveAll(GorillaComputer.instance.VStumpRoomPrepend, StringComparison.OrdinalIgnoreCase);
				PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResult));
				return;
			}
			GorillaComputer.instance.allowedMapsToJoin = CustomMapManager.instance.exitVirtualStumpJoinTrigger.myCollider.myAllowedMapsToJoin;
			CustomMapManager.waitingForRoomJoin = true;
			PhotonNetworkController.Instance.AttemptToJoinPublicRoom(CustomMapManager.instance.exitVirtualStumpJoinTrigger, JoinType.Solo);
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00165C4C File Offset: 0x00163E4C
		private static void OnJoinSpecificRoomResult(NetJoinResult result)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			switch (result)
			{
			case NetJoinResult.Failed_Full:
				CustomMapManager.instance.OnJoinRoomFailed();
				return;
			case NetJoinResult.AlreadyInRoom:
				CustomMapManager.instance.OnJoinedRoom();
				return;
			case NetJoinResult.Failed_Other:
				CustomMapManager.waitingForDisconnect = true;
				CustomMapManager.shouldRetryJoin = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00165C9C File Offset: 0x00163E9C
		private static void OnJoinSpecificRoomResultFailureAllowed(NetJoinResult result)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			switch (result)
			{
			case NetJoinResult.Success:
			case NetJoinResult.FallbackCreated:
				return;
			case NetJoinResult.Failed_Full:
			case NetJoinResult.Failed_Other:
				CustomMapManager.instance.OnJoinRoomFailed();
				return;
			case NetJoinResult.AlreadyInRoom:
				CustomMapManager.instance.OnJoinedRoom();
				return;
			default:
				return;
			}
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x00165CF0 File Offset: 0x00163EF0
		public static bool AreAllPlayersInVirtualStump()
		{
			if (CustomMapManager.disabled)
			{
				return false;
			}
			if (!CustomMapManager.hasInstance)
			{
				return false;
			}
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				if (!CustomMapManager.instance.virtualStumpPlayerDetector.playerIDsCurrentlyTouching.Contains(vrrig.creator.UserId))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00058746 File Offset: 0x00056946
		public static bool IsRemotePlayerInVirtualStump(string playerID)
		{
			return CustomMapManager.instance.virtualStumpPlayerDetector.playerIDsCurrentlyTouching.Contains(playerID);
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x0005875F File Offset: 0x0005695F
		public static bool IsLocalPlayerInVirtualStump()
		{
			return !CustomMapManager.disabled && CustomMapManager.hasInstance && CustomMapManager.instance.virtualStumpPlayerDetector.playerIDsCurrentlyTouching.Contains(VRRig.LocalRig.creator.UserId);
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00165D80 File Offset: 0x00163F80
		private void OnDisconnected()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.ClearRoomMap();
			if (CustomMapManager.waitingForDisconnect)
			{
				CustomMapManager.waitingForDisconnect = false;
				if (CustomMapManager.shouldRetryJoin)
				{
					CustomMapManager.shouldRetryJoin = false;
					PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResultFailureAllowed));
					return;
				}
				CustomMapManager.DisableTeleportHUD();
				Action<bool> action = CustomMapManager.currentTeleportCallback;
				if (action != null)
				{
					action(true);
				}
				CustomMapManager.currentTeleportCallback = null;
			}
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0005879D File Offset: 0x0005699D
		private void OnJoinRoomFailed()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (CustomMapManager.waitingForRoomJoin)
			{
				CustomMapManager.DisableTeleportHUD();
				CustomMapManager.waitingForRoomJoin = false;
				Action<bool> action = CustomMapManager.currentTeleportCallback;
				if (action != null)
				{
					action(false);
				}
				CustomMapManager.currentTeleportCallback = null;
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00165DF8 File Offset: 0x00163FF8
		private void OnJoinedRoom()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			bool flag = true;
			if (NetworkSystem.Instance.RoomName.Contains(GorillaComputer.instance.VStumpRoomPrepend) && !GorillaComputer.instance.IsPlayerInVirtualStump() && !CustomMapManager.IsLocalPlayerInVirtualStump())
			{
				NetworkSystem.Instance.ReturnToSinglePlayer();
				flag = false;
			}
			if (CustomMapManager.waitingForRoomJoin)
			{
				CustomMapManager.DisableTeleportHUD();
				CustomMapManager.waitingForRoomJoin = false;
				Action<bool> action = CustomMapManager.currentTeleportCallback;
				if (action != null)
				{
					action(true);
				}
				CustomMapManager.currentTeleportCallback = null;
				if (!CustomMapManager.hasInstance || !flag)
				{
					CustomMapManager.pendingTeleportVFXIdx = -1;
					return;
				}
				if (CustomMapManager.pendingTeleportVFXIdx > -1 && CustomMapManager.instance.arcadeTeleporterNetworkObj != null)
				{
					CustomMapManager.instance.arcadeTeleporterNetworkObj.NotifyPlayerReturning(CustomMapManager.pendingTeleportVFXIdx);
					CustomMapManager.pendingTeleportVFXIdx = -1;
				}
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x00165EC8 File Offset: 0x001640C8
		public static bool UnloadMod(bool returnToSinglePlayerIfInPublic = true)
		{
			if (CustomMapManager.disabled)
			{
				return false;
			}
			if (CustomMapManager.unloadInProgress)
			{
				return false;
			}
			if (!ModIOMapLoader.IsModLoaded(0L) && !ModIOMapLoader.IsLoading)
			{
				return false;
			}
			CustomMapManager.unloadInProgress = true;
			CustomMapManager.unloadingMapId = new ModId(ModIOMapLoader.IsModLoaded(0L) ? ModIOMapLoader.LoadedMapModId : ModIOMapLoader.LoadingMapModId);
			CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Unloading, 0, "");
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomGameMode.LuaScript = "";
			if (CustomGameMode.gameScriptRunner != null)
			{
				CustomGameMode.StopScript();
			}
			ModIOMapLoader.CloseDoorAndUnloadMod(new Action(CustomMapManager.OnMapUnloadCompleted));
			if (returnToSinglePlayerIfInPublic && NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.SessionIsPrivate)
			{
				NetworkSystem.Instance.ReturnToSinglePlayer();
			}
			return true;
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000587D8 File Offset: 0x000569D8
		private static void OnMapUnloadCompleted()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapManager.unloadInProgress = false;
			CustomMapManager.OnMapUnloadComplete.Invoke();
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomMapManager.OnRoomMapChanged.Invoke(ModId.Null);
			if (CustomMapManager.exitVirtualStumpPending)
			{
				CustomMapManager.FinalizeExitVirtualStump();
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x00165FB4 File Offset: 0x001641B4
		public static void LoadMod(ModId modId)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance || CustomMapManager.loadInProgress)
			{
				return;
			}
			if (!ModIODataStore.IsLoggedIn())
			{
				CustomMapManager.SetMapToLoadOnLogin(modId);
				return;
			}
			if (ModIOMapLoader.IsModLoaded(modId))
			{
				return;
			}
			CustomMapManager.loadInProgress = true;
			CustomMapManager.loadingMapId = modId;
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			SubscribedMod subscribedMod;
			if (!ModIODataStore.GetSubscribedModProfile(modId, out subscribedMod))
			{
				ModIODataStore.SubscribeToMod(modId, delegate(Result result)
				{
					if (!result.Succeeded())
					{
						CustomMapManager.instance.HandleMapLoadFailed("FAILED TO SUBSCRIBE TO MAP: " + result.message);
						return;
					}
					if (ModIODataStore.GetSubscribedModProfile(modId, out subscribedMod))
					{
						CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Downloading, 0, "");
						CustomMapManager.instance.LoadSubscribedMod(subscribedMod);
					}
				});
				return;
			}
			CustomMapManager.instance.LoadSubscribedMod(subscribedMod);
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x00166074 File Offset: 0x00164274
		private void LoadSubscribedMod(SubscribedMod subscribedMod)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (subscribedMod.status != SubscribedModStatus.Installed)
			{
				if (subscribedMod.status == SubscribedModStatus.WaitingToDownload || subscribedMod.status == SubscribedModStatus.WaitingToUpdate)
				{
					ModIODataStore.DownloadMod(subscribedMod.modProfile.id, delegate(ModIORequestResult result)
					{
						if (!result.success)
						{
							CustomMapManager.instance.HandleMapLoadFailed("FAILED TO START MAP DOWNLOAD: " + result.message);
						}
						GorillaTelemetry.PostCustomMapDownloadEvent(subscribedMod.modProfile.name, subscribedMod.modProfile.id, subscribedMod.modProfile.creator.username);
					});
				}
				CustomMapManager.waitingForModInstall = true;
				CustomMapManager.waitingForModInstallId = subscribedMod.modProfile.id;
				return;
			}
			this.LoadInstalledMod(subscribedMod.modProfile.id);
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x00166114 File Offset: 0x00164314
		private void LoadInstalledMod(ModId installedModId)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			SubscribedMod subscribedMod;
			ModIODataStore.GetSubscribedModProfile(installedModId, out subscribedMod);
			if (subscribedMod.status != SubscribedModStatus.Installed)
			{
				this.HandleMapLoadFailed("Map is not installed");
				return;
			}
			FileInfo[] files = new DirectoryInfo(subscribedMod.directory).GetFiles("package.json");
			if (files.Length == 0)
			{
				this.HandleMapLoadFailed("Couldn't find package.json in Map files");
				return;
			}
			ModIOMapLoader.LoadMod(installedModId.id, files[0].FullName, new Action<bool>(this.OnMapLoadFinished), new Action<MapLoadStatus, int, string>(this.OnMapLoadProgress), new Action<string>(CustomMapManager.OnSceneLoaded));
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x00058817 File Offset: 0x00056A17
		private void OnMapLoadProgress(MapLoadStatus loadStatus, int progress, string message)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapManager.OnMapLoadStatusChanged.Invoke(loadStatus, progress, message);
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x001661B4 File Offset: 0x001643B4
		private void OnMapLoadFinished(bool success)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			if (success)
			{
				ModIOMapLoader.OpenDoorToMap();
				if (ModIOMapLoader.LoadedMapDescriptor.CustomGamemode != null)
				{
					CustomGameMode.LuaScript = ModIOMapLoader.LoadedMapDescriptor.CustomGamemode.text;
					if (CustomGameMode.LuaScript != "" && CustomGameMode.GameModeInitialized && CustomGameMode.gameScriptRunner == null)
					{
						CustomGameMode.LuaStart();
					}
				}
			}
			CustomMapManager.OnMapLoadComplete.Invoke(success);
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00166248 File Offset: 0x00164448
		private void HandleMapLoadFailed(string message = null)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Error, 0, message ?? "Unknown Error");
			CustomMapManager.OnMapLoadComplete.Invoke(false);
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x0005882E File Offset: 0x00056A2E
		public static bool IsUnloading()
		{
			return !CustomMapManager.disabled && CustomMapManager.unloadInProgress;
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x0005883E File Offset: 0x00056A3E
		public static bool IsLoading(long mapId = 0L)
		{
			if (CustomMapManager.disabled)
			{
				return false;
			}
			if (mapId == 0L)
			{
				return CustomMapManager.loadInProgress || ModIOMapLoader.IsLoading;
			}
			return CustomMapManager.loadInProgress && CustomMapManager.loadingMapId.id == mapId;
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00058871 File Offset: 0x00056A71
		public static void SetMapToLoadOnLogin(ModId modId)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = modId;
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x00058889 File Offset: 0x00056A89
		public static void ClearMapToLoadOnLogin()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x001662A0 File Offset: 0x001644A0
		public static ModId GetRoomMapId()
		{
			if (CustomMapManager.disabled)
			{
				return ModId.Null;
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (CustomMapManager.currentRoomMapModId == ModId.Null && NetworkSystem.Instance.IsMasterClient && ModIOMapLoader.IsModLoaded(0L))
				{
					CustomMapManager.currentRoomMapModId = new ModId(ModIOMapLoader.LoadedMapModId);
				}
				return CustomMapManager.currentRoomMapModId;
			}
			if (CustomMapManager.IsLoading(0L))
			{
				return CustomMapManager.loadingMapId;
			}
			if (ModIOMapLoader.IsModLoaded(0L))
			{
				return new ModId(ModIOMapLoader.LoadedMapModId);
			}
			return ModId.Null;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x0016632C File Offset: 0x0016452C
		public static void SetRoomMod(long modId)
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance || modId == CustomMapManager.currentRoomMapModId.id)
			{
				return;
			}
			if (CustomMapManager.mapIdToLoadOnLogin.id != modId)
			{
				CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			}
			CustomMapManager.currentRoomMapModId = new ModId(modId);
			CustomMapManager.OnRoomMapChanged.Invoke(CustomMapManager.currentRoomMapModId);
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x00166388 File Offset: 0x00164588
		public static void ClearRoomMap()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance || CustomMapManager.currentRoomMapModId == ModId.Null)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomMapManager.OnRoomMapChanged.Invoke(ModId.Null);
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x000588A5 File Offset: 0x00056AA5
		public static bool CanLoadRoomMap()
		{
			return !CustomMapManager.disabled && CustomMapManager.currentRoomMapModId != ModId.Null;
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x000588C4 File Offset: 0x00056AC4
		public static void ApproveAndLoadRoomMap()
		{
			if (CustomMapManager.disabled)
			{
				return;
			}
			CustomMapSerializer.ResetSyncedMapObjects();
			CustomMapManager.LoadMod(CustomMapManager.currentRoomMapModId);
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x001663DC File Offset: 0x001645DC
		private void EnableTeleportHUD(bool enteringVirtualStump)
		{
			if (CustomMapManager.teleportingHUD != null)
			{
				CustomMapManager.teleportingHUD.gameObject.SetActive(true);
				CustomMapManager.teleportingHUD.Initialize(enteringVirtualStump);
				return;
			}
			if (this.teleportingHUDPrefab != null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.teleportingHUDPrefab, main.transform);
					if (gameObject != null)
					{
						CustomMapManager.teleportingHUD = gameObject.GetComponent<VirtualStumpTeleportingHUD>();
						if (CustomMapManager.teleportingHUD != null)
						{
							CustomMapManager.teleportingHUD.Initialize(enteringVirtualStump);
						}
					}
				}
			}
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x000588DD File Offset: 0x00056ADD
		public static void DisableTeleportHUD()
		{
			if (CustomMapManager.teleportingHUD != null)
			{
				CustomMapManager.teleportingHUD.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x000588FC File Offset: 0x00056AFC
		public static void LoadZoneTriggered(int[] scenesToLoad, int[] scenesToUnload, Texture2D[] lightmapsColor, Texture2D[] lightmapsDir)
		{
			ModIOMapLoader.LoadZoneTriggered(scenesToLoad, scenesToUnload, lightmapsColor, lightmapsDir, new Action<string>(CustomMapManager.OnSceneLoaded), new Action<string>(CustomMapManager.OnSceneUnloaded));
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x0005891F File Offset: 0x00056B1F
		private static void OnSceneLoaded(string sceneName)
		{
			CustomMapSerializer.ProcessSceneLoad(sceneName);
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00058927 File Offset: 0x00056B27
		private static void OnSceneUnloaded(string sceneName)
		{
			CustomMapSerializer.UnregisterTriggers(sceneName);
		}

		// Token: 0x04004055 RID: 16469
		[OnEnterPlay_SetNull]
		private static volatile CustomMapManager instance;

		// Token: 0x04004056 RID: 16470
		[OnEnterPlay_Set(false)]
		private static bool hasInstance = false;

		// Token: 0x04004057 RID: 16471
		private static bool disabled;

		// Token: 0x04004058 RID: 16472
		[SerializeField]
		private GorillaNetworkJoinTrigger exitVirtualStumpJoinTrigger;

		// Token: 0x04004059 RID: 16473
		[SerializeField]
		private List<Transform> virtualStumpTeleportLocations;

		// Token: 0x0400405A RID: 16474
		[SerializeField]
		private List<Transform> virtualStumpEjectLocations;

		// Token: 0x0400405B RID: 16475
		[SerializeField]
		private List<GTZone> virtualStumpEjectLocationZones;

		// Token: 0x0400405C RID: 16476
		[SerializeField]
		private GameObject[] rootObjectsToDeactivateAfterTeleport;

		// Token: 0x0400405D RID: 16477
		[SerializeField]
		private GorillaFriendCollider virtualStumpPlayerDetector;

		// Token: 0x0400405E RID: 16478
		[SerializeField]
		private VRTeleporterSerializer arcadeTeleporterNetworkObj;

		// Token: 0x0400405F RID: 16479
		[SerializeField]
		private GameObject teleportingHUDPrefab;

		// Token: 0x04004060 RID: 16480
		[SerializeField]
		private AudioSource localTeleportSFXSource;

		// Token: 0x04004061 RID: 16481
		private static GTZone entranceZone;

		// Token: 0x04004062 RID: 16482
		private static GameModeSelectorButtonLayout gamemodeButtonLayout;

		// Token: 0x04004063 RID: 16483
		private static bool loadInProgress = false;

		// Token: 0x04004064 RID: 16484
		private static ModId loadingMapId = ModId.Null;

		// Token: 0x04004065 RID: 16485
		private static bool unloadInProgress = false;

		// Token: 0x04004066 RID: 16486
		private static ModId unloadingMapId = ModId.Null;

		// Token: 0x04004067 RID: 16487
		private static bool waitingForModInstall = false;

		// Token: 0x04004068 RID: 16488
		private static ModId waitingForModInstallId = ModId.Null;

		// Token: 0x04004069 RID: 16489
		private static string pendingNewPrivateRoomName = "";

		// Token: 0x0400406A RID: 16490
		private static ModId mapIdToLoadOnLogin = ModId.Null;

		// Token: 0x0400406B RID: 16491
		private static Action<bool> currentTeleportCallback;

		// Token: 0x0400406C RID: 16492
		private static bool waitingForDisconnect = false;

		// Token: 0x0400406D RID: 16493
		private static bool waitingForRoomJoin = false;

		// Token: 0x0400406E RID: 16494
		private static bool shouldRetryJoin = false;

		// Token: 0x0400406F RID: 16495
		private static short pendingTeleportVFXIdx = -1;

		// Token: 0x04004070 RID: 16496
		private static bool exitVirtualStumpPending = false;

		// Token: 0x04004071 RID: 16497
		private static ModId currentRoomMapModId = ModId.Null;

		// Token: 0x04004072 RID: 16498
		private static VirtualStumpTeleportingHUD teleportingHUD;

		// Token: 0x04004073 RID: 16499
		public static UnityEvent<ModId> OnRoomMapChanged = new UnityEvent<ModId>();

		// Token: 0x04004074 RID: 16500
		public static UnityEvent<MapLoadStatus, int, string> OnMapLoadStatusChanged = new UnityEvent<MapLoadStatus, int, string>();

		// Token: 0x04004075 RID: 16501
		public static UnityEvent<bool> OnMapLoadComplete = new UnityEvent<bool>();

		// Token: 0x04004076 RID: 16502
		public static UnityEvent OnMapUnloadComplete = new UnityEvent();
	}
}
