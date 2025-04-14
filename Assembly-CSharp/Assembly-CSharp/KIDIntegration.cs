using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GorillaNetworking;
using KID.Client;
using KID.Model;
using UnityEngine;

// Token: 0x020006EA RID: 1770
public class KIDIntegration
{
	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002C05 RID: 11269 RVA: 0x000D89A0 File Offset: 0x000D6BA0
	public float ChallengeTimeoutRemaining
	{
		get
		{
			if (this._kIDChallenge == null)
			{
				Debug.LogError("[KID] Trying to access timeout remaining, but challenge is NULL");
				return 0f;
			}
			float num = (float)(this._timeoutTime - Time.timeAsDouble);
			if (num < 0f)
			{
				num = 0f;
			}
			return num;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000D89E2 File Offset: 0x000D6BE2
	// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000D89E9 File Offset: 0x000D6BE9
	public static AwaitChallengeResponse.StatusEnum ChallengeStatus { get; private set; } = AwaitChallengeResponse.StatusEnum.INPROGRESS;

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000D89F1 File Offset: 0x000D6BF1
	public static bool HasFoundSession
	{
		get
		{
			return KIDIntegration.CurrentSession != null;
		}
	}

	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06002C09 RID: 11273 RVA: 0x000D89FB File Offset: 0x000D6BFB
	// (set) Token: 0x06002C0A RID: 11274 RVA: 0x000D8A02 File Offset: 0x000D6C02
	public static bool IsReady { get; private set; }

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000D8A0A File Offset: 0x000D6C0A
	// (set) Token: 0x06002C0C RID: 11276 RVA: 0x000D8A11 File Offset: 0x000D6C11
	public static bool HasSetAge { get; private set; }

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06002C0D RID: 11277 RVA: 0x000D8A19 File Offset: 0x000D6C19
	public static string GetSessionIdPlayerPrefRef
	{
		get
		{
			if (string.IsNullOrEmpty(KIDIntegration.sessionIdPlayerPrefRef))
			{
				KIDIntegration.sessionIdPlayerPrefRef = "kIDSessionID-" + PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			}
			return KIDIntegration.sessionIdPlayerPrefRef;
		}
	}

	// Token: 0x170004AC RID: 1196
	// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000D8A47 File Offset: 0x000D6C47
	// (set) Token: 0x06002C0F RID: 11279 RVA: 0x000D8A4E File Offset: 0x000D6C4E
	private static Session CurrentSession
	{
		get
		{
			return KIDIntegration._kIDSession;
		}
		set
		{
			if (KIDIntegration._kIDSession == value)
			{
				return;
			}
			KIDIntegration._kIDSession = value;
		}
	}

	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002C10 RID: 11280 RVA: 0x000D8A64 File Offset: 0x000D6C64
	private static string GetChallengeIdPlayerPrefRef
	{
		get
		{
			if (string.IsNullOrEmpty(KIDIntegration.challengeIdPlayerPrefRef))
			{
				KIDIntegration.challengeIdPlayerPrefRef = "ageGateChallenge-" + PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			}
			return KIDIntegration.challengeIdPlayerPrefRef;
		}
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000D8A92 File Offset: 0x000D6C92
	public KIDIntegration(Action sessionUpdatedCallback, CancellationToken cancellationToken)
	{
		this._cancellationToken = cancellationToken;
		this._sessionUpdatedCallback = sessionUpdatedCallback;
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000D8AB4 File Offset: 0x000D6CB4
	public void InitialiseKIDSystem()
	{
		KIDIntegration.<InitialiseKIDSystem>d__53 <InitialiseKIDSystem>d__;
		<InitialiseKIDSystem>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<InitialiseKIDSystem>d__.<>4__this = this;
		<InitialiseKIDSystem>d__.<>1__state = -1;
		<InitialiseKIDSystem>d__.<>t__builder.Start<KIDIntegration.<InitialiseKIDSystem>d__53>(ref <InitialiseKIDSystem>d__);
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000D8AEC File Offset: 0x000D6CEC
	[return: TupleElementNames(new string[]
	{
		"updateResult",
		"newSession"
	})]
	private Task<ValueTuple<KIDIntegration.EKIDSessionUpdateType, Session>> TryGetSession(string etag = null)
	{
		KIDIntegration.<TryGetSession>d__54 <TryGetSession>d__;
		<TryGetSession>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<KIDIntegration.EKIDSessionUpdateType, Session>>.Create();
		<TryGetSession>d__.<>4__this = this;
		<TryGetSession>d__.etag = etag;
		<TryGetSession>d__.<>1__state = -1;
		<TryGetSession>d__.<>t__builder.Start<KIDIntegration.<TryGetSession>d__54>(ref <TryGetSession>d__);
		return <TryGetSession>d__.<>t__builder.Task;
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x000D8B38 File Offset: 0x000D6D38
	public static Task<R> AsyncKidRequestWithBackoff<R>(Func<Task<R>> webRequest, int timeoutMs = 10000, CancellationToken cancellationToken = default(CancellationToken))
	{
		KIDIntegration.<AsyncKidRequestWithBackoff>d__55<R> <AsyncKidRequestWithBackoff>d__;
		<AsyncKidRequestWithBackoff>d__.<>t__builder = AsyncTaskMethodBuilder<R>.Create();
		<AsyncKidRequestWithBackoff>d__.webRequest = webRequest;
		<AsyncKidRequestWithBackoff>d__.timeoutMs = timeoutMs;
		<AsyncKidRequestWithBackoff>d__.cancellationToken = cancellationToken;
		<AsyncKidRequestWithBackoff>d__.<>1__state = -1;
		<AsyncKidRequestWithBackoff>d__.<>t__builder.Start<KIDIntegration.<AsyncKidRequestWithBackoff>d__55<R>>(ref <AsyncKidRequestWithBackoff>d__);
		return <AsyncKidRequestWithBackoff>d__.<>t__builder.Task;
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x000D8B8C File Offset: 0x000D6D8C
	private static int GetRetryAfterMsOr(ApiException exception, int defaultMs)
	{
		if (exception.Headers == null || !exception.Headers.ContainsKey("Retry-After"))
		{
			return defaultMs;
		}
		IEnumerable<string> source = exception.Headers["Retry-After"];
		int num = 0;
		if (int.TryParse(source.Last<string>(), out num))
		{
			return num * 1000;
		}
		return defaultMs;
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x000D8BE0 File Offset: 0x000D6DE0
	private Task<ValueTuple<bool, Guid>> TryGetPastChallenge()
	{
		KIDIntegration.<TryGetPastChallenge>d__57 <TryGetPastChallenge>d__;
		<TryGetPastChallenge>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, Guid>>.Create();
		<TryGetPastChallenge>d__.<>4__this = this;
		<TryGetPastChallenge>d__.<>1__state = -1;
		<TryGetPastChallenge>d__.<>t__builder.Start<KIDIntegration.<TryGetPastChallenge>d__57>(ref <TryGetPastChallenge>d__);
		return <TryGetPastChallenge>d__.<>t__builder.Task;
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x000D8C24 File Offset: 0x000D6E24
	private Task TriggerChallenge(DateTime birthday)
	{
		KIDIntegration.<TriggerChallenge>d__58 <TriggerChallenge>d__;
		<TriggerChallenge>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<TriggerChallenge>d__.<>4__this = this;
		<TriggerChallenge>d__.birthday = birthday;
		<TriggerChallenge>d__.<>1__state = -1;
		<TriggerChallenge>d__.<>t__builder.Start<KIDIntegration.<TriggerChallenge>d__58>(ref <TriggerChallenge>d__);
		return <TriggerChallenge>d__.<>t__builder.Task;
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x000D8C70 File Offset: 0x000D6E70
	private Task WaitForChallengeResult(Guid challengeId)
	{
		KIDIntegration.<WaitForChallengeResult>d__59 <WaitForChallengeResult>d__;
		<WaitForChallengeResult>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForChallengeResult>d__.<>4__this = this;
		<WaitForChallengeResult>d__.challengeId = challengeId;
		<WaitForChallengeResult>d__.<>1__state = -1;
		<WaitForChallengeResult>d__.<>t__builder.Start<KIDIntegration.<WaitForChallengeResult>d__59>(ref <WaitForChallengeResult>d__);
		return <WaitForChallengeResult>d__.<>t__builder.Task;
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x000D8CBB File Offset: 0x000D6EBB
	public static void RegisterOnInitialisationComplete(Action onComplete)
	{
		KIDIntegration._onKIDInitialisationComplete = (Action)Delegate.Combine(KIDIntegration._onKIDInitialisationComplete, onComplete);
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x000D8CD2 File Offset: 0x000D6ED2
	public static void UnregisterOnInitialisationComplete(Action onComplete)
	{
		KIDIntegration._onKIDInitialisationComplete = (Action)Delegate.Remove(KIDIntegration._onKIDInitialisationComplete, onComplete);
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x000D8CEC File Offset: 0x000D6EEC
	public static Task LoadStoredPermissions()
	{
		KIDIntegration.<LoadStoredPermissions>d__62 <LoadStoredPermissions>d__;
		<LoadStoredPermissions>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadStoredPermissions>d__.<>1__state = -1;
		<LoadStoredPermissions>d__.<>t__builder.Start<KIDIntegration.<LoadStoredPermissions>d__62>(ref <LoadStoredPermissions>d__);
		return <LoadStoredPermissions>d__.<>t__builder.Task;
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x000D8D27 File Offset: 0x000D6F27
	public static string GetETAG()
	{
		return PlayerPrefs.GetString("kIDSessionETAG");
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x000D8D33 File Offset: 0x000D6F33
	public static List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration._permissionsList.Values.ToList<SKIDPermissionData>();
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x000D8D44 File Offset: 0x000D6F44
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public static ValueTuple<bool, Permission.ManagedByEnum> GetVoiceChatPermissionStatus()
	{
		SKIDPermissionData? permissionDataByName = KIDIntegration.GetPermissionDataByName("voice-chat");
		if (permissionDataByName == null)
		{
			Debug.Log("[KID] Failed to find Voice Chat Permission data with name [voice-chat]. Assuming FALSE");
			return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(permissionDataByName.Value.PermissionEnabled, permissionDataByName.Value.PermissionData.ManagedBy);
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000D8DA0 File Offset: 0x000D6FA0
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public static ValueTuple<bool, Permission.ManagedByEnum> GetCustomUserNamesPermissionStatus()
	{
		SKIDPermissionData? permissionDataByName = KIDIntegration.GetPermissionDataByName("custom-username");
		if (permissionDataByName == null)
		{
			Debug.Log("[KID] Failed to find Custom Usernames Permission data with name [custom-username]. Assuming FALSE");
			return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(permissionDataByName.Value.PermissionEnabled, permissionDataByName.Value.PermissionData.ManagedBy);
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000D8DFC File Offset: 0x000D6FFC
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public static ValueTuple<bool, Permission.ManagedByEnum> GetPrivateRoomPermissionStatus()
	{
		SKIDPermissionData? permissionDataByName = KIDIntegration.GetPermissionDataByName("join-groups");
		if (permissionDataByName == null)
		{
			Debug.Log("[KID] Failed to find Private Rooms Permission data with name [join-groups]. Assuming FALSE");
			return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(permissionDataByName.Value.PermissionEnabled, permissionDataByName.Value.PermissionData.ManagedBy);
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000D8E58 File Offset: 0x000D7058
	public void AgeGateVerificationFlow()
	{
		KIDIntegration.<AgeGateVerificationFlow>d__68 <AgeGateVerificationFlow>d__;
		<AgeGateVerificationFlow>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AgeGateVerificationFlow>d__.<>4__this = this;
		<AgeGateVerificationFlow>d__.<>1__state = -1;
		<AgeGateVerificationFlow>d__.<>t__builder.Start<KIDIntegration.<AgeGateVerificationFlow>d__68>(ref <AgeGateVerificationFlow>d__);
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x000D8E90 File Offset: 0x000D7090
	public Task StartKidFlow()
	{
		KIDIntegration.<StartKidFlow>d__69 <StartKidFlow>d__;
		<StartKidFlow>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<StartKidFlow>d__.<>4__this = this;
		<StartKidFlow>d__.<>1__state = -1;
		<StartKidFlow>d__.<>t__builder.Start<KIDIntegration.<StartKidFlow>d__69>(ref <StartKidFlow>d__);
		return <StartKidFlow>d__.<>t__builder.Task;
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x000D8ED4 File Offset: 0x000D70D4
	public Task<KIDIntegration.EKIDSessionUpdateType> RequestUpdateSession()
	{
		KIDIntegration.<RequestUpdateSession>d__70 <RequestUpdateSession>d__;
		<RequestUpdateSession>d__.<>t__builder = AsyncTaskMethodBuilder<KIDIntegration.EKIDSessionUpdateType>.Create();
		<RequestUpdateSession>d__.<>4__this = this;
		<RequestUpdateSession>d__.<>1__state = -1;
		<RequestUpdateSession>d__.<>t__builder.Start<KIDIntegration.<RequestUpdateSession>d__70>(ref <RequestUpdateSession>d__);
		return <RequestUpdateSession>d__.<>t__builder.Task;
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000D8F18 File Offset: 0x000D7118
	public void RequestNewChallenge()
	{
		int userAge = KIDAgeGate.UserAge;
		DateTime birthday = DateTime.Now.AddYears(userAge * -1);
		this.TriggerChallenge(birthday);
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000D8F44 File Offset: 0x000D7144
	public void RestartChallengeAwait()
	{
		Guid challengeId = this._kIDChallenge.ChallengeId;
		this.WaitForChallengeResult(challengeId);
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000D8F65 File Offset: 0x000D7165
	public AgeStatusType GetActiveAccountType()
	{
		return this._activeAccountStatus;
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000D8F6D File Offset: 0x000D716D
	public string GetOneTimePasscode()
	{
		Challenge kIDChallenge = this._kIDChallenge;
		return ((kIDChallenge != null) ? kIDChallenge.OneTimePassword : null) ?? "INVALID PASSCODE";
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000D8A47 File Offset: 0x000D6C47
	public Session GetKidSession()
	{
		return KIDIntegration._kIDSession;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000D8F8C File Offset: 0x000D718C
	public int GetAgeFromDateOfBirth()
	{
		if (KIDIntegration._kIDSession != null)
		{
			DateTime today = DateTime.Today;
			DateTime dateOfBirth = KIDIntegration._kIDSession.DateOfBirth;
			int num = today.Year - dateOfBirth.Year;
			int num2 = today.Month - dateOfBirth.Month;
			if (num2 < 0)
			{
				num--;
			}
			else if (num2 == 0 && today.Day - dateOfBirth.Day < 0)
			{
				num--;
			}
			return num;
		}
		return -1;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000D8FF7 File Offset: 0x000D71F7
	public void RestartKid()
	{
		this._isSettingAge = false;
		KIDIntegration.HasSetAge = false;
		this.ClearSession();
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000D900C File Offset: 0x000D720C
	public static void NotifyHasSetAge()
	{
		KIDIntegration.HasSetAge = true;
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000D9014 File Offset: 0x000D7214
	private static SKIDPermissionData? GetPermissionDataByName(string permissionName)
	{
		if (!KIDIntegration._permissionsList.ContainsKey(permissionName))
		{
			return null;
		}
		return new SKIDPermissionData?(KIDIntegration._permissionsList[permissionName]);
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x000D9048 File Offset: 0x000D7248
	private void ClearSession()
	{
		PlayerPrefs.DeleteKey(KIDIntegration.GetSessionIdPlayerPrefRef);
		PlayerPrefs.DeleteKey(KIDIntegration.GetChallengeIdPlayerPrefRef);
		PlayerPrefs.DeleteKey("kIDSessionETAG");
		KIDIntegration._kIDSession = null;
		this._kIDChallenge = null;
		this._sessionId = Guid.Empty;
		this._activeAccountStatus = (AgeStatusType)0;
		this.ClearSessionPermissions();
		this.DeleteStoredPermissions();
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x000D90A0 File Offset: 0x000D72A0
	private Task UpdateSession(Session newSession)
	{
		KIDIntegration.<UpdateSession>d__81 <UpdateSession>d__;
		<UpdateSession>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateSession>d__.<>4__this = this;
		<UpdateSession>d__.newSession = newSession;
		<UpdateSession>d__.<>1__state = -1;
		<UpdateSession>d__.<>t__builder.Start<KIDIntegration.<UpdateSession>d__81>(ref <UpdateSession>d__);
		return <UpdateSession>d__.<>t__builder.Task;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x000D90EC File Offset: 0x000D72EC
	private void UpdateSessionPermissions(List<Permission> permissions, bool debug = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("[KID] Session Found. Permissions count [" + permissions.Count.ToString() + "]:");
		foreach (Permission permission in permissions)
		{
			stringBuilder.AppendLine(string.Format("- [{0}] | Enabled: [{1}] | ManagedBy: [{2}]", permission.Name, permission.Enabled, permission.ManagedBy.ToString()));
			if (!KIDIntegration._permissionsList.ContainsKey(permission.Name))
			{
				KIDIntegration._permissionsList.Add(permission.Name, default(SKIDPermissionData));
			}
			KIDIntegration._permissionsList[permission.Name] = new SKIDPermissionData(permission, permission.Enabled, permission.Name);
		}
		if (debug)
		{
			Debug.Log(stringBuilder.ToString());
		}
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x000D91F8 File Offset: 0x000D73F8
	private void ClearSessionPermissions()
	{
		Dictionary<string, SKIDPermissionData> dictionary = new Dictionary<string, SKIDPermissionData>();
		foreach (KeyValuePair<string, SKIDPermissionData> keyValuePair in KIDIntegration._permissionsList)
		{
			dictionary.Add(keyValuePair.Key, new SKIDPermissionData(keyValuePair.Value.PermissionData, false, keyValuePair.Value.PermissionName));
		}
		KIDIntegration._permissionsList = dictionary;
	}

	// Token: 0x06002C31 RID: 11313 RVA: 0x000D9280 File Offset: 0x000D7480
	private void SaveStoredPermissions()
	{
		if (KIDIntegration.CurrentSession == null)
		{
			return;
		}
		string text = "";
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		foreach (Permission permission in KIDIntegration.CurrentSession.Permissions)
		{
			string key = permission.Name + "-" + playFabPlayerId + "-enabled";
			string key2 = permission.Name + "-" + playFabPlayerId + "-managed-by";
			PlayerPrefs.SetInt(key, permission.Enabled ? 1 : 0);
			PlayerPrefs.SetInt(key2, (int)permission.ManagedBy);
			text = text + permission.Name + ",";
		}
		text.Trim(',');
		PlayerPrefs.SetString("kid-permission-csv", text);
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x000D9360 File Offset: 0x000D7560
	private void DeleteStoredPermissions()
	{
		PlayerPrefs.DeleteKey("kIDSessionETAG");
		PlayerPrefs.DeleteKey(KIDIntegration.GetSessionIdPlayerPrefRef);
		string @string = PlayerPrefs.GetString("kid-permission-csv");
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		if (string.IsNullOrEmpty(playFabPlayerId))
		{
			Debug.LogError("[KID] Trying to delete stored permissions, but PlayFabId is not set yet");
			return;
		}
		string[] array = @string.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			PlayerPrefs.DeleteKey(array[i] + "-" + playFabPlayerId + "-enabled");
			PlayerPrefs.DeleteKey(array[i] + "-" + playFabPlayerId + "-managed-by");
		}
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000D9400 File Offset: 0x000D7600
	private Task SetDefaultKIDPermissions()
	{
		KIDIntegration.<SetDefaultKIDPermissions>d__86 <SetDefaultKIDPermissions>d__;
		<SetDefaultKIDPermissions>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetDefaultKIDPermissions>d__.<>4__this = this;
		<SetDefaultKIDPermissions>d__.<>1__state = -1;
		<SetDefaultKIDPermissions>d__.<>t__builder.Start<KIDIntegration.<SetDefaultKIDPermissions>d__86>(ref <SetDefaultKIDPermissions>d__);
		return <SetDefaultKIDPermissions>d__.<>t__builder.Task;
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000D9444 File Offset: 0x000D7644
	private void UpdateSessionWithDefaults()
	{
		if (KIDIntegration._defaultPermissionsList == null || KIDIntegration._defaultPermissionsList.Count == 0)
		{
			Debug.LogError("[KID] Trying to update session to the default values. But default permissions have not been loaded yet");
			return;
		}
		List<Permission> list = new List<Permission>();
		foreach (KeyValuePair<string, SKIDPermissionData> keyValuePair in KIDIntegration._defaultPermissionsList)
		{
			Permission item = new Permission(keyValuePair.Value.PermissionData.Name, keyValuePair.Value.PermissionData.Enabled, keyValuePair.Value.PermissionData.ManagedBy);
			list.Add(item);
		}
		this.UpdateSessionPermissions(list, false);
		this.SaveStoredPermissions();
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x000D9510 File Offset: 0x000D7710
	private void UpdateDefaultPermissions(List<Permission> defaultPermissions)
	{
		foreach (Permission permission in defaultPermissions)
		{
			if (!KIDIntegration._defaultPermissionsList.ContainsKey(permission.Name))
			{
				KIDIntegration._defaultPermissionsList.Add(permission.Name, default(SKIDPermissionData));
			}
			KIDIntegration._defaultPermissionsList[permission.Name] = new SKIDPermissionData(permission, permission.Enabled, permission.Name);
		}
		this.SaveDefaultPermissions(defaultPermissions);
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000D95AC File Offset: 0x000D77AC
	private void SaveDefaultPermissions(List<Permission> defaultPermissions)
	{
		string text = "";
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		foreach (Permission permission in defaultPermissions)
		{
			string key = string.Concat(new string[]
			{
				"default-",
				permission.Name,
				"-",
				playFabPlayerId,
				"-enabled"
			});
			string key2 = string.Concat(new string[]
			{
				"default-",
				permission.Name,
				"-",
				playFabPlayerId,
				"-managed-by"
			});
			PlayerPrefs.SetInt(key, permission.Enabled ? 1 : 0);
			PlayerPrefs.SetInt(key2, (int)permission.ManagedBy);
			text = text + permission.Name + ",";
		}
		text.Trim(',');
		PlayerPrefs.SetString("kid-default-permission-csv", text);
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000D96B4 File Offset: 0x000D78B4
	private bool LoadDefaultPermissions()
	{
		string @string = PlayerPrefs.GetString("kid-default-permission-csv");
		if (string.IsNullOrEmpty(@string))
		{
			return false;
		}
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		if (string.IsNullOrEmpty(playFabPlayerId))
		{
			Debug.LogError("[KID] Trying to load stored permissions, but PlayFabId is not set yet");
			return false;
		}
		string[] array = @string.Split(',', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			int @int = PlayerPrefs.GetInt(string.Concat(new string[]
			{
				"default-",
				array[i],
				"-",
				playFabPlayerId,
				"-enabled"
			}), -1);
			if (@int != -1)
			{
				int int2 = PlayerPrefs.GetInt(string.Concat(new string[]
				{
					"default-",
					array[i],
					"-",
					playFabPlayerId,
					"-managed-by"
				}), -1);
				if (int2 != -1)
				{
					if (!KIDIntegration._defaultPermissionsList.ContainsKey(array[i]))
					{
						KIDIntegration._defaultPermissionsList.Add(array[i], default(SKIDPermissionData));
					}
					Permission permission = new Permission(array[i], @int == 1, (Permission.ManagedByEnum)int2);
					KIDIntegration._defaultPermissionsList[array[i]] = new SKIDPermissionData(permission, permission.Enabled, permission.Name);
				}
			}
		}
		return array.Length != 0;
	}

	// Token: 0x04003140 RID: 12608
	public const string SESSION_ID_PREFIX_PLAYER_PREF = "kIDSessionID-";

	// Token: 0x04003141 RID: 12609
	public const string SESSION_ETAG_PLAYER_PREF = "kIDSessionETAG";

	// Token: 0x04003142 RID: 12610
	public const string INVALID_PASSCODE_RESPONSE = "INVALID PASSCODE";

	// Token: 0x04003143 RID: 12611
	private const string CHALLENEGE_ID_PREFIX_PLAYER_PREF = "ageGateChallenge-";

	// Token: 0x04003144 RID: 12612
	private const string KID_PERMISSIONS_CSV = "kid-permission-csv";

	// Token: 0x04003145 RID: 12613
	private const string KID_DEFAULT_PERMISSIONS_CSV = "kid-default-permission-csv";

	// Token: 0x04003146 RID: 12614
	private const string KID_PERMISSIONS_ENABLED_KEY = "-enabled";

	// Token: 0x04003147 RID: 12615
	private const string KID_PERMISSIONS_MANAGED_BY_KEY = "-managed-by";

	// Token: 0x04003148 RID: 12616
	private const int CHALLENGE_TOTAL_TIMEOUT_SEC = 900;

	// Token: 0x04003149 RID: 12617
	private const int CHALLENGE_REQUEST_TIMEOUT_SEC = 0;

	// Token: 0x0400314A RID: 12618
	private const int CHALLENGE_TIME_BETWEEN_REQUESTS_SEC = 30;

	// Token: 0x0400314B RID: 12619
	public const string VOICE_CHAT_PERMISSION_NAME = "voice-chat";

	// Token: 0x0400314C RID: 12620
	public const string CUSTOM_USERNAME_PERMISSION_NAME = "custom-username";

	// Token: 0x0400314D RID: 12621
	public const string PRIVATE_ROOM_PERMISSION_NAME = "join-groups";

	// Token: 0x0400314E RID: 12622
	private Action _sessionUpdatedCallback;

	// Token: 0x0400314F RID: 12623
	private static Action _onKIDInitialisationComplete;

	// Token: 0x04003150 RID: 12624
	private bool _isSettingAge;

	// Token: 0x04003151 RID: 12625
	private double _timeoutTime;

	// Token: 0x04003152 RID: 12626
	private double _timeoutOverride;

	// Token: 0x04003153 RID: 12627
	private Challenge _kIDChallenge;

	// Token: 0x04003154 RID: 12628
	private Guid _sessionId = Guid.Empty;

	// Token: 0x04003155 RID: 12629
	private AgeStatusType _activeAccountStatus;

	// Token: 0x04003156 RID: 12630
	private CancellationToken _cancellationToken;

	// Token: 0x04003157 RID: 12631
	private static string sessionIdPlayerPrefRef;

	// Token: 0x04003158 RID: 12632
	private static string challengeIdPlayerPrefRef;

	// Token: 0x04003159 RID: 12633
	private static Session _kIDSession;

	// Token: 0x0400315A RID: 12634
	private static Dictionary<string, SKIDPermissionData> _permissionsList = new Dictionary<string, SKIDPermissionData>();

	// Token: 0x0400315B RID: 12635
	private static Dictionary<string, SKIDPermissionData> _defaultPermissionsList = new Dictionary<string, SKIDPermissionData>();

	// Token: 0x020006EB RID: 1771
	public enum EKIDSessionUpdateType
	{
		// Token: 0x04003160 RID: 12640
		Success = 1,
		// Token: 0x04003161 RID: 12641
		Fail,
		// Token: 0x04003162 RID: 12642
		Not_Modified
	}
}
