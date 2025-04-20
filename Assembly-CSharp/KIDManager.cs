using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using GorillaNetworking;
using KID.Model;
using UnityEngine;

// Token: 0x02000715 RID: 1813
public class KIDManager : MonoBehaviour
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x0004E8ED File Offset: 0x0004CAED
	public static KIDManager Instance
	{
		get
		{
			return KIDManager._instance;
		}
	}

	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06002CF9 RID: 11513 RVA: 0x0004E8F4 File Offset: 0x0004CAF4
	// (set) Token: 0x06002CFA RID: 11514 RVA: 0x0004E8FB File Offset: 0x0004CAFB
	[OnEnterPlay_SetNull]
	public static string KidAccessToken { get; private set; }

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06002CFB RID: 11515 RVA: 0x0004E903 File Offset: 0x0004CB03
	// (set) Token: 0x06002CFC RID: 11516 RVA: 0x0004E90A File Offset: 0x0004CB0A
	[OnEnterPlay_SetNull]
	public static string KidRefreshToken { get; private set; }

	// Token: 0x170004BD RID: 1213
	// (get) Token: 0x06002CFD RID: 11517 RVA: 0x0004E912 File Offset: 0x0004CB12
	// (set) Token: 0x06002CFE RID: 11518 RVA: 0x0004E919 File Offset: 0x0004CB19
	[OnEnterPlay_SetNull]
	public static string KidBasePath { get; private set; }

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06002CFF RID: 11519 RVA: 0x0004E921 File Offset: 0x0004CB21
	// (set) Token: 0x06002D00 RID: 11520 RVA: 0x0004E928 File Offset: 0x0004CB28
	[OnEnterPlay_SetNull]
	public static string Location { get; private set; }

	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06002D01 RID: 11521 RVA: 0x0004E930 File Offset: 0x0004CB30
	// (set) Token: 0x06002D02 RID: 11522 RVA: 0x0004E937 File Offset: 0x0004CB37
	public static bool KidEnabled { get; private set; }

	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06002D03 RID: 11523 RVA: 0x0004E93F File Offset: 0x0004CB3F
	public static KIDIntegration KIDIntegration
	{
		get
		{
			return KIDManager._kIDIntegration;
		}
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x001260A0 File Offset: 0x001242A0
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

	// Token: 0x06002D05 RID: 11525 RVA: 0x001260F8 File Offset: 0x001242F8
	private void Start()
	{
		KIDManager.<Start>d__28 <Start>d__;
		<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Start>d__.<>1__state = -1;
		<Start>d__.<>t__builder.Start<KIDManager.<Start>d__28>(ref <Start>d__);
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x0004E946 File Offset: 0x0004CB46
	private void OnDestroy()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x0004E952 File Offset: 0x0004CB52
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

	// Token: 0x06002D08 RID: 11528 RVA: 0x0004E97D File Offset: 0x0004CB7D
	public List<SKIDPermissionData> GetAllPermissionsData()
	{
		return KIDIntegration.GetAllPermissionsData();
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x00126128 File Offset: 0x00124328
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

	// Token: 0x06002D0A RID: 11530 RVA: 0x00126164 File Offset: 0x00124364
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

	// Token: 0x06002D0B RID: 11531 RVA: 0x001261A0 File Offset: 0x001243A0
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

	// Token: 0x06002D0C RID: 11532 RVA: 0x001261DC File Offset: 0x001243DC
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

	// Token: 0x06002D0D RID: 11533 RVA: 0x0004E984 File Offset: 0x0004CB84
	public static void DisableKid()
	{
		KIDManager.KidEnabled = false;
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x0004E98C File Offset: 0x0004CB8C
	public static void KidRefreshed(string accessToken)
	{
		KIDManager.KidAccessToken = accessToken;
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x0004E946 File Offset: 0x0004CB46
	public static void CancelToken()
	{
		KIDManager._requestCancellationSource.Cancel();
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x0004E994 File Offset: 0x0004CB94
	public static CancellationTokenSource ResetCancellationToken()
	{
		KIDManager._requestCancellationSource.Dispose();
		KIDManager._requestCancellationSource = new CancellationTokenSource();
		return KIDManager._requestCancellationSource;
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x0004E9AF File Offset: 0x0004CBAF
	public static void RegisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Combine(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x0004E9D0 File Offset: 0x0004CBD0
	public static void UnregisterSessionUpdateCallback_AnyPermission(Action callback)
	{
		Debug.Log("[KID] Successfully unregistered a new callback to SessionUpdate which monitors any permission change");
		KIDManager._onSessionUpdated_AnyPermission = (Action)Delegate.Remove(KIDManager._onSessionUpdated_AnyPermission, callback);
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x0004E9F1 File Offset: 0x0004CBF1
	public static void RegisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x0004EA12 File Offset: 0x0004CC12
	public static void UnregisterSessionUpdatedCallback_VoiceChat(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Voice Chat permission");
		KIDManager._onSessionUpdated_VoiceChat = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_VoiceChat, callback);
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x0004EA33 File Offset: 0x0004CC33
	public static void RegisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x0004EA54 File Offset: 0x0004CC54
	public static void UnregisterSessionUpdatedCallback_CustomUsernames(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Custom Usernames permission");
		KIDManager._onSessionUpdated_CustomUsernames = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_CustomUsernames, callback);
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x0004EA75 File Offset: 0x0004CC75
	public static void RegisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully registered a new callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Combine(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x0004EA96 File Offset: 0x0004CC96
	public static void UnregisterSessionUpdatedCallback_PrivateRooms(Action<bool, Permission.ManagedByEnum> callback)
	{
		Debug.Log("[KID] Successfully unregistered a callback to SessionUpdate which monitors the Private Rooms permission");
		KIDManager._onSessionUpdated_PrivateRooms = (Action<bool, Permission.ManagedByEnum>)Delegate.Remove(KIDManager._onSessionUpdated_PrivateRooms, callback);
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x001262C8 File Offset: 0x001244C8
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

	// Token: 0x06002D1A RID: 11546 RVA: 0x0012645C File Offset: 0x0012465C
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

	// Token: 0x0400325A RID: 12890
	[OnEnterPlay_SetNull]
	private static KIDManager _instance;

	// Token: 0x04003260 RID: 12896
	[OnEnterPlay_SetNull]
	private static KIDIntegration _kIDIntegration;

	// Token: 0x04003261 RID: 12897
	private static CancellationTokenSource _requestCancellationSource = new CancellationTokenSource();

	// Token: 0x04003262 RID: 12898
	public const string KID_PERMISSION__VOICE_CHAT = "voice-chat";

	// Token: 0x04003263 RID: 12899
	public const string KID_PERMISSION__CUSTOM_NAMES = "custom-username";

	// Token: 0x04003264 RID: 12900
	public const string KID_PERMISSION__PRIVATE_ROOMS = "join-groups";

	// Token: 0x04003265 RID: 12901
	[OnEnterPlay_SetNull]
	private static Action _onSessionUpdated_AnyPermission;

	// Token: 0x04003266 RID: 12902
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_VoiceChat;

	// Token: 0x04003267 RID: 12903
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_CustomUsernames;

	// Token: 0x04003268 RID: 12904
	[OnEnterPlay_SetNull]
	private static Action<bool, Permission.ManagedByEnum> _onSessionUpdated_PrivateRooms;

	// Token: 0x04003269 RID: 12905
	[OnEnterPlay_SetNull]
	private static Dictionary<string, Permission> _previousPermissionSettings = new Dictionary<string, Permission>();
}
