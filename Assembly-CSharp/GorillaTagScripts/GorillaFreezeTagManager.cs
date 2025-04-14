using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GorillaGameModes;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BE RID: 2494
	public class GorillaFreezeTagManager : GorillaTagManager
	{
		// Token: 0x06003E01 RID: 15873 RVA: 0x000EEB6B File Offset: 0x000ECD6B
		public override GameModeType GameType()
		{
			return GameModeType.FreezeTag;
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x00125DDE File Offset: 0x00123FDE
		public override string GameModeName()
		{
			return "FREEZE TAG";
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x00125DE5 File Offset: 0x00123FE5
		public override void Awake()
		{
			base.Awake();
			this.fastJumpLimitCached = this.fastJumpLimit;
			this.fastJumpMultiplierCached = this.fastJumpMultiplier;
			this.slowJumpLimitCached = this.slowJumpLimit;
			this.slowJumpMultiplierCached = this.slowJumpMultiplier;
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x00125E20 File Offset: 0x00124020
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
					int index = Random.Range(0, GameMode.ParticipatingPlayers.Count);
					base.ChangeCurrentIt(GameMode.ParticipatingPlayers[index], false);
				}
				else if (this.isCurrentlyTag && GameMode.ParticipatingPlayers.Count >= this.infectedModeThreshold)
				{
					this.Reset();
					int index2 = Random.Range(0, GameMode.ParticipatingPlayers.Count);
					this.AddInfectedPlayer(GameMode.ParticipatingPlayers[index2], true);
				}
				else if (!this.isCurrentlyTag && GameMode.ParticipatingPlayers.Count < this.infectedModeThreshold)
				{
					this.Reset();
					base.SetisCurrentlyTag(true);
					int index3 = Random.Range(0, GameMode.ParticipatingPlayers.Count);
					base.ChangeCurrentIt(GameMode.ParticipatingPlayers[index3], false);
				}
				else if (!this.isCurrentlyTag && this.currentInfected.Count == 0)
				{
					int index4 = Random.Range(0, GameMode.ParticipatingPlayers.Count);
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

		// Token: 0x06003E05 RID: 15877 RVA: 0x00126050 File Offset: 0x00124250
		public override void Tick()
		{
			base.Tick();
			if (this.localVRRig)
			{
				this.localVRRig.IsFrozen = this.IsFrozen(NetworkSystem.Instance.LocalPlayer);
			}
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x00126080 File Offset: 0x00124280
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

		// Token: 0x06003E07 RID: 15879 RVA: 0x00126124 File Offset: 0x00124324
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

		// Token: 0x06003E08 RID: 15880 RVA: 0x00126354 File Offset: 0x00124554
		public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
		{
			if (this.isCurrentlyTag)
			{
				return myPlayer == this.currentIt && myPlayer != otherPlayer;
			}
			return (this.currentInfected.Contains(myPlayer) && !this.currentFrozen.ContainsKey(otherPlayer) && !this.currentInfected.Contains(otherPlayer)) || (!this.currentInfected.Contains(myPlayer) && !this.currentFrozen.ContainsKey(myPlayer) && (this.currentInfected.Contains(otherPlayer) || this.currentFrozen.ContainsKey(otherPlayer)));
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x001263E3 File Offset: 0x001245E3
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

		// Token: 0x06003E0A RID: 15882 RVA: 0x00126421 File Offset: 0x00124621
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

		// Token: 0x06003E0B RID: 15883 RVA: 0x00126430 File Offset: 0x00124630
		public override void Reset()
		{
			base.Reset();
			this.currentFrozen.Clear();
			this.currentRoundInfectedPlayers.Clear();
			this.lastRoundInfectedPlayers.Clear();
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x000B1CE6 File Offset: 0x000AFEE6
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

		// Token: 0x06003E0D RID: 15885 RVA: 0x0012645C File Offset: 0x0012465C
		private void TryAddNewInfectedPlayer()
		{
			List<NetPlayer> participatingPlayers = GameMode.ParticipatingPlayers;
			int index = Random.Range(0, participatingPlayers.Count);
			int num = 0;
			while (num < 10 && this.lastRoundInfectedPlayers.Contains(participatingPlayers[index]))
			{
				index = Random.Range(0, participatingPlayers.Count);
				num++;
			}
			this.AddInfectedPlayer(participatingPlayers[index], true);
			this.currentRoundInfectedPlayers.Add(participatingPlayers[index]);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x001264CA File Offset: 0x001246CA
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

		// Token: 0x06003E0F RID: 15887 RVA: 0x001264F4 File Offset: 0x001246F4
		public override void UpdatePlayerAppearance(VRRig rig)
		{
			NetPlayer netPlayer = rig.isOfflineVRRig ? NetworkSystem.Instance.LocalPlayer : rig.creator;
			rig.UpdateFrozenEffect(this.IsFrozen(netPlayer));
			int materialIndex = this.MyMatIndex(netPlayer);
			rig.ChangeMaterialLocal(materialIndex);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00126538 File Offset: 0x00124738
		private void UnfreezePlayer(NetPlayer taggedPlayer)
		{
			if (NetworkSystem.Instance.IsMasterClient && this.currentFrozen.ContainsKey(taggedPlayer))
			{
				this.currentFrozen.Remove(taggedPlayer);
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.UnTagged, taggedPlayer);
				RoomSystem.SendSoundEffectAll(10, 0.25f, true);
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x00126578 File Offset: 0x00124778
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

		// Token: 0x06003E12 RID: 15890 RVA: 0x001265D2 File Offset: 0x001247D2
		public bool IsFrozen(NetPlayer player)
		{
			return this.currentFrozen.ContainsKey(player);
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x001265E0 File Offset: 0x001247E0
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

		// Token: 0x06003E14 RID: 15892 RVA: 0x00126764 File Offset: 0x00124964
		public int GetFrozenHandTapAudioIndex()
		{
			int num = Random.Range(0, this.frozenHandTapIndices.Length);
			return this.frozenHandTapIndices[num];
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00126788 File Offset: 0x00124988
		public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
		{
			base.OnPlayerLeftRoom(otherPlayer);
			if (NetworkSystem.Instance.IsMasterClient)
			{
				if (this.isCurrentlyTag && ((otherPlayer != null && otherPlayer == this.currentIt) || this.currentIt.ActorNumber == otherPlayer.ActorNumber) && GameMode.ParticipatingPlayers.Count > 0)
				{
					int index = Random.Range(0, GameMode.ParticipatingPlayers.Count);
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

		// Token: 0x06003E16 RID: 15894 RVA: 0x00126840 File Offset: 0x00124A40
		public override void StopPlaying()
		{
			base.StopPlaying();
			foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
			{
				vrrig.ForceResetFrozenEffect();
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x0012689C File Offset: 0x00124A9C
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

		// Token: 0x06003E18 RID: 15896 RVA: 0x00126908 File Offset: 0x00124B08
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

		// Token: 0x04003F50 RID: 16208
		public Dictionary<NetPlayer, float> currentFrozen = new Dictionary<NetPlayer, float>(10);

		// Token: 0x04003F51 RID: 16209
		public float freezeDuration;

		// Token: 0x04003F52 RID: 16210
		public int infectMorePlayerLowerThreshold = 6;

		// Token: 0x04003F53 RID: 16211
		public int infectMorePlayerUpperThreshold = 10;

		// Token: 0x04003F54 RID: 16212
		[Space]
		[Header("Frozen player jump settings")]
		public float frozenPlayerFastJumpLimit;

		// Token: 0x04003F55 RID: 16213
		public float frozenPlayerFastJumpMultiplier;

		// Token: 0x04003F56 RID: 16214
		public float frozenPlayerSlowJumpLimit;

		// Token: 0x04003F57 RID: 16215
		public float frozenPlayerSlowJumpMultiplier;

		// Token: 0x04003F58 RID: 16216
		[GorillaSoundLookup]
		public int[] frozenHandTapIndices;

		// Token: 0x04003F59 RID: 16217
		private float fastJumpLimitCached;

		// Token: 0x04003F5A RID: 16218
		private float fastJumpMultiplierCached;

		// Token: 0x04003F5B RID: 16219
		private float slowJumpLimitCached;

		// Token: 0x04003F5C RID: 16220
		private float slowJumpMultiplierCached;

		// Token: 0x04003F5D RID: 16221
		private VRRig localVRRig;

		// Token: 0x04003F5E RID: 16222
		private int hapticStrength;

		// Token: 0x04003F5F RID: 16223
		private List<NetPlayer> currentRoundInfectedPlayers = new List<NetPlayer>(10);

		// Token: 0x04003F60 RID: 16224
		private List<NetPlayer> lastRoundInfectedPlayers = new List<NetPlayer>(10);
	}
}
