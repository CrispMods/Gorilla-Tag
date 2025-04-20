using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using NexusSDK;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x0200041B RID: 1051
public class NexusManager : MonoBehaviour
{
	// Token: 0x06001A0D RID: 6669 RVA: 0x00041928 File Offset: 0x0003FB28
	private void Awake()
	{
		if (NexusManager.instance == null)
		{
			NexusManager.instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x00041944 File Offset: 0x0003FB44
	private void Start()
	{
		SDKInitializer.Init(this.apiKey, this.environment);
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00041957 File Offset: 0x0003FB57
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

	// Token: 0x06001A10 RID: 6672 RVA: 0x000D46B8 File Offset: 0x000D28B8
	public void VerifyCreatorCode(string code, Action<Member> onSuccess, Action onFailure)
	{
		NexusManager.GetMemberByCodeRequest requestParams = new NexusManager.GetMemberByCodeRequest
		{
			memberCode = code
		};
		base.StartCoroutine(NexusManager.GetMemberByCode(requestParams, onSuccess, onFailure));
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x00041974 File Offset: 0x0003FB74
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

	// Token: 0x04001CF8 RID: 7416
	private string apiKey = "nexus_pk_4c18dcb1531846c7abad4cb00c5242bb";

	// Token: 0x04001CF9 RID: 7417
	private string environment = "production";

	// Token: 0x04001CFA RID: 7418
	public static NexusManager instance;

	// Token: 0x04001CFB RID: 7419
	private Member[] validatedMembers;

	// Token: 0x0200041C RID: 1052
	[Serializable]
	public struct GetMemberByCodeRequest
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x000419AF File Offset: 0x0003FBAF
		// (set) Token: 0x06001A14 RID: 6676 RVA: 0x000419B7 File Offset: 0x0003FBB7
		public string memberCode { readonly get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000419C0 File Offset: 0x0003FBC0
		// (set) Token: 0x06001A16 RID: 6678 RVA: 0x000419C8 File Offset: 0x0003FBC8
		public string groupId { readonly get; set; }
	}

	// Token: 0x0200041D RID: 1053
	[Serializable]
	public struct GetMembersRequest
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x000419D1 File Offset: 0x0003FBD1
		// (set) Token: 0x06001A18 RID: 6680 RVA: 0x000419D9 File Offset: 0x0003FBD9
		public int page { readonly get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x000419E2 File Offset: 0x0003FBE2
		// (set) Token: 0x06001A1A RID: 6682 RVA: 0x000419EA File Offset: 0x0003FBEA
		public int pageSize { readonly get; set; }
	}
}
