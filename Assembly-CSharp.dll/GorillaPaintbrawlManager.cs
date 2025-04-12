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

// Token: 0x02000587 RID: 1415
public class GorillaPaintbrawlManager : GorillaGameManager
{
	// Token: 0x060022D5 RID: 8917 RVA: 0x0004686E File Offset: 0x00044A6E
	private void ActivatePaintbrawlBalloons(bool enable)
	{
		if (GorillaTagger.Instance.offlineVRRig != null)
		{
			GorillaTagger.Instance.offlineVRRig.paintbrawlBalloons.gameObject.SetActive(enable);
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x0004689C File Offset: 0x00044A9C
	private bool HasFlag(GorillaPaintbrawlManager.PaintbrawlStatus state, GorillaPaintbrawlManager.PaintbrawlStatus statusFlag)
	{
		return (state & statusFlag) > GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000468A4 File Offset: 0x00044AA4
	public override GameModeType GameType()
	{
		return GameModeType.Paintbrawl;
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000468A7 File Offset: 0x00044AA7
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<BattleGameModeData>();
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000468B0 File Offset: 0x00044AB0
	public override string GameModeName()
	{
		return "PAINTBRAWL";
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000F9468 File Offset: 0x000F7668
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

	// Token: 0x060022DB RID: 8923 RVA: 0x000468B7 File Offset: 0x00044AB7
	public override void Awake()
	{
		base.Awake();
		this.coroutineRunning = false;
		this.currentState = GorillaPaintbrawlManager.PaintbrawlState.NotEnoughPlayers;
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000F94B4 File Offset: 0x000F76B4
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

	// Token: 0x060022DD RID: 8925 RVA: 0x000F950C File Offset: 0x000F770C
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

	// Token: 0x060022DE RID: 8926 RVA: 0x000F957C File Offset: 0x000F777C
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

	// Token: 0x060022DF RID: 8927 RVA: 0x000F95F0 File Offset: 0x000F77F0
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

	// Token: 0x060022E0 RID: 8928 RVA: 0x000468CD File Offset: 0x00044ACD
	internal override void NetworkLinkSetup(GameModeSerializer netSerializer)
	{
		base.NetworkLinkSetup(netSerializer);
		netSerializer.AddRPCComponent<PaintbrawlRPCs>();
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000468DD File Offset: 0x00044ADD
	private void Transition(GorillaPaintbrawlManager.PaintbrawlState newState)
	{
		this.currentState = newState;
		Debug.Log("current state is: " + this.currentState.ToString());
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000F9638 File Offset: 0x000F7838
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

	// Token: 0x060022E3 RID: 8931 RVA: 0x000F9750 File Offset: 0x000F7950
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

	// Token: 0x060022E4 RID: 8932 RVA: 0x00046906 File Offset: 0x00044B06
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

	// Token: 0x060022E5 RID: 8933 RVA: 0x000F9850 File Offset: 0x000F7A50
	public void StartBattle()
	{
		RoomSystem.SendSoundEffectAll(7, 0.5f, false);
		foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
		{
			this.playerLives[netPlayer.ActorNumber] = 3;
		}
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x00046915 File Offset: 0x00044B15
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

	// Token: 0x060022E7 RID: 8935 RVA: 0x0004694A File Offset: 0x00044B4A
	public bool BattleEnd()
	{
		return Time.time > this.timeBattleEnded + this.tagCoolDown;
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x00046960 File Offset: 0x00044B60
	public bool SlingshotHit(NetPlayer myPlayer, Player otherPlayer)
	{
		return this.playerLives.TryGetValue(otherPlayer.ActorNumber, out this.lives) && this.lives > 0;
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000F98BC File Offset: 0x000F7ABC
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

	// Token: 0x060022EA RID: 8938 RVA: 0x000F9A58 File Offset: 0x000F7C58
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

	// Token: 0x060022EB RID: 8939 RVA: 0x00046986 File Offset: 0x00044B86
	public override bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		return this.playerLives.TryGetValue(player.ActorNumber, out this.lives) && this.lives > 0;
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000F9AA4 File Offset: 0x000F7CA4
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

	// Token: 0x060022ED RID: 8941 RVA: 0x000F9B14 File Offset: 0x000F7D14
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

	// Token: 0x060022EE RID: 8942 RVA: 0x000F9B74 File Offset: 0x000F7D74
	public override void OnSerializeRead(object newData)
	{
		PaintbrawlData paintbrawlData = (PaintbrawlData)newData;
		paintbrawlData.playerActorNumberArray.CopyTo(this.playerActorNumberArray, true);
		paintbrawlData.playerLivesArray.CopyTo(this.playerLivesArray, true);
		paintbrawlData.playerStatusArray.CopyTo(this.playerStatusArray, true);
		this.currentState = paintbrawlData.currentPaintbrawlState;
		this.CopyArrayToBattleDict();
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000F9BDC File Offset: 0x000F7DDC
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

	// Token: 0x060022F0 RID: 8944 RVA: 0x000F9C64 File Offset: 0x000F7E64
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

	// Token: 0x060022F1 RID: 8945 RVA: 0x000F9CD4 File Offset: 0x000F7ED4
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

	// Token: 0x060022F2 RID: 8946 RVA: 0x000F9D58 File Offset: 0x000F7F58
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

	// Token: 0x060022F3 RID: 8947 RVA: 0x000F9E44 File Offset: 0x000F8044
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

	// Token: 0x060022F4 RID: 8948 RVA: 0x000469AC File Offset: 0x00044BAC
	public override void Tick()
	{
		base.Tick();
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.UpdateBattleState();
		}
		this.ActivateDefaultSlingShot();
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000F9F28 File Offset: 0x000F8128
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

	// Token: 0x060022F6 RID: 8950 RVA: 0x000469CC File Offset: 0x00044BCC
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

	// Token: 0x060022F7 RID: 8951 RVA: 0x000F9FE4 File Offset: 0x000F81E4
	public bool PlayerInHitCooldown(NetPlayer player)
	{
		float num;
		return this.playerHitTimes.TryGetValue(player.ActorNumber, out num) && num + this.hitCooldown > Time.time;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000FA018 File Offset: 0x000F8218
	public bool PlayerInStunCooldown(NetPlayer player)
	{
		float num;
		return this.playerStunTimes.TryGetValue(player.ActorNumber, out num) && num + this.hitCooldown + this.stunGracePeriod > Time.time;
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000469F4 File Offset: 0x00044BF4
	public GorillaPaintbrawlManager.PaintbrawlStatus GetPlayerStatus(NetPlayer player)
	{
		if (this.playerStatusDict.TryGetValue(player.ActorNumber, out this.tempStatus))
		{
			return this.tempStatus;
		}
		return GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x00046A17 File Offset: 0x00044C17
	public bool OnRedTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return this.HasFlag(status, GorillaPaintbrawlManager.PaintbrawlStatus.RedTeam);
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000FA054 File Offset: 0x000F8254
	public bool OnRedTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnRedTeam(playerStatus);
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x00046A21 File Offset: 0x00044C21
	public bool OnBlueTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return this.HasFlag(status, GorillaPaintbrawlManager.PaintbrawlStatus.BlueTeam);
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000FA070 File Offset: 0x000F8270
	public bool OnBlueTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnBlueTeam(playerStatus);
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x00046A2B File Offset: 0x00044C2B
	public bool OnNoTeam(GorillaPaintbrawlManager.PaintbrawlStatus status)
	{
		return !this.OnRedTeam(status) && !this.OnBlueTeam(status);
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000FA08C File Offset: 0x000F828C
	public bool OnNoTeam(NetPlayer player)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(player);
		return this.OnNoTeam(playerStatus);
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		return false;
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000FA0A8 File Offset: 0x000F82A8
	public bool OnSameTeam(GorillaPaintbrawlManager.PaintbrawlStatus playerA, GorillaPaintbrawlManager.PaintbrawlStatus playerB)
	{
		bool flag = this.OnRedTeam(playerA) && this.OnRedTeam(playerB);
		bool flag2 = this.OnBlueTeam(playerA) && this.OnBlueTeam(playerB);
		return flag || flag2;
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000FA0E0 File Offset: 0x000F82E0
	public bool OnSameTeam(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus = this.GetPlayerStatus(myPlayer);
		GorillaPaintbrawlManager.PaintbrawlStatus playerStatus2 = this.GetPlayerStatus(otherPlayer);
		return this.OnSameTeam(playerStatus, playerStatus2);
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000FA108 File Offset: 0x000F8308
	public bool LocalCanHit(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		bool flag = !this.OnSameTeam(myPlayer, otherPlayer);
		bool flag2 = this.GetPlayerLives(otherPlayer) != 0;
		return flag && flag2;
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x000FA130 File Offset: 0x000F8330
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

	// Token: 0x06002305 RID: 8965 RVA: 0x000FA1EC File Offset: 0x000F83EC
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

	// Token: 0x06002306 RID: 8966 RVA: 0x00046A42 File Offset: 0x00044C42
	private GorillaPaintbrawlManager.PaintbrawlStatus SetFlag(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return currState | flag;
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x00046A47 File Offset: 0x00044C47
	private GorillaPaintbrawlManager.PaintbrawlStatus SetFlagExclusive(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return flag;
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x00046A4A File Offset: 0x00044C4A
	private GorillaPaintbrawlManager.PaintbrawlStatus ClearFlag(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return currState & ~flag;
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x0004689C File Offset: 0x00044A9C
	private bool FlagIsSet(GorillaPaintbrawlManager.PaintbrawlStatus currState, GorillaPaintbrawlManager.PaintbrawlStatus flag)
	{
		return (currState & flag) > GorillaPaintbrawlManager.PaintbrawlStatus.None;
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000FA2D4 File Offset: 0x000F84D4
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

	// Token: 0x0600230B RID: 8971 RVA: 0x000FA3A0 File Offset: 0x000F85A0
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

	// Token: 0x0600230C RID: 8972 RVA: 0x000FA4C4 File Offset: 0x000F86C4
	private void InitializePlayerStatus()
	{
		this.keyValuePairsStatus = this.playerStatusDict.ToArray<KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus>>();
		foreach (KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus> keyValuePair in this.keyValuePairsStatus)
		{
			this.playerStatusDict[keyValuePair.Key] = GorillaPaintbrawlManager.PaintbrawlStatus.Normal;
		}
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000FA514 File Offset: 0x000F8714
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

	// Token: 0x04002674 RID: 9844
	private float playerMin = 2f;

	// Token: 0x04002675 RID: 9845
	public float tagCoolDown = 5f;

	// Token: 0x04002676 RID: 9846
	public Dictionary<int, int> playerLives = new Dictionary<int, int>();

	// Token: 0x04002677 RID: 9847
	public Dictionary<int, GorillaPaintbrawlManager.PaintbrawlStatus> playerStatusDict = new Dictionary<int, GorillaPaintbrawlManager.PaintbrawlStatus>();

	// Token: 0x04002678 RID: 9848
	public Dictionary<int, float> playerHitTimes = new Dictionary<int, float>();

	// Token: 0x04002679 RID: 9849
	public Dictionary<int, float> playerStunTimes = new Dictionary<int, float>();

	// Token: 0x0400267A RID: 9850
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

	// Token: 0x0400267B RID: 9851
	public int[] playerLivesArray = new int[10];

	// Token: 0x0400267C RID: 9852
	public GorillaPaintbrawlManager.PaintbrawlStatus[] playerStatusArray = new GorillaPaintbrawlManager.PaintbrawlStatus[10];

	// Token: 0x0400267D RID: 9853
	public bool teamBattle = true;

	// Token: 0x0400267E RID: 9854
	public int countDownTime;

	// Token: 0x0400267F RID: 9855
	private float timeBattleEnded;

	// Token: 0x04002680 RID: 9856
	public float hitCooldown = 3f;

	// Token: 0x04002681 RID: 9857
	public float stunGracePeriod = 2f;

	// Token: 0x04002682 RID: 9858
	public object objRef;

	// Token: 0x04002683 RID: 9859
	private bool playerInList;

	// Token: 0x04002684 RID: 9860
	private bool coroutineRunning;

	// Token: 0x04002685 RID: 9861
	private int lives;

	// Token: 0x04002686 RID: 9862
	private int outLives;

	// Token: 0x04002687 RID: 9863
	private int bcount;

	// Token: 0x04002688 RID: 9864
	private int rcount;

	// Token: 0x04002689 RID: 9865
	private int randInt;

	// Token: 0x0400268A RID: 9866
	private float outHitTime;

	// Token: 0x0400268B RID: 9867
	private NetworkView tempView;

	// Token: 0x0400268C RID: 9868
	private KeyValuePair<int, int>[] keyValuePairs;

	// Token: 0x0400268D RID: 9869
	private KeyValuePair<int, GorillaPaintbrawlManager.PaintbrawlStatus>[] keyValuePairsStatus;

	// Token: 0x0400268E RID: 9870
	private GorillaPaintbrawlManager.PaintbrawlStatus tempStatus;

	// Token: 0x0400268F RID: 9871
	private GorillaPaintbrawlManager.PaintbrawlState currentState;

	// Token: 0x02000588 RID: 1416
	public enum PaintbrawlStatus
	{
		// Token: 0x04002691 RID: 9873
		RedTeam = 1,
		// Token: 0x04002692 RID: 9874
		BlueTeam,
		// Token: 0x04002693 RID: 9875
		Normal = 4,
		// Token: 0x04002694 RID: 9876
		Hit = 8,
		// Token: 0x04002695 RID: 9877
		Stunned = 16,
		// Token: 0x04002696 RID: 9878
		Grace = 32,
		// Token: 0x04002697 RID: 9879
		Eliminated = 64,
		// Token: 0x04002698 RID: 9880
		None = 0
	}

	// Token: 0x02000589 RID: 1417
	public enum PaintbrawlState
	{
		// Token: 0x0400269A RID: 9882
		NotEnoughPlayers,
		// Token: 0x0400269B RID: 9883
		GameEnd,
		// Token: 0x0400269C RID: 9884
		GameEndWaiting,
		// Token: 0x0400269D RID: 9885
		StartCountdown,
		// Token: 0x0400269E RID: 9886
		CountingDownToStart,
		// Token: 0x0400269F RID: 9887
		GameStart,
		// Token: 0x040026A0 RID: 9888
		GameRunning
	}
}
