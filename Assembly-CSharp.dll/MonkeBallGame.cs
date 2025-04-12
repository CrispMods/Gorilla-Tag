using System;
using System.Collections.Generic;
using Fusion;
using GorillaExtensions;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200049E RID: 1182
[NetworkBehaviourWeaved(0)]
public class MonkeBallGame : NetworkComponent
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001C98 RID: 7320 RVA: 0x00042A9B File Offset: 0x00040C9B
	public Transform BallLauncher
	{
		get
		{
			return this._ballLauncher;
		}
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x000DBBFC File Offset: 0x000D9DFC
	protected override void Awake()
	{
		base.Awake();
		MonkeBallGame.Instance = this;
		this.gameState = MonkeBallGame.GameState.None;
		this._callLimiters = new CallLimiter[8];
		this._callLimiters[0] = new CallLimiter(20, 1f, 0.5f);
		this._callLimiters[1] = new CallLimiter(20, 10f, 0.5f);
		this._callLimiters[2] = new CallLimiter(20, 10f, 0.5f);
		this._callLimiters[3] = new CallLimiter(20, 1f, 0.5f);
		this._callLimiters[4] = new CallLimiter(20, 1f, 0.5f);
		this._callLimiters[5] = new CallLimiter(20, 1f, 0.5f);
		this._callLimiters[6] = new CallLimiter(20, 1f, 0.5f);
		this._callLimiters[7] = new CallLimiter(5, 10f, 0.5f);
		this.AssignNetworkListeners();
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x000DBCF8 File Offset: 0x000D9EF8
	private bool ValidateCallLimits(MonkeBallGame.RPC rpcCall, PhotonMessageInfo info)
	{
		if (rpcCall < MonkeBallGame.RPC.SetGameState || rpcCall >= MonkeBallGame.RPC.Count)
		{
			return false;
		}
		bool flag = this._callLimiters[(int)rpcCall].CheckCallTime(Time.time);
		if (!flag)
		{
			this.ReportRPCCall(rpcCall, info, "Too many RPC Calls!");
		}
		return flag;
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x00042AA3 File Offset: 0x00040CA3
	private void ReportRPCCall(MonkeBallGame.RPC rpcCall, PhotonMessageInfo info, string susReason)
	{
		GorillaNot.instance.SendReport(string.Format("Reason: {0}   RPC: {1}", susReason, rpcCall), info.Sender.UserId, info.Sender.NickName);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000DBD34 File Offset: 0x000D9F34
	protected override void Start()
	{
		base.Start();
		for (int i = 0; i < this.startingBalls.Count; i++)
		{
			GameBallManager.Instance.AddGameBall(this.startingBalls[i].gameBall);
		}
		for (int j = 0; j < this.scoreboards.Count; j++)
		{
			this.scoreboards[j].Setup(this);
		}
		this.gameEndTime = -1.0;
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x00042AD8 File Offset: 0x00040CD8
	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		base.Despawned(runner, hasState);
		this.UnassignNetworkListeners();
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x000DBDB4 File Offset: 0x000D9FB4
	public void OnPlayerDestroy()
	{
		if (this._setStoredLocalPlayerColor)
		{
			PlayerPrefs.SetFloat("redValue", this._storedLocalPlayerColor.r);
			PlayerPrefs.SetFloat("greenValue", this._storedLocalPlayerColor.g);
			PlayerPrefs.SetFloat("blueValue", this._storedLocalPlayerColor.b);
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x000DBE10 File Offset: 0x000DA010
	private void AssignNetworkListeners()
	{
		NetworkSystem.Instance.OnPlayerJoined += this.OnPlayerJoined;
		NetworkSystem.Instance.OnPlayerLeft += this.OnPlayerLeft;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent += this.OnMasterClientSwitched;
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000DBE60 File Offset: 0x000DA060
	private void UnassignNetworkListeners()
	{
		NetworkSystem.Instance.OnPlayerJoined -= this.OnPlayerJoined;
		NetworkSystem.Instance.OnPlayerLeft -= this.OnPlayerLeft;
		NetworkSystem.Instance.OnMasterClientSwitchedEvent -= this.OnMasterClientSwitched;
	}

	// Token: 0x06001CA1 RID: 7329 RVA: 0x000DBEB0 File Offset: 0x000DA0B0
	private void Update()
	{
		if (this.IsMasterClient() && this.gameState != MonkeBallGame.GameState.None && this.gameEndTime >= 0.0 && PhotonNetwork.Time > this.gameEndTime)
		{
			this.gameEndTime = -1.0;
			this.RequestGameState(MonkeBallGame.GameState.PostGame);
		}
		this.RefreshTime();
		if (this._forceSync)
		{
			this._forceSyncDelay -= Time.deltaTime;
			if (this._forceSyncDelay <= 0f)
			{
				this._forceSync = false;
				this.ForceSyncPlayersVisuals();
				this.RefreshTeamPlayers(false);
			}
		}
		if (this._forceOrigColorFix)
		{
			this._forceOrigColorDelay -= Time.deltaTime;
			if (this._forceOrigColorDelay <= 0f)
			{
				this._forceOrigColorFix = false;
				this.ForceOriginalColorSync();
			}
		}
	}

	// Token: 0x06001CA2 RID: 7330 RVA: 0x000DBF78 File Offset: 0x000DA178
	private void OnPlayerJoined(NetPlayer player)
	{
		this._forceSync = true;
		this._forceSyncDelay = 5f;
		if (!this.IsMasterClient())
		{
			return;
		}
		int[] array;
		int[] array2;
		int[] array3;
		long[] array4;
		long[] array5;
		this.GetCurrentGameState(out array, out array2, out array3, out array4, out array5);
		this.photonView.RPC("RequestSetGameStateRPC", player.GetPlayerRef(), new object[]
		{
			(int)this.gameState,
			this.gameEndTime,
			array,
			array2,
			array3,
			array4,
			array5
		});
	}

	// Token: 0x06001CA3 RID: 7331 RVA: 0x000DC000 File Offset: 0x000DA200
	private void OnPlayerLeft(NetPlayer player)
	{
		this._forceSync = true;
		this._forceSyncDelay = 5f;
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(player.ActorNumber);
		if (gamePlayer != null)
		{
			gamePlayer.CleanupPlayer();
		}
		if (!this.IsMasterClient())
		{
			return;
		}
		this.photonView.RPC("SetTeamRPC", RpcTarget.All, new object[]
		{
			-1,
			player.GetPlayerRef()
		});
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x000DC06C File Offset: 0x000DA26C
	private void OnMasterClientSwitched(NetPlayer player)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		int[] array;
		int[] array2;
		int[] array3;
		long[] array4;
		long[] array5;
		this.GetCurrentGameState(out array, out array2, out array3, out array4, out array5);
		this.photonView.RPC("RequestSetGameStateRPC", RpcTarget.Others, new object[]
		{
			(int)this.gameState,
			this.gameEndTime,
			array,
			array2,
			array3,
			array4,
			array5
		});
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x000DC0E0 File Offset: 0x000DA2E0
	private void GetCurrentGameState(out int[] playerIds, out int[] playerTeams, out int[] scores, out long[] packedBallPosRot, out long[] packedBallVel)
	{
		NetPlayer[] allNetPlayers = NetworkSystem.Instance.AllNetPlayers;
		playerIds = new int[allNetPlayers.Length];
		playerTeams = new int[allNetPlayers.Length];
		for (int i = 0; i < allNetPlayers.Length; i++)
		{
			playerIds[i] = allNetPlayers[i].ActorNumber;
			GamePlayer gamePlayer = GamePlayer.GetGamePlayer(allNetPlayers[i].ActorNumber);
			if (gamePlayer != null)
			{
				playerTeams[i] = gamePlayer.teamId;
			}
			else
			{
				playerTeams[i] = -1;
			}
		}
		scores = new int[this.team.Count];
		for (int j = 0; j < this.team.Count; j++)
		{
			scores[j] = this.team[j].score;
		}
		packedBallPosRot = new long[this.startingBalls.Count];
		packedBallVel = new long[this.startingBalls.Count];
		for (int k = 0; k < this.startingBalls.Count; k++)
		{
			packedBallPosRot[k] = BitPackUtils.PackHandPosRotForNetwork(this.startingBalls[k].transform.position, this.startingBalls[k].transform.rotation);
			packedBallVel[k] = BitPackUtils.PackWorldPosForNetwork(this.startingBalls[k].gameBall.GetVelocity());
		}
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x0003A201 File Offset: 0x00038401
	private bool IsMasterClient()
	{
		return PhotonNetwork.IsMasterClient;
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x00042AE8 File Offset: 0x00040CE8
	public MonkeBallGame.GameState GetGameState()
	{
		return this.gameState;
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x00042AF0 File Offset: 0x00040CF0
	public void RequestGameState(MonkeBallGame.GameState newGameState)
	{
		if (!this.IsMasterClient())
		{
			return;
		}
		this.photonView.RPC("SetGameStateRPC", RpcTarget.All, new object[]
		{
			(int)newGameState
		});
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x000DC228 File Offset: 0x000DA428
	[PunRPC]
	private void SetGameStateRPC(int newGameState, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SetGameStateRPC");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.SetGameState, info))
		{
			return;
		}
		if (newGameState < 0 || newGameState > 4)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetGameState, info, "newGameState outside of enum range.");
			return;
		}
		this.SetGameState((MonkeBallGame.GameState)newGameState);
		if (newGameState == 1)
		{
			this.gameEndTime = info.SentServerTime + (double)this.gameDuration;
		}
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x000DC290 File Offset: 0x000DA490
	private void SetGameState(MonkeBallGame.GameState newGameState)
	{
		this.gameState = newGameState;
		switch (this.gameState)
		{
		case MonkeBallGame.GameState.PreGame:
			this.OnEnterStatePreGame();
			return;
		case MonkeBallGame.GameState.Playing:
			this.OnEnterStatePlaying();
			return;
		case MonkeBallGame.GameState.PostScore:
			this.OnEnterStatePostScore();
			return;
		case MonkeBallGame.GameState.PostGame:
			this.OnEnterStatePostGame();
			return;
		default:
			return;
		}
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x000DC2E0 File Offset: 0x000DA4E0
	private void OnEnterStatePreGame()
	{
		for (int i = 0; i < this.scoreboards.Count; i++)
		{
			this.scoreboards[i].PlayGameStartFx();
		}
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00042B1B File Offset: 0x00040D1B
	private void OnEnterStatePlaying()
	{
		this._forceSync = true;
		this._forceSyncDelay = 0.1f;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void OnEnterStatePostScore()
	{
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x000DC314 File Offset: 0x000DA514
	private void OnEnterStatePostGame()
	{
		for (int i = 0; i < this.scoreboards.Count; i++)
		{
			this.scoreboards[i].PlayGameEndFx();
		}
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x000DC348 File Offset: 0x000DA548
	[PunRPC]
	private void RequestSetGameStateRPC(int newGameState, double newGameEndTime, int[] playerIds, int[] playerTeams, int[] scores, long[] packedBallPosRot, long[] packedBallVel, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "RequestSetGameStateRPC");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.RequestSetGameState, info))
		{
			return;
		}
		if (playerIds.IsNullOrEmpty<int>() || playerTeams.IsNullOrEmpty<int>() || scores.IsNullOrEmpty<int>() || packedBallPosRot.IsNullOrEmpty<long>() || packedBallVel.IsNullOrEmpty<long>())
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "Array params are null or empty.");
			return;
		}
		if (newGameState < 0 || newGameState > 4)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "newGameState outside of enum range.");
			return;
		}
		if (playerIds.Length != playerTeams.Length)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "playerIDs and playerTeams are not the same length.");
			return;
		}
		if (scores.Length > this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "scores and team are not the same length.");
			return;
		}
		if (packedBallPosRot.Length != this.startingBalls.Count || packedBallPosRot.Length != packedBallVel.Length)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "packedBall arrays are not the same length.");
			return;
		}
		if (double.IsNaN(newGameEndTime) || double.IsInfinity(newGameEndTime))
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "newGameEndTime is not valid.");
			return;
		}
		if (newGameEndTime < -1.0 || newGameEndTime > PhotonNetwork.Time + (double)this.gameDuration)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetGameState, info, "newGameEndTime exceeds possible time limits.");
			return;
		}
		this.gameState = (MonkeBallGame.GameState)newGameState;
		this.gameEndTime = newGameEndTime;
		for (int i = 0; i < playerIds.Length; i++)
		{
			if (VRRigCache.Instance.localRig.Creator.ActorNumber != playerIds[i])
			{
				GamePlayer gamePlayer = GamePlayer.GetGamePlayer(playerIds[i]);
				if (!(gamePlayer == null))
				{
					gamePlayer.teamId = playerTeams[i];
					RigContainer rigContainer;
					if (playerTeams[i] >= 0 && playerTeams[i] < this.team.Count && VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(playerIds[i]), out rigContainer))
					{
						Color color = this.team[playerTeams[i]].color;
						rigContainer.Rig.InitializeNoobMaterialLocal(color.r, color.g, color.b);
						rigContainer.Rig.LocalUpdateCosmeticsWithTryon(CosmeticsController.CosmeticSet.EmptySet, CosmeticsController.CosmeticSet.EmptySet);
					}
				}
			}
		}
		this.RefreshTeamPlayers(false);
		for (int j = 0; j < scores.Length; j++)
		{
			this.SetScore(j, scores[j], false);
		}
		for (int k = 0; k < packedBallPosRot.Length; k++)
		{
			Vector3 position;
			Quaternion rotation;
			BitPackUtils.UnpackHandPosRotFromNetwork(packedBallPosRot[k], out position, out rotation);
			float num = 10000f;
			if (position.IsValid(num) && rotation.IsValid())
			{
				this.startingBalls[k].transform.position = position;
				this.startingBalls[k].transform.rotation = rotation;
				if ((this.startingBalls[k].transform.position - base.transform.position).sqrMagnitude > 6400f)
				{
					this.startingBalls[k].transform.position = this._neutralBallStartLocation.transform.position;
				}
				Vector3 velocity = BitPackUtils.UnpackWorldPosFromNetwork(packedBallVel[k]);
				num = 10000f;
				if (velocity.IsValid(num))
				{
					this.startingBalls[k].gameBall.SetVelocity(velocity);
					this.startingBalls[k].TriggerDelayedResync();
				}
			}
		}
		this._forceSync = true;
		this._forceSyncDelay = 5f;
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00042B2F File Offset: 0x00040D2F
	public void RequestResetGame()
	{
		this.photonView.RPC("RequestResetGameRPC", RpcTarget.All, Array.Empty<object>());
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000DC6B4 File Offset: 0x000DA8B4
	[PunRPC]
	private void RequestResetGameRPC(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestResetGameRPC");
		if (!this.IsMasterClient())
		{
			return;
		}
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.RequestResetGame, info))
		{
			return;
		}
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(info.Sender.ActorNumber);
		if (gamePlayer == null)
		{
			return;
		}
		if (gamePlayer.teamId != this.resetButton.allowedTeamId)
		{
			return;
		}
		for (int i = 0; i < this.startingBalls.Count; i++)
		{
			this.RequestResetBall(this.startingBalls[i].gameBall.id, -1);
		}
		for (int j = 0; j < this.team.Count; j++)
		{
			this.RequestSetScore(j, 0);
		}
		this.RequestGameState(MonkeBallGame.GameState.PreGame);
		this.resetButton.ToggleReset(false, -1, true);
		if (this.centerResetButton != null)
		{
			this.centerResetButton.ToggleReset(false, -1, true);
		}
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000DC794 File Offset: 0x000DA994
	public void ToggleResetButton(bool toggle, int teamId)
	{
		int otherTeam = this.GetOtherTeam(teamId);
		this.photonView.RPC("SetResetButtonRPC", RpcTarget.All, new object[]
		{
			toggle,
			otherTeam
		});
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000DC7D4 File Offset: 0x000DA9D4
	[PunRPC]
	private void SetResetButtonRPC(bool toggleReset, int teamId, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SetResetButtonRPC");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.SetResetButton, info))
		{
			return;
		}
		if (teamId < -1 || teamId >= this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetResetButton, info, "teamID exceeds possible range.");
			return;
		}
		this.resetButton.ToggleReset(toggleReset, teamId, false);
		if (this.centerResetButton != null)
		{
			this.centerResetButton.ToggleReset(toggleReset, teamId, false);
		}
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x00042B47 File Offset: 0x00040D47
	public void OnBallGrabbed(GameBallId gameBallId)
	{
		if (this.gameState == MonkeBallGame.GameState.PreGame)
		{
			this.SetGameState(MonkeBallGame.GameState.Playing);
		}
		if (this.gameState == MonkeBallGame.GameState.PostScore)
		{
			this.SetGameState(MonkeBallGame.GameState.Playing);
		}
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000DC850 File Offset: 0x000DAA50
	private void RefreshTime()
	{
		for (int i = 0; i < this.scoreboards.Count; i++)
		{
			this.scoreboards[i].RefreshTime();
		}
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x000DC884 File Offset: 0x000DAA84
	public void RequestResetBall(GameBallId gameBallId, int teamId)
	{
		if (!this.IsMasterClient())
		{
			return;
		}
		if (teamId >= 0)
		{
			this.LaunchBallWithTeam(gameBallId, teamId, this.team[teamId].ballLaunchPosition, this.team[teamId].ballLaunchVelocityRange, this.team[teamId].ballLaunchAngleXRange, this.team[teamId].ballLaunchAngleXRange);
			return;
		}
		this.LaunchBallNeutral(gameBallId);
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x000DC8F4 File Offset: 0x000DAAF4
	public void RequestScore(int teamId)
	{
		if (!this.IsMasterClient())
		{
			return;
		}
		if (teamId < 0 || teamId >= this.team.Count)
		{
			return;
		}
		this.photonView.RPC("SetScoreRPC", RpcTarget.All, new object[]
		{
			teamId,
			this.team[teamId].score + 1
		});
		this.RequestGameState(MonkeBallGame.GameState.PostScore);
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x00042B69 File Offset: 0x00040D69
	public void RequestSetScore(int teamId, int score)
	{
		if (!this.IsMasterClient())
		{
			return;
		}
		this.photonView.RPC("SetScoreRPC", RpcTarget.All, new object[]
		{
			teamId,
			score
		});
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x000DC960 File Offset: 0x000DAB60
	[PunRPC]
	private void SetScoreRPC(int teamId, int score, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SetScoreRPC");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.SetScore, info))
		{
			return;
		}
		if (teamId < 0 || teamId >= this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetScore, info, "teamID exceeds possible range.");
			return;
		}
		if (score != 0 && score != this.team[teamId].score + 1)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetScore, info, "Score is being set to a non-achievable value.");
			return;
		}
		this.SetScore(teamId, Mathf.Clamp(score, 0, 999), true);
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x000DC9EC File Offset: 0x000DABEC
	private void SetScore(int teamId, int score, bool playFX = true)
	{
		if (teamId < 0 || teamId > this.team.Count)
		{
			return;
		}
		int score2 = this.team[teamId].score;
		this.team[teamId].score = score;
		if (playFX && score > score2)
		{
			this.PlayScoreFx();
			Color color = this.team[teamId].color;
			for (int i = 0; i < this.endZoneEffects.Length; i++)
			{
				this.endZoneEffects[i].startColor = color;
				this.endZoneEffects[i].Play();
			}
		}
		this.RefreshScore();
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x000DCA84 File Offset: 0x000DAC84
	private void RefreshScore()
	{
		for (int i = 0; i < this.scoreboards.Count; i++)
		{
			this.scoreboards[i].RefreshScore();
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000DCAB8 File Offset: 0x000DACB8
	private void PlayScoreFx()
	{
		for (int i = 0; i < this.scoreboards.Count; i++)
		{
			this.scoreboards[i].PlayScoreFx();
		}
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x00042B9D File Offset: 0x00040D9D
	public MonkeBallTeam GetTeam(int teamId)
	{
		return this.team[teamId];
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x00042BAB File Offset: 0x00040DAB
	public int GetOtherTeam(int teamId)
	{
		return (teamId + 1) % this.team.Count;
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000DCAEC File Offset: 0x000DACEC
	public void RequestSetTeam(int teamId)
	{
		this.photonView.RPC("RequestSetTeamRPC", RpcTarget.MasterClient, new object[]
		{
			teamId
		});
		bool flag = false;
		Color color = Color.white;
		if (teamId >= 0 && teamId < this.team.Count)
		{
			flag = true;
			this._storedLocalPlayerColor = new Color(PlayerPrefs.GetFloat("redValue", 1f), PlayerPrefs.GetFloat("greenValue", 1f), PlayerPrefs.GetFloat("blueValue", 1f));
			this._setStoredLocalPlayerColor = true;
			this._forceOrigColorFix = false;
			color = this.team[teamId].color;
		}
		else
		{
			color = this._storedLocalPlayerColor;
			this._setStoredLocalPlayerColor = false;
		}
		PlayerPrefs.SetFloat("redValue", color.r);
		PlayerPrefs.SetFloat("greenValue", color.g);
		PlayerPrefs.SetFloat("blueValue", color.b);
		PlayerPrefs.Save();
		GorillaTagger.Instance.UpdateColor(color.r, color.g, color.b);
		GorillaComputer.instance.UpdateColor(color.r, color.g, color.b);
		if (NetworkSystem.Instance.InRoom)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
			{
				color.r,
				color.g,
				color.b
			});
			if (flag)
			{
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_HideAllCosmetics", RpcTarget.All, Array.Empty<object>());
			}
			else
			{
				this._forceOrigColorFix = true;
				this._forceOrigColorDelay = 3f;
				CosmeticsController.instance.UpdateWornCosmetics(true);
			}
			this.ForceSyncPlayersVisuals();
		}
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x000DCCA4 File Offset: 0x000DAEA4
	private MonkeBall GetMonkeBall(GameBallId gameBallId)
	{
		GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
		if (!(gameBall == null))
		{
			return gameBall.GetComponent<MonkeBall>();
		}
		return null;
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000DCCD0 File Offset: 0x000DAED0
	[PunRPC]
	private void RequestSetTeamRPC(int teamId, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestSetTeamRPC");
		if (!this.IsMasterClient())
		{
			return;
		}
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.RequestSetTeam, info))
		{
			return;
		}
		if (teamId < -1 || teamId >= this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.RequestSetTeam, info, "teamID exceeds possible range.");
			return;
		}
		this.photonView.RPC("SetTeamRPC", RpcTarget.All, new object[]
		{
			teamId,
			info.Sender
		});
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000DCD48 File Offset: 0x000DAF48
	[PunRPC]
	private void SetTeamRPC(int teamId, Player player, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SetTeamRPC");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.SetTeam, info))
		{
			return;
		}
		if (teamId < -1 || teamId >= this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetTeam, info, "teamID exceeds possible range.");
			return;
		}
		this.SetTeamPlayer(teamId, player);
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
	private void SetTeamPlayer(int teamId, Player player)
	{
		if (player == null)
		{
			return;
		}
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(player.ActorNumber);
		if (gamePlayer != null)
		{
			gamePlayer.teamId = teamId;
		}
		this.RefreshTeamPlayers(true);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x000DCDD8 File Offset: 0x000DAFD8
	private void RefreshTeamPlayers(bool playSounds)
	{
		int[] array = new int[this.team.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 0;
		}
		int num = 0;
		NetPlayer[] allNetPlayers = NetworkSystem.Instance.AllNetPlayers;
		for (int j = 0; j < allNetPlayers.Length; j++)
		{
			GamePlayer gamePlayer = GamePlayer.GetGamePlayer(allNetPlayers[j].ActorNumber);
			if (!(gamePlayer == null))
			{
				int teamId = gamePlayer.teamId;
				if (teamId >= 0)
				{
					array[teamId]++;
					num++;
				}
			}
		}
		for (int k = 0; k < this.scoreboards.Count; k++)
		{
			for (int l = 0; l < array.Length; l++)
			{
				this.scoreboards[k].RefreshTeamPlayers(l, array[l]);
			}
			if (playSounds)
			{
				if (this._currentPlayerTotal < num)
				{
					this.scoreboards[k].PlayPlayerJoinFx();
				}
				else if (this._currentPlayerTotal > num)
				{
					this.scoreboards[k].PlayPlayerLeaveFx();
				}
			}
		}
		this._currentPlayerTotal = num;
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000DCEE8 File Offset: 0x000DB0E8
	private void ForceSyncPlayersVisuals()
	{
		for (int i = 0; i < NetworkSystem.Instance.AllNetPlayers.Length; i++)
		{
			int actorNumber = NetworkSystem.Instance.AllNetPlayers[i].ActorNumber;
			if (VRRigCache.Instance.localRig.Creator.ActorNumber != actorNumber)
			{
				GamePlayer gamePlayer = GamePlayer.GetGamePlayer(actorNumber);
				RigContainer rigContainer;
				if (!(gamePlayer == null) && gamePlayer.teamId >= 0 && gamePlayer.teamId < this.team.Count && VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.GetPlayer(actorNumber), out rigContainer))
				{
					Color color = this.team[gamePlayer.teamId].color;
					rigContainer.Rig.InitializeNoobMaterialLocal(color.r, color.g, color.b);
					rigContainer.Rig.LocalUpdateCosmeticsWithTryon(CosmeticsController.CosmeticSet.EmptySet, CosmeticsController.CosmeticSet.EmptySet);
				}
			}
		}
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000DCFD4 File Offset: 0x000DB1D4
	private void ForceOriginalColorSync()
	{
		GamePlayer gamePlayer = GamePlayer.GetGamePlayer(VRRigCache.Instance.localRig.Creator.ActorNumber);
		if (gamePlayer == null || (gamePlayer.teamId >= 0 && gamePlayer.teamId < this.team.Count))
		{
			return;
		}
		Color storedLocalPlayerColor = this._storedLocalPlayerColor;
		if (NetworkSystem.Instance.InRoom)
		{
			GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
			{
				storedLocalPlayerColor.r,
				storedLocalPlayerColor.g,
				storedLocalPlayerColor.b
			});
		}
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x00042BBC File Offset: 0x00040DBC
	public void RequestRestrictBallToTeam(GameBallId gameBallId, int teamId)
	{
		this.RestrictBallToTeam(gameBallId, teamId, this.restrictBallDuration);
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00042BCC File Offset: 0x00040DCC
	public void RequestRestrictBallToTeamOnScore(GameBallId gameBallId, int teamId)
	{
		this.RestrictBallToTeam(gameBallId, teamId, this.restrictBallDurationAfterScore);
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x000DD07C File Offset: 0x000DB27C
	private void RestrictBallToTeam(GameBallId gameBallId, int teamId, float restrictDuration)
	{
		if (!this.IsMasterClient())
		{
			return;
		}
		this.photonView.RPC("SetRestrictBallToTeam", RpcTarget.All, new object[]
		{
			gameBallId.index,
			teamId,
			restrictDuration
		});
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x000DD0CC File Offset: 0x000DB2CC
	[PunRPC]
	private void SetRestrictBallToTeam(int gameBallIndex, int teamId, float restrictDuration, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SetRestrictBallToTeam");
		if (!this.ValidateCallLimits(MonkeBallGame.RPC.SetRestrictBallToTeam, info))
		{
			return;
		}
		if (gameBallIndex < 0 || gameBallIndex >= this.startingBalls.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetRestrictBallToTeam, info, "gameBallIndex exceeds possible range.");
			return;
		}
		if (teamId < -1 || teamId >= this.team.Count)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetRestrictBallToTeam, info, "teamID exceeds possible range.");
			return;
		}
		if (float.IsNaN(restrictDuration) || float.IsInfinity(restrictDuration) || restrictDuration < 0f || restrictDuration > this.restrictBallDurationAfterScore + this.restrictBallDuration)
		{
			this.ReportRPCCall(MonkeBallGame.RPC.SetRestrictBallToTeam, info, "restrictDuration is not a feasible value.");
			return;
		}
		GameBallId gameBallId = new GameBallId(gameBallIndex);
		MonkeBall monkeBall = this.GetMonkeBall(gameBallId);
		bool flag = false;
		if (monkeBall != null)
		{
			flag = monkeBall.RestrictBallToTeam(teamId, restrictDuration);
		}
		if (flag)
		{
			for (int i = 0; i < this.shotclocks.Count; i++)
			{
				this.shotclocks[i].SetTime(teamId, restrictDuration);
			}
		}
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x000DD1CC File Offset: 0x000DB3CC
	public void LaunchBallNeutral(GameBallId gameBallId)
	{
		this.LaunchBall(gameBallId, this._ballLauncher, this.ballLauncherVelocityRange.x, this.ballLauncherVelocityRange.y, this.ballLaunchAngleXRange.x, this.ballLaunchAngleXRange.y, this.ballLaunchAngleYRange.x, this.ballLaunchAngleYRange.y);
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x000DD228 File Offset: 0x000DB428
	public void LaunchBallWithTeam(GameBallId gameBallId, int teamId, Transform launcher, Vector2 velocityRange, Vector2 angleXRange, Vector2 angleYRange)
	{
		this.LaunchBall(gameBallId, launcher, velocityRange.x, velocityRange.y, angleXRange.x, angleXRange.y, angleYRange.x, angleYRange.y);
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000DD268 File Offset: 0x000DB468
	private void LaunchBall(GameBallId gameBallId, Transform launcher, float minVelocity, float maxVelocity, float minXAngle, float maxXAngle, float minYAngle, float maxYAngle)
	{
		GameBall gameBall = GameBallManager.Instance.GetGameBall(gameBallId);
		if (gameBall == null)
		{
			return;
		}
		gameBall.transform.position = launcher.transform.position;
		Quaternion rotation = launcher.transform.rotation;
		launcher.transform.Rotate(Vector3.up, UnityEngine.Random.Range(minXAngle, maxXAngle));
		launcher.transform.Rotate(Vector3.right, UnityEngine.Random.Range(minYAngle, maxYAngle));
		gameBall.transform.rotation = launcher.transform.rotation;
		Vector3 velocity = launcher.transform.forward * UnityEngine.Random.Range(minVelocity, maxVelocity);
		launcher.transform.rotation = rotation;
		GameBallManager.Instance.RequestLaunchBall(gameBallId, velocity);
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void WriteDataFusion()
	{
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void ReadDataFusion()
	{
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0002F861 File Offset: 0x0002DA61
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0002F86D File Offset: 0x0002DA6D
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}

	// Token: 0x04001F92 RID: 8082
	public static MonkeBallGame Instance;

	// Token: 0x04001F93 RID: 8083
	public List<MonkeBall> startingBalls;

	// Token: 0x04001F94 RID: 8084
	public List<MonkeBallScoreboard> scoreboards;

	// Token: 0x04001F95 RID: 8085
	public List<MonkeBallShotclock> shotclocks;

	// Token: 0x04001F96 RID: 8086
	public List<MonkeBallGoalZone> goalZones;

	// Token: 0x04001F97 RID: 8087
	[Space]
	public MonkeBallResetGame resetButton;

	// Token: 0x04001F98 RID: 8088
	public MonkeBallResetGame centerResetButton;

	// Token: 0x04001F99 RID: 8089
	[Space]
	public PhotonView photonView;

	// Token: 0x04001F9A RID: 8090
	public List<MonkeBallTeam> team;

	// Token: 0x04001F9B RID: 8091
	private int _currentPlayerTotal;

	// Token: 0x04001F9C RID: 8092
	[Space]
	[Tooltip("The length of the game in seconds.")]
	public float gameDuration;

	// Token: 0x04001F9D RID: 8093
	[Space]
	[Tooltip("If the ball should be reset to a team starting position after a score. If not set to true then the will reset back to a neutral starting position.")]
	public bool resetBallPositionOnScore = true;

	// Token: 0x04001F9E RID: 8094
	[Tooltip("The duration in which a team is restricted from grabbing the ball after toss.")]
	public float restrictBallDuration = 5f;

	// Token: 0x04001F9F RID: 8095
	[Tooltip("The duration in which a team is restricted from grabbing the ball after a score.")]
	public float restrictBallDurationAfterScore = 10f;

	// Token: 0x04001FA0 RID: 8096
	[Header("Neutral Launcher")]
	[SerializeField]
	private Transform _ballLauncher;

	// Token: 0x04001FA1 RID: 8097
	[Tooltip("The min/max random velocity of the ball when launched.")]
	public Vector2 ballLauncherVelocityRange = new Vector2(8f, 15f);

	// Token: 0x04001FA2 RID: 8098
	[Tooltip("The min/max random x-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleXRange = new Vector2(0f, 0f);

	// Token: 0x04001FA3 RID: 8099
	[Tooltip("The min/max random y-angle of the ball when launched.")]
	public Vector2 ballLaunchAngleYRange = new Vector2(0f, 0f);

	// Token: 0x04001FA4 RID: 8100
	[Space]
	[SerializeField]
	private Transform _neutralBallStartLocation;

	// Token: 0x04001FA5 RID: 8101
	[SerializeField]
	private ParticleSystem[] endZoneEffects;

	// Token: 0x04001FA6 RID: 8102
	private MonkeBallGame.GameState gameState;

	// Token: 0x04001FA7 RID: 8103
	public double gameEndTime;

	// Token: 0x04001FA8 RID: 8104
	private bool _forceSync;

	// Token: 0x04001FA9 RID: 8105
	private float _forceSyncDelay;

	// Token: 0x04001FAA RID: 8106
	private bool _forceOrigColorFix;

	// Token: 0x04001FAB RID: 8107
	private float _forceOrigColorDelay;

	// Token: 0x04001FAC RID: 8108
	private Color _storedLocalPlayerColor;

	// Token: 0x04001FAD RID: 8109
	private bool _setStoredLocalPlayerColor;

	// Token: 0x04001FAE RID: 8110
	private CallLimiter[] _callLimiters;

	// Token: 0x0200049F RID: 1183
	public enum GameState
	{
		// Token: 0x04001FB0 RID: 8112
		None,
		// Token: 0x04001FB1 RID: 8113
		PreGame,
		// Token: 0x04001FB2 RID: 8114
		Playing,
		// Token: 0x04001FB3 RID: 8115
		PostScore,
		// Token: 0x04001FB4 RID: 8116
		PostGame
	}

	// Token: 0x020004A0 RID: 1184
	private enum RPC
	{
		// Token: 0x04001FB6 RID: 8118
		SetGameState,
		// Token: 0x04001FB7 RID: 8119
		RequestSetGameState,
		// Token: 0x04001FB8 RID: 8120
		RequestResetGame,
		// Token: 0x04001FB9 RID: 8121
		SetScore,
		// Token: 0x04001FBA RID: 8122
		RequestSetTeam,
		// Token: 0x04001FBB RID: 8123
		SetTeam,
		// Token: 0x04001FBC RID: 8124
		SetRestrictBallToTeam,
		// Token: 0x04001FBD RID: 8125
		SetResetButton,
		// Token: 0x04001FBE RID: 8126
		Count
	}
}
