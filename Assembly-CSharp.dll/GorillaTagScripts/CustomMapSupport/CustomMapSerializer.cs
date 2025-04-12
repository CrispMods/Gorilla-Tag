using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EF RID: 2543
	internal class CustomMapSerializer : GorillaSerializer
	{
		// Token: 0x06003F74 RID: 16244 RVA: 0x00058A5B File Offset: 0x00056C5B
		public void Awake()
		{
			if (CustomMapSerializer.instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			CustomMapSerializer.instance = this;
			CustomMapSerializer.hasInstance = true;
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00058A80 File Offset: 0x00056C80
		public void OnEnable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
			CustomMapManager.OnMapLoadComplete.AddListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x00058AAE File Offset: 0x00056CAE
		public void OnDisable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00058AC6 File Offset: 0x00056CC6
		private void OnCustomMapLoaded(bool success)
		{
			if (success)
			{
				CustomMapSerializer.RequestSyncTriggerHistory();
			}
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00058AD0 File Offset: 0x00056CD0
		public static void ResetSyncedMapObjects()
		{
			CustomMapSerializer.triggerHistory.Clear();
			CustomMapSerializer.triggerCounts.Clear();
			CustomMapSerializer.registeredTriggersPerScene.Clear();
			CustomMapSerializer.waitingForTriggerHistory = false;
			CustomMapSerializer.waitingForTriggerCounts = false;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00166BE8 File Offset: 0x00164DE8
		public static void RegisterTrigger(string sceneName, CustomMapTrigger trigger)
		{
			Dictionary<byte, CustomMapTrigger> dictionary;
			if (CustomMapSerializer.registeredTriggersPerScene.TryGetValue(sceneName, out dictionary))
			{
				if (!dictionary.ContainsKey(trigger.GetID()))
				{
					dictionary.Add(trigger.GetID(), trigger);
					return;
				}
			}
			else
			{
				CustomMapSerializer.registeredTriggersPerScene.Add(sceneName, new Dictionary<byte, CustomMapTrigger>
				{
					{
						trigger.GetID(),
						trigger
					}
				});
			}
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00166C40 File Offset: 0x00164E40
		private static bool TryGetRegisteredTrigger(byte triggerID, out CustomMapTrigger trigger)
		{
			trigger = null;
			foreach (KeyValuePair<string, Dictionary<byte, CustomMapTrigger>> keyValuePair in CustomMapSerializer.registeredTriggersPerScene)
			{
				if (keyValuePair.Value.TryGetValue(triggerID, out trigger))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00058AFC File Offset: 0x00056CFC
		public static void UnregisterTriggers(string forScene)
		{
			CustomMapSerializer.registeredTriggersPerScene.Remove(forScene);
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x00058B0A File Offset: 0x00056D0A
		public static void ResetTrigger(byte triggerID)
		{
			CustomMapSerializer.triggerCounts.Remove(triggerID);
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x00166CA8 File Offset: 0x00164EA8
		private static void RequestSyncTriggerHistory()
		{
			if (!CustomMapSerializer.hasInstance || !NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			CustomMapSerializer.waitingForTriggerHistory = true;
			CustomMapSerializer.waitingForTriggerCounts = true;
			CustomMapSerializer.instance.SendRPC("RequestSyncTriggerHistory_RPC", false, Array.Empty<object>());
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x00166CF8 File Offset: 0x00164EF8
		[PunRPC]
		private void RequestSyncTriggerHistory_RPC(PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "RequestSyncTriggerHistory_RPC");
			if (!NetworkSystem.Instance.InRoom || !NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
			if (player.CheckSingleCallRPC(NetPlayer.SingleCallRPC.CMS_RequestTriggerHistory))
			{
				return;
			}
			player.ReceivedSingleCallRPC(NetPlayer.SingleCallRPC.CMS_RequestTriggerHistory);
			byte[] array = CustomMapSerializer.triggerHistory.ToArray();
			base.SendRPC("SyncTriggerHistory_RPC", info.Sender, new object[]
			{
				array
			});
			base.SendRPC("SyncTriggerCounts_RPC", info.Sender, new object[]
			{
				CustomMapSerializer.triggerCounts
			});
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00166D90 File Offset: 0x00164F90
		[PunRPC]
		private void SyncTriggerHistory_RPC(byte[] syncedTriggerHistory, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "SyncTriggerHistory_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			if (!CustomMapSerializer.waitingForTriggerHistory)
			{
				return;
			}
			CustomMapSerializer.triggerHistory.Clear();
			if (!syncedTriggerHistory.IsNullOrEmpty<byte>())
			{
				CustomMapSerializer.triggerHistory.AddRange(syncedTriggerHistory);
			}
			CustomMapSerializer.waitingForTriggerHistory = false;
			foreach (string forScene in CustomMapSerializer.scenesWaitingForTriggerHistory)
			{
				CustomMapSerializer.ProcessTriggerHistory(forScene);
			}
			CustomMapSerializer.scenesWaitingForTriggerHistory.Clear();
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00166E3C File Offset: 0x0016503C
		[PunRPC]
		private void SyncTriggerCounts_RPC(Dictionary<byte, byte> syncedTriggerCounts, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "SyncTriggerCounts_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			if (!CustomMapSerializer.waitingForTriggerCounts)
			{
				return;
			}
			CustomMapSerializer.triggerCounts.Clear();
			if (syncedTriggerCounts != null && syncedTriggerCounts.Count > 0)
			{
				CustomMapSerializer.triggerCounts = syncedTriggerCounts;
			}
			CustomMapSerializer.waitingForTriggerCounts = false;
			foreach (string forScene in CustomMapSerializer.scenesWaitingForTriggerCounts)
			{
				CustomMapSerializer.ProcessTriggerCounts(forScene);
			}
			CustomMapSerializer.scenesWaitingForTriggerCounts.Clear();
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00058B18 File Offset: 0x00056D18
		public static void ProcessSceneLoad(string sceneName)
		{
			if (CustomMapSerializer.waitingForTriggerHistory)
			{
				CustomMapSerializer.scenesWaitingForTriggerHistory.Add(sceneName);
			}
			else
			{
				CustomMapSerializer.ProcessTriggerHistory(sceneName);
			}
			if (CustomMapSerializer.waitingForTriggerCounts)
			{
				CustomMapSerializer.scenesWaitingForTriggerCounts.Add(sceneName);
				return;
			}
			CustomMapSerializer.ProcessTriggerCounts(sceneName);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x00166EE4 File Offset: 0x001650E4
		private static void ProcessTriggerHistory(string forScene)
		{
			Dictionary<byte, CustomMapTrigger> dictionary;
			if (CustomMapSerializer.registeredTriggersPerScene.TryGetValue(forScene, out dictionary))
			{
				foreach (byte key in CustomMapSerializer.triggerHistory)
				{
					CustomMapTrigger customMapTrigger;
					if (dictionary.TryGetValue(key, out customMapTrigger))
					{
						customMapTrigger.Trigger(1.0, false, true);
					}
				}
			}
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x00166F5C File Offset: 0x0016515C
		private static void ProcessTriggerCounts(string forScene)
		{
			Dictionary<byte, CustomMapTrigger> dictionary;
			if (CustomMapSerializer.registeredTriggersPerScene.TryGetValue(forScene, out dictionary))
			{
				List<byte> list = new List<byte>();
				foreach (KeyValuePair<byte, byte> keyValuePair in CustomMapSerializer.triggerCounts)
				{
					CustomMapTrigger customMapTrigger;
					if (dictionary.TryGetValue(keyValuePair.Key, out customMapTrigger))
					{
						if (customMapTrigger.numAllowedTriggers > 0)
						{
							customMapTrigger.SetTriggerCount(keyValuePair.Value);
						}
						else
						{
							list.Add(keyValuePair.Key);
						}
					}
				}
				foreach (byte key in list)
				{
					CustomMapSerializer.triggerCounts.Remove(key);
				}
			}
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0016703C File Offset: 0x0016523C
		public static void RequestTrigger(byte triggerID)
		{
			if (!CustomMapSerializer.hasInstance)
			{
				return;
			}
			if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
			{
				double triggerTime = (double)Time.time;
				if (NetworkSystem.Instance.InRoom)
				{
					triggerTime = PhotonNetwork.Time;
					CustomMapSerializer.instance.SendRPC("ActivateTrigger_RPC", true, new object[]
					{
						triggerID,
						NetworkSystem.Instance.LocalPlayer.ActorNumber
					});
				}
				CustomMapSerializer.instance.ActivateTrigger(triggerID, triggerTime, true);
				return;
			}
			CustomMapSerializer.instance.SendRPC("RequestTrigger_RPC", false, new object[]
			{
				triggerID
			});
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x001670EC File Offset: 0x001652EC
		[PunRPC]
		private void RequestTrigger_RPC(byte triggerID, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "RequestTrigger_RPC");
			if (!NetworkSystem.Instance.InRoom || !NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
			RigContainer rigContainer;
			if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[11].CallLimitSettings.CheckCallTime(Time.unscaledTime))
			{
				return;
			}
			CustomMapTrigger customMapTrigger;
			if (CustomMapSerializer.TryGetRegisteredTrigger(triggerID, out customMapTrigger))
			{
				if (!customMapTrigger.CanTrigger())
				{
					return;
				}
				Vector3 position = customMapTrigger.gameObject.transform.position;
				RigContainer rigContainer2;
				if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer2))
				{
					return;
				}
				if ((rigContainer2.Rig.bodyTransform.position - position).sqrMagnitude > customMapTrigger.validationDistanceSquared)
				{
					return;
				}
			}
			base.SendRPC("ActivateTrigger_RPC", true, new object[]
			{
				triggerID,
				info.Sender.ActorNumber
			});
			this.ActivateTrigger(triggerID, info.SentServerTime, false);
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x00167204 File Offset: 0x00165404
		[PunRPC]
		private void ActivateTrigger_RPC(byte triggerID, int originatingPlayer, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "ActivateTrigger_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			if (!CustomMapSerializer.ActivateTriggerCallLimiter.CheckCallTime(Time.unscaledTime))
			{
				return;
			}
			this.ActivateTrigger(triggerID, info.SentServerTime, NetworkSystem.Instance.LocalPlayer.ActorNumber == originatingPlayer);
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x00167268 File Offset: 0x00165468
		private void ActivateTrigger(byte triggerID, double triggerTime = -1.0, bool originatedLocally = false)
		{
			CustomMapTrigger customMapTrigger;
			bool flag = CustomMapSerializer.TryGetRegisteredTrigger(triggerID, out customMapTrigger);
			if (!double.IsFinite(triggerTime))
			{
				triggerTime = -1.0;
			}
			byte b;
			bool flag2 = CustomMapSerializer.triggerCounts.TryGetValue(triggerID, out b);
			bool flag3 = !flag || customMapTrigger.numAllowedTriggers > 0;
			if (flag2)
			{
				CustomMapSerializer.triggerCounts[triggerID] = ((b == byte.MaxValue) ? byte.MaxValue : (b += 1));
			}
			else if (flag3)
			{
				CustomMapSerializer.triggerCounts.Add(triggerID, 1);
			}
			CustomMapSerializer.triggerHistory.Remove(triggerID);
			CustomMapSerializer.triggerHistory.Add(triggerID);
			if (flag)
			{
				customMapTrigger.Trigger(triggerTime, originatedLocally, false);
			}
		}

		// Token: 0x04004090 RID: 16528
		[OnEnterPlay_SetNull]
		private static volatile CustomMapSerializer instance;

		// Token: 0x04004091 RID: 16529
		[OnEnterPlay_Set(false)]
		private static bool hasInstance;

		// Token: 0x04004092 RID: 16530
		private static Dictionary<string, Dictionary<byte, CustomMapTrigger>> registeredTriggersPerScene = new Dictionary<string, Dictionary<byte, CustomMapTrigger>>();

		// Token: 0x04004093 RID: 16531
		private static List<byte> triggerHistory = new List<byte>();

		// Token: 0x04004094 RID: 16532
		private static Dictionary<byte, byte> triggerCounts = new Dictionary<byte, byte>();

		// Token: 0x04004095 RID: 16533
		private static bool waitingForTriggerHistory;

		// Token: 0x04004096 RID: 16534
		private static List<string> scenesWaitingForTriggerHistory = new List<string>();

		// Token: 0x04004097 RID: 16535
		private static bool waitingForTriggerCounts;

		// Token: 0x04004098 RID: 16536
		private static List<string> scenesWaitingForTriggerCounts = new List<string>();

		// Token: 0x04004099 RID: 16537
		private static CallLimiter ActivateTriggerCallLimiter = new CallLimiter(50, 1f, 0.5f);
	}
}
