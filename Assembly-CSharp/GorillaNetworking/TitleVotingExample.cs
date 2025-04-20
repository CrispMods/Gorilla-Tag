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
	// Token: 0x02000B15 RID: 2837
	public class TitleVotingExample : MonoBehaviour
	{
		// Token: 0x06004725 RID: 18213 RVA: 0x00188E80 File Offset: 0x00187080
		public void Start()
		{
			TitleVotingExample.<Start>d__8 <Start>d__;
			<Start>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Start>d__.<>4__this = this;
			<Start>d__.<>1__state = -1;
			<Start>d__.<>t__builder.Start<TitleVotingExample.<Start>d__8>(ref <Start>d__);
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x00030607 File Offset: 0x0002E807
		public void Update()
		{
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x00188EB8 File Offset: 0x001870B8
		private Task WaitForSessionToken()
		{
			TitleVotingExample.<WaitForSessionToken>d__10 <WaitForSessionToken>d__;
			<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WaitForSessionToken>d__.<>1__state = -1;
			<WaitForSessionToken>d__.<>t__builder.Start<TitleVotingExample.<WaitForSessionToken>d__10>(ref <WaitForSessionToken>d__);
			return <WaitForSessionToken>d__.<>t__builder.Task;
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x00188EF4 File Offset: 0x001870F4
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

		// Token: 0x06004729 RID: 18217 RVA: 0x00188F5C File Offset: 0x0018715C
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

		// Token: 0x0600472A RID: 18218 RVA: 0x0005E55E File Offset: 0x0005C75E
		public void Vote()
		{
			this.GetNonceForVotingCallback(null);
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0005E567 File Offset: 0x0005C767
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

		// Token: 0x0600472C RID: 18220 RVA: 0x0005E584 File Offset: 0x0005C784
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

		// Token: 0x0600472D RID: 18221 RVA: 0x0005E5A1 File Offset: 0x0005C7A1
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

		// Token: 0x0600472E RID: 18222 RVA: 0x0005E5CC File Offset: 0x0005C7CC
		private void OnVoteSuccess([CanBeNull] TitleVotingExample.VoteResponse response)
		{
			if (response != null)
			{
				Debug.Log("Voted! " + JsonConvert.SerializeObject(response));
				return;
			}
			Debug.LogError("Error: Could not vote!");
		}

		// Token: 0x04004864 RID: 18532
		private string Nonce = "";

		// Token: 0x04004865 RID: 18533
		private int PollId = 5;

		// Token: 0x04004866 RID: 18534
		private bool includeInactive = true;

		// Token: 0x04004867 RID: 18535
		private int Option;

		// Token: 0x04004868 RID: 18536
		private bool isPrediction;

		// Token: 0x04004869 RID: 18537
		private int fetchPollsRetryCount;

		// Token: 0x0400486A RID: 18538
		private int voteRetryCount;

		// Token: 0x0400486B RID: 18539
		private int maxRetriesOnFail = 3;

		// Token: 0x02000B16 RID: 2838
		[Serializable]
		private class FetchPollsRequest
		{
			// Token: 0x0400486C RID: 18540
			public string TitleId;

			// Token: 0x0400486D RID: 18541
			public string PlayFabId;

			// Token: 0x0400486E RID: 18542
			public string PlayFabTicket;

			// Token: 0x0400486F RID: 18543
			public bool IncludeInactive;
		}

		// Token: 0x02000B17 RID: 2839
		[Serializable]
		private class FetchPollsResponse
		{
			// Token: 0x04004870 RID: 18544
			public int PollId;

			// Token: 0x04004871 RID: 18545
			public string Question;

			// Token: 0x04004872 RID: 18546
			public List<string> VoteOptions;

			// Token: 0x04004873 RID: 18547
			public List<int> VoteCount;

			// Token: 0x04004874 RID: 18548
			public List<int> PredictionCount;

			// Token: 0x04004875 RID: 18549
			public DateTime StartTime;

			// Token: 0x04004876 RID: 18550
			public DateTime EndTime;
		}

		// Token: 0x02000B18 RID: 2840
		[Serializable]
		private class VoteRequest
		{
			// Token: 0x04004877 RID: 18551
			public int PollId;

			// Token: 0x04004878 RID: 18552
			public string TitleId;

			// Token: 0x04004879 RID: 18553
			public string PlayFabId;

			// Token: 0x0400487A RID: 18554
			public string OculusId;

			// Token: 0x0400487B RID: 18555
			public string UserNonce;

			// Token: 0x0400487C RID: 18556
			public string UserPlatform;

			// Token: 0x0400487D RID: 18557
			public int OptionIndex;

			// Token: 0x0400487E RID: 18558
			public bool IsPrediction;

			// Token: 0x0400487F RID: 18559
			public string PlayFabTicket;
		}

		// Token: 0x02000B19 RID: 2841
		[Serializable]
		private class VoteResponse
		{
			// Token: 0x1700074D RID: 1869
			// (get) Token: 0x06004733 RID: 18227 RVA: 0x0005E619 File Offset: 0x0005C819
			// (set) Token: 0x06004734 RID: 18228 RVA: 0x0005E621 File Offset: 0x0005C821
			public int PollId { get; set; }

			// Token: 0x1700074E RID: 1870
			// (get) Token: 0x06004735 RID: 18229 RVA: 0x0005E62A File Offset: 0x0005C82A
			// (set) Token: 0x06004736 RID: 18230 RVA: 0x0005E632 File Offset: 0x0005C832
			public string TitleId { get; set; }

			// Token: 0x1700074F RID: 1871
			// (get) Token: 0x06004737 RID: 18231 RVA: 0x0005E63B File Offset: 0x0005C83B
			// (set) Token: 0x06004738 RID: 18232 RVA: 0x0005E643 File Offset: 0x0005C843
			public List<string> VoteOptions { get; set; }

			// Token: 0x17000750 RID: 1872
			// (get) Token: 0x06004739 RID: 18233 RVA: 0x0005E64C File Offset: 0x0005C84C
			// (set) Token: 0x0600473A RID: 18234 RVA: 0x0005E654 File Offset: 0x0005C854
			public List<int> VoteCount { get; set; }

			// Token: 0x17000751 RID: 1873
			// (get) Token: 0x0600473B RID: 18235 RVA: 0x0005E65D File Offset: 0x0005C85D
			// (set) Token: 0x0600473C RID: 18236 RVA: 0x0005E665 File Offset: 0x0005C865
			public List<int> PredictionCount { get; set; }
		}
	}
}
