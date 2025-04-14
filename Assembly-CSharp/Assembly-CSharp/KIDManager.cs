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
	// (get) Token: 0x06002C6A RID: 11370 RVA: 0x000DBA5A File Offset: 0x000D9C5A
	public static KIDManager Instance
	{
		get
		{
			return KIDManager._instance;
		}
	}

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000DBA61 File Offset: 0x000D9C61
	// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000DBA68 File Offset: 0x000D9C68
	[OnEnterPlay_SetNull]
	public static string KidAccessToken { get; private set; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000DBA70 File Offset: 0x000D9C70
	// (set) Token: 0x06002C6E RID: 11374 RVA: 0x000DBA77 File Offset: 0x000D9C77
	[OnEnterPlay_SetNull]
	public static string KidRefreshToken { get; private set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000DBA7F File Offset: 0x000D9C7F
	// (set) Token: 0x06002C70 RID: 11376 RVA: 0x000DBA86 File Offset: 0x000D9C86
	[OnEnterPlay_SetNull]
	public static string KidBasePath { get; private set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000DBA8E File Offset: 0x000D9C8E
	// (set) Token: 0x06002C72 RID: 11378 RVA: 0x000DBA95 File Offset: 0x000D9C95
	[OnEnterPlay_SetNull]
	public static string Location { get; private set; }

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000DBA9D File Offset: 0x000D9C9D
	// (set) Token: 0x06002C74 RID: 11380 RVA: 0x000DBAA4 File Offset: 0x000D9CA4
	public static bool KidEnabled { get; private set; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000DBAAC File Offset: 0x000D9CAC
	public static KIDIntegration KIDIntegration
	{
		get
		{
			return KIDManager._kIDIntegration;
		}
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x000DBAB4 File Offset: 0x000D9CB4
	private void Awake()
	{
		if (KIDManager._instance != null)
		{
			Debug.LogError("Trying to create new instance of [KIDManager], but one already exists. Destroying object [" + base.gameObject.name + "].");
			Object.Destroy(base.gameObject);
			return;
		}
		Debug.Log("[KID] INIT");
		KIDManager._instance = this;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000DBB0C File Offset: 0x000D9D0C
	private void Start()
	{
		KIDManager.<Start>d__28 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDManager.<Start>d__28>(ref <Start>d__);
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000DBB3B File Offset: 0x000D9D3B
	private void OnDestroy()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000DBB47 File Offset: 0x000D9D47
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

	// Token: 0x06002C7A RID: 11386 RVA: 0x000DBB72 File Offset: 0x000D9D72
	public List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration.GetAllPermissionsData();
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000DBB7C File Offset: 0x000D9D7C
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

	// Token: 0x06002C7C RID: 11388 RVA: 0x000DBBB8 File Offset: 0x000D9DB8
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

	// Token: 0x06002C7D RID: 11389 RVA: 0x000DBBF4 File Offset: 0x000D9DF4
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

	// Token: 0x06002C7E RID: 11390 RVA: 0x000DBC30 File Offset: 0x000D9E30
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

	// Token: 0x06002C7F RID: 11391 RVA: 0x000DBD1B File Offset: 0x000D9F1B
	public static void DisableKid()
	{
		KIDManager.KidEnabled = false;
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000DBD23 File Offset: 0x000D9F23
	public static void KidRefreshed(string accessToken)
	{
		KIDManager.KidAccessToken = accessToken;
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000DBB3B File Offset: 0x000D9D3B
	public static void CancelToken()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x000DBD2B File Offset: 0x000D9F2B
	public static CancellationTokenSource ResetCancellationToken()
	{
		KIDManager._requestCancellationSource.Dispose();
		KIDManager._requestCancellationSource = new CancellationTokenSource();
		return KIDManager._requestCancellationSource;
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000DBD46 File Offset: 0x000D9F46
	public static void RegisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Combine(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000DBD67 File Offset: 0x000D9F67
	public static void UnregisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully unregistered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Remove(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x000DBD88 File Offset: 0x000D9F88
	public static void RegisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x000DBDA9 File Offset: 0x000D9FA9
	public static void UnregisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x000DBDCA File Offset: 0x000D9FCA
	public static void RegisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x000DBDEB File Offset: 0x000D9FEB
	public static void UnregisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x000DBE0C File Offset: 0x000DA00C
	public static void RegisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x000DBE2D File Offset: 0x000DA02D
	public static void UnregisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x000DBE50 File Offset: 0x000DA050
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

	// Token: 0x06002C8C RID: 11404 RVA: 0x000DBFE4 File Offset: 0x000DA1E4
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
