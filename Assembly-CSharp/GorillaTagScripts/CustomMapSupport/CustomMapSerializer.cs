using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x020009EC RID: 2540
	internal class CustomMapSerializer : GorillaSerializer
	{
		// Token: 0x06003F68 RID: 16232 RVA: 0x0012C47D File Offset: 0x0012A67D
		public void Awake()
		{
			if (CustomMapSerializer.instance != null)
			{
				Object.Destroy(this);
			}
			CustomMapSerializer.instance = this;
			CustomMapSerializer.hasInstance = true;
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x0012C4A2 File Offset: 0x0012A6A2
		public void OnEnable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
			CustomMapManager.OnMapLoadComplete.AddListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x0012C4D0 File Offset: 0x0012A6D0
		public void OnDisable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x0012C4E8 File Offset: 0x0012A6E8
		private void OnCustomMapLoaded(bool success)
		{
			if (success)
			{
				CustomMapSerializer.RequestSyncTriggerHistory();
			}
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x0012C4F2 File Offset: 0x0012A6F2
		public static void ResetSyncedMapObjects()
		{
			CustomMapSerializer.triggerHistory.Clear();
			CustomMapSerializer.triggerCounts.Clear();
			CustomMapSerializer.registeredTriggersPerScene.Clear();
			CustomMapSerializer.waitingForTriggerHistory = false;
			CustomMapSerializer.waitingForTriggerCounts = false;
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x0012C520 File Offset: 0x0012A720
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

		// Token: 0x06003F6E RID: 16238 RVA: 0x0012C578 File Offset: 0x0012A778
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

		// Token: 0x06003F6F RID: 16239 RVA: 0x0012C5E0 File Offset: 0x0012A7E0
		public static void UnregisterTriggers(string forScene)
		{
			CustomMapSerializer.registeredTriggersPerScene.Remove(forScene);
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0012C5EE File Offset: 0x0012A7EE
		public static void ResetTrigger(byte triggerID)
		{
			CustomMapSerializer.triggerCounts.Remove(triggerID);
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x0012C5FC File Offset: 0x0012A7FC
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

		// Token: 0x06003F72 RID: 16242 RVA: 0x0012C64C File Offset: 0x0012A84C
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

		// Token: 0x06003F73 RID: 16243 RVA: 0x0012C6E4 File Offset: 0x0012A8E4
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

		// Token: 0x06003F74 RID: 16244 RVA: 0x0012C790 File Offset: 0x0012A990
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

		// Token: 0x06003F75 RID: 16245 RVA: 0x0012C838 File Offset: 0x0012AA38
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

		// Token: 0x06003F76 RID: 16246 RVA: 0x0012C870 File Offset: 0x0012AA70
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

		// Token: 0x06003F77 RID: 16247 RVA: 0x0012C8E8 File Offset: 0x0012AAE8
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

		// Token: 0x06003F78 RID: 16248 RVA: 0x0012C9C8 File Offset: 0x0012ABC8
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

		// Token: 0x06003F79 RID: 16249 RVA: 0x0012CA78 File Offset: 0x0012AC78
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

		// Token: 0x06003F7A RID: 16250 RVA: 0x0012CB90 File Offset: 0x0012AD90
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

		// Token: 0x06003F7B RID: 16251 RVA: 0x0012CBF4 File Offset: 0x0012ADF4
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

		// Token: 0x0400407E RID: 16510
		[OnEnterPlay_SetNull]
		private static volatile CustomMapSerializer instance;

		// Token: 0x0400407F RID: 16511
		[OnEnterPlay_Set(false)]
		private static bool hasInstance;

		// Token: 0x04004080 RID: 16512
		private static Dictionary<string, Dictionary<byte, CustomMapTrigger>> registeredTriggersPerScene = new Dictionary<string, Dictionary<byte, CustomMapTrigger>>();

		// Token: 0x04004081 RID: 16513
		private static List<byte> triggerHistory = new List<byte>();

		// Token: 0x04004082 RID: 16514
		private static Dictionary<byte, byte> triggerCounts = new Dictionary<byte, byte>();

		// Token: 0x04004083 RID: 16515
		private static bool waitingForTriggerHistory;

		// Token: 0x04004084 RID: 16516
		private static List<string> scenesWaitingForTriggerHistory = new List<string>();

		// Token: 0x04004085 RID: 16517
		private static bool waitingForTriggerCounts;

		// Token: 0x04004086 RID: 16518
		private static List<string> scenesWaitingForTriggerCounts = new List<string>();

		// Token: 0x04004087 RID: 16519
		private static CallLimiter ActivateTriggerCallLimiter = new CallLimiter(50, 1f, 0.5f);
	}
}
