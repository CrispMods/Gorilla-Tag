using System;
using System.Collections.Generic;
using GorillaTagScripts.ModIO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace GorillaTagScripts.CustomMapSupport
{
	// Token: 0x02000A04 RID: 2564
	internal class CMSSerializer : GorillaSerializer
	{
		// Token: 0x0600400A RID: 16394 RVA: 0x00059DC5 File Offset: 0x00057FC5
		public void Awake()
		{
			if (CMSSerializer.instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			CMSSerializer.instance = this;
			CMSSerializer.hasInstance = true;
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x00059DEA File Offset: 0x00057FEA
		public void OnEnable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
			CustomMapManager.OnMapLoadComplete.AddListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x00059E18 File Offset: 0x00058018
		public void OnDisable()
		{
			CustomMapManager.OnMapLoadComplete.RemoveListener(new UnityAction<bool>(this.OnCustomMapLoaded));
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x00059E30 File Offset: 0x00058030
		private void OnCustomMapLoaded(bool success)
		{
			if (success)
			{
				CMSSerializer.RequestSyncTriggerHistory();
			}
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x00059E3A File Offset: 0x0005803A
		public static void ResetSyncedMapObjects()
		{
			CMSSerializer.triggerHistory.Clear();
			CMSSerializer.triggerCounts.Clear();
			CMSSerializer.registeredTriggersPerScene.Clear();
			CMSSerializer.waitingForTriggerHistory = false;
			CMSSerializer.waitingForTriggerCounts = false;
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0016B6C4 File Offset: 0x001698C4
		public static void RegisterTrigger(string sceneName, CMSTrigger trigger)
		{
			Dictionary<byte, CMSTrigger> dictionary;
			if (CMSSerializer.registeredTriggersPerScene.TryGetValue(sceneName, out dictionary))
			{
				if (!dictionary.ContainsKey(trigger.GetID()))
				{
					dictionary.Add(trigger.GetID(), trigger);
					return;
				}
			}
			else
			{
				CMSSerializer.registeredTriggersPerScene.Add(sceneName, new Dictionary<byte, CMSTrigger>
				{
					{
						trigger.GetID(),
						trigger
					}
				});
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0016B71C File Offset: 0x0016991C
		private static bool TryGetRegisteredTrigger(byte triggerID, out CMSTrigger trigger)
		{
			trigger = null;
			foreach (KeyValuePair<string, Dictionary<byte, CMSTrigger>> keyValuePair in CMSSerializer.registeredTriggersPerScene)
			{
				if (keyValuePair.Value.TryGetValue(triggerID, out trigger))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x00059E66 File Offset: 0x00058066
		public static void UnregisterTriggers(string forScene)
		{
			CMSSerializer.registeredTriggersPerScene.Remove(forScene);
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x00059E74 File Offset: 0x00058074
		public static void ResetTrigger(byte triggerID)
		{
			CMSSerializer.triggerCounts.Remove(triggerID);
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0016B784 File Offset: 0x00169984
		private static void RequestSyncTriggerHistory()
		{
			if (!CMSSerializer.hasInstance || !NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
			{
				return;
			}
			CMSSerializer.waitingForTriggerHistory = true;
			CMSSerializer.waitingForTriggerCounts = true;
			CMSSerializer.instance.SendRPC("RequestSyncTriggerHistory_RPC", false, Array.Empty<object>());
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0016B7D4 File Offset: 0x001699D4
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
			byte[] array = CMSSerializer.triggerHistory.ToArray();
			base.SendRPC("SyncTriggerHistory_RPC", info.Sender, new object[]
			{
				array
			});
			base.SendRPC("SyncTriggerCounts_RPC", info.Sender, new object[]
			{
				CMSSerializer.triggerCounts
			});
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0016B86C File Offset: 0x00169A6C
		[PunRPC]
		private void SyncTriggerHistory_RPC(byte[] syncedTriggerHistory, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "SyncTriggerHistory_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
			if (player.CheckSingleCallRPC(NetPlayer.SingleCallRPC.CMS_SyncTriggerHistory))
			{
				return;
			}
			player.ReceivedSingleCallRPC(NetPlayer.SingleCallRPC.CMS_SyncTriggerHistory);
			if (!CMSSerializer.waitingForTriggerHistory)
			{
				return;
			}
			CMSSerializer.triggerHistory.Clear();
			if (!syncedTriggerHistory.IsNullOrEmpty<byte>())
			{
				CMSSerializer.triggerHistory.AddRange(syncedTriggerHistory);
			}
			CMSSerializer.waitingForTriggerHistory = false;
			foreach (string forScene in CMSSerializer.scenesWaitingForTriggerHistory)
			{
				CMSSerializer.ProcessTriggerHistory(forScene);
			}
			CMSSerializer.scenesWaitingForTriggerHistory.Clear();
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0016B938 File Offset: 0x00169B38
		[PunRPC]
		private void SyncTriggerCounts_RPC(Dictionary<byte, byte> syncedTriggerCounts, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "SyncTriggerCounts_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
			if (player.CheckSingleCallRPC(NetPlayer.SingleCallRPC.CMS_SyncTriggerCounts))
			{
				return;
			}
			player.ReceivedSingleCallRPC(NetPlayer.SingleCallRPC.CMS_SyncTriggerCounts);
			if (!CMSSerializer.waitingForTriggerCounts)
			{
				return;
			}
			CMSSerializer.triggerCounts.Clear();
			if (syncedTriggerCounts != null && syncedTriggerCounts.Count > 0)
			{
				CMSSerializer.triggerCounts = syncedTriggerCounts;
			}
			CMSSerializer.waitingForTriggerCounts = false;
			foreach (string forScene in CMSSerializer.scenesWaitingForTriggerCounts)
			{
				CMSSerializer.ProcessTriggerCounts(forScene);
			}
			CMSSerializer.scenesWaitingForTriggerCounts.Clear();
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00059E82 File Offset: 0x00058082
		public static void ProcessSceneLoad(string sceneName)
		{
			if (CMSSerializer.waitingForTriggerHistory)
			{
				CMSSerializer.scenesWaitingForTriggerHistory.Add(sceneName);
			}
			else
			{
				CMSSerializer.ProcessTriggerHistory(sceneName);
			}
			if (CMSSerializer.waitingForTriggerCounts)
			{
				CMSSerializer.scenesWaitingForTriggerCounts.Add(sceneName);
				return;
			}
			CMSSerializer.ProcessTriggerCounts(sceneName);
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0016BA04 File Offset: 0x00169C04
		private static void ProcessTriggerHistory(string forScene)
		{
			Dictionary<byte, CMSTrigger> dictionary;
			if (CMSSerializer.registeredTriggersPerScene.TryGetValue(forScene, out dictionary))
			{
				foreach (byte key in CMSSerializer.triggerHistory)
				{
					CMSTrigger cmstrigger;
					if (dictionary.TryGetValue(key, out cmstrigger))
					{
						cmstrigger.Trigger(1.0, false, true);
					}
				}
			}
			UnityEvent<string> onTriggerHistoryProcessedForScene = CMSSerializer.OnTriggerHistoryProcessedForScene;
			if (onTriggerHistoryProcessedForScene == null)
			{
				return;
			}
			onTriggerHistoryProcessedForScene.Invoke(forScene);
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x0016BA8C File Offset: 0x00169C8C
		private static void ProcessTriggerCounts(string forScene)
		{
			Dictionary<byte, CMSTrigger> dictionary;
			if (CMSSerializer.registeredTriggersPerScene.TryGetValue(forScene, out dictionary))
			{
				List<byte> list = new List<byte>();
				foreach (KeyValuePair<byte, byte> keyValuePair in CMSSerializer.triggerCounts)
				{
					CMSTrigger cmstrigger;
					if (dictionary.TryGetValue(keyValuePair.Key, out cmstrigger))
					{
						if (cmstrigger.numAllowedTriggers > 0)
						{
							cmstrigger.SetTriggerCount(keyValuePair.Value);
						}
						else
						{
							list.Add(keyValuePair.Key);
						}
					}
				}
				foreach (byte key in list)
				{
					CMSSerializer.triggerCounts.Remove(key);
				}
			}
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x0016BB6C File Offset: 0x00169D6C
		public static void RequestTrigger(byte triggerID)
		{
			if (!CMSSerializer.hasInstance)
			{
				return;
			}
			if (!NetworkSystem.Instance.InRoom || NetworkSystem.Instance.IsMasterClient)
			{
				double triggerTime = (double)Time.time;
				if (NetworkSystem.Instance.InRoom)
				{
					triggerTime = PhotonNetwork.Time;
					CMSSerializer.instance.SendRPC("ActivateTrigger_RPC", true, new object[]
					{
						triggerID,
						NetworkSystem.Instance.LocalPlayer.ActorNumber
					});
				}
				CMSSerializer.instance.ActivateTrigger(triggerID, triggerTime, true);
				return;
			}
			CMSSerializer.instance.SendRPC("RequestTrigger_RPC", false, new object[]
			{
				triggerID
			});
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x0016BC1C File Offset: 0x00169E1C
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
			CMSTrigger cmstrigger;
			if (CMSSerializer.TryGetRegisteredTrigger(triggerID, out cmstrigger))
			{
				if (!cmstrigger.CanTrigger())
				{
					return;
				}
				Vector3 position = cmstrigger.gameObject.transform.position;
				RigContainer rigContainer2;
				if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer2))
				{
					return;
				}
				if ((rigContainer2.Rig.bodyTransform.position - position).sqrMagnitude > cmstrigger.validationDistanceSquared)
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

		// Token: 0x0600401C RID: 16412 RVA: 0x0016BD34 File Offset: 0x00169F34
		[PunRPC]
		private void ActivateTrigger_RPC(byte triggerID, int originatingPlayer, PhotonMessageInfo info)
		{
			GorillaNot.IncrementRPCCall(info, "ActivateTrigger_RPC");
			if (!NetworkSystem.Instance.InRoom || !info.Sender.IsMasterClient)
			{
				return;
			}
			if (info.SentServerTime < 0.0 || info.SentServerTime > 4294967.295)
			{
				return;
			}
			double num = (double)PhotonNetwork.GetPing() / 1000.0;
			if (!Utils.ValidateServerTime(info.SentServerTime, Math.Max(10.0, num * 2.0)))
			{
				return;
			}
			if (!CMSSerializer.ActivateTriggerCallLimiter.CheckCallTime(Time.unscaledTime))
			{
				return;
			}
			this.ActivateTrigger(triggerID, info.SentServerTime, NetworkSystem.Instance.LocalPlayer.ActorNumber == originatingPlayer);
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x0016BDF8 File Offset: 0x00169FF8
		private void ActivateTrigger(byte triggerID, double triggerTime = -1.0, bool originatedLocally = false)
		{
			CMSTrigger cmstrigger;
			bool flag = CMSSerializer.TryGetRegisteredTrigger(triggerID, out cmstrigger);
			if (!double.IsFinite(triggerTime))
			{
				triggerTime = -1.0;
			}
			byte b;
			bool flag2 = CMSSerializer.triggerCounts.TryGetValue(triggerID, out b);
			bool flag3 = !flag || cmstrigger.numAllowedTriggers > 0;
			if (flag2)
			{
				CMSSerializer.triggerCounts[triggerID] = ((b == byte.MaxValue) ? byte.MaxValue : (b += 1));
			}
			else if (flag3)
			{
				CMSSerializer.triggerCounts.Add(triggerID, 1);
			}
			CMSSerializer.triggerHistory.Remove(triggerID);
			CMSSerializer.triggerHistory.Add(triggerID);
			if (flag)
			{
				cmstrigger.Trigger(triggerTime, originatedLocally, false);
			}
		}

		// Token: 0x04004106 RID: 16646
		[OnEnterPlay_SetNull]
		private static volatile CMSSerializer instance;

		// Token: 0x04004107 RID: 16647
		[OnEnterPlay_Set(false)]
		private static bool hasInstance;

		// Token: 0x04004108 RID: 16648
		private static Dictionary<string, Dictionary<byte, CMSTrigger>> registeredTriggersPerScene = new Dictionary<string, Dictionary<byte, CMSTrigger>>();

		// Token: 0x04004109 RID: 16649
		private static List<byte> triggerHistory = new List<byte>();

		// Token: 0x0400410A RID: 16650
		private static Dictionary<byte, byte> triggerCounts = new Dictionary<byte, byte>();

		// Token: 0x0400410B RID: 16651
		private static bool waitingForTriggerHistory;

		// Token: 0x0400410C RID: 16652
		private static List<string> scenesWaitingForTriggerHistory = new List<string>();

		// Token: 0x0400410D RID: 16653
		private static bool waitingForTriggerCounts;

		// Token: 0x0400410E RID: 16654
		private static List<string> scenesWaitingForTriggerCounts = new List<string>();

		// Token: 0x0400410F RID: 16655
		private static CallLimiter ActivateTriggerCallLimiter = new CallLimiter(50, 1f, 0.5f);

		// Token: 0x04004110 RID: 16656
		public static UnityEvent<string> OnTriggerHistoryProcessedForScene = new UnityEvent<string>();
	}
}
