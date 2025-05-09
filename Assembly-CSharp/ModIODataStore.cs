﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using GT_CustomMapSupportRuntime;
using KID.Model;
using ModIO;
using Steamworks;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class ModIODataStore : MonoBehaviour
{
	// Token: 0x060025CF RID: 9679 RVA: 0x000BA856 File Offset: 0x000B8A56
	private void Awake()
	{
		if (ModIODataStore.instance == null)
		{
			ModIODataStore.instance = this;
			ModIODataStore.hasInstance = true;
			return;
		}
		if (ModIODataStore.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x000BA890 File Offset: 0x000B8A90
	private void OnDestroy()
	{
		if (ModIODataStore.instance == this)
		{
			ModIODataStore.instance = null;
			ModIODataStore.hasInstance = false;
		}
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x000BA8B0 File Offset: 0x000B8AB0
	private void Update()
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			return;
		}
		if (ModIODataStore.instance.modDownloadQueue.IsNullOrEmpty<ModId>())
		{
			return;
		}
		if (ModIOUnity.IsModManagementBusy())
		{
			return;
		}
		ModId modId = ModIODataStore.instance.modDownloadQueue[0];
		ModIODataStore.DownloadMod(modId, null);
		ModIODataStore.instance.modDownloadQueue.Remove(modId);
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x000BA918 File Offset: 0x000B8B18
	public static void Initialize(Action<ModIORequestResult> callback)
	{
		ModIODataStore.CheckShouldBeDisabled();
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (ModIODataStore.hasInstance)
		{
			if (!ModIODataStore.instance.initialized)
			{
				string userID = "default";
				if (SteamManager.Initialized && SteamUser.BLoggedOn())
				{
					userID = SteamUser.GetSteamID().ToString();
				}
				ModIODataStore.instance.Initialize_Internal(userID, callback);
				return;
			}
			if (callback != null)
			{
				callback(ModIORequestResult.CreateSuccessResult());
			}
		}
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000BA98C File Offset: 0x000B8B8C
	private static void CheckShouldBeDisabled()
	{
		if (!ModIODataStore.hasInstance)
		{
			return;
		}
		ModIODataStore.disabled = false;
		bool flag = false;
		if (!GorillaServer.Instance.FeatureFlagsReady)
		{
			return;
		}
		if (!GorillaServer.Instance.UseKID())
		{
			flag = PlayFabAuthenticator.instance.GetSafety();
		}
		else if (KIDIntegration.IsReady)
		{
			AgeStatusType activeAccountStatus = KIDManager.Instance.GetActiveAccountStatus();
			if (activeAccountStatus != AgeStatusType.LEGALADULT)
			{
				if (activeAccountStatus == (AgeStatusType)0)
				{
					bool safety = PlayFabAuthenticator.instance.GetSafety();
					int userAge = KIDAgeGate.UserAge;
					if (safety || userAge < 13)
					{
						flag = true;
					}
				}
				else if (KIDManager.KIDIntegration.GetAgeFromDateOfBirth() < 13)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			ModIODataStore.disabled = true;
		}
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000BAA24 File Offset: 0x000B8C24
	private void Initialize_Internal(string userID, Action<ModIORequestResult> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		Result result = ModIOUnity.InitializeForUser(userID);
		if (result.Succeeded())
		{
			this.initialized = true;
			if (callback != null)
			{
				callback(ModIORequestResult.CreateSuccessResult());
				return;
			}
		}
		else
		{
			this.linkingTerminal.DisplayLoginError("ModIO plugin failed to initialize: " + result.message);
			if (callback != null)
			{
				callback(ModIORequestResult.CreateFailureResult(result.message));
			}
		}
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000BAA90 File Offset: 0x000B8C90
	private void HasAcceptedLatestTerms(Action<bool> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (this.initialized)
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

	// Token: 0x060025D6 RID: 9686 RVA: 0x000BAAE4 File Offset: 0x000B8CE4
	private void OnReceivedTermsOfUse(TermsOfUse terms, Action<bool> callback)
	{
		this.cachedModIOTerms = terms;
		ref TermsHash hash = this.cachedModIOTerms.hash;
		string @string = PlayerPrefs.GetString("modIOAcceptedTermsHash");
		bool obj = hash.md5hash.Equals(@string);
		if (callback != null)
		{
			callback(obj);
		}
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000BAB24 File Offset: 0x000B8D24
	private void ShowModIOTermsOfUse(Action<bool> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!this.initialized)
		{
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		if (this.modIOTermsOfUsePrefab != null)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.modIOTermsOfUsePrefab, base.transform);
			if (gameObject != null)
			{
				ModIOTermsOfUse component = gameObject.GetComponent<ModIOTermsOfUse>();
				if (component != null)
				{
					this.modIOTermsAcknowledgedCallback = callback;
					gameObject.SetActive(true);
					component.Initialize(this.cachedModIOTerms, new Action<bool>(this.OnModIOTermsOfUseAcknowledged));
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

	// Token: 0x060025D8 RID: 9688 RVA: 0x000BAC38 File Offset: 0x000B8E38
	private void OnModIOTermsOfUseAcknowledged(bool accepted)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!accepted && this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError("MOD.IO TERMS OF USE HAVE NOT BEEN ACCEPTED. YOU MUST ACCEPT THE MOD.IO TERMS OF USE TO LOGIN WITH YOUR PLATFORM CREDENTIALS OR YOU CAN LOGIN WITH AN EXISTING MOD.IO ACCOUNT BY PRESSING THE 'LINK MOD.IO ACCOUNT' BUTTON AND FOLLOWING THE INSTRUCTIONS.");
		}
		if (accepted)
		{
			PlayerPrefs.SetString("modIOAcceptedTermsHash", this.cachedModIOTerms.hash.md5hash);
		}
		Action<bool> action = this.modIOTermsAcknowledgedCallback;
		if (action != null)
		{
			action(accepted);
		}
		this.modIOTermsAcknowledgedCallback = null;
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x000BACA4 File Offset: 0x000B8EA4
	private void EnableModManagement()
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!this.modManagementEnabled)
		{
			ModIODataStore.Refresh(delegate(bool result)
			{
				if (ModIOUnity.EnableModManagement(new ModManagementEventDelegate(this.HandleModManagementEvent), ModIODataStore.allowedAutomaticModOperations).Succeeded())
				{
					this.modManagementEnabled = true;
				}
			}, false);
		}
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x000BACC8 File Offset: 0x000B8EC8
	private void HandleModManagementEvent(ModManagementEventType eventType, ModId modId, Result result)
	{
		if (ModIODataStore.disabled)
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
			ModIODataStore.instance.outdatedModCMSVersions.Remove(modId.id);
			int num;
			ModIODataStore.IsModOutdated(modId, out num);
			break;
		}
		case ModManagementEventType.Downloaded:
		case ModManagementEventType.Uninstalled:
		case ModManagementEventType.Updated:
			ModIODataStore.instance.outdatedModCMSVersions.Remove(modId.id);
			break;
		default:
			return;
		}
		GameEvents.ModIOModManagementEvent.Invoke(eventType, modId, result);
	}

	// Token: 0x060025DB RID: 9691 RVA: 0x000BAD5C File Offset: 0x000B8F5C
	public static SubscribedModStatus ConvertModManagementEventToSubscribedModStatus(ModManagementEventType eventType)
	{
		if (ModIODataStore.disabled)
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

	// Token: 0x060025DC RID: 9692 RVA: 0x000BADCC File Offset: 0x000B8FCC
	public static bool IsModOutdated(ModId modId, out int exportedVersion)
	{
		exportedVersion = -1;
		SubscribedMod subscribedMod;
		return !ModIODataStore.disabled && ModIODataStore.hasInstance && (ModIODataStore.instance.outdatedModCMSVersions.TryGetValue(modId.id, out exportedVersion) || (ModIODataStore.GetSubscribedModProfile(modId, out subscribedMod) && ModIODataStore.IsInstalledModOutdated(subscribedMod)));
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x000BAE1C File Offset: 0x000B901C
	private static bool IsInstalledModOutdated(SubscribedMod subscribedMod)
	{
		if (ModIODataStore.disabled)
		{
			return false;
		}
		if (!ModIODataStore.hasInstance)
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
			MapPackageInfo packageInfo = ModIOMapLoader.GetPackageInfo(files[0].FullName);
			if (packageInfo.customMapSupportVersion != GT_CustomMapSupportRuntime.Constants.customMapSupportVersion)
			{
				ModIODataStore.instance.outdatedModCMSVersions.Add(subscribedMod.modProfile.id.id, packageInfo.customMapSupportVersion);
				return true;
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	// Token: 0x060025DE RID: 9694 RVA: 0x000BAEBC File Offset: 0x000B90BC
	public static void Refresh(Action<bool> callback = null, bool force = false)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			if (callback != null)
			{
				callback(false);
			}
			return;
		}
		if (ModIODataStore.instance.refreshing)
		{
			ModIODataStore.instance.currentRefreshCallbacks.Add(callback);
			return;
		}
		if (force || Mathf.Approximately(0f, ModIODataStore.instance.lastRefreshTime) || Time.realtimeSinceStartup - ModIODataStore.instance.lastRefreshTime >= ModIODataStore.fetchUpdatesTimeLimitSeconds)
		{
			ModIODataStore.instance.currentRefreshCallbacks.Add(callback);
			ModIODataStore.instance.lastRefreshTime = Time.realtimeSinceStartup;
			ModIODataStore.instance.refreshing = true;
			ModIOUnity.FetchUpdates(delegate(Result result)
			{
				ModIODataStore.instance.refreshing = false;
				foreach (Action<bool> action in ModIODataStore.instance.currentRefreshCallbacks)
				{
					if (action != null)
					{
						action(true);
					}
				}
				ModIODataStore.instance.currentRefreshCallbacks.Clear();
			});
			return;
		}
		if (callback != null)
		{
			callback(false);
		}
	}

	// Token: 0x060025DF RID: 9695 RVA: 0x000BAF98 File Offset: 0x000B9198
	public static void GetModProfile(ModId modId, Action<ModIORequestResultAnd<ModProfile>> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (ModIODataStore.hasInstance)
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
		callback2(ModIORequestResultAnd<ModProfile>.CreateFailureResult("No instance of ModIODataStore exists!"));
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x000BAFEE File Offset: 0x000B91EE
	public static bool IsLoggedIn()
	{
		return !ModIODataStore.disabled && ModIODataStore.hasInstance && ModIODataStore.instance.loggedIn;
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x000BB010 File Offset: 0x000B9210
	public static void IsAuthenticated(Action<Result> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			return;
		}
		ModIOUnity.IsAuthenticated(delegate(Result result)
		{
			if (result.Succeeded())
			{
				ModIODataStore.instance.loggedIn = true;
				ModIODataStore.instance.EnableModManagement();
				GameEvents.OnModIOLoggedIn.Invoke();
			}
			else
			{
				ModIODataStore.instance.loggedIn = false;
				ModIODataStore.instance.modManagementEnabled = false;
			}
			Action<Result> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(result);
		});
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x000BB04C File Offset: 0x000B924C
	public static void LogoutFromModIO()
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance || ModIODataStore.instance.loggingIn)
		{
			return;
		}
		ModIODataStore.CancelExternalAuthentication();
		ModIODataStore.instance.loggingIn = false;
		ModIODataStore.instance.loggedIn = false;
		ModIOUnity.LogOutCurrentUser();
		ModIOUnity.DisableModManagement();
		ModIODataStore.instance.modManagementEnabled = false;
		GameEvents.OnModIOLoggedOut.Invoke();
		PlayerPrefs.SetInt("modIOLassSuccessfulAuthMethod", ModIODataStore.ModIOAuthMethod.Invalid.GetIndex<ModIODataStore.ModIOAuthMethod>());
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000BB0CC File Offset: 0x000B92CC
	public static void RequestAccountLinkCode(Action<ModIORequestResult, string, string> onGetCodeCallback, Action<ModIORequestResult> onAuthCompleteCallback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			if (onGetCodeCallback != null)
			{
				onGetCodeCallback(ModIORequestResult.CreateFailureResult("ModIODataStore instance is invalid!"), null, null);
			}
			if (onAuthCompleteCallback != null)
			{
				onAuthCompleteCallback(ModIORequestResult.CreateFailureResult("ModIODataStore instance is invalid!"));
			}
			return;
		}
		if (ModIODataStore.instance.loggingIn || ModIODataStore.instance.loggedIn)
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
		ModIODataStore.instance.loggingIn = true;
		ModIODataStore.instance.externalAuthGetCodeCallback = onGetCodeCallback;
		ModIODataStore.instance.externalAuthCallback = onAuthCompleteCallback;
		ModIOUnity.RequestExternalAuthentication(new Action<ResultAnd<ExternalAuthenticationToken>>(ModIODataStore.instance.OnRequestExternalAuthentication));
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x000BB194 File Offset: 0x000B9394
	private void OnRequestExternalAuthentication(ResultAnd<ExternalAuthenticationToken> resultAndToken)
	{
		ModIODataStore.<OnRequestExternalAuthentication>d__49 <OnRequestExternalAuthentication>d__;
		<OnRequestExternalAuthentication>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnRequestExternalAuthentication>d__.<>4__this = this;
		<OnRequestExternalAuthentication>d__.resultAndToken = resultAndToken;
		<OnRequestExternalAuthentication>d__.<>1__state = -1;
		<OnRequestExternalAuthentication>d__.<>t__builder.Start<ModIODataStore.<OnRequestExternalAuthentication>d__49>(ref <OnRequestExternalAuthentication>d__);
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x000BB1D4 File Offset: 0x000B93D4
	public static void CancelExternalAuthentication()
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			return;
		}
		if (ModIODataStore.instance.externalAuthenticationToken.task != null)
		{
			ModIODataStore.instance.externalAuthenticationToken.Cancel();
			ModIODataStore.instance.externalAuthenticationToken = default(ExternalAuthenticationToken);
			Action<ModIORequestResult> action = ModIODataStore.instance.externalAuthCallback;
			if (action != null)
			{
				action(ModIORequestResult.CreateFailureResult("External Authentication Canceled"));
			}
			ModIODataStore.instance.externalAuthCallback = null;
			ModIODataStore.instance.loggedIn = false;
			ModIODataStore.instance.loggingIn = false;
		}
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x000BB270 File Offset: 0x000B9470
	public static void RequestPlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			Action<ModIORequestResult> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(ModIORequestResult.CreateFailureResult("[ModIODataStore::RequestPlatformLogin] ModIODataStore instance is invalid!"));
			return;
		}
		else
		{
			if (!ModIODataStore.instance.loggingIn)
			{
				ModIODataStore.instance.loggingIn = true;
				ModIODataStore.IsAuthenticated(delegate(Result result)
				{
					if (!result.Succeeded())
					{
						ModIODataStore.instance.InitiatePlatformLogin(callback);
						return;
					}
					ModIODataStore.instance.loggingIn = false;
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

	// Token: 0x060025E7 RID: 9703 RVA: 0x000BB2FC File Offset: 0x000B94FC
	private void InitiatePlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIODataStore.disabled)
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
				ModIODataStore <>4__this = this;
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

	// Token: 0x060025E8 RID: 9704 RVA: 0x000BB350 File Offset: 0x000B9550
	private void ContinuePlatformLogin(Action<ModIORequestResult> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (SteamManager.Initialized)
		{
			if (this.RequestEncryptedAppTicketResponse == null)
			{
				this.RequestEncryptedAppTicketResponse = CallResult<EncryptedAppTicketResponse_t>.Create(new CallResult<EncryptedAppTicketResponse_t>.APIDispatchDelegate(this.OnRequestEncryptedAppTicketFinished));
			}
			SteamAPICall_t hAPICall = SteamUser.RequestEncryptedAppTicket(null, 0);
			this.RequestEncryptedAppTicketResponse.Set(hAPICall, null);
			this.externalAuthCallback = callback;
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

	// Token: 0x060025E9 RID: 9705 RVA: 0x000BB3D9 File Offset: 0x000B95D9
	private string GetSteamEncryptedAppTicket()
	{
		if (ModIODataStore.disabled)
		{
			return "";
		}
		Array.Resize<byte>(ref ModIODataStore.instance.ticketBlob, (int)ModIODataStore.instance.ticketSize);
		return Convert.ToBase64String(ModIODataStore.instance.ticketBlob);
	}

	// Token: 0x060025EA RID: 9706 RVA: 0x000BB418 File Offset: 0x000B9618
	private void OnRequestEncryptedAppTicketFinished(EncryptedAppTicketResponse_t response, bool bIOFailure)
	{
		if (ModIODataStore.disabled)
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
				if (!SteamUser.GetEncryptedAppTicket(this.ticketBlob, this.ticketBlob.Length, out this.ticketSize))
				{
					this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult("FAILED TO RETRIEVE 'EncryptedAppTicket'."));
					if (this.linkingTerminal != null)
					{
						this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\nFAILED TO RETRIEVE 'EncryptedAppTicket'.");
					}
					return;
				}
				ModIOUnity.AuthenticateUserViaSteam(this.GetSteamEncryptedAppTicket(), null, new TermsHash?(this.cachedModIOTerms.hash), new Action<Result>(this.OnAuthWithSteam));
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

	// Token: 0x060025EB RID: 9707 RVA: 0x000BB5E4 File Offset: 0x000B97E4
	private void OnAuthWithSteam(Result result)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (result.Succeeded())
		{
			PlayerPrefs.SetInt("modIOLassSuccessfulAuthMethod", ModIODataStore.ModIOAuthMethod.Steam.GetIndex<ModIODataStore.ModIOAuthMethod>());
			this.OnExternalAuthenticationComplete(ModIORequestResult.CreateSuccessResult());
			return;
		}
		this.OnExternalAuthenticationComplete(ModIORequestResult.CreateFailureResult(result.message));
		if (this.linkingTerminal != null)
		{
			this.linkingTerminal.DisplayLoginError("FAILED TO LOGIN TO MOD.IO VIA STEAM:\n" + result.message);
		}
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x000BB65C File Offset: 0x000B985C
	private void OnExternalAuthenticationComplete(ModIORequestResult result)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (result.success)
		{
			this.loggedIn = true;
			ModIODataStore.Refresh(null, true);
			this.EnableModManagement();
			GameEvents.OnModIOLoggedIn.Invoke();
		}
		else
		{
			this.loggedIn = false;
		}
		this.loggingIn = false;
		Action<ModIORequestResult> action = this.externalAuthCallback;
		if (action != null)
		{
			action(result);
		}
		this.externalAuthCallback = null;
	}

	// Token: 0x060025ED RID: 9709 RVA: 0x000BB6C0 File Offset: 0x000B98C0
	public static ModIODataStore.ModIOAuthMethod GetLastAuthMethod()
	{
		if (ModIODataStore.disabled)
		{
			return ModIODataStore.ModIOAuthMethod.Invalid;
		}
		int @int = PlayerPrefs.GetInt("modIOLassSuccessfulAuthMethod", -1);
		if (@int == -1)
		{
			return ModIODataStore.ModIOAuthMethod.Invalid;
		}
		return (ModIODataStore.ModIOAuthMethod)@int;
	}

	// Token: 0x060025EE RID: 9710 RVA: 0x000BB6EC File Offset: 0x000B98EC
	public static SubscribedMod[] GetSubscribedMods()
	{
		if (ModIODataStore.disabled)
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

	// Token: 0x060025EF RID: 9711 RVA: 0x000BB718 File Offset: 0x000B9918
	public static void SubscribeToMod(ModId modId, Action<Result> callback)
	{
		if (ModIODataStore.disabled)
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

	// Token: 0x060025F0 RID: 9712 RVA: 0x000BB74C File Offset: 0x000B994C
	public static void UnsubscribeFromMod(ModId modId, Action<Result> callback)
	{
		if (ModIODataStore.disabled)
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

	// Token: 0x060025F1 RID: 9713 RVA: 0x000BB780 File Offset: 0x000B9980
	public static bool GetSubscribedModStatus(ModId modId, out SubscribedModStatus modStatus)
	{
		modStatus = SubscribedModStatus.None;
		if (ModIODataStore.disabled)
		{
			return false;
		}
		if (!ModIODataStore.hasInstance)
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

	// Token: 0x060025F2 RID: 9714 RVA: 0x000BB7F0 File Offset: 0x000B99F0
	public static bool GetSubscribedModProfile(ModId modId, out SubscribedMod subscribedMod)
	{
		subscribedMod = default(SubscribedMod);
		if (ModIODataStore.disabled)
		{
			return false;
		}
		if (!ModIODataStore.hasInstance)
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

	// Token: 0x060025F3 RID: 9715 RVA: 0x000BB864 File Offset: 0x000B9A64
	public static void DownloadMod(ModId modId, Action<ModIORequestResult> callback)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
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
		if (!ModIODataStore.instance.modDownloadQueue.Contains(modId))
		{
			ModIODataStore.instance.modDownloadQueue.Add(modId);
		}
		Action<ModIORequestResult> callback2 = callback;
		if (callback2 == null)
		{
			return;
		}
		callback2(ModIORequestResult.CreateSuccessResult());
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x000BB8E3 File Offset: 0x000B9AE3
	public static bool IsModInDownloadQueue(ModId modId)
	{
		return !ModIODataStore.disabled && ModIODataStore.hasInstance && ModIODataStore.instance.modDownloadQueue.Contains(modId);
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x000BB90C File Offset: 0x000B9B0C
	public static void AbortModDownload(ModId modId)
	{
		if (ModIODataStore.disabled)
		{
			return;
		}
		if (!ModIODataStore.hasInstance)
		{
			return;
		}
		if (ModIODataStore.instance.modDownloadQueue.Contains(modId))
		{
			ModIODataStore.instance.modDownloadQueue.Remove(modId);
			return;
		}
		if (ModIOUnity.IsModManagementBusy())
		{
			ModIOUnity.AbortDownload(modId);
		}
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x000BB95E File Offset: 0x000B9B5E
	public static ModId GetNewMapsModId()
	{
		if (!ModIODataStore.hasInstance)
		{
			return ModId.Null;
		}
		return new ModId(ModIODataStore.instance.newMapsModId);
	}

	// Token: 0x040029E7 RID: 10727
	private const string modIOAcceptedTermsHashKey = "modIOAcceptedTermsHash";

	// Token: 0x040029E8 RID: 10728
	private const string modIOLastSuccessfulAuthMethodKey = "modIOLassSuccessfulAuthMethod";

	// Token: 0x040029E9 RID: 10729
	[OnEnterPlay_SetNull]
	public static volatile ModIODataStore instance;

	// Token: 0x040029EA RID: 10730
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x040029EB RID: 10731
	private static float fetchUpdatesTimeLimitSeconds = 5f;

	// Token: 0x040029EC RID: 10732
	private static List<ModManagementOperationType> allowedAutomaticModOperations = new List<ModManagementOperationType>
	{
		ModManagementOperationType.Install,
		ModManagementOperationType.Uninstall
	};

	// Token: 0x040029ED RID: 10733
	private static bool disabled;

	// Token: 0x040029EE RID: 10734
	[SerializeField]
	private GameObject modIOTermsOfUsePrefab;

	// Token: 0x040029EF RID: 10735
	[SerializeField]
	private ModIOAccountLinkingTerminal linkingTerminal;

	// Token: 0x040029F0 RID: 10736
	[SerializeField]
	private long newMapsModId;

	// Token: 0x040029F1 RID: 10737
	private float lastRefreshTime;

	// Token: 0x040029F2 RID: 10738
	private bool refreshing;

	// Token: 0x040029F3 RID: 10739
	private List<Action<bool>> currentRefreshCallbacks = new List<Action<bool>>();

	// Token: 0x040029F4 RID: 10740
	private bool initialized;

	// Token: 0x040029F5 RID: 10741
	private bool modManagementEnabled;

	// Token: 0x040029F6 RID: 10742
	private bool loggingIn;

	// Token: 0x040029F7 RID: 10743
	private bool loggedIn;

	// Token: 0x040029F8 RID: 10744
	private TermsOfUse cachedModIOTerms;

	// Token: 0x040029F9 RID: 10745
	private Action<bool> modIOTermsAcknowledgedCallback;

	// Token: 0x040029FA RID: 10746
	private List<ModId> modDownloadQueue = new List<ModId>();

	// Token: 0x040029FB RID: 10747
	private Dictionary<long, int> outdatedModCMSVersions = new Dictionary<long, int>();

	// Token: 0x040029FC RID: 10748
	private Action<ModIORequestResult> externalAuthCallback;

	// Token: 0x040029FD RID: 10749
	private ExternalAuthenticationToken externalAuthenticationToken;

	// Token: 0x040029FE RID: 10750
	private Action<ModIORequestResult, string, string> externalAuthGetCodeCallback;

	// Token: 0x040029FF RID: 10751
	private byte[] ticketBlob = new byte[1024];

	// Token: 0x04002A00 RID: 10752
	private uint ticketSize;

	// Token: 0x04002A01 RID: 10753
	protected CallResult<EncryptedAppTicketResponse_t> RequestEncryptedAppTicketResponse;

	// Token: 0x020005F2 RID: 1522
	public enum ModIOAuthMethod
	{
		// Token: 0x04002A03 RID: 10755
		Invalid,
		// Token: 0x04002A04 RID: 10756
		LinkedAccount,
		// Token: 0x04002A05 RID: 10757
		Steam,
		// Token: 0x04002A06 RID: 10758
		Oculus
	}
}
