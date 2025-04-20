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

// Token: 0x020006FE RID: 1790
public class KIDIntegration
{
	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002C93 RID: 11411 RVA: 0x00123424 File Offset: 0x00121624
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

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002C94 RID: 11412 RVA: 0x0004E4C9 File Offset: 0x0004C6C9
	// (set) Token: 0x06002C95 RID: 11413 RVA: 0x0004E4D0 File Offset: 0x0004C6D0
	public static AwaitChallengeResponse.StatusEnum ChallengeStatus { get; private set; } = AwaitChallengeResponse.StatusEnum.INPROGRESS;

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06002C96 RID: 11414 RVA: 0x0004E4D8 File Offset: 0x0004C6D8
	public static bool HasFoundSession
	{
		get
		{
			return KIDIntegration.CurrentSession != null;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06002C97 RID: 11415 RVA: 0x0004E4E2 File Offset: 0x0004C6E2
	// (set) Token: 0x06002C98 RID: 11416 RVA: 0x0004E4E9 File Offset: 0x0004C6E9
	public static bool IsReady { get; private set; }

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06002C99 RID: 11417 RVA: 0x0004E4F1 File Offset: 0x0004C6F1
	// (set) Token: 0x06002C9A RID: 11418 RVA: 0x0004E4F8 File Offset: 0x0004C6F8
	public static bool HasSetAge { get; private set; }

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06002C9B RID: 11419 RVA: 0x0004E500 File Offset: 0x0004C700
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

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06002C9C RID: 11420 RVA: 0x0004E52E File Offset: 0x0004C72E
	// (set) Token: 0x06002C9D RID: 11421 RVA: 0x0004E535 File Offset: 0x0004C735
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

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06002C9E RID: 11422 RVA: 0x0004E54B File Offset: 0x0004C74B
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

	// Token: 0x06002C9F RID: 11423 RVA: 0x0004E579 File Offset: 0x0004C779
	public KIDIntegration(Action sessionUpdatedCallback, CancellationToken cancellationToken)
	{
		this._cancellationToken = cancellationToken;
		this._sessionUpdatedCallback = sessionUpdatedCallback;
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x00123468 File Offset: 0x00121668
	public void InitialiseKIDSystem()
	{
		KIDIntegration.<InitialiseKIDSystem>d__53 <InitialiseKIDSystem>d__;
		<InitialiseKIDSystem>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<InitialiseKIDSystem>d__.<>4__this = this;
		<InitialiseKIDSystem>d__.<>1__state = -1;
		<InitialiseKIDSystem>d__.<>t__builder.Start<KIDIntegration.<InitialiseKIDSystem>d__53>(ref <InitialiseKIDSystem>d__);
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x001234A0 File Offset: 0x001216A0
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

	// Token: 0x06002CA2 RID: 11426 RVA: 0x001234EC File Offset: 0x001216EC
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

	// Token: 0x06002CA3 RID: 11427 RVA: 0x00123540 File Offset: 0x00121740
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

	// Token: 0x06002CA4 RID: 11428 RVA: 0x00123594 File Offset: 0x00121794
	private Task<ValueTuple<bool, Guid>> TryGetPastChallenge()
	{
		KIDIntegration.<TryGetPastChallenge>d__57 <TryGetPastChallenge>d__;
		<TryGetPastChallenge>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, Guid>>.Create();
		<TryGetPastChallenge>d__.<>4__this = this;
		<TryGetPastChallenge>d__.<>1__state = -1;
		<TryGetPastChallenge>d__.<>t__builder.Start<KIDIntegration.<TryGetPastChallenge>d__57>(ref <TryGetPastChallenge>d__);
		return <TryGetPastChallenge>d__.<>t__builder.Task;
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x001235D8 File Offset: 0x001217D8
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

	// Token: 0x06002CA6 RID: 11430 RVA: 0x00123624 File Offset: 0x00121824
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

	// Token: 0x06002CA7 RID: 11431 RVA: 0x0004E59A File Offset: 0x0004C79A
	public static void RegisterOnInitialisationComplete(Action onComplete)
	{
		KIDIntegration._onKIDInitialisationComplete = (Action)Delegate.Combine(KIDIntegration._onKIDInitialisationComplete, onComplete);
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x0004E5B1 File Offset: 0x0004C7B1
	public static void UnregisterOnInitialisationComplete(Action onComplete)
	{
		KIDIntegration._onKIDInitialisationComplete = (Action)Delegate.Remove(KIDIntegration._onKIDInitialisationComplete, onComplete);
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x00123670 File Offset: 0x00121870
	public static Task LoadStoredPermissions()
	{
		KIDIntegration.<LoadStoredPermissions>d__62 <LoadStoredPermissions>d__;
		<LoadStoredPermissions>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadStoredPermissions>d__.<>1__state = -1;
		<LoadStoredPermissions>d__.<>t__builder.Start<KIDIntegration.<LoadStoredPermissions>d__62>(ref <LoadStoredPermissions>d__);
		return <LoadStoredPermissions>d__.<>t__builder.Task;
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x0004E5C8 File Offset: 0x0004C7C8
	public static string GetETAG()
	{
		return PlayerPrefs.GetString("kIDSessionETAG");
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x0004E5D4 File Offset: 0x0004C7D4
	public static List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration._permissionsList.Values.ToList<SKIDPermissionData>();
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x001236AC File Offset: 0x001218AC
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

	// Token: 0x06002CAD RID: 11437 RVA: 0x00123708 File Offset: 0x00121908
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

	// Token: 0x06002CAE RID: 11438 RVA: 0x00123764 File Offset: 0x00121964
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

	// Token: 0x06002CAF RID: 11439 RVA: 0x001237C0 File Offset: 0x001219C0
	public void AgeGateVerificationFlow()
	{
		KIDIntegration.<AgeGateVerificationFlow>d__68 <AgeGateVerificationFlow>d__;
		<AgeGateVerificationFlow>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AgeGateVerificationFlow>d__.<>4__this = this;
		<AgeGateVerificationFlow>d__.<>1__state = -1;
		<AgeGateVerificationFlow>d__.<>t__builder.Start<KIDIntegration.<AgeGateVerificationFlow>d__68>(ref <AgeGateVerificationFlow>d__);
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x001237F8 File Offset: 0x001219F8
	public Task StartKidFlow()
	{
		KIDIntegration.<StartKidFlow>d__69 <StartKidFlow>d__;
		<StartKidFlow>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<StartKidFlow>d__.<>4__this = this;
		<StartKidFlow>d__.<>1__state = -1;
		<StartKidFlow>d__.<>t__builder.Start<KIDIntegration.<StartKidFlow>d__69>(ref <StartKidFlow>d__);
		return <StartKidFlow>d__.<>t__builder.Task;
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x0012383C File Offset: 0x00121A3C
	public Task<KIDIntegration.EKIDSessionUpdateType> RequestUpdateSession()
	{
		KIDIntegration.<RequestUpdateSession>d__70 <RequestUpdateSession>d__;
		<RequestUpdateSession>d__.<>t__builder = AsyncTaskMethodBuilder<KIDIntegration.EKIDSessionUpdateType>.Create();
		<RequestUpdateSession>d__.<>4__this = this;
		<RequestUpdateSession>d__.<>1__state = -1;
		<RequestUpdateSession>d__.<>t__builder.Start<KIDIntegration.<RequestUpdateSession>d__70>(ref <RequestUpdateSession>d__);
		return <RequestUpdateSession>d__.<>t__builder.Task;
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x00123880 File Offset: 0x00121A80
	public void RequestNewChallenge()
	{
		int userAge = KIDAgeGate.UserAge;
		DateTime birthday = DateTime.Now.AddYears(userAge * -1);
		this.TriggerChallenge(birthday);
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x001238AC File Offset: 0x00121AAC
	public void RestartChallengeAwait()
	{
		Guid challengeId = this._kIDChallenge.ChallengeId;
		this.WaitForChallengeResult(challengeId);
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x0004E5E5 File Offset: 0x0004C7E5
	public AgeStatusType GetActiveAccountType()
	{
		return this._activeAccountStatus;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x0004E5ED File Offset: 0x0004C7ED
	public string GetOneTimePasscode()
	{
		Challenge kIDChallenge = this._kIDChallenge;
		return ((kIDChallenge != null) ? kIDChallenge.OneTimePassword : null) ?? "INVALID PASSCODE";
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x0004E52E File Offset: 0x0004C72E
	public Session GetKidSession()
	{
		return KIDIntegration._kIDSession;
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x001238D0 File Offset: 0x00121AD0
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

	// Token: 0x06002CB8 RID: 11448 RVA: 0x0004E60A File Offset: 0x0004C80A
	public void RestartKid()
	{
		this._isSettingAge = false;
		KIDIntegration.HasSetAge = false;
		this.ClearSession();
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x0004E61F File Offset: 0x0004C81F
	public static void NotifyHasSetAge()
	{
		KIDIntegration.HasSetAge = true;
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0012393C File Offset: 0x00121B3C
	private static SKIDPermissionData? GetPermissionDataByName(string permissionName)
	{
		if (!KIDIntegration._permissionsList.ContainsKey(permissionName))
		{
			return null;
		}
		return new SKIDPermissionData?(KIDIntegration._permissionsList[permissionName]);
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x00123970 File Offset: 0x00121B70
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

	// Token: 0x06002CBC RID: 11452 RVA: 0x001239C8 File Offset: 0x00121BC8
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

	// Token: 0x06002CBD RID: 11453 RVA: 0x00123A14 File Offset: 0x00121C14
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

	// Token: 0x06002CBE RID: 11454 RVA: 0x00123B20 File Offset: 0x00121D20
	private void ClearSessionPermissions()
	{
		Dictionary<string, SKIDPermissionData> dictionary = new Dictionary<string, SKIDPermissionData>();
		foreach (KeyValuePair<string, SKIDPermissionData> keyValuePair in KIDIntegration._permissionsList)
		{
			dictionary.Add(keyValuePair.Key, new SKIDPermissionData(keyValuePair.Value.PermissionData, false, keyValuePair.Value.PermissionName));
		}
		KIDIntegration._permissionsList = dictionary;
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x00123BA8 File Offset: 0x00121DA8
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

	// Token: 0x06002CC0 RID: 11456 RVA: 0x00123C88 File Offset: 0x00121E88
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

	// Token: 0x06002CC1 RID: 11457 RVA: 0x00123D28 File Offset: 0x00121F28
	private Task SetDefaultKIDPermissions()
	{
		KIDIntegration.<SetDefaultKIDPermissions>d__86 <SetDefaultKIDPermissions>d__;
		<SetDefaultKIDPermissions>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetDefaultKIDPermissions>d__.<>4__this = this;
		<SetDefaultKIDPermissions>d__.<>1__state = -1;
		<SetDefaultKIDPermissions>d__.<>t__builder.Start<KIDIntegration.<SetDefaultKIDPermissions>d__86>(ref <SetDefaultKIDPermissions>d__);
		return <SetDefaultKIDPermissions>d__.<>t__builder.Task;
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x00123D6C File Offset: 0x00121F6C
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

	// Token: 0x06002CC3 RID: 11459 RVA: 0x00123E38 File Offset: 0x00122038
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

	// Token: 0x06002CC4 RID: 11460 RVA: 0x00123ED4 File Offset: 0x001220D4
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

	// Token: 0x06002CC5 RID: 11461 RVA: 0x00123FDC File Offset: 0x001221DC
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

	// Token: 0x040031D7 RID: 12759
	public const string SESSION_ID_PREFIX_PLAYER_PREF = "kIDSessionID-";

	// Token: 0x040031D8 RID: 12760
	public const string SESSION_ETAG_PLAYER_PREF = "kIDSessionETAG";

	// Token: 0x040031D9 RID: 12761
	public const string INVALID_PASSCODE_RESPONSE = "INVALID PASSCODE";

	// Token: 0x040031DA RID: 12762
	private const string CHALLENEGE_ID_PREFIX_PLAYER_PREF = "ageGateChallenge-";

	// Token: 0x040031DB RID: 12763
	private const string KID_PERMISSIONS_CSV = "kid-permission-csv";

	// Token: 0x040031DC RID: 12764
	private const string KID_DEFAULT_PERMISSIONS_CSV = "kid-default-permission-csv";

	// Token: 0x040031DD RID: 12765
	private const string KID_PERMISSIONS_ENABLED_KEY = "-enabled";

	// Token: 0x040031DE RID: 12766
	private const string KID_PERMISSIONS_MANAGED_BY_KEY = "-managed-by";

	// Token: 0x040031DF RID: 12767
	private const int CHALLENGE_TOTAL_TIMEOUT_SEC = 900;

	// Token: 0x040031E0 RID: 12768
	private const int CHALLENGE_REQUEST_TIMEOUT_SEC = 0;

	// Token: 0x040031E1 RID: 12769
	private const int CHALLENGE_TIME_BETWEEN_REQUESTS_SEC = 30;

	// Token: 0x040031E2 RID: 12770
	public const string VOICE_CHAT_PERMISSION_NAME = "voice-chat";

	// Token: 0x040031E3 RID: 12771
	public const string CUSTOM_USERNAME_PERMISSION_NAME = "custom-username";

	// Token: 0x040031E4 RID: 12772
	public const string PRIVATE_ROOM_PERMISSION_NAME = "join-groups";

	// Token: 0x040031E5 RID: 12773
	private Action _sessionUpdatedCallback;

	// Token: 0x040031E6 RID: 12774
	private static Action _onKIDInitialisationComplete;

	// Token: 0x040031E7 RID: 12775
	private bool _isSettingAge;

	// Token: 0x040031E8 RID: 12776
	private double _timeoutTime;

	// Token: 0x040031E9 RID: 12777
	private double _timeoutOverride;

	// Token: 0x040031EA RID: 12778
	private Challenge _kIDChallenge;

	// Token: 0x040031EB RID: 12779
	private Guid _sessionId = Guid.Empty;

	// Token: 0x040031EC RID: 12780
	private AgeStatusType _activeAccountStatus;

	// Token: 0x040031ED RID: 12781
	private CancellationToken _cancellationToken;

	// Token: 0x040031EE RID: 12782
	private static string sessionIdPlayerPrefRef;

	// Token: 0x040031EF RID: 12783
	private static string challengeIdPlayerPrefRef;

	// Token: 0x040031F0 RID: 12784
	private static Session _kIDSession;

	// Token: 0x040031F1 RID: 12785
	private static Dictionary<string, SKIDPermissionData> _permissionsList = new Dictionary<string, SKIDPermissionData>();

	// Token: 0x040031F2 RID: 12786
	private static Dictionary<string, SKIDPermissionData> _defaultPermissionsList = new Dictionary<string, SKIDPermissionData>();

	// Token: 0x020006FF RID: 1791
	public enum EKIDSessionUpdateType
	{
		// Token: 0x040031F7 RID: 12791
		Success = 1,
		// Token: 0x040031F8 RID: 12792
		Fail,
		// Token: 0x040031F9 RID: 12793
		Not_Modified
	}
}
