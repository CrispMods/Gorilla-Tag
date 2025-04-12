using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class GreyZoneManager : MonoBehaviourPun, IPunObservable, IInRoomCallbacks
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600065C RID: 1628 RVA: 0x00033B45 File Offset: 0x00031D45
	public bool GreyZoneActive
	{
		get
		{
			return this.greyZoneActive;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600065D RID: 1629 RVA: 0x00084DFC File Offset: 0x00082FFC
	public bool GreyZoneAvailable
	{
		get
		{
			bool result = false;
			if (GorillaComputer.instance != null)
			{
				result = (GorillaComputer.instance.GetServerTime().DayOfYear >= this.greyZoneAvailableDayOfYear);
			}
			return result;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x00033B4D File Offset: 0x00031D4D
	public int GravityFactorSelection
	{
		get
		{
			return this.gravityFactorOptionSelection;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x00033B55 File Offset: 0x00031D55
	// (set) Token: 0x06000660 RID: 1632 RVA: 0x00033B5D File Offset: 0x00031D5D
	public bool TickRunning
	{
		get
		{
			return this._tickRunning;
		}
		set
		{
			this._tickRunning = value;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x00033B66 File Offset: 0x00031D66
	public bool HasAuthority
	{
		get
		{
			return !PhotonNetwork.InRoom || base.photonView.IsMine;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x00033B7C File Offset: 0x00031D7C
	public float SummoningProgress
	{
		get
		{
			return this.summoningProgress;
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00033B84 File Offset: 0x00031D84
	public void RegisterSummoner(GreyZoneSummoner summoner)
	{
		if (!this.activeSummoners.Contains(summoner))
		{
			this.activeSummoners.Add(summoner);
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00033BA0 File Offset: 0x00031DA0
	public void DeregisterSummoner(GreyZoneSummoner summoner)
	{
		if (this.activeSummoners.Contains(summoner))
		{
			this.activeSummoners.Remove(summoner);
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00033BBD File Offset: 0x00031DBD
	public void RegisterMoon(MoonController moon)
	{
		this.moonController = moon;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00033BC6 File Offset: 0x00031DC6
	public void UnregisterMoon(MoonController moon)
	{
		if (this.moonController == moon)
		{
			this.moonController = null;
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00033BDD File Offset: 0x00031DDD
	public void ActivateGreyZoneAuthority()
	{
		this.greyZoneActive = true;
		this.photonConnectedDuringActivation = PhotonNetwork.InRoom;
		this.greyZoneActivationTime = (this.photonConnectedDuringActivation ? PhotonNetwork.Time : ((double)Time.time));
		this.ActivateGreyZoneLocal();
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00084E3C File Offset: 0x0008303C
	private void ActivateGreyZoneLocal()
	{
		Shader.SetGlobalInt(this._GreyZoneActive, 1);
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null)
		{
			instance.SetGravityOverride(this, new Action<GTPlayer>(this.GravityOverrideFunction));
			this.gravityOverrideSet = true;
		}
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.FadeOutMusic(2f);
		}
		if (this.audioFadeCoroutine != null)
		{
			base.StopCoroutine(this.audioFadeCoroutine);
		}
		this.audioFadeCoroutine = base.StartCoroutine(this.FadeAudioIn(this.greyZoneAmbience, this.greyZoneAmbienceVolume, this.ambienceFadeTime));
		if (this.greyZoneAmbience != null)
		{
			this.greyZoneAmbience.Play();
		}
		this.greyZoneParticles.gameObject.SetActive(true);
		this.summoningProgress = 1f;
		this.UpdateSummonerVisuals();
		for (int i = 0; i < this.activeSummoners.Count; i++)
		{
			this.activeSummoners[i].OnGreyZoneActivated();
		}
		if (this.OnGreyZoneActivated != null)
		{
			this.OnGreyZoneActivated();
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00084F50 File Offset: 0x00083150
	public void DeactivateGreyZoneAuthority()
	{
		this.greyZoneActive = false;
		foreach (KeyValuePair<int, ValueTuple<VRRig, GreyZoneSummoner>> keyValuePair in this.summoningPlayers)
		{
			this.summoningPlayerProgress[keyValuePair.Key] = 0f;
		}
		this.DeactivateGreyZoneLocal();
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00084FC0 File Offset: 0x000831C0
	private void DeactivateGreyZoneLocal()
	{
		Shader.SetGlobalInt(this._GreyZoneActive, 0);
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.FadeInMusic(4f);
		}
		if (this.audioFadeCoroutine != null)
		{
			base.StopCoroutine(this.audioFadeCoroutine);
		}
		this.audioFadeCoroutine = base.StartCoroutine(this.FadeAudioOut(this.greyZoneAmbience, this.ambienceFadeTime));
		this.greyZoneParticles.gameObject.SetActive(false);
		this.summoningProgress = 0f;
		this.UpdateSummonerVisuals();
		if (this.OnGreyZoneDeactivated != null)
		{
			this.OnGreyZoneDeactivated();
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00085068 File Offset: 0x00083268
	public void ForceStopGreyZone()
	{
		this.greyZoneActive = false;
		Shader.SetGlobalInt(this._GreyZoneActive, 0);
		GTPlayer instance = GTPlayer.Instance;
		if (instance != null)
		{
			instance.UnsetGravityOverride(this);
		}
		this.gravityOverrideSet = false;
		if (this.moonController != null)
		{
			this.moonController.UpdateDistance(1f);
		}
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.FadeInMusic(0f);
		}
		if (this.greyZoneAmbience != null)
		{
			this.greyZoneAmbience.volume = 0f;
			this.greyZoneAmbience.GTStop();
		}
		this.greyZoneParticles.gameObject.SetActive(false);
		this.summoningProgress = 0f;
		this.UpdateSummonerVisuals();
		if (this.OnGreyZoneDeactivated != null)
		{
			this.OnGreyZoneDeactivated();
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00085148 File Offset: 0x00083348
	public void GravityOverrideFunction(GTPlayer player)
	{
		this.gravityReductionAmount = 0f;
		if (this.moonController != null)
		{
			this.gravityReductionAmount = Mathf.InverseLerp(1f - this.skyMonsterDistGravityRampBuffer, this.skyMonsterDistGravityRampBuffer, this.moonController.Distance);
		}
		float d = Mathf.Lerp(1f, this.gravityFactorOptions[this.gravityFactorOptionSelection], this.gravityReductionAmount);
		player.AddForce(Physics.gravity * d * player.scale, ForceMode.Acceleration);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00033C12 File Offset: 0x00031E12
	private IEnumerator FadeAudioIn(AudioSource source, float maxVolume, float duration)
	{
		if (source != null)
		{
			float startingVolume = source.volume;
			float startTime = Time.time;
			source.GTPlay();
			for (float num = 0f; num < 1f; num = (Time.time - startTime) / duration)
			{
				source.volume = Mathf.Lerp(startingVolume, maxVolume, num);
				yield return null;
			}
			source.volume = maxVolume;
		}
		yield break;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00033C2F File Offset: 0x00031E2F
	private IEnumerator FadeAudioOut(AudioSource source, float duration)
	{
		if (source != null)
		{
			float startingVolume = source.volume;
			float startTime = Time.time;
			for (float num = 0f; num < 1f; num = (Time.time - startTime) / duration)
			{
				source.volume = Mathf.Lerp(startingVolume, 0f, num);
				yield return null;
			}
			source.volume = 0f;
			source.Stop();
		}
		yield break;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x000851D4 File Offset: 0x000833D4
	public void VRRigEnteredSummonerProximity(VRRig rig, GreyZoneSummoner summoner)
	{
		if (!this.summoningPlayers.ContainsKey(rig.Creator.ActorNumber))
		{
			this.summoningPlayers.Add(rig.Creator.ActorNumber, new ValueTuple<VRRig, GreyZoneSummoner>(rig, summoner));
			this.summoningPlayerProgress.Add(rig.Creator.ActorNumber, 0f);
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00085234 File Offset: 0x00083434
	public void VRRigExitedSummonerProximity(VRRig rig, GreyZoneSummoner summoner)
	{
		if (this.summoningPlayers.ContainsKey(rig.Creator.ActorNumber))
		{
			this.summoningPlayers.Remove(rig.Creator.ActorNumber);
			this.summoningPlayerProgress.Remove(rig.Creator.ActorNumber);
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00085288 File Offset: 0x00083488
	private void UpdateSummonerVisuals()
	{
		bool greyZoneAvailable = this.GreyZoneAvailable;
		for (int i = 0; i < this.activeSummoners.Count; i++)
		{
			this.activeSummoners[i].UpdateProgressFeedback(greyZoneAvailable);
		}
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x000852C4 File Offset: 0x000834C4
	private void ValidateSummoningPlayers()
	{
		this.invalidSummoners.Clear();
		foreach (KeyValuePair<int, ValueTuple<VRRig, GreyZoneSummoner>> keyValuePair in this.summoningPlayers)
		{
			VRRig item = keyValuePair.Value.Item1;
			GreyZoneSummoner item2 = keyValuePair.Value.Item2;
			if (item.Creator.ActorNumber != keyValuePair.Key || (item.head.rigTarget.position - item2.SummoningFocusPoint).sqrMagnitude > item2.SummonerMaxDistance * item2.SummonerMaxDistance)
			{
				this.invalidSummoners.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.invalidSummoners)
		{
			this.summoningPlayers.Remove(key);
			this.summoningPlayerProgress.Remove(key);
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x000853EC File Offset: 0x000835EC
	private int DayNightOverrideFunction(int inputIndex)
	{
		int num = 0;
		int num2 = 8;
		int num3 = inputIndex - num;
		int num4 = num2 - inputIndex;
		if (num3 <= 0 || num4 <= 0)
		{
			return inputIndex;
		}
		if (num4 > num3)
		{
			return num2;
		}
		return num;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00033C45 File Offset: 0x00031E45
	private void Awake()
	{
		if (GreyZoneManager.Instance == null)
		{
			GreyZoneManager.Instance = this;
			this.greyZoneAmbienceVolume = this.greyZoneAmbience.volume;
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00085418 File Offset: 0x00083618
	private void OnEnable()
	{
		if (this.forceTimeOfDayToNight)
		{
			BetterDayNightManager instance = BetterDayNightManager.instance;
			if (instance != null)
			{
				instance.SetTimeIndexOverrideFunction(new Func<int, int>(this.DayNightOverrideFunction));
			}
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00085450 File Offset: 0x00083650
	private void OnDisable()
	{
		this.ForceStopGreyZone();
		if (this.forceTimeOfDayToNight)
		{
			BetterDayNightManager instance = BetterDayNightManager.instance;
			if (instance != null)
			{
				instance.UnsetTimeIndexOverrideFunction();
			}
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00033C78 File Offset: 0x00031E78
	private void Update()
	{
		if (this.HasAuthority)
		{
			this.AuthorityUpdate();
		}
		this.SharedUpdate();
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00085484 File Offset: 0x00083684
	private void AuthorityUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (this.greyZoneActive)
		{
			this.summoningProgress = 1f;
			double num;
			if (this.photonConnectedDuringActivation && PhotonNetwork.InRoom)
			{
				num = PhotonNetwork.Time;
			}
			else if (!this.photonConnectedDuringActivation && !PhotonNetwork.InRoom)
			{
				num = (double)Time.time;
			}
			else
			{
				num = -100.0;
			}
			if (num > this.greyZoneActivationTime + (double)this.greyZoneActiveDuration || num < this.greyZoneActivationTime - 10.0)
			{
				this.DeactivateGreyZoneAuthority();
				return;
			}
		}
		else if (this.GreyZoneAvailable)
		{
			this.roomPlayerList = PhotonNetwork.PlayerList;
			int num2 = 1;
			if (this.roomPlayerList != null && this.roomPlayerList.Length != 0)
			{
				num2 = Mathf.Max((this.roomPlayerList.Length + 1) / 2, 1);
			}
			float num3 = 0f;
			float num4 = 1f / this.summoningActivationTime;
			foreach (KeyValuePair<int, ValueTuple<VRRig, GreyZoneSummoner>> keyValuePair in this.summoningPlayers)
			{
				VRRig item = keyValuePair.Value.Item1;
				GreyZoneSummoner item2 = keyValuePair.Value.Item2;
				float num5 = this.summoningPlayerProgress[keyValuePair.Key];
				Vector3 lhs = item2.SummoningFocusPoint - item.leftHand.rigTarget.position;
				Vector3 rhs = -item.leftHand.rigTarget.right;
				bool flag = Vector3.Dot(lhs, rhs) > 0f;
				Vector3 lhs2 = item2.SummoningFocusPoint - item.rightHand.rigTarget.position;
				Vector3 right = item.rightHand.rigTarget.right;
				bool flag2 = Vector3.Dot(lhs2, right) > 0f;
				if (flag && flag2)
				{
					num5 = Mathf.MoveTowards(num5, 1f, num4 * deltaTime);
				}
				else
				{
					num5 = Mathf.MoveTowards(num5, 0f, num4 * deltaTime);
				}
				num3 += num5;
				this.summoningPlayerProgress[keyValuePair.Key] = num5;
			}
			float num6 = 0.95f;
			this.summoningProgress = Mathf.Clamp01(num3 / num6 / (float)num2);
			this.UpdateSummonerVisuals();
			if (this.summoningProgress > 0.99f)
			{
				this.ActivateGreyZoneAuthority();
			}
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000856DC File Offset: 0x000838DC
	private void SharedUpdate()
	{
		GTPlayer instance = GTPlayer.Instance;
		if (this.greyZoneActive)
		{
			Vector3 b = Vector3.ClampMagnitude(instance.InstantaneousVelocity * this.particlePredictiveSpawnVelocityFactor, this.particlePredictiveSpawnMaxDist);
			this.greyZoneParticles.transform.position = instance.HeadCenterPosition + Vector3.down * 0.5f + b;
		}
		else if (this.gravityOverrideSet && this.gravityReductionAmount < 0.01f)
		{
			instance.UnsetGravityOverride(this);
			this.gravityOverrideSet = false;
		}
		float num = this.greyZoneActive ? 0f : 1f;
		float smoothTime = this.greyZoneActive ? this.skyMonsterMovementEnterTime : this.skyMonsterMovementExitTime;
		if (this.moonController != null && this.moonController.Distance != num)
		{
			float num2 = Mathf.SmoothDamp(this.moonController.Distance, num, ref this.skyMonsterMovementVelocity, smoothTime);
			if ((double)Mathf.Abs(num2 - num) < 0.001)
			{
				num2 = num;
			}
			this.moonController.UpdateDistance(num2);
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x000857F0 File Offset: 0x000839F0
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(this.greyZoneActive);
			stream.SendNext(this.greyZoneActivationTime);
			stream.SendNext(this.photonConnectedDuringActivation);
			stream.SendNext(this.gravityFactorOptionSelection);
			stream.SendNext(this.summoningProgress);
			return;
		}
		if (stream.IsReading && info.Sender.IsMasterClient)
		{
			bool flag = this.greyZoneActive;
			this.greyZoneActive = (bool)stream.ReceiveNext();
			this.greyZoneActivationTime = ((double)stream.ReceiveNext()).GetFinite();
			this.photonConnectedDuringActivation = (bool)stream.ReceiveNext();
			this.gravityFactorOptionSelection = (int)stream.ReceiveNext();
			this.summoningProgress = ((float)stream.ReceiveNext()).ClampSafe(0f, 1f);
			this.UpdateSummonerVisuals();
			if (this.greyZoneActive && !flag)
			{
				this.ActivateGreyZoneLocal();
				return;
			}
			if (!this.greyZoneActive && flag)
			{
				this.DeactivateGreyZoneLocal();
			}
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00033C8E File Offset: 0x00031E8E
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.ValidateSummoningPlayers();
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00033C8E File Offset: 0x00031E8E
	public void OnMasterClientSwitched(Player newMasterClient)
	{
		this.ValidateSummoningPlayers();
	}

	// Token: 0x040007A8 RID: 1960
	[OnEnterPlay_SetNull]
	public static volatile GreyZoneManager Instance;

	// Token: 0x040007A9 RID: 1961
	[SerializeField]
	private float greyZoneActiveDuration = 90f;

	// Token: 0x040007AA RID: 1962
	[SerializeField]
	private float[] gravityFactorOptions = new float[]
	{
		0.25f,
		0.5f,
		0.75f
	};

	// Token: 0x040007AB RID: 1963
	[SerializeField]
	private int gravityFactorOptionSelection = 1;

	// Token: 0x040007AC RID: 1964
	[SerializeField]
	private float summoningActivationTime = 3f;

	// Token: 0x040007AD RID: 1965
	[SerializeField]
	private AudioSource greyZoneAmbience;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	private float ambienceFadeTime = 4f;

	// Token: 0x040007AF RID: 1967
	[SerializeField]
	private bool forceTimeOfDayToNight;

	// Token: 0x040007B0 RID: 1968
	[SerializeField]
	private float skyMonsterMovementEnterTime = 4.5f;

	// Token: 0x040007B1 RID: 1969
	[SerializeField]
	private float skyMonsterMovementExitTime = 3.2f;

	// Token: 0x040007B2 RID: 1970
	[SerializeField]
	private float skyMonsterDistGravityRampBuffer = 0.15f;

	// Token: 0x040007B3 RID: 1971
	[SerializeField]
	[Range(0f, 1f)]
	private float gravityReductionAmount = 1f;

	// Token: 0x040007B4 RID: 1972
	[SerializeField]
	private ParticleSystem greyZoneParticles;

	// Token: 0x040007B5 RID: 1973
	[SerializeField]
	private float particlePredictiveSpawnMaxDist = 4f;

	// Token: 0x040007B6 RID: 1974
	[SerializeField]
	private float particlePredictiveSpawnVelocityFactor = 0.5f;

	// Token: 0x040007B7 RID: 1975
	private bool photonConnectedDuringActivation;

	// Token: 0x040007B8 RID: 1976
	private double greyZoneActivationTime;

	// Token: 0x040007B9 RID: 1977
	private bool greyZoneActive;

	// Token: 0x040007BA RID: 1978
	private bool _tickRunning;

	// Token: 0x040007BB RID: 1979
	private float summoningProgress;

	// Token: 0x040007BC RID: 1980
	private List<GreyZoneSummoner> activeSummoners = new List<GreyZoneSummoner>();

	// Token: 0x040007BD RID: 1981
	private Dictionary<int, ValueTuple<VRRig, GreyZoneSummoner>> summoningPlayers = new Dictionary<int, ValueTuple<VRRig, GreyZoneSummoner>>();

	// Token: 0x040007BE RID: 1982
	private Dictionary<int, float> summoningPlayerProgress = new Dictionary<int, float>();

	// Token: 0x040007BF RID: 1983
	private HashSet<int> invalidSummoners = new HashSet<int>();

	// Token: 0x040007C0 RID: 1984
	private Coroutine audioFadeCoroutine;

	// Token: 0x040007C1 RID: 1985
	private Player[] roomPlayerList;

	// Token: 0x040007C2 RID: 1986
	private ShaderHashId _GreyZoneActive = new ShaderHashId("_GreyZoneActive");

	// Token: 0x040007C3 RID: 1987
	private MoonController moonController;

	// Token: 0x040007C4 RID: 1988
	private float skyMonsterMovementVelocity;

	// Token: 0x040007C5 RID: 1989
	private bool gravityOverrideSet;

	// Token: 0x040007C6 RID: 1990
	private float greyZoneAmbienceVolume = 0.15f;

	// Token: 0x040007C7 RID: 1991
	private int greyZoneAvailableDayOfYear = new DateTime(2024, 10, 25).DayOfYear;

	// Token: 0x040007C8 RID: 1992
	public Action OnGreyZoneActivated;

	// Token: 0x040007C9 RID: 1993
	public Action OnGreyZoneDeactivated;
}
