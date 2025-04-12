using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GorillaExtensions;
using JetBrains.Annotations;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GorillaNetworking
{
	// Token: 0x02000AD8 RID: 2776
	public class PlayFabAuthenticator : MonoBehaviour
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06004576 RID: 17782 RVA: 0x0005C73F File Offset: 0x0005A93F
		public GorillaComputer gorillaComputer
		{
			get
			{
				return GorillaComputer.instance;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06004577 RID: 17783 RVA: 0x0005C748 File Offset: 0x0005A948
		// (set) Token: 0x06004578 RID: 17784 RVA: 0x0005C750 File Offset: 0x0005A950
		public bool postAuthSetSafety { get; private set; }

		// Token: 0x06004579 RID: 17785 RVA: 0x0018083C File Offset: 0x0017EA3C
		private void Awake()
		{
			if (PlayFabAuthenticator.instance == null)
			{
				PlayFabAuthenticator.instance = this;
			}
			else if (PlayFabAuthenticator.instance != this)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (PlayFabAuthenticator.instance.photonAuthenticator == null)
			{
				PlayFabAuthenticator.instance.photonAuthenticator = PlayFabAuthenticator.instance.gameObject.GetOrAddComponent<PhotonAuthenticator>();
			}
			this.platform = ScriptableObject.CreateInstance<PlatformTagJoin>();
			PlayFabSettings.CompressApiData = false;
			new byte[1];
			if (this.screenDebugMode)
			{
				this.debugText.text = "";
			}
			Debug.Log("doing steam thing");
			if (PlayFabAuthenticator.instance.steamAuthenticator == null)
			{
				PlayFabAuthenticator.instance.steamAuthenticator = PlayFabAuthenticator.instance.gameObject.GetOrAddComponent<SteamAuthenticator>();
			}
			this.platform.PlatformTag = "Steam";
			PlayFabSettings.TitleId = PlayFabAuthenticatorSettings.TitleId;
			PlayFabSettings.DisableFocusTimeCollection = true;
			if (!MothershipClientApiUnity.IsEnabled())
			{
				this.AuthenticateWithPlayFab();
				return;
			}
			if (PlayFabAuthenticator.instance.mothershipAuthenticator == null)
			{
				PlayFabAuthenticator.instance.mothershipAuthenticator = (MothershipAuthenticator.Instance ?? PlayFabAuthenticator.instance.gameObject.GetOrAddComponent<MothershipAuthenticator>());
				MothershipAuthenticator mothershipAuthenticator = PlayFabAuthenticator.instance.mothershipAuthenticator;
				mothershipAuthenticator.OnLoginSuccess = (Action)Delegate.Combine(mothershipAuthenticator.OnLoginSuccess, new Action(delegate()
				{
					PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
				}));
				MothershipAuthenticator mothershipAuthenticator2 = PlayFabAuthenticator.instance.mothershipAuthenticator;
				mothershipAuthenticator2.OnLoginAttemptFailure = (Action<int>)Delegate.Combine(mothershipAuthenticator2.OnLoginAttemptFailure, new Action<int>(delegate(int attempts)
				{
					if (attempts == 1)
					{
						PlayFabAuthenticator.instance.AuthenticateWithPlayFab();
					}
				}));
				PlayFabAuthenticator.instance.mothershipAuthenticator.BeginLoginFlow();
			}
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void Start()
		{
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x0005C759 File Offset: 0x0005A959
		private void OnEnable()
		{
			NetworkSystem.Instance.OnCustomAuthenticationResponse += this.OnCustomAuthenticationResponse;
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x0005C771 File Offset: 0x0005A971
		private void OnDisable()
		{
			NetworkSystem.Instance.OnCustomAuthenticationResponse -= this.OnCustomAuthenticationResponse;
			SteamAuthTicket steamAuthTicket = this.steamAuthTicketForPhoton;
			if (steamAuthTicket != null)
			{
				steamAuthTicket.Dispose();
			}
			SteamAuthTicket steamAuthTicket2 = this.steamAuthTicketForPlayFab;
			if (steamAuthTicket2 == null)
			{
				return;
			}
			steamAuthTicket2.Dispose();
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x0005C7AA File Offset: 0x0005A9AA
		public void RefreshSteamAuthTicketForPhoton(Action<string> successCallback, Action<EResult> failureCallback)
		{
			SteamAuthTicket steamAuthTicket = this.steamAuthTicketForPhoton;
			if (steamAuthTicket != null)
			{
				steamAuthTicket.Dispose();
			}
			this.steamAuthTicketForPhoton = this.steamAuthenticator.GetAuthTicketForWebApi(this.steamAuthIdForPhoton, successCallback, failureCallback);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x00180A18 File Offset: 0x0017EC18
		private void OnCustomAuthenticationResponse(Dictionary<string, object> response)
		{
			SteamAuthTicket steamAuthTicket = this.steamAuthTicketForPhoton;
			if (steamAuthTicket != null)
			{
				steamAuthTicket.Dispose();
			}
			object obj;
			if (response.TryGetValue("SteamAuthIdForPhoton", out obj))
			{
				string text = obj as string;
				if (text != null)
				{
					this.steamAuthIdForPhoton = text;
					return;
				}
			}
			this.steamAuthIdForPhoton = null;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x00180A60 File Offset: 0x0017EC60
		public void AuthenticateWithPlayFab()
		{
			if (!this.loginFailed)
			{
				Debug.Log("authenticating with playFab!");
				GorillaServer gorillaServer = GorillaServer.Instance;
				if (gorillaServer != null && gorillaServer.FeatureFlagsReady)
				{
					if (GorillaServer.Instance.UseKID())
					{
						this.SetSafety(true, true, false);
						Debug.Log("[KID] Safety has been set to TRUE");
					}
				}
				else
				{
					this.postAuthSetSafety = true;
				}
				if (SteamManager.Initialized)
				{
					this.userID = SteamUser.GetSteamID().ToString();
					Debug.Log("trying to auth with steam");
					this.steamAuthTicketForPlayFab = this.steamAuthenticator.GetAuthTicket(delegate(string ticket)
					{
						Debug.Log("Got steam auth session ticket!");
						PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
						{
							CreateAccount = new bool?(true),
							SteamTicket = ticket
						}, new Action<LoginResult>(this.OnLoginWithSteamResponse), new Action<PlayFabError>(this.OnPlayFabError), null, null);
					}, delegate(EResult result)
					{
						base.StartCoroutine(this.DisplayGeneralFailureMessageOnGorillaComputerAfter1Frame());
					});
					return;
				}
				base.StartCoroutine(this.DisplayGeneralFailureMessageOnGorillaComputerAfter1Frame());
			}
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x0005C7DB File Offset: 0x0005A9DB
		private IEnumerator VerifyKidAuthenticated(string playFabId, string kidAccessToken, string kidRefreshToken, string locationCode, string kidUrlBasePath)
		{
			while (!GorillaServer.Instance.FeatureFlagsReady)
			{
				yield return new WaitForSeconds(0.05f);
			}
			if (GorillaServer.Instance.UseKID())
			{
				this.SetSafety(false, true, false);
			}
			else
			{
				KIDManager.DisableKid();
			}
			yield break;
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x0005C7EA File Offset: 0x0005A9EA
		private IEnumerator DisplayGeneralFailureMessageOnGorillaComputerAfter1Frame()
		{
			yield return null;
			if (this.gorillaComputer != null)
			{
				this.gorillaComputer.GeneralFailureMessage("UNABLE TO AUTHENTICATE YOUR STEAM ACCOUNT! PLEASE MAKE SURE STEAM IS RUNNING AND YOU ARE LAUNCHING THE GAME DIRECTLY FROM STEAM.");
				this.gorillaComputer.screenText.Text = "UNABLE TO AUTHENTICATE YOUR STEAM ACCOUNT! PLEASE MAKE SURE STEAM IS RUNNING AND YOU ARE LAUNCHING THE GAME DIRECTLY FROM STEAM.";
				Debug.Log("Couldn't authenticate steam account");
			}
			else
			{
				Debug.LogError("PlayFabAuthenticator: gorillaComputer is null, so could not set GeneralFailureMessage notifying user that the steam account could not be authenticated.", this);
			}
			yield break;
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00180B28 File Offset: 0x0017ED28
		private void OnLoginWithSteamResponse(LoginResult obj)
		{
			this._playFabPlayerIdCache = obj.PlayFabId;
			this._sessionTicket = obj.SessionTicket;
			base.StartCoroutine(this.CachePlayFabId(new PlayFabAuthenticator.CachePlayFabIdRequest
			{
				Platform = this.platform.ToString(),
				PlatformUserId = this.userID,
				SessionTicket = this._sessionTicket,
				PlayFabId = this._playFabPlayerIdCache,
				TitleId = PlayFabSettings.TitleId,
				MothershipEnvId = MothershipClientApiUnity.EnvironmentId,
				MothershipToken = MothershipClientContext.Token,
				MothershipId = MothershipClientContext.MothershipId
			}, new Action<PlayFabAuthenticator.CachePlayFabIdResponse>(this.OnCachePlayFabIdRequest)));
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x00180BCC File Offset: 0x0017EDCC
		private void OnCachePlayFabIdRequest([CanBeNull] PlayFabAuthenticator.CachePlayFabIdResponse response)
		{
			if (response != null)
			{
				this.steamAuthIdForPhoton = response.SteamAuthIdForPhoton;
				base.StartCoroutine(this.VerifyKidAuthenticated(response.PlayFabId, response.KidAccessToken, response.KidRefreshToken, response.LocationCode, response.KidUrlBasePath));
				Debug.Log("Successfully cached PlayFab Id.  Continuing!");
				this.AdvanceLogin();
				return;
			}
			Debug.LogError("Could not cache PlayFab Id.  Cannot continue.");
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x0005C7F9 File Offset: 0x0005A9F9
		private void AdvanceLogin()
		{
			this.LogMessage("PlayFab authenticated ... Getting Nonce");
			this.RefreshSteamAuthTicketForPhoton(delegate(string ticket)
			{
				this._nonce = ticket;
				Debug.Log("Got nonce!  Authenticating...");
				this.AuthenticateWithPhoton();
			}, delegate(EResult result)
			{
				Debug.LogWarning("Failed to get nonce!");
				this.AuthenticateWithPhoton();
			});
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x00180C30 File Offset: 0x0017EE30
		private void AuthenticateWithPhoton()
		{
			this.photonAuthenticator.SetCustomAuthenticationParameters(new Dictionary<string, object>
			{
				{
					"AppId",
					PlayFabSettings.TitleId
				},
				{
					"AppVersion",
					NetworkSystemConfig.AppVersion ?? "-1"
				},
				{
					"Ticket",
					this._sessionTicket
				},
				{
					"Nonce",
					this._nonce
				},
				{
					"MothershipEnvId",
					MothershipClientApiUnity.EnvironmentId
				},
				{
					"MothershipToken",
					MothershipClientContext.Token
				}
			});
			this.GetPlayerDisplayName(this._playFabPlayerIdCache);
			GorillaServer.Instance.AddOrRemoveDLCOwnership(delegate(ExecuteFunctionResult result)
			{
				Debug.Log("got results! updating!");
				if (GorillaTagger.Instance != null)
				{
					GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
				}
			}, delegate(PlayFabError error)
			{
				Debug.Log("Got error retrieving user data:");
				Debug.Log(error.GenerateErrorReport());
				if (GorillaTagger.Instance != null)
				{
					GorillaTagger.Instance.offlineVRRig.GetCosmeticsPlayFabCatalogData();
				}
			});
			if (CosmeticsController.instance != null)
			{
				Debug.Log("initializing cosmetics");
				CosmeticsController.instance.Initialize();
			}
			if (this.gorillaComputer != null)
			{
				this.gorillaComputer.OnConnectedToMasterStuff();
			}
			else
			{
				base.StartCoroutine(this.ComputerOnConnectedToMaster());
			}
			if (PhotonNetworkController.Instance != null)
			{
				Debug.Log("Finish authenticating");
				NetworkSystem.Instance.FinishAuthenticating();
			}
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x0005C824 File Offset: 0x0005AA24
		private IEnumerator ComputerOnConnectedToMaster()
		{
			WaitForEndOfFrame frameYield = new WaitForEndOfFrame();
			while (this.gorillaComputer == null)
			{
				yield return frameYield;
			}
			this.gorillaComputer.OnConnectedToMasterStuff();
			yield break;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x00180D80 File Offset: 0x0017EF80
		private void OnPlayFabError(PlayFabError obj)
		{
			this.LogMessage(obj.ErrorMessage);
			Debug.Log("OnPlayFabError(): " + obj.ErrorMessage);
			this.loginFailed = true;
			if (obj.ErrorMessage == "The account making this request is currently banned")
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = obj.ErrorDetails.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return;
					}
					KeyValuePair<string, List<string>> keyValuePair = enumerator.Current;
					if (keyValuePair.Value[0] != "Indefinite")
					{
						this.gorillaComputer.GeneralFailureMessage("YOUR ACCOUNT HAS BEEN BANNED. YOU WILL NOT BE ABLE TO PLAY UNTIL THE BAN EXPIRES.\nREASON: " + keyValuePair.Key + "\nHOURS LEFT: " + ((int)((DateTime.Parse(keyValuePair.Value[0]) - DateTime.UtcNow).TotalHours + 1.0)).ToString());
						return;
					}
					this.gorillaComputer.GeneralFailureMessage("YOUR ACCOUNT HAS BEEN BANNED INDEFINITELY.\nREASON: " + keyValuePair.Key);
					return;
				}
			}
			if (obj.ErrorMessage == "The IP making this request is currently banned")
			{
				using (Dictionary<string, List<string>>.Enumerator enumerator = obj.ErrorDetails.GetEnumerator())
				{
					if (!enumerator.MoveNext())
					{
						return;
					}
					KeyValuePair<string, List<string>> keyValuePair2 = enumerator.Current;
					if (keyValuePair2.Value[0] != "Indefinite")
					{
						this.gorillaComputer.GeneralFailureMessage("THIS IP HAS BEEN BANNED. YOU WILL NOT BE ABLE TO PLAY UNTIL THE BAN EXPIRES.\nREASON: " + keyValuePair2.Key + "\nHOURS LEFT: " + ((int)((DateTime.Parse(keyValuePair2.Value[0]) - DateTime.UtcNow).TotalHours + 1.0)).ToString());
						return;
					}
					this.gorillaComputer.GeneralFailureMessage("THIS IP HAS BEEN BANNED INDEFINITELY.\nREASON: " + keyValuePair2.Key);
					return;
				}
			}
			if (this.gorillaComputer != null)
			{
				this.gorillaComputer.GeneralFailureMessage(this.gorillaComputer.unableToConnect);
			}
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void LogMessage(string message)
		{
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00180FB8 File Offset: 0x0017F1B8
		private void GetPlayerDisplayName(string playFabId)
		{
			GetPlayerProfileRequest getPlayerProfileRequest = new GetPlayerProfileRequest();
			getPlayerProfileRequest.PlayFabId = playFabId;
			getPlayerProfileRequest.ProfileConstraints = new PlayerProfileViewConstraints
			{
				ShowDisplayName = true
			};
			PlayFabClientAPI.GetPlayerProfile(getPlayerProfileRequest, delegate(GetPlayerProfileResult result)
			{
				this._displayName = result.PlayerProfile.DisplayName;
			}, delegate(PlayFabError error)
			{
				Debug.LogError(error.GenerateErrorReport());
			}, null, null);
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00181018 File Offset: 0x0017F218
		public void SetDisplayName(string playerName)
		{
			if (this._displayName == null || (this._displayName.Length > 4 && this._displayName.Substring(0, this._displayName.Length - 4) != playerName))
			{
				UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new UpdateUserTitleDisplayNameRequest();
				updateUserTitleDisplayNameRequest.DisplayName = playerName;
				PlayFabClientAPI.UpdateUserTitleDisplayName(updateUserTitleDisplayNameRequest, delegate(UpdateUserTitleDisplayNameResult result)
				{
					this._displayName = playerName;
				}, delegate(PlayFabError error)
				{
					Debug.LogError(error.GenerateErrorReport());
				}, null, null);
			}
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x0005C833 File Offset: 0x0005AA33
		public void ScreenDebug(string debugString)
		{
			Debug.Log(debugString);
			if (this.screenDebugMode)
			{
				Text text = this.debugText;
				text.text = text.text + debugString + "\n";
			}
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x0005C85F File Offset: 0x0005AA5F
		public void ScreenDebugClear()
		{
			this.debugText.text = "";
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x0005C871 File Offset: 0x0005AA71
		public IEnumerator PlayfabAuthenticate(PlayFabAuthenticator.PlayfabAuthRequestData data, Action<PlayFabAuthenticator.PlayfabAuthResponseData> callback)
		{
			UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.AuthApiBaseUrl + "/api/PlayFabAuthentication", "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
			bool retry = false;
			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
			{
				PlayFabAuthenticator.PlayfabAuthResponseData obj = JsonUtility.FromJson<PlayFabAuthenticator.PlayfabAuthResponseData>(request.downloadHandler.text);
				callback(obj);
			}
			else
			{
				if (request.responseCode == 403L)
				{
					Debug.LogError(string.Format("HTTP {0}: {1}, with body: {2}", request.responseCode, request.error, request.downloadHandler.text));
					PlayFabAuthenticator.BanInfo banInfo = JsonUtility.FromJson<PlayFabAuthenticator.BanInfo>(request.downloadHandler.text);
					this.ShowBanMessage(banInfo);
					callback(null);
				}
				if (request.result == UnityWebRequest.Result.ProtocolError && request.responseCode != 400L)
				{
					retry = true;
					Debug.LogError(string.Format("HTTP {0} error: {1} message:{2}", request.responseCode, request.error, request.downloadHandler.text));
				}
				else if (request.result == UnityWebRequest.Result.ConnectionError)
				{
					retry = true;
					Debug.LogError("NETWORK ERROR: " + request.error + "\nMessage: " + request.downloadHandler.text);
				}
				else
				{
					Debug.LogError("HTTP ERROR: " + request.error + "\nMessage: " + request.downloadHandler.text);
					retry = true;
				}
			}
			if (retry)
			{
				if (this.playFabAuthRetryCount < this.playFabMaxRetries)
				{
					int num = (int)Mathf.Pow(2f, (float)(this.playFabAuthRetryCount + 1));
					Debug.LogWarning(string.Format("Retrying PlayFab auth... Retry attempt #{0}, waiting for {1} seconds", this.playFabAuthRetryCount + 1, num));
					this.playFabAuthRetryCount++;
					yield return new WaitForSeconds((float)num);
				}
				else
				{
					Debug.LogError("Maximum retries attempted. Please check your network connection.");
					callback(null);
				}
			}
			yield break;
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x001810B8 File Offset: 0x0017F2B8
		private void ShowBanMessage(PlayFabAuthenticator.BanInfo banInfo)
		{
			try
			{
				if (banInfo.BanExpirationTime != null && banInfo.BanMessage != null)
				{
					if (banInfo.BanExpirationTime != "Indefinite")
					{
						this.gorillaComputer.GeneralFailureMessage("YOUR ACCOUNT HAS BEEN BANNED. YOU WILL NOT BE ABLE TO PLAY UNTIL THE BAN EXPIRES.\nREASON: " + banInfo.BanMessage + "\nHOURS LEFT: " + ((int)((DateTime.Parse(banInfo.BanExpirationTime) - DateTime.UtcNow).TotalHours + 1.0)).ToString());
					}
					else
					{
						this.gorillaComputer.GeneralFailureMessage("YOUR ACCOUNT HAS BEEN BANNED INDEFINITELY.\nREASON: " + banInfo.BanMessage);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x0005C88E File Offset: 0x0005AA8E
		public IEnumerator CachePlayFabId(PlayFabAuthenticator.CachePlayFabIdRequest data, Action<PlayFabAuthenticator.CachePlayFabIdResponse> callback)
		{
			Debug.Log("Trying to cache playfab Id");
			UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.AuthApiBaseUrl + "/api/CachePlayFabId", "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
			bool retry = false;
			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			yield return request.SendWebRequest();
			if (request.result != UnityWebRequest.Result.ConnectionError && request.result != UnityWebRequest.Result.ProtocolError)
			{
				if (request.responseCode == 200L)
				{
					PlayFabAuthenticator.CachePlayFabIdResponse obj = JsonUtility.FromJson<PlayFabAuthenticator.CachePlayFabIdResponse>(request.downloadHandler.text);
					callback(obj);
				}
			}
			else if (request.result == UnityWebRequest.Result.ProtocolError && request.responseCode != 400L)
			{
				retry = true;
				Debug.LogError(string.Format("HTTP {0} error: {1}", request.responseCode, request.error));
			}
			else
			{
				retry = (request.result != UnityWebRequest.Result.ConnectionError || true);
			}
			if (retry)
			{
				if (this.playFabCacheRetryCount < this.playFabCacheMaxRetries)
				{
					int num = (int)Mathf.Pow(2f, (float)(this.playFabCacheRetryCount + 1));
					Debug.LogWarning(string.Format("Retrying PlayFab auth... Retry attempt #{0}, waiting for {1} seconds", this.playFabCacheRetryCount + 1, num));
					this.playFabCacheRetryCount++;
					yield return new WaitForSeconds((float)num);
					base.StartCoroutine(this.CachePlayFabId(new PlayFabAuthenticator.CachePlayFabIdRequest
					{
						Platform = this.platform.ToString(),
						PlatformUserId = this.userID,
						SessionTicket = this._sessionTicket,
						PlayFabId = this._playFabPlayerIdCache,
						TitleId = PlayFabSettings.TitleId,
						MothershipEnvId = MothershipClientApiUnity.EnvironmentId,
						MothershipToken = MothershipClientContext.Token,
						MothershipId = MothershipClientContext.MothershipId
					}, new Action<PlayFabAuthenticator.CachePlayFabIdResponse>(this.OnCachePlayFabIdRequest)));
				}
				else
				{
					Debug.LogError("Maximum retries attempted. Please check your network connection.");
					callback(null);
				}
			}
			yield break;
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x0018116C File Offset: 0x0017F36C
		public void SetSafety(bool isSafety, bool isAutoSet, bool setPlayfab = false)
		{
			this.postAuthSetSafety = false;
			Action<bool> onSafetyUpdate = this.OnSafetyUpdate;
			if (onSafetyUpdate != null)
			{
				onSafetyUpdate(isSafety);
			}
			Debug.Log("[KID] Setting safety to: [" + isSafety.ToString() + "]");
			this.isSafeAccount = isSafety;
			this.safetyType = PlayFabAuthenticator.SafetyType.None;
			if (!isSafety)
			{
				if (isAutoSet)
				{
					PlayerPrefs.SetInt("autoSafety", 0);
				}
				else
				{
					PlayerPrefs.SetInt("optSafety", 0);
				}
				PlayerPrefs.Save();
				return;
			}
			if (isAutoSet)
			{
				PlayerPrefs.SetInt("autoSafety", 1);
				this.safetyType = PlayFabAuthenticator.SafetyType.Auto;
				return;
			}
			PlayerPrefs.SetInt("optSafety", 1);
			this.safetyType = PlayFabAuthenticator.SafetyType.OptIn;
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x0005C8AB File Offset: 0x0005AAAB
		public string GetPlayFabSessionTicket()
		{
			return this._sessionTicket;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x0005C8B3 File Offset: 0x0005AAB3
		public string GetPlayFabPlayerId()
		{
			return this._playFabPlayerIdCache;
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0005C8BB File Offset: 0x0005AABB
		public bool GetSafety()
		{
			return this.isSafeAccount;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x0005C8C3 File Offset: 0x0005AAC3
		public PlayFabAuthenticator.SafetyType GetSafetyType()
		{
			return this.safetyType;
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x0005C8CB File Offset: 0x0005AACB
		public string GetUserID()
		{
			return this.userID;
		}

		// Token: 0x0400470D RID: 18189
		public static volatile PlayFabAuthenticator instance;

		// Token: 0x0400470E RID: 18190
		private string _playFabPlayerIdCache;

		// Token: 0x0400470F RID: 18191
		private string _sessionTicket;

		// Token: 0x04004710 RID: 18192
		private string _displayName;

		// Token: 0x04004711 RID: 18193
		private string _nonce;

		// Token: 0x04004712 RID: 18194
		public string userID;

		// Token: 0x04004713 RID: 18195
		private string userToken;

		// Token: 0x04004714 RID: 18196
		public PlatformTagJoin platform;

		// Token: 0x04004715 RID: 18197
		private bool isSafeAccount;

		// Token: 0x04004716 RID: 18198
		public Action<bool> OnSafetyUpdate;

		// Token: 0x04004717 RID: 18199
		private PlayFabAuthenticator.SafetyType safetyType;

		// Token: 0x04004718 RID: 18200
		private byte[] m_Ticket;

		// Token: 0x04004719 RID: 18201
		private uint m_pcbTicket;

		// Token: 0x0400471A RID: 18202
		public Text debugText;

		// Token: 0x0400471B RID: 18203
		public bool screenDebugMode;

		// Token: 0x0400471C RID: 18204
		public bool loginFailed;

		// Token: 0x0400471D RID: 18205
		[FormerlySerializedAs("loginDisplayID")]
		public GameObject emptyObject;

		// Token: 0x0400471E RID: 18206
		private int playFabAuthRetryCount;

		// Token: 0x0400471F RID: 18207
		private int playFabMaxRetries = 5;

		// Token: 0x04004720 RID: 18208
		private int playFabCacheRetryCount;

		// Token: 0x04004721 RID: 18209
		private int playFabCacheMaxRetries = 5;

		// Token: 0x04004722 RID: 18210
		public MetaAuthenticator metaAuthenticator;

		// Token: 0x04004723 RID: 18211
		public SteamAuthenticator steamAuthenticator;

		// Token: 0x04004724 RID: 18212
		public MothershipAuthenticator mothershipAuthenticator;

		// Token: 0x04004725 RID: 18213
		public PhotonAuthenticator photonAuthenticator;

		// Token: 0x04004726 RID: 18214
		private SteamAuthTicket steamAuthTicketForPlayFab;

		// Token: 0x04004727 RID: 18215
		private SteamAuthTicket steamAuthTicketForPhoton;

		// Token: 0x04004728 RID: 18216
		private string steamAuthIdForPhoton;

		// Token: 0x02000AD9 RID: 2777
		public enum SafetyType
		{
			// Token: 0x0400472B RID: 18219
			None,
			// Token: 0x0400472C RID: 18220
			Auto,
			// Token: 0x0400472D RID: 18221
			OptIn
		}

		// Token: 0x02000ADA RID: 2778
		[Serializable]
		public class CachePlayFabIdRequest
		{
			// Token: 0x0400472E RID: 18222
			public string Platform;

			// Token: 0x0400472F RID: 18223
			public string PlatformUserId;

			// Token: 0x04004730 RID: 18224
			public string SessionTicket;

			// Token: 0x04004731 RID: 18225
			public string PlayFabId;

			// Token: 0x04004732 RID: 18226
			public string TitleId;

			// Token: 0x04004733 RID: 18227
			public string MothershipEnvId;

			// Token: 0x04004734 RID: 18228
			public string MothershipToken;

			// Token: 0x04004735 RID: 18229
			public string MothershipId;
		}

		// Token: 0x02000ADB RID: 2779
		[Serializable]
		public class PlayfabAuthRequestData
		{
			// Token: 0x04004736 RID: 18230
			public string AppId;

			// Token: 0x04004737 RID: 18231
			public string AppVersion;

			// Token: 0x04004738 RID: 18232
			public string Nonce;

			// Token: 0x04004739 RID: 18233
			public string OculusId;

			// Token: 0x0400473A RID: 18234
			public string Platform;

			// Token: 0x0400473B RID: 18235
			public string AgeCategory;

			// Token: 0x0400473C RID: 18236
			public string MothershipEnvId;

			// Token: 0x0400473D RID: 18237
			public string MothershipToken;

			// Token: 0x0400473E RID: 18238
			public string MothershipId;
		}

		// Token: 0x02000ADC RID: 2780
		[Serializable]
		public class PlayfabAuthResponseData
		{
			// Token: 0x0400473F RID: 18239
			public string SessionTicket;

			// Token: 0x04004740 RID: 18240
			public string EntityToken;

			// Token: 0x04004741 RID: 18241
			public string PlayFabId;

			// Token: 0x04004742 RID: 18242
			public string EntityId;

			// Token: 0x04004743 RID: 18243
			public string EntityType;

			// Token: 0x04004744 RID: 18244
			public string KidAccessToken;

			// Token: 0x04004745 RID: 18245
			public string KidRefreshToken;

			// Token: 0x04004746 RID: 18246
			public string KidUrlBasePath;

			// Token: 0x04004747 RID: 18247
			public string LocationCode;
		}

		// Token: 0x02000ADD RID: 2781
		[Serializable]
		public class CachePlayFabIdResponse
		{
			// Token: 0x04004748 RID: 18248
			public string PlayFabId;

			// Token: 0x04004749 RID: 18249
			public string SteamAuthIdForPhoton;

			// Token: 0x0400474A RID: 18250
			public string KidAccessToken;

			// Token: 0x0400474B RID: 18251
			public string KidRefreshToken;

			// Token: 0x0400474C RID: 18252
			public string KidUrlBasePath;

			// Token: 0x0400474D RID: 18253
			public string LocationCode;
		}

		// Token: 0x02000ADE RID: 2782
		public class BanInfo
		{
			// Token: 0x0400474E RID: 18254
			public string BanMessage;

			// Token: 0x0400474F RID: 18255
			public string BanExpirationTime;
		}
	}
}
