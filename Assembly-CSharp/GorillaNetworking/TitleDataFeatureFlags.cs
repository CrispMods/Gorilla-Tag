using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AFB RID: 2811
	public class TitleDataFeatureFlags
	{
		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x0005DF65 File Offset: 0x0005C165
		// (set) Token: 0x0600467C RID: 18044 RVA: 0x0005DF6D File Offset: 0x0005C16D
		public bool ready { get; private set; }

		// Token: 0x0600467D RID: 18045 RVA: 0x0005DF76 File Offset: 0x0005C176
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

		// Token: 0x0600467E RID: 18046 RVA: 0x00186320 File Offset: 0x00184520
		public bool IsEnabledForUser(string flagName)
		{
			string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			Debug.Log(string.Concat(new string[]
			{
				"GorillaServer: Checking flag ",
				flagName,
				" for ",
				playFabPlayerId,
				"\nFlag values:\n",
				JsonConvert.SerializeObject(this.flagValueByName),
				"\n\nDefaults:\n",
				JsonConvert.SerializeObject(this.defaults)
			}));
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
			uint num2 = XXHash32.Compute(Encoding.UTF8.GetBytes(playFabPlayerId), 0U) % 100U;
			Debug.Log(string.Format("GorillaServer: Partial rollout, seed = {0} flag value = {1}", num2, (ulong)num2 < (ulong)((long)num)));
			return (ulong)num2 < (ulong)((long)num);
		}

		// Token: 0x0400479E RID: 18334
		public string TitleDataKey = "DeployFeatureFlags";

		// Token: 0x040047A0 RID: 18336
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
			},
			{
				"2025-04-CosmeticsAuthenticationV2-SetData",
				false
			},
			{
				"2025-04-CosmeticsAuthenticationV2-ReadData",
				false
			},
			{
				"2025-04-CosmeticsAuthenticationV2-Compat",
				true
			}
		};

		// Token: 0x040047A1 RID: 18337
		private Dictionary<string, int> flagValueByName = new Dictionary<string, int>();

		// Token: 0x040047A2 RID: 18338
		private Dictionary<string, List<string>> flagValueByUser = new Dictionary<string, List<string>>();
	}
}
