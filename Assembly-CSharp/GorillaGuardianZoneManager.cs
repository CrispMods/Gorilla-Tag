using System;
using System.Collections.Generic;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class GorillaGuardianZoneManager : MonoBehaviourPunCallbacks, IPunObservable, IGorillaSliceableSimple
{
	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06002239 RID: 8761 RVA: 0x0004742E File Offset: 0x0004562E
	public NetPlayer CurrentGuardian
	{
		get
		{
			return this.guardianPlayer;
		}
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000F79FC File Offset: 0x000F5BFC
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

	// Token: 0x0600223B RID: 8763 RVA: 0x00047436 File Offset: 0x00045636
	private void Start()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Combine(instance.onZoneChanged, new Action(this.OnZoneChanged));
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x0004745E File Offset: 0x0004565E
	public void OnDestroy()
	{
		ZoneManagement instance = ZoneManagement.instance;
		instance.onZoneChanged = (Action)Delegate.Remove(instance.onZoneChanged, new Action(this.OnZoneChanged));
		GorillaGuardianZoneManager.zoneManagers.Remove(this);
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x00047492 File Offset: 0x00045692
	public override void OnEnable()
	{
		base.OnEnable();
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000474A1 File Offset: 0x000456A1
	public override void OnDisable()
	{
		base.OnDisable();
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000F7A94 File Offset: 0x000F5C94
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

	// Token: 0x06002240 RID: 8768 RVA: 0x000474B0 File Offset: 0x000456B0
	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
		this.StopPlaying();
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000474BE File Offset: 0x000456BE
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

	// Token: 0x06002242 RID: 8770 RVA: 0x000F7B2C File Offset: 0x000F5D2C
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

	// Token: 0x06002243 RID: 8771 RVA: 0x000F7C10 File Offset: 0x000F5E10
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

	// Token: 0x06002244 RID: 8772 RVA: 0x000F7C78 File Offset: 0x000F5E78
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

	// Token: 0x06002245 RID: 8773 RVA: 0x000474F9 File Offset: 0x000456F9
	public void SetScaleCenterPoint(Transform scaleCenterPoint)
	{
		this.guardianSizeChanger.SetScaleCenterPoint(scaleCenterPoint);
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x00047507 File Offset: 0x00045707
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

	// Token: 0x06002247 RID: 8775 RVA: 0x0004753C File Offset: 0x0004573C
	public bool IsZoneValid()
	{
		return NetworkSystem.Instance.SessionIsPrivate || ZoneManagement.IsInZone(this.zone);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000F7CC8 File Offset: 0x000F5EC8
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

	// Token: 0x06002249 RID: 8777 RVA: 0x00047557 File Offset: 0x00045757
	private void IdolActivated(NetPlayer activater)
	{
		this._currentActivationTime = -1f;
		this.SetGuardian(activater);
		this.SelectNextIdol();
		this.MoveIdolPosition(this.currentIdol);
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000F7D58 File Offset: 0x000F5F58
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

	// Token: 0x0600224B RID: 8779 RVA: 0x0004757E File Offset: 0x0004577E
	public bool IsPlayerGuardian(NetPlayer player)
	{
		return player == this.guardianPlayer;
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x00047589 File Offset: 0x00045789
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

	// Token: 0x0600224D RID: 8781 RVA: 0x000F7EDC File Offset: 0x000F60DC
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

	// Token: 0x0600224E RID: 8782 RVA: 0x000F7F3C File Offset: 0x000F613C
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

	// Token: 0x0600224F RID: 8783 RVA: 0x000F7FE4 File Offset: 0x000F61E4
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

	// Token: 0x06002250 RID: 8784 RVA: 0x000F8058 File Offset: 0x000F6258
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

	// Token: 0x06002251 RID: 8785 RVA: 0x000F814C File Offset: 0x000F634C
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

	// Token: 0x06002252 RID: 8786 RVA: 0x000F8248 File Offset: 0x000F6448
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

	// Token: 0x06002253 RID: 8787 RVA: 0x000F82A0 File Offset: 0x000F64A0
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

	// Token: 0x06002254 RID: 8788 RVA: 0x000F8314 File Offset: 0x000F6514
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

	// Token: 0x06002257 RID: 8791 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040025BF RID: 9663
	public static List<GorillaGuardianZoneManager> zoneManagers = new List<GorillaGuardianZoneManager>();

	// Token: 0x040025C0 RID: 9664
	[SerializeField]
	private GTZone zone;

	// Token: 0x040025C1 RID: 9665
	[SerializeField]
	private SizeChanger guardianSizeChanger;

	// Token: 0x040025C2 RID: 9666
	[SerializeField]
	private TappableGuardianIdol idol;

	// Token: 0x040025C3 RID: 9667
	[SerializeField]
	private List<Transform> idolPositions;

	// Token: 0x040025C4 RID: 9668
	[Space]
	[SerializeField]
	private float requiredActivationTime = 10f;

	// Token: 0x040025C5 RID: 9669
	[SerializeField]
	private float activationTimePerTap = 1f;

	// Token: 0x040025C6 RID: 9670
	[Space]
	[SerializeField]
	private bool knockbackIncludesGuardian = true;

	// Token: 0x040025C7 RID: 9671
	[SerializeField]
	private float idolKnockbackRadius = 6f;

	// Token: 0x040025C8 RID: 9672
	[SerializeField]
	private float idolKnockbackStrengthVert = 12f;

	// Token: 0x040025C9 RID: 9673
	[SerializeField]
	private float idolKnockbackStrengthHoriz = 15f;

	// Token: 0x040025CA RID: 9674
	[Space]
	[SerializeField]
	private SoundBankPlayer PlayerGainGuardianSFX;

	// Token: 0x040025CB RID: 9675
	[SerializeField]
	private SoundBankPlayer PlayerLostGuardianSFX;

	// Token: 0x040025CC RID: 9676
	[SerializeField]
	private SoundBankPlayer ObserverGainGuardianSFX;

	// Token: 0x040025CD RID: 9677
	private NetPlayer guardianPlayer;

	// Token: 0x040025CE RID: 9678
	private NetPlayer _previousGuardian;

	// Token: 0x040025CF RID: 9679
	private int currentIdol = -1;

	// Token: 0x040025D0 RID: 9680
	private int idolMoveCount;

	// Token: 0x040025D1 RID: 9681
	private List<Transform> _sortedIdolPositions = new List<Transform>();

	// Token: 0x040025D2 RID: 9682
	private float _currentActivationTime = -1f;

	// Token: 0x040025D3 RID: 9683
	private float _lastTappedTime;

	// Token: 0x040025D4 RID: 9684
	private bool _progressing;

	// Token: 0x040025D5 RID: 9685
	private float _idolActivationDisplay;

	// Token: 0x040025D6 RID: 9686
	private bool _zoneIsActive;

	// Token: 0x040025D7 RID: 9687
	private bool _zoneStateChanged;
}
