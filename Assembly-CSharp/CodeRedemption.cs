using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020003D8 RID: 984
public class CodeRedemption : MonoBehaviour
{
	// Token: 0x060017B5 RID: 6069 RVA: 0x000400D9 File Offset: 0x0003E2D9
	public void Awake()
	{
		if (CodeRedemption.Instance == null)
		{
			CodeRedemption.Instance = this;
			return;
		}
		if (CodeRedemption.Instance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x000C9274 File Offset: 0x000C7474
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

	// Token: 0x060017B7 RID: 6071 RVA: 0x000C9314 File Offset: 0x000C7514
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

	// Token: 0x060017B8 RID: 6072 RVA: 0x00040108 File Offset: 0x0003E308
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

	// Token: 0x060017B9 RID: 6073 RVA: 0x00040125 File Offset: 0x0003E325
	private static IEnumerator ProcessWebRequest(string url, string data, string contentType, Action<UnityWebRequest> callback)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, data, contentType);
		yield return request.SendWebRequest();
		callback(request);
		yield break;
	}

	// Token: 0x04001A68 RID: 6760
	public static volatile CodeRedemption Instance;

	// Token: 0x04001A69 RID: 6761
	private const string HiddenPathCollabEndpoint = "/api/ConsumeCodeItem";

	// Token: 0x020003D9 RID: 985
	[Serializable]
	public class CodeRedemptionRequest
	{
		// Token: 0x04001A6A RID: 6762
		public string result;

		// Token: 0x04001A6B RID: 6763
		public string itemID;

		// Token: 0x04001A6C RID: 6764
		public string playFabItemName;
	}
}
