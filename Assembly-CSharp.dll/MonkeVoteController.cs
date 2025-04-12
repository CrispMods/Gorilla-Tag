using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GorillaNetworking;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x020000F9 RID: 249
public class MonkeVoteController : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600069D RID: 1693 RVA: 0x00033D5E File Offset: 0x00031F5E
	// (set) Token: 0x0600069E RID: 1694 RVA: 0x00033D65 File Offset: 0x00031F65
	public static MonkeVoteController instance { get; private set; }

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600069F RID: 1695 RVA: 0x00085F34 File Offset: 0x00084134
	// (remove) Token: 0x060006A0 RID: 1696 RVA: 0x00085F6C File Offset: 0x0008416C
	public event Action OnPollsUpdated;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060006A1 RID: 1697 RVA: 0x00085FA4 File Offset: 0x000841A4
	// (remove) Token: 0x060006A2 RID: 1698 RVA: 0x00085FDC File Offset: 0x000841DC
	public event Action OnVoteAccepted;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060006A3 RID: 1699 RVA: 0x00086014 File Offset: 0x00084214
	// (remove) Token: 0x060006A4 RID: 1700 RVA: 0x0008604C File Offset: 0x0008424C
	public event Action OnVoteFailed;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060006A5 RID: 1701 RVA: 0x00086084 File Offset: 0x00084284
	// (remove) Token: 0x060006A6 RID: 1702 RVA: 0x000860BC File Offset: 0x000842BC
	public event Action OnCurrentPollEnded;

	// Token: 0x060006A7 RID: 1703 RVA: 0x00033D6D File Offset: 0x00031F6D
	public void Awake()
	{
		if (MonkeVoteController.instance == null)
		{
			MonkeVoteController.instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000860F4 File Offset: 0x000842F4
	public void SliceUpdate()
	{
		if (this.isCurrentPollActive && !this.hasCurrentPollCompleted && this.currentPollCompletionTime < DateTime.UtcNow)
		{
			GTDev.Log<string>("Active vote poll completed.", null);
			this.hasCurrentPollCompleted = true;
			Action onCurrentPollEnded = this.OnCurrentPollEnded;
			if (onCurrentPollEnded == null)
			{
				return;
			}
			onCurrentPollEnded();
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00031B26 File Offset: 0x0002FD26
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00031B2F File Offset: 0x0002FD2F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00086148 File Offset: 0x00084348
	public void RequestPolls()
	{
		MonkeVoteController.<RequestPolls>d__34 <RequestPolls>d__;
		<RequestPolls>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestPolls>d__.<>4__this = this;
		<RequestPolls>d__.<>1__state = -1;
		<RequestPolls>d__.<>t__builder.Start<MonkeVoteController.<RequestPolls>d__34>(ref <RequestPolls>d__);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00086180 File Offset: 0x00084380
	private Task WaitForSessionToken()
	{
		MonkeVoteController.<WaitForSessionToken>d__35 <WaitForSessionToken>d__;
		<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForSessionToken>d__.<>1__state = -1;
		<WaitForSessionToken>d__.<>t__builder.Start<MonkeVoteController.<WaitForSessionToken>d__35>(ref <WaitForSessionToken>d__);
		return <WaitForSessionToken>d__.<>t__builder.Task;
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000861BC File Offset: 0x000843BC
	private void FetchPolls()
	{
		base.StartCoroutine(this.DoFetchPolls(new MonkeVoteController.FetchPollsRequest
		{
			TitleId = PlayFabAuthenticatorSettings.TitleId,
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			IncludeInactive = this.includeInactive
		}, new Action<List<MonkeVoteController.FetchPollsResponse>>(this.OnFetchPollsResponse)));
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00033D89 File Offset: 0x00031F89
	private IEnumerator DoFetchPolls(MonkeVoteController.FetchPollsRequest data, Action<List<MonkeVoteController.FetchPollsResponse>> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.VotingApiBaseUrl + "/api/FetchPoll", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
		bool retry = false;
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			List<MonkeVoteController.FetchPollsResponse> obj = JsonConvert.DeserializeObject<List<MonkeVoteController.FetchPollsResponse>>(request.downloadHandler.text);
			callback(obj);
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				retry = true;
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				retry = true;
			}
		}
		if (retry)
		{
			if (this.fetchPollsRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.fetchPollsRetryCount + 1));
				this.fetchPollsRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.FetchPolls();
			}
			else
			{
				GTDev.LogError<string>("Maximum FetchPolls retries attempted. Please check your network connection.", null);
				this.fetchPollsRetryCount = 0;
				callback(null);
			}
		}
		yield break;
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00086224 File Offset: 0x00084424
	private void OnFetchPollsResponse([CanBeNull] List<MonkeVoteController.FetchPollsResponse> response)
	{
		this.isFetchingPoll = false;
		this.hasPoll = false;
		this.lastPollData = null;
		this.currentPollData = null;
		this.isCurrentPollActive = false;
		this.hasCurrentPollCompleted = false;
		if (response != null)
		{
			DateTime minValue = DateTime.MinValue;
			using (List<MonkeVoteController.FetchPollsResponse>.Enumerator enumerator = response.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MonkeVoteController.FetchPollsResponse fetchPollsResponse = enumerator.Current;
					if (fetchPollsResponse.isActive)
					{
						this.hasPoll = true;
						this.currentPollData = fetchPollsResponse;
						if (this.currentPollData.EndTime > DateTime.UtcNow)
						{
							this.isCurrentPollActive = true;
							this.hasCurrentPollCompleted = false;
							this.currentPollCompletionTime = this.currentPollData.EndTime;
							this.currentPollCompletionTime = this.currentPollCompletionTime.AddMinutes(1.0);
						}
					}
					if (!fetchPollsResponse.isActive && fetchPollsResponse.EndTime > minValue && fetchPollsResponse.EndTime < DateTime.UtcNow)
					{
						this.lastPollData = fetchPollsResponse;
					}
				}
				goto IL_106;
			}
		}
		GTDev.LogError<string>("Error: Could not fetch polls!", null);
		IL_106:
		Action onPollsUpdated = this.OnPollsUpdated;
		if (onPollsUpdated == null)
		{
			return;
		}
		onPollsUpdated();
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00033DA6 File Offset: 0x00031FA6
	public void Vote(int pollId, int option, bool isPrediction)
	{
		if (!this.hasPoll)
		{
			return;
		}
		if (this.isSendingVote)
		{
			return;
		}
		this.isSendingVote = true;
		this.pollId = pollId;
		this.option = option;
		this.isPrediction = isPrediction;
		this.SendVote();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00033DDC File Offset: 0x00031FDC
	private void SendVote()
	{
		this.GetNonceForVotingCallback(null);
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00086358 File Offset: 0x00084558
	private void GetNonceForVotingCallback([CanBeNull] Message<UserProof> message)
	{
		if (message != null)
		{
			UserProof data = message.Data;
			this.Nonce = ((data != null) ? data.Value : null);
		}
		base.StartCoroutine(this.DoVote(new MonkeVoteController.VoteRequest
		{
			PollId = this.pollId,
			TitleId = PlayFabAuthenticatorSettings.TitleId,
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			OculusId = PlayFabAuthenticator.instance.userID,
			UserPlatform = PlayFabAuthenticator.instance.platform.ToString(),
			UserNonce = this.Nonce,
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			OptionIndex = this.option,
			IsPrediction = this.isPrediction
		}, new Action<MonkeVoteController.VoteResponse>(this.OnVoteSuccess)));
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x00033DE5 File Offset: 0x00031FE5
	private IEnumerator DoVote(MonkeVoteController.VoteRequest data, Action<MonkeVoteController.VoteResponse> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.VotingApiBaseUrl + "/api/Vote", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
		bool retry = false;
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			MonkeVoteController.VoteResponse obj = JsonConvert.DeserializeObject<MonkeVoteController.VoteResponse>(request.downloadHandler.text);
			callback(obj);
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				retry = true;
			}
			else if (request.responseCode == 429L)
			{
				GTDev.LogWarning<string>("User already voted on this poll!", null);
				callback(null);
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				retry = true;
			}
		}
		if (retry)
		{
			if (this.voteRetryCount < this.maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this.voteRetryCount + 1));
				this.voteRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.SendVote();
			}
			else
			{
				GTDev.LogError<string>("Maximum Vote retries attempted. Please check your network connection.", null);
				this.voteRetryCount = 0;
				callback(null);
			}
		}
		else
		{
			this.isSendingVote = false;
		}
		yield break;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00033E02 File Offset: 0x00032002
	private void OnVoteSuccess([CanBeNull] MonkeVoteController.VoteResponse response)
	{
		this.isSendingVote = false;
		if (response != null)
		{
			this.lastVoteData = response;
			Action onVoteAccepted = this.OnVoteAccepted;
			if (onVoteAccepted == null)
			{
				return;
			}
			onVoteAccepted();
			return;
		}
		else
		{
			Action onVoteFailed = this.OnVoteFailed;
			if (onVoteFailed == null)
			{
				return;
			}
			onVoteFailed();
			return;
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00033E36 File Offset: 0x00032036
	public MonkeVoteController.FetchPollsResponse GetLastPollData()
	{
		return this.lastPollData;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00033E3E File Offset: 0x0003203E
	public MonkeVoteController.FetchPollsResponse GetCurrentPollData()
	{
		return this.currentPollData;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00033E46 File Offset: 0x00032046
	public MonkeVoteController.VoteResponse GetVoteData()
	{
		return this.lastVoteData;
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00033E4E File Offset: 0x0003204E
	public int GetLastVotePollId()
	{
		return this.pollId;
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00033E56 File Offset: 0x00032056
	public int GetLastVoteSelectedOption()
	{
		return this.option;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00033E5E File Offset: 0x0003205E
	public bool GetLastVoteWasPrediction()
	{
		return this.isPrediction;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00033E66 File Offset: 0x00032066
	public DateTime GetCurrentPollCompletionTime()
	{
		return this.currentPollCompletionTime;
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040007E9 RID: 2025
	private string Nonce = "";

	// Token: 0x040007EA RID: 2026
	private bool includeInactive = true;

	// Token: 0x040007EB RID: 2027
	private int fetchPollsRetryCount;

	// Token: 0x040007EC RID: 2028
	private int maxRetriesOnFail = 3;

	// Token: 0x040007ED RID: 2029
	private int voteRetryCount;

	// Token: 0x040007F2 RID: 2034
	private MonkeVoteController.FetchPollsResponse lastPollData;

	// Token: 0x040007F3 RID: 2035
	private MonkeVoteController.FetchPollsResponse currentPollData;

	// Token: 0x040007F4 RID: 2036
	private MonkeVoteController.VoteResponse lastVoteData;

	// Token: 0x040007F5 RID: 2037
	private bool isFetchingPoll;

	// Token: 0x040007F6 RID: 2038
	private bool hasPoll;

	// Token: 0x040007F7 RID: 2039
	private bool isCurrentPollActive;

	// Token: 0x040007F8 RID: 2040
	private bool hasCurrentPollCompleted;

	// Token: 0x040007F9 RID: 2041
	private DateTime currentPollCompletionTime;

	// Token: 0x040007FA RID: 2042
	private bool isSendingVote;

	// Token: 0x040007FB RID: 2043
	private int pollId = -1;

	// Token: 0x040007FC RID: 2044
	private int option;

	// Token: 0x040007FD RID: 2045
	private bool isPrediction;

	// Token: 0x020000FA RID: 250
	[Serializable]
	private class FetchPollsRequest
	{
		// Token: 0x040007FE RID: 2046
		public string TitleId;

		// Token: 0x040007FF RID: 2047
		public string PlayFabId;

		// Token: 0x04000800 RID: 2048
		public string PlayFabTicket;

		// Token: 0x04000801 RID: 2049
		public bool IncludeInactive;
	}

	// Token: 0x020000FB RID: 251
	[Serializable]
	public class FetchPollsResponse
	{
		// Token: 0x04000802 RID: 2050
		public int PollId;

		// Token: 0x04000803 RID: 2051
		public string Question;

		// Token: 0x04000804 RID: 2052
		public List<string> VoteOptions;

		// Token: 0x04000805 RID: 2053
		public List<int> VoteCount;

		// Token: 0x04000806 RID: 2054
		public List<int> PredictionCount;

		// Token: 0x04000807 RID: 2055
		public DateTime StartTime;

		// Token: 0x04000808 RID: 2056
		public DateTime EndTime;

		// Token: 0x04000809 RID: 2057
		public bool isActive;
	}

	// Token: 0x020000FC RID: 252
	[Serializable]
	private class VoteRequest
	{
		// Token: 0x0400080A RID: 2058
		public int PollId;

		// Token: 0x0400080B RID: 2059
		public string TitleId;

		// Token: 0x0400080C RID: 2060
		public string PlayFabId;

		// Token: 0x0400080D RID: 2061
		public string OculusId;

		// Token: 0x0400080E RID: 2062
		public string UserNonce;

		// Token: 0x0400080F RID: 2063
		public string UserPlatform;

		// Token: 0x04000810 RID: 2064
		public int OptionIndex;

		// Token: 0x04000811 RID: 2065
		public bool IsPrediction;

		// Token: 0x04000812 RID: 2066
		public string PlayFabTicket;
	}

	// Token: 0x020000FD RID: 253
	[Serializable]
	public class VoteResponse
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00033E96 File Offset: 0x00032096
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00033E9E File Offset: 0x0003209E
		public int PollId { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00033EA7 File Offset: 0x000320A7
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00033EAF File Offset: 0x000320AF
		public string TitleId { get; set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00033EB8 File Offset: 0x000320B8
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x00033EC0 File Offset: 0x000320C0
		public List<string> VoteOptions { get; set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00033EC9 File Offset: 0x000320C9
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00033ED1 File Offset: 0x000320D1
		public List<int> VoteCount { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00033EDA File Offset: 0x000320DA
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00033EE2 File Offset: 0x000320E2
		public List<int> PredictionCount { get; set; }
	}
}
