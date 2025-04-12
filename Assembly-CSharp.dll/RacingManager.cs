using System;
using System.Collections.Generic;
using System.Text;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class RacingManager : NetworkSceneObject, ITickSystemTick
{
	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00036DA8 File Offset: 0x00034FA8
	// (set) Token: 0x06000B15 RID: 2837 RVA: 0x00036DAF File Offset: 0x00034FAF
	public static RacingManager instance { get; private set; }

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00036DB7 File Offset: 0x00034FB7
	// (set) Token: 0x06000B17 RID: 2839 RVA: 0x00036DBF File Offset: 0x00034FBF
	public bool TickRunning { get; set; }

	// Token: 0x06000B18 RID: 2840 RVA: 0x00097EF0 File Offset: 0x000960F0
	private void Awake()
	{
		RacingManager.instance = this;
		HashSet<int> actorsInAnyRace = new HashSet<int>();
		this.races = new RacingManager.Race[this.raceSetups.Length];
		for (int i = 0; i < this.raceSetups.Length; i++)
		{
			this.races[i] = new RacingManager.Race(i, this.raceSetups[i], actorsInAnyRace, this.photonView);
		}
		RoomSystem.JoinedRoomEvent = (Action)Delegate.Combine(RoomSystem.JoinedRoomEvent, new Action(this.OnRoomJoin));
		RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerJoined));
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00036DC8 File Offset: 0x00034FC8
	protected override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		TickSystem<object>.AddTickCallback(this);
		base.OnEnable();
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00036DDC File Offset: 0x00034FDC
	protected override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		TickSystem<object>.RemoveTickCallback(this);
		base.OnDisable();
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00097F90 File Offset: 0x00096190
	private void OnRoomJoin()
	{
		for (int i = 0; i < this.races.Length; i++)
		{
			this.races[i].Clear();
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00097FC0 File Offset: 0x000961C0
	private void OnPlayerJoined(NetPlayer player)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		for (int i = 0; i < this.races.Length; i++)
		{
			this.races[i].SendStateToNewPlayer(player);
		}
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00097FFC File Offset: 0x000961FC
	public void RegisterVisual(RaceVisual visual)
	{
		int raceId = visual.raceId;
		if (raceId >= 0 && raceId < this.races.Length)
		{
			this.races[raceId].RegisterVisual(visual);
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00036DF0 File Offset: 0x00034FF0
	public void Button_StartRace(int raceId, int laps)
	{
		if (raceId >= 0 && raceId < this.races.Length)
		{
			this.races[raceId].Button_StartRace(laps);
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x00098030 File Offset: 0x00096230
	[PunRPC]
	private void RequestRaceStart_RPC(int raceId, int laps, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestRaceStart_RPC");
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (laps != 1 && laps != 3 && laps != 5)
		{
			return;
		}
		if (raceId >= 0 && raceId < this.races.Length)
		{
			this.races[raceId].Host_RequestRaceStart(laps, info.Sender.ActorNumber);
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x00098088 File Offset: 0x00096288
	[PunRPC]
	private void RaceBeginCountdown_RPC(byte raceId, byte laps, double startTime, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RaceBeginCountdown_RPC");
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		if (laps != 1 && laps != 3 && laps != 5)
		{
			return;
		}
		if (!double.IsFinite(startTime))
		{
			return;
		}
		if (startTime < PhotonNetwork.Time || startTime > PhotonNetwork.Time + 4.0)
		{
			return;
		}
		if (raceId >= 0 && (int)raceId < this.races.Length)
		{
			this.races[(int)raceId].BeginCountdown(startTime, (int)laps);
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x00098100 File Offset: 0x00096300
	[PunRPC]
	private void RaceLockInParticipants_RPC(byte raceId, int[] participantActorNumbers, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RaceLockInParticipants_RPC");
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		if (participantActorNumbers.Length > 10)
		{
			return;
		}
		for (int i = 1; i < participantActorNumbers.Length; i++)
		{
			if (participantActorNumbers[i] <= participantActorNumbers[i - 1])
			{
				return;
			}
		}
		if (raceId >= 0 && (int)raceId < this.races.Length)
		{
			this.races[(int)raceId].LockInParticipants(participantActorNumbers, false);
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x00036E0F File Offset: 0x0003500F
	public void OnCheckpointPassed(int raceId, int checkpointIndex)
	{
		this.photonView.RPC("PassCheckpoint_RPC", RpcTarget.All, new object[]
		{
			(byte)raceId,
			(byte)checkpointIndex
		});
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x00036E3C File Offset: 0x0003503C
	[PunRPC]
	private void PassCheckpoint_RPC(byte raceId, byte checkpointIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "PassCheckpoint_RPC");
		if (raceId >= 0 && (int)raceId < this.races.Length)
		{
			this.races[(int)raceId].PassCheckpoint(info.Sender, (int)checkpointIndex, info.SentServerTime);
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x00036E73 File Offset: 0x00035073
	[PunRPC]
	private void RaceEnded_RPC(byte raceId, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RaceEnded_RPC");
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		if (raceId >= 0 && (int)raceId < this.races.Length)
		{
			this.races[(int)raceId].RaceEnded();
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00098168 File Offset: 0x00096368
	void ITickSystemTick.Tick()
	{
		for (int i = 0; i < this.races.Length; i++)
		{
			this.races[i].Tick();
		}
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00098198 File Offset: 0x00096398
	public bool IsActorLockedIntoAnyRace(int actorNumber)
	{
		for (int i = 0; i < this.races.Length; i++)
		{
			if (this.races[i].IsActorLockedIntoRace(actorNumber))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000D79 RID: 3449
	[SerializeField]
	private RacingManager.RaceSetup[] raceSetups;

	// Token: 0x04000D7A RID: 3450
	private const int MinPlayersInRace = 1;

	// Token: 0x04000D7B RID: 3451
	private const float ResultsDuration = 10f;

	// Token: 0x04000D7C RID: 3452
	private RacingManager.Race[] races;

	// Token: 0x020001DB RID: 475
	[Serializable]
	private struct RaceSetup
	{
		// Token: 0x04000D7D RID: 3453
		public BoxCollider startVolume;

		// Token: 0x04000D7E RID: 3454
		public int numCheckpoints;

		// Token: 0x04000D7F RID: 3455
		public float dqBaseDuration;

		// Token: 0x04000D80 RID: 3456
		public float dqInterval;
	}

	// Token: 0x020001DC RID: 476
	private struct RacerData
	{
		// Token: 0x04000D81 RID: 3457
		public int actorNumber;

		// Token: 0x04000D82 RID: 3458
		public string playerName;

		// Token: 0x04000D83 RID: 3459
		public int numCheckpointsPassed;

		// Token: 0x04000D84 RID: 3460
		public double latestCheckpointTime;

		// Token: 0x04000D85 RID: 3461
		public bool isDisqualified;
	}

	// Token: 0x020001DD RID: 477
	private class RacerComparer : IComparer<RacingManager.RacerData>
	{
		// Token: 0x06000B28 RID: 2856 RVA: 0x000981CC File Offset: 0x000963CC
		public int Compare(RacingManager.RacerData a, RacingManager.RacerData b)
		{
			int num = a.isDisqualified.CompareTo(b.isDisqualified);
			if (num != 0)
			{
				return num;
			}
			int num2 = a.numCheckpointsPassed.CompareTo(b.numCheckpointsPassed);
			if (num2 != 0)
			{
				return -num2;
			}
			if (a.numCheckpointsPassed > 0)
			{
				return a.latestCheckpointTime.CompareTo(b.latestCheckpointTime);
			}
			return a.actorNumber.CompareTo(b.actorNumber);
		}

		// Token: 0x04000D86 RID: 3462
		public static RacingManager.RacerComparer instance = new RacingManager.RacerComparer();
	}

	// Token: 0x020001DE RID: 478
	public enum RacingState
	{
		// Token: 0x04000D88 RID: 3464
		Inactive,
		// Token: 0x04000D89 RID: 3465
		Countdown,
		// Token: 0x04000D8A RID: 3466
		InProgress,
		// Token: 0x04000D8B RID: 3467
		Results
	}

	// Token: 0x020001DF RID: 479
	private class Race
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x00098238 File Offset: 0x00096438
		public Race(int raceIndex, RacingManager.RaceSetup setup, HashSet<int> actorsInAnyRace, PhotonView photonView)
		{
			this.raceIndex = raceIndex;
			this.numCheckpoints = setup.numCheckpoints;
			this.raceStartZone = setup.startVolume;
			this.dqBaseDuration = setup.dqBaseDuration;
			this.dqInterval = setup.dqInterval;
			this.photonView = photonView;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00036EBE File Offset: 0x000350BE
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x00036EC6 File Offset: 0x000350C6
		public RacingManager.RacingState racingState { get; private set; }

		// Token: 0x06000B2E RID: 2862 RVA: 0x00036ECF File Offset: 0x000350CF
		public void RegisterVisual(RaceVisual visual)
		{
			this.raceVisual = visual;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00036ED8 File Offset: 0x000350D8
		public void Clear()
		{
			this.hasLockedInParticipants = false;
			this.racers.Clear();
			this.playerLookup.Clear();
			this.racingState = RacingManager.RacingState.Inactive;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000982CC File Offset: 0x000964CC
		public bool IsActorLockedIntoRace(int actorNumber)
		{
			if (this.racingState != RacingManager.RacingState.InProgress || !this.hasLockedInParticipants)
			{
				return false;
			}
			for (int i = 0; i < this.racers.Count; i++)
			{
				if (this.racers[i].actorNumber == actorNumber)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0009831C File Offset: 0x0009651C
		public void SendStateToNewPlayer(NetPlayer newPlayer)
		{
			switch (this.racingState)
			{
			case RacingManager.RacingState.Inactive:
			case RacingManager.RacingState.Results:
				return;
			case RacingManager.RacingState.Countdown:
				this.photonView.RPC("RaceBeginCountdown_RPC", RpcTarget.All, new object[]
				{
					(byte)this.raceIndex,
					(byte)this.numLapsSelected,
					this.raceStartTime
				});
				return;
			case RacingManager.RacingState.InProgress:
				return;
			default:
				return;
			}
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00036EFE File Offset: 0x000350FE
		public void Tick()
		{
			if (Time.time >= this.nextTickTimestamp)
			{
				this.nextTickTimestamp = Time.time + this.TickWithNextDelay();
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0009838C File Offset: 0x0009658C
		public float TickWithNextDelay()
		{
			bool flag = this.raceVisual != null;
			if (flag)
			{
				this.raceVisual.ActivateStartingWall(this.racingState == RacingManager.RacingState.Countdown);
			}
			switch (this.racingState)
			{
			case RacingManager.RacingState.Inactive:
				if (flag)
				{
					this.RefreshStartingPlayerList();
				}
				return 1f;
			case RacingManager.RacingState.Countdown:
				if (this.raceStartTime > PhotonNetwork.Time)
				{
					if (flag)
					{
						this.RefreshStartingPlayerList();
						this.raceVisual.UpdateCountdown(Mathf.CeilToInt((float)(this.raceStartTime - PhotonNetwork.Time)));
					}
				}
				else
				{
					this.RaceCountdownEnds();
				}
				return 0.1f;
			case RacingManager.RacingState.InProgress:
				if (PhotonNetwork.IsMasterClient)
				{
					if (PhotonNetwork.Time > this.abortRaceAtTimestamp)
					{
						this.photonView.RPC("RaceEnded_RPC", RpcTarget.All, new object[]
						{
							(byte)this.raceIndex
						});
					}
					else
					{
						int num = 0;
						for (int i = 0; i < this.racers.Count; i++)
						{
							if (this.racers[i].numCheckpointsPassed < this.numCheckpointsToWin)
							{
								num++;
							}
						}
						if (num == 0)
						{
							this.photonView.RPC("RaceEnded_RPC", RpcTarget.All, new object[]
							{
								(byte)this.raceIndex
							});
						}
					}
				}
				return 1f;
			case RacingManager.RacingState.Results:
				if (Time.time >= this.resultsEndTimestamp)
				{
					if (flag)
					{
						this.raceVisual.OnRaceReset();
					}
					this.racingState = RacingManager.RacingState.Inactive;
				}
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00098500 File Offset: 0x00096700
		public void RaceEnded()
		{
			if (this.racingState != RacingManager.RacingState.InProgress)
			{
				return;
			}
			this.racingState = RacingManager.RacingState.Results;
			this.resultsEndTimestamp = Time.time + 10f;
			if (this.raceVisual != null)
			{
				this.raceVisual.OnRaceEnded();
			}
			for (int i = 0; i < this.racers.Count; i++)
			{
				RacingManager.RacerData racerData = this.racers[i];
				if (racerData.numCheckpointsPassed < this.numCheckpointsToWin)
				{
					racerData.isDisqualified = true;
					this.racers[i] = racerData;
				}
			}
			this.racers.Sort(RacingManager.RacerComparer.instance);
			this.OnRacerOrderChanged();
			for (int j = 0; j < this.racers.Count; j++)
			{
				if (this.racers[j].actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
				{
					VRRig.LocalRig.hoverboardVisual.SetRaceDisplay("");
					VRRig.LocalRig.hoverboardVisual.SetRaceLapsDisplay("");
					return;
				}
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00098600 File Offset: 0x00096800
		private void RefreshStartingPlayerList()
		{
			if (this.raceVisual != null && this.UpdateActorsInStartZone())
			{
				RacingManager.Race.stringBuilder.Clear();
				RacingManager.Race.stringBuilder.AppendLine("NEXT RACE LINEUP");
				for (int i = 0; i < this.actorsInStartZone.Count; i++)
				{
					RacingManager.Race.stringBuilder.Append("    ");
					RacingManager.Race.stringBuilder.AppendLine(this.playerNamesInStartZone[this.actorsInStartZone[i]]);
				}
				this.raceVisual.SetRaceStartScoreboardText(RacingManager.Race.stringBuilder.ToString(), "");
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00036F1F File Offset: 0x0003511F
		public void Button_StartRace(int laps)
		{
			if (this.racingState != RacingManager.RacingState.Inactive)
			{
				return;
			}
			this.photonView.RPC("RequestRaceStart_RPC", RpcTarget.MasterClient, new object[]
			{
				this.raceIndex,
				laps
			});
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x000986A4 File Offset: 0x000968A4
		public void Host_RequestRaceStart(int laps, int requestedByActorNumber)
		{
			if (this.racingState != RacingManager.RacingState.Inactive)
			{
				return;
			}
			this.UpdateActorsInStartZone();
			if (this.actorsInStartZone.Contains(requestedByActorNumber))
			{
				this.photonView.RPC("RaceBeginCountdown_RPC", RpcTarget.All, new object[]
				{
					(byte)this.raceIndex,
					(byte)laps,
					PhotonNetwork.Time + 4.0
				});
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00098718 File Offset: 0x00096918
		public void BeginCountdown(double startTime, int laps)
		{
			if (this.racingState != RacingManager.RacingState.Inactive)
			{
				return;
			}
			this.racingState = RacingManager.RacingState.Countdown;
			this.raceStartTime = startTime;
			this.abortRaceAtTimestamp = startTime + (double)this.dqBaseDuration;
			this.numLapsSelected = laps;
			this.numCheckpointsToWin = this.numCheckpoints * laps + 1;
			this.hasLockedInParticipants = false;
			if (this.raceVisual != null)
			{
				this.raceVisual.OnCountdownStart(laps, (float)(startTime - PhotonNetwork.Time));
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0009878C File Offset: 0x0009698C
		public void RaceCountdownEnds()
		{
			if (this.racingState != RacingManager.RacingState.Countdown)
			{
				return;
			}
			this.racingState = RacingManager.RacingState.InProgress;
			if (this.raceVisual != null)
			{
				this.raceVisual.OnRaceStart();
			}
			this.UpdateActorsInStartZone();
			if (PhotonNetwork.IsMasterClient)
			{
				this.photonView.RPC("RaceLockInParticipants_RPC", RpcTarget.All, new object[]
				{
					(byte)this.raceIndex,
					this.actorsInStartZone.ToArray()
				});
				return;
			}
			if (this.actorsInStartZone.Count >= 1)
			{
				this.LockInParticipants(this.actorsInStartZone.ToArray(), true);
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00098828 File Offset: 0x00096A28
		public void LockInParticipants(int[] participantActorNumbers, bool isProvisional = false)
		{
			if (this.hasLockedInParticipants)
			{
				return;
			}
			if (!isProvisional && participantActorNumbers.Length < 1)
			{
				this.racingState = RacingManager.RacingState.Inactive;
				return;
			}
			this.racers.Clear();
			if (participantActorNumbers.Length != 0)
			{
				foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
				{
					int actorNumber = vrrig.OwningNetPlayer.ActorNumber;
					if (participantActorNumbers.BinarySearch(actorNumber) >= 0 && !RacingManager.instance.IsActorLockedIntoAnyRace(actorNumber))
					{
						this.racers.Add(new RacingManager.RacerData
						{
							actorNumber = actorNumber,
							playerName = vrrig.OwningNetPlayer.SanitizedNickName,
							latestCheckpointTime = this.raceStartTime
						});
					}
				}
			}
			if (!isProvisional)
			{
				if (this.racers.Count < 1)
				{
					this.racingState = RacingManager.RacingState.Inactive;
					return;
				}
				this.hasLockedInParticipants = true;
			}
			this.racers.Sort(RacingManager.RacerComparer.instance);
			this.OnRacerOrderChanged();
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0009893C File Offset: 0x00096B3C
		public void PassCheckpoint(Player player, int checkpointIndex, double time)
		{
			if (this.racingState == RacingManager.RacingState.Inactive)
			{
				return;
			}
			if (time < this.raceStartTime || time < PhotonNetwork.Time - 5.0 || time > PhotonNetwork.Time + 0.10000000149011612)
			{
				return;
			}
			if (this.abortRaceAtTimestamp < time + (double)this.dqInterval)
			{
				this.abortRaceAtTimestamp = time + (double)this.dqInterval;
			}
			RacingManager.RacerData racerData = default(RacingManager.RacerData);
			int i = 0;
			while (i < this.racers.Count)
			{
				racerData = this.racers[i];
				if (racerData.actorNumber == player.ActorNumber)
				{
					if (racerData.numCheckpointsPassed >= this.numCheckpointsToWin || racerData.isDisqualified)
					{
						return;
					}
					if (checkpointIndex != racerData.numCheckpointsPassed % this.numCheckpoints)
					{
						return;
					}
					RigContainer rigContainer;
					if (this.raceVisual != null && VRRigCache.Instance.TryGetVrrig(player, out rigContainer) && !this.raceVisual.IsPlayerNearCheckpoint(rigContainer.Rig, checkpointIndex))
					{
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (racerData.actorNumber != player.ActorNumber)
			{
				return;
			}
			racerData.numCheckpointsPassed++;
			racerData.latestCheckpointTime = time;
			this.racers[i] = racerData;
			if (racerData.numCheckpointsPassed >= this.numCheckpointsToWin || (i > 0 && RacingManager.RacerComparer.instance.Compare(this.racers[i - 1], racerData) > 0))
			{
				this.racers.Sort(RacingManager.RacerComparer.instance);
				this.OnRacerOrderChanged();
			}
			if (player.IsLocal)
			{
				if (checkpointIndex == this.numCheckpoints - 1)
				{
					int num = racerData.numCheckpointsPassed / this.numCheckpoints + 1;
					if (num > this.numLapsSelected)
					{
						this.raceVisual.ShowFinishLineText("FINISH");
						this.raceVisual.EnableRaceEndSound();
						return;
					}
					if (num == this.numLapsSelected)
					{
						this.raceVisual.ShowFinishLineText("FINAL LAP");
						return;
					}
					this.raceVisual.ShowFinishLineText("NEXT LAP");
					return;
				}
				else if (checkpointIndex == 0)
				{
					int num2 = racerData.numCheckpointsPassed / this.numCheckpoints + 1;
					if (num2 > this.numLapsSelected)
					{
						VRRig.LocalRig.hoverboardVisual.SetRaceLapsDisplay("");
						return;
					}
					VRRig.LocalRig.hoverboardVisual.SetRaceLapsDisplay(string.Format("LAP {0}/{1}", num2, this.numLapsSelected));
				}
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00098B80 File Offset: 0x00096D80
		private void OnRacerOrderChanged()
		{
			if (this.raceVisual != null)
			{
				RacingManager.Race.stringBuilder.Clear();
				RacingManager.Race.timesStringBuilder.Clear();
				RacingManager.Race.timesStringBuilder.AppendLine("");
				bool flag = false;
				switch (this.racingState)
				{
				case RacingManager.RacingState.Inactive:
					return;
				case RacingManager.RacingState.Countdown:
					RacingManager.Race.stringBuilder.AppendLine("STARTING LINEUP");
					flag = true;
					break;
				case RacingManager.RacingState.InProgress:
					RacingManager.Race.stringBuilder.AppendLine("RACE LEADERBOARD");
					break;
				case RacingManager.RacingState.Results:
					RacingManager.Race.stringBuilder.AppendLine("RACE RESULTS");
					break;
				}
				for (int i = 0; i < this.racers.Count; i++)
				{
					RacingManager.RacerData racerData = this.racers[i];
					if (racerData.actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
					{
						VRRig.LocalRig.hoverboardVisual.SetRaceDisplay(racerData.isDisqualified ? "DQ" : (i + 1).ToString());
					}
					string text = racerData.isDisqualified ? "DQ. " : (flag ? "    " : ((i + 1).ToString() + ". "));
					RacingManager.Race.stringBuilder.Append(text);
					if (text.Length <= 3)
					{
						RacingManager.Race.stringBuilder.Append(" ");
					}
					RacingManager.Race.stringBuilder.AppendLine(racerData.playerName);
					if (racerData.isDisqualified)
					{
						RacingManager.Race.timesStringBuilder.AppendLine("--.--");
					}
					else if (racerData.numCheckpointsPassed < this.numCheckpointsToWin)
					{
						RacingManager.Race.timesStringBuilder.AppendLine("");
					}
					else
					{
						RacingManager.Race.timesStringBuilder.AppendLine(string.Format("{0:0.00}", racerData.latestCheckpointTime - this.raceStartTime));
					}
				}
				string mainText = RacingManager.Race.stringBuilder.ToString();
				string timesText = RacingManager.Race.timesStringBuilder.ToString();
				this.raceVisual.SetScoreboardText(mainText, timesText);
				this.raceVisual.SetRaceStartScoreboardText(mainText, timesText);
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00098D8C File Offset: 0x00096F8C
		private bool UpdateActorsInStartZone()
		{
			if (Time.time < this.nextStartZoneUpdateTimestamp)
			{
				return false;
			}
			this.nextStartZoneUpdateTimestamp = Time.time + 0.1f;
			List<int> list = this.actorsInStartZone2;
			List<int> list2 = this.actorsInStartZone;
			this.actorsInStartZone = list;
			this.actorsInStartZone2 = list2;
			this.actorsInStartZone.Clear();
			this.playerNamesInStartZone.Clear();
			int num = Physics.OverlapBoxNonAlloc(this.raceStartZone.transform.position, this.raceStartZone.size / 2f, RacingManager.Race.overlapColliders, this.raceStartZone.transform.rotation, RacingManager.Race.playerLayerMask);
			num = Mathf.Min(num, RacingManager.Race.overlapColliders.Length);
			for (int i = 0; i < num; i++)
			{
				Collider collider = RacingManager.Race.overlapColliders[i];
				if (!(collider == null))
				{
					VRRig component = collider.attachedRigidbody.gameObject.GetComponent<VRRig>();
					int count = this.actorsInStartZone.Count;
					if (!(component == null))
					{
						if (component.isLocal)
						{
							if (NetworkSystem.Instance.LocalPlayer == null)
							{
								RacingManager.Race.overlapColliders[i] = null;
								goto IL_1E2;
							}
							if (RacingManager.instance.IsActorLockedIntoAnyRace(NetworkSystem.Instance.LocalPlayer.ActorNumber))
							{
								goto IL_1E2;
							}
							this.actorsInStartZone.AddSortedUnique(NetworkSystem.Instance.LocalPlayer.ActorNumber);
							if (this.actorsInStartZone.Count > count)
							{
								this.playerNamesInStartZone.Add(NetworkSystem.Instance.LocalPlayer.ActorNumber, component.playerNameVisible);
							}
						}
						else
						{
							if (RacingManager.instance.IsActorLockedIntoAnyRace(component.OwningNetPlayer.ActorNumber))
							{
								goto IL_1E2;
							}
							this.actorsInStartZone.AddSortedUnique(component.OwningNetPlayer.ActorNumber);
							if (this.actorsInStartZone.Count > count)
							{
								this.playerNamesInStartZone.Add(component.OwningNetPlayer.ActorNumber, component.playerNameVisible);
							}
						}
						RacingManager.Race.overlapColliders[i] = null;
					}
				}
				IL_1E2:;
			}
			if (this.actorsInStartZone2.Count != this.actorsInStartZone.Count)
			{
				return true;
			}
			for (int j = 0; j < this.actorsInStartZone.Count; j++)
			{
				if (this.actorsInStartZone[j] != this.actorsInStartZone2[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000D8C RID: 3468
		private int raceIndex;

		// Token: 0x04000D8D RID: 3469
		private int numCheckpoints;

		// Token: 0x04000D8E RID: 3470
		private float dqBaseDuration;

		// Token: 0x04000D8F RID: 3471
		private float dqInterval;

		// Token: 0x04000D90 RID: 3472
		private BoxCollider raceStartZone;

		// Token: 0x04000D91 RID: 3473
		private PhotonView photonView;

		// Token: 0x04000D92 RID: 3474
		private List<RacingManager.RacerData> racers = new List<RacingManager.RacerData>(10);

		// Token: 0x04000D93 RID: 3475
		private Dictionary<NetPlayer, int> playerLookup = new Dictionary<NetPlayer, int>();

		// Token: 0x04000D94 RID: 3476
		private List<int> actorsInStartZone = new List<int>();

		// Token: 0x04000D95 RID: 3477
		private List<int> actorsInStartZone2 = new List<int>();

		// Token: 0x04000D96 RID: 3478
		private Dictionary<int, string> playerNamesInStartZone = new Dictionary<int, string>();

		// Token: 0x04000D97 RID: 3479
		private int numLapsSelected = 1;

		// Token: 0x04000D99 RID: 3481
		private double raceStartTime;

		// Token: 0x04000D9A RID: 3482
		private double abortRaceAtTimestamp;

		// Token: 0x04000D9B RID: 3483
		private float resultsEndTimestamp;

		// Token: 0x04000D9C RID: 3484
		private bool isInstanceLoaded;

		// Token: 0x04000D9D RID: 3485
		private int numCheckpointsToWin;

		// Token: 0x04000D9E RID: 3486
		private RaceVisual raceVisual;

		// Token: 0x04000D9F RID: 3487
		private bool hasLockedInParticipants;

		// Token: 0x04000DA0 RID: 3488
		private float nextTickTimestamp;

		// Token: 0x04000DA1 RID: 3489
		private static StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04000DA2 RID: 3490
		private static StringBuilder timesStringBuilder = new StringBuilder();

		// Token: 0x04000DA3 RID: 3491
		private static Collider[] overlapColliders = new Collider[20];

		// Token: 0x04000DA4 RID: 3492
		private static int playerLayerMask = UnityLayer.GorillaBodyCollider.ToLayerMask() | UnityLayer.GorillaTagCollider.ToLayerMask();

		// Token: 0x04000DA5 RID: 3493
		private float nextStartZoneUpdateTimestamp;
	}
}
