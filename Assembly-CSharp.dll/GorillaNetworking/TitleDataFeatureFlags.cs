using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AD1 RID: 2769
	public class TitleDataFeatureFlags
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x0005C566 File Offset: 0x0005A766
		// (set) Token: 0x06004540 RID: 17728 RVA: 0x0005C56E File Offset: 0x0005A76E
		public bool ready { get; private set; }

		// Token: 0x06004541 RID: 17729 RVA: 0x0005C577 File Offset: 0x0005A777
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

		// Token: 0x06004542 RID: 17730 RVA: 0x0017F43C File Offset: 0x0017D63C
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
			int num2 = new System.Random(entityId.GetHashCode()).Next(0, 100);
			Debug.Log(string.Format("GorillaServer: Partial rollout, seed = {0} flag value = {1}", num2, num2 <= num));
			return num2 <= num;
		}

		// Token: 0x040046B9 RID: 18105
		public string TitleDataKey = "DeployFeatureFlags";

		// Token: 0x040046BB RID: 18107
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

		// Token: 0x040046BC RID: 18108
		private Dictionary<string, int> flagValueByName = new Dictionary<string, int>();

		// Token: 0x040046BD RID: 18109
		private Dictionary<string, List<string>> flagValueByUser = new Dictionary<string, List<string>>();
	}
}
