using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using GorillaGameModes;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class GorillaPaintbrawlManager : GorillaGameManager
{
	// Token: 0x0600232D RID: 9005 RVA: 0x00047C6C File Offset: 0x00045E6C
	private void ActivatePaintbrawlBalloons(bool enable)
	{
		if (GorillaTagger.Instance.offlineVRRig != null)
		{
			GorillaTagger.Instance.offlineVRRig.paintbrawlBalloons.gameObject.SetActive(enable);
		}
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x00047C9A File Offset: 0x00045E9A
	private bool HasFlag(GorillaPaintbrawlManager.PaintbrawlStatus state, GorillaPaintbrawlManager.PaintbrawlStatus statusFlag)
	{
		return (state & statusFlag) > GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x00047CA2 File Offset: 0x00045EA2
	public override GameModeType GameType()
	{
		return GameModeType.Paintbrawl;
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x00047CA5 File Offset: 0x00045EA5
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<BattleGameModeData>();
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x00047CAE File Offset: 0x00045EAE
	public override string GameModeName()
	{
		return "PAINTBRAWL";
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000FC1E4 File Offset: 0x000FA3E4
	private void ActivateDefaultSlingShot()
	{
		VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
		if (offlineVRRig != null && !Slingshot.IsSlingShotEnabled())
		{
			CosmeticsController instance = CosmeticsController.instance;
			CosmeticsController.CosmeticItem itemFromDict = instance.GetItemFromDict("Slingshot");
			instance.ApplyCosmeticItemToSet(offlineVRRig.cosmeticSet, itemFromDict, true, false);
		}
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x00047CB5 File Offset: 0x00045EB5
	public override void Awake()
	{
		base.Awake();
		this.coroutineRunning = false;
		this.currentState = GorillaPaintbrawlManager.PaintbrawlState.NotEnoughPlayers;
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000FC230 File Offset: 0x000FA430
	public override void StartPlaying()
	{
		base.StartPlaying();
		this.ActivatePaintbrawlBalloons(true);
		this.VerifyPlayersInDict<int>(this.playerLives);
		this.VerifyPlayersInDict<GorillaPaintbrawlManager.PaintbrawlStatus>(this.playerStatusDict);
		this.VerifyPlayersInDict<float>(this.playerHitTimes);
		this.VerifyPlayersInDict<float>(this.playerStunTimes);
		this.CopyBattleDictToArray();
		this.UpdateBattleState();
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000FC288 File Offset: 0x000FA488
	public override void StopPlaying()
	{
		base.StopPlaying();
		if (Slingshot.IsSlingShotEnabled())
		{
			CosmeticsController instance = CosmeticsController.instance;
			VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			CosmeticsController.CosmeticItem itemFromDict = instance.GetItemFromDict("Slingshot");
			if (offlineVRRig.cosmeticSet.HasItem("Slingshot"))
			{
				instance.ApplyCosmeticItemToSet(offlineVRRig.cosmeticSet, itemFromDict, true, false);
			}
		}
		this.ActivatePaintbrawlBalloons(false);
		base.StopAllCoroutines();
		this.coroutineRunning = false;
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000FC2F8 File Offset: 0x000FA4F8
	public override void Reset()
	{
		base.Reset();
		this.playerLives.Clear();
		this.playerStatusDict.Clear();
		this.playerHitTimes.Clear();
		this.playerStunTimes.Clear();
		for (int i = 0; i < this.playerActorNumberArray.Length; i++)
		{
			this.playerLivesArray[i] = 0;
			this.playerActorNumberArray[i] = -1;
			this.playerStatusArray[i] = GorillaPaintbrawlManager.PaintbrawlStatus.None;
		}
		this.currentState = GorillaPaintbrawlManager.PaintbrawlState.NotEnoughPlayers;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000FC36C File Offset: 0x000FA56C
	private void VerifyPlayersInDict<T>(Dictionary<int, T> dict)
	{
		if (dict.Count < 1)
		{
			return;
		}
		int[] array = dict.Keys.ToArray<int>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!Utils.PlayerInRoom(array[i]))
			{
				dict.Remove(array[i]);
			}
		}
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x00047CCB File Offset: 0x00045ECB
	internal override void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		base.NetworkLinkSetup(netSerializer);
		netSerializer.AddRPCComponent<PaintbrawlRPCs>();
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x00047CDB File Offset: 0x00045EDB
	private void Transition(GorillaPaintbrawlManager.PaintbrawlState newState)
	{
		this.currentState = newState;
		Debug.Log("current state is: " + this.currentState.ToString());
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000FC3B4 File Offset: 0x000FA5B4
	public void UpdateBattleState()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			switch (this.currentState)
			{
			case GorillaPaintbrawlManager.PaintbrawlState.NotEnoughPlayers:
				if ((float)RoomSystem.PlayersInRoom.Count >= this.playerMin)
				{
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.StartCountdown);
				}
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.GameEnd:
				if (this.EndBattleGame())
				{
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.GameEndWaiting);
				}
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.GameEndWaiting:
				if (this.BattleEnd())
				{
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.StartCountdown);
				}
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.StartCountdown:
				this.RandomizeTeams();
				this.ActivatePaintbrawlBalloons(true);
				base.StartCoroutine(this.StartBattleCountdown());
				this.Transition(GorillaPaintbrawlManager.PaintbrawlState.CountingDownToStart);
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.CountingDownToStart:
				if (!this.coroutineRunning)
				{
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.StartCountdown);
				}
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.GameStart:
				this.StartBattle();
				this.Transition(GorillaPaintbrawlManager.PaintbrawlState.GameRunning);
				break;
			case GorillaPaintbrawlManager.PaintbrawlState.GameRunning:
				if (this.CheckForGameEnd())
				{
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.GameEnd);
					PlayerGameEvents.GameModeCompleteRound();
					GorillaGameModes.GameMode.BroadcastRoundComplete();
				}
				if ((float)RoomSystem.PlayersInRoom.Count < this.playerMin)
				{
					this.InitializePlayerStatus();
					this.ActivatePaintbrawlBalloons(false);
					this.Transition(GorillaPaintbrawlManager.PaintbrawlState.NotEnoughPlayers);
				}
				break;
			}
			this.UpdatePlayerStatus();
		}
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000FC4CC File Offset: 0x000FA6CC
	private bool CheckForGameEnd()
	{
		this.bcount = 0;
		this.rcount = 0;
		foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
		{
			if (this.playerLives.TryGetValue(netPlayer.ActorNumber, out this.lives))
			{
				if (this.lives > 0 && this.playerStatusDict.TryGetValue(netPlayer.ActorNumber, out this.tempStatus))
				{
					if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam))
					{
						this.rcount++;
					}
					else if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam))
					{
						this.bcount++;
					}
				}
			}
			else
			{
				this.playerLives.Add(netPlayer.ActorNumber, 0);
			}
		}
		return this.bcount == 0 || this.rcount == 0;
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x00047D04 File Offset: 0x00045F04
	public IEnumerator StartBattleCountdown()
	{
		this.coroutineRunning = true;
		this.countDownTime = 5;
		while (this.countDownTime > 0)
		{
			try
			{
				RoomSystem.SendSoundEffectAll(6, 0.25f, false);
				foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
				{
					this.playerLives[netPlayer.ActorNumber] = 3;
				}
			}
			catch
			{
			}
			yield return new WaitForSeconds(1f);
			this.countDownTime--;
		}
		this.coroutineRunning = false;
		this.currentState = GorillaPaintbrawlManager.PaintbrawlState.GameStart;
		yield return null;
		yield break;
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000FC5CC File Offset: 0x000FA7CC
	public void StartBattle()
	{
		RoomSystem.SendSoundEffectAll(7, 0.5f, false);
		foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
		{
			this.playerLives[netPlayer.ActorNumber] = 3;
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x00047D13 File Offset: 0x00045F13
	private bool EndBattleGame()
	{
		if ((float)RoomSystem.PlayersInRoom.Count >= this.playerMin)
		{
			RoomSystem.SendStatusEffectAll(RoomSystem.StatusEffects.TaggedTime);
			RoomSystem.SendSoundEffectAll(2, 0.25f, false);
			this.timeBattleEnded = Time.time;
			return true;
		}
		return false;
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x00047D48 File Offset: 0x00045F48
	public bool BattleEnd()
	{
		return Time.time > this.timeBattleEnded + this.tagCoolDown;
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x00047D5E File Offset: 0x00045F5E
	public bool SlingshotHit(NetPlayer myPlayer, Player otherPlayer)
	{
		return this.playerLives.TryGetValue(otherPlayer.ActorNumber, out this.lives) && this.lives > 0;
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000FC638 File Offset: 0x000FA838
	public void ReportSlingshotHit(NetPlayer taggedPlayer, Vector3 hitLocation, int projectileCount, PhotonMessageInfoWrapped info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		if (this.currentState != GorillaPaintbrawlManager.PaintbrawlState.GameRunning)
		{
			return;
		}
		if (this.OnSameTeam(taggedPlayer, player))
		{
			return;
		}
		if (this.GetPlayerLives(taggedPlayer) > 0 && this.GetPlayerLives(player) > 0 && !this.PlayerInHitCooldown(taggedPlayer))
		{
			if (!this.playerHitTimes.TryGetValue(taggedPlayer.ActorNumber, out this.outHitTime))
			{
				this.playerHitTimes.Add(taggedPlayer.ActorNumber, Time.time);
			}
			else
			{
				this.playerHitTimes[taggedPlayer.ActorNumber] = Time.time;
			}
			Dictionary<int, int> dictionary = this.playerLives;
			int actorNumber = taggedPlayer.ActorNumber;
			int num = dictionary[actorNumber];
			dictionary[actorNumber] = num - 1;
			RoomSystem.SendSoundEffectOnOther(0, 0.25f, taggedPlayer, false);
			return;
		}
		if (this.GetPlayerLives(player) == 0 && this.GetPlayerLives(taggedPlayer) > 0)
		{
			this.tempStatus = this.GetPlayerStatus(taggedPlayer);
			if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Normal) && !this.PlayerInHitCooldown(taggedPlayer) && !this.PlayerInStunCooldown(taggedPlayer))
			{
				if (!this.playerStunTimes.TryGetValue(taggedPlayer.ActorNumber, out this.outHitTime))
				{
					this.playerStunTimes.Add(taggedPlayer.ActorNumber, Time.time);
				}
				else
				{
					this.playerStunTimes[taggedPlayer.ActorNumber] = Time.time;
				}
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.SetSlowedTime, taggedPlayer);
				RoomSystem.SendSoundEffectOnOther(5, 0.125f, taggedPlayer, false);
				RigContainer rigContainer;
				if (VRRigCache.Instance.TryGetVrrig(taggedPlayer, out rigContainer))
				{
					this.tempView = rigContainer.Rig.netView;
				}
			}
		}
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000FC7D4 File Offset: 0x000FA9D4
	public override void HitPlayer(NetPlayer player)
	{
		if (!NetworkSystem.Instance.IsMasterClient || this.currentState != GorillaPaintbrawlManager.PaintbrawlState.GameRunning)
		{
			return;
		}
		if (this.GetPlayerLives(player) > 0)
		{
			this.playerLives[player.ActorNumber] = 0;
			RoomSystem.SendSoundEffectOnOther(0, 0.25f, player, false);
		}
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x00047D84 File Offset: 0x00045F84
	public override bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		return this.playerLives.TryGetValue(player.ActorNumber, out this.lives) && this.lives > 0;
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000FC820 File Offset: 0x000FAA20
	public override void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (this.currentState == GorillaPaintbrawlManager.PaintbrawlState.GameRunning)
			{
				this.playerLives.Add(newPlayer.ActorNumber, 0);
			}
			else
			{
				this.playerLives.Add(newPlayer.ActorNumber, 3);
			}
			this.playerStatusDict.Add(newPlayer.ActorNumber, GorillaPaintbrawlManager.PaintbrawlStatus.None);
			this.CopyBattleDictToArray();
			this.AddPlayerToCorrectTeam(newPlayer);
		}
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000FC890 File Offset: 0x000FAA90
	public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		if (this.playerLives.ContainsKey(otherPlayer.ActorNumber))
		{
			this.playerLives.Remove(otherPlayer.ActorNumber);
		}
		if (this.playerStatusDict.ContainsKey(otherPlayer.ActorNumber))
		{
			this.playerStatusDict.Remove(otherPlayer.ActorNumber);
		}
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000FC8F0 File Offset: 0x000FAAF0
	public override void OnSerializeRead(object newData)
	{
		PaintbrawlData paintbrawlData = (PaintbrawlData)newData;
		paintbrawlData.playerActorNumberArray.CopyTo(this.playerActorNumberArray, true);
		paintbrawlData.playerLivesArray.CopyTo(this.playerLivesArray, true);
		paintbrawlData.playerStatusArray.CopyTo(this.playerStatusArray, true);
		this.currentState = paintbrawlData.currentPaintbrawlState;
		this.CopyArrayToBattleDict();
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000FC958 File Offset: 0x000FAB58
	public override object OnSerializeWrite()
	{
		this.CopyBattleDictToArray();
		PaintbrawlData paintbrawlData = default(PaintbrawlData);
		paintbrawlData.playerActorNumberArray.CopyFrom(this.playerActorNumberArray, 0, this.playerActorNumberArray.Length);
		paintbrawlData.playerLivesArray.CopyFrom(this.playerLivesArray, 0, this.playerLivesArray.Length);
		paintbrawlData.playerStatusArray.CopyFrom(this.playerStatusArray, 0, this.playerStatusArray.Length);
		paintbrawlData.currentPaintbrawlState = this.currentState;
		return paintbrawlData;
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000FC9E0 File Offset: 0x000FABE0
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
		this.CopyBattleDictToArray();
		for (int i = 0; i < this.playerLivesArray.Length; i++)
		{
			stream.SendNext(this.playerActorNumberArray[i]);
			stream.SendNext(this.playerLivesArray[i]);
			stream.SendNext(this.playerStatusArray[i]);
		}
		stream.SendNext((int)this.currentState);
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000FCA50 File Offset: 0x000FAC50
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
		NetworkSystem.Instance.GetPlayer(info.Sender);
		for (int i = 0; i < this.playerLivesArray.Length; i++)
		{
			this.playerActorNumberArray[i] = (int)stream.ReceiveNext();
			this.playerLivesArray[i] = (int)stream.ReceiveNext();
			this.playerStatusArray[i] = (GorillaPaintbrawlManager.PaintbrawlStatus)stream.ReceiveNext();
		}
		this.currentState = (GorillaPaintbrawlManager.PaintbrawlState)stream.ReceiveNext();
		this.CopyArrayToBattleDict();
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000FCAD4 File Offset: 0x000FACD4
	public override int MyMatIndex(NetPlayer forPlayer)
	{
		this.tempStatus = this.GetPlayerStatus(forPlayer);
		if (this.tempStatus != GorillaPaintbrawlManager.PaintbrawlStatus.None)
		{
			if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam))
			{
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Normal))
				{
					return 8;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Hit))
				{
					return 9;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Stunned))
				{
					return 10;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Grace))
				{
					return 10;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Eliminated))
				{
					return 11;
				}
			}
			else
			{
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Normal))
				{
					return 4;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Hit))
				{
					return 5;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Stunned))
				{
					return 6;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Grace))
				{
					return 6;
				}
				if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Eliminated))
				{
					return 7;
				}
			}
		}
		return 0;
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000FCBC0 File Offset: 0x000FADC0
	public override float[] LocalPlayerSpeed()
	{
		if (this.playerStatusDict.TryGetValue(NetworkSystem.Instance.LocalPlayerID, out this.tempStatus))
		{
			if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Normal))
			{
				this.playerSpeed[0] = 6.5f;
				this.playerSpeed[1] = 1.1f;
				return this.playerSpeed;
			}
			if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Stunned))
			{
				this.playerSpeed[0] = 2f;
				this.playerSpeed[1] = 0.5f;
				return this.playerSpeed;
			}
			if (this.HasFlag(this.tempStatus, GorillaPaintbrawlManager.PaintbrawlStatus.Eliminated))
			{
				this.playerSpeed[0] = this.fastJumpLimit;
				this.playerSpeed[1] = this.fastJumpMultiplier;
				return this.playerSpeed;
			}
		}
		this.playerSpeed[0] = 6.5f;
		this.playerSpeed[1] = 1.1f;
		return this.playerSpeed;
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x00047DAA File Offset: 0x00045FAA
	public override void Tick()
	{
		base.Tick();
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.UpdateBattleState();
		}
		this.ActivateDefaultSlingShot();
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000FCCA4 File Offset: 0x000FAEA4
	public override void InfrequentUpdate()
	{
		base.InfrequentUpdate();
		foreach (int num in this.playerLives.Keys)
		{
			this.playerInList = false;
			using (List<NetPlayer>.Enumerator enumerator2 = RoomSystem.PlayersInRoom.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.ActorNumber == num)
					{
						this.playerInList = true;
					}
				}
			}
			if (!this.playerInList)
			{
				this.playerLives.Remove(num);
			}
		}
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x00047DCA File Offset: 0x00045FCA
	public int GetPlayerLives(NetPlayer player)
	{
		if (player == null)
		{
			return 0;
		}
		if (this.playerLives.TryGetValue(player.ActorNumber, out this.outLives))
		{
			return this.outLives;
		}
		return 0;
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000FCD60 File Offset: 0x000FAF60
	public bool PlayerInHitCooldown(NetPlayer player)
	{
		float num;
		return this.playerHitTimes.TryGetValue(player.ActorNumber, out num) && num + this.hitCooldown > Time.time;
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000FCD94 File Offset: 0x000FAF94
	public bool PlayerInStunCooldown(NetPlayer player)
	{
		float num;
		return this.playerStunTimes.TryGetValue(player.ActorNumber, out num) && num + this.hitCooldown + this.stunGracePeriod > Time.time;
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x00047DF2 File Offset: 0x00045FF2
	public GorillaPaintbrawlManager.PaintbrawlStatus GetPlayerStatus(NetPlayer player)
	{
		if (this.playerStatusDict.TryGetValue(player.ActorNumber, out this.tempStatus))
		{
			return this.tempStatus;
		}
		return GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x00047E15 File Offset: 0x00046015
	public bool OnRedTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return this.HasFlag(status, GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam);
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000FCDD0 File Offset: 0x000FAFD0
	public bool OnRedTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnRedTeam(playerStatus);
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x00047E1F File Offset: 0x0004601F
	public bool OnBlueTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return this.HasFlag(status, GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam);
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000FCDEC File Offset: 0x000FAFEC
	public bool OnBlueTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnBlueTeam(playerStatus);
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x00047E29 File Offset: 0x00046029
	public bool OnNoTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return !this.OnRedTeam(status) && !this.OnBlueTeam(status);
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000FCE08 File Offset: 0x000FB008
	public bool OnNoTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnNoTeam(playerStatus);
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x00030498 File Offset: 0x0002E698
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return false;
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000FCE24 File Offset: 0x000FB024
	public bool OnSameTeam(GorillaPaintbrawlManager.PaintbrawlStatus playerA, GorillaPaintbrawlManager.PaintbrawlStatus playerB)
	{
		bool flag = this.OnRedTeam(playerA) && this.OnRedTeam(playerB);
		bool flag2 = this.OnBlueTeam(playerA) && this.OnBlueTeam(playerB);
		return flag || flag2;
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000FCE5C File Offset: 0x000FB05C
	public bool OnSameTeam(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(myPlayer);
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus2 = this.GetPlayerStatus(otherPlayer);
		return this.OnSameTeam(playerStatus, playerStatus2);
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000FCE84 File Offset: 0x000FB084
	public bool LocalCanHit(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		bool flag = !this.OnSameTeam(myPlayer, otherPlayer);
		bool flag2 = this.GetPlayerLives(otherPlayer) != 0;
		return flag && flag2;
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000FCEAC File Offset: 0x000FB0AC
	private void CopyBattleDictToArray()
	{
		for (int i = 0; i < this.playerLivesArray.Length; i++)
		{
			this.playerLivesArray[i] = 0;
			this.playerActorNumberArray[i] = -1;
		}
		this.keyValuePairs = this.playerLives.ToArray<KeyValuePair<int, int>>();
		int num = 0;
		while (num < this.playerLivesArray.Length && num < this.keyValuePairs.Length)
		{
			this.playerActorNumberArray[num] = this.keyValuePairs[num].Key;
			this.playerLivesArray[num] = this.keyValuePairs[num].Value;
			this.playerStatusArray[num] = this.GetPlayerStatus(NetworkSystem.Instance.GetPlayer(this.keyValuePairs[num].Key));
			num++;
		}
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000FCF68 File Offset: 0x000FB168
	private void CopyArrayToBattleDict()
	{
		for (int i = 0; i < this.playerLivesArray.Length; i++)
		{
			if (this.playerActorNumberArray[i] != -1 && Utils.PlayerInRoom(this.playerActorNumberArray[i]))
			{
				if (this.playerLives.TryGetValue(this.playerActorNumberArray[i], out this.outLives))
				{
					this.playerLives[this.playerActorNumberArray[i]] = this.playerLivesArray[i];
				}
				else
				{
					this.playerLives.Add(this.playerActorNumberArray[i], this.playerLivesArray[i]);
				}
				if (this.playerStatusDict.ContainsKey(this.playerActorNumberArray[i]))
				{
					this.playerStatusDict[this.playerActorNumberArray[i]] = this.playerStatusArray[i];
				}
				else
				{
					this.playerStatusDict.Add(this.playerActorNumberArray[i], this.playerStatusArray[i]);
				}
			}
		}
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x00047E40 File Offset: 0x00046040
	private GorillaPaintbrawlManager.PaintbrawlStatus SetFlag(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return currState | flag;
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x00047E45 File Offset: 0x00046045
	private GorillaPaintbrawlManager.PaintbrawlStatus SetFlagExclusive(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return flag;
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x00047E48 File Offset: 0x00046048
	private GorillaPaintbrawlManager.PaintbrawlStatus ClearFlag(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return currState & ~flag;
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x00047C9A File Offset: 0x00045E9A
	private bool FlagIsSet(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return (currState & flag) > GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000FD050 File Offset: 0x000FB250
	public void RandomizeTeams()
	{
		int[] array = new int[RoomSystem.PlayersInRoom.Count];
		for (int i = 0; i < RoomSystem.PlayersInRoom.Count; i++)
		{
			array[i] = i;
		}
		System.Random rand = new System.Random();
		int[] array2 = (from x in array
		orderby rand.Next()
		select x).ToArray<int>();
		GorillaPaintbrawlManager.PaintbrawlStatus paintbrawlStatus = (rand.Next(0, 2) == 0) ? GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam : GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam;
		GorillaPaintbrawlManager.PaintbrawlStatus paintbrawlStatus2 = (paintbrawlStatus == GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam) ? GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam : GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam;
		for (int j = 0; j < RoomSystem.PlayersInRoom.Count; j++)
		{
			GorillaPaintbrawlManager.PaintbrawlStatus value = (array2[j] % 2 == 0) ? paintbrawlStatus2 : paintbrawlStatus;
			this.playerStatusDict[RoomSystem.PlayersInRoom[j].ActorNumber] = value;
		}
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000FD11C File Offset: 0x000FB31C
	public void AddPlayerToCorrectTeam(NetPlayer newPlayer)
	{
		this.rcount = 0;
		for (int i = 0; i < RoomSystem.PlayersInRoom.Count; i++)
		{
			if (this.playerStatusDict.ContainsKey(RoomSystem.PlayersInRoom[i].ActorNumber))
			{
				GorillaPaintbrawlManager.PaintbrawlStatus state = this.playerStatusDict[RoomSystem.PlayersInRoom[i].ActorNumber];
				this.rcount = (this.HasFlag(state, GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam) ? (this.rcount + 1) : this.rcount);
			}
		}
		if ((RoomSystem.PlayersInRoom.Count - 1) / 2 == this.rcount)
		{
			this.playerStatusDict[newPlayer.ActorNumber] = ((UnityEngine.Random.Range(0, 2) == 0) ? this.SetFlag(this.playerStatusDict[newPlayer.ActorNumber], GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam) : this.SetFlag(this.playerStatusDict[newPlayer.ActorNumber], GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam));
			return;
		}
		if (this.rcount <= (RoomSystem.PlayersInRoom.Count - 1) / 2)
		{
			this.playerStatusDict[newPlayer.ActorNumber] = this.SetFlag(this.playerStatusDict[newPlayer.ActorNumber], GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam);
		}
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000FD240 File Offset: 0x000FB440
	private void InitializePlayerStatus()
	{
		this.keyValuePairsStatus = this.playerStatusDict.ToArray<KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus>>();
		foreach (KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus> keyValuePair in this.keyValuePairsStatus)
		{
			this.playerStatusDict[keyValuePair.Key] = GorillaPaintbrawlManager.PaintbrawlStatus.Normal;
		}
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000FD290 File Offset: 0x000FB490
	private void UpdatePlayerStatus()
	{
		this.keyValuePairsStatus = this.playerStatusDict.ToArray<KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus>>();
		foreach (KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus> keyValuePair in this.keyValuePairsStatus)
		{
			GorillaPaintbrawlManager.PaintbrawlStatus paintbrawlStatus = this.HasFlag(this.playerStatusDict[keyValuePair.Key], GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam) ? GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam : GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam;
			if (this.playerLives.TryGetValue(keyValuePair.Key, out this.outLives) && this.outLives == 0)
			{
				this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Eliminated);
			}
			else if (this.playerHitTimes.TryGetValue(keyValuePair.Key, out this.outHitTime) && this.outHitTime + this.hitCooldown > Time.time)
			{
				this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Hit);
			}
			else if (this.playerStunTimes.TryGetValue(keyValuePair.Key, out this.outHitTime))
			{
				if (this.outHitTime + this.hitCooldown > Time.time)
				{
					this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Stunned);
				}
				else if (this.outHitTime + this.hitCooldown + this.stunGracePeriod > Time.time)
				{
					this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Grace);
				}
				else
				{
					this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Normal);
				}
			}
			else
			{
				this.playerStatusDict[keyValuePair.Key] = (paintbrawlStatus | GorillaPaintbrawlManager.PaintbrawlStatus.Normal);
			}
		}
	}

	// Token: 0x040026C9 RID: 9929
	private float playerMin = 2f;

	// Token: 0x040026CA RID: 9930
	public float tagCoolDown = 5f;

	// Token: 0x040026CB RID: 9931
	public Dictionary<int, int> playerLives = new Dictionary<int, int>();

	// Token: 0x040026CC RID: 9932
	public Dictionary<int, GorillaPaintbrawlManager.PaintbrawlStatus> playerStatusDict = new Dictionary<int, GorillaPaintbrawlManager.PaintbrawlStatus>();

	// Token: 0x040026CD RID: 9933
	public Dictionary<int, float> playerHitTimes = new Dictionary<int, float>();

	// Token: 0x040026CE RID: 9934
	public Dictionary<int, float> playerStunTimes = new Dictionary<int, float>();

	// Token: 0x040026CF RID: 9935
	public int[] playerActorNumberArray = new int[]
	{
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1
	};

	// Token: 0x040026D0 RID: 9936
	public int[] playerLivesArray = new int[10];

	// Token: 0x040026D1 RID: 9937
	public GorillaPaintbrawlManager.PaintbrawlStatus[] playerStatusArray = new GorillaPaintbrawlManager.PaintbrawlStatus[10];

	// Token: 0x040026D2 RID: 9938
	public bool teamBattle = true;

	// Token: 0x040026D3 RID: 9939
	public int countDownTime;

	// Token: 0x040026D4 RID: 9940
	private float timeBattleEnded;

	// Token: 0x040026D5 RID: 9941
	public float hitCooldown = 3f;

	// Token: 0x040026D6 RID: 9942
	public float stunGracePeriod = 2f;

	// Token: 0x040026D7 RID: 9943
	public object objRef;

	// Token: 0x040026D8 RID: 9944
	private bool playerInList;

	// Token: 0x040026D9 RID: 9945
	private bool coroutineRunning;

	// Token: 0x040026DA RID: 9946
	private int lives;

	// Token: 0x040026DB RID: 9947
	private int outLives;

	// Token: 0x040026DC RID: 9948
	private int bcount;

	// Token: 0x040026DD RID: 9949
	private int rcount;

	// Token: 0x040026DE RID: 9950
	private int randInt;

	// Token: 0x040026DF RID: 9951
	private float outHitTime;

	// Token: 0x040026E0 RID: 9952
	private NetworkView tempView;

	// Token: 0x040026E1 RID: 9953
	private KeyValuePair<int, int>[] keyValuePairs;

	// Token: 0x040026E2 RID: 9954
	private KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus>[] keyValuePairsStatus;

	// Token: 0x040026E3 RID: 9955
	private GorillaPaintbrawlManager.PaintbrawlStatus tempStatus;

	// Token: 0x040026E4 RID: 9956
	private GorillaPaintbrawlManager.PaintbrawlState currentState;

	// Token: 0x02000595 RID: 1429
	public enum PaintbrawlStatus
	{
		// Token: 0x040026E6 RID: 9958
		RedTeam = 1,
		// Token: 0x040026E7 RID: 9959
		BlueTeam,
		// Token: 0x040026E8 RID: 9960
		Normal = 4,
		// Token: 0x040026E9 RID: 9961
		Hit = 8,
		// Token: 0x040026EA RID: 9962
		Stunned = 16,
		// Token: 0x040026EB RID: 9963
		Grace = 32,
		// Token: 0x040026EC RID: 9964
		Eliminated = 64,
		// Token: 0x040026ED RID: 9965
		None = 0
	}

	// Token: 0x02000596 RID: 1430
	public enum PaintbrawlState
	{
		// Token: 0x040026EF RID: 9967
		NotEnoughPlayers,
		// Token: 0x040026F0 RID: 9968
		GameEnd,
		// Token: 0x040026F1 RID: 9969
		GameEndWaiting,
		// Token: 0x040026F2 RID: 9970
		StartCountdown,
		// Token: 0x040026F3 RID: 9971
		CountingDownToStart,
		// Token: 0x040026F4 RID: 9972
		GameStart,
		// Token: 0x040026F5 RID: 9973
		GameRunning
	}
}
