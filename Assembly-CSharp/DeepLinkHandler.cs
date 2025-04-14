using System;
using System.Collections;
using GorillaNetworking;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000458 RID: 1112
public class DeepLinkHandler : MonoBehaviour
{
	// Token: 0x06001B52 RID: 6994 RVA: 0x000867E8 File Offset: 0x000849E8
	public void Awake()
	{
		if (DeepLinkHandler.instance == null)
		{
			DeepLinkHandler.instance = this;
			return;
		}
		if (DeepLinkHandler.instance != this)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x00086818 File Offset: 0x00084A18
	public static void Initialize(GameObject parent)
	{
		if (DeepLinkHandler.instance == null && parent != null)
		{
			parent.AddComponent<DeepLinkHandler>();
		}
		if (DeepLinkHandler.instance == null)
		{
			return;
		}
		DeepLinkHandler.instance.RefreshLaunchDetails();
		if (DeepLinkHandler.instance.cachedLaunchDetails != null && DeepLinkHandler.instance.cachedLaunchDetails.LaunchType == LaunchType.Deeplink)
		{
			DeepLinkHandler.instance.HandleDeepLink();
			return;
		}
		Object.Destroy(DeepLinkHandler.instance);
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x0008689C File Offset: 0x00084A9C
	private void RefreshLaunchDetails()
	{
		if (UnityEngine.Application.platform != RuntimePlatform.Android)
		{
			Debug.Log("[DeepLinkHandler::RefreshLaunchDetails] Not on Android Platform!");
			return;
		}
		this.cachedLaunchDetails = ApplicationLifecycle.GetLaunchDetails();
		Debug.Log(string.Concat(new string[]
		{
			"[DeepLinkHandler::RefreshLaunchDetails] LaunchType: ",
			this.cachedLaunchDetails.LaunchType.ToString(),
			"\n[DeepLinkHandler::RefreshLaunchDetails] LaunchSource: ",
			this.cachedLaunchDetails.LaunchSource,
			"\n[DeepLinkHandler::RefreshLaunchDetails] DeepLinkMessage: ",
			this.cachedLaunchDetails.DeeplinkMessage
		}));
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x00086925 File Offset: 0x00084B25
	private static IEnumerator ProcessWebRequest(string url, string data, string contentType, Action<UnityWebRequest> callback)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, data, contentType);
		yield return request.SendWebRequest();
		callback(request);
		yield break;
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x0008694C File Offset: 0x00084B4C
	private void HandleDeepLink()
	{
		Debug.Log("[DeepLinkHandler::HandleDeepLink] Handling deep link...");
		if (this.cachedLaunchDetails.LaunchSource.Contains("7221491444554579"))
		{
			Debug.Log("[DeepLinkHandler::HandleDeepLink] DeepLink received from Witchblood, processing...");
			string deeplinkMessage = this.cachedLaunchDetails.DeeplinkMessage;
			string launchSource = this.cachedLaunchDetails.LaunchSource;
			string userID = PlayFabAuthenticator.instance.userID;
			string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			string playFabSessionTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket();
			string text = string.Concat(new string[]
			{
				"{ \"itemGUID\": \"",
				deeplinkMessage,
				"\", \"launchSource\": \"",
				launchSource,
				"\", \"oculusUserID\": \"",
				userID,
				"\", \"playFabID\": \"",
				playFabPlayerId,
				"\", \"playFabSessionTicket\": \"",
				playFabSessionTicket,
				"\" }"
			});
			Debug.Log("[DeepLinkHandler::HandleDeepLink] Web Request body: \n" + text);
			base.StartCoroutine(DeepLinkHandler.ProcessWebRequest(PlayFabAuthenticatorSettings.HpPromoApiBaseUrl + "/api/ConsumeItem", text, "application/json", new Action<UnityWebRequest>(this.OnWitchbloodCollabResponse)));
			return;
		}
		if (this.cachedLaunchDetails.LaunchSource.Contains("1903584373052985"))
		{
			Debug.Log("[DeepLinkHandler::HandleDeepLink] DeepLink received from Racoon Lagoon, processing...");
			string deeplinkMessage2 = this.cachedLaunchDetails.DeeplinkMessage;
			string launchSource2 = this.cachedLaunchDetails.LaunchSource;
			string userID2 = PlayFabAuthenticator.instance.userID;
			string playFabPlayerId2 = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
			string playFabSessionTicket2 = PlayFabAuthenticator.instance.GetPlayFabSessionTicket();
			string text2 = string.Concat(new string[]
			{
				"{ \"itemGUID\": \"",
				deeplinkMessage2,
				"\", \"launchSource\": \"",
				launchSource2,
				"\", \"oculusUserID\": \"",
				userID2,
				"\", \"playFabID\": \"",
				playFabPlayerId2,
				"\", \"playFabSessionTicket\": \"",
				playFabSessionTicket2,
				"\" }"
			});
			Debug.Log("[DeepLinkHandler::HandleDeepLink] Web Request body: \n" + text2);
			base.StartCoroutine(DeepLinkHandler.ProcessWebRequest(PlayFabAuthenticatorSettings.HpPromoApiBaseUrl + "/api/ConsumeItem", text2, "application/json", new Action<UnityWebRequest>(this.OnRaccoonLagoonCollabResponse)));
			return;
		}
		Debug.LogError("[DeepLinkHandler::HandleDeepLink] App launched via DeepLink, but from an unknown app. App ID: " + this.cachedLaunchDetails.LaunchSource);
		Object.Destroy(this);
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x00086B80 File Offset: 0x00084D80
	private void OnWitchbloodCollabResponse(UnityWebRequest completedRequest)
	{
		if (completedRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("[DeepLinkHandler::OnWitchbloodCollabResponse] Web Request failed: " + completedRequest.error + "\nDetails: " + completedRequest.downloadHandler.text);
			Object.Destroy(this);
			return;
		}
		if (completedRequest.downloadHandler.text.Contains("AlreadyRedeemed", StringComparison.OrdinalIgnoreCase))
		{
			Debug.Log("[DeepLinkHandler::OnWitchbloodCollabResponse] Item has already been redeemed!");
			Object.Destroy(this);
			return;
		}
		Debug.Log("[DeepLinkHandler::OnWitchbloodCollabResponse] Item successfully granted, processing external unlock...");
		base.StartCoroutine(this.CheckProcessExternalUnlock(this.WitchbloodCollabCosmeticID, true, true, true));
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x00086C0C File Offset: 0x00084E0C
	private void OnRaccoonLagoonCollabResponse(UnityWebRequest completedRequest)
	{
		if (completedRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("[DeepLinkHandler::OnRaccoonLagoonCollabResponse] Web Request failed: " + completedRequest.error + "\nDetails: " + completedRequest.downloadHandler.text);
			Object.Destroy(this);
			return;
		}
		if (completedRequest.downloadHandler.text.Contains("AlreadyRedeemed", StringComparison.OrdinalIgnoreCase))
		{
			Debug.Log("[DeepLinkHandler::OnRaccoonLagoonCollabResponse] Item has already been redeemed!");
			Object.Destroy(this);
			return;
		}
		Debug.Log("[DeepLinkHandler::OnRaccoonLagoonCollabResponse] Item successfully granted, processing external unlock...");
		base.StartCoroutine(this.CheckProcessExternalUnlock(this.RaccoonLagoonCosmeticIDs, true, true, true));
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x00086C97 File Offset: 0x00084E97
	private IEnumerator CheckProcessExternalUnlock(string[] itemIDs, bool autoEquip, bool isLeftHand, bool destroyOnFinish)
	{
		Debug.Log("[DeepLinkHandler::CheckProcessExternalUnlock] Checking if we can process external cosmetic unlock...");
		while (!CosmeticsController.instance.allCosmeticsDict_isInitialized || !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			yield return null;
		}
		Debug.Log("[DeepLinkHandler::CheckProcessExternalUnlock] Cosmetics initialized, proceeding to process external unlock...");
		foreach (string itemID in itemIDs)
		{
			CosmeticsController.instance.ProcessExternalUnlock(itemID, autoEquip, isLeftHand);
		}
		if (destroyOnFinish)
		{
			Object.Destroy(this);
		}
		yield break;
	}

	// Token: 0x04001E51 RID: 7761
	public static volatile DeepLinkHandler instance;

	// Token: 0x04001E52 RID: 7762
	private LaunchDetails cachedLaunchDetails;

	// Token: 0x04001E53 RID: 7763
	private const string WitchbloodAppID = "7221491444554579";

	// Token: 0x04001E54 RID: 7764
	private readonly string[] WitchbloodCollabCosmeticID = new string[]
	{
		"LMAKT."
	};

	// Token: 0x04001E55 RID: 7765
	private const string RaccoonLagoonAppID = "1903584373052985";

	// Token: 0x04001E56 RID: 7766
	private readonly string[] RaccoonLagoonCosmeticIDs = new string[]
	{
		"LMALI.",
		"LHAGS."
	};

	// Token: 0x04001E57 RID: 7767
	private const string HiddenPathCollabEndpoint = "/api/ConsumeItem";
}
