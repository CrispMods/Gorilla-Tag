using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace GorillaNetworking
{
	// Token: 0x02000AE8 RID: 2792
	public class TitleVotingExample : MonoBehaviour
	{
		// Token: 0x060045DD RID: 17885 RVA: 0x0014BC5C File Offset: 0x00149E5C
		public void Start()
		{
			TitleVotingExample.<Start>d__8 <Start>d__;
			<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Start>d__.<>4__this = this;
			<Start>d__.<>1__state = -1;
			<Start>d__.<>t__builder.Start<TitleVotingExample.<Start>d__8>(ref <Start>d__);
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x000023F4 File Offset: 0x000005F4
		public void Update()
		{
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x0014BC94 File Offset: 0x00149E94
		private Task WaitForSessionToken()
		{
			TitleVotingExample.<WaitForSessionToken>d__10 <WaitForSessionToken>d__;
			<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WaitForSessionToken>d__.<>1__state = -1;
			<WaitForSessionToken>d__.<>t__builder.Start<TitleVotingExample.<WaitForSessionToken>d__10>(ref <WaitForSessionToken>d__);
			return <WaitForSessionToken>d__.<>t__builder.Task;
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x0014BCD0 File Offset: 0x00149ED0
		public void FetchPollsAndVote()
		{
			base.StartCoroutine(this.DoFetchPolls(new TitleVotingExample.FetchPollsRequest
			{
				TitleId = PlayFabAuthenticatorSettings.TitleId,
				PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
				PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
				IncludeInactive = this.includeInactive
			}, new Action<List<TitleVotingExample.FetchPollsResponse>>(this.OnFetchPollsResponse)));
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x0014BD38 File Offset: 0x00149F38
		private void GetNonceForVotingCallback([CanBeNull] Message<UserProof> message)
		{
			if (message != null)
			{
				UserProof data = message.Data;
				this.Nonce = ((data != null) ? data.ToString() : null);
			}
			base.StartCoroutine(this.DoVote(new TitleVotingExample.VoteRequest
			{
				PollId = this.PollId,
				TitleId = PlayFabAuthenticatorSettings.TitleId,
				PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
				OculusId = PlayFabAuthenticator.instance.userID,
				UserPlatform = PlayFabAuthenticator.instance.platform.ToString(),
				UserNonce = this.Nonce,
				PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
				OptionIndex = this.Option,
				IsPrediction = this.isPrediction
			}, new Action<TitleVotingExample.VoteResponse>(this.OnVoteSuccess)));
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x0014BE06 File Offset: 0x0014A006
		public void Vote()
		{
			this.GetNonceForVotingCallback(null);
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x0014BE0F File Offset: 0x0014A00F
		private IEnumerator DoFetchPolls(TitleVotingExample.FetchPollsRequest data, Action<List<TitleVotingExample.FetchPollsResponse>> callback)
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
				List<TitleVotingExample.FetchPollsResponse> obj = JsonConvert.DeserializeObject<List<TitleVotingExample.FetchPollsResponse>>(request.downloadHandler.text);
				callback(obj);
			}
			else
			{
				Debug.LogError(string.Format("FetchPolls Error: {0} -- raw response: ", request.responseCode) + request.downloadHandler.text);
				long responseCode = request.responseCode;
				if (responseCode >= 500L && responseCode < 600L)
				{
					retry = true;
					Debug.LogError(string.Format("HTTP {0} error: {1}", request.responseCode, request.error));
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
					Debug.LogWarning(string.Format("Retrying Title Voting FetchPolls... Retry attempt #{0}, waiting for {1} seconds", this.fetchPollsRetryCount + 1, num));
					this.fetchPollsRetryCount++;
					yield return new WaitForSeconds((float)num);
					this.FetchPollsAndVote();
				}
				else
				{
					Debug.LogError("Maximum FetchPolls retries attempted. Please check your network connection.");
					this.fetchPollsRetryCount = 0;
					callback(null);
				}
			}
			yield break;
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x0014BE2C File Offset: 0x0014A02C
		private IEnumerator DoVote(TitleVotingExample.VoteRequest data, Action<TitleVotingExample.VoteResponse> callback)
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
				TitleVotingExample.VoteResponse obj = JsonConvert.DeserializeObject<TitleVotingExample.VoteResponse>(request.downloadHandler.text);
				callback(obj);
			}
			else
			{
				Debug.LogError(string.Format("Vote Error: {0} -- raw response: ", request.responseCode) + request.downloadHandler.text);
				long responseCode = request.responseCode;
				if (responseCode >= 500L && responseCode < 600L)
				{
					retry = true;
					Debug.LogError(string.Format("HTTP {0} error: {1}", request.responseCode, request.error));
				}
				else if (request.responseCode == 409L)
				{
					Debug.LogWarning("User already voted on this poll!");
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
					Debug.LogWarning(string.Format("Retrying Voting... Retry attempt #{0}, waiting for {1} seconds", this.voteRetryCount + 1, num));
					this.voteRetryCount++;
					yield return new WaitForSeconds((float)num);
					this.Vote();
				}
				else
				{
					Debug.LogError("Maximum Vote retries attempted. Please check your network connection.");
					this.voteRetryCount = 0;
					callback(null);
				}
			}
			yield break;
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0014BE49 File Offset: 0x0014A049
		private void OnFetchPollsResponse([CanBeNull] List<TitleVotingExample.FetchPollsResponse> response)
		{
			if (response != null)
			{
				Debug.Log("Got polls: " + JsonConvert.SerializeObject(response));
				this.Vote();
				return;
			}
			Debug.LogError("Error: Could not fetch polls!");
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x0014BE74 File Offset: 0x0014A074
		private void OnVoteSuccess([CanBeNull] TitleVotingExample.VoteResponse response)
		{
			if (response != null)
			{
				Debug.Log("Voted! " + JsonConvert.SerializeObject(response));
				return;
			}
			Debug.LogError("Error: Could not vote!");
		}

		// Token: 0x0400476F RID: 18287
		private string Nonce = "";

		// Token: 0x04004770 RID: 18288
		private int PollId = 5;

		// Token: 0x04004771 RID: 18289
		private bool includeInactive = true;

		// Token: 0x04004772 RID: 18290
		private int Option;

		// Token: 0x04004773 RID: 18291
		private bool isPrediction;

		// Token: 0x04004774 RID: 18292
		private int fetchPollsRetryCount;

		// Token: 0x04004775 RID: 18293
		private int voteRetryCount;

		// Token: 0x04004776 RID: 18294
		private int maxRetriesOnFail = 3;

		// Token: 0x02000AE9 RID: 2793
		[Serializable]
		private class FetchPollsRequest
		{
			// Token: 0x04004777 RID: 18295
			public string TitleId;

			// Token: 0x04004778 RID: 18296
			public string PlayFabId;

			// Token: 0x04004779 RID: 18297
			public string PlayFabTicket;

			// Token: 0x0400477A RID: 18298
			public bool IncludeInactive;
		}

		// Token: 0x02000AEA RID: 2794
		[Serializable]
		private class FetchPollsResponse
		{
			// Token: 0x0400477B RID: 18299
			public int PollId;

			// Token: 0x0400477C RID: 18300
			public string Question;

			// Token: 0x0400477D RID: 18301
			public List<string> VoteOptions;

			// Token: 0x0400477E RID: 18302
			public List<int> VoteCount;

			// Token: 0x0400477F RID: 18303
			public List<int> PredictionCount;

			// Token: 0x04004780 RID: 18304
			public DateTime StartTime;

			// Token: 0x04004781 RID: 18305
			public DateTime EndTime;
		}

		// Token: 0x02000AEB RID: 2795
		[Serializable]
		private class VoteRequest
		{
			// Token: 0x04004782 RID: 18306
			public int PollId;

			// Token: 0x04004783 RID: 18307
			public string TitleId;

			// Token: 0x04004784 RID: 18308
			public string PlayFabId;

			// Token: 0x04004785 RID: 18309
			public string OculusId;

			// Token: 0x04004786 RID: 18310
			public string UserNonce;

			// Token: 0x04004787 RID: 18311
			public string UserPlatform;

			// Token: 0x04004788 RID: 18312
			public int OptionIndex;

			// Token: 0x04004789 RID: 18313
			public bool IsPrediction;

			// Token: 0x0400478A RID: 18314
			public string PlayFabTicket;
		}

		// Token: 0x02000AEC RID: 2796
		[Serializable]
		private class VoteResponse
		{
			// Token: 0x17000731 RID: 1841
			// (get) Token: 0x060045EB RID: 17899 RVA: 0x0014BEC1 File Offset: 0x0014A0C1
			// (set) Token: 0x060045EC RID: 17900 RVA: 0x0014BEC9 File Offset: 0x0014A0C9
			public int PollId { get; set; }

			// Token: 0x17000732 RID: 1842
			// (get) Token: 0x060045ED RID: 17901 RVA: 0x0014BED2 File Offset: 0x0014A0D2
			// (set) Token: 0x060045EE RID: 17902 RVA: 0x0014BEDA File Offset: 0x0014A0DA
			public string TitleId { get; set; }

			// Token: 0x17000733 RID: 1843
			// (get) Token: 0x060045EF RID: 17903 RVA: 0x0014BEE3 File Offset: 0x0014A0E3
			// (set) Token: 0x060045F0 RID: 17904 RVA: 0x0014BEEB File Offset: 0x0014A0EB
			public List<string> VoteOptions { get; set; }

			// Token: 0x17000734 RID: 1844
			// (get) Token: 0x060045F1 RID: 17905 RVA: 0x0014BEF4 File Offset: 0x0014A0F4
			// (set) Token: 0x060045F2 RID: 17906 RVA: 0x0014BEFC File Offset: 0x0014A0FC
			public List<int> VoteCount { get; set; }

			// Token: 0x17000735 RID: 1845
			// (get) Token: 0x060045F3 RID: 17907 RVA: 0x0014BF05 File Offset: 0x0014A105
			// (set) Token: 0x060045F4 RID: 17908 RVA: 0x0014BF0D File Offset: 0x0014A10D
			public List<int> PredictionCount { get; set; }
		}
	}
}
