using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GorillaNetworking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PlayFab;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class RotatingQuestsManager : MonoBehaviour, ITickSystemTick
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002B44E File Offset: 0x0002964E
	// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0002B456 File Offset: 0x00029656
	public bool TickRunning { get; set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002B45F File Offset: 0x0002965F
	// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0002B467 File Offset: 0x00029667
	public DateTime DailyQuestCountdown { get; private set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002B470 File Offset: 0x00029670
	// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0002B478 File Offset: 0x00029678
	public DateTime WeeklyQuestCountdown { get; private set; }

	// Token: 0x060007E5 RID: 2021 RVA: 0x0002B481 File Offset: 0x00029681
	private void Start()
	{
		this._questAudio = base.GetComponent<AudioSource>();
		this.RequestQuestsFromTitleData();
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0002B495 File Offset: 0x00029695
	private void OnEnable()
	{
		TickSystem<object>.AddTickCallback(this);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0002B49D File Offset: 0x0002969D
	private void OnDisable()
	{
		TickSystem<object>.RemoveTickCallback(this);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0002B4A5 File Offset: 0x000296A5
	public void Tick()
	{
		if (this.hasQuest && this.nextQuestUpdateTime < DateTime.UtcNow)
		{
			this.SetupQuests();
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0002B4C8 File Offset: 0x000296C8
	private void ProcessAllQuests(Action<RotatingQuestsManager.RotatingQuest> action)
	{
		RotatingQuestsManager.<>c__DisplayClass30_0 CS$<>8__locals1;
		CS$<>8__locals1.action = action;
		RotatingQuestsManager.<ProcessAllQuests>g__ProcessAllQuestsInList|30_0(this.quests.DailyQuests, ref CS$<>8__locals1);
		RotatingQuestsManager.<ProcessAllQuests>g__ProcessAllQuestsInList|30_0(this.quests.WeeklyQuests, ref CS$<>8__locals1);
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0002B501 File Offset: 0x00029701
	private void QuestLoadPostProcess(RotatingQuestsManager.RotatingQuest quest)
	{
		if (quest.requiredZones.Count == 1 && quest.requiredZones[0] == GTZone.none)
		{
			quest.requiredZones.Clear();
		}
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0002B52C File Offset: 0x0002972C
	private void QuestSavePreProcess(RotatingQuestsManager.RotatingQuest quest)
	{
		if (quest.requiredZones.Count == 0)
		{
			quest.requiredZones.Add(GTZone.none);
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0002B548 File Offset: 0x00029748
	public void LoadTestQuestsFromFile()
	{
		TextAsset textAsset = Resources.Load<TextAsset>(this.localQuestPath);
		this.LoadQuestsFromJson(textAsset.text);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0002B56D File Offset: 0x0002976D
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

	// Token: 0x060007EE RID: 2030 RVA: 0x0002B5AC File Offset: 0x000297AC
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

	// Token: 0x060007EF RID: 2031 RVA: 0x0002B60C File Offset: 0x0002980C
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

	// Token: 0x060007F0 RID: 2032 RVA: 0x0002B660 File Offset: 0x00029860
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
		Random.InitState(this.dailyQuestSetID);
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
				float num3 = Random.Range(0f, num2);
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
		Random.InitState(this.weeklyQuestSetID);
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
				float num6 = Random.Range(0f, num5);
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

	// Token: 0x060007F1 RID: 2033 RVA: 0x0002BB94 File Offset: 0x00029D94
	private void RemoveDisabledQuests()
	{
		RotatingQuestsManager.<RemoveDisabledQuests>g__RemoveDisabledQuestsFromGroupList|38_0(this.quests.DailyQuests);
		RotatingQuestsManager.<RemoveDisabledQuests>g__RemoveDisabledQuestsFromGroupList|38_0(this.quests.WeeklyQuests);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0002BBB8 File Offset: 0x00029DB8
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

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002BDB4 File Offset: 0x00029FB4
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

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002BF84 File Offset: 0x0002A184
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

	// Token: 0x060007F5 RID: 2037 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
	private void ClearAllQuestEventListeners()
	{
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup in this.quests.DailyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest in rotatingQuestGroup.quests)
			{
				if (rotatingQuest.isQuestActive && !rotatingQuest.isQuestComplete)
				{
					rotatingQuest.RemoveEventListener();
				}
			}
		}
		foreach (RotatingQuestsManager.RotatingQuestGroup rotatingQuestGroup2 in this.quests.WeeklyQuests)
		{
			foreach (RotatingQuestsManager.RotatingQuest rotatingQuest2 in rotatingQuestGroup2.quests)
			{
				if (rotatingQuest2.isQuestActive && !rotatingQuest2.isQuestComplete)
				{
					rotatingQuest2.RemoveEventListener();
				}
			}
		}
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002C1F4 File Offset: 0x0002A3F4
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

	// Token: 0x060007F7 RID: 2039 RVA: 0x0002C236 File Offset: 0x0002A436
	private void HandleQuestProgressChanged(bool initialLoad)
	{
		if (!initialLoad)
		{
			this.SaveQuestProgress();
		}
		RotatingQuestsManager.LastQuestChange = Time.frameCount;
		ProgressionController.ReportQuestChanged(initialLoad);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002C264 File Offset: 0x0002A464
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

	// Token: 0x060007FB RID: 2043 RVA: 0x0002C2FC File Offset: 0x0002A4FC
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

	// Token: 0x0400093E RID: 2366
	private bool hasQuest;

	// Token: 0x0400093F RID: 2367
	[SerializeField]
	private bool useTestLocalQuests;

	// Token: 0x04000940 RID: 2368
	[SerializeField]
	private string localQuestPath = "TestingRotatingQuests";

	// Token: 0x04000941 RID: 2369
	public static int LastQuestChange;

	// Token: 0x04000942 RID: 2370
	public static int LastQuestDailyID;

	// Token: 0x04000943 RID: 2371
	public RotatingQuestsManager.RotatingQuestList quests;

	// Token: 0x04000944 RID: 2372
	public int dailyQuestSetID;

	// Token: 0x04000945 RID: 2373
	public int weeklyQuestSetID;

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	private bool _playQuestSounds;

	// Token: 0x04000947 RID: 2375
	private AudioSource _questAudio;

	// Token: 0x0400094A RID: 2378
	private DateTime nextQuestUpdateTime;

	// Token: 0x0400094B RID: 2379
	private const string kDailyQuestSetIDKey = "Rotating_Quest_Daily_SetID_Key";

	// Token: 0x0400094C RID: 2380
	private const string kDailyQuestSaveCountKey = "Rotating_Quest_Daily_SaveCount_Key";

	// Token: 0x0400094D RID: 2381
	private const string kDailyQuestIDKey = "Rotating_Quest_Daily_ID_Key";

	// Token: 0x0400094E RID: 2382
	private const string kDailyQuestProgressKey = "Rotating_Quest_Daily_Progress_Key";

	// Token: 0x0400094F RID: 2383
	private const string kWeeklyQuestSetIDKey = "Rotating_Quest_Weekly_SetID_Key";

	// Token: 0x04000950 RID: 2384
	private const string kWeeklyQuestSaveCountKey = "Rotating_Quest_Weekly_SaveCount_Key";

	// Token: 0x04000951 RID: 2385
	private const string kWeeklyQuestIDKey = "Rotating_Quest_Weekly_ID_Key";

	// Token: 0x04000952 RID: 2386
	private const string kWeeklyQuestProgressKey = "Rotating_Quest_Weekly_Progress_Key";

	// Token: 0x02000127 RID: 295
	[Serializable]
	public class RotatingQuest
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0002C37C File Offset: 0x0002A57C
		[JsonIgnore]
		public bool IsMovementQuest
		{
			get
			{
				return this.questType == QuestType.moveDistance || this.questType == QuestType.swimDistance;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x0002C393 File Offset: 0x0002A593
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x0002C39B File Offset: 0x0002A59B
		[JsonIgnore]
		public GTZone RequiredZone { get; private set; } = GTZone.none;

		// Token: 0x060007FF RID: 2047 RVA: 0x0002C3A4 File Offset: 0x0002A5A4
		public void SetRequiredZone()
		{
			this.RequiredZone = ((this.requiredZones.Count > 0) ? this.requiredZones[Random.Range(0, this.requiredZones.Count)] : GTZone.none);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002C3DC File Offset: 0x0002A5DC
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
			default:
				return;
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002C50C File Offset: 0x0002A70C
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
			default:
				return;
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002C630 File Offset: 0x0002A830
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

		// Token: 0x06000803 RID: 2051 RVA: 0x0002C69F File Offset: 0x0002A89F
		public int GetProgress()
		{
			if (this.questType == QuestType.moveDistance || this.questType == QuestType.swimDistance)
			{
				return Mathf.FloorToInt(this.moveDistance);
			}
			return this.occurenceCount;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002C6C8 File Offset: 0x0002A8C8
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

		// Token: 0x06000805 RID: 2053 RVA: 0x0002C715 File Offset: 0x0002A915
		private void OnGameMoveEvent(float distance, float speed)
		{
			if (this.RequiredZone != GTZone.none && !ZoneManagement.IsInZone(this.RequiredZone))
			{
				return;
			}
			this.moveDistance += distance;
			this.SetProgress(Mathf.FloorToInt(this.moveDistance));
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002C750 File Offset: 0x0002A950
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

		// Token: 0x06000807 RID: 2055 RVA: 0x0002C7A2 File Offset: 0x0002A9A2
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

		// Token: 0x06000808 RID: 2056 RVA: 0x0002C7CB File Offset: 0x0002A9CB
		public string GetTextDescription()
		{
			return this.<GetTextDescription>g__GetActionName|30_0().ToUpper() + this.<GetTextDescription>g__GetLocationText|30_1().ToUpper();
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002C7E8 File Offset: 0x0002A9E8
		public string GetProgressText()
		{
			if (!this.isQuestComplete)
			{
				return string.Format("{0}/{1}", this.occurenceCount, this.requiredOccurenceCount);
			}
			return "[DONE]";
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002C848 File Offset: 0x0002AA48
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

		// Token: 0x0600080C RID: 2060 RVA: 0x0002C90B File Offset: 0x0002AB0B
		[CompilerGenerated]
		private string <GetTextDescription>g__GetLocationText|30_1()
		{
			if (this.RequiredZone == GTZone.none)
			{
				return "";
			}
			return string.Format(" IN {0}", this.RequiredZone);
		}

		// Token: 0x04000953 RID: 2387
		public bool disable;

		// Token: 0x04000954 RID: 2388
		public int questID;

		// Token: 0x04000955 RID: 2389
		public float weight = 1f;

		// Token: 0x04000956 RID: 2390
		public string questName = "UNNAMED QUEST";

		// Token: 0x04000957 RID: 2391
		public QuestType questType;

		// Token: 0x04000958 RID: 2392
		public string questOccurenceFilter;

		// Token: 0x04000959 RID: 2393
		public int requiredOccurenceCount = 1;

		// Token: 0x0400095A RID: 2394
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public List<GTZone> requiredZones;

		// Token: 0x0400095B RID: 2395
		[Space]
		[NonSerialized]
		public bool isQuestActive;

		// Token: 0x0400095C RID: 2396
		[NonSerialized]
		public bool isQuestComplete;

		// Token: 0x0400095D RID: 2397
		[NonSerialized]
		public bool isDailyQuest;

		// Token: 0x0400095E RID: 2398
		[NonSerialized]
		public int lastChange;

		// Token: 0x04000960 RID: 2400
		[NonSerialized]
		public int occurenceCount;

		// Token: 0x04000961 RID: 2401
		private float moveDistance;

		// Token: 0x04000962 RID: 2402
		[NonSerialized]
		public RotatingQuestsManager questManager;
	}

	// Token: 0x02000128 RID: 296
	[Serializable]
	public class RotatingQuestGroup
	{
		// Token: 0x04000963 RID: 2403
		public int selectCount;

		// Token: 0x04000964 RID: 2404
		public string name;

		// Token: 0x04000965 RID: 2405
		public List<RotatingQuestsManager.RotatingQuest> quests;
	}

	// Token: 0x02000129 RID: 297
	[Serializable]
	public class RotatingQuestList
	{
		// Token: 0x0600080E RID: 2062 RVA: 0x0002C932 File Offset: 0x0002AB32
		public void Init()
		{
			RotatingQuestsManager.RotatingQuestList.<Init>g__SetIsDaily|2_0(this.DailyQuests, true);
			RotatingQuestsManager.RotatingQuestList.<Init>g__SetIsDaily|2_0(this.WeeklyQuests, false);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002C94C File Offset: 0x0002AB4C
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

		// Token: 0x06000811 RID: 2065 RVA: 0x0002C984 File Offset: 0x0002AB84
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

		// Token: 0x06000812 RID: 2066 RVA: 0x0002CA0C File Offset: 0x0002AC0C
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

		// Token: 0x04000966 RID: 2406
		public List<RotatingQuestsManager.RotatingQuestGroup> DailyQuests;

		// Token: 0x04000967 RID: 2407
		public List<RotatingQuestsManager.RotatingQuestGroup> WeeklyQuests;
	}
}
