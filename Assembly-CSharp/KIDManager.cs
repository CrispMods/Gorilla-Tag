using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using GorillaNetworking;
using KID.Model;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class KIDManager : MonoBehaviour
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000DB5DA File Offset: 0x000D97DA
	public static KIDManager Instance
	{
		get
		{
			return KIDManager._instance;
		}
	}

	// Token: 0x170004AE RID: 1198
	// (get) Token: 0x06002C63 RID: 11363 RVA: 0x000DB5E1 File Offset: 0x000D97E1
	// (set) Token: 0x06002C64 RID: 11364 RVA: 0x000DB5E8 File Offset: 0x000D97E8
	[OnEnterPlay_SetNull]
	public static string KidAccessToken { get; private set; }

	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000DB5F0 File Offset: 0x000D97F0
	// (set) Token: 0x06002C66 RID: 11366 RVA: 0x000DB5F7 File Offset: 0x000D97F7
	[OnEnterPlay_SetNull]
	public static string KidRefreshToken { get; private set; }

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06002C67 RID: 11367 RVA: 0x000DB5FF File Offset: 0x000D97FF
	// (set) Token: 0x06002C68 RID: 11368 RVA: 0x000DB606 File Offset: 0x000D9806
	[OnEnterPlay_SetNull]
	public static string KidBasePath { get; private set; }

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000DB60E File Offset: 0x000D980E
	// (set) Token: 0x06002C6A RID: 11370 RVA: 0x000DB615 File Offset: 0x000D9815
	[OnEnterPlay_SetNull]
	public static string Location { get; private set; }

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000DB61D File Offset: 0x000D981D
	// (set) Token: 0x06002C6C RID: 11372 RVA: 0x000DB624 File Offset: 0x000D9824
	public static bool KidEnabled { get; private set; }

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000DB62C File Offset: 0x000D982C
	public static KIDIntegration KIDIntegration
	{
		get
		{
			return KIDManager._kIDIntegration;
		}
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000DB634 File Offset: 0x000D9834
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

	// Token: 0x06002C6F RID: 11375 RVA: 0x000DB68C File Offset: 0x000D988C
	private void Start()
	{
		KIDManager.<Start>d__28 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDManager.<Start>d__28>(ref <Start>d__);
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000DB6BB File Offset: 0x000D98BB
	private void OnDestroy()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000DB6C7 File Offset: 0x000D98C7
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

	// Token: 0x06002C72 RID: 11378 RVA: 0x000DB6F2 File Offset: 0x000D98F2
	public List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration.GetAllPermissionsData();
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000DB6FC File Offset: 0x000D98FC
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

	// Token: 0x06002C74 RID: 11380 RVA: 0x000DB738 File Offset: 0x000D9938
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

	// Token: 0x06002C75 RID: 11381 RVA: 0x000DB774 File Offset: 0x000D9974
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

	// Token: 0x06002C76 RID: 11382 RVA: 0x000DB7B0 File Offset: 0x000D99B0
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

	// Token: 0x06002C77 RID: 11383 RVA: 0x000DB89B File Offset: 0x000D9A9B
	public static void DisableKid()
	{
		KIDManager.KidEnabled = false;
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000DB8A3 File Offset: 0x000D9AA3
	public static void KidRefreshed(string accessToken)
	{
		KIDManager.KidAccessToken = accessToken;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000DB6BB File Offset: 0x000D98BB
	public static void CancelToken()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000DB8AB File Offset: 0x000D9AAB
	public static CancellationTokenSource ResetCancellationToken()
	{
		KIDManager._requestCancellationSource.Dispose();
		KIDManager._requestCancellationSource = new CancellationTokenSource();
		return KIDManager._requestCancellationSource;
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000DB8C6 File Offset: 0x000D9AC6
	public static void RegisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Combine(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000DB8E7 File Offset: 0x000D9AE7
	public static void UnregisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully unregistered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Remove(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x000DB908 File Offset: 0x000D9B08
	public static void RegisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x000DB929 File Offset: 0x000D9B29
	public static void UnregisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x000DB94A File Offset: 0x000D9B4A
	public static void RegisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000DB96B File Offset: 0x000D9B6B
	public static void UnregisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000DB98C File Offset: 0x000D9B8C
	public static void RegisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x000DB9AD File Offset: 0x000D9BAD
	public static void UnregisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000DB9D0 File Offset: 0x000D9BD0
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

	// Token: 0x06002C84 RID: 11396 RVA: 0x000DBB64 File Offset: 0x000D9D64
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

	// Token: 0x040031BD RID: 12733
	[OnEnterPlay_SetNull]
	private static KIDManager _instance;

	// Token: 0x040031C3 RID: 12739
	[OnEnterPlay_SetNull]
	private static KIDIntegration _kIDIntegration;

	// Token: 0x040031C4 RID: 12740
	private static CancellationTokenSource _requestCancellationSource = new CancellationTokenSource();

	// Token: 0x040031C5 RID: 12741
	public const string KID_PERMISSION__VOICE_CHAT = "voice-chat";

	// Token: 0x040031C6 RID: 12742
	public const string KID_PERMISSION__CUSTOM_NAMES = "custom-username";

	// Token: 0x040031C7 RID: 12743
	public const string KID_PERMISSION__PRIVATE_ROOMS = "join-groups";

	// Token: 0x040031C8 RID: 12744
	[OnEnterPlay_SetNull]
	private static Action _onSessionUpdated_AnyPermission;

	// Token: 0x040031C9 RID: 12745
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_VoiceChat;

	// Token: 0x040031CA RID: 12746
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_CustomUsernames;

	// Token: 0x040031CB RID: 12747
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_PrivateRooms;

	// Token: 0x040031CC RID: 12748
	[OnEnterPlay_SetNull]
	private static Dictionary<string, Permission> _previousPermissionSettings = new Dictionary<string, Permission>();
}
