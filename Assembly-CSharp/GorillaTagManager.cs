using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000597 RID: 1431
public class GorillaTagManager : GorillaGameManager
{
	// Token: 0x0600238B RID: 9099 RVA: 0x000B1178 File Offset: 0x000AF378
	public override void StartPlaying()
	{
		base.StartPlaying();
		if (NetworkSystem.Instance.IsMasterClient)
		{
			for (int i = 0; i < this.currentInfected.Count; i++)
			{
				this.tempPlayer = this.currentInfected[i];
				if (this.tempPlayer == null || !this.tempPlayer.InRoom())
				{
					this.currentInfected.RemoveAt(i);
					i--;
				}
			}
			if (this.currentIt != null && !this.currentIt.InRoom())
			{
				this.currentIt = null;
			}
			if (this.lastInfectedPlayer != null && !this.lastInfectedPlayer.InRoom())
			{
				this.lastInfectedPlayer = null;
			}
			this.UpdateState();
		}
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000B1225 File Offset: 0x000AF425
	public override void StopPlaying()
	{
		base.StopPlaying();
		base.StopAllCoroutines();
		this.lastTaggedActorNr.Clear();
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000B1240 File Offset: 0x000AF440
	public override void Reset()
	{
		base.Reset();
		for (int i = 0; i < this.currentInfectedArray.Length; i++)
		{
			this.currentInfectedArray[i] = -1;
		}
		this.currentInfected.Clear();
		this.lastTag = 0.0;
		this.timeInfectedGameEnded = 0.0;
		this.allInfected = false;
		this.isCurrentlyTag = false;
		this.waitingToStartNextInfectionGame = false;
		this.currentIt = null;
		this.lastInfectedPlayer = null;
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000B12BC File Offset: 0x000AF4BC
	public virtual void UpdateState()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 1)
			{
				this.isCurrentlyTag = true;
				this.ClearInfectionState();
				this.lastInfectedPlayer = null;
				this.currentIt = null;
				return;
			}
			if (this.isCurrentlyTag && this.currentIt == null)
			{
				int index = Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.ChangeCurrentIt(GorillaGameModes.GameMode.ParticipatingPlayers[index], false);
				return;
			}
			if (this.isCurrentlyTag && GorillaGameModes.GameMode.ParticipatingPlayers.Count >= this.infectedModeThreshold)
			{
				this.SetisCurrentlyTag(false);
				this.ClearInfectionState();
				int index2 = Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.AddInfectedPlayer(GorillaGameModes.GameMode.ParticipatingPlayers[index2], true);
				this.lastInfectedPlayer = GorillaGameModes.GameMode.ParticipatingPlayers[index2];
				return;
			}
			if (!this.isCurrentlyTag && GorillaGameModes.GameMode.ParticipatingPlayers.Count < this.infectedModeThreshold)
			{
				this.ClearInfectionState();
				this.lastInfectedPlayer = null;
				this.SetisCurrentlyTag(true);
				int index3 = Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.ChangeCurrentIt(GorillaGameModes.GameMode.ParticipatingPlayers[index3], false);
				return;
			}
			if (!this.isCurrentlyTag && this.currentInfected.Count == 0)
			{
				int index4 = Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.AddInfectedPlayer(GorillaGameModes.GameMode.ParticipatingPlayers[index4], true);
				return;
			}
			if (!this.isCurrentlyTag)
			{
				this.UpdateInfectionState();
			}
		}
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000B142A File Offset: 0x000AF62A
	public override void InfrequentUpdate()
	{
		base.InfrequentUpdate();
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.UpdateState();
		}
		this.inspectorLocalPlayerSpeed = this.LocalPlayerSpeed();
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000B1450 File Offset: 0x000AF650
	protected virtual IEnumerator InfectionEnd()
	{
		while ((double)Time.time < this.timeInfectedGameEnded + (double)this.tagCoolDown)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (!this.isCurrentlyTag && this.waitingToStartNextInfectionGame)
		{
			this.ClearInfectionState();
			GorillaGameModes.GameMode.RefreshPlayers();
			List<NetPlayer> participatingPlayers = GorillaGameModes.GameMode.ParticipatingPlayers;
			if (participatingPlayers.Count > 0)
			{
				int index = Random.Range(0, participatingPlayers.Count);
				int num = 0;
				while (num < 10 && participatingPlayers[index] == this.lastInfectedPlayer)
				{
					index = Random.Range(0, participatingPlayers.Count);
					num++;
				}
				this.AddInfectedPlayer(participatingPlayers[index], true);
				this.lastInfectedPlayer = participatingPlayers[index];
				this.lastTag = (double)Time.time;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000B1460 File Offset: 0x000AF660
	public void UpdateInfectionState()
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		this.allInfected = true;
		foreach (NetPlayer item in GorillaGameModes.GameMode.ParticipatingPlayers)
		{
			if (!this.currentInfected.Contains(item))
			{
				this.allInfected = false;
				break;
			}
		}
		if (!this.isCurrentlyTag && !this.waitingToStartNextInfectionGame && this.allInfected)
		{
			this.EndInfectionGame();
		}
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000B14F4 File Offset: 0x000AF6F4
	public void UpdateTagState(bool withTagFreeze = true)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		foreach (NetPlayer netPlayer in GorillaGameModes.GameMode.ParticipatingPlayers)
		{
			if (this.currentIt == netPlayer)
			{
				if (withTagFreeze)
				{
					RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.TaggedTime, netPlayer);
				}
				else
				{
					RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.JoinedTaggedTime, netPlayer);
				}
				RoomSystem.SendSoundEffectOnOther(0, 0.25f, netPlayer, false);
				break;
			}
		}
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000B1578 File Offset: 0x000AF778
	protected void EndInfectionGame()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			foreach (NetPlayer player in GorillaGameModes.GameMode.ParticipatingPlayers)
			{
				RoomSystem.SendSoundEffectToPlayer(2, 0.25f, player, true);
			}
			PlayerGameEvents.GameModeCompleteRound();
			GorillaGameModes.GameMode.BroadcastRoundComplete();
			this.lastTaggedActorNr.Clear();
			this.waitingToStartNextInfectionGame = true;
			this.timeInfectedGameEnded = (double)Time.time;
			base.StartCoroutine(this.InfectionEnd());
		}
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000B1614 File Offset: 0x000AF814
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		if (this.isCurrentlyTag)
		{
			return myPlayer == this.currentIt && myPlayer != otherPlayer;
		}
		return this.currentInfected.Contains(myPlayer) && !this.currentInfected.Contains(otherPlayer);
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000B1650 File Offset: 0x000AF850
	public override void LocalTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer, bool bodyHit, bool leftHand)
	{
		if (this.lastTaggedPlayer != taggedPlayer)
		{
			this.lastTaggedPlayer = taggedPlayer;
			PlayerGameEvents.MiscEvent("GameModeTag");
			if (!this.isCurrentlyTag)
			{
				PlayerGameEvents.GameModeObjectiveTriggered();
			}
		}
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000B167C File Offset: 0x000AF87C
	protected float InterpolatedInfectedJumpMultiplier(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.fastJumpMultiplier;
		}
		return (this.fastJumpMultiplier - this.slowJumpMultiplier) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - infectedCount) + this.slowJumpMultiplier;
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000B16D0 File Offset: 0x000AF8D0
	protected float InterpolatedInfectedJumpSpeed(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.fastJumpLimit;
		}
		return (this.fastJumpLimit - this.slowJumpLimit) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - infectedCount) + this.slowJumpLimit;
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000B1724 File Offset: 0x000AF924
	protected float InterpolatedNoobJumpMultiplier(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.slowJumpMultiplier;
		}
		return (this.fastJumpMultiplier - this.slowJumpMultiplier) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(infectedCount - 1) * 0.9f + this.slowJumpMultiplier;
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000B1774 File Offset: 0x000AF974
	protected float InterpolatedNoobJumpSpeed(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.slowJumpLimit;
		}
		return (this.fastJumpLimit - this.fastJumpLimit) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(infectedCount - 1) * 0.9f + this.slowJumpLimit;
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000B17C4 File Offset: 0x000AF9C4
	public override void ReportTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.taggingRig = this.FindPlayerVRRig(taggingPlayer);
			this.taggedRig = this.FindPlayerVRRig(taggedPlayer);
			if (this.taggingRig == null || this.taggedRig == null)
			{
				return;
			}
			this.taggedRig.SetTaggedBy(this.taggingRig);
			if (this.isCurrentlyTag)
			{
				if (taggingPlayer == this.currentIt && taggingPlayer != taggedPlayer && (double)Time.time > this.lastTag + (double)this.tagCoolDown)
				{
					base.AddLastTagged(taggedPlayer, taggingPlayer);
					this.ChangeCurrentIt(taggedPlayer, true);
					this.lastTag = (double)Time.time;
					return;
				}
			}
			else if (this.currentInfected.Contains(taggingPlayer) && !this.currentInfected.Contains(taggedPlayer) && (double)Time.time > this.lastTag + (double)this.tagCoolDown)
			{
				if (!this.taggingRig.IsPositionInRange(this.taggedRig.transform.position, 6f) && !this.taggingRig.CheckTagDistanceRollback(this.taggedRig, 6f, 0.2f))
				{
					GorillaNot.instance.SendReport("extremely far tag", taggingPlayer.UserId, taggingPlayer.NickName);
					return;
				}
				base.AddLastTagged(taggedPlayer, taggingPlayer);
				this.AddInfectedPlayer(taggedPlayer, true);
				int count = this.currentInfected.Count;
			}
		}
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000B192C File Offset: 0x000AFB2C
	public override void HitPlayer(NetPlayer taggedPlayer)
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.taggedRig = this.FindPlayerVRRig(taggedPlayer);
			if (this.taggedRig == null || this.waitingToStartNextInfectionGame || (double)Time.time < this.timeInfectedGameEnded + (double)(2f * this.tagCoolDown))
			{
				return;
			}
			if (this.isCurrentlyTag)
			{
				base.AddLastTagged(taggedPlayer, taggedPlayer);
				this.ChangeCurrentIt(taggedPlayer, false);
				return;
			}
			if (!this.currentInfected.Contains(taggedPlayer))
			{
				base.AddLastTagged(taggedPlayer, taggedPlayer);
				this.AddInfectedPlayer(taggedPlayer, false);
				int count = this.currentInfected.Count;
			}
		}
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000B19CC File Offset: 0x000AFBCC
	public override bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		if (this.isCurrentlyTag)
		{
			return this.currentIt != player && thisFrame;
		}
		return !this.waitingToStartNextInfectionGame && (double)Time.time >= this.timeInfectedGameEnded + (double)(2f * this.tagCoolDown) && !this.currentInfected.Contains(player);
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000B1A25 File Offset: 0x000AFC25
	public bool IsInfected(NetPlayer player)
	{
		if (this.isCurrentlyTag)
		{
			return this.currentIt == player;
		}
		return this.currentInfected.Contains(player);
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000B1A45 File Offset: 0x000AFC45
	public override void NewVRRig(NetPlayer player, int vrrigPhotonViewID, bool didTutorial)
	{
		base.NewVRRig(player, vrrigPhotonViewID, didTutorial);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			bool flag = this.isCurrentlyTag;
			this.UpdateState();
			if (!flag && !this.isCurrentlyTag)
			{
				if (didTutorial)
				{
					this.AddInfectedPlayer(player, false);
				}
				this.UpdateInfectionState();
			}
		}
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000B1A84 File Offset: 0x000AFC84
	public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (this.isCurrentlyTag && ((otherPlayer != null && otherPlayer == this.currentIt) || this.currentIt.ActorNumber == otherPlayer.ActorNumber))
			{
				if (GorillaGameModes.GameMode.ParticipatingPlayers.Count > 0)
				{
					int index = Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
					this.ChangeCurrentIt(GorillaGameModes.GameMode.ParticipatingPlayers[index], false);
				}
			}
			else if (!this.isCurrentlyTag && GorillaGameModes.GameMode.ParticipatingPlayers.Count >= this.infectedModeThreshold)
			{
				while (this.currentInfected.Contains(otherPlayer))
				{
					this.currentInfected.Remove(otherPlayer);
				}
				this.UpdateInfectionState();
			}
			this.UpdateState();
		}
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000B1B44 File Offset: 0x000AFD44
	private void CopyInfectedListToArray()
	{
		this.iterator1 = 0;
		while (this.iterator1 < this.currentInfectedArray.Length)
		{
			this.currentInfectedArray[this.iterator1] = -1;
			this.iterator1++;
		}
		this.iterator1 = this.currentInfected.Count - 1;
		while (this.iterator1 >= 0)
		{
			if (this.currentInfected[this.iterator1] == null)
			{
				this.currentInfected.RemoveAt(this.iterator1);
			}
			this.iterator1--;
		}
		this.iterator1 = 0;
		while (this.iterator1 < this.currentInfected.Count)
		{
			this.currentInfectedArray[this.iterator1] = this.currentInfected[this.iterator1].ActorNumber;
			this.iterator1++;
		}
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000B1C24 File Offset: 0x000AFE24
	private void CopyInfectedArrayToList()
	{
		this.currentInfected.Clear();
		this.iterator1 = 0;
		while (this.iterator1 < this.currentInfectedArray.Length)
		{
			if (this.currentInfectedArray[this.iterator1] != -1)
			{
				this.tempPlayer = NetworkSystem.Instance.GetPlayer(this.currentInfectedArray[this.iterator1]);
				if (this.tempPlayer != null)
				{
					this.currentInfected.Add(this.tempPlayer);
				}
			}
			this.iterator1++;
		}
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000B1CA9 File Offset: 0x000AFEA9
	public void ChangeCurrentIt(NetPlayer newCurrentIt, bool withTagFreeze = true)
	{
		this.lastTag = (double)Time.time;
		this.currentIt = newCurrentIt;
		this.UpdateTagState(withTagFreeze);
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000B1CC5 File Offset: 0x000AFEC5
	public void SetisCurrentlyTag(bool newTagSetting)
	{
		if (newTagSetting)
		{
			this.isCurrentlyTag = true;
		}
		else
		{
			this.isCurrentlyTag = false;
		}
		RoomSystem.SendSoundEffectAll(2, 0.25f, false);
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000B1CE6 File Offset: 0x000AFEE6
	public void AddInfectedPlayer(NetPlayer infectedPlayer, bool withTagStop = true)
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.currentInfected.Add(infectedPlayer);
			if (!withTagStop)
			{
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.JoinedTaggedTime, infectedPlayer);
			}
			else
			{
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.TaggedTime, infectedPlayer);
			}
			RoomSystem.SendSoundEffectOnOther(0, 0.25f, infectedPlayer, false);
			this.UpdateInfectionState();
		}
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000B1D26 File Offset: 0x000AFF26
	public void ClearInfectionState()
	{
		this.currentInfected.Clear();
		this.waitingToStartNextInfectionGame = false;
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000B1D3A File Offset: 0x000AFF3A
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitched(newMasterClient);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.CopyRoomDataToLocalData();
			this.UpdateState();
		}
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000B1D5B File Offset: 0x000AFF5B
	public void CopyRoomDataToLocalData()
	{
		this.lastTag = 0.0;
		this.timeInfectedGameEnded = 0.0;
		this.waitingToStartNextInfectionGame = false;
		if (this.isCurrentlyTag)
		{
			this.UpdateTagState(true);
			return;
		}
		this.UpdateInfectionState();
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000B1D98 File Offset: 0x000AFF98
	public override void OnSerializeRead(object newData)
	{
		TagData tagData = (TagData)newData;
		this.isCurrentlyTag = tagData.isCurrentlyTag;
		this.tempItInt = tagData.currentItID;
		this.currentIt = ((this.tempItInt != -1) ? NetworkSystem.Instance.GetPlayer(this.tempItInt) : null);
		tagData.infectedPlayerList.CopyTo(this.currentInfectedArray, true);
		this.CopyInfectedArrayToList();
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000B1E08 File Offset: 0x000B0008
	public override object OnSerializeWrite()
	{
		this.CopyInfectedListToArray();
		TagData tagData = default(TagData);
		tagData.isCurrentlyTag = this.isCurrentlyTag;
		tagData.currentItID = ((this.currentIt != null) ? this.currentIt.ActorNumber : -1);
		tagData.infectedPlayerList.CopyFrom(this.currentInfectedArray, 0, this.currentInfectedArray.Length);
		return tagData;
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000B1E78 File Offset: 0x000B0078
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
		this.CopyInfectedListToArray();
		stream.SendNext(this.isCurrentlyTag);
		stream.SendNext((this.currentIt != null) ? this.currentIt.ActorNumber : -1);
		stream.SendNext(this.currentInfectedArray[0]);
		stream.SendNext(this.currentInfectedArray[1]);
		stream.SendNext(this.currentInfectedArray[2]);
		stream.SendNext(this.currentInfectedArray[3]);
		stream.SendNext(this.currentInfectedArray[4]);
		stream.SendNext(this.currentInfectedArray[5]);
		stream.SendNext(this.currentInfectedArray[6]);
		stream.SendNext(this.currentInfectedArray[7]);
		stream.SendNext(this.currentInfectedArray[8]);
		stream.SendNext(this.currentInfectedArray[9]);
		base.WriteLastTagged(stream);
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000B1F84 File Offset: 0x000B0184
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
		NetworkSystem.Instance.GetPlayer(info.Sender);
		this.isCurrentlyTag = (bool)stream.ReceiveNext();
		this.tempItInt = (int)stream.ReceiveNext();
		this.currentIt = ((this.tempItInt != -1) ? NetworkSystem.Instance.GetPlayer(this.tempItInt) : null);
		this.currentInfectedArray[0] = (int)stream.ReceiveNext();
		this.currentInfectedArray[1] = (int)stream.ReceiveNext();
		this.currentInfectedArray[2] = (int)stream.ReceiveNext();
		this.currentInfectedArray[3] = (int)stream.ReceiveNext();
		this.currentInfectedArray[4] = (int)stream.ReceiveNext();
		this.currentInfectedArray[5] = (int)stream.ReceiveNext();
		this.currentInfectedArray[6] = (int)stream.ReceiveNext();
		this.currentInfectedArray[7] = (int)stream.ReceiveNext();
		this.currentInfectedArray[8] = (int)stream.ReceiveNext();
		this.currentInfectedArray[9] = (int)stream.ReceiveNext();
		base.ReadLastTagged(stream);
		this.CopyInfectedArrayToList();
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x000444E2 File Offset: 0x000426E2
	public override GameModeType GameType()
	{
		return GameModeType.Infection;
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x000B20B2 File Offset: 0x000B02B2
	public override void AddFusionDataBehaviour(NetworkObject netObject)
	{
		netObject.AddBehaviour<TagGameModeData>();
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x000B20BB File Offset: 0x000B02BB
	public override string GameModeName()
	{
		return "INFECTION";
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x000B20C2 File Offset: 0x000B02C2
	public override int MyMatIndex(NetPlayer forPlayer)
	{
		if (this.isCurrentlyTag && forPlayer == this.currentIt)
		{
			return 1;
		}
		if (this.currentInfected.Contains(forPlayer))
		{
			return 2;
		}
		return 0;
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x000B20E8 File Offset: 0x000B02E8
	public override float[] LocalPlayerSpeed()
	{
		if (this.isCurrentlyTag)
		{
			if (NetworkSystem.Instance.LocalPlayer == this.currentIt)
			{
				this.playerSpeed[0] = this.fastJumpLimit;
				this.playerSpeed[1] = this.fastJumpMultiplier;
				return this.playerSpeed;
			}
			this.playerSpeed[0] = this.slowJumpLimit;
			this.playerSpeed[1] = this.slowJumpMultiplier;
			return this.playerSpeed;
		}
		else
		{
			if (this.currentInfected.Contains(NetworkSystem.Instance.LocalPlayer))
			{
				this.playerSpeed[0] = this.InterpolatedInfectedJumpSpeed(this.currentInfected.Count);
				this.playerSpeed[1] = this.InterpolatedInfectedJumpMultiplier(this.currentInfected.Count);
				return this.playerSpeed;
			}
			this.playerSpeed[0] = this.InterpolatedNoobJumpSpeed(this.currentInfected.Count);
			this.playerSpeed[1] = this.InterpolatedNoobJumpMultiplier(this.currentInfected.Count);
			return this.playerSpeed;
		}
	}

	// Token: 0x0400276D RID: 10093
	public float tagCoolDown = 5f;

	// Token: 0x0400276E RID: 10094
	public int infectedModeThreshold = 4;

	// Token: 0x0400276F RID: 10095
	public const byte ReportTagEvent = 1;

	// Token: 0x04002770 RID: 10096
	public const byte ReportInfectionTagEvent = 2;

	// Token: 0x04002771 RID: 10097
	public List<NetPlayer> currentInfected = new List<NetPlayer>(10);

	// Token: 0x04002772 RID: 10098
	public int[] currentInfectedArray = new int[]
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

	// Token: 0x04002773 RID: 10099
	public NetPlayer currentIt;

	// Token: 0x04002774 RID: 10100
	public NetPlayer lastInfectedPlayer;

	// Token: 0x04002775 RID: 10101
	public double lastTag;

	// Token: 0x04002776 RID: 10102
	public double timeInfectedGameEnded;

	// Token: 0x04002777 RID: 10103
	public bool waitingToStartNextInfectionGame;

	// Token: 0x04002778 RID: 10104
	public bool isCurrentlyTag;

	// Token: 0x04002779 RID: 10105
	private int tempItInt;

	// Token: 0x0400277A RID: 10106
	private int iterator1;

	// Token: 0x0400277B RID: 10107
	private NetPlayer tempPlayer;

	// Token: 0x0400277C RID: 10108
	private bool allInfected;

	// Token: 0x0400277D RID: 10109
	public float[] inspectorLocalPlayerSpeed;

	// Token: 0x0400277E RID: 10110
	private protected VRRig taggingRig;

	// Token: 0x0400277F RID: 10111
	private protected VRRig taggedRig;

	// Token: 0x04002780 RID: 10112
	private NetPlayer lastTaggedPlayer;
}
