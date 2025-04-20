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
	// Token: 0x02000B02 RID: 2818
	public class PlayFabAuthenticator : MonoBehaviour
	{
		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x0005E13E File Offset: 0x0005C33E
		public GorillaComputer gorillaComputer
		{
			get
			{
				return GorillaComputer.instance;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x0005E147 File Offset: 0x0005C347
		// (set) Token: 0x060046B4 RID: 18100 RVA: 0x0005E14F File Offset: 0x0005C34F
		public bool postAuthSetSafety { get; private set; }

		// Token: 0x060046B5 RID: 18101 RVA: 0x00187748 File Offset: 0x00185948
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

		// Token: 0x060046B6 RID: 18102 RVA: 0x00030607 File Offset: 0x0002E807
		private void Start()
		{
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0005E158 File Offset: 0x0005C358
		private void OnEnable()
		{
			NetworkSystem.Instance.OnCustomAuthenticationResponse += this.OnCustomAuthenticationResponse;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x0005E170 File Offset: 0x0005C370
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

		// Token: 0x060046B9 RID: 18105 RVA: 0x0005E1A9 File Offset: 0x0005C3A9
		public void RefreshSteamAuthTicketForPhoton(Action<string> successCallback, Action<EResult> failureCallback)
		{
			SteamAuthTicket steamAuthTicket = this.steamAuthTicketForPhoton;
			if (steamAuthTicket != null)
			{
				steamAuthTicket.Dispose();
			}
			this.steamAuthTicketForPhoton = this.steamAuthenticator.GetAuthTicketForWebApi(this.steamAuthIdForPhoton, successCallback, failureCallback);
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x00187924 File Offset: 0x00185B24
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

		// Token: 0x060046BB RID: 18107 RVA: 0x0018796C File Offset: 0x00185B6C
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

		// Token: 0x060046BC RID: 18108 RVA: 0x0005E1DA File Offset: 0x0005C3DA
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

		// Token: 0x060046BD RID: 18109 RVA: 0x0005E1E9 File Offset: 0x0005C3E9
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

		// Token: 0x060046BE RID: 18110 RVA: 0x00187A34 File Offset: 0x00185C34
		private void OnLoginWithSteamResponse(LoginResult obj)
		{
			this._playFabPlayerIdCache = obj.PlayFabId;
			this._sessionTicket = obj.SessionTicket;
			base.StartCoroutine(this.CachePlayFabId(new PlayFabAuthenticator.CachePlayFabIdRequest
			{
				Platform = this.platform.ToString(),
				SessionTicket = this._sessionTicket,
				PlayFabId = this._playFabPlayerIdCache,
				TitleId = PlayFabSettings.TitleId,
				MothershipEnvId = MothershipClientApiUnity.EnvironmentId,
				MothershipToken = MothershipClientContext.Token,
				MothershipId = MothershipClientContext.MothershipId
			}, new Action<PlayFabAuthenticator.CachePlayFabIdResponse>(this.OnCachePlayFabIdRequest)));
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x00187ACC File Offset: 0x00185CCC
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

		// Token: 0x060046C0 RID: 18112 RVA: 0x0005E1F8 File Offset: 0x0005C3F8
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

		// Token: 0x060046C1 RID: 18113 RVA: 0x00187B30 File Offset: 0x00185D30
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
			ModIOManager.PreInit();
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0005E223 File Offset: 0x0005C423
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

		// Token: 0x060046C3 RID: 18115 RVA: 0x00187C84 File Offset: 0x00185E84
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

		// Token: 0x060046C4 RID: 18116 RVA: 0x00030607 File Offset: 0x0002E807
		private void LogMessage(string message)
		{
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x00187EBC File Offset: 0x001860BC
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

		// Token: 0x060046C6 RID: 18118 RVA: 0x00187F1C File Offset: 0x0018611C
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

		// Token: 0x060046C7 RID: 18119 RVA: 0x0005E232 File Offset: 0x0005C432
		public void ScreenDebug(string debugString)
		{
			Debug.Log(debugString);
			if (this.screenDebugMode)
			{
				Text text = this.debugText;
				text.text = text.text + debugString + "\n";
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0005E25E File Offset: 0x0005C45E
		public void ScreenDebugClear()
		{
			this.debugText.text = "";
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x0005E270 File Offset: 0x0005C470
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

		// Token: 0x060046CA RID: 18122 RVA: 0x00187FBC File Offset: 0x001861BC
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

		// Token: 0x060046CB RID: 18123 RVA: 0x0005E28D File Offset: 0x0005C48D
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

		// Token: 0x060046CC RID: 18124 RVA: 0x00188070 File Offset: 0x00186270
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

		// Token: 0x060046CD RID: 18125 RVA: 0x0005E2AA File Offset: 0x0005C4AA
		public string GetPlayFabSessionTicket()
		{
			return this._sessionTicket;
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x0005E2B2 File Offset: 0x0005C4B2
		public string GetPlayFabPlayerId()
		{
			return this._playFabPlayerIdCache;
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0005E2BA File Offset: 0x0005C4BA
		public bool GetSafety()
		{
			return this.isSafeAccount;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x0005E2C2 File Offset: 0x0005C4C2
		public PlayFabAuthenticator.SafetyType GetSafetyType()
		{
			return this.safetyType;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0005E2CA File Offset: 0x0005C4CA
		public string GetUserID()
		{
			return this.userID;
		}

		// Token: 0x040047F2 RID: 18418
		public static volatile PlayFabAuthenticator instance;

		// Token: 0x040047F3 RID: 18419
		private string _playFabPlayerIdCache;

		// Token: 0x040047F4 RID: 18420
		private string _sessionTicket;

		// Token: 0x040047F5 RID: 18421
		private string _displayName;

		// Token: 0x040047F6 RID: 18422
		private string _nonce;

		// Token: 0x040047F7 RID: 18423
		public string userID;

		// Token: 0x040047F8 RID: 18424
		private string userToken;

		// Token: 0x040047F9 RID: 18425
		public PlatformTagJoin platform;

		// Token: 0x040047FA RID: 18426
		private bool isSafeAccount;

		// Token: 0x040047FB RID: 18427
		public Action<bool> OnSafetyUpdate;

		// Token: 0x040047FC RID: 18428
		private PlayFabAuthenticator.SafetyType safetyType;

		// Token: 0x040047FD RID: 18429
		private byte[] m_Ticket;

		// Token: 0x040047FE RID: 18430
		private uint m_pcbTicket;

		// Token: 0x040047FF RID: 18431
		public Text debugText;

		// Token: 0x04004800 RID: 18432
		public bool screenDebugMode;

		// Token: 0x04004801 RID: 18433
		public bool loginFailed;

		// Token: 0x04004802 RID: 18434
		[FormerlySerializedAs("loginDisplayID")]
		public GameObject emptyObject;

		// Token: 0x04004803 RID: 18435
		private int playFabAuthRetryCount;

		// Token: 0x04004804 RID: 18436
		private int playFabMaxRetries = 5;

		// Token: 0x04004805 RID: 18437
		private int playFabCacheRetryCount;

		// Token: 0x04004806 RID: 18438
		private int playFabCacheMaxRetries = 5;

		// Token: 0x04004807 RID: 18439
		public MetaAuthenticator metaAuthenticator;

		// Token: 0x04004808 RID: 18440
		public SteamAuthenticator steamAuthenticator;

		// Token: 0x04004809 RID: 18441
		public MothershipAuthenticator mothershipAuthenticator;

		// Token: 0x0400480A RID: 18442
		public PhotonAuthenticator photonAuthenticator;

		// Token: 0x0400480B RID: 18443
		private SteamAuthTicket steamAuthTicketForPlayFab;

		// Token: 0x0400480C RID: 18444
		private SteamAuthTicket steamAuthTicketForPhoton;

		// Token: 0x0400480D RID: 18445
		private string steamAuthIdForPhoton;

		// Token: 0x02000B03 RID: 2819
		public enum SafetyType
		{
			// Token: 0x04004810 RID: 18448
			None,
			// Token: 0x04004811 RID: 18449
			Auto,
			// Token: 0x04004812 RID: 18450
			OptIn
		}

		// Token: 0x02000B04 RID: 2820
		[Serializable]
		public class CachePlayFabIdRequest
		{
			// Token: 0x04004813 RID: 18451
			public string Platform;

			// Token: 0x04004814 RID: 18452
			public string SessionTicket;

			// Token: 0x04004815 RID: 18453
			public string PlayFabId;

			// Token: 0x04004816 RID: 18454
			public string TitleId;

			// Token: 0x04004817 RID: 18455
			public string MothershipEnvId;

			// Token: 0x04004818 RID: 18456
			public string MothershipToken;

			// Token: 0x04004819 RID: 18457
			public string MothershipId;
		}

		// Token: 0x02000B05 RID: 2821
		[Serializable]
		public class PlayfabAuthRequestData
		{
			// Token: 0x0400481A RID: 18458
			public string AppId;

			// Token: 0x0400481B RID: 18459
			public string Nonce;

			// Token: 0x0400481C RID: 18460
			public string OculusId;

			// Token: 0x0400481D RID: 18461
			public string Platform;

			// Token: 0x0400481E RID: 18462
			public string AgeCategory;

			// Token: 0x0400481F RID: 18463
			public string MothershipEnvId;

			// Token: 0x04004820 RID: 18464
			public string MothershipToken;

			// Token: 0x04004821 RID: 18465
			public string MothershipId;
		}

		// Token: 0x02000B06 RID: 2822
		[Serializable]
		public class PlayfabAuthResponseData
		{
			// Token: 0x04004822 RID: 18466
			public string SessionTicket;

			// Token: 0x04004823 RID: 18467
			public string EntityToken;

			// Token: 0x04004824 RID: 18468
			public string PlayFabId;

			// Token: 0x04004825 RID: 18469
			public string EntityId;

			// Token: 0x04004826 RID: 18470
			public string EntityType;

			// Token: 0x04004827 RID: 18471
			public string KidAccessToken;

			// Token: 0x04004828 RID: 18472
			public string KidRefreshToken;

			// Token: 0x04004829 RID: 18473
			public string KidUrlBasePath;

			// Token: 0x0400482A RID: 18474
			public string LocationCode;
		}

		// Token: 0x02000B07 RID: 2823
		[Serializable]
		public class CachePlayFabIdResponse
		{
			// Token: 0x0400482B RID: 18475
			public string PlayFabId;

			// Token: 0x0400482C RID: 18476
			public string SteamAuthIdForPhoton;

			// Token: 0x0400482D RID: 18477
			public string KidAccessToken;

			// Token: 0x0400482E RID: 18478
			public string KidRefreshToken;

			// Token: 0x0400482F RID: 18479
			public string KidUrlBasePath;

			// Token: 0x04004830 RID: 18480
			public string LocationCode;
		}

		// Token: 0x02000B08 RID: 2824
		public class BanInfo
		{
			// Token: 0x04004831 RID: 18481
			public string BanMessage;

			// Token: 0x04004832 RID: 18482
			public string BanExpirationTime;
		}
	}
}
