using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using GorillaNetworking;
using KID.Model;
using UnityEngine;

// Token: 0x02000701 RID: 1793
public class KIDManager : MonoBehaviour
{
	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C6A RID: 11370 RVA: 0x0004D5A8 File Offset: 0x0004B7A8
	public static KIDManager Instance
	{
		get
		{
			return KIDManager._instance;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C6B RID: 11371 RVA: 0x0004D5AF File Offset: 0x0004B7AF
	// (set) Token: 0x06002C6C RID: 11372 RVA: 0x0004D5B6 File Offset: 0x0004B7B6
	[OnEnterPlay_SetNull]
	public static string KidAccessToken { get; private set; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C6D RID: 11373 RVA: 0x0004D5BE File Offset: 0x0004B7BE
	// (set) Token: 0x06002C6E RID: 11374 RVA: 0x0004D5C5 File Offset: 0x0004B7C5
	[OnEnterPlay_SetNull]
	public static string KidRefreshToken { get; private set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002C6F RID: 11375 RVA: 0x0004D5CD File Offset: 0x0004B7CD
	// (set) Token: 0x06002C70 RID: 11376 RVA: 0x0004D5D4 File Offset: 0x0004B7D4
	[OnEnterPlay_SetNull]
	public static string KidBasePath { get; private set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002C71 RID: 11377 RVA: 0x0004D5DC File Offset: 0x0004B7DC
	// (set) Token: 0x06002C72 RID: 11378 RVA: 0x0004D5E3 File Offset: 0x0004B7E3
	[OnEnterPlay_SetNull]
	public static string Location { get; private set; }

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002C73 RID: 11379 RVA: 0x0004D5EB File Offset: 0x0004B7EB
	// (set) Token: 0x06002C74 RID: 11380 RVA: 0x0004D5F2 File Offset: 0x0004B7F2
	public static bool KidEnabled { get; private set; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06002C75 RID: 11381 RVA: 0x0004D5FA File Offset: 0x0004B7FA
	public static KIDIntegration KIDIntegration
	{
		get
		{
			return KIDManager._kIDIntegration;
		}
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x001214E8 File Offset: 0x0011F6E8
	private void Awake()
	{
		if (KIDManager._instance != null)
		{
			Debug.LogError("Trying to create new instance of [KIDManager], but one already exists. Destroying object [" + base.gameObject.name + "].");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Debug.Log("[KID] INIT");
		KIDManager._instance = this;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x00121540 File Offset: 0x0011F740
	private void Start()
	{
		KIDManager.<Start>d__28 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDManager.<Start>d__28>(ref <Start>d__);
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x0004D601 File Offset: 0x0004B801
	private void OnDestroy()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x0004D60D File Offset: 0x0004B80D
	public AgeStatusType GetActiveAccountStatus()
	{
		if (KIDManager._kIDIntegration == null)
		{
			if (!PlayFabAuthenticator.instance.GetSafety())
			{
				return AgeStatusType.LEGALADULT;
			}
			return AgeStatusType.DIGITALMINOR;
		}
		else
		{
			KIDIntegration kIDIntegration = KIDManager._kIDIntegration;
			if (kIDIntegration == null)
			{
				return (AgeStatusType)0;
			}
			return kIDIntegration.GetActiveAccountType();
		}
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x0004D638 File Offset: 0x0004B838
	public List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration.GetAllPermissionsData();
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x00121570 File Offset: 0x0011F770
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public ValueTuple<bool, Permission.ManagedByEnum> GetVoiceChatPermissionStatus()
	{
		bool flag = !PlayFabAuthenticator.instance.GetSafety();
		if (KIDManager._kIDIntegration != null)
		{
			return KIDIntegration.GetVoiceChatPermissionStatus();
		}
		if (flag)
		{
			return new ValueTuple<bool, Permission.ManagedByEnum>(true, Permission.ManagedByEnum.PLAYER);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x001215AC File Offset: 0x0011F7AC
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public ValueTuple<bool, Permission.ManagedByEnum> GetCustomNicknamePermissionStatus()
	{
		bool flag = !PlayFabAuthenticator.instance.GetSafety();
		if (KIDManager._kIDIntegration != null)
		{
			return KIDIntegration.GetCustomUserNamesPermissionStatus();
		}
		if (flag)
		{
			return new ValueTuple<bool, Permission.ManagedByEnum>(true, Permission.ManagedByEnum.PLAYER);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x001215E8 File Offset: 0x0011F7E8
	[return: TupleElementNames(new string[]
	{
		"enabled",
		"managedBy"
	})]
	public ValueTuple<bool, Permission.ManagedByEnum> GetPrivateRoomPermissionStatus()
	{
		bool flag = !PlayFabAuthenticator.instance.GetSafety();
		if (KIDManager._kIDIntegration != null)
		{
			return KIDIntegration.GetPrivateRoomPermissionStatus();
		}
		if (flag)
		{
			return new ValueTuple<bool, Permission.ManagedByEnum>(true, Permission.ManagedByEnum.PLAYER);
		}
		return new ValueTuple<bool, Permission.ManagedByEnum>(false, Permission.ManagedByEnum.GUARDIAN);
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x00121624 File Offset: 0x0011F824
	public static void VerifyKidAuthenticated(string accessToken, string refreshToken, string location, string basePath)
	{
		KIDManager.KidEnabled = true;
		if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(basePath))
		{
			bool flag = string.IsNullOrEmpty(accessToken);
			bool flag2 = string.IsNullOrEmpty(refreshToken);
			bool flag3 = string.IsNullOrEmpty(location);
			bool flag4 = string.IsNullOrEmpty(basePath);
			string text = flag ? "\nAccess Token" : "";
			text += (flag2 ? "\nRefresh Token" : "");
			text += (flag3 ? "\nLocation" : "");
			text += (flag4 ? "\nBase Path" : "");
			Debug.LogError("[KID] One or more k-ID elements are invalid. Unable to authenticate k-ID. Is Null Or Empty: " + text);
			return;
		}
		KIDManager.KidAccessToken = accessToken;
		KIDManager.KidRefreshToken = refreshToken;
		KIDManager.Location = location;
		KIDManager.KidBasePath = basePath;
		KIDManager._kIDIntegration = new KIDIntegration(new Action(KIDManager.OnSessionUpdated), KIDManager._requestCancellationSource.Token);
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x0004D63F File Offset: 0x0004B83F
	public static void DisableKid()
	{
		KIDManager.KidEnabled = false;
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x0004D647 File Offset: 0x0004B847
	public static void KidRefreshed(string accessToken)
	{
		KIDManager.KidAccessToken = accessToken;
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x0004D601 File Offset: 0x0004B801
	public static void CancelToken()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x0004D64F File Offset: 0x0004B84F
	public static CancellationTokenSource ResetCancellationToken()
	{
		KIDManager._requestCancellationSource.Dispose();
		KIDManager._requestCancellationSource = new CancellationTokenSource();
		return KIDManager._requestCancellationSource;
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x0004D66A File Offset: 0x0004B86A
	public static void RegisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Combine(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x0004D68B File Offset: 0x0004B88B
	public static void UnregisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully unregistered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Remove(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x0004D6AC File Offset: 0x0004B8AC
	public static void RegisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x0004D6CD File Offset: 0x0004B8CD
	public static void UnregisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x0004D6EE File Offset: 0x0004B8EE
	public static void RegisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x0004D70F File Offset: 0x0004B90F
	public static void UnregisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x0004D730 File Offset: 0x0004B930
	public static void RegisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x0004D751 File Offset: 0x0004B951
	public static void UnregisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x00121710 File Offset: 0x0011F910
	private static void OnSessionUpdated()
	{
		Action onSessionUpdated_AnyPermission = KIDManager._onSessionUpdated_AnyPermission;
		if (onSessionUpdated_AnyPermission != null)
		{
			onSessionUpdated_AnyPermission();
		}
		bool voiceChatEnabled = false;
		bool joinGroupsEnabled = false;
		bool customUsernamesEnabled = false;
		List<SKIDPermissionData> allPermissionsData = KIDIntegration.GetAllPermissionsData();
		int count = allPermissionsData.Count;
		for (int i = 0; i < count; i++)
		{
			Permission permissionData = allPermissionsData[i].PermissionData;
			string name = permissionData.Name;
			if (!(name == "voice-chat"))
			{
				if (!(name == "custom-username"))
				{
					if (!(name == "join-groups"))
					{
						Debug.Log("[KID] Tried updating permission with name [" + permissionData.Name + "] but did not match any of the set cases. Unable to process");
					}
					else
					{
						if (KIDManager.HasPermissionChanged(permissionData))
						{
							Action<bool, Permission.ManagedByEnum> onSessionUpdated_PrivateRooms = KIDManager._onSessionUpdated_PrivateRooms;
							if (onSessionUpdated_PrivateRooms != null)
							{
								onSessionUpdated_PrivateRooms(permissionData.Enabled, permissionData.ManagedBy);
							}
							KIDManager._previousPermissionSettings[permissionData.Name] = permissionData;
						}
						joinGroupsEnabled = permissionData.Enabled;
					}
				}
				else
				{
					if (KIDManager.HasPermissionChanged(permissionData))
					{
						Action<bool, Permission.ManagedByEnum> onSessionUpdated_CustomUsernames = KIDManager._onSessionUpdated_CustomUsernames;
						if (onSessionUpdated_CustomUsernames != null)
						{
							onSessionUpdated_CustomUsernames(permissionData.Enabled, permissionData.ManagedBy);
						}
						KIDManager._previousPermissionSettings[permissionData.Name] = permissionData;
					}
					customUsernamesEnabled = permissionData.Enabled;
				}
			}
			else
			{
				if (KIDManager.HasPermissionChanged(permissionData))
				{
					Action<bool, Permission.ManagedByEnum> onSessionUpdated_VoiceChat = KIDManager._onSessionUpdated_VoiceChat;
					if (onSessionUpdated_VoiceChat != null)
					{
						onSessionUpdated_VoiceChat(permissionData.Enabled, permissionData.ManagedBy);
					}
					KIDManager._previousPermissionSettings[permissionData.Name] = permissionData;
				}
				voiceChatEnabled = permissionData.Enabled;
			}
		}
		GorillaTelemetry.PostKidEvent(joinGroupsEnabled, voiceChatEnabled, customUsernamesEnabled, KIDManager.KIDIntegration.GetActiveAccountType(), GTKidEventType.permission_update);
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x001218A4 File Offset: 0x0011FAA4
	private static bool HasPermissionChanged(Permission newValue)
	{
		Permission permission;
		if (KIDManager._previousPermissionSettings.TryGetValue(newValue.Name, out permission))
		{
			return permission != newValue;
		}
		KIDManager._previousPermissionSettings.Add(newValue.Name, newValue);
		return true;
	}

	// Token: 0x040031C3 RID: 12739
	[OnEnterPlay_SetNull]
	private static KIDManager _instance;

	// Token: 0x040031C9 RID: 12745
	[OnEnterPlay_SetNull]
	private static KIDIntegration _kIDIntegration;

	// Token: 0x040031CA RID: 12746
	private static CancellationTokenSource _requestCancellationSource = new CancellationTokenSource();

	// Token: 0x040031CB RID: 12747
	public const string KID_PERMISSION__VOICE_CHAT = "voice-chat";

	// Token: 0x040031CC RID: 12748
	public const string KID_PERMISSION__CUSTOM_NAMES = "custom-username";

	// Token: 0x040031CD RID: 12749
	public const string KID_PERMISSION__PRIVATE_ROOMS = "join-groups";

	// Token: 0x040031CE RID: 12750
	[OnEnterPlay_SetNull]
	private static Action _onSessionUpdated_AnyPermission;

	// Token: 0x040031CF RID: 12751
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_VoiceChat;

	// Token: 0x040031D0 RID: 12752
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_CustomUsernames;

	// Token: 0x040031D1 RID: 12753
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_PrivateRooms;

	// Token: 0x040031D2 RID: 12754
	[OnEnterPlay_SetNull]
	private static Dictionary<string, Permission> _previousPermissionSettings = new Dictionary<string, Permission>();
}
