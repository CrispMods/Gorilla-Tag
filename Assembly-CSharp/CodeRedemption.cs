using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020003CD RID: 973
public class CodeRedemption : MonoBehaviour
{
	// Token: 0x06001768 RID: 5992 RVA: 0x000722AE File Offset: 0x000704AE
	public void Awake()
	{
		if (CodeRedemption.Instance == null)
		{
			CodeRedemption.Instance = this;
			return;
		}
		if (CodeRedemption.Instance != this)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000722E0 File Offset: 0x000704E0
	public void HandleCodeRedemption(string code)
	{
		string playFabPlayerId = PlayFabAuthenticator.instance.GetPlayFabPlayerId();
		string playFabSessionTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket();
		string text = string.Concat(new string[]
		{
			"{ \"itemGUID\": \"",
			code,
			"\", \"playFabID\": \"",
			playFabPlayerId,
			"\", \"playFabSessionTicket\": \"",
			playFabSessionTicket,
			"\" }"
		});
		Debug.Log("[CodeRedemption] Web Request body: \n" + text);
		base.StartCoroutine(CodeRedemption.ProcessWebRequest(PlayFabAuthenticatorSettings.HpPromoApiBaseUrl + "/api/ConsumeCodeItem", text, "application/json", new Action<UnityWebRequest>(this.OnCodeRedemptionResponse)));
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x00072380 File Offset: 0x00070580
	private void OnCodeRedemptionResponse(UnityWebRequest completedRequest)
	{
		if (completedRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("[CodeRedemption] Web Request failed: " + completedRequest.error + "\nDetails: " + completedRequest.downloadHandler.text);
			GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Invalid;
			return;
		}
		string text = string.Empty;
		try
		{
			CodeRedemption.CodeRedemptionRequest codeRedemptionRequest = JsonUtility.FromJson<CodeRedemption.CodeRedemptionRequest>(completedRequest.downloadHandler.text);
			if (codeRedemptionRequest.result.Contains("AlreadyRedeemed", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Log("[CodeRedemption] Item has already been redeemed!");
				GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.AlreadyUsed;
				return;
			}
			text = codeRedemptionRequest.playFabItemName;
		}
		catch (Exception ex)
		{
			string str = "[CodeRedemption] Error parsing JSON response: ";
			Exception ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
			GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Invalid;
			return;
		}
		Debug.Log("[CodeRedemption] Item successfully granted, processing external unlock...");
		GorillaComputer.instance.RedemptionStatus = GorillaComputer.RedemptionResult.Success;
		GorillaComputer.instance.RedemptionCode = "";
		base.StartCoroutine(this.CheckProcessExternalUnlock(new string[]
		{
			text
		}, true, true, true));
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x00072494 File Offset: 0x00070694
	private IEnumerator CheckProcessExternalUnlock(string[] itemIDs, bool autoEquip, bool isLeftHand, bool destroyOnFinish)
	{
		Debug.Log("[CodeRedemption] Checking if we can process external cosmetic unlock...");
		while (!CosmeticsController.instance.allCosmeticsDict_isInitialized || !CosmeticsV2Spawner_Dirty.allPartsInstantiated)
		{
			yield return null;
		}
		Debug.Log("[CodeRedemption] Cosmetics initialized, proceeding to process external unlock...");
		foreach (string itemID in itemIDs)
		{
			CosmeticsController.instance.ProcessExternalUnlock(itemID, autoEquip, isLeftHand);
		}
		yield break;
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000724B1 File Offset: 0x000706B1
	private static IEnumerator ProcessWebRequest(string url, string data, string contentType, Action<UnityWebRequest> callback)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, data, contentType);
		yield return request.SendWebRequest();
		callback(request);
		yield break;
	}

	// Token: 0x04001A1F RID: 6687
	public static volatile CodeRedemption Instance;

	// Token: 0x04001A20 RID: 6688
	private const string HiddenPathCollabEndpoint = "/api/ConsumeCodeItem";

	// Token: 0x020003CE RID: 974
	[Serializable]
	public class CodeRedemptionRequest
	{
		// Token: 0x04001A21 RID: 6689
		public string result;

		// Token: 0x04001A22 RID: 6690
		public string itemID;

		// Token: 0x04001A23 RID: 6691
		public string playFabItemName;
	}
}
