using System;
using System.Collections.Generic;
using System.Text;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class RacingManager : NetworkSceneObject, ITickSystemTick
{
	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00038068 File Offset: 0x00036268
	// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0003806F File Offset: 0x0003626F
	public static RacingManager instance { get; private set; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00038077 File Offset: 0x00036277
	// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0003807F File Offset: 0x0003627F
	public bool TickRunning { get; set; }

	// Token: 0x06000B62 RID: 2914 RVA: 0x0009A7E4 File Offset: 0x000989E4
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

	// Token: 0x06000B63 RID: 2915 RVA: 0x00038088 File Offset: 0x00036288
	protected override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		TickSystem<object>.AddTickCallback(this);
		base.OnEnable();
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0003809C File Offset: 0x0003629C
	protected override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		TickSystem<object>.RemoveTickCallback(this);
		base.OnDisable();
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0009A884 File Offset: 0x00098A84
	private void OnRoomJoin()
	{
		for (int i = 0; i < this.races.Length; i++)
		{
			this.races[i].Clear();
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0009A8B4 File Offset: 0x00098AB4
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

	// Token: 0x06000B67 RID: 2919 RVA: 0x0009A8F0 File Offset: 0x00098AF0
	public void RegisterVisual(RaceVisual visual)
	{
		int raceId = visual.raceId;
		if (raceId >= 0 && raceId < this.races.Length)
		{
			this.races[raceId].RegisterVisual(visual);
		}
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x000380B0 File Offset: 0x000362B0
	public void Button_StartRace(int raceId, int laps)
	{
		if (raceId >= 0 && raceId < this.races.Length)
		{
			this.races[raceId].Button_StartRace(laps);
		}
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x0009A924 File Offset: 0x00098B24
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

	// Token: 0x06000B6A RID: 2922 RVA: 0x0009A97C File Offset: 0x00098B7C
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

	// Token: 0x06000B6B RID: 2923 RVA: 0x0009A9F4 File Offset: 0x00098BF4
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

	// Token: 0x06000B6C RID: 2924 RVA: 0x000380CF File Offset: 0x000362CF
	public void OnCheckpointPassed(int raceId, int checkpointIndex)
	{
		this.photonView.RPC("PassCheckpoint_RPC", RpcTarget.All, new object[]
		{
			(byte)raceId,
			(byte)checkpointIndex
		});
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x000380FC File Offset: 0x000362FC
	[PunRPC]
	private void PassCheckpoint_RPC(byte raceId, byte checkpointIndex, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "PassCheckpoint_RPC");
		if (raceId >= 0 && (int)raceId < this.races.Length)
		{
			this.races[(int)raceId].PassCheckpoint(info.Sender, (int)checkpointIndex, info.SentServerTime);
		}
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00038133 File Offset: 0x00036333
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

	// Token: 0x06000B6F RID: 2927 RVA: 0x0009AA5C File Offset: 0x00098C5C
	void ITickSystemTick.Tick()
	{
		for (int i = 0; i < this.races.Length; i++)
		{
			this.races[i].Tick();
		}
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0009AA8C File Offset: 0x00098C8C
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

	// Token: 0x04000DBE RID: 3518
	[SerializeField]
	private RacingManager.RaceSetup[] raceSetups;

	// Token: 0x04000DBF RID: 3519
	private const int MinPlayersInRace = 1;

	// Token: 0x04000DC0 RID: 3520
	private const float ResultsDuration = 10f;

	// Token: 0x04000DC1 RID: 3521
	private RacingManager.Race[] races;

	// Token: 0x020001E6 RID: 486
	[Serializable]
	private struct RaceSetup
	{
		// Token: 0x04000DC2 RID: 3522
		public BoxCollider startVolume;

		// Token: 0x04000DC3 RID: 3523
		public int numCheckpoints;

		// Token: 0x04000DC4 RID: 3524
		public float dqBaseDuration;

		// Token: 0x04000DC5 RID: 3525
		public float dqInterval;
	}

	// Token: 0x020001E7 RID: 487
	private struct RacerData
	{
		// Token: 0x04000DC6 RID: 3526
		public int actorNumber;

		// Token: 0x04000DC7 RID: 3527
		public string playerName;

		// Token: 0x04000DC8 RID: 3528
		public int numCheckpointsPassed;

		// Token: 0x04000DC9 RID: 3529
		public double latestCheckpointTime;

		// Token: 0x04000DCA RID: 3530
		public bool isDisqualified;
	}

	// Token: 0x020001E8 RID: 488
	private class RacerComparer : IComparer<RacingManager.RacerData>
	{
		// Token: 0x06000B72 RID: 2930 RVA: 0x0009AAC0 File Offset: 0x00098CC0
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

		// Token: 0x04000DCB RID: 3531
		public static RacingManager.RacerComparer instance = new RacingManager.RacerComparer();
	}

	// Token: 0x020001E9 RID: 489
	public enum RacingState
	{
		// Token: 0x04000DCD RID: 3533
		Inactive,
		// Token: 0x04000DCE RID: 3534
		Countdown,
		// Token: 0x04000DCF RID: 3535
		InProgress,
		// Token: 0x04000DD0 RID: 3536
		Results
	}

	// Token: 0x020001EA RID: 490
	private class Race
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x0009AB2C File Offset: 0x00098D2C
		public Race(int raceIndex, RacingManager.RaceSetup setup, HashSet<int> actorsInAnyRace, PhotonView photonView)
		{
			this.raceIndex = raceIndex;
			this.numCheckpoints = setup.numCheckpoints;
			this.raceStartZone = setup.startVolume;
			this.dqBaseDuration = setup.dqBaseDuration;
			this.dqInterval = setup.dqInterval;
			this.photonView = photonView;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0003817E File Offset: 0x0003637E
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x00038186 File Offset: 0x00036386
		public RacingManager.RacingState racingState { get; private set; }

		// Token: 0x06000B78 RID: 2936 RVA: 0x0003818F File Offset: 0x0003638F
		public void RegisterVisual(RaceVisual visual)
		{
			this.raceVisual = visual;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00038198 File Offset: 0x00036398
		public void Clear()
		{
			this.hasLockedInParticipants = false;
			this.racers.Clear();
			this.playerLookup.Clear();
			this.racingState = RacingManager.RacingState.Inactive;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0009ABC0 File Offset: 0x00098DC0
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

		// Token: 0x06000B7B RID: 2939 RVA: 0x0009AC10 File Offset: 0x00098E10
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

		// Token: 0x06000B7C RID: 2940 RVA: 0x000381BE File Offset: 0x000363BE
		public void Tick()
		{
			if (Time.time >= this.nextTickTimestamp)
			{
				this.nextTickTimestamp = Time.time + this.TickWithNextDelay();
			}
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0009AC80 File Offset: 0x00098E80
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

		// Token: 0x06000B7E RID: 2942 RVA: 0x0009ADF4 File Offset: 0x00098FF4
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

		// Token: 0x06000B7F RID: 2943 RVA: 0x0009AEF4 File Offset: 0x000990F4
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

		// Token: 0x06000B80 RID: 2944 RVA: 0x000381DF File Offset: 0x000363DF
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

		// Token: 0x06000B81 RID: 2945 RVA: 0x0009AF98 File Offset: 0x00099198
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

		// Token: 0x06000B82 RID: 2946 RVA: 0x0009B00C File Offset: 0x0009920C
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

		// Token: 0x06000B83 RID: 2947 RVA: 0x0009B080 File Offset: 0x00099280
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

		// Token: 0x06000B84 RID: 2948 RVA: 0x0009B11C File Offset: 0x0009931C
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

		// Token: 0x06000B85 RID: 2949 RVA: 0x0009B230 File Offset: 0x00099430
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

		// Token: 0x06000B86 RID: 2950 RVA: 0x0009B474 File Offset: 0x00099674
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

		// Token: 0x06000B87 RID: 2951 RVA: 0x0009B680 File Offset: 0x00099880
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

		// Token: 0x04000DD1 RID: 3537
		private int raceIndex;

		// Token: 0x04000DD2 RID: 3538
		private int numCheckpoints;

		// Token: 0x04000DD3 RID: 3539
		private float dqBaseDuration;

		// Token: 0x04000DD4 RID: 3540
		private float dqInterval;

		// Token: 0x04000DD5 RID: 3541
		private BoxCollider raceStartZone;

		// Token: 0x04000DD6 RID: 3542
		private PhotonView photonView;

		// Token: 0x04000DD7 RID: 3543
		private List<RacingManager.RacerData> racers = new List<RacingManager.RacerData>(10);

		// Token: 0x04000DD8 RID: 3544
		private Dictionary<NetPlayer, int> playerLookup = new Dictionary<NetPlayer, int>();

		// Token: 0x04000DD9 RID: 3545
		private List<int> actorsInStartZone = new List<int>();

		// Token: 0x04000DDA RID: 3546
		private List<int> actorsInStartZone2 = new List<int>();

		// Token: 0x04000DDB RID: 3547
		private Dictionary<int, string> playerNamesInStartZone = new Dictionary<int, string>();

		// Token: 0x04000DDC RID: 3548
		private int numLapsSelected = 1;

		// Token: 0x04000DDE RID: 3550
		private double raceStartTime;

		// Token: 0x04000DDF RID: 3551
		private double abortRaceAtTimestamp;

		// Token: 0x04000DE0 RID: 3552
		private float resultsEndTimestamp;

		// Token: 0x04000DE1 RID: 3553
		private bool isInstanceLoaded;

		// Token: 0x04000DE2 RID: 3554
		private int numCheckpointsToWin;

		// Token: 0x04000DE3 RID: 3555
		private RaceVisual raceVisual;

		// Token: 0x04000DE4 RID: 3556
		private bool hasLockedInParticipants;

		// Token: 0x04000DE5 RID: 3557
		private float nextTickTimestamp;

		// Token: 0x04000DE6 RID: 3558
		private static StringBuilder stringBuilder = new StringBuilder();

		// Token: 0x04000DE7 RID: 3559
		private static StringBuilder timesStringBuilder = new StringBuilder();

		// Token: 0x04000DE8 RID: 3560
		private static Collider[] overlapColliders = new Collider[20];

		// Token: 0x04000DE9 RID: 3561
		private static int playerLayerMask = UnityLayer.GorillaBodyCollider.ToLayerMask() | UnityLayer.GorillaTagCollider.ToLayerMask();

		// Token: 0x04000DEA RID: 3562
		private float nextStartZoneUpdateTimestamp;
	}
}
