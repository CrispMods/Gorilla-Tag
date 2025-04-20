using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AEF RID: 2799
	public class GorillaServer : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x0005DDA4 File Offset: 0x0005BFA4
		public bool FeatureFlagsReady
		{
			get
			{
				return this.featureFlags.ready;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x0005DDB1 File Offset: 0x0005BFB1
		private PlayFab.CloudScriptModels.EntityKey playerEntity
		{
			get
			{
				return new PlayFab.CloudScriptModels.EntityKey
				{
					Id = PlayFabSettings.staticPlayer.EntityId,
					Type = PlayFabSettings.staticPlayer.EntityType
				};
			}
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0005DDD8 File Offset: 0x0005BFD8
		public void Start()
		{
			this.featureFlags.FetchFeatureFlags();
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0005DDE5 File Offset: 0x0005BFE5
		private void Awake()
		{
			if (GorillaServer.Instance == null)
			{
				GorillaServer.Instance = this;
				return;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x001856CC File Offset: 0x001838CC
		public void ReturnCurrentVersion(ReturnCurrentVersionRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "ReturnCurrentVersion result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "ReturnCurrentVersion error");
			if (this.featureFlags.IsEnabledForUser("2024-05-ReturnCurrentVersionV2"))
			{
				Debug.Log("GorillaServer: ReturnCurrentVersion V2 call");
				PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
				{
					Entity = this.playerEntity,
					FunctionName = "ReturnCurrentVersionV2",
					FunctionParameter = request
				}, successCallback, errorCallback, null, null);
				return;
			}
			Debug.Log("GorillaServer: ReturnCurrentVersion LEGACY call");
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "ReturnCurrentVersionNew",
				FunctionParameter = request
			}, delegate(PlayFab.ClientModels.ExecuteCloudScriptResult result)
			{
				successCallback(this.toFunctionResult(result));
			}, errorCallback, null, null);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x00185798 File Offset: 0x00183998
		public void ReturnMyOculusHash(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "ReturnMyOculusHash result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "ReturnMyOculusHash error");
			if (this.featureFlags.IsEnabledForUser("2024-05-ReturnMyOculusHashV2"))
			{
				Debug.Log("GorillaServer: ReturnMyOculusHash V2 call");
				PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
				{
					Entity = this.playerEntity,
					FunctionName = "ReturnMyOculusHashV2",
					FunctionParameter = new
					{

					}
				}, successCallback, errorCallback, null, null);
				return;
			}
			Debug.Log("GorillaServer: ReturnMyOculusHash LEGACY call");
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "ReturnMyOculusHash"
			}, delegate(PlayFab.ClientModels.ExecuteCloudScriptResult result)
			{
				successCallback(this.toFunctionResult(result));
			}, errorCallback, null, null);
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x00185860 File Offset: 0x00183A60
		public void TryDistributeCurrency(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "TryDistributeCurrency result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "TryDistributeCurrency error");
			if (this.featureFlags.IsEnabledForUser("2024-05-TryDistributeCurrencyV2"))
			{
				Debug.Log("GorillaServer: TryDistributeCurrency V2 call");
				PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
				{
					Entity = this.playerEntity,
					FunctionName = "TryDistributeCurrencyV2",
					FunctionParameter = new
					{

					}
				}, successCallback, errorCallback, null, null);
				return;
			}
			Debug.Log("GorillaServer: TryDistributeCurrency LEGACY call");
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "TryDistributeCurrency",
				FunctionParameter = new
				{

				}
			}, delegate(PlayFab.ClientModels.ExecuteCloudScriptResult result)
			{
				successCallback(this.toFunctionResult(result));
			}, errorCallback, null, null);
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x00185934 File Offset: 0x00183B34
		public void AddOrRemoveDLCOwnership(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "AddOrRemoveDLCOwnership result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "AddOrRemoveDLCOwnership error");
			if (this.featureFlags.IsEnabledForUser("2024-05-AddOrRemoveDLCOwnershipV2"))
			{
				Debug.Log("GorillaServer: AddOrRemoveDLCOwnership V2 call");
				PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
				{
					Entity = this.playerEntity,
					FunctionName = "AddOrRemoveDLCOwnershipV2",
					FunctionParameter = new
					{

					}
				}, successCallback, errorCallback, null, null);
				return;
			}
			Debug.Log("GorillaServer: AddOrRemoveDLCOwnership LEGACY call");
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "AddOrRemoveDLCOwnership",
				FunctionParameter = new
				{

				}
			}, delegate(PlayFab.ClientModels.ExecuteCloudScriptResult result)
			{
				successCallback(this.toFunctionResult(result));
			}, errorCallback, null, null);
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00185A08 File Offset: 0x00183C08
		public void BroadcastMyRoom(BroadcastMyRoomRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "BroadcastMyRoom result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "BroadcastMyRoom error");
			if (this.featureFlags.IsEnabledForUser("2024-05-BroadcastMyRoomV2"))
			{
				Debug.Log(string.Format("GorillaServer: BroadcastMyRoom V2 call ({0})", request));
				PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
				{
					Entity = this.playerEntity,
					FunctionName = "BroadcastMyRoomV2",
					FunctionParameter = request
				}, successCallback, errorCallback, null, null);
				return;
			}
			Debug.Log(string.Format("GorillaServer: BroadcastMyRoom LEGACY call ({0})", request));
			PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
			{
				FunctionName = "BroadcastMyRoom",
				FunctionParameter = request
			}, delegate(PlayFab.ClientModels.ExecuteCloudScriptResult result)
			{
				successCallback(this.toFunctionResult(result));
			}, errorCallback, null, null);
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x0005DE05 File Offset: 0x0005C005
		public bool NewCosmeticsPath()
		{
			return this.featureFlags.IsEnabledForUser("2024-06-CosmeticsAuthenticationV2");
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x0005DE17 File Offset: 0x0005C017
		public bool NewCosmeticsPathShouldSetSharedGroupData()
		{
			return this.featureFlags.IsEnabledForUser("2025-04-CosmeticsAuthenticationV2-SetData");
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x0005DE29 File Offset: 0x0005C029
		public bool NewCosmeticsPathShouldReadSharedGroupData()
		{
			return this.featureFlags.IsEnabledForUser("2025-04-CosmeticsAuthenticationV2-ReadData");
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x0005DE3B File Offset: 0x0005C03B
		public bool NewCosmeticsPathShouldSetRoomData()
		{
			return this.featureFlags.IsEnabledForUser("2025-04-CosmeticsAuthenticationV2-Compat");
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00185AE0 File Offset: 0x00183CE0
		public void UpdateUserCosmetics()
		{
			ExecuteFunctionRequest executeFunctionRequest = new ExecuteFunctionRequest();
			executeFunctionRequest.Entity = this.playerEntity;
			executeFunctionRequest.FunctionName = "UpdatePersonalCosmeticsList";
			executeFunctionRequest.FunctionParameter = new
			{

			};
			executeFunctionRequest.GeneratePlayStreamEvent = new bool?(false);
			PlayFabCloudScriptAPI.ExecuteFunction(executeFunctionRequest, delegate(ExecuteFunctionResult result)
			{
				if (CosmeticsController.instance != null)
				{
					CosmeticsController.instance.CheckCosmeticsSharedGroup();
				}
			}, delegate(PlayFabError error)
			{
			}, null, null);
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00185B68 File Offset: 0x00183D68
		public void GetAcceptedAgreements(GetAcceptedAgreementsRequest request, Action<Dictionary<string, string>> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<Dictionary<string, string>>(successCallback, "GetAcceptedAgreements result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "GetAcceptedAgreements json error");
			Debug.Log(string.Format("GorillaServer: GetAcceptedAgreements call ({0})", request));
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "GetAcceptedAgreements",
				FunctionParameter = string.Join(",", request.AgreementKeys),
				GeneratePlayStreamEvent = new bool?(false)
			}, delegate(ExecuteFunctionResult result)
			{
				try
				{
					string value = Convert.ToString(result.FunctionResult);
					successCallback(JsonConvert.DeserializeObject<Dictionary<string, string>>(value));
				}
				catch (Exception arg)
				{
					errorCallback(new PlayFabError
					{
						ErrorMessage = string.Format("Invalid format for GetAcceptedAgreements ({0})", arg),
						Error = PlayFabErrorCode.JsonParseError
					});
				}
			}, errorCallback, null, null);
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00185C20 File Offset: 0x00183E20
		public void SubmitAcceptedAgreements(SubmitAcceptedAgreementsRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "SubmitAcceptedAgreements result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "SubmitAcceptedAgreements error");
			Debug.Log(string.Format("GorillaServer: SubmitAcceptedAgreements call ({0})", request));
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "SubmitAcceptedAgreements",
				FunctionParameter = request.Agreements,
				GeneratePlayStreamEvent = new bool?(false)
			}, successCallback, errorCallback, null, null);
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00185C98 File Offset: 0x00183E98
		public void GetUserAge(Action<int> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<int>(successCallback, "GetUserAge result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "GetUserAge json error");
			Debug.Log("GorillaServer: GetUserAge call");
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "GetUserAge",
				GeneratePlayStreamEvent = new bool?(false)
			}, delegate(ExecuteFunctionResult result)
			{
				try
				{
					string value = Convert.ToString(result.FunctionResult);
					successCallback(JsonConvert.DeserializeObject<int>(value));
				}
				catch (Exception arg)
				{
					errorCallback(new PlayFabError
					{
						ErrorMessage = string.Format("Invalid format for GetAcceptedAgreements ({0})", arg),
						Error = PlayFabErrorCode.JsonParseError
					});
				}
			}, errorCallback, null, null);
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x00185D34 File Offset: 0x00183F34
		public void SubmitUserAge(SubmitUserAgeRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "SubmitUserAge result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "SubmitUserAge error");
			Debug.Log(string.Format("GorillaServer: SubmitUserAge call ({0})", request));
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "SubmitUserAge",
				FunctionParameter = request.UserAge,
				GeneratePlayStreamEvent = new bool?(false)
			}, successCallback, errorCallback, null, null);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00185DAC File Offset: 0x00183FAC
		public void UploadGorillanalytics(object uploadData)
		{
			Debug.Log(string.Format("GorillaServer: UploadGorillanalytics call ({0})", uploadData));
			ExecuteFunctionRequest executeFunctionRequest = new ExecuteFunctionRequest();
			executeFunctionRequest.Entity = this.playerEntity;
			executeFunctionRequest.FunctionName = "Gorillanalytics";
			executeFunctionRequest.FunctionParameter = uploadData;
			executeFunctionRequest.GeneratePlayStreamEvent = new bool?(false);
			PlayFabCloudScriptAPI.ExecuteFunction(executeFunctionRequest, delegate(ExecuteFunctionResult result)
			{
				Debug.Log(string.Format("The {0} function took {1} to complete", result.FunctionName, result.ExecutionTimeMilliseconds));
			}, delegate(PlayFabError error)
			{
				Debug.Log("Error uploading Gorillanalytics: " + error.GenerateErrorReport());
			}, null, null);
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00185E40 File Offset: 0x00184040
		public void CheckForBadName(CheckForBadNameRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "CheckForBadName result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "CheckForBadName error");
			Debug.Log(string.Format("GorillaServer: CheckForBadName call ({0})", request));
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "CheckForBadName",
				FunctionParameter = new
				{
					name = request.name,
					forRoom = request.forRoom.ToString()
				},
				GeneratePlayStreamEvent = new bool?(false)
			}, successCallback, errorCallback, null, null);
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00185EC8 File Offset: 0x001840C8
		public void GetRandomName(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "GetRandomName result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "GetRandomName error");
			Debug.Log("GorillaServer: GetRandomName call");
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "GetRandomName",
				GeneratePlayStreamEvent = new bool?(false)
			}, successCallback, errorCallback, null, null);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00185F2C File Offset: 0x0018412C
		public void ReturnQueueStats(ReturnQueueStatsRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "ReturnQueueStats result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "ReturnQueueStats error");
			Debug.Log("GorillaServer: ReturnQueueStats call");
			PlayFabCloudScriptAPI.ExecuteFunction(new ExecuteFunctionRequest
			{
				Entity = this.playerEntity,
				FunctionName = "ReturnQueueStats",
				FunctionParameter = new
				{
					QueueName = request.queueName
				},
				GeneratePlayStreamEvent = new bool?(false)
			}, successCallback, errorCallback, null, null);
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0005DE4D File Offset: 0x0005C04D
		public bool UseKID()
		{
			return this.featureFlags.IsEnabledForUser("2024-08-KIDIntegrationV1");
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x0005DE5F File Offset: 0x0005C05F
		private Action<T> DebugWrapCb<T>(Action<T> cb, string label)
		{
			return delegate(T arg)
			{
				if (this.debug)
				{
					try
					{
						Debug.Log(string.Concat(new string[]
						{
							"GorillaServer: ",
							label,
							" (",
							JsonConvert.SerializeObject(arg, this.serializationSettings),
							")"
						}));
					}
					catch (Exception arg2)
					{
						Debug.LogError(string.Format("GorillaServer: {0} Error printing failure log: {1}", label, arg2));
					}
				}
				cb(arg);
			};
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00185FA4 File Offset: 0x001841A4
		private ExecuteFunctionResult toFunctionResult(PlayFab.ClientModels.ExecuteCloudScriptResult csResult)
		{
			FunctionExecutionError error = null;
			if (csResult.Error != null)
			{
				error = new FunctionExecutionError
				{
					Error = csResult.Error.Error,
					Message = csResult.Error.Message,
					StackTrace = csResult.Error.StackTrace
				};
			}
			return new ExecuteFunctionResult
			{
				CustomData = csResult.CustomData,
				Error = error,
				ExecutionTimeMilliseconds = Convert.ToInt32(Math.Round(csResult.ExecutionTimeSeconds * 1000.0)),
				FunctionName = csResult.FunctionName,
				FunctionResult = csResult.FunctionResult,
				FunctionResultTooLarge = csResult.FunctionResultTooLarge
			};
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00186050 File Offset: 0x00184250
		public void OnBeforeSerialize()
		{
			this.FeatureFlagsTitleDataKey = this.featureFlags.TitleDataKey;
			this.DefaultDeployFeatureFlagsEnabled.Clear();
			foreach (KeyValuePair<string, bool> keyValuePair in this.featureFlags.defaults)
			{
				if (keyValuePair.Value)
				{
					this.DefaultDeployFeatureFlagsEnabled.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x001860D8 File Offset: 0x001842D8
		public void OnAfterDeserialize()
		{
			this.featureFlags.TitleDataKey = this.FeatureFlagsTitleDataKey;
			foreach (string key in this.DefaultDeployFeatureFlagsEnabled)
			{
				this.featureFlags.defaults.AddOrUpdate(key, true);
			}
		}

		// Token: 0x0400477D RID: 18301
		public static volatile GorillaServer Instance;

		// Token: 0x0400477E RID: 18302
		public string FeatureFlagsTitleDataKey = "DeployFeatureFlags";

		// Token: 0x0400477F RID: 18303
		public List<string> DefaultDeployFeatureFlagsEnabled = new List<string>();

		// Token: 0x04004780 RID: 18304
		private TitleDataFeatureFlags featureFlags = new TitleDataFeatureFlags();

		// Token: 0x04004781 RID: 18305
		private bool debug;

		// Token: 0x04004782 RID: 18306
		private JsonSerializerSettings serializationSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			DefaultValueHandling = DefaultValueHandling.Ignore,
			MissingMemberHandling = MissingMemberHandling.Ignore,
			ObjectCreationHandling = ObjectCreationHandling.Replace,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			TypeNameHandling = TypeNameHandling.Auto
		};
	}
}
