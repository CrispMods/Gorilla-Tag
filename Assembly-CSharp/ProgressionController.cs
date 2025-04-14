﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GorillaNetworking;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000115 RID: 277
public class ProgressionController : MonoBehaviour
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06000781 RID: 1921 RVA: 0x00029E44 File Offset: 0x00028044
	// (remove) Token: 0x06000782 RID: 1922 RVA: 0x00029E78 File Offset: 0x00028078
	public static event Action OnQuestSelectionChanged;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06000783 RID: 1923 RVA: 0x00029EAC File Offset: 0x000280AC
	// (remove) Token: 0x06000784 RID: 1924 RVA: 0x00029EE0 File Offset: 0x000280E0
	public static event Action OnProgressEvent;

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x00029F13 File Offset: 0x00028113
	// (set) Token: 0x06000786 RID: 1926 RVA: 0x00029F1A File Offset: 0x0002811A
	public static int WeeklyCap { get; private set; } = 25;

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000787 RID: 1927 RVA: 0x00029F22 File Offset: 0x00028122
	public static int TotalPoints
	{
		get
		{
			return ProgressionController._gInstance.totalPointsRaw - ProgressionController._gInstance.unclaimedPoints;
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00029F39 File Offset: 0x00028139
	public static void ReportQuestChanged(bool initialLoad)
	{
		ProgressionController._gInstance.OnQuestProgressChanged(initialLoad);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00029F46 File Offset: 0x00028146
	public static void ReportQuestSelectionChanged()
	{
		ProgressionController._gInstance.LoadCompletedQuestQueue();
		Action onQuestSelectionChanged = ProgressionController.OnQuestSelectionChanged;
		if (onQuestSelectionChanged == null)
		{
			return;
		}
		onQuestSelectionChanged();
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00029F61 File Offset: 0x00028161
	public static void ReportQuestComplete(int questId, bool isDaily)
	{
		ProgressionController._gInstance.OnQuestComplete(questId, isDaily);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00029F6F File Offset: 0x0002816F
	public static void RedeemProgress()
	{
		ProgressionController._gInstance.RequestProgressRedemption(new Action(ProgressionController._gInstance.OnProgressRedeemed));
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00029F8B File Offset: 0x0002818B
	[return: TupleElementNames(new string[]
	{
		"weekly",
		"unclaimed",
		"total"
	})]
	public static ValueTuple<int, int, int> GetProgressionData()
	{
		return ProgressionController._gInstance.GetProgress();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00029F97 File Offset: 0x00028197
	public static void RequestProgressUpdate()
	{
		ProgressionController gInstance = ProgressionController._gInstance;
		if (gInstance == null)
		{
			return;
		}
		gInstance.ReportProgress();
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00029FA8 File Offset: 0x000281A8
	private void Awake()
	{
		if (ProgressionController._gInstance)
		{
			Debug.LogError("Duplicate ProgressionController detected. Destroying self.", base.gameObject);
			Object.Destroy(this);
			return;
		}
		ProgressionController._gInstance = this;
		this.unclaimedPoints = PlayerPrefs.GetInt("Claimed_Points_Key", 0);
		this.RequestStatus();
		this.LoadCompletedQuestQueue();
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00029FFC File Offset: 0x000281FC
	private void RequestStatus()
	{
		ProgressionController.<RequestStatus>d__36 <RequestStatus>d__;
		<RequestStatus>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestStatus>d__.<>4__this = this;
		<RequestStatus>d__.<>1__state = -1;
		<RequestStatus>d__.<>t__builder.Start<ProgressionController.<RequestStatus>d__36>(ref <RequestStatus>d__);
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002A034 File Offset: 0x00028234
	private Task WaitForSessionToken()
	{
		ProgressionController.<WaitForSessionToken>d__37 <WaitForSessionToken>d__;
		<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForSessionToken>d__.<>1__state = -1;
		<WaitForSessionToken>d__.<>t__builder.Start<ProgressionController.<WaitForSessionToken>d__37>(ref <WaitForSessionToken>d__);
		return <WaitForSessionToken>d__.<>t__builder.Task;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0002A070 File Offset: 0x00028270
	private void FetchStatus()
	{
		base.StartCoroutine(this.DoFetchStatus(new ProgressionController.GetQuestsStatusRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipId = "",
			MothershipToken = ""
		}, new Action<ProgressionController.GetQuestStatusResponse>(this.OnFetchStatusResponse)));
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0002A0D5 File Offset: 0x000282D5
	private IEnumerator DoFetchStatus(ProgressionController.GetQuestsStatusRequest data, Action<ProgressionController.GetQuestStatusResponse> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.ProgressionApiBaseUrl + "/api/GetQuestStatus", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
		bool retry = false;
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			ProgressionController.GetQuestStatusResponse obj = JsonConvert.DeserializeObject<ProgressionController.GetQuestStatusResponse>(request.downloadHandler.text);
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
			if (this._fetchStatusRetryCount < this._maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this._fetchStatusRetryCount + 1));
				this._fetchStatusRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.FetchStatus();
			}
			else
			{
				GTDev.LogError<string>("Maximum FetchStatus retries attempted. Please check your network connection.", null);
				this._fetchStatusRetryCount = 0;
				callback(null);
			}
		}
		yield break;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0002A0F4 File Offset: 0x000282F4
	private void OnFetchStatusResponse([CanBeNull] ProgressionController.GetQuestStatusResponse response)
	{
		this._isFetchingStatus = false;
		this._statusReceived = false;
		if (response != null)
		{
			this.SetProgressionValues(response.result.GetWeeklyPoints(), this.unclaimedPoints, response.result.userPointsTotal);
			this.ReportProgress();
			return;
		}
		GTDev.LogError<string>("Error: Could not fetch status!", null);
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0002A146 File Offset: 0x00028346
	private void SendQuestCompleted(int questId)
	{
		if (this._isSendingQuestComplete)
		{
			return;
		}
		this._isSendingQuestComplete = true;
		this.StartSendQuestComplete(questId);
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002A160 File Offset: 0x00028360
	private void StartSendQuestComplete(int questId)
	{
		base.StartCoroutine(this.DoSendQuestComplete(new ProgressionController.SetQuestCompleteRequest
		{
			PlayFabId = PlayFabAuthenticator.instance.GetPlayFabPlayerId(),
			PlayFabTicket = PlayFabAuthenticator.instance.GetPlayFabSessionTicket(),
			MothershipId = "",
			MothershipToken = "",
			QuestId = questId,
			ClientVersion = Application.version
		}, new Action<ProgressionController.SetQuestCompleteResponse>(this.OnSendQuestCompleteSuccess)));
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0002A1D7 File Offset: 0x000283D7
	private IEnumerator DoSendQuestComplete(ProgressionController.SetQuestCompleteRequest data, Action<ProgressionController.SetQuestCompleteResponse> callback)
	{
		UnityWebRequest request = new UnityWebRequest(PlayFabAuthenticatorSettings.ProgressionApiBaseUrl + "/api/SetQuestComplete", "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
		bool retry = false;
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.Success)
		{
			ProgressionController.SetQuestCompleteResponse obj = JsonConvert.DeserializeObject<ProgressionController.SetQuestCompleteResponse>(request.downloadHandler.text);
			callback(obj);
			this.ProcessQuestSubmittedSuccess();
		}
		else
		{
			long responseCode = request.responseCode;
			if (responseCode >= 500L && responseCode < 600L)
			{
				retry = true;
			}
			else if (request.responseCode == 403L)
			{
				GTDev.LogWarning<string>("User already reached the max number of completion points for this time period!", null);
				callback(null);
				this.ClearQuestQueue();
			}
			else if (request.result == UnityWebRequest.Result.ConnectionError)
			{
				retry = true;
			}
		}
		if (retry)
		{
			if (this._sendQuestCompleteRetryCount < this._maxRetriesOnFail)
			{
				int num = (int)Mathf.Pow(2f, (float)(this._sendQuestCompleteRetryCount + 1));
				this._sendQuestCompleteRetryCount++;
				yield return new WaitForSeconds((float)num);
				this.StartSendQuestComplete(data.QuestId);
			}
			else
			{
				GTDev.LogError<string>("Maximum SendQuestComplete retries attempted. Please check your network connection.", null);
				this._sendQuestCompleteRetryCount = 0;
				callback(null);
				this.ProcessQuestSubmittedFail();
			}
		}
		else
		{
			this._isSendingQuestComplete = false;
		}
		yield break;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0002A1F4 File Offset: 0x000283F4
	private void OnSendQuestCompleteSuccess([CanBeNull] ProgressionController.SetQuestCompleteResponse response)
	{
		this._isSendingQuestComplete = false;
		if (response != null)
		{
			this.UpdateProgressionValues(response.result.GetWeeklyPoints(), response.result.userPointsTotal);
			this.ReportProgress();
		}
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0002A222 File Offset: 0x00028422
	private void OnQuestProgressChanged(bool initialLoad)
	{
		this.ReportProgress();
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0002A22A File Offset: 0x0002842A
	private void OnQuestComplete(int questId, bool isDaily)
	{
		this.QueueQuestCompletion(questId, isDaily);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0002A234 File Offset: 0x00028434
	private void QueueQuestCompletion(int questId, bool isDaily)
	{
		if (isDaily)
		{
			this._queuedDailyCompletedQuests.Add(questId);
		}
		else
		{
			this._queuedWeeklyCompletedQuests.Add(questId);
		}
		this.SaveCompletedQuestQueue();
		this.SubmitNextQuestInQueue();
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0002A260 File Offset: 0x00028460
	private void SubmitNextQuestInQueue()
	{
		if (this._currentlyProcessingQuest == -1 && this.AreCompletedQuestsQueued())
		{
			int num = -1;
			if (this._queuedWeeklyCompletedQuests.Count > 0)
			{
				num = this._queuedWeeklyCompletedQuests[0];
			}
			else if (this._queuedDailyCompletedQuests.Count > 0)
			{
				num = this._queuedDailyCompletedQuests[0];
			}
			this._currentlyProcessingQuest = num;
			this.SendQuestCompleted(num);
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0002A2C6 File Offset: 0x000284C6
	private void ClearQuestQueue()
	{
		this._currentlyProcessingQuest = -1;
		this._queuedDailyCompletedQuests.Clear();
		this._queuedWeeklyCompletedQuests.Clear();
		this.SaveCompletedQuestQueue();
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0002A2EC File Offset: 0x000284EC
	private void ProcessQuestSubmittedSuccess()
	{
		if (this._currentlyProcessingQuest != -1)
		{
			if (this.AreCompletedQuestsQueued())
			{
				if (this._queuedWeeklyCompletedQuests.Remove(this._currentlyProcessingQuest))
				{
					this.SaveCompletedQuestQueue();
				}
				else if (this._queuedDailyCompletedQuests.Remove(this._currentlyProcessingQuest))
				{
					this.SaveCompletedQuestQueue();
				}
			}
			this._currentlyProcessingQuest = -1;
			this.SubmitNextQuestInQueue();
		}
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0002A34B File Offset: 0x0002854B
	private void ProcessQuestSubmittedFail()
	{
		this._currentlyProcessingQuest = -1;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x0002A354 File Offset: 0x00028554
	private bool AreCompletedQuestsQueued()
	{
		return this._queuedDailyCompletedQuests.Count > 0 || this._queuedWeeklyCompletedQuests.Count > 0;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0002A374 File Offset: 0x00028574
	private void SaveCompletedQuestQueue()
	{
		int num = 0;
		for (int i = 0; i < this._queuedDailyCompletedQuests.Count; i++)
		{
			PlayerPrefs.SetInt(string.Format("{0}{1}", "Queued_Quest_Daily_ID_Key", num), this._queuedDailyCompletedQuests[i]);
			num++;
		}
		int dailyQuestSetID = this._questManager.dailyQuestSetID;
		PlayerPrefs.SetInt("Queued_Quest_Daily_SetID_Key", dailyQuestSetID);
		PlayerPrefs.SetInt("Queued_Quest_Daily_SaveCount_Key", num);
		int num2 = 0;
		for (int j = 0; j < this._queuedWeeklyCompletedQuests.Count; j++)
		{
			PlayerPrefs.SetInt(string.Format("{0}{1}", "Queued_Quest_Weekly_ID_Key", num2), this._queuedWeeklyCompletedQuests[j]);
			num2++;
		}
		int weeklyQuestSetID = this._questManager.weeklyQuestSetID;
		PlayerPrefs.SetInt("Queued_Quest_Weekly_SetID_Key", weeklyQuestSetID);
		PlayerPrefs.SetInt("Queued_Quest_Weekly_SaveCount_Key", num2);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0002A454 File Offset: 0x00028654
	private void LoadCompletedQuestQueue()
	{
		this._queuedDailyCompletedQuests.Clear();
		int @int = PlayerPrefs.GetInt("Queued_Quest_Daily_SetID_Key", -1);
		int int2 = PlayerPrefs.GetInt("Queued_Quest_Daily_SaveCount_Key", -1);
		int dailyQuestSetID = this._questManager.dailyQuestSetID;
		if (@int == dailyQuestSetID)
		{
			for (int i = 0; i < int2; i++)
			{
				int int3 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Queued_Quest_Daily_ID_Key", i), -1);
				if (int3 != -1)
				{
					this._queuedDailyCompletedQuests.Add(int3);
				}
			}
		}
		this._queuedWeeklyCompletedQuests.Clear();
		int int4 = PlayerPrefs.GetInt("Queued_Quest_Weekly_SetID_Key", -1);
		int int5 = PlayerPrefs.GetInt("Queued_Quest_Weekly_SaveCount_Key", -1);
		int weeklyQuestSetID = this._questManager.weeklyQuestSetID;
		if (int4 == weeklyQuestSetID)
		{
			for (int j = 0; j < int5; j++)
			{
				int int6 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Queued_Quest_Weekly_ID_Key", j), -1);
				if (int6 != -1)
				{
					this._queuedWeeklyCompletedQuests.Add(int6);
				}
			}
		}
		this.SubmitNextQuestInQueue();
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0002A54C File Offset: 0x0002874C
	private void RequestProgressRedemption(Action onComplete)
	{
		ProgressionController.<RequestProgressRedemption>d__66 <RequestProgressRedemption>d__;
		<RequestProgressRedemption>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestProgressRedemption>d__.onComplete = onComplete;
		<RequestProgressRedemption>d__.<>1__state = -1;
		<RequestProgressRedemption>d__.<>t__builder.Start<ProgressionController.<RequestProgressRedemption>d__66>(ref <RequestProgressRedemption>d__);
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0002A583 File Offset: 0x00028783
	private void OnProgressRedeemed()
	{
		this.unclaimedPoints = 0;
		PlayerPrefs.SetInt("Claimed_Points_Key", this.unclaimedPoints);
		this.ReportProgress();
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0002A5A4 File Offset: 0x000287A4
	private void AddPoints(int points)
	{
		if (this.weeklyPoints >= ProgressionController.WeeklyCap)
		{
			return;
		}
		int num = Mathf.Clamp(points, 0, ProgressionController.WeeklyCap - this.weeklyPoints);
		this.SetProgressionValues(this.weeklyPoints + num, this.unclaimedPoints + num, this.totalPointsRaw + num);
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0002A5F4 File Offset: 0x000287F4
	private void UpdateProgressionValues(int weekly, int totalRaw)
	{
		int num = totalRaw - this.totalPointsRaw;
		this.unclaimedPoints += num;
		this.SetProgressionValues(weekly, this.unclaimedPoints, totalRaw);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0002A626 File Offset: 0x00028826
	private void SetProgressionValues(int weekly, int unclaimed, int totalRaw)
	{
		this.weeklyPoints = weekly;
		this.unclaimedPoints = unclaimed;
		this.totalPointsRaw = totalRaw;
		this.ReportScoreChange();
		PlayerPrefs.SetInt("Claimed_Points_Key", unclaimed);
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0002A650 File Offset: 0x00028850
	private void ReportProgress()
	{
		ProgressionController.<ReportProgress>d__71 <ReportProgress>d__;
		<ReportProgress>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ReportProgress>d__.<>4__this = this;
		<ReportProgress>d__.<>1__state = -1;
		<ReportProgress>d__.<>t__builder.Start<ProgressionController.<ReportProgress>d__71>(ref <ReportProgress>d__);
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0002A688 File Offset: 0x00028888
	private void ReportScoreChange()
	{
		ValueTuple<int, int, int> valueTuple = new ValueTuple<int, int, int>(this.weeklyPoints, this.unclaimedPoints, this.totalPointsRaw);
		ValueTuple<int, int, int> lastProgressReport = this._lastProgressReport;
		ValueTuple<int, int, int> valueTuple2 = valueTuple;
		if (lastProgressReport.Item1 == valueTuple2.Item1 && lastProgressReport.Item2 == valueTuple2.Item2 && lastProgressReport.Item3 == valueTuple2.Item3)
		{
			return;
		}
		if (VRRig.LocalRig)
		{
			VRRig.LocalRig.SetQuestScore(ProgressionController.TotalPoints);
		}
		this._lastProgressReport = valueTuple;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0002A704 File Offset: 0x00028904
	[return: TupleElementNames(new string[]
	{
		"weekly",
		"unclaimed",
		"total"
	})]
	private ValueTuple<int, int, int> GetProgress()
	{
		return new ValueTuple<int, int, int>(this.weeklyPoints, this.unclaimedPoints, this.totalPointsRaw - this.unclaimedPoints);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0002A759 File Offset: 0x00028959
	[CompilerGenerated]
	private bool <RequestStatus>g__ShouldFetchStatus|36_0()
	{
		return !this._isFetchingStatus && !this._statusReceived;
	}

	// Token: 0x040008D3 RID: 2259
	private static ProgressionController _gInstance;

	// Token: 0x040008D6 RID: 2262
	[SerializeField]
	private RotatingQuestsManager _questManager;

	// Token: 0x040008D7 RID: 2263
	private int weeklyPoints;

	// Token: 0x040008D8 RID: 2264
	private int totalPointsRaw;

	// Token: 0x040008D9 RID: 2265
	private int unclaimedPoints;

	// Token: 0x040008DA RID: 2266
	private bool _progressReportPending;

	// Token: 0x040008DB RID: 2267
	[TupleElementNames(new string[]
	{
		"weeklyPoints",
		"unclaimedPoints",
		"totalPointsRaw"
	})]
	private ValueTuple<int, int, int> _lastProgressReport;

	// Token: 0x040008DC RID: 2268
	private bool _isFetchingStatus;

	// Token: 0x040008DD RID: 2269
	private bool _statusReceived;

	// Token: 0x040008DE RID: 2270
	private bool _isSendingQuestComplete;

	// Token: 0x040008DF RID: 2271
	private int _fetchStatusRetryCount;

	// Token: 0x040008E0 RID: 2272
	private int _sendQuestCompleteRetryCount;

	// Token: 0x040008E1 RID: 2273
	private int _maxRetriesOnFail = 3;

	// Token: 0x040008E2 RID: 2274
	private List<int> _queuedDailyCompletedQuests = new List<int>();

	// Token: 0x040008E3 RID: 2275
	private List<int> _queuedWeeklyCompletedQuests = new List<int>();

	// Token: 0x040008E4 RID: 2276
	private int _currentlyProcessingQuest = -1;

	// Token: 0x040008E5 RID: 2277
	private const string kUnclaimedPointKey = "Claimed_Points_Key";

	// Token: 0x040008E7 RID: 2279
	private const string kQueuedDailyQuestSetIDKey = "Queued_Quest_Daily_SetID_Key";

	// Token: 0x040008E8 RID: 2280
	private const string kQueuedDailyQuestSaveCountKey = "Queued_Quest_Daily_SaveCount_Key";

	// Token: 0x040008E9 RID: 2281
	private const string kQueuedDailyQuestIDKey = "Queued_Quest_Daily_ID_Key";

	// Token: 0x040008EA RID: 2282
	private const string kQueuedWeeklyQuestSetIDKey = "Queued_Quest_Weekly_SetID_Key";

	// Token: 0x040008EB RID: 2283
	private const string kQueuedWeeklyQuestSaveCountKey = "Queued_Quest_Weekly_SaveCount_Key";

	// Token: 0x040008EC RID: 2284
	private const string kQueuedWeeklyQuestIDKey = "Queued_Quest_Weekly_ID_Key";

	// Token: 0x02000116 RID: 278
	[Serializable]
	private class GetQuestsStatusRequest
	{
		// Token: 0x040008ED RID: 2285
		public string PlayFabId;

		// Token: 0x040008EE RID: 2286
		public string PlayFabTicket;

		// Token: 0x040008EF RID: 2287
		public string MothershipId;

		// Token: 0x040008F0 RID: 2288
		public string MothershipToken;
	}

	// Token: 0x02000117 RID: 279
	[Serializable]
	public class GetQuestStatusResponse
	{
		// Token: 0x040008F1 RID: 2289
		public ProgressionController.UserQuestsStatus result;
	}

	// Token: 0x02000118 RID: 280
	public class UserQuestsStatus
	{
		// Token: 0x060007AF RID: 1967 RVA: 0x0002A770 File Offset: 0x00028970
		public int GetWeeklyPoints()
		{
			int num = 0;
			if (this.dailyPoints != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in this.dailyPoints)
				{
					num += keyValuePair.Value;
				}
			}
			if (this.weeklyPoints != null)
			{
				foreach (KeyValuePair<int, int> keyValuePair2 in this.weeklyPoints)
				{
					num += keyValuePair2.Value;
				}
			}
			return Mathf.Min(num, ProgressionController.WeeklyCap);
		}

		// Token: 0x040008F2 RID: 2290
		public Dictionary<string, int> dailyPoints;

		// Token: 0x040008F3 RID: 2291
		public Dictionary<int, int> weeklyPoints;

		// Token: 0x040008F4 RID: 2292
		public int userPointsTotal;
	}

	// Token: 0x02000119 RID: 281
	[Serializable]
	private class SetQuestCompleteRequest
	{
		// Token: 0x040008F5 RID: 2293
		public string PlayFabId;

		// Token: 0x040008F6 RID: 2294
		public string PlayFabTicket;

		// Token: 0x040008F7 RID: 2295
		public string MothershipId;

		// Token: 0x040008F8 RID: 2296
		public string MothershipToken;

		// Token: 0x040008F9 RID: 2297
		public int QuestId;

		// Token: 0x040008FA RID: 2298
		public string ClientVersion;
	}

	// Token: 0x0200011A RID: 282
	[Serializable]
	public class SetQuestCompleteResponse
	{
		// Token: 0x040008FB RID: 2299
		public ProgressionController.UserQuestsStatus result;
	}
}
