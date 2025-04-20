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

// Token: 0x02000103 RID: 259
public class MonkeVoteController : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x00034FC2 File Offset: 0x000331C2
	// (set) Token: 0x060006DD RID: 1757 RVA: 0x00034FC9 File Offset: 0x000331C9
	public static MonkeVoteController instance { get; private set; }

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060006DE RID: 1758 RVA: 0x0008883C File Offset: 0x00086A3C
	// (remove) Token: 0x060006DF RID: 1759 RVA: 0x00088874 File Offset: 0x00086A74
	public event Action OnPollsUpdated;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060006E0 RID: 1760 RVA: 0x000888AC File Offset: 0x00086AAC
	// (remove) Token: 0x060006E1 RID: 1761 RVA: 0x000888E4 File Offset: 0x00086AE4
	public event Action OnVoteAccepted;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060006E2 RID: 1762 RVA: 0x0008891C File Offset: 0x00086B1C
	// (remove) Token: 0x060006E3 RID: 1763 RVA: 0x00088954 File Offset: 0x00086B54
	public event Action OnVoteFailed;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060006E4 RID: 1764 RVA: 0x0008898C File Offset: 0x00086B8C
	// (remove) Token: 0x060006E5 RID: 1765 RVA: 0x000889C4 File Offset: 0x00086BC4
	public event Action OnCurrentPollEnded;

	// Token: 0x060006E6 RID: 1766 RVA: 0x00034FD1 File Offset: 0x000331D1
	public void Awake()
	{
		if (MonkeVoteController.instance == null)
		{
			MonkeVoteController.instance = this;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x000889FC File Offset: 0x00086BFC
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

	// Token: 0x060006E8 RID: 1768 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00088A50 File Offset: 0x00086C50
	public void RequestPolls()
	{
		MonkeVoteController.<RequestPolls>d__34 <RequestPolls>d__;
		<RequestPolls>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestPolls>d__.<>4__this = this;
		<RequestPolls>d__.<>1__state = -1;
		<RequestPolls>d__.<>t__builder.Start<MonkeVoteController.<RequestPolls>d__34>(ref <RequestPolls>d__);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00088A88 File Offset: 0x00086C88
	private Task WaitForSessionToken()
	{
		MonkeVoteController.<WaitForSessionToken>d__35 <WaitForSessionToken>d__;
		<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForSessionToken>d__.<>1__state = -1;
		<WaitForSessionToken>d__.<>t__builder.Start<MonkeVoteController.<WaitForSessionToken>d__35>(ref <WaitForSessionToken>d__);
		return <WaitForSessionToken>d__.<>t__builder.Task;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00088AC4 File Offset: 0x00086CC4
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

	// Token: 0x060006ED RID: 1773 RVA: 0x00034FED File Offset: 0x000331ED
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

	// Token: 0x060006EE RID: 1774 RVA: 0x00088B2C File Offset: 0x00086D2C
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

	// Token: 0x060006EF RID: 1775 RVA: 0x0003500A File Offset: 0x0003320A
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

	// Token: 0x060006F0 RID: 1776 RVA: 0x00035040 File Offset: 0x00033240
	private void SendVote()
	{
		this.GetNonceForVotingCallback(null);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00088C60 File Offset: 0x00086E60
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

	// Token: 0x060006F2 RID: 1778 RVA: 0x00035049 File Offset: 0x00033249
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

	// Token: 0x060006F3 RID: 1779 RVA: 0x00035066 File Offset: 0x00033266
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

	// Token: 0x060006F4 RID: 1780 RVA: 0x0003509A File Offset: 0x0003329A
	public MonkeVoteController.FetchPollsResponse GetLastPollData()
	{
		return this.lastPollData;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x000350A2 File Offset: 0x000332A2
	public MonkeVoteController.FetchPollsResponse GetCurrentPollData()
	{
		return this.currentPollData;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x000350AA File Offset: 0x000332AA
	public MonkeVoteController.VoteResponse GetVoteData()
	{
		return this.lastVoteData;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x000350B2 File Offset: 0x000332B2
	public int GetLastVotePollId()
	{
		return this.pollId;
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000350BA File Offset: 0x000332BA
	public int GetLastVoteSelectedOption()
	{
		return this.option;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x000350C2 File Offset: 0x000332C2
	public bool GetLastVoteWasPrediction()
	{
		return this.isPrediction;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x000350CA File Offset: 0x000332CA
	public DateTime GetCurrentPollCompletionTime()
	{
		return this.currentPollCompletionTime;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000829 RID: 2089
	private string Nonce = "";

	// Token: 0x0400082A RID: 2090
	private bool includeInactive = true;

	// Token: 0x0400082B RID: 2091
	private int fetchPollsRetryCount;

	// Token: 0x0400082C RID: 2092
	private int maxRetriesOnFail = 3;

	// Token: 0x0400082D RID: 2093
	private int voteRetryCount;

	// Token: 0x04000832 RID: 2098
	private MonkeVoteController.FetchPollsResponse lastPollData;

	// Token: 0x04000833 RID: 2099
	private MonkeVoteController.FetchPollsResponse currentPollData;

	// Token: 0x04000834 RID: 2100
	private MonkeVoteController.VoteResponse lastVoteData;

	// Token: 0x04000835 RID: 2101
	private bool isFetchingPoll;

	// Token: 0x04000836 RID: 2102
	private bool hasPoll;

	// Token: 0x04000837 RID: 2103
	private bool isCurrentPollActive;

	// Token: 0x04000838 RID: 2104
	private bool hasCurrentPollCompleted;

	// Token: 0x04000839 RID: 2105
	private DateTime currentPollCompletionTime;

	// Token: 0x0400083A RID: 2106
	private bool isSendingVote;

	// Token: 0x0400083B RID: 2107
	private int pollId = -1;

	// Token: 0x0400083C RID: 2108
	private int option;

	// Token: 0x0400083D RID: 2109
	private bool isPrediction;

	// Token: 0x02000104 RID: 260
	[Serializable]
	private class FetchPollsRequest
	{
		// Token: 0x0400083E RID: 2110
		public string TitleId;

		// Token: 0x0400083F RID: 2111
		public string PlayFabId;

		// Token: 0x04000840 RID: 2112
		public string PlayFabTicket;

		// Token: 0x04000841 RID: 2113
		public bool IncludeInactive;
	}

	// Token: 0x02000105 RID: 261
	[Serializable]
	public class FetchPollsResponse
	{
		// Token: 0x04000842 RID: 2114
		public int PollId;

		// Token: 0x04000843 RID: 2115
		public string Question;

		// Token: 0x04000844 RID: 2116
		public List<string> VoteOptions;

		// Token: 0x04000845 RID: 2117
		public List<int> VoteCount;

		// Token: 0x04000846 RID: 2118
		public List<int> PredictionCount;

		// Token: 0x04000847 RID: 2119
		public DateTime StartTime;

		// Token: 0x04000848 RID: 2120
		public DateTime EndTime;

		// Token: 0x04000849 RID: 2121
		public bool isActive;
	}

	// Token: 0x02000106 RID: 262
	[Serializable]
	private class VoteRequest
	{
		// Token: 0x0400084A RID: 2122
		public int PollId;

		// Token: 0x0400084B RID: 2123
		public string TitleId;

		// Token: 0x0400084C RID: 2124
		public string PlayFabId;

		// Token: 0x0400084D RID: 2125
		public string OculusId;

		// Token: 0x0400084E RID: 2126
		public string UserNonce;

		// Token: 0x0400084F RID: 2127
		public string UserPlatform;

		// Token: 0x04000850 RID: 2128
		public int OptionIndex;

		// Token: 0x04000851 RID: 2129
		public bool IsPrediction;

		// Token: 0x04000852 RID: 2130
		public string PlayFabTicket;
	}

	// Token: 0x02000107 RID: 263
	[Serializable]
	public class VoteResponse
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x000350FA File Offset: 0x000332FA
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x00035102 File Offset: 0x00033302
		public int PollId { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0003510B File Offset: 0x0003330B
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x00035113 File Offset: 0x00033313
		public string TitleId { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0003511C File Offset: 0x0003331C
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x00035124 File Offset: 0x00033324
		public List<string> VoteOptions { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0003512D File Offset: 0x0003332D
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x00035135 File Offset: 0x00033335
		public List<int> VoteCount { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0003513E File Offset: 0x0003333E
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x00035146 File Offset: 0x00033346
		public List<int> PredictionCount { get; set; }
	}
}
