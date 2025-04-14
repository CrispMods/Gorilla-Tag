using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CjLib;
using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using GorillaNetworking;
using GorillaTag.GuidedRefs;
using GorillaTagScripts;
using GorillaTagScripts.Builder;
using Photon.Pun;
using Photon.Voice.Unity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR;

// Token: 0x02000593 RID: 1427
public class GorillaTagger : MonoBehaviour, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06002358 RID: 9048 RVA: 0x000AF4D0 File Offset: 0x000AD6D0
	public static GorillaTagger Instance
	{
		get
		{
			return GorillaTagger._instance;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06002359 RID: 9049 RVA: 0x000AF4D7 File Offset: 0x000AD6D7
	public NetworkView myVRRig
	{
		get
		{
			return this.offlineVRRig.netView;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x0600235A RID: 9050 RVA: 0x000AF4E4 File Offset: 0x000AD6E4
	internal VRRigSerializer rigSerializer
	{
		get
		{
			return this.offlineVRRig.rigSerializer;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x0600235B RID: 9051 RVA: 0x000AF4F1 File Offset: 0x000AD6F1
	// (set) Token: 0x0600235C RID: 9052 RVA: 0x000AF4F9 File Offset: 0x000AD6F9
	public Rigidbody rigidbody { get; private set; }

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x0600235D RID: 9053 RVA: 0x000AF502 File Offset: 0x000AD702
	public float DefaultHandTapVolume
	{
		get
		{
			return this.cacheHandTapVolume;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x0600235E RID: 9054 RVA: 0x000AF50A File Offset: 0x000AD70A
	// (set) Token: 0x0600235F RID: 9055 RVA: 0x000AF512 File Offset: 0x000AD712
	public Recorder myRecorder { get; private set; }

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06002360 RID: 9056 RVA: 0x000AF51B File Offset: 0x000AD71B
	public float sphereCastRadius
	{
		get
		{
			if (this.tagRadiusOverride == null)
			{
				return 0.03f;
			}
			return this.tagRadiusOverride.Value;
		}
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000AF53B File Offset: 0x000AD73B
	public void SetTagRadiusOverrideThisFrame(float radius)
	{
		this.tagRadiusOverride = new float?(radius);
		this.tagRadiusOverrideFrame = Time.frameCount;
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000AF554 File Offset: 0x000AD754
	protected void Awake()
	{
		this.GuidedRefInitialize();
		this.RecoverMissingRefs();
		this.MirrorCameraCullingMask = new Watchable<int>(this.BaseMirrorCameraCullingMask);
		if (GorillaTagger._instance != null && GorillaTagger._instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			GorillaTagger._instance = this;
			GorillaTagger.hasInstance = true;
			Action action = GorillaTagger.onPlayerSpawnedRootCallback;
			if (action != null)
			{
				action();
			}
		}
		if (!this.disableTutorial && (this.testTutorial || (PlayerPrefs.GetString("tutorial") != "done" && PlayerPrefs.GetString("didTutorial") != "done" && NetworkSystemConfig.AppVersion != "dev")))
		{
			base.transform.parent.position = new Vector3(-140f, 28f, -102f);
			base.transform.parent.eulerAngles = new Vector3(0f, 180f, 0f);
			GTPlayer.Instance.InitializeValues();
			PlayerPrefs.SetFloat("redValue", Random.value);
			PlayerPrefs.SetFloat("greenValue", Random.value);
			PlayerPrefs.SetFloat("blueValue", Random.value);
			PlayerPrefs.Save();
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("didTutorial", true);
			PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable, null, null);
			PlayerPrefs.SetString("didTutorial", "done");
			PlayerPrefs.Save();
		}
		this.thirdPersonCamera.SetActive(Application.platform != RuntimePlatform.Android);
		this.inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		this.wasInOverlay = false;
		this.baseSlideControl = GTPlayer.Instance.slideControl;
		this.gorillaTagColliderLayerMask = LayerMask.GetMask(new string[]
		{
			"Gorilla Tag Collider"
		});
		this.rigidbody = base.GetComponent<Rigidbody>();
		this.cacheHandTapVolume = this.handTapVolume;
		OVRManager.foveatedRenderingLevel = OVRManager.FoveatedRenderingLevel.Medium;
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000AF74F File Offset: 0x000AD94F
	protected void OnDestroy()
	{
		if (GorillaTagger._instance == this)
		{
			GorillaTagger._instance = null;
			GorillaTagger.hasInstance = false;
		}
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000AF76C File Offset: 0x000AD96C
	private void IsXRSubsystemActive()
	{
		this.loadedDeviceName = XRSettings.loadedDeviceName;
		List<XRDisplaySubsystem> list = new List<XRDisplaySubsystem>();
		SubsystemManager.GetInstances<XRDisplaySubsystem>(list);
		using (List<XRDisplaySubsystem>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.running)
				{
					this.xrSubsystemIsActive = true;
					return;
				}
			}
		}
		this.xrSubsystemIsActive = false;
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000AF7E0 File Offset: 0x000AD9E0
	protected void Start()
	{
		this.IsXRSubsystemActive();
		if (this.loadedDeviceName == "OpenVR Display")
		{
			GTPlayer.Instance.leftHandOffset = new Vector3(-0.02f, 0f, -0.07f);
			GTPlayer.Instance.rightHandOffset = new Vector3(0.02f, 0f, -0.07f);
			Quaternion rotation = Quaternion.Euler(new Vector3(-90f, 180f, -20f));
			Quaternion rotation2 = Quaternion.Euler(new Vector3(-90f, 180f, 20f));
			Quaternion lhs = Quaternion.Euler(new Vector3(-141f, 204f, -27f));
			Quaternion lhs2 = Quaternion.Euler(new Vector3(-141f, 156f, 27f));
			GTPlayer.Instance.leftHandRotOffset = lhs * Quaternion.Inverse(rotation);
			GTPlayer.Instance.rightHandRotOffset = lhs2 * Quaternion.Inverse(rotation2);
		}
		this.bodyVector = new Vector3(0f, this.bodyCollider.height / 2f - this.bodyCollider.radius, 0f);
		if (SteamManager.Initialized)
		{
			this.gameOverlayActivatedCb = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnGameOverlayActivated));
		}
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000AF928 File Offset: 0x000ADB28
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		this.isGameOverlayActive = (pCallback.m_bActive > 0);
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000AF93C File Offset: 0x000ADB3C
	protected void LateUpdate()
	{
		GorillaTagger.<>c__DisplayClass111_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (this.isGameOverlayActive)
		{
			if (this.leftHandTriggerCollider.activeSelf)
			{
				this.leftHandTriggerCollider.SetActive(false);
				this.rightHandTriggerCollider.SetActive(true);
			}
			GTPlayer.Instance.inOverlay = true;
		}
		else
		{
			if (!this.leftHandTriggerCollider.activeSelf)
			{
				this.leftHandTriggerCollider.SetActive(true);
				this.rightHandTriggerCollider.SetActive(true);
			}
			GTPlayer.Instance.inOverlay = false;
		}
		if (this.xrSubsystemIsActive && Application.platform != RuntimePlatform.Android)
		{
			if (Mathf.Abs(Time.fixedDeltaTime - 1f / XRDevice.refreshRate) > 0.0001f)
			{
				Debug.Log(" =========== adjusting refresh size =========");
				Debug.Log(" fixedDeltaTime before:\t" + Time.fixedDeltaTime.ToString());
				Debug.Log(" refresh rate         :\t" + XRDevice.refreshRate.ToString());
				Time.fixedDeltaTime = 1f / XRDevice.refreshRate;
				Debug.Log(" fixedDeltaTime after :\t" + Time.fixedDeltaTime.ToString());
				Debug.Log(" history size before  :\t" + GTPlayer.Instance.velocityHistorySize.ToString());
				GTPlayer.Instance.velocityHistorySize = Mathf.Max(Mathf.Min(Mathf.FloorToInt(XRDevice.refreshRate * 0.083333336f), 10), 6);
				if (GTPlayer.Instance.velocityHistorySize > 9)
				{
					GTPlayer.Instance.velocityHistorySize--;
				}
				Debug.Log("new history size: " + GTPlayer.Instance.velocityHistorySize.ToString());
				Debug.Log(" ============================================");
				GTPlayer.Instance.slideControl = 1f - this.CalcSlideControl(XRDevice.refreshRate);
				GTPlayer.Instance.InitializeValues();
			}
		}
		else if (Application.platform != RuntimePlatform.Android && OVRManager.instance != null && OVRManager.OVRManagerinitialized && OVRManager.instance.gameObject != null && OVRManager.instance.gameObject.activeSelf)
		{
			Object.Destroy(OVRManager.instance.gameObject);
		}
		if (!this.frameRateUpdated && Application.platform == RuntimePlatform.Android && OVRManager.instance.gameObject.activeSelf)
		{
			InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsManually;
			int num = OVRManager.display.displayFrequenciesAvailable.Length - 1;
			float num2 = OVRManager.display.displayFrequenciesAvailable[num];
			float systemDisplayFrequency = OVRPlugin.systemDisplayFrequency;
			if (systemDisplayFrequency != 60f)
			{
				if (systemDisplayFrequency == 71f)
				{
					num2 = 72f;
				}
			}
			else
			{
				num2 = 60f;
			}
			while (num2 > 90f)
			{
				num--;
				if (num < 0)
				{
					break;
				}
				num2 = OVRManager.display.displayFrequenciesAvailable[num];
			}
			float num3 = 1f;
			if (Mathf.Abs(Time.fixedDeltaTime - 1f / num2 * num3) > 0.0001f)
			{
				float num4 = Time.fixedDeltaTime - 1f / num2 * num3;
				Debug.Log(" =========== adjusting refresh size =========");
				Debug.Log("!!!!Time.fixedDeltaTime - (1f / newRefreshRate) * " + num3.ToString() + ")" + num4.ToString());
				Debug.Log("Old Refresh rate: " + systemDisplayFrequency.ToString());
				Debug.Log("New Refresh rate: " + num2.ToString());
				Debug.Log(" fixedDeltaTime before:\t" + Time.fixedDeltaTime.ToString());
				Debug.Log(" fixedDeltaTime after :\t" + (1f / num2).ToString());
				Time.fixedDeltaTime = 1f / num2 * num3;
				OVRPlugin.systemDisplayFrequency = num2;
				GTPlayer.Instance.velocityHistorySize = Mathf.FloorToInt(num2 * 0.083333336f);
				if (GTPlayer.Instance.velocityHistorySize > 9)
				{
					GTPlayer.Instance.velocityHistorySize--;
				}
				Debug.Log(" fixedDeltaTime after :\t" + Time.fixedDeltaTime.ToString());
				Debug.Log(" history size before  :\t" + GTPlayer.Instance.velocityHistorySize.ToString());
				Debug.Log("new history size: " + GTPlayer.Instance.velocityHistorySize.ToString());
				Debug.Log(" ============================================");
				GTPlayer.Instance.slideControl = 1f - this.CalcSlideControl(XRDevice.refreshRate);
				GTPlayer.Instance.InitializeValues();
				OVRManager.instance.gameObject.SetActive(false);
				this.frameRateUpdated = true;
			}
		}
		if (!this.xrSubsystemIsActive && Application.platform != RuntimePlatform.Android && Mathf.Abs(Time.fixedDeltaTime - 0.0069444445f) > 0.0001f)
		{
			Debug.Log("updating delta time. was: " + Time.fixedDeltaTime.ToString() + ". now it's " + 0.0069444445f.ToString());
			Application.targetFrameRate = 144;
			Time.fixedDeltaTime = 0.0069444445f;
			GTPlayer.Instance.velocityHistorySize = Mathf.Min(Mathf.FloorToInt(12f), 10);
			if (GTPlayer.Instance.velocityHistorySize > 9)
			{
				GTPlayer.Instance.velocityHistorySize--;
			}
			Debug.Log("new history size: " + GTPlayer.Instance.velocityHistorySize.ToString());
			GTPlayer.Instance.slideControl = 1f - this.CalcSlideControl(144f);
			GTPlayer.Instance.InitializeValues();
		}
		this.leftRaycastSweep = this.leftHandTransform.position - this.lastLeftHandPositionForTag;
		this.leftHeadRaycastSweep = this.leftHandTransform.position - this.headCollider.transform.position;
		this.rightRaycastSweep = this.rightHandTransform.position - this.lastRightHandPositionForTag;
		this.rightHeadRaycastSweep = this.rightHandTransform.position - this.headCollider.transform.position;
		this.headRaycastSweep = this.headCollider.transform.position - this.lastHeadPositionForTag;
		this.bodyRaycastSweep = this.bodyCollider.transform.position - this.lastBodyPositionForTag;
		this.otherPlayer = null;
		this.touchedPlayer = null;
		CS$<>8__locals1.otherTouchedPlayer = null;
		if (this.tagRadiusOverrideFrame < Time.frameCount)
		{
			this.tagRadiusOverride = null;
		}
		float num5 = this.sphereCastRadius * GTPlayer.Instance.scale;
		CS$<>8__locals1.bodyHit = false;
		CS$<>8__locals1.leftHandHit = false;
		this.nonAllocHits = Physics.SphereCastNonAlloc(this.lastLeftHandPositionForTag, num5, this.leftRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.leftRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(false, true, ref CS$<>8__locals1);
		this.nonAllocHits = Physics.SphereCastNonAlloc(this.headCollider.transform.position, num5, this.leftHeadRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.leftHeadRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(false, true, ref CS$<>8__locals1);
		this.nonAllocHits = Physics.SphereCastNonAlloc(this.lastRightHandPositionForTag, num5, this.rightRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.rightRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(false, false, ref CS$<>8__locals1);
		this.nonAllocHits = Physics.SphereCastNonAlloc(this.headCollider.transform.position, num5, this.rightHeadRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.rightHeadRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(false, false, ref CS$<>8__locals1);
		this.nonAllocHits = Physics.SphereCastNonAlloc(this.headCollider.transform.position, this.headCollider.radius * this.headCollider.transform.localScale.x * GTPlayer.Instance.scale, this.headRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.headRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(true, false, ref CS$<>8__locals1);
		this.topVector = this.lastBodyPositionForTag + this.bodyVector;
		this.bottomVector = this.lastBodyPositionForTag - this.bodyVector;
		this.nonAllocHits = Physics.CapsuleCastNonAlloc(this.topVector, this.bottomVector, this.bodyCollider.radius * 2f * GTPlayer.Instance.scale, this.bodyRaycastSweep.normalized, this.nonAllocRaycastHits, Mathf.Max(this.bodyRaycastSweep.magnitude, num5), this.gorillaTagColliderLayerMask, QueryTriggerInteraction.Collide);
		this.<LateUpdate>g__TryTaggingAllHits|111_0(true, false, ref CS$<>8__locals1);
		if (this.otherPlayer != null)
		{
			GameMode.ActiveGameMode.LocalTag(this.otherPlayer, NetworkSystem.Instance.LocalPlayer, CS$<>8__locals1.bodyHit, CS$<>8__locals1.leftHandHit);
			GameMode.ReportTag(this.otherPlayer);
		}
		if (CS$<>8__locals1.otherTouchedPlayer != null && GorillaGameManager.instance != null)
		{
			CustomGameMode.TouchPlayer(CS$<>8__locals1.otherTouchedPlayer);
		}
		GTPlayer instance = GTPlayer.Instance;
		bool flag = true;
		this.ProcessHandTapping(flag, ref this.lastLeftTap, ref this.leftHandTouching, instance.leftHandMaterialTouchIndex, instance.leftHandSurfaceOverride, instance.leftHandHitInfo, instance.leftHandFollower, this.leftHandSlideSource, instance.leftHandCenterVelocityTracker);
		flag = false;
		this.ProcessHandTapping(flag, ref this.lastRightTap, ref this.rightHandTouching, instance.rightHandMaterialTouchIndex, instance.rightHandSurfaceOverride, instance.rightHandHitInfo, instance.rightHandFollower, this.rightHandSlideSource, instance.rightHandCenterVelocityTracker);
		this.CheckEndStatusEffect();
		this.lastLeftHandPositionForTag = this.leftHandTransform.position;
		this.lastRightHandPositionForTag = this.rightHandTransform.position;
		this.lastBodyPositionForTag = this.bodyCollider.transform.position;
		this.lastHeadPositionForTag = this.headCollider.transform.position;
		if (GTPlayer.Instance.IsBodySliding && (double)GTPlayer.Instance.RigidbodyVelocity.magnitude >= 0.15)
		{
			if (!this.bodySlideSource.isPlaying)
			{
				this.bodySlideSource.Play();
			}
		}
		else
		{
			this.bodySlideSource.Stop();
		}
		if (GorillaComputer.instance == null || NetworkSystem.Instance.LocalRecorder == null)
		{
			return;
		}
		if (GorillaComputer.instance.voiceChatOn == "TRUE")
		{
			this.myRecorder = NetworkSystem.Instance.LocalRecorder;
			if (this.offlineVRRig.remoteUseReplacementVoice)
			{
				this.offlineVRRig.remoteUseReplacementVoice = false;
			}
			if (GorillaComputer.instance.pttType != "ALL CHAT")
			{
				this.primaryButtonPressRight = false;
				this.secondaryButtonPressRight = false;
				this.primaryButtonPressLeft = false;
				this.secondaryButtonPressLeft = false;
				this.primaryButtonPressRight = ControllerInputPoller.PrimaryButtonPress(XRNode.RightHand);
				this.secondaryButtonPressRight = ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand);
				this.primaryButtonPressLeft = ControllerInputPoller.PrimaryButtonPress(XRNode.LeftHand);
				this.secondaryButtonPressLeft = ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand);
				if (this.primaryButtonPressRight || this.secondaryButtonPressRight || this.primaryButtonPressLeft || this.secondaryButtonPressLeft)
				{
					if (GorillaComputer.instance.pttType == "PUSH TO MUTE")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = false;
						bool transmitEnabled = this.myRecorder.TransmitEnabled;
						this.myRecorder.TransmitEnabled = false;
						return;
					}
					if (GorillaComputer.instance.pttType == "PUSH TO TALK")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = true;
						bool transmitEnabled2 = this.myRecorder.TransmitEnabled;
						this.myRecorder.TransmitEnabled = true;
						return;
					}
				}
				else
				{
					if (GorillaComputer.instance.pttType == "PUSH TO MUTE")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = true;
						bool transmitEnabled3 = this.myRecorder.TransmitEnabled;
						this.myRecorder.TransmitEnabled = true;
						return;
					}
					if (GorillaComputer.instance.pttType == "PUSH TO TALK")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = false;
						bool transmitEnabled4 = this.myRecorder.TransmitEnabled;
						this.myRecorder.TransmitEnabled = false;
						return;
					}
				}
			}
			else
			{
				if (!this.myRecorder.TransmitEnabled)
				{
					this.myRecorder.TransmitEnabled = true;
				}
				if (!this.offlineVRRig.shouldSendSpeakingLoudness)
				{
					this.offlineVRRig.shouldSendSpeakingLoudness = true;
					return;
				}
			}
		}
		else if (GorillaComputer.instance.voiceChatOn == "FALSE")
		{
			this.myRecorder = NetworkSystem.Instance.LocalRecorder;
			if (!this.offlineVRRig.remoteUseReplacementVoice)
			{
				this.offlineVRRig.remoteUseReplacementVoice = true;
			}
			if (this.myRecorder.TransmitEnabled)
			{
				this.myRecorder.TransmitEnabled = false;
			}
			if (GorillaComputer.instance.pttType != "ALL CHAT")
			{
				this.primaryButtonPressRight = false;
				this.secondaryButtonPressRight = false;
				this.primaryButtonPressLeft = false;
				this.secondaryButtonPressLeft = false;
				this.primaryButtonPressRight = ControllerInputPoller.PrimaryButtonPress(XRNode.RightHand);
				this.secondaryButtonPressRight = ControllerInputPoller.SecondaryButtonPress(XRNode.RightHand);
				this.primaryButtonPressLeft = ControllerInputPoller.PrimaryButtonPress(XRNode.LeftHand);
				this.secondaryButtonPressLeft = ControllerInputPoller.SecondaryButtonPress(XRNode.LeftHand);
				if (this.primaryButtonPressRight || this.secondaryButtonPressRight || this.primaryButtonPressLeft || this.secondaryButtonPressLeft)
				{
					if (GorillaComputer.instance.pttType == "PUSH TO MUTE")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = false;
						return;
					}
					if (GorillaComputer.instance.pttType == "PUSH TO TALK")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = true;
						return;
					}
				}
				else
				{
					if (GorillaComputer.instance.pttType == "PUSH TO MUTE")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = true;
						return;
					}
					if (GorillaComputer.instance.pttType == "PUSH TO TALK")
					{
						this.offlineVRRig.shouldSendSpeakingLoudness = false;
						return;
					}
				}
			}
			else if (!this.offlineVRRig.shouldSendSpeakingLoudness)
			{
				this.offlineVRRig.shouldSendSpeakingLoudness = true;
				return;
			}
		}
		else
		{
			this.myRecorder = NetworkSystem.Instance.LocalRecorder;
			if (this.offlineVRRig.remoteUseReplacementVoice)
			{
				this.offlineVRRig.remoteUseReplacementVoice = false;
			}
			if (this.offlineVRRig.shouldSendSpeakingLoudness)
			{
				this.offlineVRRig.shouldSendSpeakingLoudness = false;
			}
			if (this.myRecorder.TransmitEnabled)
			{
				this.myRecorder.TransmitEnabled = false;
			}
		}
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000B0788 File Offset: 0x000AE988
	private bool TryToTag(RaycastHit hitInfo, bool isBodyTag, out NetPlayer taggedPlayer, out NetPlayer touchedPlayer)
	{
		taggedPlayer = null;
		touchedPlayer = null;
		if (NetworkSystem.Instance.InRoom)
		{
			VRRig componentInParent = hitInfo.collider.GetComponentInParent<VRRig>();
			this.tempCreator = ((componentInParent != null) ? componentInParent.creator : null);
			if (this.tempCreator != null && NetworkSystem.Instance.LocalPlayer != this.tempCreator)
			{
				touchedPlayer = this.tempCreator;
				if (GorillaGameManager.instance != null && Time.time > this.taggedTime + this.tagCooldown && GorillaGameManager.instance.LocalCanTag(NetworkSystem.Instance.LocalPlayer, this.tempCreator))
				{
					if (!isBodyTag)
					{
						this.StartVibration((this.leftHandTransform.position - hitInfo.collider.transform.position).magnitude < (this.rightHandTransform.position - hitInfo.collider.transform.position).magnitude, this.tagHapticStrength, this.tagHapticDuration);
					}
					else
					{
						this.StartVibration(true, this.tagHapticStrength, this.tagHapticDuration);
						this.StartVibration(false, this.tagHapticStrength, this.tagHapticDuration);
					}
					taggedPlayer = this.tempCreator;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000B08D8 File Offset: 0x000AEAD8
	public void StartVibration(bool forLeftController, float amplitude, float duration)
	{
		base.StartCoroutine(this.HapticPulses(forLeftController, amplitude, duration));
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000B08EA File Offset: 0x000AEAEA
	private IEnumerator HapticPulses(bool forLeftController, float amplitude, float duration)
	{
		float startTime = Time.time;
		uint channel = 0U;
		UnityEngine.XR.InputDevice device;
		if (forLeftController)
		{
			device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		}
		else
		{
			device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		}
		while (Time.time < startTime + duration)
		{
			device.SendHapticImpulse(channel, amplitude, this.hapticWaitSeconds);
			yield return new WaitForSeconds(this.hapticWaitSeconds * 0.9f);
		}
		yield break;
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000B0910 File Offset: 0x000AEB10
	public void PlayHapticClip(bool forLeftController, AudioClip clip, float strength)
	{
		if (forLeftController)
		{
			if (this.leftHapticsRoutine != null)
			{
				base.StopCoroutine(this.leftHapticsRoutine);
			}
			this.leftHapticsRoutine = base.StartCoroutine(this.AudioClipHapticPulses(forLeftController, clip, strength));
			return;
		}
		if (this.rightHapticsRoutine != null)
		{
			base.StopCoroutine(this.rightHapticsRoutine);
		}
		this.rightHapticsRoutine = base.StartCoroutine(this.AudioClipHapticPulses(forLeftController, clip, strength));
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000B0973 File Offset: 0x000AEB73
	public void StopHapticClip(bool forLeftController)
	{
		if (forLeftController)
		{
			if (this.leftHapticsRoutine != null)
			{
				base.StopCoroutine(this.leftHapticsRoutine);
				this.leftHapticsRoutine = null;
				return;
			}
		}
		else if (this.rightHapticsRoutine != null)
		{
			base.StopCoroutine(this.rightHapticsRoutine);
			this.rightHapticsRoutine = null;
		}
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000B09AF File Offset: 0x000AEBAF
	private IEnumerator AudioClipHapticPulses(bool forLeftController, AudioClip clip, float strength)
	{
		uint channel = 0U;
		int bufferSize = 8192;
		int sampleWindowSize = 256;
		float[] audioData;
		UnityEngine.XR.InputDevice device;
		if (forLeftController)
		{
			float[] array;
			if ((array = this.leftHapticsBuffer) == null)
			{
				array = (this.leftHapticsBuffer = new float[bufferSize]);
			}
			audioData = array;
			device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
		}
		else
		{
			float[] array2;
			if ((array2 = this.rightHapticsBuffer) == null)
			{
				array2 = (this.rightHapticsBuffer = new float[bufferSize]);
			}
			audioData = array2;
			device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
		}
		int sampleOffset = -bufferSize;
		float startTime = Time.time;
		float length = clip.length;
		float endTime = Time.time + length;
		float sampleRate = (float)clip.samples;
		while (Time.time <= endTime)
		{
			float num = (Time.time - startTime) / length;
			int num2 = (int)(sampleRate * num);
			if (Mathf.Max(num2 + sampleWindowSize - 1, audioData.Length - 1) >= sampleOffset + bufferSize)
			{
				clip.GetData(audioData, num2);
				sampleOffset = num2;
			}
			float num3 = 0f;
			int num4 = Mathf.Min(clip.samples - num2, sampleWindowSize);
			for (int i = 0; i < num4; i++)
			{
				float num5 = audioData[num2 - sampleOffset + i];
				num3 += num5 * num5;
			}
			float amplitude = Mathf.Clamp01(((num4 > 0) ? Mathf.Sqrt(num3 / (float)num4) : 0f) * strength);
			device.SendHapticImpulse(channel, amplitude, Time.fixedDeltaTime);
			yield return null;
		}
		if (forLeftController)
		{
			this.leftHapticsRoutine = null;
		}
		else
		{
			this.rightHapticsRoutine = null;
		}
		yield break;
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000B09D4 File Offset: 0x000AEBD4
	public void DoVibration(XRNode node, float amplitude, float duration)
	{
		UnityEngine.XR.InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(node);
		if (deviceAtXRNode.isValid)
		{
			deviceAtXRNode.SendHapticImpulse(0U, amplitude, duration);
		}
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000B09FC File Offset: 0x000AEBFC
	public void UpdateColor(float red, float green, float blue)
	{
		this.offlineVRRig.InitializeNoobMaterialLocal(red, green, blue);
		if (NetworkSystem.Instance != null && !NetworkSystem.Instance.InRoom)
		{
			this.offlineVRRig.mainSkin.sharedMaterial = this.offlineVRRig.materialsToChangeTo[0];
		}
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000B0A50 File Offset: 0x000AEC50
	protected void OnTriggerEnter(Collider other)
	{
		GorillaTriggerBox gorillaTriggerBox;
		if (other.TryGetComponent<GorillaTriggerBox>(out gorillaTriggerBox))
		{
			gorillaTriggerBox.OnBoxTriggered();
		}
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000B0A70 File Offset: 0x000AEC70
	protected void OnTriggerExit(Collider other)
	{
		GorillaTriggerBox gorillaTriggerBox;
		if (other.TryGetComponent<GorillaTriggerBox>(out gorillaTriggerBox))
		{
			gorillaTriggerBox.OnBoxExited();
		}
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000B0A90 File Offset: 0x000AEC90
	public void ShowCosmeticParticles(bool showParticles)
	{
		if (showParticles)
		{
			this.mainCamera.GetComponent<Camera>().cullingMask |= UnityLayer.GorillaCosmeticParticle.ToLayerMask();
			this.MirrorCameraCullingMask.value |= UnityLayer.GorillaCosmeticParticle.ToLayerMask();
			return;
		}
		this.mainCamera.GetComponent<Camera>().cullingMask &= ~UnityLayer.GorillaCosmeticParticle.ToLayerMask();
		this.MirrorCameraCullingMask.value &= ~UnityLayer.GorillaCosmeticParticle.ToLayerMask();
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000B0B11 File Offset: 0x000AED11
	public void ApplyStatusEffect(GorillaTagger.StatusEffect newStatus, float duration)
	{
		this.EndStatusEffect(this.currentStatus);
		this.currentStatus = newStatus;
		this.statusEndTime = Time.time + duration;
		switch (newStatus)
		{
		case GorillaTagger.StatusEffect.None:
		case GorillaTagger.StatusEffect.Slowed:
			break;
		case GorillaTagger.StatusEffect.Frozen:
			GTPlayer.Instance.disableMovement = true;
			break;
		default:
			return;
		}
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000B0B51 File Offset: 0x000AED51
	private void CheckEndStatusEffect()
	{
		if (Time.time > this.statusEndTime)
		{
			this.EndStatusEffect(this.currentStatus);
		}
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000B0B6C File Offset: 0x000AED6C
	private void EndStatusEffect(GorillaTagger.StatusEffect effectToEnd)
	{
		switch (effectToEnd)
		{
		case GorillaTagger.StatusEffect.None:
			break;
		case GorillaTagger.StatusEffect.Frozen:
			GTPlayer.Instance.disableMovement = false;
			this.currentStatus = GorillaTagger.StatusEffect.None;
			return;
		case GorillaTagger.StatusEffect.Slowed:
			this.currentStatus = GorillaTagger.StatusEffect.None;
			break;
		default:
			return;
		}
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000B0B9B File Offset: 0x000AED9B
	private float CalcSlideControl(float fps)
	{
		return Mathf.Pow(Mathf.Pow(1f - this.baseSlideControl, 120f), 1f / fps);
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000B0BBF File Offset: 0x000AEDBF
	public static void OnPlayerSpawned(Action action)
	{
		if (GorillaTagger._instance)
		{
			action();
			return;
		}
		GorillaTagger.onPlayerSpawnedRootCallback = (Action)Delegate.Combine(GorillaTagger.onPlayerSpawnedRootCallback, action);
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000B0BEC File Offset: 0x000AEDEC
	private void ProcessHandTapping(in bool leftHand, ref float lastTapTime, ref bool wasHandTouching, in int handMatIndex, in GorillaSurfaceOverride surfaceOverride, in RaycastHit handHitInfo, in Transform handFollower, in AudioSource handSlideSource, in GorillaVelocityTracker handVelocityTracker)
	{
		bool flag = GTPlayer.Instance.IsHandTouching(leftHand);
		if (GTPlayer.Instance.inOverlay)
		{
			wasHandTouching = flag;
			handSlideSource.GTStop();
			return;
		}
		if (GTPlayer.Instance.IsHandSliding(leftHand))
		{
			this.StartVibration(leftHand, this.tapHapticStrength / 5f, Time.fixedDeltaTime);
			if (!handSlideSource.isPlaying)
			{
				handSlideSource.GTPlay();
			}
		}
		else
		{
			handSlideSource.GTStop();
			if ((!wasHandTouching && flag) || (wasHandTouching && !flag))
			{
				bool flag2 = Time.time > lastTapTime + this.tapCoolDown;
				Tappable tappable = null;
				bool flag3 = surfaceOverride && surfaceOverride.TryGetComponent<Tappable>(out tappable);
				if (flag3 && tappable.overrideTapCooldown)
				{
					flag2 = tappable.CanTap(leftHand);
				}
				if (flag2)
				{
					lastTapTime = Time.time;
					GorillaAmbushManager gorillaAmbushManager = GameMode.ActiveGameMode as GorillaAmbushManager;
					if (gorillaAmbushManager != null && gorillaAmbushManager.IsInfected(NetworkSystem.Instance.LocalPlayer))
					{
						float sqrMagnitude = (handVelocityTracker.GetAverageVelocity(true, 0.03f, false) / GTPlayer.Instance.scale).sqrMagnitude;
						float sqrMagnitude2 = handVelocityTracker.GetAverageVelocity(false, 0.03f, false).sqrMagnitude;
						this.handTapVolume = ((sqrMagnitude > sqrMagnitude2) ? Mathf.Sqrt(sqrMagnitude) : Mathf.Sqrt(sqrMagnitude2));
						this.handTapVolume = Mathf.Clamp(this.handTapVolume, 0f, gorillaAmbushManager.crawlingSpeedForMaxVolume);
					}
					else
					{
						this.handTapVolume = this.cacheHandTapVolume;
					}
					if (surfaceOverride != null)
					{
						this.tempInt = surfaceOverride.overrideIndex;
						if (surfaceOverride.sendOnTapEvent)
						{
							BuilderPieceTappable builderPieceTappable;
							if (flag3)
							{
								tappable.OnTap(this.handTapVolume);
							}
							else if (surfaceOverride.TryGetComponent<BuilderPieceTappable>(out builderPieceTappable))
							{
								builderPieceTappable.OnTapLocal(this.handTapVolume);
							}
						}
						PlayerGameEvents.TapObject(surfaceOverride.name);
					}
					else
					{
						this.tempInt = handMatIndex;
					}
					GorillaFreezeTagManager gorillaFreezeTagManager = GameMode.ActiveGameMode as GorillaFreezeTagManager;
					if (gorillaFreezeTagManager != null && gorillaFreezeTagManager.IsFrozen(NetworkSystem.Instance.LocalPlayer))
					{
						this.tempInt = gorillaFreezeTagManager.GetFrozenHandTapAudioIndex();
					}
					this.StartVibration(leftHand, this.tapHapticStrength, this.tapHapticDuration);
					RaycastHit raycastHit = handHitInfo;
					this.tempHitDir = Vector3.Normalize(raycastHit.point - handFollower.position);
					this.offlineVRRig.OnHandTap(this.tempInt, leftHand, this.handTapVolume, this.tempHitDir);
					if (GameMode.ActiveGameMode != null)
					{
						GorillaGameManager activeGameMode = GameMode.ActiveGameMode;
						NetPlayer localPlayer = NetworkSystem.Instance.LocalPlayer;
						Tappable hitTappable = tappable;
						bool leftHand2 = leftHand;
						Vector3 averageVelocity = handVelocityTracker.GetAverageVelocity(true, 0.03f, false);
						raycastHit = handHitInfo;
						activeGameMode.HandleHandTap(localPlayer, hitTappable, leftHand2, averageVelocity, raycastHit.normal);
					}
					if (NetworkSystem.Instance.InRoom && this.myVRRig.IsNotNull() && this.myVRRig != null)
					{
						this.myVRRig.GetView.RPC("OnHandTapRPC", RpcTarget.Others, new object[]
						{
							this.tempInt,
							leftHand,
							this.handTapVolume,
							Utils.PackVector3ToLong(this.tempHitDir)
						});
					}
				}
			}
		}
		wasHandTouching = flag;
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000B0F2C File Offset: 0x000AF12C
	public void DebugDrawTagCasts(Color color)
	{
		float num = this.sphereCastRadius * GTPlayer.Instance.scale;
		this.DrawSphereCast(this.lastLeftHandPositionForTag, this.leftRaycastSweep.normalized, num, Mathf.Max(this.leftRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.headCollider.transform.position, this.leftHeadRaycastSweep.normalized, num, Mathf.Max(this.leftHeadRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.lastRightHandPositionForTag, this.rightRaycastSweep.normalized, num, Mathf.Max(this.rightRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.headCollider.transform.position, this.rightHeadRaycastSweep.normalized, num, Mathf.Max(this.rightHeadRaycastSweep.magnitude, num), color);
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x000B1007 File Offset: 0x000AF207
	private void DrawSphereCast(Vector3 start, Vector3 dir, float radius, float dist, Color color)
	{
		DebugUtil.DrawCapsule(start, start + dir * dist, radius, 16, 16, color, true, DebugUtil.Style.Wireframe);
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000B1026 File Offset: 0x000AF226
	private void RecoverMissingRefs()
	{
		if (!this.offlineVRRig)
		{
			this.RecoverMissingRefs_Asdf<AudioSource>(ref this.leftHandSlideSource, "leftHandSlideSource", "./**/Left Arm IK/SlideAudio");
			this.RecoverMissingRefs_Asdf<AudioSource>(ref this.rightHandSlideSource, "rightHandSlideSource", "./**/Right Arm IK/SlideAudio");
		}
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000B1064 File Offset: 0x000AF264
	private void RecoverMissingRefs_Asdf<T>(ref T objRef, string objFieldName, string recoveryPath) where T : Object
	{
		if (objRef)
		{
			return;
		}
		Transform transform;
		if (!this.offlineVRRig.transform.TryFindByPath(recoveryPath, out transform, false))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"`",
				objFieldName,
				"` reference missing and could not find by path: \"",
				recoveryPath,
				"\""
			}), this);
		}
		objRef = transform.GetComponentInChildren<T>();
		if (!objRef)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"`",
				objFieldName,
				"` reference is missing. Found transform with recover path, but did not find the component. Recover path: \"",
				recoveryPath,
				"\""
			}), this);
		}
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000B111A File Offset: 0x000AF31A
	public void GuidedRefInitialize()
	{
		GuidedRefHub.RegisterReceiverField<GorillaTagger>(this, "offlineVRRig", ref this.offlineVRRig_gRef);
		GuidedRefHub.ReceiverFullyRegistered<GorillaTagger>(this);
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x0600237E RID: 9086 RVA: 0x000B1133 File Offset: 0x000AF333
	// (set) Token: 0x0600237F RID: 9087 RVA: 0x000B113B File Offset: 0x000AF33B
	int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

	// Token: 0x06002380 RID: 9088 RVA: 0x000B1144 File Offset: 0x000AF344
	bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
	{
		if (this.offlineVRRig_gRef.fieldId == target.fieldId && this.offlineVRRig == null)
		{
			this.offlineVRRig = (target.targetMono.GuidedRefTargetObject as VRRig);
			return this.offlineVRRig != null;
		}
		return false;
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000023F4 File Offset: 0x000005F4
	void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
	{
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000023F4 File Offset: 0x000005F4
	void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
	{
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x0004316D File Offset: 0x0004136D
	Transform IGuidedRefMonoBehaviour.get_transform()
	{
		return base.transform;
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x00015DCD File Offset: 0x00013FCD
	int IGuidedRefObject.GetInstanceID()
	{
		return base.GetInstanceID();
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000B1244 File Offset: 0x000AF444
	[CompilerGenerated]
	private void <LateUpdate>g__TryTaggingAllHits|111_0(bool isBodyTag, bool isLeftHand, ref GorillaTagger.<>c__DisplayClass111_0 A_3)
	{
		for (int i = 0; i < this.nonAllocHits; i++)
		{
			if (this.nonAllocRaycastHits[i].collider.gameObject.activeSelf)
			{
				if (this.TryToTag(this.nonAllocRaycastHits[i], isBodyTag, out this.tryPlayer, out this.touchedPlayer))
				{
					this.otherPlayer = this.tryPlayer;
					A_3.bodyHit = isBodyTag;
					A_3.leftHandHit = isLeftHand;
					return;
				}
				if (this.touchedPlayer != null)
				{
					A_3.otherTouchedPlayer = this.touchedPlayer;
				}
			}
		}
	}

	// Token: 0x040026F5 RID: 9973
	[OnEnterPlay_SetNull]
	private static GorillaTagger _instance;

	// Token: 0x040026F6 RID: 9974
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x040026F7 RID: 9975
	public bool inCosmeticsRoom;

	// Token: 0x040026F8 RID: 9976
	public SphereCollider headCollider;

	// Token: 0x040026F9 RID: 9977
	public CapsuleCollider bodyCollider;

	// Token: 0x040026FA RID: 9978
	private Vector3 lastLeftHandPositionForTag;

	// Token: 0x040026FB RID: 9979
	private Vector3 lastRightHandPositionForTag;

	// Token: 0x040026FC RID: 9980
	private Vector3 lastBodyPositionForTag;

	// Token: 0x040026FD RID: 9981
	private Vector3 lastHeadPositionForTag;

	// Token: 0x040026FE RID: 9982
	public Transform rightHandTransform;

	// Token: 0x040026FF RID: 9983
	public Transform leftHandTransform;

	// Token: 0x04002700 RID: 9984
	public float hapticWaitSeconds = 0.05f;

	// Token: 0x04002701 RID: 9985
	public float handTapVolume = 0.1f;

	// Token: 0x04002702 RID: 9986
	public float tapCoolDown = 0.15f;

	// Token: 0x04002703 RID: 9987
	public float lastLeftTap;

	// Token: 0x04002704 RID: 9988
	public float lastRightTap;

	// Token: 0x04002705 RID: 9989
	public float tapHapticDuration = 0.05f;

	// Token: 0x04002706 RID: 9990
	public float tapHapticStrength = 0.5f;

	// Token: 0x04002707 RID: 9991
	public float tagHapticDuration = 0.15f;

	// Token: 0x04002708 RID: 9992
	public float tagHapticStrength = 1f;

	// Token: 0x04002709 RID: 9993
	public float taggedHapticDuration = 0.35f;

	// Token: 0x0400270A RID: 9994
	public float taggedHapticStrength = 1f;

	// Token: 0x0400270B RID: 9995
	private bool leftHandTouching;

	// Token: 0x0400270C RID: 9996
	private bool rightHandTouching;

	// Token: 0x0400270D RID: 9997
	public float taggedTime;

	// Token: 0x0400270E RID: 9998
	public float tagCooldown;

	// Token: 0x0400270F RID: 9999
	public float slowCooldown = 3f;

	// Token: 0x04002710 RID: 10000
	public VRRig offlineVRRig;

	// Token: 0x04002711 RID: 10001
	[FormerlySerializedAs("offlineVRRig_guidedRef")]
	public GuidedRefReceiverFieldInfo offlineVRRig_gRef = new GuidedRefReceiverFieldInfo(false);

	// Token: 0x04002712 RID: 10002
	public GameObject thirdPersonCamera;

	// Token: 0x04002713 RID: 10003
	public GameObject mainCamera;

	// Token: 0x04002714 RID: 10004
	public bool testTutorial;

	// Token: 0x04002715 RID: 10005
	public bool disableTutorial;

	// Token: 0x04002716 RID: 10006
	public bool frameRateUpdated;

	// Token: 0x04002717 RID: 10007
	public GameObject leftHandTriggerCollider;

	// Token: 0x04002718 RID: 10008
	public GameObject rightHandTriggerCollider;

	// Token: 0x04002719 RID: 10009
	public AudioSource leftHandSlideSource;

	// Token: 0x0400271A RID: 10010
	public AudioSource rightHandSlideSource;

	// Token: 0x0400271B RID: 10011
	public AudioSource bodySlideSource;

	// Token: 0x0400271C RID: 10012
	public bool overrideNotInFocus;

	// Token: 0x0400271E RID: 10014
	private Vector3 leftRaycastSweep;

	// Token: 0x0400271F RID: 10015
	private Vector3 leftHeadRaycastSweep;

	// Token: 0x04002720 RID: 10016
	private Vector3 rightRaycastSweep;

	// Token: 0x04002721 RID: 10017
	private Vector3 rightHeadRaycastSweep;

	// Token: 0x04002722 RID: 10018
	private Vector3 headRaycastSweep;

	// Token: 0x04002723 RID: 10019
	private Vector3 bodyRaycastSweep;

	// Token: 0x04002724 RID: 10020
	private UnityEngine.XR.InputDevice rightDevice;

	// Token: 0x04002725 RID: 10021
	private UnityEngine.XR.InputDevice leftDevice;

	// Token: 0x04002726 RID: 10022
	private bool primaryButtonPressRight;

	// Token: 0x04002727 RID: 10023
	private bool secondaryButtonPressRight;

	// Token: 0x04002728 RID: 10024
	private bool primaryButtonPressLeft;

	// Token: 0x04002729 RID: 10025
	private bool secondaryButtonPressLeft;

	// Token: 0x0400272A RID: 10026
	private RaycastHit hitInfo;

	// Token: 0x0400272B RID: 10027
	public NetPlayer otherPlayer;

	// Token: 0x0400272C RID: 10028
	private NetPlayer tryPlayer;

	// Token: 0x0400272D RID: 10029
	private NetPlayer touchedPlayer;

	// Token: 0x0400272E RID: 10030
	private Vector3 topVector;

	// Token: 0x0400272F RID: 10031
	private Vector3 bottomVector;

	// Token: 0x04002730 RID: 10032
	private Vector3 bodyVector;

	// Token: 0x04002731 RID: 10033
	private Vector3 tempHitDir;

	// Token: 0x04002732 RID: 10034
	private int tempInt;

	// Token: 0x04002733 RID: 10035
	private UnityEngine.XR.InputDevice inputDevice;

	// Token: 0x04002734 RID: 10036
	private bool wasInOverlay;

	// Token: 0x04002735 RID: 10037
	private PhotonView tempView;

	// Token: 0x04002736 RID: 10038
	private NetPlayer tempCreator;

	// Token: 0x04002737 RID: 10039
	private float cacheHandTapVolume;

	// Token: 0x04002738 RID: 10040
	public GorillaTagger.StatusEffect currentStatus;

	// Token: 0x04002739 RID: 10041
	public float statusStartTime;

	// Token: 0x0400273A RID: 10042
	public float statusEndTime;

	// Token: 0x0400273B RID: 10043
	private float refreshRate;

	// Token: 0x0400273C RID: 10044
	private float baseSlideControl;

	// Token: 0x0400273D RID: 10045
	private int gorillaTagColliderLayerMask;

	// Token: 0x0400273E RID: 10046
	private RaycastHit[] nonAllocRaycastHits = new RaycastHit[30];

	// Token: 0x0400273F RID: 10047
	private int nonAllocHits;

	// Token: 0x04002741 RID: 10049
	private bool xrSubsystemIsActive;

	// Token: 0x04002742 RID: 10050
	public string loadedDeviceName = "";

	// Token: 0x04002743 RID: 10051
	[SerializeField]
	private LayerMask BaseMirrorCameraCullingMask;

	// Token: 0x04002744 RID: 10052
	public Watchable<int> MirrorCameraCullingMask;

	// Token: 0x04002745 RID: 10053
	private float[] leftHapticsBuffer;

	// Token: 0x04002746 RID: 10054
	private float[] rightHapticsBuffer;

	// Token: 0x04002747 RID: 10055
	private Coroutine leftHapticsRoutine;

	// Token: 0x04002748 RID: 10056
	private Coroutine rightHapticsRoutine;

	// Token: 0x04002749 RID: 10057
	private Callback<GameOverlayActivated_t> gameOverlayActivatedCb;

	// Token: 0x0400274A RID: 10058
	private bool isGameOverlayActive;

	// Token: 0x0400274B RID: 10059
	private float? tagRadiusOverride;

	// Token: 0x0400274C RID: 10060
	private int tagRadiusOverrideFrame = -1;

	// Token: 0x0400274D RID: 10061
	private static Action onPlayerSpawnedRootCallback;

	// Token: 0x02000594 RID: 1428
	public enum StatusEffect
	{
		// Token: 0x04002750 RID: 10064
		None,
		// Token: 0x04002751 RID: 10065
		Frozen,
		// Token: 0x04002752 RID: 10066
		Slowed,
		// Token: 0x04002753 RID: 10067
		Dead,
		// Token: 0x04002754 RID: 10068
		Infected,
		// Token: 0x04002755 RID: 10069
		It
	}
}
