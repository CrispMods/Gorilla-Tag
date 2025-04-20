using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaLocomotion.Swimming;
using GorillaNetworking;
using GorillaTag.Rendering;
using GorillaTagScripts.CustomMapSupport;
using GorillaTagScripts.UI.ModIO;
using GT_CustomMapSupportRuntime;
using ModIO;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.ModIO
{
	// Token: 0x02000A0D RID: 2573
	public class CustomMapManager : MonoBehaviour, IBuildValidation
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x00059FCF File Offset: 0x000581CF
		public static bool WaitingForRoomJoin
		{
			get
			{
				return CustomMapManager.waitingForRoomJoin;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x00059FD6 File Offset: 0x000581D6
		public static bool WaitingForDisconnect
		{
			get
			{
				return CustomMapManager.waitingForDisconnect;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x00059FDD File Offset: 0x000581DD
		public static long LoadingMapId
		{
			get
			{
				return CustomMapManager.loadingMapId;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x00059FE9 File Offset: 0x000581E9
		public static long UnloadingMapId
		{
			get
			{
				return CustomMapManager.unloadingMapId;
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0016C668 File Offset: 0x0016A868
		public bool BuildValidationCheck()
		{
			for (int i = 0; i < this.virtualStumpEjectLocations.Length; i++)
			{
				if (this.virtualStumpEjectLocations[i].ejectLocations.IsNullOrEmpty<GameObject>())
				{
					Debug.LogError("List of VirtualStumpEjectLocations is empty for zone " + this.virtualStumpEjectLocations[i].ejectZone.ToString(), base.gameObject);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x00059FF5 File Offset: 0x000581F5
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

		// Token: 0x06004045 RID: 16453 RVA: 0x0016C6CC File Offset: 0x0016A8CC
		public void OnEnable()
		{
			CustomMapManager.gamemodeButtonLayout = UnityEngine.Object.FindObjectOfType<GameModeSelectorButtonLayout>();
			ModIOManager.ModIOEnabledEvent -= this.OnModIOEnabled;
			ModIOManager.ModIOEnabledEvent += this.OnModIOEnabled;
			ModIOManager.ModIODisabledEvent -= this.OnModIODisabled;
			ModIOManager.ModIODisabledEvent += this.OnModIODisabled;
			CMSSerializer.OnTriggerHistoryProcessedForScene.RemoveListener(new UnityAction<string>(CustomMapManager.OnSceneTriggerHistoryProcessed));
			CMSSerializer.OnTriggerHistoryProcessedForScene.AddListener(new UnityAction<string>(CustomMapManager.OnSceneTriggerHistoryProcessed));
			GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.ModIOModManagementEvent.AddListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
			GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Remove(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
			RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnJoinedRoom));
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnected;
			NetworkSystem.Instance.OnReturnedToSinglePlayer += this.OnDisconnected;
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0016C844 File Offset: 0x0016AA44
		public void OnDisable()
		{
			ModIOManager.ModIOEnabledEvent -= this.OnModIOEnabled;
			ModIOManager.ModIODisabledEvent -= this.OnModIODisabled;
			CMSSerializer.OnTriggerHistoryProcessedForScene.RemoveListener(new UnityAction<string>(CustomMapManager.OnSceneTriggerHistoryProcessed));
			GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnected;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnModIOEnabled()
		{
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnModIODisabled()
		{
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0016C8F8 File Offset: 0x0016AAF8
		private void Start()
		{
			for (int i = this.virtualStumpTeleportLocations.Count - 1; i >= 0; i--)
			{
				if (this.virtualStumpTeleportLocations[i] == null)
				{
					this.virtualStumpTeleportLocations.RemoveAt(i);
				}
			}
			for (int j = 0; j < this.virtualStumpEjectLocations.Length; j++)
			{
				if (this.virtualStumpEjectLocations[j].ejectLocations.IsNullOrEmpty<GameObject>())
				{
					GTDev.LogError<string>("List of VirtualStumpEjectLocations is empty for zone " + this.virtualStumpEjectLocations[j].ejectZone.ToString() + "!", null);
				}
				List<GameObject> list = new List<GameObject>(this.virtualStumpEjectLocations[j].ejectLocations);
				for (int k = list.Count - 1; k >= 0; k--)
				{
					if (list[k].IsNull())
					{
						list.RemoveAt(k);
					}
				}
				this.virtualStumpEjectLocations[j].ejectLocations = list.ToArray();
			}
			this.virtualStumpToggleableRoot.SetActive(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0016CA00 File Offset: 0x0016AC00
		private void OnDestroy()
		{
			if (CustomMapManager.instance == this)
			{
				CustomMapManager.instance = null;
				CustomMapManager.hasInstance = false;
			}
			ModIOManager.ModIOEnabledEvent -= this.OnModIOEnabled;
			ModIOManager.ModIODisabledEvent -= this.OnModIODisabled;
			CMSSerializer.OnTriggerHistoryProcessedForScene.RemoveListener(new UnityAction<string>(CustomMapManager.OnSceneTriggerHistoryProcessed));
			GameEvents.ModIOModManagementEvent.RemoveListener(new UnityAction<ModManagementEventType, ModId, Result>(this.HandleModManagementEvent));
			GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
			GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
			NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
			NetworkSystem.Instance.OnReturnedToSinglePlayer -= this.OnDisconnected;
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0016CAD0 File Offset: 0x0016ACD0
		private void HandleModManagementEvent(ModManagementEventType eventType, ModId modId, Result result)
		{
			if (ModIOManager.IsDisabled)
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

		// Token: 0x0600404C RID: 16460 RVA: 0x0016CB64 File Offset: 0x0016AD64
		private void OnModIOLoggedOut()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			ModId @null = ModId.Null;
			if (CustomMapLoader.IsModLoaded(0L) && NetworkSystem.Instance.InRoom && NetworkSystem.Instance.SessionIsPrivate)
			{
				@null = new ModId(CustomMapLoader.LoadedMapModId);
			}
			CustomMapManager.UnloadMod(true);
			CustomMapManager.mapIdToLoadOnLogin = @null;
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x0005A02F File Offset: 0x0005822F
		private void OnModIOLoggedIn()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (CustomMapManager.mapIdToLoadOnLogin != ModId.Null && CustomMapManager.mapIdToLoadOnLogin.Equals(CustomMapManager.currentRoomMapModId) && CustomMapManager.currentRoomMapApproved)
			{
				CustomMapManager.LoadMod(CustomMapManager.mapIdToLoadOnLogin);
			}
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0005A06C File Offset: 0x0005826C
		internal static IEnumerator TeleportToVirtualStump(short teleporterIdx, Action<bool> callback, GTZone playerEntranceZone, VirtualStumpTeleporterSerializer teleporterSerializer)
		{
			if (ModIOManager.IsDisabled)
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
			CustomMapManager.instance.gameObject.SetActive(true);
			CustomMapManager.entranceZone = playerEntranceZone;
			CustomMapManager.teleporterNetworkObj = teleporterSerializer;
			PrivateUIRoom.ForceStartOverlay();
			GorillaTagger.Instance.overrideNotInFocus = true;
			GreyZoneManager greyZoneManager = GreyZoneManager.Instance;
			if (greyZoneManager != null)
			{
				greyZoneManager.ForceStopGreyZone();
			}
			if (CustomMapManager.instance.virtualStumpTeleportLocations.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpTeleportLocations.Count);
				Transform randTeleportTarget = CustomMapManager.instance.virtualStumpTeleportLocations[index];
				CustomMapManager.instance.EnableTeleportHUD(true);
				if (CustomMapManager.teleporterNetworkObj != null)
				{
					CustomMapManager.teleporterNetworkObj.NotifyPlayerTeleporting(teleporterIdx, CustomMapManager.instance.localTeleportSFXSource);
				}
				yield return new WaitForSeconds(0.75f);
				CustomMapManager.instance.virtualStumpToggleableRoot.SetActive(true);
				GTPlayer.Instance.TeleportTo(randTeleportTarget, true, false);
				GorillaComputer.instance.SetInVirtualStump(true);
				yield return null;
				if (VRRig.LocalRig.IsNotNull() && VRRig.LocalRig.zoneEntity.IsNotNull())
				{
					VRRig.LocalRig.zoneEntity.DisableZoneChanges();
				}
				ZoneManagement.SetActiveZone(GTZone.customMaps);
				foreach (GameObject gameObject in CustomMapManager.instance.rootObjectsToDeactivateAfterTeleport)
				{
					if (gameObject != null)
					{
						gameObject.gameObject.SetActive(false);
					}
				}
				if (CustomMapManager.hasInstance && CustomMapManager.instance.virtualStumpZoneShaderSettings.IsNotNull())
				{
					CustomMapManager.instance.virtualStumpZoneShaderSettings.BecomeActiveInstance(false);
				}
				else
				{
					ZoneShaderSettings.ActivateDefaultSettings();
				}
				CustomMapManager.currentTeleportCallback = callback;
				CustomMapManager.pendingNewPrivateRoomName = "";
				CustomMapManager.preTeleportInPrivateRoom = false;
				if (NetworkSystem.Instance.InRoom)
				{
					if (NetworkSystem.Instance.SessionIsPrivate)
					{
						CustomMapManager.preTeleportInPrivateRoom = true;
						CustomMapManager.waitingForRoomJoin = true;
						CustomMapManager.pendingNewPrivateRoomName = GorillaComputer.instance.VStumpRoomPrepend + NetworkSystem.Instance.RoomName;
					}
					CustomMapManager.waitingForLoginDisconnect = true;
					NetworkSystem.Instance.ReturnToSinglePlayer();
				}
				else
				{
					CustomMapManager.RequestPlatformLogin();
				}
				randTeleportTarget = null;
			}
			yield break;
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0016CBBC File Offset: 0x0016ADBC
		private static void OnPlatformLoginComplete(ModIORequestResult result)
		{
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (CustomMapManager.preTeleportInPrivateRoom)
			{
				CustomMapManager.delayedEndTeleportCoroutine = CustomMapManager.instance.StartCoroutine(CustomMapManager.DelayedEndTeleport());
				if (NetworkSystem.Instance.netState != NetSystemState.Idle)
				{
					CustomMapManager.delayedJoinCoroutine = CustomMapManager.instance.StartCoroutine(CustomMapManager.DelayedJoinVStumpPrivateRoom());
				}
				else
				{
					PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResult));
				}
			}
			if (!CustomMapManager.preTeleportInPrivateRoom && !CustomMapManager.waitingForDisconnect)
			{
				CustomMapManager.EndTeleport(true);
			}
			CustomMapManager.preTeleportInPrivateRoom = false;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x0005A090 File Offset: 0x00058290
		private static IEnumerator DelayedJoinVStumpPrivateRoom()
		{
			while (NetworkSystem.Instance.netState != NetSystemState.Idle)
			{
				yield return null;
			}
			PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResult));
			yield break;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0016CC4C File Offset: 0x0016AE4C
		public static void ExitVirtualStump(Action<bool> callback)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			for (int i = 0; i < CustomMapManager.instance.virtualStumpEjectLocations.Length; i++)
			{
				if (CustomMapManager.instance.virtualStumpEjectLocations[i].ejectZone == CustomMapManager.entranceZone && CustomMapManager.instance.virtualStumpEjectLocations[i].ejectLocations.IsNullOrEmpty<GameObject>())
				{
					if (callback != null)
					{
						callback(false);
					}
					return;
				}
			}
			CustomMapManager.instance.dayNightManager.RequestRepopulateLightmaps();
			PrivateUIRoom.ForceStartOverlay();
			GorillaTagger.Instance.overrideNotInFocus = true;
			CustomMapManager.instance.EnableTeleportHUD(false);
			CustomMapManager.currentTeleportCallback = callback;
			CustomMapManager.exitVirtualStumpPending = true;
			if (!CustomMapManager.UnloadMod(false))
			{
				CustomMapManager.FinalizeExitVirtualStump();
			}
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0016CD08 File Offset: 0x0016AF08
		private static void FinalizeExitVirtualStump()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
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
			Transform destination = null;
			for (int j = 0; j < CustomMapManager.instance.virtualStumpEjectLocations.Length; j++)
			{
				if (CustomMapManager.instance.virtualStumpEjectLocations[j].ejectZone == CustomMapManager.entranceZone)
				{
					CustomMapManager.pendingTeleportVFXIdx = (short)UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpEjectLocations[j].ejectLocations.Length);
					destination = CustomMapManager.instance.virtualStumpEjectLocations[j].ejectLocations[(int)CustomMapManager.pendingTeleportVFXIdx].transform;
					break;
				}
			}
			if (VRRig.LocalRig.IsNotNull() && VRRig.LocalRig.zoneEntity.IsNotNull())
			{
				VRRig.LocalRig.zoneEntity.EnableZoneChanges();
			}
			GorillaComputer.instance.SetInVirtualStump(false);
			GTPlayer.Instance.TeleportTo(destination, true, false);
			CustomMapManager.instance.virtualStumpToggleableRoot.SetActive(false);
			ZoneShaderSettings.ActivateDefaultSettings();
			VRRig.LocalRig.EnableVStumpReturnWatch(false);
			CustomMapManager.exitVirtualStumpPending = false;
			if (CustomMapManager.delayedEndTeleportCoroutine != null)
			{
				CustomMapManager.instance.StopCoroutine(CustomMapManager.delayedEndTeleportCoroutine);
			}
			CustomMapManager.delayedEndTeleportCoroutine = CustomMapManager.instance.StartCoroutine(CustomMapManager.DelayedEndTeleport());
			if (CustomMapManager.preTeleportInPrivateRoom)
			{
				CustomMapManager.waitingForRoomJoin = true;
				CustomMapManager.pendingNewPrivateRoomName = CustomMapManager.pendingNewPrivateRoomName.RemoveAll(GorillaComputer.instance.VStumpRoomPrepend, StringComparison.OrdinalIgnoreCase);
				PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResult));
				return;
			}
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

		// Token: 0x06004053 RID: 16467 RVA: 0x0016CFC0 File Offset: 0x0016B1C0
		private static void OnJoinSpecificRoomResult(NetJoinResult result)
		{
			if (ModIOManager.IsDisabled)
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

		// Token: 0x06004054 RID: 16468 RVA: 0x0016D010 File Offset: 0x0016B210
		private static void OnJoinSpecificRoomResultFailureAllowed(NetJoinResult result)
		{
			if (ModIOManager.IsDisabled)
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

		// Token: 0x06004055 RID: 16469 RVA: 0x0016D064 File Offset: 0x0016B264
		public static bool AreAllPlayersInVirtualStump()
		{
			if (ModIOManager.IsDisabled)
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

		// Token: 0x06004056 RID: 16470 RVA: 0x0005A098 File Offset: 0x00058298
		public static bool IsRemotePlayerInVirtualStump(string playerID)
		{
			return CustomMapManager.instance.virtualStumpPlayerDetector.playerIDsCurrentlyTouching.Contains(playerID);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0005A0B1 File Offset: 0x000582B1
		public static bool IsLocalPlayerInVirtualStump()
		{
			return !ModIOManager.IsDisabled && CustomMapManager.hasInstance && CustomMapManager.instance.virtualStumpPlayerDetector.playerIDsCurrentlyTouching.Contains(VRRig.LocalRig.creator.UserId);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x0016D0F4 File Offset: 0x0016B2F4
		private void OnDisconnected()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.ClearRoomMap();
			if (CustomMapManager.waitingForLoginDisconnect)
			{
				CustomMapManager.waitingForLoginDisconnect = false;
				CustomMapManager.RequestPlatformLogin();
				return;
			}
			if (CustomMapManager.waitingForDisconnect)
			{
				CustomMapManager.waitingForDisconnect = false;
				if (CustomMapManager.shouldRetryJoin)
				{
					CustomMapManager.shouldRetryJoin = false;
					PhotonNetworkController.Instance.AttemptToJoinSpecificRoomWithCallback(CustomMapManager.pendingNewPrivateRoomName, JoinType.Solo, new Action<NetJoinResult>(CustomMapManager.OnJoinSpecificRoomResultFailureAllowed));
					return;
				}
				CustomMapManager.EndTeleport(true);
			}
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x0005A0EF File Offset: 0x000582EF
		private static void RequestPlatformLogin()
		{
			ModIOManager.Initialize(delegate(ModIORequestResult result)
			{
				if (result.success)
				{
					ModIOManager.RequestPlatformLogin(new Action<ModIORequestResult>(CustomMapManager.OnPlatformLoginComplete));
					return;
				}
				CustomMapManager.OnPlatformLoginComplete(result);
			});
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x0005A115 File Offset: 0x00058315
		private void OnJoinRoomFailed()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (CustomMapManager.waitingForRoomJoin)
			{
				CustomMapManager.waitingForRoomJoin = false;
				CustomMapManager.EndTeleport(false);
			}
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x0016D168 File Offset: 0x0016B368
		private static void EndTeleport(bool teleportSuccessful)
		{
			if (CustomMapManager.hasInstance)
			{
				if (CustomMapManager.delayedEndTeleportCoroutine != null)
				{
					CustomMapManager.instance.StopCoroutine(CustomMapManager.delayedEndTeleportCoroutine);
					CustomMapManager.delayedEndTeleportCoroutine = null;
				}
				if (CustomMapManager.delayedJoinCoroutine != null)
				{
					CustomMapManager.instance.StopCoroutine(CustomMapManager.delayedJoinCoroutine);
					CustomMapManager.delayedJoinCoroutine = null;
				}
			}
			CustomMapManager.DisableTeleportHUD();
			GorillaTagger.Instance.overrideNotInFocus = false;
			PrivateUIRoom.StopForcedOverlay();
			Action<bool> action = CustomMapManager.currentTeleportCallback;
			if (action != null)
			{
				action(teleportSuccessful);
			}
			CustomMapManager.currentTeleportCallback = null;
			if (CustomMapManager.hasInstance && !GorillaComputer.instance.IsPlayerInVirtualStump())
			{
				CustomMapManager.instance.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x0005A13A File Offset: 0x0005833A
		private static IEnumerator DelayedEndTeleport()
		{
			yield return new WaitForSecondsRealtime(CustomMapManager.instance.maxPostTeleportRoomProcessingTime);
			CustomMapManager.EndTeleport(false);
			yield break;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0016D20C File Offset: 0x0016B40C
		private void OnJoinedRoom()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (CustomMapManager.waitingForRoomJoin)
			{
				CustomMapManager.waitingForRoomJoin = false;
				CustomMapManager.EndTeleport(true);
				if (CustomMapManager.pendingTeleportVFXIdx > -1 && CustomMapManager.teleporterNetworkObj != null)
				{
					CustomMapManager.teleporterNetworkObj.NotifyPlayerReturning(CustomMapManager.pendingTeleportVFXIdx);
					CustomMapManager.pendingTeleportVFXIdx = -1;
				}
			}
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0016D268 File Offset: 0x0016B468
		public static bool UnloadMod(bool returnToSinglePlayerIfInPublic = true)
		{
			if (ModIOManager.IsDisabled)
			{
				return false;
			}
			if (CustomMapManager.unloadInProgress)
			{
				return false;
			}
			if (!CustomMapLoader.IsModLoaded(0L) && !CustomMapLoader.IsLoading)
			{
				return false;
			}
			CustomMapManager.unloadInProgress = true;
			CustomMapManager.unloadingMapId = new ModId(CustomMapLoader.IsModLoaded(0L) ? CustomMapLoader.LoadedMapModId : CustomMapLoader.LoadingMapModId);
			CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Unloading, 0, "");
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomMapManager.currentRoomMapApproved = false;
			CustomGameMode.LuaScript = "";
			if (CustomGameMode.gameScriptRunner != null)
			{
				CustomGameMode.StopScript();
			}
			CustomMapManager.customMapDefaultZoneShaderSettingsInitialized = false;
			CustomMapManager.customMapDefaultZoneShaderProperties = default(CMSZoneShaderSettings.CMSZoneShaderProperties);
			CustomMapManager.loadedCustomMapDefaultZoneShaderSettings = null;
			if (CustomMapManager.hasInstance)
			{
				CustomMapManager.instance.customMapDefaultZoneShaderSettings.CopySettings(CustomMapManager.instance.virtualStumpZoneShaderSettings, false, false);
				CustomMapManager.instance.virtualStumpZoneShaderSettings.BecomeActiveInstance(false);
				CustomMapManager.allCustomMapZoneShaderSettings.Clear();
			}
			CustomMapLoader.CloseDoorAndUnloadMod(new Action(CustomMapManager.OnMapUnloadCompleted));
			if (returnToSinglePlayerIfInPublic && NetworkSystem.Instance.InRoom && !NetworkSystem.Instance.SessionIsPrivate)
			{
				NetworkSystem.Instance.ReturnToSinglePlayer();
			}
			return true;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0016D3B0 File Offset: 0x0016B5B0
		private static void OnMapUnloadCompleted()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.unloadInProgress = false;
			CustomMapManager.OnMapUnloadComplete.Invoke();
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomMapManager.currentRoomMapApproved = false;
			CustomMapManager.OnRoomMapChanged.Invoke(ModId.Null);
			if (CustomMapManager.exitVirtualStumpPending)
			{
				CustomMapManager.FinalizeExitVirtualStump();
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0016D400 File Offset: 0x0016B600
		public static void LoadMod(ModId modId)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance || CustomMapManager.loadInProgress)
			{
				return;
			}
			if (!ModIOManager.IsLoggedIn())
			{
				CustomMapManager.SetMapToLoadOnLogin(modId);
				return;
			}
			if (CustomMapLoader.IsModLoaded(modId))
			{
				return;
			}
			CustomMapManager.loadInProgress = true;
			CustomMapManager.loadingMapId = modId;
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			SubscribedMod subscribedMod;
			if (!ModIOManager.GetSubscribedModProfile(modId, out subscribedMod))
			{
				ModIOManager.SubscribeToMod(modId, delegate(Result result)
				{
					if (!result.Succeeded())
					{
						CustomMapManager.instance.HandleMapLoadFailed("FAILED TO SUBSCRIBE TO MAP: " + result.message);
						return;
					}
					if (ModIOManager.GetSubscribedModProfile(modId, out subscribedMod))
					{
						CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Downloading, 0, "");
						CustomMapManager.instance.LoadSubscribedMod(subscribedMod);
					}
				});
				return;
			}
			CustomMapManager.instance.LoadSubscribedMod(subscribedMod);
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x0016D4C0 File Offset: 0x0016B6C0
		private void LoadSubscribedMod(SubscribedMod subscribedMod)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (subscribedMod.status != SubscribedModStatus.Installed)
			{
				if (subscribedMod.status == SubscribedModStatus.WaitingToDownload || subscribedMod.status == SubscribedModStatus.WaitingToUpdate)
				{
					ModIOManager.DownloadMod(subscribedMod.modProfile.id, delegate(ModIORequestResult result)
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

		// Token: 0x06004062 RID: 16482 RVA: 0x0016D560 File Offset: 0x0016B760
		private void LoadInstalledMod(ModId installedModId)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			SubscribedMod subscribedMod;
			ModIOManager.GetSubscribedModProfile(installedModId, out subscribedMod);
			if (subscribedMod.status != SubscribedModStatus.Installed)
			{
				this.HandleMapLoadFailed("MAP IS NOT INSTALLED");
				return;
			}
			FileInfo[] files = new DirectoryInfo(subscribedMod.directory).GetFiles("package.json");
			if (files.Length == 0)
			{
				this.HandleMapLoadFailed("COULD NOT FIND PACKAGE.JSON IN MAP FILES");
				return;
			}
			CustomMapLoader.LoadMap(installedModId.id, files[0].FullName, new Action<bool>(this.OnMapLoadFinished), new Action<MapLoadStatus, int, string>(this.OnMapLoadProgress), new Action<string>(CustomMapManager.OnSceneLoaded));
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0005A142 File Offset: 0x00058342
		private void OnMapLoadProgress(MapLoadStatus loadStatus, int progress, string message)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.OnMapLoadStatusChanged.Invoke(loadStatus, progress, message);
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0016D600 File Offset: 0x0016B800
		private void OnMapLoadFinished(bool success)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			if (success)
			{
				CustomMapLoader.OpenDoorToMap();
				if (CustomMapLoader.LoadedMapDescriptor.CustomGamemode != null)
				{
					CustomGameMode.LuaScript = CustomMapLoader.LoadedMapDescriptor.CustomGamemode.text;
					if (CustomGameMode.LuaScript != "" && CustomGameMode.GameModeInitialized && CustomGameMode.gameScriptRunner == null)
					{
						CustomGameMode.LuaStart();
					}
				}
			}
			CustomMapManager.OnMapLoadComplete.Invoke(success);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0016D694 File Offset: 0x0016B894
		private void HandleMapLoadFailed(string message = null)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.loadInProgress = false;
			CustomMapManager.loadingMapId = ModId.Null;
			CustomMapManager.waitingForModInstall = false;
			CustomMapManager.waitingForModInstallId = ModId.Null;
			CustomMapManager.OnMapLoadStatusChanged.Invoke(MapLoadStatus.Error, 0, message ?? "UNKNOWN ERROR");
			CustomMapManager.OnMapLoadComplete.Invoke(false);
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0005A159 File Offset: 0x00058359
		public static bool IsUnloading()
		{
			return !ModIOManager.IsDisabled && CustomMapManager.unloadInProgress;
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0005A169 File Offset: 0x00058369
		public static bool IsLoading(long mapId = 0L)
		{
			if (ModIOManager.IsDisabled)
			{
				return false;
			}
			if (mapId == 0L)
			{
				return CustomMapManager.loadInProgress || CustomMapLoader.IsLoading;
			}
			return CustomMapManager.loadInProgress && CustomMapManager.loadingMapId.id == mapId;
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0005A19C File Offset: 0x0005839C
		public static void SetMapToLoadOnLogin(ModId modId)
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = modId;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0005A1B4 File Offset: 0x000583B4
		public static void ClearMapToLoadOnLogin()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0016D6EC File Offset: 0x0016B8EC
		public static ModId GetRoomMapId()
		{
			if (ModIOManager.IsDisabled)
			{
				return ModId.Null;
			}
			if (NetworkSystem.Instance.InRoom)
			{
				if (CustomMapManager.currentRoomMapModId == ModId.Null && NetworkSystem.Instance.IsMasterClient && CustomMapLoader.IsModLoaded(0L))
				{
					CustomMapManager.currentRoomMapModId = new ModId(CustomMapLoader.LoadedMapModId);
				}
				return CustomMapManager.currentRoomMapModId;
			}
			if (CustomMapManager.IsLoading(0L))
			{
				return CustomMapManager.loadingMapId;
			}
			if (CustomMapLoader.IsModLoaded(0L))
			{
				return new ModId(CustomMapLoader.LoadedMapModId);
			}
			return ModId.Null;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0016D778 File Offset: 0x0016B978
		public static void SetRoomMod(long modId)
		{
			if (ModIOManager.IsDisabled)
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
			CustomMapManager.currentRoomMapApproved = false;
			CustomMapManager.OnRoomMapChanged.Invoke(CustomMapManager.currentRoomMapModId);
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x0016D7DC File Offset: 0x0016B9DC
		public static void ClearRoomMap()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (!CustomMapManager.hasInstance || CustomMapManager.currentRoomMapModId == ModId.Null)
			{
				return;
			}
			CustomMapManager.mapIdToLoadOnLogin = ModId.Null;
			CustomMapManager.currentRoomMapModId = ModId.Null;
			CustomMapManager.currentRoomMapApproved = false;
			CustomMapManager.OnRoomMapChanged.Invoke(ModId.Null);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0005A1D0 File Offset: 0x000583D0
		public static bool CanLoadRoomMap()
		{
			return !ModIOManager.IsDisabled && CustomMapManager.currentRoomMapModId != ModId.Null;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0005A1EF File Offset: 0x000583EF
		public static void ApproveAndLoadRoomMap()
		{
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			CustomMapManager.currentRoomMapApproved = true;
			CMSSerializer.ResetSyncedMapObjects();
			CustomMapManager.LoadMod(CustomMapManager.currentRoomMapModId);
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0005A20E File Offset: 0x0005840E
		public static void RequestEnableTeleportHUD(bool enteringVirtualStump)
		{
			if (CustomMapManager.hasInstance)
			{
				CustomMapManager.instance.EnableTeleportHUD(enteringVirtualStump);
			}
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0016D834 File Offset: 0x0016BA34
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

		// Token: 0x06004071 RID: 16497 RVA: 0x0005A224 File Offset: 0x00058424
		public static void DisableTeleportHUD()
		{
			if (CustomMapManager.teleportingHUD != null)
			{
				CustomMapManager.teleportingHUD.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0005A243 File Offset: 0x00058443
		public static void LoadZoneTriggered(int[] scenesToLoad, int[] scenesToUnload)
		{
			CustomMapLoader.LoadZoneTriggered(scenesToLoad, scenesToUnload, new Action<string>(CustomMapManager.OnSceneLoaded), new Action<string>(CustomMapManager.OnSceneUnloaded));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0005A264 File Offset: 0x00058464
		private static void OnSceneLoaded(string sceneName)
		{
			CMSSerializer.ProcessSceneLoad(sceneName);
			CustomMapManager.ProcessZoneShaderSettings(sceneName);
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0016D8C8 File Offset: 0x0016BAC8
		private static void OnSceneUnloaded(string sceneName)
		{
			CMSSerializer.UnregisterTriggers(sceneName);
			for (int i = CustomMapManager.allCustomMapZoneShaderSettings.Count - 1; i >= 0; i--)
			{
				if (CustomMapManager.allCustomMapZoneShaderSettings[i].IsNull())
				{
					CustomMapManager.allCustomMapZoneShaderSettings.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x0016D910 File Offset: 0x0016BB10
		private static void OnSceneTriggerHistoryProcessed(string sceneName)
		{
			CapsuleCollider bodyCollider = GTPlayer.Instance.bodyCollider;
			SphereCollider headCollider = GTPlayer.Instance.headCollider;
			Vector3 position = bodyCollider.transform.TransformPoint(bodyCollider.center);
			float radius = Mathf.Max(bodyCollider.height, bodyCollider.radius) * GTPlayer.Instance.scale;
			Collider[] array = new Collider[100];
			Physics.OverlapSphereNonAlloc(position, radius, array);
			foreach (Collider collider in array)
			{
				if (collider != null && collider.gameObject.scene.name.Equals(sceneName))
				{
					CMSTrigger[] components = collider.gameObject.GetComponents<CMSTrigger>();
					for (int j = 0; j < components.Length; j++)
					{
						if (components[j] != null)
						{
							components[j].OnTriggerEnter(bodyCollider);
							components[j].OnTriggerEnter(headCollider);
						}
					}
					CMSLoadingZone[] components2 = collider.gameObject.GetComponents<CMSLoadingZone>();
					for (int k = 0; k < components2.Length; k++)
					{
						if (components2[k] != null)
						{
							components2[k].OnTriggerEnter(bodyCollider);
						}
					}
					CMSZoneShaderSettingsTrigger[] components3 = collider.gameObject.GetComponents<CMSZoneShaderSettingsTrigger>();
					for (int l = 0; l < components3.Length; l++)
					{
						if (components3[l] != null)
						{
							components3[l].OnTriggerEnter(bodyCollider);
						}
					}
					HoverboardAreaTrigger[] components4 = collider.gameObject.GetComponents<HoverboardAreaTrigger>();
					for (int m = 0; m < components4.Length; m++)
					{
						if (components4[m] != null)
						{
							components4[m].OnTriggerEnter(headCollider);
						}
					}
					WaterVolume[] components5 = collider.gameObject.GetComponents<WaterVolume>();
					for (int n = 0; n < components5.Length; n++)
					{
						if (components5[n] != null)
						{
							components5[n].OnTriggerEnter(bodyCollider);
							components5[n].OnTriggerEnter(headCollider);
						}
					}
				}
			}
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0005A272 File Offset: 0x00058472
		public static void SetDefaultZoneShaderSettings(ZoneShaderSettings defaultCustomMapShaderSettings, CMSZoneShaderSettings.CMSZoneShaderProperties defaultZoneShaderProperties)
		{
			if (CustomMapManager.hasInstance)
			{
				CustomMapManager.instance.customMapDefaultZoneShaderSettings.CopySettings(defaultCustomMapShaderSettings, true, false);
				CustomMapManager.loadedCustomMapDefaultZoneShaderSettings = defaultCustomMapShaderSettings;
				CustomMapManager.customMapDefaultZoneShaderProperties = defaultZoneShaderProperties;
				CustomMapManager.customMapDefaultZoneShaderSettingsInitialized = true;
			}
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0016DAF0 File Offset: 0x0016BCF0
		private static void ProcessZoneShaderSettings(string loadedSceneName)
		{
			if (CustomMapManager.hasInstance && CustomMapManager.customMapDefaultZoneShaderSettingsInitialized && CustomMapManager.customMapDefaultZoneShaderProperties.isInitialized)
			{
				for (int i = 0; i < CustomMapManager.allCustomMapZoneShaderSettings.Count; i++)
				{
					if (CustomMapManager.allCustomMapZoneShaderSettings[i].IsNotNull() && CustomMapManager.allCustomMapZoneShaderSettings[i] != CustomMapManager.loadedCustomMapDefaultZoneShaderSettings && CustomMapManager.allCustomMapZoneShaderSettings[i].gameObject.scene.name.Equals(loadedSceneName))
					{
						CustomMapManager.allCustomMapZoneShaderSettings[i].ReplaceDefaultValues(CustomMapManager.customMapDefaultZoneShaderProperties, true);
					}
				}
				return;
			}
			if (CustomMapManager.hasInstance && CustomMapManager.instance.virtualStumpZoneShaderSettings.IsNotNull())
			{
				for (int j = 0; j < CustomMapManager.allCustomMapZoneShaderSettings.Count; j++)
				{
					if (CustomMapManager.allCustomMapZoneShaderSettings[j].IsNotNull() && CustomMapManager.allCustomMapZoneShaderSettings[j].gameObject.scene.name.Equals(loadedSceneName))
					{
						CustomMapManager.allCustomMapZoneShaderSettings[j].ReplaceDefaultValues(CustomMapManager.instance.virtualStumpZoneShaderSettings, true);
					}
				}
			}
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0005A2A1 File Offset: 0x000584A1
		public static void AddZoneShaderSettings(ZoneShaderSettings zoneShaderSettings)
		{
			CustomMapManager.allCustomMapZoneShaderSettings.AddIfNew(zoneShaderSettings);
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0005A2AE File Offset: 0x000584AE
		public static void ActivateDefaultZoneShaderSettings()
		{
			if (CustomMapManager.hasInstance && CustomMapManager.customMapDefaultZoneShaderSettingsInitialized)
			{
				CustomMapManager.instance.customMapDefaultZoneShaderSettings.BecomeActiveInstance(true);
				return;
			}
			if (CustomMapManager.hasInstance)
			{
				CustomMapManager.instance.virtualStumpZoneShaderSettings.BecomeActiveInstance(true);
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0016DC1C File Offset: 0x0016BE1C
		public static void ReturnToVirtualStump()
		{
			if (!CustomMapManager.hasInstance)
			{
				return;
			}
			if (ModIOManager.IsDisabled)
			{
				return;
			}
			if (CustomMapManager.instance.virtualStumpTeleportLocations.Count > 0)
			{
				Transform destination = CustomMapManager.instance.virtualStumpTeleportLocations[UnityEngine.Random.Range(0, CustomMapManager.instance.virtualStumpTeleportLocations.Count)];
				GTPlayer gtplayer = GTPlayer.Instance;
				if (gtplayer != null)
				{
					CustomMapLoader.ResetToInitialZone(new Action<string>(CustomMapManager.OnSceneLoaded), new Action<string>(CustomMapManager.OnSceneUnloaded));
					gtplayer.TeleportTo(destination, true, false);
				}
			}
		}

		// Token: 0x04004133 RID: 16691
		[OnEnterPlay_SetNull]
		private static volatile CustomMapManager instance;

		// Token: 0x04004134 RID: 16692
		[OnEnterPlay_Set(false)]
		private static bool hasInstance = false;

		// Token: 0x04004135 RID: 16693
		[SerializeField]
		private GameObject virtualStumpToggleableRoot;

		// Token: 0x04004136 RID: 16694
		[SerializeField]
		private GorillaNetworkJoinTrigger exitVirtualStumpJoinTrigger;

		// Token: 0x04004137 RID: 16695
		[SerializeField]
		private List<Transform> virtualStumpTeleportLocations;

		// Token: 0x04004138 RID: 16696
		[SerializeField]
		private ZoneEjectLocations[] virtualStumpEjectLocations;

		// Token: 0x04004139 RID: 16697
		[SerializeField]
		private GameObject[] rootObjectsToDeactivateAfterTeleport;

		// Token: 0x0400413A RID: 16698
		[SerializeField]
		private GorillaFriendCollider virtualStumpPlayerDetector;

		// Token: 0x0400413B RID: 16699
		[SerializeField]
		private ZoneShaderSettings virtualStumpZoneShaderSettings;

		// Token: 0x0400413C RID: 16700
		[SerializeField]
		private BetterDayNightManager dayNightManager;

		// Token: 0x0400413D RID: 16701
		[SerializeField]
		private ZoneShaderSettings customMapDefaultZoneShaderSettings;

		// Token: 0x0400413E RID: 16702
		[SerializeField]
		private GameObject teleportingHUDPrefab;

		// Token: 0x0400413F RID: 16703
		[SerializeField]
		private AudioSource localTeleportSFXSource;

		// Token: 0x04004140 RID: 16704
		private static GTZone entranceZone;

		// Token: 0x04004141 RID: 16705
		private static VirtualStumpTeleporterSerializer teleporterNetworkObj = null;

		// Token: 0x04004142 RID: 16706
		[SerializeField]
		private float maxPostTeleportRoomProcessingTime = 15f;

		// Token: 0x04004143 RID: 16707
		private static bool customMapDefaultZoneShaderSettingsInitialized;

		// Token: 0x04004144 RID: 16708
		private static ZoneShaderSettings loadedCustomMapDefaultZoneShaderSettings;

		// Token: 0x04004145 RID: 16709
		private static CMSZoneShaderSettings.CMSZoneShaderProperties customMapDefaultZoneShaderProperties;

		// Token: 0x04004146 RID: 16710
		private static readonly List<ZoneShaderSettings> allCustomMapZoneShaderSettings = new List<ZoneShaderSettings>();

		// Token: 0x04004147 RID: 16711
		private static GameModeSelectorButtonLayout gamemodeButtonLayout;

		// Token: 0x04004148 RID: 16712
		private static bool loadInProgress = false;

		// Token: 0x04004149 RID: 16713
		private static ModId loadingMapId = ModId.Null;

		// Token: 0x0400414A RID: 16714
		private static bool unloadInProgress = false;

		// Token: 0x0400414B RID: 16715
		private static ModId unloadingMapId = ModId.Null;

		// Token: 0x0400414C RID: 16716
		private static bool waitingForModInstall = false;

		// Token: 0x0400414D RID: 16717
		private static ModId waitingForModInstallId = ModId.Null;

		// Token: 0x0400414E RID: 16718
		private static bool preTeleportInPrivateRoom = false;

		// Token: 0x0400414F RID: 16719
		private static string pendingNewPrivateRoomName = "";

		// Token: 0x04004150 RID: 16720
		private static ModId mapIdToLoadOnLogin = ModId.Null;

		// Token: 0x04004151 RID: 16721
		private static Action<bool> currentTeleportCallback;

		// Token: 0x04004152 RID: 16722
		private static bool waitingForLoginDisconnect = false;

		// Token: 0x04004153 RID: 16723
		private static bool waitingForDisconnect = false;

		// Token: 0x04004154 RID: 16724
		private static bool waitingForRoomJoin = false;

		// Token: 0x04004155 RID: 16725
		private static bool shouldRetryJoin = false;

		// Token: 0x04004156 RID: 16726
		private static short pendingTeleportVFXIdx = -1;

		// Token: 0x04004157 RID: 16727
		private static bool exitVirtualStumpPending = false;

		// Token: 0x04004158 RID: 16728
		private static ModId currentRoomMapModId = ModId.Null;

		// Token: 0x04004159 RID: 16729
		private static bool currentRoomMapApproved = false;

		// Token: 0x0400415A RID: 16730
		private static VirtualStumpTeleportingHUD teleportingHUD;

		// Token: 0x0400415B RID: 16731
		private static Coroutine delayedEndTeleportCoroutine;

		// Token: 0x0400415C RID: 16732
		private static Coroutine delayedJoinCoroutine;

		// Token: 0x0400415D RID: 16733
		public static UnityEvent<ModId> OnRoomMapChanged = new UnityEvent<ModId>();

		// Token: 0x0400415E RID: 16734
		public static UnityEvent<MapLoadStatus, int, string> OnMapLoadStatusChanged = new UnityEvent<MapLoadStatus, int, string>();

		// Token: 0x0400415F RID: 16735
		public static UnityEvent<bool> OnMapLoadComplete = new UnityEvent<bool>();

		// Token: 0x04004160 RID: 16736
		public static UnityEvent OnMapUnloadComplete = new UnityEvent();
	}
}
