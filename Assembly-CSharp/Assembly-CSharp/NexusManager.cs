using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NexusSDK;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000410 RID: 1040
public class NexusManager : MonoBehaviour
{
	// Token: 0x060019C3 RID: 6595 RVA: 0x0007F363 File Offset: 0x0007D563
	private void Awake()
	{
		if (NexusManager.instance == null)
		{
			NexusManager.instance = this;
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x0007F37F File Offset: 0x0007D57F
	private void Start()
	{
		SDKInitializer.Init(this.apiKey, this.environment);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x0007F392 File Offset: 0x0007D592
	public static IEnumerator GetMembers(NexusManager.GetMembersRequest RequestParams, Action<AttributionAPI.GetMembers200Response> onSuccess, Action<string> onFailure)
	{
		string text = SDKInitializer.ApiBaseUrl + "/manage/members";
		List<string> list = new List<string>();
		if (RequestParams.page != 0)
		{
			list.Add("page=" + RequestParams.page.ToString());
		}
		if (RequestParams.pageSize != 0)
		{
			list.Add("pageSize=" + RequestParams.pageSize.ToString());
		}
		text += "?";
		text += string.Join("&", list);
		using (UnityWebRequest webRequest = UnityWebRequest.Get(text))
		{
			webRequest.SetRequestHeader("x-shared-secret", SDKInitializer.ApiKey);
			yield return webRequest.SendWebRequest();
			if (webRequest.responseCode == 200L)
			{
				AttributionAPI.GetMembers200Response obj = JsonConvert.DeserializeObject<AttributionAPI.GetMembers200Response>(webRequest.downloadHandler.text, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});
				if (onSuccess != null)
				{
					onSuccess(obj);
				}
			}
			else if (onFailure != null)
			{
				onFailure(webRequest.error);
			}
		}
		UnityWebRequest webRequest = null;
		yield break;
		yield break;
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x0007F3B0 File Offset: 0x0007D5B0
	public void VerifyCreatorCode(string code, Action<Member> onSuccess, Action onFailure)
	{
		NexusManager.GetMemberByCodeRequest requestParams = new NexusManager.GetMemberByCodeRequest
		{
			memberCode = code
		};
		base.StartCoroutine(NexusManager.GetMemberByCode(requestParams, onSuccess, onFailure));
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x0007F3DE File Offset: 0x0007D5DE
	public static IEnumerator GetMemberByCode(NexusManager.GetMemberByCodeRequest RequestParams, Action<Member> onSuccess, Action onFailure)
	{
		string text = SDKInitializer.ApiBaseUrl + "/manage/members/{memberCode}";
		text = text.Replace("{memberCode}", RequestParams.memberCode);
		List<string> values = new List<string>();
		text += "?";
		text += string.Join("&", values);
		using (UnityWebRequest webRequest = UnityWebRequest.Get(text))
		{
			webRequest.SetRequestHeader("x-shared-secret", SDKInitializer.ApiKey);
			yield return webRequest.SendWebRequest();
			if (webRequest.responseCode == 200L)
			{
				Member obj = JsonConvert.DeserializeObject<Member>(webRequest.downloadHandler.text, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});
				if (onSuccess != null)
				{
					onSuccess(obj);
				}
			}
			else if (onFailure != null)
			{
				onFailure();
			}
		}
		UnityWebRequest webRequest = null;
		yield break;
		yield break;
	}

	// Token: 0x04001CB0 RID: 7344
	private string apiKey = "nexus_pk_4c18dcb1531846c7abad4cb00c5242bb";

	// Token: 0x04001CB1 RID: 7345
	private string environment = "production";

	// Token: 0x04001CB2 RID: 7346
	public static NexusManager instance;

	// Token: 0x04001CB3 RID: 7347
	private Member[] validatedMembers;

	// Token: 0x02000411 RID: 1041
	[Serializable]
	public struct GetMemberByCodeRequest
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x0007F419 File Offset: 0x0007D619
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x0007F421 File Offset: 0x0007D621
		public string memberCode { readonly get; set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060019CB RID: 6603 RVA: 0x0007F42A File Offset: 0x0007D62A
		// (set) Token: 0x060019CC RID: 6604 RVA: 0x0007F432 File Offset: 0x0007D632
		public string groupId { readonly get; set; }
	}

	// Token: 0x02000412 RID: 1042
	[Serializable]
	public struct GetMembersRequest
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x0007F43B File Offset: 0x0007D63B
		// (set) Token: 0x060019CE RID: 6606 RVA: 0x0007F443 File Offset: 0x0007D643
		public int page { readonly get; set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0007F44C File Offset: 0x0007D64C
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x0007F454 File Offset: 0x0007D654
		public int pageSize { readonly get; set; }
	}
}
