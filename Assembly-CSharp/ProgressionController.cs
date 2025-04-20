using System;
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

// Token: 0x0200011F RID: 287
public class ProgressionController : MonoBehaviour
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060007C5 RID: 1989 RVA: 0x0008B874 File Offset: 0x00089A74
	// (remove) Token: 0x060007C6 RID: 1990 RVA: 0x0008B8A8 File Offset: 0x00089AA8
	public static event Action OnQuestSelectionChanged;

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060007C7 RID: 1991 RVA: 0x0008B8DC File Offset: 0x00089ADC
	// (remove) Token: 0x060007C8 RID: 1992 RVA: 0x0008B910 File Offset: 0x00089B10
	public static event Action OnProgressEvent;

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000357EA File Offset: 0x000339EA
	// (set) Token: 0x060007CA RID: 1994 RVA: 0x000357F1 File Offset: 0x000339F1
	public static int WeeklyCap { get; private set; } = 25;

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x000357F9 File Offset: 0x000339F9
	public static int TotalPoints
	{
		get
		{
			return ProgressionController._gInstance.totalPointsRaw - ProgressionController._gInstance.unclaimedPoints;
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00035810 File Offset: 0x00033A10
	public static void ReportQuestChanged(bool initialLoad)
	{
		ProgressionController._gInstance.OnQuestProgressChanged(initialLoad);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0003581D File Offset: 0x00033A1D
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

	// Token: 0x060007CE RID: 1998 RVA: 0x00035838 File Offset: 0x00033A38
	public static void ReportQuestComplete(int questId, bool isDaily)
	{
		ProgressionController._gInstance.OnQuestComplete(questId, isDaily);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00035846 File Offset: 0x00033A46
	public static void RedeemProgress()
	{
		ProgressionController._gInstance.RequestProgressRedemption(new Action(ProgressionController._gInstance.OnProgressRedeemed));
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00035862 File Offset: 0x00033A62
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

	// Token: 0x060007D1 RID: 2001 RVA: 0x0003586E File Offset: 0x00033A6E
	public static void RequestProgressUpdate()
	{
		ProgressionController gInstance = ProgressionController._gInstance;
		if (gInstance == null)
		{
			return;
		}
		gInstance.ReportProgress();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0008B944 File Offset: 0x00089B44
	private void Awake()
	{
		if (ProgressionController._gInstance)
		{
			Debug.LogError("Duplicate ProgressionController detected. Destroying self.", base.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		ProgressionController._gInstance = this;
		this.unclaimedPoints = PlayerPrefs.GetInt("Claimed_Points_Key", 0);
		this.RequestStatus();
		this.LoadCompletedQuestQueue();
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0008B998 File Offset: 0x00089B98
	private void RequestStatus()
	{
		ProgressionController.<RequestStatus>d__36 <RequestStatus>d__;
		<RequestStatus>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestStatus>d__.<>4__this = this;
		<RequestStatus>d__.<>1__state = -1;
		<RequestStatus>d__.<>t__builder.Start<ProgressionController.<RequestStatus>d__36>(ref <RequestStatus>d__);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0008B9D0 File Offset: 0x00089BD0
	private Task WaitForSessionToken()
	{
		ProgressionController.<WaitForSessionToken>d__37 <WaitForSessionToken>d__;
		<WaitForSessionToken>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForSessionToken>d__.<>1__state = -1;
		<WaitForSessionToken>d__.<>t__builder.Start<ProgressionController.<WaitForSessionToken>d__37>(ref <WaitForSessionToken>d__);
		return <WaitForSessionToken>d__.<>t__builder.Task;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0008BA0C File Offset: 0x00089C0C
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

	// Token: 0x060007D6 RID: 2006 RVA: 0x0003587F File Offset: 0x00033A7F
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0008BA74 File Offset: 0x00089C74
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

	// Token: 0x060007D8 RID: 2008 RVA: 0x0003589C File Offset: 0x00033A9C
	private void SendQuestCompleted(int questId)
	{
		if (this._isSendingQuestComplete)
		{
			return;
		}
		this._isSendingQuestComplete = true;
		this.StartSendQuestComplete(questId);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0008BAC8 File Offset: 0x00089CC8
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

	// Token: 0x060007DA RID: 2010 RVA: 0x000358B5 File Offset: 0x00033AB5
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

	// Token: 0x060007DB RID: 2011 RVA: 0x000358D2 File Offset: 0x00033AD2
	private void OnSendQuestCompleteSuccess([CanBeNull] ProgressionController.SetQuestCompleteResponse response)
	{
		this._isSendingQuestComplete = false;
		if (response != null)
		{
			this.UpdateProgressionValues(response.result.GetWeeklyPoints(), response.result.userPointsTotal);
			this.ReportProgress();
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00035900 File Offset: 0x00033B00
	private void OnQuestProgressChanged(bool initialLoad)
	{
		this.ReportProgress();
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00035908 File Offset: 0x00033B08
	private void OnQuestComplete(int questId, bool isDaily)
	{
		this.QueueQuestCompletion(questId, isDaily);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00035912 File Offset: 0x00033B12
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

	// Token: 0x060007DF RID: 2015 RVA: 0x0008BB40 File Offset: 0x00089D40
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

	// Token: 0x060007E0 RID: 2016 RVA: 0x0003593D File Offset: 0x00033B3D
	private void ClearQuestQueue()
	{
		this._currentlyProcessingQuest = -1;
		this._queuedDailyCompletedQuests.Clear();
		this._queuedWeeklyCompletedQuests.Clear();
		this.SaveCompletedQuestQueue();
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0008BBA8 File Offset: 0x00089DA8
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

	// Token: 0x060007E2 RID: 2018 RVA: 0x00035962 File Offset: 0x00033B62
	private void ProcessQuestSubmittedFail()
	{
		this._currentlyProcessingQuest = -1;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0003596B File Offset: 0x00033B6B
	private bool AreCompletedQuestsQueued()
	{
		return this._queuedDailyCompletedQuests.Count > 0 || this._queuedWeeklyCompletedQuests.Count > 0;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0008BC08 File Offset: 0x00089E08
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

	// Token: 0x060007E5 RID: 2021 RVA: 0x0008BCE8 File Offset: 0x00089EE8
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

	// Token: 0x060007E6 RID: 2022 RVA: 0x0008BDE0 File Offset: 0x00089FE0
	private void RequestProgressRedemption(Action onComplete)
	{
		ProgressionController.<RequestProgressRedemption>d__66 <RequestProgressRedemption>d__;
		<RequestProgressRedemption>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<RequestProgressRedemption>d__.onComplete = onComplete;
		<RequestProgressRedemption>d__.<>1__state = -1;
		<RequestProgressRedemption>d__.<>t__builder.Start<ProgressionController.<RequestProgressRedemption>d__66>(ref <RequestProgressRedemption>d__);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0003598B File Offset: 0x00033B8B
	private void OnProgressRedeemed()
	{
		this.unclaimedPoints = 0;
		PlayerPrefs.SetInt("Claimed_Points_Key", this.unclaimedPoints);
		this.ReportProgress();
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0008BE18 File Offset: 0x0008A018
	private void AddPoints(int points)
	{
		if (this.weeklyPoints >= ProgressionController.WeeklyCap)
		{
			return;
		}
		int num = Mathf.Clamp(points, 0, ProgressionController.WeeklyCap - this.weeklyPoints);
		this.SetProgressionValues(this.weeklyPoints + num, this.unclaimedPoints + num, this.totalPointsRaw + num);
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0008BE68 File Offset: 0x0008A068
	private void UpdateProgressionValues(int weekly, int totalRaw)
	{
		int num = totalRaw - this.totalPointsRaw;
		this.unclaimedPoints += num;
		this.SetProgressionValues(weekly, this.unclaimedPoints, totalRaw);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x000359AA File Offset: 0x00033BAA
	private void SetProgressionValues(int weekly, int unclaimed, int totalRaw)
	{
		this.weeklyPoints = weekly;
		this.unclaimedPoints = unclaimed;
		this.totalPointsRaw = totalRaw;
		this.ReportScoreChange();
		PlayerPrefs.SetInt("Claimed_Points_Key", unclaimed);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0008BE9C File Offset: 0x0008A09C
	private void ReportProgress()
	{
		ProgressionController.<ReportProgress>d__71 <ReportProgress>d__;
		<ReportProgress>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ReportProgress>d__.<>4__this = this;
		<ReportProgress>d__.<>1__state = -1;
		<ReportProgress>d__.<>t__builder.Start<ProgressionController.<ReportProgress>d__71>(ref <ReportProgress>d__);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0008BED4 File Offset: 0x0008A0D4
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

	// Token: 0x060007ED RID: 2029 RVA: 0x000359D2 File Offset: 0x00033BD2
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

	// Token: 0x060007F0 RID: 2032 RVA: 0x00035A27 File Offset: 0x00033C27
	[CompilerGenerated]
	private bool <RequestStatus>g__ShouldFetchStatus|36_0()
	{
		return !this._isFetchingStatus && !this._statusReceived;
	}

	// Token: 0x04000915 RID: 2325
	private static ProgressionController _gInstance;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private RotatingQuestsManager _questManager;

	// Token: 0x04000919 RID: 2329
	private int weeklyPoints;

	// Token: 0x0400091A RID: 2330
	private int totalPointsRaw;

	// Token: 0x0400091B RID: 2331
	private int unclaimedPoints;

	// Token: 0x0400091C RID: 2332
	private bool _progressReportPending;

	// Token: 0x0400091D RID: 2333
	[TupleElementNames(new string[]
	{
		"weeklyPoints",
		"unclaimedPoints",
		"totalPointsRaw"
	})]
	private ValueTuple<int, int, int> _lastProgressReport;

	// Token: 0x0400091E RID: 2334
	private bool _isFetchingStatus;

	// Token: 0x0400091F RID: 2335
	private bool _statusReceived;

	// Token: 0x04000920 RID: 2336
	private bool _isSendingQuestComplete;

	// Token: 0x04000921 RID: 2337
	private int _fetchStatusRetryCount;

	// Token: 0x04000922 RID: 2338
	private int _sendQuestCompleteRetryCount;

	// Token: 0x04000923 RID: 2339
	private int _maxRetriesOnFail = 3;

	// Token: 0x04000924 RID: 2340
	private List<int> _queuedDailyCompletedQuests = new List<int>();

	// Token: 0x04000925 RID: 2341
	private List<int> _queuedWeeklyCompletedQuests = new List<int>();

	// Token: 0x04000926 RID: 2342
	private int _currentlyProcessingQuest = -1;

	// Token: 0x04000927 RID: 2343
	private const string kUnclaimedPointKey = "Claimed_Points_Key";

	// Token: 0x04000929 RID: 2345
	private const string kQueuedDailyQuestSetIDKey = "Queued_Quest_Daily_SetID_Key";

	// Token: 0x0400092A RID: 2346
	private const string kQueuedDailyQuestSaveCountKey = "Queued_Quest_Daily_SaveCount_Key";

	// Token: 0x0400092B RID: 2347
	private const string kQueuedDailyQuestIDKey = "Queued_Quest_Daily_ID_Key";

	// Token: 0x0400092C RID: 2348
	private const string kQueuedWeeklyQuestSetIDKey = "Queued_Quest_Weekly_SetID_Key";

	// Token: 0x0400092D RID: 2349
	private const string kQueuedWeeklyQuestSaveCountKey = "Queued_Quest_Weekly_SaveCount_Key";

	// Token: 0x0400092E RID: 2350
	private const string kQueuedWeeklyQuestIDKey = "Queued_Quest_Weekly_ID_Key";

	// Token: 0x02000120 RID: 288
	[Serializable]
	private class GetQuestsStatusRequest
	{
		// Token: 0x0400092F RID: 2351
		public string PlayFabId;

		// Token: 0x04000930 RID: 2352
		public string PlayFabTicket;

		// Token: 0x04000931 RID: 2353
		public string MothershipId;

		// Token: 0x04000932 RID: 2354
		public string MothershipToken;
	}

	// Token: 0x02000121 RID: 289
	[Serializable]
	public class GetQuestStatusResponse
	{
		// Token: 0x04000933 RID: 2355
		public ProgressionController.UserQuestsStatus result;
	}

	// Token: 0x02000122 RID: 290
	public class UserQuestsStatus
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x0008BF50 File Offset: 0x0008A150
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

		// Token: 0x04000934 RID: 2356
		public Dictionary<string, int> dailyPoints;

		// Token: 0x04000935 RID: 2357
		public Dictionary<int, int> weeklyPoints;

		// Token: 0x04000936 RID: 2358
		public int userPointsTotal;
	}

	// Token: 0x02000123 RID: 291
	[Serializable]
	private class SetQuestCompleteRequest
	{
		// Token: 0x04000937 RID: 2359
		public string PlayFabId;

		// Token: 0x04000938 RID: 2360
		public string PlayFabTicket;

		// Token: 0x04000939 RID: 2361
		public string MothershipId;

		// Token: 0x0400093A RID: 2362
		public string MothershipToken;

		// Token: 0x0400093B RID: 2363
		public int QuestId;

		// Token: 0x0400093C RID: 2364
		public string ClientVersion;
	}

	// Token: 0x02000124 RID: 292
	[Serializable]
	public class SetQuestCompleteResponse
	{
		// Token: 0x0400093D RID: 2365
		public ProgressionController.UserQuestsStatus result;
	}
}
