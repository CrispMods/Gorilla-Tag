﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaGameModes;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009E4 RID: 2532
	public class GorillaFreezeTagManager : GorillaTagManager
	{
		// Token: 0x06003F19 RID: 16153 RVA: 0x000514ED File Offset: 0x0004F6ED
		public override GameModeType GameType()
		{
			return GameModeType.FreezeTag;
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x000591DC File Offset: 0x000573DC
		public override string GameModeName()
		{
			return "FREEZE TAG";
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x000591E3 File Offset: 0x000573E3
		public override void Awake()
		{
			base.Awake();
			this.fastJumpLimitCached = this.fastJumpLimit;
			this.fastJumpMultiplierCached = this.fastJumpMultiplier;
			this.slowJumpLimitCached = this.slowJumpLimit;
			this.slowJumpMultiplierCached = this.slowJumpMultiplier;
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x001676A0 File Offset: 0x001658A0
		public override void UpdateState()
		{
			if (NetworkSystem.Instance.IsMasterClient)
			{
				foreach (KeyValuePair<NetPlayer, float> keyValuePair in this.currentFrozen.ToList<KeyValuePair<NetPlayer, float>>())
				{
					if (Time.time - keyValuePair.Value >= this.freezeDuration)
					{
						this.currentFrozen.Remove(keyValuePair.Key);
						this.AddInfectedPlayer(keyValuePair.Key, false);
						RoomSystem.SendSoundEffectAll(11, 0.25f, false);
					}
				}
				if (GameMode.ParticipatingPlayers.Count < 1)
				{
					this.Reset();
					base.SetisCurrentlyTag(true);
					return;
				}
				if (this.isCurrentlyTag && this.currentIt == null)
				{
					int index = UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count);
					base.ChangeCurrentIt(GameMode.ParticipatingPlayers[index], false);
				}
				else if (this.isCurrentlyTag && GameMode.ParticipatingPlayers.Count >= this.infectedModeThreshold)
				{
					this.Reset();
					int index2 = UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count);
					this.AddInfectedPlayer(GameMode.ParticipatingPlayers[index2], true);
				}
				else if (!this.isCurrentlyTag && GameMode.ParticipatingPlayers.Count < this.infectedModeThreshold)
				{
					this.Reset();
					base.SetisCurrentlyTag(true);
					int index3 = UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count);
					base.ChangeCurrentIt(GameMode.ParticipatingPlayers[index3], false);
				}
				else if (!this.isCurrentlyTag && this.currentInfected.Count == 0)
				{
					int index4 = UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count);
					this.AddInfectedPlayer(GameMode.ParticipatingPlayers[index4], true);
				}
				bool flag = true;
				foreach (NetPlayer player in GameMode.ParticipatingPlayers)
				{
					if (!this.IsFrozen(player) && !base.IsInfected(player))
					{
						flag = false;
						break;
					}
				}
				if (flag && !this.isCurrentlyTag)
				{
					base.EndInfectionGame();
				}
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0005921B File Offset: 0x0005741B
		public override void Tick()
		{
			base.Tick();
			if (this.localVRRig)
			{
				this.localVRRig.IsFrozen = this.IsFrozen(NetworkSystem.Instance.LocalPlayer);
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x001678D0 File Offset: 0x00165AD0
		public override void StartPlaying()
		{
			base.StartPlaying();
			this.localVRRig = this.FindPlayerVRRig(NetworkSystem.Instance.LocalPlayer);
			if (NetworkSystem.Instance.IsMasterClient)
			{
				foreach (NetPlayer netPlayer in this.lastRoundInfectedPlayers.ToArray())
				{
					if (netPlayer != null && !netPlayer.InRoom)
					{
						this.lastRoundInfectedPlayers.Remove(netPlayer);
					}
				}
				foreach (NetPlayer netPlayer2 in this.currentRoundInfectedPlayers.ToArray())
				{
					if (netPlayer2 != null && !netPlayer2.InRoom)
					{
						this.currentRoundInfectedPlayers.Remove(netPlayer2);
					}
				}
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x00167974 File Offset: 0x00165B74
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
				Debug.LogWarning("Report TAG - tagged " + this.taggedRig.playerNameVisible + ", tagging " + this.taggingRig.playerNameVisible);
				if (this.isCurrentlyTag)
				{
					if (taggingPlayer == this.currentIt && taggingPlayer != taggedPlayer && (double)Time.time > this.lastTag + (double)this.tagCoolDown)
					{
						base.AddLastTagged(taggedPlayer, taggingPlayer);
						base.ChangeCurrentIt(taggedPlayer, false);
						this.lastTag = (double)Time.time;
						return;
					}
				}
				else if (this.currentInfected.Contains(taggingPlayer) && !this.currentInfected.Contains(taggedPlayer) && !this.currentFrozen.ContainsKey(taggedPlayer) && (double)Time.time > this.lastTag + (double)this.tagCoolDown)
				{
					if (!this.taggingRig.IsPositionInRange(this.taggedRig.transform.position, 6f) && !this.taggingRig.CheckTagDistanceRollback(this.taggedRig, 6f, 0.2f))
					{
						GorillaNot.instance.SendReport("extremely far tag", taggingPlayer.UserId, taggingPlayer.NickName);
						return;
					}
					base.AddLastTagged(taggedPlayer, taggingPlayer);
					this.AddFrozenPlayer(taggedPlayer);
					return;
				}
				else if (!this.currentInfected.Contains(taggingPlayer) && !this.currentInfected.Contains(taggedPlayer) && this.currentFrozen.ContainsKey(taggedPlayer) && (double)Time.time > this.lastTag + (double)this.tagCoolDown)
				{
					if (!this.taggingRig.IsPositionInRange(this.taggedRig.transform.position, 6f) && !this.taggingRig.CheckTagDistanceRollback(this.taggedRig, 6f, 0.2f))
					{
						GorillaNot.instance.SendReport("extremely far tag", taggingPlayer.UserId, taggingPlayer.NickName);
						return;
					}
					this.UnfreezePlayer(taggedPlayer);
				}
			}
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x00167BA4 File Offset: 0x00165DA4
		public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
		{
			if (this.isCurrentlyTag)
			{
				return myPlayer == this.currentIt && myPlayer != otherPlayer;
			}
			return (this.currentInfected.Contains(myPlayer) && !this.currentFrozen.ContainsKey(otherPlayer) && !this.currentInfected.Contains(otherPlayer)) || (!this.currentInfected.Contains(myPlayer) && !this.currentFrozen.ContainsKey(myPlayer) && (this.currentInfected.Contains(otherPlayer) || this.currentFrozen.ContainsKey(otherPlayer)));
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0005924B File Offset: 0x0005744B
		public override void NewVRRig(NetPlayer player, int vrrigPhotonViewID, bool didTutorial)
		{
			if (NetworkSystem.Instance.IsMasterClient)
			{
				GameMode.RefreshPlayers();
				if (!this.isCurrentlyTag && !base.IsInfected(player))
				{
					this.AddInfectedPlayer(player, true);
					this.currentRoundInfectedPlayers.Add(player);
				}
				base.UpdateInfectionState();
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00059289 File Offset: 0x00057489
		protected override IEnumerator InfectionEnd()
		{
			while ((double)Time.time < this.timeInfectedGameEnded + (double)this.tagCoolDown)
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (!this.isCurrentlyTag && this.waitingToStartNextInfectionGame)
			{
				base.ClearInfectionState();
				this.currentFrozen.Clear();
				GameMode.RefreshPlayers();
				this.lastRoundInfectedPlayers.Clear();
				this.lastRoundInfectedPlayers.AddRange(this.currentRoundInfectedPlayers);
				this.currentRoundInfectedPlayers.Clear();
				List<NetPlayer> participatingPlayers = GameMode.ParticipatingPlayers;
				int num = 0;
				if (participatingPlayers.Count > 0 && participatingPlayers.Count < this.infectMorePlayerLowerThreshold)
				{
					num = 1;
				}
				else if (participatingPlayers.Count >= this.infectMorePlayerLowerThreshold && participatingPlayers.Count < this.infectMorePlayerUpperThreshold)
				{
					num = 2;
				}
				else if (participatingPlayers.Count >= this.infectMorePlayerUpperThreshold)
				{
					num = 3;
				}
				for (int i = 0; i < num; i++)
				{
					this.TryAddNewInfectedPlayer();
				}
				this.lastTag = (double)Time.time;
			}
			yield return null;
			yield break;
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00059298 File Offset: 0x00057498
		public override void Reset()
		{
			base.Reset();
			this.currentFrozen.Clear();
			this.currentRoundInfectedPlayers.Clear();
			this.lastRoundInfectedPlayers.Clear();
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x000485DA File Offset: 0x000467DA
		private new void AddInfectedPlayer(NetPlayer infectedPlayer, bool withTagStop = true)
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
				base.UpdateInfectionState();
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x00167C34 File Offset: 0x00165E34
		private void TryAddNewInfectedPlayer()
		{
			List<NetPlayer> participatingPlayers = GameMode.ParticipatingPlayers;
			int index = UnityEngine.Random.Range(0, participatingPlayers.Count);
			int num = 0;
			while (num < 10 && this.lastRoundInfectedPlayers.Contains(participatingPlayers[index]))
			{
				index = UnityEngine.Random.Range(0, participatingPlayers.Count);
				num++;
			}
			this.AddInfectedPlayer(participatingPlayers[index], true);
			this.currentRoundInfectedPlayers.Add(participatingPlayers[index]);
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x000592C1 File Offset: 0x000574C1
		public override int MyMatIndex(NetPlayer forPlayer)
		{
			if (this.isCurrentlyTag && forPlayer == this.currentIt)
			{
				return 14;
			}
			if (this.currentInfected.Contains(forPlayer))
			{
				return 14;
			}
			return 0;
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00167CA4 File Offset: 0x00165EA4
		public override void UpdatePlayerAppearance(VRRig rig)
		{
			NetPlayer netPlayer = rig.isOfflineVRRig ? NetworkSystem.Instance.LocalPlayer : rig.creator;
			rig.UpdateFrozenEffect(this.IsFrozen(netPlayer));
			int materialIndex = this.MyMatIndex(netPlayer);
			rig.ChangeMaterialLocal(materialIndex);
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x000592E9 File Offset: 0x000574E9
		private void UnfreezePlayer(NetPlayer taggedPlayer)
		{
			if (NetworkSystem.Instance.IsMasterClient && this.currentFrozen.ContainsKey(taggedPlayer))
			{
				this.currentFrozen.Remove(taggedPlayer);
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.UnTagged, taggedPlayer);
				RoomSystem.SendSoundEffectAll(10, 0.25f, true);
			}
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x00167CE8 File Offset: 0x00165EE8
		private void AddFrozenPlayer(NetPlayer taggedPlayer)
		{
			if (NetworkSystem.Instance.IsMasterClient && !this.currentFrozen.ContainsKey(taggedPlayer))
			{
				this.currentFrozen.Add(taggedPlayer, Time.time);
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.FrozenTime, taggedPlayer);
				RoomSystem.SendSoundEffectAll(9, 0.25f, false);
				RoomSystem.SendSoundEffectToPlayer(12, 0.05f, taggedPlayer, false);
			}
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00059326 File Offset: 0x00057526
		public bool IsFrozen(NetPlayer player)
		{
			return this.currentFrozen.ContainsKey(player);
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00167D44 File Offset: 0x00165F44
		public override float[] LocalPlayerSpeed()
		{
			this.fastJumpLimit = this.fastJumpLimitCached;
			this.fastJumpMultiplier = this.fastJumpMultiplierCached;
			this.slowJumpLimit = this.slowJumpLimitCached;
			this.slowJumpMultiplier = this.slowJumpMultiplierCached;
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
				if (!this.currentInfected.Contains(NetworkSystem.Instance.LocalPlayer) && !this.currentFrozen.ContainsKey(NetworkSystem.Instance.LocalPlayer))
				{
					this.playerSpeed[0] = base.InterpolatedNoobJumpSpeed(this.currentInfected.Count);
					this.playerSpeed[1] = base.InterpolatedNoobJumpMultiplier(this.currentInfected.Count);
					return this.playerSpeed;
				}
				if (this.currentFrozen.ContainsKey(NetworkSystem.Instance.LocalPlayer))
				{
					this.fastJumpLimit = this.frozenPlayerFastJumpLimit;
					this.fastJumpMultiplier = this.frozenPlayerFastJumpMultiplier;
					this.slowJumpLimit = this.frozenPlayerSlowJumpLimit;
					this.slowJumpMultiplier = this.frozenPlayerSlowJumpMultiplier;
				}
				this.playerSpeed[0] = base.InterpolatedInfectedJumpSpeed(this.currentInfected.Count);
				this.playerSpeed[1] = base.InterpolatedInfectedJumpMultiplier(this.currentInfected.Count);
				return this.playerSpeed;
			}
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00167EC8 File Offset: 0x001660C8
		public int GetFrozenHandTapAudioIndex()
		{
			int num = UnityEngine.Random.Range(0, this.frozenHandTapIndices.Length);
			return this.frozenHandTapIndices[num];
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00167EEC File Offset: 0x001660EC
		public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
		{
			base.OnPlayerLeftRoom(otherPlayer);
			if (NetworkSystem.Instance.IsMasterClient)
			{
				if (this.isCurrentlyTag && ((otherPlayer != null && otherPlayer == this.currentIt) || this.currentIt.ActorNumber == otherPlayer.ActorNumber) && GameMode.ParticipatingPlayers.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, GameMode.ParticipatingPlayers.Count);
					base.ChangeCurrentIt(GameMode.ParticipatingPlayers[index], false);
				}
				if (this.currentInfected.Contains(otherPlayer))
				{
					this.currentInfected.Remove(otherPlayer);
				}
				if (this.currentFrozen.ContainsKey(otherPlayer))
				{
					this.currentFrozen.Remove(otherPlayer);
				}
				this.UpdateState();
			}
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00167FA4 File Offset: 0x001661A4
		public override void StopPlaying()
		{
			base.StopPlaying();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				vrrig.ForceResetFrozenEffect();
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00168000 File Offset: 0x00166200
		public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
		{
			base.OnSerializeRead(stream, info);
			this.currentFrozen.Clear();
			int num = (int)stream.ReceiveNext();
			for (int i = 0; i < num; i++)
			{
				int playerID = (int)stream.ReceiveNext();
				float value = (float)stream.ReceiveNext();
				NetPlayer player = NetworkSystem.Instance.GetPlayer(playerID);
				this.currentFrozen.Add(player, value);
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x0016806C File Offset: 0x0016626C
		public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
		{
			base.OnSerializeWrite(stream, info);
			stream.SendNext(this.currentFrozen.Count);
			foreach (KeyValuePair<NetPlayer, float> keyValuePair in this.currentFrozen)
			{
				stream.SendNext(keyValuePair.Key.ActorNumber);
				stream.SendNext(keyValuePair.Value);
			}
		}

		// Token: 0x0400402A RID: 16426
		public Dictionary<NetPlayer, float> currentFrozen = new Dictionary<NetPlayer, float>(10);

		// Token: 0x0400402B RID: 16427
		public float freezeDuration;

		// Token: 0x0400402C RID: 16428
		public int infectMorePlayerLowerThreshold = 6;

		// Token: 0x0400402D RID: 16429
		public int infectMorePlayerUpperThreshold = 10;

		// Token: 0x0400402E RID: 16430
		[Space]
		[Header("Frozen player jump settings")]
		public float frozenPlayerFastJumpLimit;

		// Token: 0x0400402F RID: 16431
		public float frozenPlayerFastJumpMultiplier;

		// Token: 0x04004030 RID: 16432
		public float frozenPlayerSlowJumpLimit;

		// Token: 0x04004031 RID: 16433
		public float frozenPlayerSlowJumpMultiplier;

		// Token: 0x04004032 RID: 16434
		[GorillaSoundLookup]
		public int[] frozenHandTapIndices;

		// Token: 0x04004033 RID: 16435
		private float fastJumpLimitCached;

		// Token: 0x04004034 RID: 16436
		private float fastJumpMultiplierCached;

		// Token: 0x04004035 RID: 16437
		private float slowJumpLimitCached;

		// Token: 0x04004036 RID: 16438
		private float slowJumpMultiplierCached;

		// Token: 0x04004037 RID: 16439
		private VRRig localVRRig;

		// Token: 0x04004038 RID: 16440
		private int hapticStrength;

		// Token: 0x04004039 RID: 16441
		private List<NetPlayer> currentRoundInfectedPlayers = new List<NetPlayer>(10);

		// Token: 0x0400403A RID: 16442
		private List<NetPlayer> lastRoundInfectedPlayers = new List<NetPlayer>(10);
	}
}
