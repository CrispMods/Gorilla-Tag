using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using GorillaTagScripts.ModIO;
using GT_CustomMapSupportRuntime;
using KID.Model;
using ModIO;
using Steamworks;
using UnityEngine;

// Token: 0x0200067C RID: 1660
public class ModIOManager : MonoBehaviour
{
	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x0600292B RID: 10539 RVA: 0x0004BDF1 File Offset: 0x00049FF1
	public static bool IsDisabled
	{
		get
		{
			return ModIOManager.disabled;
		}
	}

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x0600292C RID: 10540 RVA: 0x0011516C File Offset: 0x0011336C
	// (remove) Token: 0x0600292D RID: 10541 RVA: 0x001151A0 File Offset: 0x001133A0
	public static event ModIOManager.ModIOEnabled ModIOEnabledEvent;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x0600292E RID: 10542 RVA: 0x001151D4 File Offset: 0x001133D4
	// (remove) Token: 0x0600292F RID: 10543 RVA: 0x00115208 File Offset: 0x00113408
	public static event ModIOManager.ModIODisabled ModIODisabledEvent;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06002930 RID: 10544 RVA: 0x0011523C File Offset: 0x0011343C
	// (remove) Token: 0x06002931 RID: 10545 RVA: 0x00115270 File Offset: 0x00113470
	public static event ModIOManager.InitializationFinished InitializationFinishedEvent;

	// Token: 0x06002932 RID: 10546 RVA: 0x0004BDF8 File Offset: 0x00049FF8
	private void Awake()
	{
		if (ModIOManager.instance == null)
		{
			ModIOManager.instance = this;
			ModIOManager.hasInstance = true;
			return;
		}
		if (ModIOManager.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x0004BE32 File Offset: 0x0004A032
	private void Start()
	{
		NetworkSystem.Instance.OnMultiplayerStarted += this.OnJoinedRoom;
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x0004BE4A File Offset: 0x0004A04A
	private void OnDestroy()
	{
		if (ModIOManager.instance == this)
		{
			ModIOManager.instance = null;
			ModIOManager.hasInstance = false;
		}
		NetworkSystem.Instance.OnMultiplayerStarted -= this.OnJoinedRoom;
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x001152A4 File Offset: 0x001134A4
	private void Update()
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		if (ModIOManager.modDownloadQueue.IsNullOrEmpty<ModId>())
		{
			return;
		}
		if (ModIOUnity.IsModManagementBusy())
		{
			return;
		}
		ModId modId = ModIOManager.modDownloadQueue[0];
		ModIOManager.DownloadMod(modId, null);
		ModIOManager.modDownloadQueue.Remove(modId);
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x0004BE7F File Offset: 0x0004A07F
	public static void PreInit()
	{
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		if (ModIOManager.preInitCoroutine != null)
		{
			return;
		}
		if (!ModIOManager.initialized)
		{
			ModIOManager.preInitCoroutine = ModIOManager.instance.StartCoroutine(ModIOManager.DelayedPreInit());
			return;
		}
		ModIOManager.InitializationFinished initializationFinishedEvent = ModIOManager.InitializationFinishedEvent;
		if (initializationFinishedEvent == null)
		{
			return;
		}
		initializationFinishedEvent();
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x0004BEBE File Offset: 0x0004A0BE
	private static IEnumerator DelayedPreInit()
	{
		ModIOManager.disabled = true;
		while (!GorillaServer.Instance.FeatureFlagsReady)
		{
			yield return null;
		}
		bool shouldBeDisabled = false;
		if (!GorillaServer.Instance.UseKID())
		{
			bool safety = PlayFabAuthenticator.instance.GetSafety();
			shouldBeDisabled = safety;
		}
		else
		{
			while (!KIDIntegration.IsReady)
			{
				yield return null;
			}
			AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
			if (activeAccountStatus != AgeStatusType.LEGALADULT)
			{
				if (activeAccountStatus == (AgeStatusType)0)
				{
					bool safety2 = PlayFabAuthenticator.instance.GetSafety();
					int userAge = KIDAgeGate.UserAge;
					if (safety2 || userAge < 13)
					{
						shouldBeDisabled = true;
					}
				}
				else if (KIDManager.KIDIntegration.GetAgeFromDateOfBirth() < 13)
				{
					shouldBeDisabled = true;
				}
			}
		}
		if (!shouldBeDisabled)
		{
			ModIOManager.disabled = false;
			ModIOManager.ModIOEnabled modIOEnabledEvent = ModIOManager.ModIOEnabledEvent;
			if (modIOEnabledEvent != null)
			{
				modIOEnabledEvent();
			}
		}
		else
		{
			ModIOManager.disabled = true;
			ModIOManager.ModIODisabled modIODisabledEvent = ModIOManager.ModIODisabledEvent;
			if (modIODisabledEvent != null)
			{
				modIODisabledEvent();
			}
		}
		ModIOManager.preInitCoroutine = null;
		yield break;
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x0004BEC6 File Offset: 0x0004A0C6
	public static void RefreshDisabledStatus()
	{
		if (!ModIOManager.hasInstance || ModIOManager.preInitCoroutine != null || ModIOManager.refreshDisabledCoroutine != null)
		{
			return;
		}
		ModIOManager.refreshDisabledCoroutine = ModIOManager.instance.StartCoroutine(ModIOManager.RefreshDisabledStatusInternal());
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x0004BEF4 File Offset: 0x0004A0F4
	private static IEnumerator RefreshDisabledStatusInternal()
	{
		if (ModIOManager.preInitCoroutine != null)
		{
			yield break;
		}
		bool previouslyDisabled = ModIOManager.disabled;
		ModIOManager.disabled = true;
		bool shouldBeDisabled = false;
		int waitForFeatureFlagsRetryCount = 0;
		while (!GorillaServer.Instance.FeatureFlagsReady)
		{
			int num = waitForFeatureFlagsRetryCount + 1;
			waitForFeatureFlagsRetryCount = num;
			if (waitForFeatureFlagsRetryCount > 4)
			{
				shouldBeDisabled = true;
				break;
			}
			yield return new WaitForSecondsRealtime(Mathf.Pow(2f, (float)waitForFeatureFlagsRetryCount));
		}
		if (!shouldBeDisabled)
		{
			if (!GorillaServer.Instance.UseKID())
			{
				bool safety = PlayFabAuthenticator.instance.GetSafety();
				shouldBeDisabled = safety;
			}
			else
			{
				int waitForKIDRetryCount = 0;
				while (!KIDIntegration.IsReady)
				{
					int num = waitForKIDRetryCount + 1;
					waitForKIDRetryCount = num;
					if (waitForKIDRetryCount > 4)
					{
						shouldBeDisabled = true;
						break;
					}
					yield return new WaitForSecondsRealtime(Mathf.Pow(2f, (float)waitForKIDRetryCount));
				}
				if (!shouldBeDisabled)
				{
					AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
					if (activeAccountStatus != AgeStatusType.LEGALADULT)
					{
						if (activeAccountStatus == (AgeStatusType)0)
						{
							bool safety2 = PlayFabAuthenticator.instance.GetSafety();
							int userAge = KIDAgeGate.UserAge;
							if (safety2 || userAge < 13)
							{
								shouldBeDisabled = true;
							}
						}
						else if (KIDManager.KIDIntegration.GetAgeFromDateOfBirth() < 13)
						{
							shouldBeDisabled = true;
						}
					}
				}
			}
		}
		if (shouldBeDisabled)
		{
			ModIOManager.disabled = true;
			if (!previouslyDisabled)
			{
				ModIOManager.ModIODisabled modIODisabledEvent = ModIOManager.ModIODisabledEvent;
				if (modIODisabledEvent != null)
				{
					modIODisabledEvent();
				}
			}
		}
		else
		{
			ModIOManager.disabled = false;
			if (previouslyDisabled)
			{
				ModIOManager.ModIOEnabled modIOEnabledEvent = ModIOManager.ModIOEnabledEvent;
				if (modIOEnabledEvent != null)
				{
					modIOEnabledEvent();
				}
			}
		}
		ModIOManager.refreshDisabledCoroutine = null;
		yield break;
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x0004BEFC File Offset: 0x0004A0FC
	public static void Initialize(Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			if (callback != null)
			{
				callback(ModIORequestResult.CreateFailureResult("MOD.IO FUNCTIONALITY IS CURRENTLY DISABLED."));
			}
			return;
		}
		if (ModIOManager.initialized && callback != null)
		{
			callback(ModIORequestResult.CreateSuccessResult());
		}
		ModIOManager.InitInternal(callback);
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x001152F8 File Offset: 0x001134F8
	private static void InitInternal(Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		string userProfileIdentifier = "default";
		if (SteamManager.Initialized && SteamUser.BLoggedOn())
		{
			userProfileIdentifier = SteamUser.GetSteamID().ToString();
		}
		Result result = ModIOUnity.InitializeForUser(userProfileIdentifier);
		if (result.Succeeded())
		{
			ModIOManager.initialized = true;
			if (callback != null)
			{
				callback(ModIORequestResult.CreateSuccessResult());
				return;
			}
		}
		else if (callback != null)
		{
			callback(ModIORequestResult.CreateFailureResult(result.message));
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x00115370 File Offset: 0x00113570
	private void HasAcceptedLatestTerms(Action<bool> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (ModIOManager.initialized)
		{
			ModIOUnity.GetTermsOfUse(delegate(ResultAnd<TermsOfUse> result)
			{
				if (result.result.Succeeded())
				{
					this.OnReceivedTermsOfUse(result.value, callback);
					return;
				}
				Action<bool> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false);
			});
			return;
		}
		Action<bool> callback2 = callback;
		if (callback2 == null)
		{
			return;
		}
		callback2(false);
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x001153C4 File Offset: 0x001135C4
	private void OnReceivedTermsOfUse(TermsOfUse terms, Action<bool> callback)
	{
		ModIOManager.cachedModIOTerms = terms;
		ref TermsHash hash = ModIOManager.cachedModIOTerms.hash;
		string @string = PlayerPrefs.GetString("modIOAcceptedTermsHash");
		bool obj = hash.md5hash.Equals(@string);
		if (callback != null)
		{
			callback(obj);
		}
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x00115404 File Offset: 0x00113604
	private void ShowModIOTermsOfUse(Action<bool> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.initialized)
		{
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		if (this.modIOTermsOfUsePrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.modIOTermsOfUsePrefab, base.transform);
			if (gameObject != null)
			{
				ModIOTermsOfUse component = gameObject.GetComponent<ModIOTermsOfUse>();
				if (component != null)
				{
					CustomMapManager.DisableTeleportHUD();
					ModIOManager.modIOTermsAcknowledgedCallback = callback;
					gameObject.SetActive(true);
					component.Initialize(ModIOManager.cachedModIOTerms, new Action<bool>(this.OnModIOTermsOfUseAcknowledged));
					return;
				}
				if (this.linkingTerminal != null)
				{
					this.linkingTerminal.DisplayLoginError("FAILED TO DISPLAY MOD.IO TERMS OF USE: \n'ModIOTermsOfUse' PREFAB DOES NOT CONTAIN A 'ModIOTermsOfUse' SCRIPT COMPONENT.");
				}
				if (callback != null)
				{
					callback(false);
					return;
				}
			}
			else
			{
				if (this.linkingTerminal != null)
				{
					this.linkingTerminal.DisplayLoginError("FAILED TO DISPLAY MOD.IO TERMS OF USE: \nFAILED TO INSTANTIATE TERMS OF USE GAME OBJECT FROM 'ModIOTermsOfUse' PREFAB");
				}
				if (callback != null)
				{
					callback(false);
					return;
				}
			}
		}
		else if (this.modIOTermsOfUsePrefab == null)
		{
			if (this.linkingTerminal != null)
			{
				this.linkingTerminal.DisplayLoginError("FAILED TO DISPLAY MOD.IO TERMS OF USE: \n`ModIOTermsOfUse` PREFAB IS SET TO NULL.");
			}
			if (callback != null)
			{
				callback(false);
			}
		}
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x0011551C File Offset: 0x0011371C
	private void OnModIOTermsOfUseAcknowledged(bool accepted)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!accepted && this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError("MOD.IO TERMS OF USE HAVE NOT BEEN ACCEPTED. YOU MUST ACCEPT THE MOD.IO TERMS OF USE TO LOGIN WITH YOUR PLATFORM CREDENTIALS OR YOU CAN LOGIN WITH AN EXISTING MOD.IO ACCOUNT BY PRESSING THE 'LINK MOD.IO ACCOUNT' BUTTON AND FOLLOWING THE INSTRUCTIONS.");
		}
		if (accepted)
		{
			PlayerPrefs.SetString("modIOAcceptedTermsHash", ModIOManager.cachedModIOTerms.hash.md5hash);
		}
		CustomMapManager.RequestEnableTeleportHUD(true);
		Action<bool> action = ModIOManager.modIOTermsAcknowledgedCallback;
		if (action != null)
		{
			action(accepted);
		}
		ModIOManager.modIOTermsAcknowledgedCallback = null;
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x0004BF34 File Offset: 0x0004A134
	private void EnableModManagement()
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.modManagementEnabled)
		{
			ModIOManager.Refresh(delegate(bool result)
			{
				if (ModIOUnity.EnableModManagement(new ModManagementEventDelegate(this.HandleModManagementEvent), ModIOManager.allowedAutomaticModOperations).Succeeded())
				{
					ModIOManager.modManagementEnabled = true;
				}
			}, false);
		}
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x0011558C File Offset: 0x0011378C
	private void HandleModManagementEvent(ModManagementEventType eventType, ModId modId, Result result)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		switch (eventType)
		{
		case ModManagementEventType.InstallStarted:
		case ModManagementEventType.InstallFailed:
		case ModManagementEventType.DownloadStarted:
		case ModManagementEventType.DownloadFailed:
		case ModManagementEventType.UninstallStarted:
		case ModManagementEventType.UninstallFailed:
		case ModManagementEventType.UpdateStarted:
		case ModManagementEventType.UpdateFailed:
			break;
		case ModManagementEventType.Installed:
		{
			ModIOManager.outdatedModCMSVersions.Remove(modId.id);
			int num;
			ModIOManager.IsModOutdated(modId, out num);
			break;
		}
		case ModManagementEventType.Downloaded:
		case ModManagementEventType.Uninstalled:
		case ModManagementEventType.Updated:
			ModIOManager.outdatedModCMSVersions.Remove(modId.id);
			break;
		default:
			return;
		}
		GameEvents.ModIOModManagementEvent.Invoke(eventType, modId, result);
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x00115614 File Offset: 0x00113814
	public static SubscribedModStatus ConvertModManagementEventToSubscribedModStatus(ModManagementEventType eventType)
	{
		if (ModIOManager.disabled)
		{
			return SubscribedModStatus.None;
		}
		switch (eventType)
		{
		case ModManagementEventType.InstallStarted:
			return SubscribedModStatus.Installing;
		case ModManagementEventType.Installed:
			return SubscribedModStatus.Installed;
		case ModManagementEventType.InstallFailed:
			return SubscribedModStatus.ProblemOccurred;
		case ModManagementEventType.DownloadStarted:
			return SubscribedModStatus.Downloading;
		case ModManagementEventType.Downloaded:
			return SubscribedModStatus.Downloading;
		case ModManagementEventType.DownloadFailed:
			return SubscribedModStatus.ProblemOccurred;
		case ModManagementEventType.UninstallStarted:
			return SubscribedModStatus.Uninstalling;
		case ModManagementEventType.Uninstalled:
			return SubscribedModStatus.None;
		case ModManagementEventType.UninstallFailed:
			return SubscribedModStatus.ProblemOccurred;
		case ModManagementEventType.UpdateStarted:
			return SubscribedModStatus.Updating;
		case ModManagementEventType.Updated:
			return SubscribedModStatus.Installed;
		case ModManagementEventType.UpdateFailed:
			return SubscribedModStatus.ProblemOccurred;
		default:
			return SubscribedModStatus.None;
		}
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x00115684 File Offset: 0x00113884
	public static bool IsModOutdated(ModId modId, out int exportedVersion)
	{
		exportedVersion = -1;
		SubscribedMod subscribedMod;
		return !ModIOManager.disabled && ModIOManager.hasInstance && (ModIOManager.outdatedModCMSVersions.TryGetValue(modId.id, out exportedVersion) || (ModIOManager.GetSubscribedModProfile(modId, out subscribedMod) && ModIOManager.IsInstalledModOutdated(subscribedMod)));
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x001156D0 File Offset: 0x001138D0
	private static bool IsInstalledModOutdated(SubscribedMod subscribedMod)
	{
		if (ModIOManager.disabled)
		{
			return false;
		}
		if (!ModIOManager.hasInstance)
		{
			return false;
		}
		if (subscribedMod.status != SubscribedModStatus.Installed)
		{
			return false;
		}
		FileInfo[] files = new DirectoryInfo(subscribedMod.directory).GetFiles("package.json");
		try
		{
			MapPackageInfo packageInfo = CustomMapLoader.GetPackageInfo(files[0].FullName);
			if (packageInfo.customMapSupportVersion != GT_CustomMapSupportRuntime.Constants.customMapSupportVersion)
			{
				ModIOManager.outdatedModCMSVersions.Add(subscribedMod.modProfile.id.id, packageInfo.customMapSupportVersion);
				return true;
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x00115768 File Offset: 0x00113968
	public static void Refresh(Action<bool> callback = null, bool force = false)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		if (ModIOManager.refreshing)
		{
			ModIOManager.currentRefreshCallbacks.Add(callback);
			return;
		}
		if (force || Mathf.Approximately(0f, ModIOManager.lastRefreshTime) || Time.realtimeSinceStartup - ModIOManager.lastRefreshTime >= 5f)
		{
			ModIOManager.currentRefreshCallbacks.Add(callback);
			ModIOManager.lastRefreshTime = Time.realtimeSinceStartup;
			ModIOManager.refreshing = true;
			ModIOUnity.FetchUpdates(delegate(Result result)
			{
				ModIOManager.refreshing = false;
				foreach (Action<bool> action in ModIOManager.currentRefreshCallbacks)
				{
					if (action != null)
					{
						action(true);
					}
				}
				ModIOManager.currentRefreshCallbacks.Clear();
			});
			return;
		}
		if (callback != null)
		{
			callback(false);
		}
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x00115814 File Offset: 0x00113A14
	public static void GetModProfile(ModId modId, Action<ModIORequestResultAnd<ModProfile>> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (ModIOManager.hasInstance)
		{
			ModIOUnity.GetMod(modId, delegate(ResultAnd<ModProfile> result)
			{
				if (!result.result.Succeeded())
				{
					Action<ModIORequestResultAnd<ModProfile>> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(ModIORequestResultAnd<ModProfile>.CreateFailureResult(result.result.message));
					return;
				}
				else
				{
					Action<ModIORequestResultAnd<ModProfile>> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(ModIORequestResultAnd<ModProfile>.CreateSuccessResult(result.value));
					return;
				}
			});
			return;
		}
		Action<ModIORequestResultAnd<ModProfile>> callback2 = callback;
		if (callback2 == null)
		{
			return;
		}
		callback2(ModIORequestResultAnd<ModProfile>.CreateFailureResult("MODIODATASTORE INSTANCE DOES NOT EXIST!"));
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x0004BF57 File Offset: 0x0004A157
	public static bool IsLoggedIn()
	{
		return !ModIOManager.disabled && ModIOManager.hasInstance && ModIOManager.loggedIn;
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x0011586C File Offset: 0x00113A6C
	public static void IsAuthenticated(Action<Result> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		ModIOUnity.IsAuthenticated(delegate(Result result)
		{
			if (result.Succeeded())
			{
				ModIOManager.loggedIn = true;
				ModIOManager.instance.EnableModManagement();
				GameEvents.OnModIOLoggedIn.Invoke();
			}
			else
			{
				ModIOManager.loggedIn = false;
				ModIOManager.modManagementEnabled = false;
			}
			Action<Result> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(result);
		});
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x001158A8 File Offset: 0x00113AA8
	public static void LogoutFromModIO()
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance || ModIOManager.loggingIn)
		{
			return;
		}
		ModIOManager.CancelExternalAuthentication();
		ModIOManager.loggingIn = false;
		ModIOManager.loggedIn = false;
		ModIOUnity.LogOutCurrentUser();
		ModIOUnity.DisableModManagement();
		ModIOManager.modManagementEnabled = false;
		GameEvents.OnModIOLoggedOut.Invoke();
		PlayerPrefs.SetInt("modIOLassSuccessfulAuthMethod", ModIOManager.ModIOAuthMethod.Invalid.GetIndex<ModIOManager.ModIOAuthMethod>());
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x0011590C File Offset: 0x00113B0C
	public static void RequestAccountLinkCode(Action<ModIORequestResult, string, string> onGetCodeCallback, Action<ModIORequestResult> onAuthCompleteCallback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			if (onGetCodeCallback != null)
			{
				onGetCodeCallback(ModIORequestResult.CreateFailureResult("MODIODATASTORE INSTANCE DOES NOT EXIST!"), null, null);
			}
			if (onAuthCompleteCallback != null)
			{
				onAuthCompleteCallback(ModIORequestResult.CreateFailureResult("MODIODATASTORE INSTANCE DOES NOT EXIST!"));
			}
			return;
		}
		if (ModIOManager.loggingIn || ModIOManager.loggedIn)
		{
			if (onGetCodeCallback != null)
			{
				onGetCodeCallback(ModIORequestResult.CreateFailureResult("YOU MUST BE LOGGED OUT OF MOD.IO TO LINK AN EXISTING ACCOUNT."), null, null);
			}
			if (onAuthCompleteCallback != null)
			{
				onAuthCompleteCallback(ModIORequestResult.CreateFailureResult("YOU MUST BE LOGGED OUT OF MOD.IO TO LINK AN EXISTING ACCOUNT."));
			}
			return;
		}
		ModIOManager.loggingIn = true;
		ModIOManager.externalAuthGetCodeCallback = onGetCodeCallback;
		ModIOManager.externalAuthCallback = onAuthCompleteCallback;
		ModIOUnity.RequestExternalAuthentication(new Action<ResultAnd<ExternalAuthenticationToken>>(ModIOManager.instance.OnRequestExternalAuthentication));
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x001159B4 File Offset: 0x00113BB4
	private void OnRequestExternalAuthentication(ResultAnd<ExternalAuthenticationToken> resultAndToken)
	{
		ModIOManager.<OnRequestExternalAuthentication>d__69 <OnRequestExternalAuthentication>d__;
		<OnRequestExternalAuthentication>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnRequestExternalAuthentication>d__.<>4__this = this;
		<OnRequestExternalAuthentication>d__.resultAndToken = resultAndToken;
		<OnRequestExternalAuthentication>d__.<>1__state = -1;
		<OnRequestExternalAuthentication>d__.<>t__builder.Start<ModIOManager.<OnRequestExternalAuthentication>d__69>(ref <OnRequestExternalAuthentication>d__);
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x001159F4 File Offset: 0x00113BF4
	public static void CancelExternalAuthentication()
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		if (ModIOManager.externalAuthenticationToken.task != null)
		{
			ModIOManager.externalAuthenticationToken.Cancel();
			ModIOManager.externalAuthenticationToken = default(ExternalAuthenticationToken);
			Action<ModIORequestResult> action = ModIOManager.externalAuthCallback;
			if (action != null)
			{
				action(ModIORequestResult.CreateFailureResult("AUTHENTICATION CANCELED"));
			}
			ModIOManager.externalAuthCallback = null;
			ModIOManager.loggedIn = false;
			ModIOManager.loggingIn = false;
		}
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x00115A60 File Offset: 0x00113C60
	public static void RequestPlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			Action<ModIORequestResult> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(ModIORequestResult.CreateFailureResult("MODIODATASTORE INSTANCE DOES NOT EXIST!"));
			return;
		}
		else
		{
			if (!ModIOManager.loggingIn)
			{
				ModIOManager.loggingIn = true;
				ModIOManager.IsAuthenticated(delegate(Result result)
				{
					if (!result.Succeeded())
					{
						ModIOManager.instance.InitiatePlatformLogin(callback);
						return;
					}
					ModIOManager.loggingIn = false;
					Action<ModIORequestResult> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(ModIORequestResult.CreateSuccessResult());
				});
				return;
			}
			Action<ModIORequestResult> callback3 = callback;
			if (callback3 == null)
			{
				return;
			}
			callback3(ModIORequestResult.CreateFailureResult("LOGIN ALREADY IN PROGRESS"));
			return;
		}
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x00115AE0 File Offset: 0x00113CE0
	private void InitiatePlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (this.linkingTerminal != null)
		{
			this.linkingTerminal.NotifyLoggingIn();
		}
		Action<bool> <>9__1;
		this.HasAcceptedLatestTerms(delegate(bool termsAlreadyAccepted)
		{
			if (!termsAlreadyAccepted)
			{
				ModIOManager <>4__this = this;
				Action<bool> callback2;
				if ((callback2 = <>9__1) == null)
				{
					callback2 = (<>9__1 = delegate(bool termsAccepted)
					{
						if (!termsAccepted)
						{
							ModIORequestResult obj = ModIORequestResult.CreateFailureResult("MOD.IO TERMS OF USE HAVE NOT BEEN ACCEPTED, CANNOT PERFORM PLATFORM LOGIN.");
							Action<ModIORequestResult> callback3 = callback;
							if (callback3 != null)
							{
								callback3(obj);
							}
							this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("MOD.IO TERMS OF USE HAVE NOT BEEN ACCEPTED, CANNOT PERFORM PLATFORM LOGIN."));
							return;
						}
						this.ContinuePlatformLogin(callback);
					});
				}
				<>4__this.ShowModIOTermsOfUse(callback2);
				return;
			}
			this.ContinuePlatformLogin(callback);
		});
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x00115B34 File Offset: 0x00113D34
	private void ContinuePlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (SteamManager.Initialized)
		{
			if (ModIOManager.RequestEncryptedAppTicketResponse == null)
			{
				ModIOManager.RequestEncryptedAppTicketResponse = CallResult<EncryptedAppTicketResponse_t>.Create(new CallResult<EncryptedAppTicketResponse_t>.APIDispatchDelegate(this.OnRequestEncryptedAppTicketFinished));
			}
			SteamAPICall_t hAPICall = SteamUser.RequestEncryptedAppTicket(null, 0);
			ModIOManager.RequestEncryptedAppTicketResponse.Set(hAPICall, null);
			ModIOManager.externalAuthCallback = callback;
			return;
		}
		if (this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nSTEAM IS ENABLED BUT NOT INITIALIZED.");
		}
		if (callback != null)
		{
			callback(ModIORequestResult.CreateFailureResult("STEAM IS ENABLED BUT NOT INITIALIZED"));
		}
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x0004BF70 File Offset: 0x0004A170
	private string GetSteamEncryptedAppTicket()
	{
		if (ModIOManager.disabled)
		{
			return "";
		}
		Array.Resize<byte>(ref ModIOManager.ticketBlob, (int)ModIOManager.ticketSize);
		return Convert.ToBase64String(ModIOManager.ticketBlob);
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x00115BBC File Offset: 0x00113DBC
	private void OnRequestEncryptedAppTicketFinished(EncryptedAppTicketResponse_t response, bool bIOFailure)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (bIOFailure)
		{
			this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("FAILED TO RETRIEVE 'EncryptedAppTicket' DUE TO A STEAM API IO FAILURE."));
			if (this.linkingTerminal != null)
			{
				this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nFAILED TO RETRIEVE 'EncryptedAppTicket' DUE TO A STEAM API IO FAILURE.");
			}
			return;
		}
		EResult eResult = response.m_eResult;
		if (eResult <= EResult.k_EResultNoConnection)
		{
			if (eResult != EResult.k_EResultOK)
			{
				if (eResult == EResult.k_EResultNoConnection)
				{
					this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("NOT CONNECTED TO STEAM."));
					if (this.linkingTerminal != null)
					{
						this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nNOT CONNECTED TO STEAM.");
						return;
					}
					return;
				}
			}
			else
			{
				if (!SteamUser.GetEncryptedAppTicket(ModIOManager.ticketBlob, ModIOManager.ticketBlob.Length, out ModIOManager.ticketSize))
				{
					this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("FAILED TO RETRIEVE 'EncryptedAppTicket'."));
					if (this.linkingTerminal != null)
					{
						this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nFAILED TO RETRIEVE 'EncryptedAppTicket'.");
					}
					return;
				}
				ModIOUnity.AuthenticateUserViaSteam(this.GetSteamEncryptedAppTicket(), null, new TermsHash?(ModIOManager.cachedModIOTerms.hash), new Action<Result>(this.OnAuthWithSteam));
				return;
			}
		}
		else if (eResult != EResult.k_EResultLimitExceeded)
		{
			if (eResult == EResult.k_EResultDuplicateRequest)
			{
				this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("THERE IS ALREADY AN 'EncryptedAppTicket' REQUEST IN PROGRESS."));
				if (this.linkingTerminal != null)
				{
					this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nTHERE IS ALREADY AN 'EncryptedAppTicket' REQUEST IN PROGRESS.");
					return;
				}
				return;
			}
		}
		else
		{
			this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("RATE LIMIT EXCEEDED, CAN ONLY REQUEST ONE 'EncryptedAppTicket' PER MINUTE."));
			if (this.linkingTerminal != null)
			{
				this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nRATE LIMIT EXCEEDED, CAN ONLY REQUEST ONE 'EncryptedAppTicket' PER MINUTE.");
				return;
			}
			return;
		}
		this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult(string.Format("ERROR: {0}", response.m_eResult)));
		if (this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError(string.Format("FAILED TO LOGIN TO MOD.IO VIA STEAM:\n{0}", response.m_eResult));
		}
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x00115D84 File Offset: 0x00113F84
	private void OnAuthWithSteam(Result result)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (result.Succeeded())
		{
			PlayerPrefs.SetInt("modIOLassSuccessfulAuthMethod", ModIOManager.ModIOAuthMethod.Steam.GetIndex<ModIOManager.ModIOAuthMethod>());
			this.OnExternalAuthenticationComplete(ModIORequestResult.CreateSuccessResult());
			return;
		}
		this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult(result.message));
		if (this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\n" + result.message);
		}
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x00115DFC File Offset: 0x00113FFC
	private void OnExternalAuthenticationComplete(ModIORequestResult result)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (result.success)
		{
			ModIOManager.loggedIn = true;
			ModIOManager.Refresh(null, true);
			this.EnableModManagement();
			GameEvents.OnModIOLoggedIn.Invoke();
		}
		else
		{
			ModIOManager.loggedIn = false;
		}
		ModIOManager.loggingIn = false;
		Action<ModIORequestResult> action = ModIOManager.externalAuthCallback;
		if (action != null)
		{
			action(result);
		}
		ModIOManager.externalAuthCallback = null;
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x00115E5C File Offset: 0x0011405C
	public static ModIOManager.ModIOAuthMethod GetLastAuthMethod()
	{
		if (ModIOManager.disabled)
		{
			return ModIOManager.ModIOAuthMethod.Invalid;
		}
		int @int = PlayerPrefs.GetInt("modIOLassSuccessfulAuthMethod", -1);
		if (@int == -1)
		{
			return ModIOManager.ModIOAuthMethod.Invalid;
		}
		return (ModIOManager.ModIOAuthMethod)@int;
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x00115E88 File Offset: 0x00114088
	public static SubscribedMod[] GetSubscribedMods()
	{
		if (ModIOManager.disabled)
		{
			return null;
		}
		Result result;
		SubscribedMod[] subscribedMods = ModIOUnity.GetSubscribedMods(out result);
		if (result.Succeeded())
		{
			return subscribedMods;
		}
		return null;
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x00115EB4 File Offset: 0x001140B4
	public static void SubscribeToMod(ModId modId, Action<Result> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		ModIOUnity.SubscribeToMod(modId, delegate(Result result)
		{
			Action<Result> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(result);
		});
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x00115EE8 File Offset: 0x001140E8
	public static void UnsubscribeFromMod(ModId modId, Action<Result> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		ModIOUnity.UnsubscribeFromMod(modId, delegate(Result result)
		{
			Action<Result> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(result);
		});
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x00115F1C File Offset: 0x0011411C
	public static bool GetSubscribedModStatus(ModId modId, out SubscribedModStatus modStatus)
	{
		modStatus = SubscribedModStatus.None;
		if (ModIOManager.disabled)
		{
			return false;
		}
		if (!ModIOManager.hasInstance)
		{
			return false;
		}
		Result result;
		SubscribedMod[] subscribedMods = ModIOUnity.GetSubscribedMods(out result);
		if (!result.Succeeded())
		{
			return false;
		}
		foreach (SubscribedMod subscribedMod in subscribedMods)
		{
			if (subscribedMod.modProfile.id.Equals(modId))
			{
				modStatus = subscribedMod.status;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x00115F8C File Offset: 0x0011418C
	public static bool GetSubscribedModProfile(ModId modId, out SubscribedMod subscribedMod)
	{
		subscribedMod = default(SubscribedMod);
		if (ModIOManager.disabled)
		{
			return false;
		}
		if (!ModIOManager.hasInstance)
		{
			return false;
		}
		Result result;
		SubscribedMod[] subscribedMods = ModIOUnity.GetSubscribedMods(out result);
		if (!result.Succeeded())
		{
			return false;
		}
		foreach (SubscribedMod subscribedMod2 in subscribedMods)
		{
			if (subscribedMod2.modProfile.id.Equals(modId))
			{
				subscribedMod = subscribedMod2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x00116000 File Offset: 0x00114200
	public static void DownloadMod(ModId modId, Action<ModIORequestResult> callback)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		if (!ModIOUnity.IsModManagementBusy())
		{
			ModIOUnity.DownloadNow(modId, delegate(Result result)
			{
				if (result.Succeeded())
				{
					Action<ModIORequestResult> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(ModIORequestResult.CreateSuccessResult());
					return;
				}
				else
				{
					Action<ModIORequestResult> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(ModIORequestResult.CreateFailureResult(result.message));
					return;
				}
			});
			return;
		}
		if (!ModIOManager.modDownloadQueue.Contains(modId))
		{
			ModIOManager.modDownloadQueue.Add(modId);
		}
		Action<ModIORequestResult> callback2 = callback;
		if (callback2 == null)
		{
			return;
		}
		callback2(ModIORequestResult.CreateSuccessResult());
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x0004BF98 File Offset: 0x0004A198
	public static bool IsModInDownloadQueue(ModId modId)
	{
		return !ModIOManager.disabled && ModIOManager.hasInstance && ModIOManager.modDownloadQueue.Contains(modId);
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x0004BFB7 File Offset: 0x0004A1B7
	public static void AbortModDownload(ModId modId)
	{
		if (ModIOManager.disabled)
		{
			return;
		}
		if (!ModIOManager.hasInstance)
		{
			return;
		}
		if (ModIOManager.modDownloadQueue.Contains(modId))
		{
			ModIOManager.modDownloadQueue.Remove(modId);
			return;
		}
		if (ModIOUnity.IsModManagementBusy())
		{
			ModIOUnity.AbortDownload(modId);
		}
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x00116074 File Offset: 0x00114274
	private void OnJoinedRoom()
	{
		if (ModIOManager.disabled || (NetworkSystem.Instance.RoomName.Contains(GorillaComputer.instance.VStumpRoomPrepend) && !GorillaComputer.instance.IsPlayerInVirtualStump() && !CustomMapManager.IsLocalPlayerInVirtualStump()))
		{
			Debug.LogError("[ModIOManager::OnJoinedRoom] Player joined @ room while not in the VStump! Leaving the room...");
			NetworkSystem.Instance.ReturnToSinglePlayer();
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x0004BFF0 File Offset: 0x0004A1F0
	public static ModId GetNewMapsModId()
	{
		if (!ModIOManager.hasInstance)
		{
			return ModId.Null;
		}
		return new ModId(ModIOManager.instance.newMapsModId);
	}

	// Token: 0x04002E82 RID: 11906
	private const string MODIO_ACCEPTED_TERMS_KEY = "modIOAcceptedTermsHash";

	// Token: 0x04002E83 RID: 11907
	private const string MODIO_LAST_AUTH_METHOD_KEY = "modIOLassSuccessfulAuthMethod";

	// Token: 0x04002E84 RID: 11908
	private const float REFRESH_RATE_LIMIT = 5f;

	// Token: 0x04002E85 RID: 11909
	[OnEnterPlay_SetNull]
	private static volatile ModIOManager instance;

	// Token: 0x04002E86 RID: 11910
	[OnEnterPlay_Set(false)]
	private static bool hasInstance;

	// Token: 0x04002E87 RID: 11911
	private static readonly List<ModManagementOperationType> allowedAutomaticModOperations = new List<ModManagementOperationType>
	{
		ModManagementOperationType.Install,
		ModManagementOperationType.Uninstall
	};

	// Token: 0x04002E88 RID: 11912
	private static bool disabled = true;

	// Token: 0x04002E89 RID: 11913
	private static bool initialized;

	// Token: 0x04002E8A RID: 11914
	private static bool refreshing;

	// Token: 0x04002E8B RID: 11915
	private static bool modManagementEnabled;

	// Token: 0x04002E8C RID: 11916
	private static bool loggingIn;

	// Token: 0x04002E8D RID: 11917
	private static bool loggedIn;

	// Token: 0x04002E8E RID: 11918
	private static Coroutine preInitCoroutine;

	// Token: 0x04002E8F RID: 11919
	private static Coroutine refreshDisabledCoroutine;

	// Token: 0x04002E90 RID: 11920
	private static float lastRefreshTime;

	// Token: 0x04002E91 RID: 11921
	private static List<Action<bool>> currentRefreshCallbacks = new List<Action<bool>>();

	// Token: 0x04002E92 RID: 11922
	private static TermsOfUse cachedModIOTerms;

	// Token: 0x04002E93 RID: 11923
	private static Action<bool> modIOTermsAcknowledgedCallback;

	// Token: 0x04002E94 RID: 11924
	private static List<ModId> modDownloadQueue = new List<ModId>();

	// Token: 0x04002E95 RID: 11925
	private static Dictionary<long, int> outdatedModCMSVersions = new Dictionary<long, int>();

	// Token: 0x04002E96 RID: 11926
	private static Action<ModIORequestResult> externalAuthCallback;

	// Token: 0x04002E97 RID: 11927
	private static ExternalAuthenticationToken externalAuthenticationToken;

	// Token: 0x04002E98 RID: 11928
	private static Action<ModIORequestResult, string, string> externalAuthGetCodeCallback;

	// Token: 0x04002E99 RID: 11929
	private static byte[] ticketBlob = new byte[1024];

	// Token: 0x04002E9A RID: 11930
	private static uint ticketSize;

	// Token: 0x04002E9B RID: 11931
	protected static CallResult<EncryptedAppTicketResponse_t> RequestEncryptedAppTicketResponse = null;

	// Token: 0x04002E9C RID: 11932
	[SerializeField]
	private GameObject modIOTermsOfUsePrefab;

	// Token: 0x04002E9D RID: 11933
	[SerializeField]
	private ModIOAccountLinkingTerminal linkingTerminal;

	// Token: 0x04002E9E RID: 11934
	[SerializeField]
	private long newMapsModId;

	// Token: 0x0200067D RID: 1661
	public enum ModIOAuthMethod
	{
		// Token: 0x04002EA3 RID: 11939
		Invalid,
		// Token: 0x04002EA4 RID: 11940
		LinkedAccount,
		// Token: 0x04002EA5 RID: 11941
		Steam,
		// Token: 0x04002EA6 RID: 11942
		Oculus
	}

	// Token: 0x0200067E RID: 1662
	// (Invoke) Token: 0x06002963 RID: 10595
	public delegate void ModIOEnabled();

	// Token: 0x0200067F RID: 1663
	// (Invoke) Token: 0x06002967 RID: 10599
	public delegate void ModIODisabled();

	// Token: 0x02000680 RID: 1664
	// (Invoke) Token: 0x0600296B RID: 10603
	public delegate void InitializationFinished();
}
