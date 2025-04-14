using System;
using System.Collections.Generic;
using GorillaNetworking;
using GorillaTag;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public class GorillaScoreboardTotalUpdater : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x060028CA RID: 10442 RVA: 0x000C8884 File Offset: 0x000C6A84
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

	// Token: 0x060028CB RID: 10443 RVA: 0x000C88EE File Offset: 0x000C6AEE
	protected void Awake()
	{
		if (GorillaScoreboardTotalUpdater.hasInstance && GorillaScoreboardTotalUpdater.instance != this)
		{
			Object.Destroy(this);
			return;
		}
		GorillaScoreboardTotalUpdater.SetInstance(this);
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x000C8914 File Offset: 0x000C6B14
	private void Start()
	{
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.JoinedRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerEnteredRoom));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000C89A1 File Offset: 0x000C6BA1
	public static void CreateManager()
	{
		GorillaScoreboardTotalUpdater.SetInstance(new GameObject("GorillaScoreboardTotalUpdater").AddComponent<GorillaScoreboardTotalUpdater>());
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000C89B7 File Offset: 0x000C6BB7
	private static void SetInstance(GorillaScoreboardTotalUpdater manager)
	{
		GorillaScoreboardTotalUpdater.instance = manager;
		GorillaScoreboardTotalUpdater.hasInstance = true;
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(manager);
		}
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x000C89D2 File Offset: 0x000C6BD2
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

	// Token: 0x060028D0 RID: 10448 RVA: 0x000C89F8 File Offset: 0x000C6BF8
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

	// Token: 0x060028D1 RID: 10449 RVA: 0x000C8A1F File Offset: 0x000C6C1F
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

	// Token: 0x060028D2 RID: 10450 RVA: 0x000C8A50 File Offset: 0x000C6C50
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

	// Token: 0x060028D3 RID: 10451 RVA: 0x000C8A78 File Offset: 0x000C6C78
	public void UpdateActiveScoreboards()
	{
		for (int i = 0; i < GorillaScoreboardTotalUpdater.allScoreboards.Count; i++)
		{
			this.UpdateScoreboard(GorillaScoreboardTotalUpdater.allScoreboards[i]);
		}
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x000C8AAB File Offset: 0x000C6CAB
	public void SetOfflineFailureText(string failureText)
	{
		this.offlineTextErrorString = failureText;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000C8ABA File Offset: 0x000C6CBA
	public void ClearOfflineFailureText()
	{
		this.offlineTextErrorString = null;
		this.UpdateActiveScoreboards();
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x000C8ACC File Offset: 0x000C6CCC
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

	// Token: 0x060028D7 RID: 10455 RVA: 0x00015C1D File Offset: 0x00013E1D
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x00015C26 File Offset: 0x00013E26
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x000C8BF4 File Offset: 0x000C6DF4
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

	// Token: 0x060028DA RID: 10458 RVA: 0x000C8C8D File Offset: 0x000C6E8D
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

	// Token: 0x060028DB RID: 10459 RVA: 0x000C8CBC File Offset: 0x000C6EBC
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

	// Token: 0x060028DC RID: 10460 RVA: 0x000C8D1C File Offset: 0x000C6F1C
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

	// Token: 0x060028DD RID: 10461 RVA: 0x000C8DCC File Offset: 0x000C6FCC
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

	// Token: 0x060028DE RID: 10462 RVA: 0x000C8E90 File Offset: 0x000C7090
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

	// Token: 0x060028E1 RID: 10465 RVA: 0x0000FD18 File Offset: 0x0000DF18
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04002DD1 RID: 11729
	public static GorillaScoreboardTotalUpdater instance;

	// Token: 0x04002DD2 RID: 11730
	[OnEnterPlay_Set(false)]
	public static bool hasInstance = false;

	// Token: 0x04002DD3 RID: 11731
	public static List<GorillaPlayerScoreboardLine> allScoreboardLines = new List<GorillaPlayerScoreboardLine>();

	// Token: 0x04002DD4 RID: 11732
	public static int lineIndex = 0;

	// Token: 0x04002DD5 RID: 11733
	private static int linesPerFrame = 2;

	// Token: 0x04002DD6 RID: 11734
	public static List<GorillaScoreBoard> allScoreboards = new List<GorillaScoreBoard>();

	// Token: 0x04002DD7 RID: 11735
	public static int boardIndex = 0;

	// Token: 0x04002DD8 RID: 11736
	private List<NetPlayer> playersInRoom = new List<NetPlayer>();

	// Token: 0x04002DD9 RID: 11737
	private bool joinedRoom;

	// Token: 0x04002DDA RID: 11738
	private bool wasGameManagerNull;

	// Token: 0x04002DDB RID: 11739
	public bool forOverlay;

	// Token: 0x04002DDC RID: 11740
	public string offlineTextErrorString;

	// Token: 0x04002DDD RID: 11741
	public Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports> reportDict = new Dictionary<int, GorillaScoreboardTotalUpdater.PlayerReports>();

	// Token: 0x04002DDE RID: 11742
	private static readonly Dictionary<int, ReportMuteTimer> m_reportMuteTimerDict = new Dictionary<int, ReportMuteTimer>(10);

	// Token: 0x04002DDF RID: 11743
	private static readonly ObjectPool<ReportMuteTimer> m_reportMuteTimerPool = new ObjectPool<ReportMuteTimer>(10);

	// Token: 0x02000670 RID: 1648
	public struct PlayerReports
	{
		// Token: 0x060028E2 RID: 10466 RVA: 0x000C8F7C File Offset: 0x000C717C
		public PlayerReports(GorillaScoreboardTotalUpdater.PlayerReports reportToUpdate, GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = (reportToUpdate.cheating || lineToUpdate.reportedCheating);
			this.toxicity = (reportToUpdate.toxicity || lineToUpdate.reportedToxicity);
			this.hateSpeech = (reportToUpdate.hateSpeech || lineToUpdate.reportedHateSpeech);
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000C8FDA File Offset: 0x000C71DA
		public PlayerReports(GorillaPlayerScoreboardLine lineToUpdate)
		{
			this.cheating = lineToUpdate.reportedCheating;
			this.toxicity = lineToUpdate.reportedToxicity;
			this.hateSpeech = lineToUpdate.reportedHateSpeech;
			this.pressedReport = lineToUpdate.reportInProgress;
		}

		// Token: 0x04002DE0 RID: 11744
		public bool cheating;

		// Token: 0x04002DE1 RID: 11745
		public bool toxicity;

		// Token: 0x04002DE2 RID: 11746
		public bool hateSpeech;

		// Token: 0x04002DE3 RID: 11747
		public bool pressedReport;
	}
}
