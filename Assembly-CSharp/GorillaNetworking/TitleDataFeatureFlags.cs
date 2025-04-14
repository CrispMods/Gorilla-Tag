using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ACE RID: 2766
	public class TitleDataFeatureFlags
	{
		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x00148B0C File Offset: 0x00146D0C
		// (set) Token: 0x06004534 RID: 17716 RVA: 0x00148B14 File Offset: 0x00146D14
		public bool ready { get; private set; }

		// Token: 0x06004535 RID: 17717 RVA: 0x00148B1D File Offset: 0x00146D1D
		public void FetchFeatureFlags()
		{
			PlayFabTitleDataCache.Instance.GetTitleData(this.TitleDataKey, delegate(string json)
			{
				FeatureFlagListData featureFlagListData = JsonUtility.FromJson<FeatureFlagListData>(json);
				foreach (FeatureFlagData featureFlagData in featureFlagListData.flags)
				{
					if (featureFlagData.valueType == "percent")
					{
						this.flagValueByName.AddOrUpdate(featureFlagData.name, featureFlagData.value);
					}
					List<string> alwaysOnForUsers = featureFlagData.alwaysOnForUsers;
					if (alwaysOnForUsers != null && alwaysOnForUsers.Count > 0)
					{
						this.flagValueByUser.AddOrUpdate(featureFlagData.name, featureFlagData.alwaysOnForUsers);
					}
				}
				Debug.Log(string.Format("GorillaServer: Fetched flags ({0})", featureFlagListData));
				this.ready = true;
				if (PlayFabAuthenticator.instance.postAuthSetSafety && GorillaServer.Instance.UseKID())
				{
					PlayFabAuthenticator.instance.SetSafety(true, true, false);
				}
			}, delegate(PlayFabError e)
			{
				Debug.LogError("Error fetching rollout feature flags: " + e.ErrorMessage);
				this.ready = true;
			});
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00148B48 File Offset: 0x00146D48
		public bool IsEnabledForUser(string flagName, string entityId)
		{
			Debug.Log(string.Concat(new string[]
			{
				"GorillaServer: Checking flag ",
				flagName,
				" for ",
				entityId,
				"\nFlag values:\n",
				JsonConvert.SerializeObject(this.flagValueByName),
				"\n\nDefaults:\n",
				JsonConvert.SerializeObject(this.defaults)
			}));
			string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			List<string> list;
			if (this.flagValueByUser.TryGetValue(flagName, out list) && list != null && list.Contains(playFabPlayerId))
			{
				return true;
			}
			int num;
			if (!this.flagValueByName.TryGetValue(flagName, out num))
			{
				Debug.Log("GorillaServer: Returning default");
				bool flag;
				return this.defaults.TryGetValue(flagName, out flag) && flag;
			}
			Debug.Log(string.Format("GorillaServer: Rollout % is {0}", num));
			if (num <= 0)
			{
				Debug.Log("GorillaServer: " + flagName + " is off (<=0%).");
				return false;
			}
			if (num >= 100)
			{
				Debug.Log("GorillaServer: " + flagName + " is on (>=100%).");
				return true;
			}
			int num2 = new Random(entityId.GetHashCode()).Next(0, 100);
			Debug.Log(string.Format("GorillaServer: Partial rollout, seed = {0} flag value = {1}", num2, num2 <= num));
			return num2 <= num;
		}

		// Token: 0x040046A7 RID: 18087
		public string TitleDataKey = "DeployFeatureFlags";

		// Token: 0x040046A9 RID: 18089
		public Dictionary<string, bool> defaults = new Dictionary<string, bool>
		{
			{
				"2024-05-ReturnCurrentVersionV2",
				true
			},
			{
				"2024-05-ReturnMyOculusHashV2",
				true
			},
			{
				"2024-05-TryDistributeCurrencyV2",
				true
			},
			{
				"2024-05-AddOrRemoveDLCOwnershipV2",
				true
			},
			{
				"2024-05-BroadcastMyRoomV2",
				true
			},
			{
				"2024-06-CosmeticsAuthenticationV2",
				true
			},
			{
				"2024-08-KIDIntegrationV1",
				true
			}
		};

		// Token: 0x040046AA RID: 18090
		private Dictionary<string, int> flagValueByName = new Dictionary<string, int>();

		// Token: 0x040046AB RID: 18091
		private Dictionary<string, List<string>> flagValueByUser = new Dictionary<string, List<string>>();
	}
}
