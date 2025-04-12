using System;
using System.Collections.Generic;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000560 RID: 1376
public class GorillaGuardianZoneManager : MonoBehaviourPunCallbacks, IPunObservable, IGorillaSliceableSimple
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x060021E3 RID: 8675 RVA: 0x00046089 File Offset: 0x00044289
	public NetPlayer CurrentGuardian
	{
		get
		{
			return this.guardianPlayer;
		}
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000F4C80 File Offset: 0x000F2E80
	public void Awake()
	{
		GorillaGuardianZoneManager.zoneManagers.Add(this);
		this.idol.gameObject.SetActive(false);
		foreach (Transform transform in this.idolPositions)
		{
			transform.gameObject.SetActive(false);
		}
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null && gorillaGuardianManager.isPlaying && PhotonNetwork.IsMasterClient)
		{
			this.StartPlaying();
		}
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x00046091 File Offset: 0x00044291
	private void Start()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000460B9 File Offset: 0x000442B9
	public void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		GorillaGuardianZoneManager.zoneManagers.Remove(this);
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000460ED File Offset: 0x000442ED
	public override void OnEnable()
	{
		base.OnEnable();
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000460FC File Offset: 0x000442FC
	public override void OnDisable()
	{
		base.OnDisable();
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000F4D18 File Offset: 0x000F2F18
	public void SliceUpdate()
	{
		float idolActivationDisplay = this._idolActivationDisplay;
		float num = 0f;
		if (this._currentActivationTime < 0f)
		{
			this._idolActivationDisplay = 0f;
			this._progressing = false;
		}
		else
		{
			num = Mathf.Min(Time.time - this._lastTappedTime, this.activationTimePerTap);
			this._progressing = (num < this.activationTimePerTap);
			this._idolActivationDisplay = (this._currentActivationTime + num) / this.requiredActivationTime;
		}
		if (idolActivationDisplay != this._idolActivationDisplay)
		{
			this.idol.UpdateActivationProgress(this._currentActivationTime + num, this._progressing);
		}
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x0004610B File Offset: 0x0004430B
	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
		this.StopPlaying();
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x00046119 File Offset: 0x00044319
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (this.guardianPlayer == null || this.guardianPlayer.GetPlayerRef() == otherPlayer)
		{
			this.SetGuardian(null);
		}
		NetPlayer previousGuardian = this._previousGuardian;
		if (((previousGuardian != null) ? previousGuardian.GetPlayerRef() : null) == otherPlayer)
		{
			this._previousGuardian = null;
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000F4DB0 File Offset: 0x000F2FB0
	private void OnZoneChanged()
	{
		bool flag = ZoneManagement.IsInZone(this.zone);
		if (flag != this._zoneIsActive || !this._zoneStateChanged)
		{
			GTDev.Log<string>(string.Format("{0} {1} Active: {2}->{3}", new object[]
			{
				this,
				this.zone,
				this._zoneIsActive,
				flag
			}), this, null);
			this._zoneIsActive = flag;
			this.idol.OnZoneActiveStateChanged(this._zoneIsActive);
			this._zoneStateChanged = true;
		}
		if (!this._zoneIsActive)
		{
			return;
		}
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null && gorillaGuardianManager.isPlaying && gorillaGuardianManager.IsPlayerGuardian(NetworkSystem.Instance.LocalPlayer) && this.guardianPlayer != null && this.guardianPlayer != NetworkSystem.Instance.LocalPlayer)
		{
			gorillaGuardianManager.RequestEjectGuardian(NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000F4E94 File Offset: 0x000F3094
	public void StartPlaying()
	{
		if (!this.IsZoneValid())
		{
			return;
		}
		this._currentActivationTime = -1f;
		if (this.guardianPlayer != null && !this.guardianPlayer.InRoom())
		{
			this.SetGuardian(null);
			this._previousGuardian = null;
		}
		this.idol.gameObject.SetActive(true);
		this.SelectNextIdol();
		this.SetIdolPosition(this.currentIdol);
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000F4EFC File Offset: 0x000F30FC
	public void StopPlaying()
	{
		this._currentActivationTime = -1f;
		this.currentIdol = -1;
		this.idol.gameObject.SetActive(false);
		this._progressing = false;
		this._lastTappedTime = 0f;
		this.SetGuardian(null);
		this._previousGuardian = null;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x00046154 File Offset: 0x00044354
	public void SetScaleCenterPoint(Transform scaleCenterPoint)
	{
		this.guardianSizeChanger.SetScaleCenterPoint(scaleCenterPoint);
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x00046162 File Offset: 0x00044362
	public void IdolWasTapped(NetPlayer tapper)
	{
		if (tapper != null && (!GameMode.ParticipatingPlayers.Contains(tapper) || tapper == this.guardianPlayer))
		{
			return;
		}
		if (!this.IsZoneValid())
		{
			return;
		}
		if (this.UpdateTapCount(tapper))
		{
			this.IdolActivated(tapper);
		}
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x00046197 File Offset: 0x00044397
	public bool IsZoneValid()
	{
		return NetworkSystem.Instance.SessionIsPrivate || ZoneManagement.IsInZone(this.zone);
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000F4F4C File Offset: 0x000F314C
	private bool UpdateTapCount(NetPlayer tapper)
	{
		if (this.guardianPlayer == null && this._previousGuardian == null)
		{
			return true;
		}
		if (this._currentActivationTime < 0f)
		{
			this._currentActivationTime = 0f;
			this._lastTappedTime = Time.time;
		}
		if (!this._progressing)
		{
			float num = Mathf.Min(Time.time - this._lastTappedTime, this.activationTimePerTap);
			this._lastTappedTime = Time.time;
			if (num + this._currentActivationTime >= this.requiredActivationTime)
			{
				return true;
			}
			this._currentActivationTime += num;
		}
		return false;
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000461B2 File Offset: 0x000443B2
	private void IdolActivated(NetPlayer activater)
	{
		this._currentActivationTime = -1f;
		this.SetGuardian(activater);
		this.SelectNextIdol();
		this.MoveIdolPosition(this.currentIdol);
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000F4FDC File Offset: 0x000F31DC
	public void SetGuardian(NetPlayer newGuardian)
	{
		if (this.guardianPlayer == newGuardian)
		{
			return;
		}
		if (this.guardianPlayer != null)
		{
			if (NetworkSystem.Instance.LocalPlayer == this.guardianPlayer)
			{
				this.PlayerLostGuardianSFX.Play();
			}
			RigContainer rigContainer;
			if (VRRigCache.Instance.TryGetVrrig(this.guardianPlayer, out rigContainer))
			{
				rigContainer.Rig.EnableGuardianEjectWatch(false);
				this.guardianSizeChanger.unacceptRig(rigContainer.Rig);
				int num = RoomSystem.JoinedRoom ? rigContainer.netView.ViewID : rigContainer.CachedNetViewID;
				if (GorillaTagger.Instance.offlineVRRig.grabbedRopeIndex == num)
				{
					GorillaTagger.Instance.offlineVRRig.DroppedByPlayer(rigContainer.Rig, Vector3.zero);
					if (this.guardianPlayer == NetworkSystem.Instance.LocalPlayer)
					{
						bool forLeftHand = GorillaTagger.Instance.offlineVRRig.grabbedRopeBoneIndex == 1;
						EquipmentInteractor.instance.UpdateHandEquipment(null, forLeftHand);
					}
				}
			}
		}
		this._previousGuardian = this.guardianPlayer;
		this.guardianPlayer = newGuardian;
		if (this.guardianPlayer != null)
		{
			if (NetworkSystem.Instance.LocalPlayer == this.guardianPlayer)
			{
				this.PlayerGainGuardianSFX.Play();
			}
			else
			{
				this.ObserverGainGuardianSFX.Play();
			}
			RigContainer rigContainer2;
			if (VRRigCache.Instance.TryGetVrrig(this.guardianPlayer, out rigContainer2))
			{
				rigContainer2.Rig.EnableGuardianEjectWatch(true);
				this.guardianSizeChanger.acceptRig(rigContainer2.Rig);
			}
			PlayerGameEvents.GameModeCompleteRound();
			if (NetworkSystem.Instance.LocalPlayer == this.guardianPlayer)
			{
				PlayerGameEvents.GameModeObjectiveTriggered();
			}
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000461D9 File Offset: 0x000443D9
	public bool IsPlayerGuardian(NetPlayer player)
	{
		return player == this.guardianPlayer;
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000461E4 File Offset: 0x000443E4
	private int SelectNextIdol()
	{
		if (this.idolPositions == null || this.idolPositions.Count == 0)
		{
			GTDev.Log<string>("No Guardian Idols possible to select.", null);
			return -1;
		}
		this.currentIdol = this.SelectRandomIdol();
		return this.currentIdol;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000F5160 File Offset: 0x000F3360
	private int SelectRandomIdol()
	{
		int result;
		if (this.currentIdol != -1 && this.idolPositions.Count > 1)
		{
			result = (this.currentIdol + UnityEngine.Random.Range(1, this.idolPositions.Count)) % this.idolPositions.Count;
		}
		else
		{
			result = UnityEngine.Random.Range(0, this.idolPositions.Count);
		}
		return result;
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000F51C0 File Offset: 0x000F33C0
	private int SelectFarthestFromGuardian()
	{
		if (!(GorillaGameManager.instance is GorillaGuardianManager))
		{
			return this.SelectRandomIdol();
		}
		RigContainer rigContainer;
		if (this.guardianPlayer != null && VRRigCache.Instance.TryGetVrrig(this.guardianPlayer, out rigContainer))
		{
			Vector3 position = rigContainer.transform.position;
			int num = -1;
			float num2 = 0f;
			for (int i = 0; i < this.idolPositions.Count; i++)
			{
				float num3 = Vector3.SqrMagnitude(this.idolPositions[i].transform.position - position);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num != -1)
			{
				return num;
			}
		}
		return this.SelectRandomIdol();
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000F5268 File Offset: 0x000F3468
	private int SelectFarFromNearestPlayer()
	{
		List<Transform> list = this.SortByDistanceToNearestPlayer();
		if (list.Count > 1 && this.currentIdol >= 0 && this.currentIdol < list.Count)
		{
			list.Remove(this.idolPositions[this.currentIdol]);
		}
		int index = UnityEngine.Random.Range(list.Count / 2, list.Count);
		Transform item = list[index];
		return this.idolPositions.IndexOf(item);
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000F52DC File Offset: 0x000F34DC
	private List<Transform> SortByDistanceToNearestPlayer()
	{
		GorillaGuardianZoneManager.<>c__DisplayClass49_0 CS$<>8__locals1 = new GorillaGuardianZoneManager.<>c__DisplayClass49_0();
		CS$<>8__locals1.playerPositions = new List<Vector3>();
		foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
		{
			if (!(vrrig == null))
			{
				CS$<>8__locals1.playerPositions.Add(vrrig.transform.position);
			}
		}
		this._sortedIdolPositions.Clear();
		foreach (Transform item in this.idolPositions)
		{
			this._sortedIdolPositions.Add(item);
		}
		this._sortedIdolPositions.Sort(new Comparison<Transform>(CS$<>8__locals1.<SortByDistanceToNearestPlayer>g__CompareNearestPlayerDistance|0));
		return this._sortedIdolPositions;
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000F53D0 File Offset: 0x000F35D0
	public void TriggerIdolKnockback()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		for (int i = 0; i < RoomSystem.PlayersInRoom.Count; i++)
		{
			RigContainer rigContainer;
			if ((this.knockbackIncludesGuardian || RoomSystem.PlayersInRoom[i] != this.guardianPlayer) && VRRigCache.Instance.TryGetVrrig(RoomSystem.PlayersInRoom[i], out rigContainer))
			{
				Vector3 vector = rigContainer.Rig.transform.position - this.idol.transform.position;
				if (Vector3.SqrMagnitude(vector) < this.idolKnockbackRadius * this.idolKnockbackRadius)
				{
					Vector3 velocity = (vector - Vector3.up * Vector3.Dot(Vector3.up, vector)).normalized * this.idolKnockbackStrengthHoriz + Vector3.up * this.idolKnockbackStrengthVert;
					RoomSystem.LaunchPlayer(RoomSystem.PlayersInRoom[i], velocity);
				}
			}
		}
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x000F54CC File Offset: 0x000F36CC
	private void SetIdolPosition(int index)
	{
		if (index < 0 || index >= this.idolPositions.Count)
		{
			GTDev.Log<string>("Invalid index received", null);
			return;
		}
		this.idol.gameObject.SetActive(true);
		this.idol.SetPosition(this.idolPositions[index].position);
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x000F5524 File Offset: 0x000F3724
	private void MoveIdolPosition(int index)
	{
		if (index < 0 || index >= this.idolPositions.Count)
		{
			GTDev.Log<string>("Invalid index received", null);
			return;
		}
		this.idol.gameObject.SetActive(true);
		this.idol.MovePositions(this.idolPositions[index].position);
		if (base.photonView.IsMine)
		{
			this.idolMoveCount++;
		}
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000F5598 File Offset: 0x000F3798
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager == null || !gorillaGuardianManager.isPlaying || player != NetworkSystem.Instance.MasterClient)
		{
			return;
		}
		if (stream.IsWriting)
		{
			stream.SendNext((this.guardianPlayer != null) ? this.guardianPlayer.ActorNumber : 0);
			stream.SendNext(this._currentActivationTime);
			stream.SendNext(this.currentIdol);
			stream.SendNext(this.idolMoveCount);
			return;
		}
		int num = (int)stream.ReceiveNext();
		float num2 = (float)stream.ReceiveNext();
		int num3 = (int)stream.ReceiveNext();
		int num4 = (int)stream.ReceiveNext();
		if (float.IsNaN(num2) || float.IsInfinity(num2))
		{
			return;
		}
		this.SetGuardian((num != 0) ? NetworkSystem.Instance.GetPlayer(num) : null);
		if (num2 != this._currentActivationTime)
		{
			this._currentActivationTime = num2;
			this._lastTappedTime = Time.time;
		}
		if (num3 != this.currentIdol || num4 != this.idolMoveCount)
		{
			if (this.currentIdol == -1)
			{
				this.SetIdolPosition(num3);
			}
			else
			{
				this.MoveIdolPosition(num3);
			}
			this.currentIdol = num3;
			this.idolMoveCount = num4;
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x0400256D RID: 9581
	public static List<GorillaGuardianZoneManager> zoneManagers = new List<GorillaGuardianZoneManager>();

	// Token: 0x0400256E RID: 9582
	[SerializeField]
	private GTZone zone;

	// Token: 0x0400256F RID: 9583
	[SerializeField]
	private SizeChanger guardianSizeChanger;

	// Token: 0x04002570 RID: 9584
	[SerializeField]
	private TappableGuardianIdol idol;

	// Token: 0x04002571 RID: 9585
	[SerializeField]
	private List<Transform> idolPositions;

	// Token: 0x04002572 RID: 9586
	[Space]
	[SerializeField]
	private float requiredActivationTime = 10f;

	// Token: 0x04002573 RID: 9587
	[SerializeField]
	private float activationTimePerTap = 1f;

	// Token: 0x04002574 RID: 9588
	[Space]
	[SerializeField]
	private bool knockbackIncludesGuardian = true;

	// Token: 0x04002575 RID: 9589
	[SerializeField]
	private float idolKnockbackRadius = 6f;

	// Token: 0x04002576 RID: 9590
	[SerializeField]
	private float idolKnockbackStrengthVert = 12f;

	// Token: 0x04002577 RID: 9591
	[SerializeField]
	private float idolKnockbackStrengthHoriz = 15f;

	// Token: 0x04002578 RID: 9592
	[Space]
	[SerializeField]
	private SoundBankPlayer PlayerGainGuardianSFX;

	// Token: 0x04002579 RID: 9593
	[SerializeField]
	private SoundBankPlayer PlayerLostGuardianSFX;

	// Token: 0x0400257A RID: 9594
	[SerializeField]
	private SoundBankPlayer ObserverGainGuardianSFX;

	// Token: 0x0400257B RID: 9595
	private NetPlayer guardianPlayer;

	// Token: 0x0400257C RID: 9596
	private NetPlayer _previousGuardian;

	// Token: 0x0400257D RID: 9597
	private int currentIdol = -1;

	// Token: 0x0400257E RID: 9598
	private int idolMoveCount;

	// Token: 0x0400257F RID: 9599
	private List<Transform> _sortedIdolPositions = new List<Transform>();

	// Token: 0x04002580 RID: 9600
	private float _currentActivationTime = -1f;

	// Token: 0x04002581 RID: 9601
	private float _lastTappedTime;

	// Token: 0x04002582 RID: 9602
	private bool _progressing;

	// Token: 0x04002583 RID: 9603
	private float _idolActivationDisplay;

	// Token: 0x04002584 RID: 9604
	private bool _zoneIsActive;

	// Token: 0x04002585 RID: 9605
	private bool _zoneStateChanged;
}
