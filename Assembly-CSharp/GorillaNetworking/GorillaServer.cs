using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AC2 RID: 2754
	public class GorillaServer : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06004502 RID: 17666 RVA: 0x00147CEB File Offset: 0x00145EEB
		public bool FeatureFlagsReady
		{
			get
			{
				return this.featureFlags.ready;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x00147CF8 File Offset: 0x00145EF8
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

		// Token: 0x06004504 RID: 17668 RVA: 0x00147D1F File Offset: 0x00145F1F
		public void Start()
		{
			this.featureFlags.FetchFeatureFlags();
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00147D2C File Offset: 0x00145F2C
		private void Awake()
		{
			if (GorillaServer.Instance == null)
			{
				GorillaServer.Instance = this;
				return;
			}
			Object.Destroy(this);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00147D4C File Offset: 0x00145F4C
		public void ReturnCurrentVersion(ReturnCurrentVersionRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "ReturnCurrentVersion result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "ReturnCurrentVersion error");
			if (this.featureFlags.IsEnabledForUser("2024-05-ReturnCurrentVersionV2", this.playerEntity.Id))
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

		// Token: 0x06004507 RID: 17671 RVA: 0x00147E20 File Offset: 0x00146020
		public void ReturnMyOculusHash(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "ReturnMyOculusHash result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "ReturnMyOculusHash error");
			if (this.featureFlags.IsEnabledForUser("2024-05-ReturnMyOculusHashV2", this.playerEntity.Id))
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

		// Token: 0x06004508 RID: 17672 RVA: 0x00147EF4 File Offset: 0x001460F4
		public void TryDistributeCurrency(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "TryDistributeCurrency result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "TryDistributeCurrency error");
			if (this.featureFlags.IsEnabledForUser("2024-05-TryDistributeCurrencyV2", this.playerEntity.Id))
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

		// Token: 0x06004509 RID: 17673 RVA: 0x00147FD0 File Offset: 0x001461D0
		public void AddOrRemoveDLCOwnership(Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "AddOrRemoveDLCOwnership result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "AddOrRemoveDLCOwnership error");
			if (this.featureFlags.IsEnabledForUser("2024-05-AddOrRemoveDLCOwnershipV2", this.playerEntity.Id))
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

		// Token: 0x0600450A RID: 17674 RVA: 0x001480AC File Offset: 0x001462AC
		public void BroadcastMyRoom(BroadcastMyRoomRequest request, Action<ExecuteFunctionResult> successCallback, Action<PlayFabError> errorCallback)
		{
			successCallback = this.DebugWrapCb<ExecuteFunctionResult>(successCallback, "BroadcastMyRoom result");
			errorCallback = this.DebugWrapCb<PlayFabError>(errorCallback, "BroadcastMyRoom error");
			if (this.featureFlags.IsEnabledForUser("2024-05-BroadcastMyRoomV2", this.playerEntity.Id))
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

		// Token: 0x0600450B RID: 17675 RVA: 0x0014818C File Offset: 0x0014638C
		public bool NewCosmeticsPath()
		{
			return this.featureFlags.IsEnabledForUser("2024-06-CosmeticsAuthenticationV2", this.playerEntity.Id);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x001481AC File Offset: 0x001463AC
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

		// Token: 0x0600450D RID: 17677 RVA: 0x00148234 File Offset: 0x00146434
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

		// Token: 0x0600450E RID: 17678 RVA: 0x001482EC File Offset: 0x001464EC
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

		// Token: 0x0600450F RID: 17679 RVA: 0x00148364 File Offset: 0x00146564
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

		// Token: 0x06004510 RID: 17680 RVA: 0x00148400 File Offset: 0x00146600
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

		// Token: 0x06004511 RID: 17681 RVA: 0x00148478 File Offset: 0x00146678
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

		// Token: 0x06004512 RID: 17682 RVA: 0x0014850C File Offset: 0x0014670C
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

		// Token: 0x06004513 RID: 17683 RVA: 0x00148594 File Offset: 0x00146794
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

		// Token: 0x06004514 RID: 17684 RVA: 0x001485F8 File Offset: 0x001467F8
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

		// Token: 0x06004515 RID: 17685 RVA: 0x0014866D File Offset: 0x0014686D
		public bool UseKID()
		{
			return this.featureFlags.IsEnabledForUser("2024-08-KIDIntegrationV1", this.playerEntity.Id);
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x0014868A File Offset: 0x0014688A
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

		// Token: 0x06004517 RID: 17687 RVA: 0x001486B4 File Offset: 0x001468B4
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

		// Token: 0x06004518 RID: 17688 RVA: 0x00148760 File Offset: 0x00146960
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

		// Token: 0x06004519 RID: 17689 RVA: 0x001487E8 File Offset: 0x001469E8
		public void OnAfterDeserialize()
		{
			this.featureFlags.TitleDataKey = this.FeatureFlagsTitleDataKey;
			foreach (string key in this.DefaultDeployFeatureFlagsEnabled)
			{
				this.featureFlags.defaults.AddOrUpdate(key, true);
			}
		}

		// Token: 0x04004686 RID: 18054
		public static volatile GorillaServer Instance;

		// Token: 0x04004687 RID: 18055
		public string FeatureFlagsTitleDataKey = "DeployFeatureFlags";

		// Token: 0x04004688 RID: 18056
		public List<string> DefaultDeployFeatureFlagsEnabled = new List<string>();

		// Token: 0x04004689 RID: 18057
		private TitleDataFeatureFlags featureFlags = new TitleDataFeatureFlags();

		// Token: 0x0400468A RID: 18058
		private bool debug;

		// Token: 0x0400468B RID: 18059
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
