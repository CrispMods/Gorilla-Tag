using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class GorillaHuntManager : GorillaGameManager
{
	// Token: 0x06002239 RID: 8761 RVA: 0x00010ED3 File Offset: 0x0000F0D3
	public override GameModeType GameType()
	{
		return GameModeType.Hunt;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000A9987 File Offset: 0x000A7B87
	public override string GameModeName()
	{
		return "HUNT";
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x000A998E File Offset: 0x000A7B8E
	public override void AddFusionDataBehaviour(NetworkObject behaviour)
	{
		behaviour.AddBehaviour<HuntGameModeData>();
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000A9998 File Offset: 0x000A7B98
	public override void StartPlaying()
	{
		base.StartPlaying();
		GorillaTagger.Instance.offlineVRRig.huntComputer.SetActive(true);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			for (int i = 0; i < this.currentHunted.Count; i++)
			{
				this.tempPlayer = this.currentHunted[i];
				if (this.tempPlayer == null || !this.tempPlayer.InRoom())
				{
					this.currentHunted.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < this.currentTarget.Count; i++)
			{
				this.tempPlayer = this.currentTarget[i];
				if (this.tempPlayer == null || !this.tempPlayer.InRoom())
				{
					this.currentTarget.RemoveAt(i);
					i--;
				}
			}
			this.UpdateState();
		}
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000A9A6F File Offset: 0x000A7C6F
	public override void StopPlaying()
	{
		base.StopPlaying();
		GorillaTagger.Instance.offlineVRRig.huntComputer.SetActive(false);
		base.StopAllCoroutines();
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000A9A94 File Offset: 0x000A7C94
	public override void Reset()
	{
		base.Reset();
		this.currentHunted.Clear();
		this.currentTarget.Clear();
		for (int i = 0; i < this.currentHuntedArray.Length; i++)
		{
			this.currentHuntedArray[i] = -1;
			this.currentTargetArray[i] = -1;
		}
		this.huntStarted = false;
		this.waitingToStartNextHuntGame = false;
		this.inStartCountdown = false;
		this.timeHuntGameEnded = 0.0;
		this.countDownTime = 0;
		this.timeLastSlowTagged = 0f;
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000A9B18 File Offset: 0x000A7D18
	public void UpdateState()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (NetworkSystem.Instance.RoomPlayerCount <= 3)
			{
				this.CleanUpHunt();
				this.huntStarted = false;
				this.waitingToStartNextHuntGame = false;
				this.iterator1 = 0;
				while (this.iterator1 < RoomSystem.PlayersInRoom.Count)
				{
					RoomSystem.SendSoundEffectToPlayer(0, 0.25f, RoomSystem.PlayersInRoom[this.iterator1], false);
					this.iterator1++;
				}
				return;
			}
			if (NetworkSystem.Instance.RoomPlayerCount > 3 && !this.huntStarted && !this.waitingToStartNextHuntGame && !this.inStartCountdown)
			{
				Utils.Log("<color=red> there are enough players</color>", this);
				base.StartCoroutine(this.StartHuntCountdown());
				return;
			}
			this.UpdateHuntState();
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000A9BDF File Offset: 0x000A7DDF
	public void CleanUpHunt()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.currentHunted.Clear();
			this.currentTarget.Clear();
		}
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000A9C03 File Offset: 0x000A7E03
	public IEnumerator StartHuntCountdown()
	{
		if (NetworkSystem.Instance.IsMasterClient && !this.inStartCountdown)
		{
			this.inStartCountdown = true;
			this.countDownTime = 5;
			this.CleanUpHunt();
			while (this.countDownTime > 0)
			{
				yield return new WaitForSeconds(1f);
				this.countDownTime--;
			}
			this.StartHunt();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000A9C14 File Offset: 0x000A7E14
	public void StartHunt()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.huntStarted = true;
			this.waitingToStartNextHuntGame = false;
			this.countDownTime = 0;
			this.inStartCountdown = false;
			this.CleanUpHunt();
			this.iterator1 = 0;
			while (this.iterator1 < NetworkSystem.Instance.AllNetPlayers.Count<NetPlayer>())
			{
				if (this.currentTarget.Count < 10)
				{
					this.currentTarget.Add(NetworkSystem.Instance.AllNetPlayers[this.iterator1]);
					RoomSystem.SendSoundEffectToPlayer(0, 0.25f, NetworkSystem.Instance.AllNetPlayers[this.iterator1], false);
				}
				this.iterator1++;
			}
			this.RandomizePlayerList(ref this.currentTarget);
		}
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x000A9CD4 File Offset: 0x000A7ED4
	public void RandomizePlayerList(ref List<NetPlayer> listToRandomize)
	{
		for (int i = 0; i < listToRandomize.Count - 1; i++)
		{
			this.tempRandIndex = Random.Range(i, listToRandomize.Count);
			this.tempRandPlayer = listToRandomize[i];
			listToRandomize[i] = listToRandomize[this.tempRandIndex];
			listToRandomize[this.tempRandIndex] = this.tempRandPlayer;
		}
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000A9D3E File Offset: 0x000A7F3E
	public IEnumerator HuntEnd()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			while ((double)Time.time < this.timeHuntGameEnded + (double)this.tagCoolDown)
			{
				yield return new WaitForSeconds(0.1f);
			}
			if (this.waitingToStartNextHuntGame)
			{
				base.StartCoroutine(this.StartHuntCountdown());
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x000A9D50 File Offset: 0x000A7F50
	public void UpdateHuntState()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.notHuntedCount = 0;
			foreach (NetPlayer item in RoomSystem.PlayersInRoom)
			{
				if (this.currentTarget.Contains(item) && !this.currentHunted.Contains(item))
				{
					this.notHuntedCount++;
				}
			}
			if (this.notHuntedCount <= 2 && this.huntStarted)
			{
				this.EndHuntGame();
			}
		}
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000A9DF0 File Offset: 0x000A7FF0
	private void EndHuntGame()
	{
		if (NetworkSystem.Instance.IsMasterClient)
		{
			foreach (NetPlayer netPlayer in RoomSystem.PlayersInRoom)
			{
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.TaggedTime, netPlayer);
				RoomSystem.SendSoundEffectToPlayer(2, 0.25f, netPlayer, false);
			}
			this.huntStarted = false;
			this.timeHuntGameEnded = (double)Time.time;
			this.waitingToStartNextHuntGame = true;
			base.StartCoroutine(this.HuntEnd());
		}
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000A9E84 File Offset: 0x000A8084
	public override bool LocalCanTag(NetPlayer myPlayer, NetPlayer otherPlayer)
	{
		if (this.waitingToStartNextHuntGame || this.countDownTime > 0 || GorillaTagger.Instance.currentStatus == GorillaTagger.StatusEffect.Frozen)
		{
			return false;
		}
		if (this.currentHunted.Contains(myPlayer) && !this.currentHunted.Contains(otherPlayer) && Time.time > this.timeLastSlowTagged + 1f)
		{
			this.timeLastSlowTagged = Time.time;
			return true;
		}
		return this.IsTargetOf(myPlayer, otherPlayer);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000A9EFC File Offset: 0x000A80FC
	public override void ReportTag(NetPlayer taggedPlayer, NetPlayer taggingPlayer)
	{
		if (NetworkSystem.Instance.IsMasterClient && !this.waitingToStartNextHuntGame)
		{
			if ((this.currentHunted.Contains(taggingPlayer) || !this.currentTarget.Contains(taggingPlayer)) && !this.currentHunted.Contains(taggedPlayer) && this.currentTarget.Contains(taggedPlayer))
			{
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.SetSlowedTime, taggedPlayer);
				RoomSystem.SendSoundEffectOnOther(5, 0.125f, taggedPlayer, false);
				return;
			}
			if (this.IsTargetOf(taggingPlayer, taggedPlayer))
			{
				RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.TaggedTime, taggedPlayer);
				RoomSystem.SendSoundEffectOnOther(0, 0.25f, taggedPlayer, false);
				this.currentHunted.Add(taggedPlayer);
				this.UpdateHuntState();
			}
		}
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000A9FA0 File Offset: 0x000A81A0
	public bool IsTargetOf(NetPlayer huntingPlayer, NetPlayer huntedPlayer)
	{
		return !this.currentHunted.Contains(huntingPlayer) && !this.currentHunted.Contains(huntedPlayer) && this.currentTarget.Contains(huntingPlayer) && this.currentTarget.Contains(huntedPlayer) && huntedPlayer == this.GetTargetOf(huntingPlayer);
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000A9FF4 File Offset: 0x000A81F4
	public NetPlayer GetTargetOf(NetPlayer netPlayer)
	{
		if (this.currentHunted.Contains(netPlayer) || !this.currentTarget.Contains(netPlayer))
		{
			return null;
		}
		this.tempTargetIndex = this.currentTarget.IndexOf(netPlayer);
		for (int num = (this.tempTargetIndex + 1) % this.currentTarget.Count; num != this.tempTargetIndex; num = (num + 1) % this.currentTarget.Count)
		{
			if (this.currentTarget[num] == netPlayer)
			{
				return null;
			}
			if (!this.currentHunted.Contains(this.currentTarget[num]) && this.currentTarget[num] != null)
			{
				return this.currentTarget[num];
			}
		}
		return null;
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x000AA0A8 File Offset: 0x000A82A8
	public override void HitPlayer(NetPlayer taggedPlayer)
	{
		if (NetworkSystem.Instance.IsMasterClient && !this.waitingToStartNextHuntGame && !this.currentHunted.Contains(taggedPlayer) && this.currentTarget.Contains(taggedPlayer))
		{
			RoomSystem.SendStatusEffectToPlayer(RoomSystem.StatusEffects.TaggedTime, taggedPlayer);
			RoomSystem.SendSoundEffectOnOther(0, 0.25f, taggedPlayer, false);
			this.currentHunted.Add(taggedPlayer);
			this.UpdateHuntState();
		}
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000AA10B File Offset: 0x000A830B
	public override bool CanAffectPlayer(NetPlayer player, bool thisFrame)
	{
		return !this.waitingToStartNextHuntGame && !this.currentHunted.Contains(player) && this.currentTarget.Contains(player);
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000AA131 File Offset: 0x000A8331
	public override void OnPlayerEnteredRoom(NetPlayer newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		bool isMasterClient = NetworkSystem.Instance.IsMasterClient;
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000AA145 File Offset: 0x000A8345
	public override void NewVRRig(NetPlayer player, int vrrigPhotonViewID, bool didTutorial)
	{
		base.NewVRRig(player, vrrigPhotonViewID, didTutorial);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (!this.waitingToStartNextHuntGame && this.huntStarted)
			{
				this.currentHunted.Add(player);
			}
			this.UpdateState();
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000AA180 File Offset: 0x000A8380
	public override void OnPlayerLeftRoom(NetPlayer otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			if (this.currentTarget.Contains(otherPlayer))
			{
				this.currentTarget.Remove(otherPlayer);
			}
			if (this.currentHunted.Contains(otherPlayer))
			{
				this.currentHunted.Remove(otherPlayer);
			}
			this.UpdateState();
		}
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000AA1DC File Offset: 0x000A83DC
	private void CopyHuntDataListToArray()
	{
		this.copyListToArrayIndex = 0;
		while (this.copyListToArrayIndex < 10)
		{
			this.currentHuntedArray[this.copyListToArrayIndex] = -1;
			this.currentTargetArray[this.copyListToArrayIndex] = -1;
			this.copyListToArrayIndex++;
		}
		this.copyListToArrayIndex = this.currentHunted.Count - 1;
		while (this.copyListToArrayIndex >= 0)
		{
			if (this.currentHunted[this.copyListToArrayIndex] == null)
			{
				this.currentHunted.RemoveAt(this.copyListToArrayIndex);
			}
			this.copyListToArrayIndex--;
		}
		this.copyListToArrayIndex = this.currentTarget.Count - 1;
		while (this.copyListToArrayIndex >= 0)
		{
			if (this.currentTarget[this.copyListToArrayIndex] == null)
			{
				this.currentTarget.RemoveAt(this.copyListToArrayIndex);
			}
			this.copyListToArrayIndex--;
		}
		this.copyListToArrayIndex = 0;
		while (this.copyListToArrayIndex < this.currentHunted.Count)
		{
			this.currentHuntedArray[this.copyListToArrayIndex] = this.currentHunted[this.copyListToArrayIndex].ActorNumber;
			this.copyListToArrayIndex++;
		}
		this.copyListToArrayIndex = 0;
		while (this.copyListToArrayIndex < this.currentTarget.Count)
		{
			this.currentTargetArray[this.copyListToArrayIndex] = this.currentTarget[this.copyListToArrayIndex].ActorNumber;
			this.copyListToArrayIndex++;
		}
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000AA360 File Offset: 0x000A8560
	private void CopyHuntDataArrayToList()
	{
		this.currentTarget.Clear();
		this.copyArrayToListIndex = 0;
		while (this.copyArrayToListIndex < this.currentTargetArray.Length)
		{
			if (this.currentTargetArray[this.copyArrayToListIndex] != -1)
			{
				this.tempPlayer = NetworkSystem.Instance.GetPlayer(this.currentTargetArray[this.copyArrayToListIndex]);
				if (this.tempPlayer != null)
				{
					this.currentTarget.Add(this.tempPlayer);
				}
			}
			this.copyArrayToListIndex++;
		}
		this.currentHunted.Clear();
		this.copyArrayToListIndex = 0;
		while (this.copyArrayToListIndex < this.currentHuntedArray.Length)
		{
			if (this.currentHuntedArray[this.copyArrayToListIndex] != -1)
			{
				this.tempPlayer = NetworkSystem.Instance.GetPlayer(this.currentHuntedArray[this.copyArrayToListIndex]);
				if (this.tempPlayer != null)
				{
					this.currentHunted.Add(this.tempPlayer);
				}
			}
			this.copyArrayToListIndex++;
		}
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000AA45D File Offset: 0x000A865D
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitched(newMasterClient);
		if (NetworkSystem.Instance.IsMasterClient)
		{
			this.CopyRoomDataToLocalData();
			this.UpdateState();
		}
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000AA47E File Offset: 0x000A867E
	public void CopyRoomDataToLocalData()
	{
		this.waitingToStartNextHuntGame = false;
		this.UpdateHuntState();
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000AA490 File Offset: 0x000A8690
	public override void OnSerializeRead(object newData)
	{
		HuntData huntData = (HuntData)newData;
		huntData.currentHuntedArray.CopyTo(this.currentHuntedArray, true);
		huntData.currentTargetArray.CopyTo(this.currentTargetArray, true);
		this.huntStarted = huntData.huntStarted;
		this.waitingToStartNextHuntGame = huntData.waitingToStartNextHuntGame;
		this.countDownTime = huntData.countDownTime;
		this.CopyHuntDataArrayToList();
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000AA504 File Offset: 0x000A8704
	public override object OnSerializeWrite()
	{
		this.CopyHuntDataListToArray();
		HuntData huntData = default(HuntData);
		huntData.currentHuntedArray.CopyFrom(this.currentHuntedArray, 0, this.currentHuntedArray.Length);
		huntData.currentTargetArray.CopyFrom(this.currentTargetArray, 0, this.currentTargetArray.Length);
		huntData.huntStarted = this.huntStarted;
		huntData.waitingToStartNextHuntGame = this.waitingToStartNextHuntGame;
		huntData.countDownTime = this.countDownTime;
		return huntData;
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000AA594 File Offset: 0x000A8794
	public override void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
	{
		this.CopyHuntDataListToArray();
		stream.SendNext(this.currentHuntedArray[0]);
		stream.SendNext(this.currentHuntedArray[1]);
		stream.SendNext(this.currentHuntedArray[2]);
		stream.SendNext(this.currentHuntedArray[3]);
		stream.SendNext(this.currentHuntedArray[4]);
		stream.SendNext(this.currentHuntedArray[5]);
		stream.SendNext(this.currentHuntedArray[6]);
		stream.SendNext(this.currentHuntedArray[7]);
		stream.SendNext(this.currentHuntedArray[8]);
		stream.SendNext(this.currentHuntedArray[9]);
		stream.SendNext(this.currentTargetArray[0]);
		stream.SendNext(this.currentTargetArray[1]);
		stream.SendNext(this.currentTargetArray[2]);
		stream.SendNext(this.currentTargetArray[3]);
		stream.SendNext(this.currentTargetArray[4]);
		stream.SendNext(this.currentTargetArray[5]);
		stream.SendNext(this.currentTargetArray[6]);
		stream.SendNext(this.currentTargetArray[7]);
		stream.SendNext(this.currentTargetArray[8]);
		stream.SendNext(this.currentTargetArray[9]);
		stream.SendNext(this.huntStarted);
		stream.SendNext(this.waitingToStartNextHuntGame);
		stream.SendNext(this.countDownTime);
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000AA758 File Offset: 0x000A8958
	public override void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
	{
		this.currentHuntedArray[0] = (int)stream.ReceiveNext();
		this.currentHuntedArray[1] = (int)stream.ReceiveNext();
		this.currentHuntedArray[2] = (int)stream.ReceiveNext();
		this.currentHuntedArray[3] = (int)stream.ReceiveNext();
		this.currentHuntedArray[4] = (int)stream.ReceiveNext();
		this.currentHuntedArray[5] = (int)stream.ReceiveNext();
		this.currentHuntedArray[6] = (int)stream.ReceiveNext();
		this.currentHuntedArray[7] = (int)stream.ReceiveNext();
		this.currentHuntedArray[8] = (int)stream.ReceiveNext();
		this.currentHuntedArray[9] = (int)stream.ReceiveNext();
		this.currentTargetArray[0] = (int)stream.ReceiveNext();
		this.currentTargetArray[1] = (int)stream.ReceiveNext();
		this.currentTargetArray[2] = (int)stream.ReceiveNext();
		this.currentTargetArray[3] = (int)stream.ReceiveNext();
		this.currentTargetArray[4] = (int)stream.ReceiveNext();
		this.currentTargetArray[5] = (int)stream.ReceiveNext();
		this.currentTargetArray[6] = (int)stream.ReceiveNext();
		this.currentTargetArray[7] = (int)stream.ReceiveNext();
		this.currentTargetArray[8] = (int)stream.ReceiveNext();
		this.currentTargetArray[9] = (int)stream.ReceiveNext();
		this.huntStarted = (bool)stream.ReceiveNext();
		this.waitingToStartNextHuntGame = (bool)stream.ReceiveNext();
		this.countDownTime = (int)stream.ReceiveNext();
		this.CopyHuntDataArrayToList();
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000AA91C File Offset: 0x000A8B1C
	public override int MyMatIndex(NetPlayer forPlayer)
	{
		NetPlayer targetOf = this.GetTargetOf(forPlayer);
		if (this.currentHunted.Contains(forPlayer) || (this.huntStarted && targetOf == null))
		{
			return 3;
		}
		return 0;
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000AA950 File Offset: 0x000A8B50
	public override float[] LocalPlayerSpeed()
	{
		if (this.currentHunted.Contains(NetworkSystem.Instance.LocalPlayer) || (this.huntStarted && this.GetTargetOf(NetworkSystem.Instance.LocalPlayer) == null))
		{
			return new float[]
			{
				8.5f,
				1.3f
			};
		}
		if (GorillaTagger.Instance.currentStatus == GorillaTagger.StatusEffect.Slowed)
		{
			return new float[]
			{
				5.5f,
				0.9f
			};
		}
		return new float[]
		{
			6.5f,
			1.1f
		};
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000AA9DF File Offset: 0x000A8BDF
	public override void InfrequentUpdate()
	{
		base.InfrequentUpdate();
	}

	// Token: 0x040025C5 RID: 9669
	public float tagCoolDown = 5f;

	// Token: 0x040025C6 RID: 9670
	public int[] currentHuntedArray = new int[]
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

	// Token: 0x040025C7 RID: 9671
	public List<NetPlayer> currentHunted = new List<NetPlayer>(10);

	// Token: 0x040025C8 RID: 9672
	public int[] currentTargetArray = new int[]
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

	// Token: 0x040025C9 RID: 9673
	public List<NetPlayer> currentTarget = new List<NetPlayer>(10);

	// Token: 0x040025CA RID: 9674
	public bool huntStarted;

	// Token: 0x040025CB RID: 9675
	public bool waitingToStartNextHuntGame;

	// Token: 0x040025CC RID: 9676
	public bool inStartCountdown;

	// Token: 0x040025CD RID: 9677
	public int countDownTime;

	// Token: 0x040025CE RID: 9678
	public double timeHuntGameEnded;

	// Token: 0x040025CF RID: 9679
	public float timeLastSlowTagged;

	// Token: 0x040025D0 RID: 9680
	public object objRef;

	// Token: 0x040025D1 RID: 9681
	private int iterator1;

	// Token: 0x040025D2 RID: 9682
	private NetPlayer tempRandPlayer;

	// Token: 0x040025D3 RID: 9683
	private int tempRandIndex;

	// Token: 0x040025D4 RID: 9684
	private int notHuntedCount;

	// Token: 0x040025D5 RID: 9685
	private int tempTargetIndex;

	// Token: 0x040025D6 RID: 9686
	private NetPlayer tempPlayer;

	// Token: 0x040025D7 RID: 9687
	private int copyListToArrayIndex;

	// Token: 0x040025D8 RID: 9688
	private int copyArrayToListIndex;
}
