using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
public class GorillaTagManager : GorillaGameManager
{
	// Token: 0x060023EB RID: 9195 RVA: 0x001003DC File Offset: 0x000FE5DC
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

	// Token: 0x060023EC RID: 9196 RVA: 0x000484B5 File Offset: 0x000466B5
	public override void StopPlaying()
	{
		base.StopPlaying();
		base.StopAllCoroutines();
		this.lastTaggedActorNr.Clear();
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x0010048C File Offset: 0x000FE68C
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

	// Token: 0x060023EE RID: 9198 RVA: 0x00100508 File Offset: 0x000FE708
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
				int index = UnityEngine.Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.ChangeCurrentIt(GorillaGameModes.GameMode.ParticipatingPlayers[index], false);
				return;
			}
			if (this.isCurrentlyTag && GorillaGameModes.GameMode.ParticipatingPlayers.Count >= this.infectedModeThreshold)
			{
				this.SetisCurrentlyTag(false);
				this.ClearInfectionState();
				int index2 = UnityEngine.Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.AddInfectedPlayer(GorillaGameModes.GameMode.ParticipatingPlayers[index2], true);
				this.lastInfectedPlayer = GorillaGameModes.GameMode.ParticipatingPlayers[index2];
				return;
			}
			if (!this.isCurrentlyTag && GorillaGameModes.GameMode.ParticipatingPlayers.Count < this.infectedModeThreshold)
			{
				this.ClearInfectionState();
				this.lastInfectedPlayer = null;
				this.SetisCurrentlyTag(true);
				int index3 = UnityEngine.Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.ChangeCurrentIt(GorillaGameModes.GameMode.ParticipatingPlayers[index3], false);
				return;
			}
			if (!this.isCurrentlyTag && this.currentInfected.Count == 0)
			{
				int index4 = UnityEngine.Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
				this.AddInfectedPlayer(GorillaGameModes.GameMode.ParticipatingPlayers[index4], true);
				return;
			}
			if (!this.isCurrentlyTag)
			{
				this.UpdateInfectionState();
			}
		}
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000484CE File Offset: 0x000466CE
	public override void InfrequentUpdate()
	{
		base.InfrequentUpdate();
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.UpdateState();
		}
		this.inspectorLocalPlayerSpeed = this.LocalPlayerSpeed();
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000484F4 File Offset: 0x000466F4
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
				int index = UnityEngine.Random.Range(0, participatingPlayers.Count);
				int num = 0;
				while (num < 10 && participatingPlayers[index] == this.lastInfectedPlayer)
				{
					index = UnityEngine.Random.Range(0, participatingPlayers.Count);
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

	// Token: 0x060023F1 RID: 9201 RVA: 0x00100678 File Offset: 0x000FE878
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

	// Token: 0x060023F2 RID: 9202 RVA: 0x0010070C File Offset: 0x000FE90C
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

	// Token: 0x060023F3 RID: 9203 RVA: 0x00100790 File Offset: 0x000FE990
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

	// Token: 0x060023F4 RID: 9204 RVA: 0x00048503 File Offset: 0x00046703
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		if (this.isCurrentlyTag)
		{
			return myPlayer == this.currentIt && myPlayer != otherPlayer;
		}
		return this.currentInfected.Contains(myPlayer) && !this.currentInfected.Contains(otherPlayer);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x0010082C File Offset: 0x000FEA2C
	public override void LocalTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer, bool bodyHit, bool leftHand)
	{
		if (this.LocalCanTag(NetworkSystem.Instance.LocalPlayer, taggedPlayer) && (double)Time.time > this.lastQuestTagTime + (double)this.tagCoolDown)
		{
			PlayerGameEvents.MiscEvent("GameModeTag");
			this.lastQuestTagTime = (double)Time.time;
			if (!this.isCurrentlyTag)
			{
				PlayerGameEvents.GameModeObjectiveTriggered();
			}
		}
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x00100888 File Offset: 0x000FEA88
	protected float InterpolatedInfectedJumpMultiplier(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.fastJumpMultiplier;
		}
		return (this.fastJumpMultiplier - this.slowJumpMultiplier) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - infectedCount) + this.slowJumpMultiplier;
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x001008DC File Offset: 0x000FEADC
	protected float InterpolatedInfectedJumpSpeed(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.fastJumpLimit;
		}
		return (this.fastJumpLimit - this.slowJumpLimit) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - infectedCount) + this.slowJumpLimit;
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x00100930 File Offset: 0x000FEB30
	protected float InterpolatedNoobJumpMultiplier(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.slowJumpMultiplier;
		}
		return (this.fastJumpMultiplier - this.slowJumpMultiplier) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(infectedCount - 1) * 0.9f + this.slowJumpMultiplier;
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x00100980 File Offset: 0x000FEB80
	protected float InterpolatedNoobJumpSpeed(int infectedCount)
	{
		if (GorillaGameModes.GameMode.ParticipatingPlayers.Count < 2)
		{
			return this.slowJumpLimit;
		}
		return (this.fastJumpLimit - this.fastJumpLimit) / (float)(GorillaGameModes.GameMode.ParticipatingPlayers.Count - 1) * (float)(infectedCount - 1) * 0.9f + this.slowJumpLimit;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x001009D0 File Offset: 0x000FEBD0
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

	// Token: 0x060023FB RID: 9211 RVA: 0x00100B38 File Offset: 0x000FED38
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

	// Token: 0x060023FC RID: 9212 RVA: 0x00100BD8 File Offset: 0x000FEDD8
	public override bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		if (this.isCurrentlyTag)
		{
			return this.currentIt != player && thisFrame;
		}
		return !this.waitingToStartNextInfectionGame && (double)Time.time >= this.timeInfectedGameEnded + (double)(2f * this.tagCoolDown) && !this.currentInfected.Contains(player);
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x0004853F File Offset: 0x0004673F
	public bool IsInfected(NetPlayer player)
	{
		if (this.isCurrentlyTag)
		{
			return this.currentIt == player;
		}
		return this.currentInfected.Contains(player);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x0004855F File Offset: 0x0004675F
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

	// Token: 0x060023FF RID: 9215 RVA: 0x00100C34 File Offset: 0x000FEE34
	public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (this.isCurrentlyTag && ((otherPlayer != null && otherPlayer == this.currentIt) || this.currentIt.ActorNumber == otherPlayer.ActorNumber))
			{
				if (GorillaGameModes.GameMode.ParticipatingPlayers.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, GorillaGameModes.GameMode.ParticipatingPlayers.Count);
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

	// Token: 0x06002400 RID: 9216 RVA: 0x00100CF4 File Offset: 0x000FEEF4
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

	// Token: 0x06002401 RID: 9217 RVA: 0x00100DD4 File Offset: 0x000FEFD4
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

	// Token: 0x06002402 RID: 9218 RVA: 0x0004859D File Offset: 0x0004679D
	public void ChangeCurrentIt(NetPlayer newCurrentIt, bool withTagFreeze = true)
	{
		this.lastTag = (double)Time.time;
		this.currentIt = newCurrentIt;
		this.UpdateTagState(withTagFreeze);
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x000485B9 File Offset: 0x000467B9
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

	// Token: 0x06002404 RID: 9220 RVA: 0x000485DA File Offset: 0x000467DA
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

	// Token: 0x06002405 RID: 9221 RVA: 0x0004861A File Offset: 0x0004681A
	public void ClearInfectionState()
	{
		this.currentInfected.Clear();
		this.waitingToStartNextInfectionGame = false;
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x0004862E File Offset: 0x0004682E
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitched(newMasterClient);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.CopyRoomDataToLocalData();
			this.UpdateState();
		}
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x0004864F File Offset: 0x0004684F
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

	// Token: 0x06002408 RID: 9224 RVA: 0x00100E5C File Offset: 0x000FF05C
	public override void OnSerializeRead(object newData)
	{
		TagData tagData = (TagData)newData;
		this.isCurrentlyTag = tagData.isCurrentlyTag;
		this.tempItInt = tagData.currentItID;
		this.currentIt = ((this.tempItInt != -1) ? NetworkSystem.Instance.GetPlayer(this.tempItInt) : null);
		tagData.infectedPlayerList.CopyTo(this.currentInfectedArray, true);
		this.CopyInfectedArrayToList();
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x00100ECC File Offset: 0x000FF0CC
	public override object OnSerializeWrite()
	{
		this.CopyInfectedListToArray();
		TagData tagData = default(TagData);
		tagData.isCurrentlyTag = this.isCurrentlyTag;
		tagData.currentItID = ((this.currentIt != null) ? this.currentIt.ActorNumber : -1);
		tagData.infectedPlayerList.CopyFrom(this.currentInfectedArray, 0, this.currentInfectedArray.Length);
		return tagData;
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x00100F3C File Offset: 0x000FF13C
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

	// Token: 0x0600240B RID: 9227 RVA: 0x00101048 File Offset: 0x000FF248
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

	// Token: 0x0600240C RID: 9228 RVA: 0x00039846 File Offset: 0x00037A46
	public override GameModeType GameType()
	{
		return GameModeType.Infection;
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x0004868C File Offset: 0x0004688C
	public override void AddFusionDataBehaviour(NetworkObject netObject)
	{
		netObject.AddBehaviour<TagGameModeData>();
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x00048695 File Offset: 0x00046895
	public override string GameModeName()
	{
		return "INFECTION";
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x0004869C File Offset: 0x0004689C
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

	// Token: 0x06002410 RID: 9232 RVA: 0x00101178 File Offset: 0x000FF378
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

	// Token: 0x040027C8 RID: 10184
	public float tagCoolDown = 5f;

	// Token: 0x040027C9 RID: 10185
	public int infectedModeThreshold = 4;

	// Token: 0x040027CA RID: 10186
	public const byte ReportTagEvent = 1;

	// Token: 0x040027CB RID: 10187
	public const byte ReportInfectionTagEvent = 2;

	// Token: 0x040027CC RID: 10188
	public List<NetPlayer> currentInfected = new List<NetPlayer>(10);

	// Token: 0x040027CD RID: 10189
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

	// Token: 0x040027CE RID: 10190
	public NetPlayer currentIt;

	// Token: 0x040027CF RID: 10191
	public NetPlayer lastInfectedPlayer;

	// Token: 0x040027D0 RID: 10192
	public double lastTag;

	// Token: 0x040027D1 RID: 10193
	public double timeInfectedGameEnded;

	// Token: 0x040027D2 RID: 10194
	public bool waitingToStartNextInfectionGame;

	// Token: 0x040027D3 RID: 10195
	public bool isCurrentlyTag;

	// Token: 0x040027D4 RID: 10196
	private int tempItInt;

	// Token: 0x040027D5 RID: 10197
	private int iterator1;

	// Token: 0x040027D6 RID: 10198
	private NetPlayer tempPlayer;

	// Token: 0x040027D7 RID: 10199
	private bool allInfected;

	// Token: 0x040027D8 RID: 10200
	public float[] inspectorLocalPlayerSpeed;

	// Token: 0x040027D9 RID: 10201
	private protected VRRig taggingRig;

	// Token: 0x040027DA RID: 10202
	private protected VRRig taggedRig;

	// Token: 0x040027DB RID: 10203
	private NetPlayer lastTaggedPlayer;

	// Token: 0x040027DC RID: 10204
	private double lastQuestTagTime;
}
