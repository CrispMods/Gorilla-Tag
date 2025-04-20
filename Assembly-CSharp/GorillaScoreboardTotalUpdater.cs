using System;
using System.Collections.Generic;
using GorillaNetworking;
using GorillaTag;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class GorillaScoreboardTotalUpdater : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060027ED RID: 10221 RVA: 0x0010F0E4 File Offset: 0x0010D2E4
	public void UpdateLineState(GorillaPlayerScoreboardLine line)
	{
		if (line.playerActorNumber == -1)
		{
			return;
		}
		if (this.reportDict.ContainsKey(line.playerActorNumber))
		{
			this.reportDict[line.playerActorNumber] = new GorillaScoreboardTotalUpdater.PlayerReports(this.reportDict[line.playerActorNumber], line);
			return;
		}
		this.reportDict.Add(line.playerActorNumber, new GorillaScoreboardTotalUpdater.PlayerReports(line));
	}

	// Token: 0x060027EE RID: 10222 RVA: 0x0004B213 File Offset: 0x00049413
	protected void Awake()
	{
		if (GorillaScoreboardTotalUpdater.hasInstance && GorillaScoreboardTotalUpdater.instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		GorillaScoreboardTotalUpdater.SetInstance(this);
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x0010F150 File Offset: 0x0010D350
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.JoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerEnteredRoom));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x0004B236 File Offset: 0x00049436
	public static void CreateManager()
	{
		GorillaScoreboardTotalUpdater.SetInstance(new GameObject("GorillaScoreboardTotalUpdater").AddComponent<GorillaScoreboardTotalUpdater>());
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x0004B24C File Offset: 0x0004944C
	private static void SetInstance(GorillaScoreboardTotalUpdater manager)
	{
		GorillaScoreboardTotalUpdater.instance = manager;
		GorillaScoreboardTotalUpdater.hasInstance = true;
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x0004B267 File Offset: 0x00049467
	public static void RegisterSL(GorillaPlayerScoreboardLine sL)
	{
		if (!GorillaScoreboardTotalUpdater.hasInstance)
		{
			GorillaScoreboardTotalUpdater.CreateManager();
		}
		if (!GorillaScoreboardTotalUpdater.allScoreboardLines.Contains(sL))
		{
			GorillaScoreboardTotalUpdater.allScoreboardLines.Add(sL);
		}
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x0004B28D File Offset: 0x0004948D
	public static void UnregisterSL(GorillaPlayerScoreboardLine sL)
	{
		if (!GorillaScoreboardTotalUpdater.hasInstance)
		{
			GorillaScoreboardTotalUpdater.CreateManager();
		}
		if (GorillaScoreboardTotalUpdater.allScoreboardLines.Contains(sL))
		{
			GorillaScoreboardTotalUpdater.allScoreboardLines.Remove(sL);
		}
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x0004B2B4 File Offset: 0x000494B4
	public static void RegisterScoreboard(GorillaScoreBoard sB)
	{
		if (!GorillaScoreboardTotalUpdater.hasInstance)
		{
			GorillaScoreboardTotalUpdater.CreateManager();
		}
		if (!GorillaScoreboardTotalUpdater.allScoreboards.Contains(sB))
		{
			GorillaScoreboardTotalUpdater.allScoreboards.Add(sB);
			GorillaScoreboardTotalUpdater.instance.UpdateScoreboard(sB);
		}
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x0004B2E5 File Offset: 0x000494E5
	public static void UnregisterScoreboard(GorillaScoreBoard sB)
	{
		if (!GorillaScoreboardTotalUpdater.hasInstance)
		{
			GorillaScoreboardTotalUpdater.CreateManager();
		}
		if (GorillaScoreboardTotalUpdater.allScoreboards.Contains(sB))
		{
			GorillaScoreboardTotalUpdater.allScoreboards.Remove(sB);
		}
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x0010F1E0 File Offset: 0x0010D3E0
	public void UpdateActiveScoreboards()
	{
		for (int i = 0; i < GorillaScoreboardTotalUpdater.allScoreboards.Count; i++)
		{
			this.UpdateScoreboard(GorillaScoreboardTotalUpdater.allScoreboards[i]);
		}
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x0004B30C File Offset: 0x0004950C
	public void SetOfflineFailureText(string failureText)
	{
		this.offlineTextErrorString = failureText;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060027F8 RID: 10232 RVA: 0x0004B31B File Offset: 0x0004951B
	public void ClearOfflineFailureText()
	{
		this.offlineTextErrorString = null;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x0010F214 File Offset: 0x0010D414
	public void UpdateScoreboard(GorillaScoreBoard sB)
	{
		sB.SetSleepState(this.joinedRoom);
		if (GorillaComputer.instance == null)
		{
			return;
		}
		if (!this.joinedRoom)
		{
			if (sB.notInRoomText != null)
			{
				sB.notInRoomText.gameObject.SetActive(true);
				sB.notInRoomText.text = ((this.offlineTextErrorString != null) ? this.offlineTextErrorString : GorillaComputer.instance.offlineTextInitialString);
			}
			for (int i = 0; i < sB.lines.Count; i++)
			{
				sB.lines[i].ResetData();
			}
			return;
		}
		if (sB.notInRoomText != null)
		{
			sB.notInRoomText.gameObject.SetActive(false);
		}
		for (int j = 0; j < sB.lines.Count; j++)
		{
			GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine = sB.lines[j];
			if (j < this.playersInRoom.Count)
			{
				gorillaPlayerScoreboardLine.gameObject.SetActive(true);
				gorillaPlayerScoreboardLine.SetLineData(this.playersInRoom[j]);
			}
			else
			{
				gorillaPlayerScoreboardLine.ResetData();
				gorillaPlayerScoreboardLine.gameObject.SetActive(false);
			}
		}
		sB.RedrawPlayerLines();
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x00032C89 File Offset: 0x00030E89
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x00032C92 File Offset: 0x00030E92
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x0010F33C File Offset: 0x0010D53C
	public void SliceUpdate()
	{
		if (GorillaScoreboardTotalUpdater.allScoreboardLines.Count == 0)
		{
			return;
		}
		for (int i = 0; i < GorillaScoreboardTotalUpdater.linesPerFrame; i++)
		{
			if (GorillaScoreboardTotalUpdater.lineIndex >= GorillaScoreboardTotalUpdater.allScoreboardLines.Count)
			{
				GorillaScoreboardTotalUpdater.lineIndex = 0;
			}
			GorillaScoreboardTotalUpdater.allScoreboardLines[GorillaScoreboardTotalUpdater.lineIndex].UpdateLine();
			GorillaScoreboardTotalUpdater.lineIndex++;
		}
		for (int j = 0; j < GorillaScoreboardTotalUpdater.allScoreboards.Count; j++)
		{
			if (GorillaScoreboardTotalUpdater.allScoreboards[j].IsDirty)
			{
				this.UpdateScoreboard(GorillaScoreboardTotalUpdater.allScoreboards[j]);
			}
		}
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x0004B32A File Offset: 0x0004952A
	private void OnPlayerEnteredRoom(NetPlayer netPlayer)
	{
		if (netPlayer == null)
		{
			Debug.LogError("Null netplayer");
		}
		if (!this.playersInRoom.Contains(netPlayer))
		{
			this.playersInRoom.Add(netPlayer);
		}
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x0010F3D8 File Offset: 0x0010D5D8
	private void OnPlayerLeftRoom(NetPlayer netPlayer)
	{
		if (netPlayer == null)
		{
			Debug.LogError("Null netplayer");
		}
		this.playersInRoom.Remove(netPlayer);
		this.UpdateActiveScoreboards();
		ReportMuteTimer reportMuteTimer;
		if (GorillaScoreboardTotalUpdater.m_reportMuteTimerDict.TryGetValue(netPlayer.ActorNumber, out reportMuteTimer))
		{
			GorillaScoreboardTotalUpdater.m_reportMuteTimerDict.Remove(netPlayer.ActorNumber);
			GorillaScoreboardTotalUpdater.m_reportMuteTimerPool.Return(reportMuteTimer);
		}
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x0010F438 File Offset: 0x0010D638
	internal void JoinedRoom()
	{
		this.joinedRoom = true;
		foreach (NetPlayer item in NetworkSystem.Instance.AllNetPlayers)
		{
			this.playersInRoom.Add(item);
		}
		this.playersInRoom.Sort((NetPlayer x, NetPlayer y) => x.ActorNumber.CompareTo(y.ActorNumber));
		foreach (GorillaScoreBoard sB in GorillaScoreboardTotalUpdater.allScoreboards)
		{
			this.UpdateScoreboard(sB);
		}
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x0010F4E8 File Offset: 0x0010D6E8
	private void OnLeftRoom()
	{
		this.joinedRoom = false;
		this.playersInRoom.Clear();
		this.reportDict.Clear();
		foreach (GorillaScoreBoard sB in GorillaScoreboardTotalUpdater.allScoreboards)
		{
			this.UpdateScoreboard(sB);
		}
		foreach (KeyValuePair<int, ReportMuteTimer> keyValuePair in GorillaScoreboardTotalUpdater.m_reportMuteTimerDict)
		{
			GorillaScoreboardTotalUpdater.m_reportMuteTimerPool.Return(keyValuePair.Value);
		}
		GorillaScoreboardTotalUpdater.m_reportMuteTimerDict.Clear();
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x0010F5AC File Offset: 0x0010D7AC
	public static void ReportMute(NetPlayer player, int muted)
	{
		ReportMuteTimer reportMuteTimer;
		if (GorillaScoreboardTotalUpdater.m_reportMuteTimerDict.TryGetValue(player.ActorNumber, out reportMuteTimer))
		{
			reportMuteTimer.Muted = muted;
			if (!reportMuteTimer.Running)
			{
				reportMuteTimer.Start();
			}
			return;
		}
		reportMuteTimer = GorillaScoreboardTotalUpdater.m_reportMuteTimerPool.Take();
		reportMuteTimer.SetReportData(player.UserId, player.NickName, muted);
		reportMuteTimer.coolDown = 5f;
		reportMuteTimer.Start();
		GorillaScoreboardTotalUpdater.m_reportMuteTimerDict[player.ActorNumber] = reportMuteTimer;
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002D31 RID: 11569
	public static GorillaScoreboardTotalUpdater instance;

	// Token: 0x04002D32 RID: 11570
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04002D33 RID: 11571
	public static List<GorillaPlayerScoreboardLine> allScoreboardLines = new List<GorillaPlayerScoreboardLine>();

	// Token: 0x04002D34 RID: 11572
	public static int lineIndex = 0;

	// Token: 0x04002D35 RID: 11573
	private static int linesPerFrame = 2;

	// Token: 0x04002D36 RID: 11574
	public static List<GorillaScoreBoard> allScoreboards = new List<GorillaScoreBoard>();

	// Token: 0x04002D37 RID: 11575
	public static int boardIndex = 0;

	// Token: 0x04002D38 RID: 11576
	private List<NetPlayer> playersInRoom = new List<NetPlayer>();

	// Token: 0x04002D39 RID: 11577
	private bool joinedRoom;

	// Token: 0x04002D3A RID: 11578
	private bool wasGameManagerNull;

	// Token: 0x04002D3B RID: 11579
	public bool forOverlay;

	// Token: 0x04002D3C RID: 11580
	public string offlineTextErrorString;

	// Token: 0x04002D3D RID: 11581
	public Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports> reportDict = new Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports>();

	// Token: 0x04002D3E RID: 11582
	private static readonly Dictionary<int, ReportMuteTimer> m_reportMuteTimerDict = new Dictionary<int, ReportMuteTimer>(10);

	// Token: 0x04002D3F RID: 11583
	private static readonly ObjectPool<ReportMuteTimer> m_reportMuteTimerPool = new ObjectPool<ReportMuteTimer>(10);

	// Token: 0x0200064E RID: 1614
	public struct PlayerReports
	{
		// Token: 0x06002805 RID: 10245 RVA: 0x0010F678 File Offset: 0x0010D878
		public PlayerReports(GorillaScoreboardTotalUpdater.PlayerReports reportToUpdate, GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = (reportToUpdate.cheating || lineToUpdate.reportedCheating);
			this.toxicity = (reportToUpdate.toxicity || lineToUpdate.reportedToxicity);
			this.hateSpeech = (reportToUpdate.hateSpeech || lineToUpdate.reportedHateSpeech);
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x0004B377 File Offset: 0x00049577
		public PlayerReports(GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = lineToUpdate.reportedCheating;
			this.toxicity = lineToUpdate.reportedToxicity;
			this.hateSpeech = lineToUpdate.reportedHateSpeech;
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x04002D40 RID: 11584
		public bool cheating;

		// Token: 0x04002D41 RID: 11585
		public bool toxicity;

		// Token: 0x04002D42 RID: 11586
		public bool hateSpeech;

		// Token: 0x04002D43 RID: 11587
		public bool pressedReport;
	}
}
