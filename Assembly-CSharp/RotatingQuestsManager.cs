using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlayFab;
using UnityEngine;

// Token: 0x02000130 RID: 304
public class RotatingQuestsManager : MonoBehaviour, ITickSystemTick
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000823 RID: 2083 RVA: 0x00035B92 File Offset: 0x00033D92
	// (set) Token: 0x06000824 RID: 2084 RVA: 0x00035B9A File Offset: 0x00033D9A
	public bool TickRunning { get; set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x00035BA3 File Offset: 0x00033DA3
	// (set) Token: 0x06000826 RID: 2086 RVA: 0x00035BAB File Offset: 0x00033DAB
	public DateTime DailyQuestCountdown { get; private set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x00035BB4 File Offset: 0x00033DB4
	// (set) Token: 0x06000828 RID: 2088 RVA: 0x00035BBC File Offset: 0x00033DBC
	public DateTime WeeklyQuestCountdown { get; private set; }

	// Token: 0x06000829 RID: 2089 RVA: 0x00035BC5 File Offset: 0x00033DC5
	private void Start()
	{
		this._questAudio = base.GetComponent<AudioSource>();
		this.RequestQuestsFromTitleData();
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00035BD9 File Offset: 0x00033DD9
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00035BE1 File Offset: 0x00033DE1
	private void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00035BE9 File Offset: 0x00033DE9
	public void Tick()
	{
		if (this.hasQuest && this.nextQuestUpdateTime < DateTime.UtcNow)
		{
			this.SetupQuests();
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0008CAD4 File Offset: 0x0008ACD4
	private void ProcessAllQuests(Action<RotatingQuestsManager.RotatingQuest> action)
	{
		RotatingQuestsManager.<>c__DisplayClass30_0 CS$<>8__locals1;
		CS$<>8__locals1.action = action;
		RotatingQuestsManager.<ProcessAllQuests>g__ProcessAllQuestsInList|30_0(this.quests.DailyQuests, ref CS$<>8__locals1);
		RotatingQuestsManager.<ProcessAllQuests>g__ProcessAllQuestsInList|30_0(this.quests.WeeklyQuests, ref CS$<>8__locals1);
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00035C0B File Offset: 0x00033E0B
	private void QuestLoadPostProcess(RotatingQuestsManager.RotatingQuest quest)
	{
		if (quest.requiredZones.Count == 1 && quest.requiredZones[0] == GTZone.none)
		{
			quest.requiredZones.Clear();
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00035C36 File Offset: 0x00033E36
	private void QuestSavePreProcess(RotatingQuestsManager.RotatingQuest quest)
	{
		if (quest.requiredZones.Count == 0)
		{
			quest.requiredZones.Add(GTZone.none);
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0008CB10 File Offset: 0x0008AD10
	public void LoadTestQuestsFromFile()
	{
		TextAsset textAsset = Resources.Load<TextAsset>(this.localQuestPath);
		this.LoadQuestsFromJson(textAsset.text);
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00035C52 File Offset: 0x00033E52
	public void RequestQuestsFromTitleData()
	{
		PlayFabTitleDataCache.Instance.GetTitleData("AllActiveQuests", delegate(string data)
		{
			this.LoadQuestsFromJson(data);
		}, delegate(PlayFabError e)
		{
			Debug.LogError(string.Format("Error getting AllActiveQuests data: {0}", e));
		});
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0008CB38 File Offset: 0x0008AD38
	private void LoadQuestsFromJson(string jsonString)
	{
		this.quests = JsonConvert.DeserializeObject<RotatingQuestsManager.RotatingQuestList>(jsonString);
		this.ProcessAllQuests(new Action<RotatingQuestsManager.RotatingQuest>(this.QuestLoadPostProcess));
		if (this.quests == null)
		{
			Debug.LogError("Error: Quests failed to parse!");
			return;
		}
		this.hasQuest = true;
		this.quests.Init();
		if (Application.isPlaying)
		{
			this.SetupQuests();
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0008CB98 File Offset: 0x0008AD98
	private void SetupQuests()
	{
		this.ClearAllQuestEventListeners();
		this.SelectActiveQuests();
		this.LoadQuestProgress();
		this.HandleQuestProgressChanged(true);
		this.SetupAllQuestEventListeners();
		this.nextQuestUpdateTime = this.DailyQuestCountdown;
		this.nextQuestUpdateTime = this.nextQuestUpdateTime.AddMinutes(1.0);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0008CBEC File Offset: 0x0008ADEC
	private void SelectActiveQuests()
	{
		DateTime dateTime = new DateTime(2025, 1, 10, 18, 0, 0, DateTimeKind.Utc);
		TimeSpan timeSpan = TimeSpan.FromHours(-8.0);
		DateTime dateStart = new DateTime(1, 1, 1, 0, 0, 0);
		DateTime dateEnd = new DateTime(2006, 12, 31, 0, 0, 0);
		TimeSpan daylightDelta = TimeSpan.FromHours(1.0);
		TimeZoneInfo.TransitionTime daylightTransitionStart = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 4, 1, DayOfWeek.Sunday);
		TimeZoneInfo.TransitionTime daylightTransitionEnd = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 10, 5, DayOfWeek.Sunday);
		DateTime dateStart2 = new DateTime(2007, 1, 1, 0, 0, 0);
		DateTime dateEnd2 = new DateTime(9999, 12, 31, 0, 0, 0);
		TimeSpan daylightDelta2 = TimeSpan.FromHours(1.0);
		TimeZoneInfo.TransitionTime daylightTransitionStart2 = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 3, 2, DayOfWeek.Sunday);
		TimeZoneInfo.TransitionTime daylightTransitionEnd2 = TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 11, 1, DayOfWeek.Sunday);
		TimeZoneInfo timeZoneInfo = TimeZoneInfo.CreateCustomTimeZone("Pacific Standard Time", timeSpan, "Pacific Standard Time", "Pacific Standard Time", "Pacific Standard Time", new TimeZoneInfo.AdjustmentRule[]
		{
			TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart, dateEnd, daylightDelta, daylightTransitionStart, daylightTransitionEnd),
			TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart2, dateEnd2, daylightDelta2, daylightTransitionStart2, daylightTransitionEnd2)
		});
		if (timeZoneInfo != null && timeZoneInfo.IsDaylightSavingTime(DateTime.UtcNow - timeSpan))
		{
			dateTime -= TimeSpan.FromHours(1.0);
		}
		TimeSpan timeSpan2 = DateTime.UtcNow - dateTime;
		this.RemoveDisabledQuests();
		int days = timeSpan2.Days;
		this.dailyQuestSetID = days;
		this.weeklyQuestSetID = days / 7;
		RotatingQuestsManager.LastQuestDailyID = this.dailyQuestSetID;
		this.DailyQuestCountdown = dateTime + TimeSpan.FromDays((double)(this.dailyQuestSetID + 1));
		this.WeeklyQuestCountdown = dateTime + TimeSpan.FromDays((double)((this.weeklyQuestSetID + 1) * 7));
		UnityEngine.Random.InitState(this.dailyQuestSetID);
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in this.quests.DailyQuests)
		{
			int num = Math.Min(rotatingQuestGroup.selectCount, rotatingQuestGroup.quests.Count);
			float num2 = 0f;
			List<ValueTuple<int, float>> list = new List<ValueTuple<int, float>>(rotatingQuestGroup.quests.Count);
			for (int i = 0; i < rotatingQuestGroup.quests.Count; i++)
			{
				rotatingQuestGroup.quests[i].isQuestActive = false;
				num2 += rotatingQuestGroup.quests[i].weight;
				list.Add(new ValueTuple<int, float>(i, rotatingQuestGroup.quests[i].weight));
			}
			for (int j = 0; j < num; j++)
			{
				float num3 = UnityEngine.Random.Range(0f, num2);
				for (int k = 0; k < list.Count; k++)
				{
					float item = list[k].Item2;
					if (num3 <= item || k == list.Count - 1)
					{
						num2 -= item;
						int item2 = list[k].Item1;
						list.RemoveAt(k);
						rotatingQuestGroup.quests[item2].isQuestActive = true;
						rotatingQuestGroup.quests[item2].SetRequiredZone();
						break;
					}
					num3 -= item;
				}
			}
		}
		UnityEngine.Random.InitState(this.weeklyQuestSetID);
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup2 in this.quests.WeeklyQuests)
		{
			int num4 = Math.Min(rotatingQuestGroup2.selectCount, rotatingQuestGroup2.quests.Count);
			float num5 = 0f;
			List<ValueTuple<int, float>> list2 = new List<ValueTuple<int, float>>(rotatingQuestGroup2.quests.Count);
			for (int l = 0; l < rotatingQuestGroup2.quests.Count; l++)
			{
				rotatingQuestGroup2.quests[l].isQuestActive = false;
				num5 += rotatingQuestGroup2.quests[l].weight;
				list2.Add(new ValueTuple<int, float>(l, rotatingQuestGroup2.quests[l].weight));
			}
			for (int m = 0; m < num4; m++)
			{
				float num6 = UnityEngine.Random.Range(0f, num5);
				for (int n = 0; n < list2.Count; n++)
				{
					float item3 = list2[n].Item2;
					if (num6 <= item3 || n == list2.Count - 1)
					{
						num5 -= item3;
						int item4 = list2[n].Item1;
						list2.RemoveAt(n);
						rotatingQuestGroup2.quests[item4].isQuestActive = true;
						rotatingQuestGroup2.quests[item4].SetRequiredZone();
						break;
					}
					num6 -= item3;
				}
			}
		}
		ProgressionController.ReportQuestSelectionChanged();
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00035C8E File Offset: 0x00033E8E
	private void RemoveDisabledQuests()
	{
		RotatingQuestsManager.<RemoveDisabledQuests>g__RemoveDisabledQuestsFromGroupList|38_0(this.quests.DailyQuests);
		RotatingQuestsManager.<RemoveDisabledQuests>g__RemoveDisabledQuestsFromGroupList|38_0(this.quests.WeeklyQuests);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0008D120 File Offset: 0x0008B320
	private void LoadQuestProgress()
	{
		int @int = PlayerPrefs.GetInt("Rotating_Quest_Daily_SetID_Key", -1);
		int int2 = PlayerPrefs.GetInt("Rotating_Quest_Daily_SaveCount_Key", -1);
		if (@int == this.dailyQuestSetID)
		{
			for (int i = 0; i < int2; i++)
			{
				int int3 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Rotating_Quest_Daily_ID_Key", i), -1);
				int int4 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Rotating_Quest_Daily_Progress_Key", i), -1);
				if (int3 != -1)
				{
					for (int j = 0; j < this.quests.DailyQuests.Count; j++)
					{
						for (int k = 0; k < this.quests.DailyQuests[j].quests.Count; k++)
						{
							RotatingQuestsManager.RotatingQuest rotatingQuest = this.quests.DailyQuests[j].quests[k];
							if (rotatingQuest.questID == int3)
							{
								rotatingQuest.ApplySavedProgress(int4);
								break;
							}
						}
					}
				}
			}
		}
		int int5 = PlayerPrefs.GetInt("Rotating_Quest_Weekly_SetID_Key", -1);
		int int6 = PlayerPrefs.GetInt("Rotating_Quest_Weekly_SaveCount_Key", -1);
		if (int5 == this.weeklyQuestSetID)
		{
			for (int l = 0; l < int6; l++)
			{
				int int7 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Rotating_Quest_Weekly_ID_Key", l), -1);
				int int8 = PlayerPrefs.GetInt(string.Format("{0}{1}", "Rotating_Quest_Weekly_Progress_Key", l), -1);
				if (int7 != -1)
				{
					for (int m = 0; m < this.quests.WeeklyQuests.Count; m++)
					{
						for (int n = 0; n < this.quests.WeeklyQuests[m].quests.Count; n++)
						{
							RotatingQuestsManager.RotatingQuest rotatingQuest2 = this.quests.WeeklyQuests[m].quests[n];
							if (rotatingQuest2.questID == int7)
							{
								rotatingQuest2.ApplySavedProgress(int8);
								break;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0008D31C File Offset: 0x0008B51C
	private void SaveQuestProgress()
	{
		int num = 0;
		for (int i = 0; i < this.quests.DailyQuests.Count; i++)
		{
			for (int j = 0; j < this.quests.DailyQuests[i].quests.Count; j++)
			{
				RotatingQuestsManager.RotatingQuest rotatingQuest = this.quests.DailyQuests[i].quests[j];
				int progress = rotatingQuest.GetProgress();
				if (progress > 0)
				{
					PlayerPrefs.SetInt(string.Format("{0}{1}", "Rotating_Quest_Daily_ID_Key", num), rotatingQuest.questID);
					PlayerPrefs.SetInt(string.Format("{0}{1}", "Rotating_Quest_Daily_Progress_Key", num), progress);
					num++;
				}
			}
		}
		if (num > 0)
		{
			PlayerPrefs.SetInt("Rotating_Quest_Daily_SetID_Key", this.dailyQuestSetID);
			PlayerPrefs.SetInt("Rotating_Quest_Daily_SaveCount_Key", num);
		}
		int num2 = 0;
		for (int k = 0; k < this.quests.WeeklyQuests.Count; k++)
		{
			for (int l = 0; l < this.quests.WeeklyQuests[k].quests.Count; l++)
			{
				RotatingQuestsManager.RotatingQuest rotatingQuest2 = this.quests.WeeklyQuests[k].quests[l];
				int progress2 = rotatingQuest2.GetProgress();
				if (progress2 > 0)
				{
					PlayerPrefs.SetInt(string.Format("{0}{1}", "Rotating_Quest_Weekly_ID_Key", num2), rotatingQuest2.questID);
					PlayerPrefs.SetInt(string.Format("{0}{1}", "Rotating_Quest_Weekly_Progress_Key", num2), progress2);
					num2++;
				}
			}
		}
		if (num2 > 0)
		{
			PlayerPrefs.SetInt("Rotating_Quest_Weekly_SetID_Key", this.weeklyQuestSetID);
			PlayerPrefs.SetInt("Rotating_Quest_Weekly_SaveCount_Key", num2);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0008D4EC File Offset: 0x0008B6EC
	private void SetupAllQuestEventListeners()
	{
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in this.quests.DailyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
			{
				rotatingQuest.questManager = this;
				if (rotatingQuest.isQuestActive && !rotatingQuest.isQuestComplete)
				{
					rotatingQuest.AddEventListener();
				}
			}
		}
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup2 in this.quests.WeeklyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest2 in rotatingQuestGroup2.quests)
			{
				rotatingQuest2.questManager = this;
				if (rotatingQuest2.isQuestActive && !rotatingQuest2.isQuestComplete)
				{
					rotatingQuest2.AddEventListener();
				}
			}
		}
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0008D62C File Offset: 0x0008B82C
	private void ClearAllQuestEventListeners()
	{
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in this.quests.DailyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
			{
				rotatingQuest.RemoveEventListener();
			}
		}
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup2 in this.quests.WeeklyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest2 in rotatingQuestGroup2.quests)
			{
				rotatingQuest2.RemoveEventListener();
			}
		}
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0008D738 File Offset: 0x0008B938
	private void HandleQuestCompleted(int questID)
	{
		RotatingQuestsManager.RotatingQuest quest = this.quests.GetQuest(questID);
		if (quest == null)
		{
			return;
		}
		ProgressionController.ReportQuestComplete(questID, quest.isDailyQuest);
		if (this._playQuestSounds)
		{
			AudioSource questAudio = this._questAudio;
			if (questAudio == null)
			{
				return;
			}
			questAudio.Play();
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00035CB0 File Offset: 0x00033EB0
	private void HandleQuestProgressChanged(bool initialLoad)
	{
		if (!initialLoad)
		{
			this.SaveQuestProgress();
		}
		RotatingQuestsManager.LastQuestChange = Time.frameCount;
		ProgressionController.ReportQuestChanged(initialLoad);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0008D77C File Offset: 0x0008B97C
	[CompilerGenerated]
	internal static void <ProcessAllQuests>g__ProcessAllQuestsInList|30_0(List<RotatingQuestsManager.RotatingQuestGroup> questGroups, ref RotatingQuestsManager.<>c__DisplayClass30_0 A_1)
	{
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in questGroups)
		{
			foreach (RotatingQuestsManager.RotatingQuest obj in rotatingQuestGroup.quests)
			{
				A_1.action(obj);
			}
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x0008D808 File Offset: 0x0008BA08
	[CompilerGenerated]
	internal static void <RemoveDisabledQuests>g__RemoveDisabledQuestsFromGroupList|38_0(List<RotatingQuestsManager.RotatingQuestGroup> questList)
	{
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in questList)
		{
			for (int i = rotatingQuestGroup.quests.Count - 1; i >= 0; i--)
			{
				if (rotatingQuestGroup.quests[i].disable)
				{
					rotatingQuestGroup.quests.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04000981 RID: 2433
	private bool hasQuest;

	// Token: 0x04000982 RID: 2434
	[SerializeField]
	private bool useTestLocalQuests;

	// Token: 0x04000983 RID: 2435
	[SerializeField]
	private string localQuestPath = "TestingRotatingQuests";

	// Token: 0x04000984 RID: 2436
	public static int LastQuestChange;

	// Token: 0x04000985 RID: 2437
	public static int LastQuestDailyID;

	// Token: 0x04000986 RID: 2438
	public RotatingQuestsManager.RotatingQuestList quests;

	// Token: 0x04000987 RID: 2439
	public int dailyQuestSetID;

	// Token: 0x04000988 RID: 2440
	public int weeklyQuestSetID;

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	private bool _playQuestSounds;

	// Token: 0x0400098A RID: 2442
	private AudioSource _questAudio;

	// Token: 0x0400098D RID: 2445
	private DateTime nextQuestUpdateTime;

	// Token: 0x0400098E RID: 2446
	private const string kDailyQuestSetIDKey = "Rotating_Quest_Daily_SetID_Key";

	// Token: 0x0400098F RID: 2447
	private const string kDailyQuestSaveCountKey = "Rotating_Quest_Daily_SaveCount_Key";

	// Token: 0x04000990 RID: 2448
	private const string kDailyQuestIDKey = "Rotating_Quest_Daily_ID_Key";

	// Token: 0x04000991 RID: 2449
	private const string kDailyQuestProgressKey = "Rotating_Quest_Daily_Progress_Key";

	// Token: 0x04000992 RID: 2450
	private const string kWeeklyQuestSetIDKey = "Rotating_Quest_Weekly_SetID_Key";

	// Token: 0x04000993 RID: 2451
	private const string kWeeklyQuestSaveCountKey = "Rotating_Quest_Weekly_SaveCount_Key";

	// Token: 0x04000994 RID: 2452
	private const string kWeeklyQuestIDKey = "Rotating_Quest_Weekly_ID_Key";

	// Token: 0x04000995 RID: 2453
	private const string kWeeklyQuestProgressKey = "Rotating_Quest_Weekly_Progress_Key";

	// Token: 0x02000131 RID: 305
	[Serializable]
	public class RotatingQuest
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00035CE7 File Offset: 0x00033EE7
		[JsonIgnore]
		public bool IsMovementQuest
		{
			get
			{
				return this.questType == QuestType.moveDistance || this.questType == QuestType.swimDistance;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x00035CFE File Offset: 0x00033EFE
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x00035D06 File Offset: 0x00033F06
		[JsonIgnore]
		public GTZone RequiredZone { get; private set; } = GTZone.none;

		// Token: 0x06000843 RID: 2115 RVA: 0x00035D0F File Offset: 0x00033F0F
		public void SetRequiredZone()
		{
			this.RequiredZone = ((this.requiredZones.Count > 0) ? this.requiredZones[UnityEngine.Random.Range(0, this.requiredZones.Count)] : GTZone.none);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0008D888 File Offset: 0x0008BA88
		public void AddEventListener()
		{
			if (this.isQuestComplete)
			{
				return;
			}
			switch (this.questType)
			{
			case QuestType.gameModeObjective:
				PlayerGameEvents.OnGameModeObjectiveTrigger += this.OnGameEventOccurence;
				return;
			case QuestType.gameModeRound:
				PlayerGameEvents.OnGameModeCompleteRound += this.OnGameEventOccurence;
				return;
			case QuestType.grabObject:
				PlayerGameEvents.OnGrabbedObject += this.OnGameEventOccurence;
				return;
			case QuestType.dropObject:
				PlayerGameEvents.OnDroppedObject += this.OnGameEventOccurence;
				return;
			case QuestType.eatObject:
				PlayerGameEvents.OnEatObject += this.OnGameEventOccurence;
				return;
			case QuestType.tapObject:
				PlayerGameEvents.OnTapObject += this.OnGameEventOccurence;
				return;
			case QuestType.launchedProjectile:
				PlayerGameEvents.OnLaunchedProjectile += this.OnGameEventOccurence;
				return;
			case QuestType.moveDistance:
				PlayerGameEvents.OnPlayerMoved += this.OnGameMoveEvent;
				return;
			case QuestType.swimDistance:
				PlayerGameEvents.OnPlayerSwam += this.OnGameMoveEvent;
				return;
			case QuestType.triggerHandEffect:
				PlayerGameEvents.OnTriggerHandEffect += this.OnGameEventOccurence;
				return;
			case QuestType.enterLocation:
				PlayerGameEvents.OnEnterLocation += this.OnGameEventOccurence;
				return;
			case QuestType.misc:
				PlayerGameEvents.OnMiscEvent += this.OnGameEventOccurence;
				return;
			case QuestType.critter:
				PlayerGameEvents.OnCritterEvent += this.OnGameEventOccurence;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0008D9CC File Offset: 0x0008BBCC
		public void RemoveEventListener()
		{
			switch (this.questType)
			{
			case QuestType.gameModeObjective:
				PlayerGameEvents.OnGameModeObjectiveTrigger -= this.OnGameEventOccurence;
				return;
			case QuestType.gameModeRound:
				PlayerGameEvents.OnGameModeCompleteRound -= this.OnGameEventOccurence;
				return;
			case QuestType.grabObject:
				PlayerGameEvents.OnGrabbedObject -= this.OnGameEventOccurence;
				return;
			case QuestType.dropObject:
				PlayerGameEvents.OnDroppedObject -= this.OnGameEventOccurence;
				return;
			case QuestType.eatObject:
				PlayerGameEvents.OnEatObject -= this.OnGameEventOccurence;
				return;
			case QuestType.tapObject:
				PlayerGameEvents.OnTapObject -= this.OnGameEventOccurence;
				return;
			case QuestType.launchedProjectile:
				PlayerGameEvents.OnLaunchedProjectile -= this.OnGameEventOccurence;
				return;
			case QuestType.moveDistance:
				PlayerGameEvents.OnPlayerMoved -= this.OnGameMoveEvent;
				return;
			case QuestType.swimDistance:
				PlayerGameEvents.OnPlayerSwam -= this.OnGameMoveEvent;
				return;
			case QuestType.triggerHandEffect:
				PlayerGameEvents.OnTriggerHandEffect -= this.OnGameEventOccurence;
				return;
			case QuestType.enterLocation:
				PlayerGameEvents.OnEnterLocation -= this.OnGameEventOccurence;
				return;
			case QuestType.misc:
				PlayerGameEvents.OnMiscEvent -= this.OnGameEventOccurence;
				return;
			case QuestType.critter:
				PlayerGameEvents.OnCritterEvent -= this.OnGameEventOccurence;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0008DB08 File Offset: 0x0008BD08
		public void ApplySavedProgress(int progress)
		{
			if (this.questType == QuestType.moveDistance || this.questType == QuestType.swimDistance)
			{
				this.moveDistance = (float)progress;
				this.occurenceCount = Mathf.FloorToInt(this.moveDistance);
				this.isQuestComplete = (this.occurenceCount >= this.requiredOccurenceCount);
				return;
			}
			this.occurenceCount = progress;
			this.isQuestComplete = (this.occurenceCount >= this.requiredOccurenceCount);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00035D45 File Offset: 0x00033F45
		public int GetProgress()
		{
			if (this.questType == QuestType.moveDistance || this.questType == QuestType.swimDistance)
			{
				return Mathf.FloorToInt(this.moveDistance);
			}
			return this.occurenceCount;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0008DB78 File Offset: 0x0008BD78
		private void OnGameEventOccurence(string eventName)
		{
			if (this.RequiredZone != GTZone.none && !ZoneManagement.IsInZone(this.RequiredZone))
			{
				return;
			}
			string.IsNullOrEmpty(this.questOccurenceFilter);
			if (eventName.StartsWith(this.questOccurenceFilter))
			{
				this.SetProgress(this.occurenceCount + 1);
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00035D6C File Offset: 0x00033F6C
		private void OnGameMoveEvent(float distance, float speed)
		{
			if (this.RequiredZone != GTZone.none && !ZoneManagement.IsInZone(this.RequiredZone))
			{
				return;
			}
			this.moveDistance += distance;
			this.SetProgress(Mathf.FloorToInt(this.moveDistance));
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0008DBC8 File Offset: 0x0008BDC8
		private void SetProgress(int progress)
		{
			if (this.isQuestComplete)
			{
				return;
			}
			if (this.occurenceCount == progress)
			{
				return;
			}
			this.lastChange = Time.frameCount;
			this.occurenceCount = progress;
			if (this.occurenceCount >= this.requiredOccurenceCount)
			{
				this.Complete();
			}
			this.questManager.HandleQuestProgressChanged(false);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00035DA5 File Offset: 0x00033FA5
		private void Complete()
		{
			if (this.isQuestComplete)
			{
				return;
			}
			this.isQuestComplete = true;
			this.RemoveEventListener();
			this.questManager.HandleQuestCompleted(this.questID);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00035DCE File Offset: 0x00033FCE
		public string GetTextDescription()
		{
			return this.<GetTextDescription>g__GetActionName|30_0().ToUpper() + this.<GetTextDescription>g__GetLocationText|30_1().ToUpper();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00035DEB File Offset: 0x00033FEB
		public string GetProgressText()
		{
			if (!this.isQuestComplete)
			{
				return string.Format("{0}/{1}", this.occurenceCount, this.requiredOccurenceCount);
			}
			return "[DONE]";
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0008DC1C File Offset: 0x0008BE1C
		[CompilerGenerated]
		private string <GetTextDescription>g__GetActionName|30_0()
		{
			switch (this.questType)
			{
			case QuestType.none:
				return "[UNDEFINED]";
			case QuestType.gameModeObjective:
				return this.questName;
			case QuestType.gameModeRound:
				return this.questName;
			case QuestType.grabObject:
				return this.questName;
			case QuestType.dropObject:
				return this.questName;
			case QuestType.eatObject:
				return this.questName;
			case QuestType.launchedProjectile:
				return this.questName;
			case QuestType.moveDistance:
				return this.questName;
			case QuestType.swimDistance:
				return this.questName;
			case QuestType.triggerHandEffect:
				return this.questName;
			case QuestType.enterLocation:
				return this.questName;
			case QuestType.misc:
				return this.questName;
			}
			return this.questName;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00035E48 File Offset: 0x00034048
		[CompilerGenerated]
		private string <GetTextDescription>g__GetLocationText|30_1()
		{
			if (this.RequiredZone == GTZone.none)
			{
				return "";
			}
			return string.Format(" IN {0}", this.RequiredZone);
		}

		// Token: 0x04000996 RID: 2454
		public bool disable;

		// Token: 0x04000997 RID: 2455
		public int questID;

		// Token: 0x04000998 RID: 2456
		public float weight = 1f;

		// Token: 0x04000999 RID: 2457
		public string questName = "UNNAMED QUEST";

		// Token: 0x0400099A RID: 2458
		public QuestType questType;

		// Token: 0x0400099B RID: 2459
		public string questOccurenceFilter;

		// Token: 0x0400099C RID: 2460
		public int requiredOccurenceCount = 1;

		// Token: 0x0400099D RID: 2461
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public List<GTZone> requiredZones;

		// Token: 0x0400099E RID: 2462
		[Space]
		[NonSerialized]
		public bool isQuestActive;

		// Token: 0x0400099F RID: 2463
		[NonSerialized]
		public bool isQuestComplete;

		// Token: 0x040009A0 RID: 2464
		[NonSerialized]
		public bool isDailyQuest;

		// Token: 0x040009A1 RID: 2465
		[NonSerialized]
		public int lastChange;

		// Token: 0x040009A3 RID: 2467
		[NonSerialized]
		public int occurenceCount;

		// Token: 0x040009A4 RID: 2468
		private float moveDistance;

		// Token: 0x040009A5 RID: 2469
		[NonSerialized]
		public RotatingQuestsManager questManager;
	}

	// Token: 0x02000132 RID: 306
	[Serializable]
	public class RotatingQuestGroup
	{
		// Token: 0x040009A6 RID: 2470
		public int selectCount;

		// Token: 0x040009A7 RID: 2471
		public string name;

		// Token: 0x040009A8 RID: 2472
		public List<RotatingQuestsManager.RotatingQuest> quests;
	}

	// Token: 0x02000133 RID: 307
	[Serializable]
	public class RotatingQuestList
	{
		// Token: 0x06000852 RID: 2130 RVA: 0x00035E6F File Offset: 0x0003406F
		public void Init()
		{
			RotatingQuestsManager.RotatingQuestList.<Init>g__SetIsDaily|2_0(this.DailyQuests, true);
			RotatingQuestsManager.RotatingQuestList.<Init>g__SetIsDaily|2_0(this.WeeklyQuests, false);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0008DCE0 File Offset: 0x0008BEE0
		public RotatingQuestsManager.RotatingQuest GetQuest(int questID)
		{
			RotatingQuestsManager.RotatingQuestList.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.questID = questID;
			RotatingQuestsManager.RotatingQuest rotatingQuest = RotatingQuestsManager.RotatingQuestList.<GetQuest>g__GetQuestFrom|3_0(this.DailyQuests, ref CS$<>8__locals1);
			if (rotatingQuest == null)
			{
				rotatingQuest = RotatingQuestsManager.RotatingQuestList.<GetQuest>g__GetQuestFrom|3_0(this.WeeklyQuests, ref CS$<>8__locals1);
			}
			return rotatingQuest;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0008DD18 File Offset: 0x0008BF18
		[CompilerGenerated]
		internal static void <Init>g__SetIsDaily|2_0(List<RotatingQuestsManager.RotatingQuestGroup> questList, bool isDaily)
		{
			foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in questList)
			{
				foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
				{
					rotatingQuest.isDailyQuest = isDaily;
				}
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0008DDA0 File Offset: 0x0008BFA0
		[CompilerGenerated]
		internal static RotatingQuestsManager.RotatingQuest <GetQuest>g__GetQuestFrom|3_0(List<RotatingQuestsManager.RotatingQuestGroup> list, ref RotatingQuestsManager.RotatingQuestList.<>c__DisplayClass3_0 A_1)
		{
			foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in list)
			{
				foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
				{
					if (rotatingQuest.questID == A_1.questID)
					{
						return rotatingQuest;
					}
				}
			}
			return null;
		}

		// Token: 0x040009A9 RID: 2473
		public List<RotatingQuestsManager.RotatingQuestGroup> DailyQuests;

		// Token: 0x040009AA RID: 2474
		public List<RotatingQuestsManager.RotatingQuestGroup> WeeklyQuests;
	}
}
