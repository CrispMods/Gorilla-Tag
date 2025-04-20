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

// Token: 0x020000FE RID: 254
public class GreyZoneManager : MonoBehaviourPun, IPunObservable, IInRoomCallbacks
{
	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600069B RID: 1691 RVA: 0x00034DA9 File Offset: 0x00032FA9
	public bool GreyZoneActive
	{
		get
		{
			return this.greyZoneActive;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x00087704 File Offset: 0x00085904
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

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600069D RID: 1693 RVA: 0x00034DB1 File Offset: 0x00032FB1
	public int GravityFactorSelection
	{
		get
		{
			return this.gravityFactorOptionSelection;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x00034DB9 File Offset: 0x00032FB9
	// (set) Token: 0x0600069F RID: 1695 RVA: 0x00034DC1 File Offset: 0x00032FC1
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

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00034DCA File Offset: 0x00032FCA
	public bool HasAuthority
	{
		get
		{
			return !PhotonNetwork.InRoom || base.photonView.IsMine;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00034DE0 File Offset: 0x00032FE0
	public float SummoningProgress
	{
		get
		{
			return this.summoningProgress;
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00034DE8 File Offset: 0x00032FE8
	public void RegisterSummoner(GreyZoneSummoner summoner)
	{
		if (!this.activeSummoners.Contains(summoner))
		{
			this.activeSummoners.Add(summoner);
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00034E04 File Offset: 0x00033004
	public void DeregisterSummoner(GreyZoneSummoner summoner)
	{
		if (this.activeSummoners.Contains(summoner))
		{
			this.activeSummoners.Remove(summoner);
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00034E21 File Offset: 0x00033021
	public void RegisterMoon(MoonController moon)
	{
		this.moonController = moon;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00034E2A File Offset: 0x0003302A
	public void UnregisterMoon(MoonController moon)
	{
		if (this.moonController == moon)
		{
			this.moonController = null;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00034E41 File Offset: 0x00033041
	public void ActivateGreyZoneAuthority()
	{
		this.greyZoneActive = true;
		this.photonConnectedDuringActivation = PhotonNetwork.InRoom;
		this.greyZoneActivationTime = (this.photonConnectedDuringActivation ? PhotonNetwork.Time : ((double)Time.time));
		this.ActivateGreyZoneLocal();
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00087744 File Offset: 0x00085944
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

	// Token: 0x060006A8 RID: 1704 RVA: 0x00087858 File Offset: 0x00085A58
	public void DeactivateGreyZoneAuthority()
	{
		this.greyZoneActive = false;
		foreach (KeyValuePair<int, ValueTuple<VRRig, GreyZoneSummoner>> keyValuePair in this.summoningPlayers)
		{
			this.summoningPlayerProgress[keyValuePair.Key] = 0f;
		}
		this.DeactivateGreyZoneLocal();
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x000878C8 File Offset: 0x00085AC8
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

	// Token: 0x060006AA RID: 1706 RVA: 0x00087970 File Offset: 0x00085B70
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

	// Token: 0x060006AB RID: 1707 RVA: 0x00087A50 File Offset: 0x00085C50
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

	// Token: 0x060006AC RID: 1708 RVA: 0x00034E76 File Offset: 0x00033076
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

	// Token: 0x060006AD RID: 1709 RVA: 0x00034E93 File Offset: 0x00033093
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

	// Token: 0x060006AE RID: 1710 RVA: 0x00087ADC File Offset: 0x00085CDC
	public void VRRigEnteredSummonerProximity(VRRig rig, GreyZoneSummoner summoner)
	{
		if (!this.summoningPlayers.ContainsKey(rig.Creator.ActorNumber))
		{
			this.summoningPlayers.Add(rig.Creator.ActorNumber, new ValueTuple<VRRig, GreyZoneSummoner>(rig, summoner));
			this.summoningPlayerProgress.Add(rig.Creator.ActorNumber, 0f);
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00087B3C File Offset: 0x00085D3C
	public void VRRigExitedSummonerProximity(VRRig rig, GreyZoneSummoner summoner)
	{
		if (this.summoningPlayers.ContainsKey(rig.Creator.ActorNumber))
		{
			this.summoningPlayers.Remove(rig.Creator.ActorNumber);
			this.summoningPlayerProgress.Remove(rig.Creator.ActorNumber);
		}
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00087B90 File Offset: 0x00085D90
	private void UpdateSummonerVisuals()
	{
		bool greyZoneAvailable = this.GreyZoneAvailable;
		for (int i = 0; i < this.activeSummoners.Count; i++)
		{
			this.activeSummoners[i].UpdateProgressFeedback(greyZoneAvailable);
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00087BCC File Offset: 0x00085DCC
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

	// Token: 0x060006B2 RID: 1714 RVA: 0x00087CF4 File Offset: 0x00085EF4
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

	// Token: 0x060006B3 RID: 1715 RVA: 0x00034EA9 File Offset: 0x000330A9
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

	// Token: 0x060006B4 RID: 1716 RVA: 0x00087D20 File Offset: 0x00085F20
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

	// Token: 0x060006B5 RID: 1717 RVA: 0x00087D58 File Offset: 0x00085F58
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

	// Token: 0x060006B6 RID: 1718 RVA: 0x00034EDC File Offset: 0x000330DC
	private void Update()
	{
		if (this.HasAuthority)
		{
			this.AuthorityUpdate();
		}
		this.SharedUpdate();
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00087D8C File Offset: 0x00085F8C
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

	// Token: 0x060006B8 RID: 1720 RVA: 0x00087FE4 File Offset: 0x000861E4
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

	// Token: 0x060006B9 RID: 1721 RVA: 0x000880F8 File Offset: 0x000862F8
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

	// Token: 0x060006BA RID: 1722 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00034EF2 File Offset: 0x000330F2
	public void OnPlayerLeftRoom(Player otherPlayer)
	{
		this.ValidateSummoningPlayers();
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00034EF2 File Offset: 0x000330F2
	public void OnMasterClientSwitched(Player newMasterClient)
	{
		this.ValidateSummoningPlayers();
	}

	// Token: 0x040007E8 RID: 2024
	[OnEnterPlay_SetNull]
	public static volatile GreyZoneManager Instance;

	// Token: 0x040007E9 RID: 2025
	[SerializeField]
	private float greyZoneActiveDuration = 90f;

	// Token: 0x040007EA RID: 2026
	[SerializeField]
	private float[] gravityFactorOptions = new float[]
	{
		0.25f,
		0.5f,
		0.75f
	};

	// Token: 0x040007EB RID: 2027
	[SerializeField]
	private int gravityFactorOptionSelection = 1;

	// Token: 0x040007EC RID: 2028
	[SerializeField]
	private float summoningActivationTime = 3f;

	// Token: 0x040007ED RID: 2029
	[SerializeField]
	private AudioSource greyZoneAmbience;

	// Token: 0x040007EE RID: 2030
	[SerializeField]
	private float ambienceFadeTime = 4f;

	// Token: 0x040007EF RID: 2031
	[SerializeField]
	private bool forceTimeOfDayToNight;

	// Token: 0x040007F0 RID: 2032
	[SerializeField]
	private float skyMonsterMovementEnterTime = 4.5f;

	// Token: 0x040007F1 RID: 2033
	[SerializeField]
	private float skyMonsterMovementExitTime = 3.2f;

	// Token: 0x040007F2 RID: 2034
	[SerializeField]
	private float skyMonsterDistGravityRampBuffer = 0.15f;

	// Token: 0x040007F3 RID: 2035
	[SerializeField]
	[Range(0f, 1f)]
	private float gravityReductionAmount = 1f;

	// Token: 0x040007F4 RID: 2036
	[SerializeField]
	private ParticleSystem greyZoneParticles;

	// Token: 0x040007F5 RID: 2037
	[SerializeField]
	private float particlePredictiveSpawnMaxDist = 4f;

	// Token: 0x040007F6 RID: 2038
	[SerializeField]
	private float particlePredictiveSpawnVelocityFactor = 0.5f;

	// Token: 0x040007F7 RID: 2039
	private bool photonConnectedDuringActivation;

	// Token: 0x040007F8 RID: 2040
	private double greyZoneActivationTime;

	// Token: 0x040007F9 RID: 2041
	private bool greyZoneActive;

	// Token: 0x040007FA RID: 2042
	private bool _tickRunning;

	// Token: 0x040007FB RID: 2043
	private float summoningProgress;

	// Token: 0x040007FC RID: 2044
	private List<GreyZoneSummoner> activeSummoners = new List<GreyZoneSummoner>();

	// Token: 0x040007FD RID: 2045
	private Dictionary<int, ValueTuple<VRRig, GreyZoneSummoner>> summoningPlayers = new Dictionary<int, ValueTuple<VRRig, GreyZoneSummoner>>();

	// Token: 0x040007FE RID: 2046
	private Dictionary<int, float> summoningPlayerProgress = new Dictionary<int, float>();

	// Token: 0x040007FF RID: 2047
	private HashSet<int> invalidSummoners = new HashSet<int>();

	// Token: 0x04000800 RID: 2048
	private Coroutine audioFadeCoroutine;

	// Token: 0x04000801 RID: 2049
	private Player[] roomPlayerList;

	// Token: 0x04000802 RID: 2050
	private ShaderHashId _GreyZoneActive = new ShaderHashId("_GreyZoneActive");

	// Token: 0x04000803 RID: 2051
	private MoonController moonController;

	// Token: 0x04000804 RID: 2052
	private float skyMonsterMovementVelocity;

	// Token: 0x04000805 RID: 2053
	private bool gravityOverrideSet;

	// Token: 0x04000806 RID: 2054
	private float greyZoneAmbienceVolume = 0.15f;

	// Token: 0x04000807 RID: 2055
	private int greyZoneAvailableDayOfYear = new DateTime(2024, 10, 25).DayOfYear;

	// Token: 0x04000808 RID: 2056
	public Action OnGreyZoneActivated;

	// Token: 0x04000809 RID: 2057
	public Action OnGreyZoneDeactivated;
}
