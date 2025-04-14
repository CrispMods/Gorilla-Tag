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
	// Token: 0x02000AEB RID: 2795
	public class TitleVotingExample : MonoBehaviour
	{
		// Token: 0x060045E9 RID: 17897 RVA: 0x0014C224 File Offset: 0x0014A424
		public void Start()
		{
			TitleVotingExample.<Start>d__8 <Start>d__;
			<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Start>d__.<>4__this = this;
			<Start>d__.<>1__state = -1;
			<Start>d__.<>t__builder.Start<TitleVotingExample.<Start>d__8>(ref <Start>d__);
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x000023F4 File Offset: 0x000005F4
		public void Update()
		{
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x0014C25C File Offset: 0x0014A45C
		private Task WaitForSessionToken()
		{
			TitleVotingExample.<WaitForSessionToken>d__10 <WaitForSessionToken>d__;
			<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WaitForSessionToken>d__.<>1__state = -1;
			<WaitForSessionToken>d__.<>t__builder.Start<TitleVotingExample.<WaitForSessionToken>d__10>(ref <WaitForSessionToken>d__);
			return <WaitForSessionToken>d__.<>t__builder.Task;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x0014C298 File Offset: 0x0014A498
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

		// Token: 0x060045ED RID: 17901 RVA: 0x0014C300 File Offset: 0x0014A500
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

		// Token: 0x060045EE RID: 17902 RVA: 0x0014C3CE File Offset: 0x0014A5CE
		public void Vote()
		{
			this.GetNonceForVotingCallback(null);
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x0014C3D7 File Offset: 0x0014A5D7
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

		// Token: 0x060045F0 RID: 17904 RVA: 0x0014C3F4 File Offset: 0x0014A5F4
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

		// Token: 0x060045F1 RID: 17905 RVA: 0x0014C411 File Offset: 0x0014A611
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

		// Token: 0x060045F2 RID: 17906 RVA: 0x0014C43C File Offset: 0x0014A63C
		private void OnVoteSuccess([CanBeNull] TitleVotingExample.VoteResponse response)
		{
			if (response != null)
			{
				Debug.Log("Voted! " + JsonConvert.SerializeObject(response));
				return;
			}
			Debug.LogError("Error: Could not vote!");
		}

		// Token: 0x04004781 RID: 18305
		private string Nonce = "";

		// Token: 0x04004782 RID: 18306
		private int PollId = 5;

		// Token: 0x04004783 RID: 18307
		private bool includeInactive = true;

		// Token: 0x04004784 RID: 18308
		private int Option;

		// Token: 0x04004785 RID: 18309
		private bool isPrediction;

		// Token: 0x04004786 RID: 18310
		private int fetchPollsRetryCount;

		// Token: 0x04004787 RID: 18311
		private int voteRetryCount;

		// Token: 0x04004788 RID: 18312
		private int maxRetriesOnFail = 3;

		// Token: 0x02000AEC RID: 2796
		[Serializable]
		private class FetchPollsRequest
		{
			// Token: 0x04004789 RID: 18313
			public string TitleId;

			// Token: 0x0400478A RID: 18314
			public string PlayFabId;

			// Token: 0x0400478B RID: 18315
			public string PlayFabTicket;

			// Token: 0x0400478C RID: 18316
			public bool IncludeInactive;
		}

		// Token: 0x02000AED RID: 2797
		[Serializable]
		private class FetchPollsResponse
		{
			// Token: 0x0400478D RID: 18317
			public int PollId;

			// Token: 0x0400478E RID: 18318
			public string Question;

			// Token: 0x0400478F RID: 18319
			public List<string> VoteOptions;

			// Token: 0x04004790 RID: 18320
			public List<int> VoteCount;

			// Token: 0x04004791 RID: 18321
			public List<int> PredictionCount;

			// Token: 0x04004792 RID: 18322
			public DateTime StartTime;

			// Token: 0x04004793 RID: 18323
			public DateTime EndTime;
		}

		// Token: 0x02000AEE RID: 2798
		[Serializable]
		private class VoteRequest
		{
			// Token: 0x04004794 RID: 18324
			public int PollId;

			// Token: 0x04004795 RID: 18325
			public string TitleId;

			// Token: 0x04004796 RID: 18326
			public string PlayFabId;

			// Token: 0x04004797 RID: 18327
			public string OculusId;

			// Token: 0x04004798 RID: 18328
			public string UserNonce;

			// Token: 0x04004799 RID: 18329
			public string UserPlatform;

			// Token: 0x0400479A RID: 18330
			public int OptionIndex;

			// Token: 0x0400479B RID: 18331
			public bool IsPrediction;

			// Token: 0x0400479C RID: 18332
			public string PlayFabTicket;
		}

		// Token: 0x02000AEF RID: 2799
		[Serializable]
		private class VoteResponse
		{
			// Token: 0x17000732 RID: 1842
			// (get) Token: 0x060045F7 RID: 17911 RVA: 0x0014C489 File Offset: 0x0014A689
			// (set) Token: 0x060045F8 RID: 17912 RVA: 0x0014C491 File Offset: 0x0014A691
			public int PollId { get; set; }

			// Token: 0x17000733 RID: 1843
			// (get) Token: 0x060045F9 RID: 17913 RVA: 0x0014C49A File Offset: 0x0014A69A
			// (set) Token: 0x060045FA RID: 17914 RVA: 0x0014C4A2 File Offset: 0x0014A6A2
			public string TitleId { get; set; }

			// Token: 0x17000734 RID: 1844
			// (get) Token: 0x060045FB RID: 17915 RVA: 0x0014C4AB File Offset: 0x0014A6AB
			// (set) Token: 0x060045FC RID: 17916 RVA: 0x0014C4B3 File Offset: 0x0014A6B3
			public List<string> VoteOptions { get; set; }

			// Token: 0x17000735 RID: 1845
			// (get) Token: 0x060045FD RID: 17917 RVA: 0x0014C4BC File Offset: 0x0014A6BC
			// (set) Token: 0x060045FE RID: 17918 RVA: 0x0014C4C4 File Offset: 0x0014A6C4
			public List<int> VoteCount { get; set; }

			// Token: 0x17000736 RID: 1846
			// (get) Token: 0x060045FF RID: 17919 RVA: 0x0014C4CD File Offset: 0x0014A6CD
			// (set) Token: 0x06004600 RID: 17920 RVA: 0x0014C4D5 File Offset: 0x0014A6D5
			public List<int> PredictionCount { get; set; }
		}
	}
}
