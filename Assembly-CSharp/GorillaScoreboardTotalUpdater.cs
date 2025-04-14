using System;
using System.Collections.Generic;
using GorillaNetworking;
using GorillaTag;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class GorillaScoreboardTotalUpdater : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060028C2 RID: 10434 RVA: 0x000C8404 File Offset: 0x000C6604
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

	// Token: 0x060028C3 RID: 10435 RVA: 0x000C846E File Offset: 0x000C666E
	protected void Awake()
	{
		if (GorillaScoreboardTotalUpdater.hasInstance && GorillaScoreboardTotalUpdater.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		GorillaScoreboardTotalUpdater.SetInstance(this);
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000C8494 File Offset: 0x000C6694
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.JoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerEnteredRoom));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x000C8521 File Offset: 0x000C6721
	public static void CreateManager()
	{
		GorillaScoreboardTotalUpdater.SetInstance(new GameObject("GorillaScoreboardTotalUpdater").AddComponent<GorillaScoreboardTotalUpdater>());
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x000C8537 File Offset: 0x000C6737
	private static void SetInstance(GorillaScoreboardTotalUpdater manager)
	{
		GorillaScoreboardTotalUpdater.instance = manager;
		GorillaScoreboardTotalUpdater.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x000C8552 File Offset: 0x000C6752
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

	// Token: 0x060028C8 RID: 10440 RVA: 0x000C8578 File Offset: 0x000C6778
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

	// Token: 0x060028C9 RID: 10441 RVA: 0x000C859F File Offset: 0x000C679F
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

	// Token: 0x060028CA RID: 10442 RVA: 0x000C85D0 File Offset: 0x000C67D0
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

	// Token: 0x060028CB RID: 10443 RVA: 0x000C85F8 File Offset: 0x000C67F8
	public void UpdateActiveScoreboards()
	{
		for (int i = 0; i < GorillaScoreboardTotalUpdater.allScoreboards.Count; i++)
		{
			this.UpdateScoreboard(GorillaScoreboardTotalUpdater.allScoreboards[i]);
		}
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x000C862B File Offset: 0x000C682B
	public void SetOfflineFailureText(string failureText)
	{
		this.offlineTextErrorString = failureText;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000C863A File Offset: 0x000C683A
	public void ClearOfflineFailureText()
	{
		this.offlineTextErrorString = null;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000C864C File Offset: 0x000C684C
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

	// Token: 0x060028CF RID: 10447 RVA: 0x000158F9 File Offset: 0x00013AF9
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x00015902 File Offset: 0x00013B02
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000C8774 File Offset: 0x000C6974
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

	// Token: 0x060028D2 RID: 10450 RVA: 0x000C880D File Offset: 0x000C6A0D
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

	// Token: 0x060028D3 RID: 10451 RVA: 0x000C883C File Offset: 0x000C6A3C
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

	// Token: 0x060028D4 RID: 10452 RVA: 0x000C889C File Offset: 0x000C6A9C
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

	// Token: 0x060028D5 RID: 10453 RVA: 0x000C894C File Offset: 0x000C6B4C
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

	// Token: 0x060028D6 RID: 10454 RVA: 0x000C8A10 File Offset: 0x000C6C10
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

	// Token: 0x060028D9 RID: 10457 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002DCB RID: 11723
	public static GorillaScoreboardTotalUpdater instance;

	// Token: 0x04002DCC RID: 11724
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04002DCD RID: 11725
	public static List<GorillaPlayerScoreboardLine> allScoreboardLines = new List<GorillaPlayerScoreboardLine>();

	// Token: 0x04002DCE RID: 11726
	public static int lineIndex = 0;

	// Token: 0x04002DCF RID: 11727
	private static int linesPerFrame = 2;

	// Token: 0x04002DD0 RID: 11728
	public static List<GorillaScoreBoard> allScoreboards = new List<GorillaScoreBoard>();

	// Token: 0x04002DD1 RID: 11729
	public static int boardIndex = 0;

	// Token: 0x04002DD2 RID: 11730
	private List<NetPlayer> playersInRoom = new List<NetPlayer>();

	// Token: 0x04002DD3 RID: 11731
	private bool joinedRoom;

	// Token: 0x04002DD4 RID: 11732
	private bool wasGameManagerNull;

	// Token: 0x04002DD5 RID: 11733
	public bool forOverlay;

	// Token: 0x04002DD6 RID: 11734
	public string offlineTextErrorString;

	// Token: 0x04002DD7 RID: 11735
	public Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports> reportDict = new Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports>();

	// Token: 0x04002DD8 RID: 11736
	private static readonly Dictionary<int, ReportMuteTimer> m_reportMuteTimerDict = new Dictionary<int, ReportMuteTimer>(10);

	// Token: 0x04002DD9 RID: 11737
	private static readonly ObjectPool<ReportMuteTimer> m_reportMuteTimerPool = new ObjectPool<ReportMuteTimer>(10);

	// Token: 0x0200066F RID: 1647
	public struct PlayerReports
	{
		// Token: 0x060028DA RID: 10458 RVA: 0x000C8AFC File Offset: 0x000C6CFC
		public PlayerReports(GorillaScoreboardTotalUpdater.PlayerReports reportToUpdate, GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = (reportToUpdate.cheating || lineToUpdate.reportedCheating);
			this.toxicity = (reportToUpdate.toxicity || lineToUpdate.reportedToxicity);
			this.hateSpeech = (reportToUpdate.hateSpeech || lineToUpdate.reportedHateSpeech);
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000C8B5A File Offset: 0x000C6D5A
		public PlayerReports(GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = lineToUpdate.reportedCheating;
			this.toxicity = lineToUpdate.reportedToxicity;
			this.hateSpeech = lineToUpdate.reportedHateSpeech;
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x04002DDA RID: 11738
		public bool cheating;

		// Token: 0x04002DDB RID: 11739
		public bool toxicity;

		// Token: 0x04002DDC RID: 11740
		public bool hateSpeech;

		// Token: 0x04002DDD RID: 11741
		public bool pressedReport;
	}
}
