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

// Token: 0x020005A0 RID: 1440
public class GorillaTagger : MonoBehaviour, IGuidedRefReceiverMono, IGuidedRefMonoBehaviour, IGuidedRefObject
{
	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000481E5 File Offset: 0x000463E5
	public static GorillaTagger Instance
	{
		get
		{
			return GorillaTagger._instance;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000481EC File Offset: 0x000463EC
	public NetworkView myVRRig
	{
		get
		{
			return this.offlineVRRig.netView;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060023B2 RID: 9138 RVA: 0x000481F9 File Offset: 0x000463F9
	internal VRRigSerializer rigSerializer
	{
		get
		{
			return this.offlineVRRig.rigSerializer;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060023B3 RID: 9139 RVA: 0x00048206 File Offset: 0x00046406
	// (set) Token: 0x060023B4 RID: 9140 RVA: 0x0004820E File Offset: 0x0004640E
	public Rigidbody rigidbody { get; private set; }

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060023B5 RID: 9141 RVA: 0x00048217 File Offset: 0x00046417
	public float DefaultHandTapVolume
	{
		get
		{
			return this.cacheHandTapVolume;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060023B6 RID: 9142 RVA: 0x0004821F File Offset: 0x0004641F
	// (set) Token: 0x060023B7 RID: 9143 RVA: 0x00048227 File Offset: 0x00046427
	public Recorder myRecorder { get; private set; }

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060023B8 RID: 9144 RVA: 0x00048230 File Offset: 0x00046430
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

	// Token: 0x060023B9 RID: 9145 RVA: 0x00048250 File Offset: 0x00046450
	public void SetTagRadiusOverrideThisFrame(float radius)
	{
		this.tagRadiusOverride = new float?(radius);
		this.tagRadiusOverrideFrame = Time.frameCount;
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000FE58C File Offset: 0x000FC78C
	protected void Awake()
	{
		this.GuidedRefInitialize();
		this.RecoverMissingRefs();
		this.MirrorCameraCullingMask = new Watchable<int>(this.BaseMirrorCameraCullingMask);
		if (GorillaTagger._instance != null && GorillaTagger._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
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
			PlayerPrefs.SetFloat("redValue", UnityEngine.Random.value);
			PlayerPrefs.SetFloat("greenValue", UnityEngine.Random.value);
			PlayerPrefs.SetFloat("blueValue", UnityEngine.Random.value);
			PlayerPrefs.Save();
		}
		else
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
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

	// Token: 0x060023BB RID: 9147 RVA: 0x00048269 File Offset: 0x00046469
	protected void OnDestroy()
	{
		if (GorillaTagger._instance == this)
		{
			GorillaTagger._instance = null;
			GorillaTagger.hasInstance = false;
		}
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x000FE788 File Offset: 0x000FC988
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

	// Token: 0x060023BD RID: 9149 RVA: 0x000FE7FC File Offset: 0x000FC9FC
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

	// Token: 0x060023BE RID: 9150 RVA: 0x00048284 File Offset: 0x00046484
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		this.isGameOverlayActive = (pCallback.m_bActive > 0);
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000FE944 File Offset: 0x000FCB44
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
			UnityEngine.Object.Destroy(OVRManager.instance.gameObject);
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

	// Token: 0x060023C0 RID: 9152 RVA: 0x000FF790 File Offset: 0x000FD990
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

	// Token: 0x060023C1 RID: 9153 RVA: 0x00048295 File Offset: 0x00046495
	public void StartVibration(bool forLeftController, float amplitude, float duration)
	{
		base.StartCoroutine(this.HapticPulses(forLeftController, amplitude, duration));
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000482A7 File Offset: 0x000464A7
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

	// Token: 0x060023C3 RID: 9155 RVA: 0x000FF8E0 File Offset: 0x000FDAE0
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

	// Token: 0x060023C4 RID: 9156 RVA: 0x000482CB File Offset: 0x000464CB
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

	// Token: 0x060023C5 RID: 9157 RVA: 0x00048307 File Offset: 0x00046507
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

	// Token: 0x060023C6 RID: 9158 RVA: 0x000FF944 File Offset: 0x000FDB44
	public void DoVibration(XRNode node, float amplitude, float duration)
	{
		UnityEngine.XR.InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(node);
		if (deviceAtXRNode.isValid)
		{
			deviceAtXRNode.SendHapticImpulse(0U, amplitude, duration);
		}
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000FF96C File Offset: 0x000FDB6C
	public void UpdateColor(float red, float green, float blue)
	{
		this.offlineVRRig.InitializeNoobMaterialLocal(red, green, blue);
		if (NetworkSystem.Instance != null && !NetworkSystem.Instance.InRoom)
		{
			this.offlineVRRig.mainSkin.sharedMaterial = this.offlineVRRig.materialsToChangeTo[0];
		}
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000FF9C0 File Offset: 0x000FDBC0
	protected void OnTriggerEnter(Collider other)
	{
		GorillaTriggerBox gorillaTriggerBox;
		if (other.TryGetComponent<GorillaTriggerBox>(out gorillaTriggerBox))
		{
			gorillaTriggerBox.OnBoxTriggered();
		}
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000FF9E0 File Offset: 0x000FDBE0
	protected void OnTriggerExit(Collider other)
	{
		GorillaTriggerBox gorillaTriggerBox;
		if (other.TryGetComponent<GorillaTriggerBox>(out gorillaTriggerBox))
		{
			gorillaTriggerBox.OnBoxExited();
		}
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000FFA00 File Offset: 0x000FDC00
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

	// Token: 0x060023CB RID: 9163 RVA: 0x0004832B File Offset: 0x0004652B
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

	// Token: 0x060023CC RID: 9164 RVA: 0x0004836B File Offset: 0x0004656B
	private void CheckEndStatusEffect()
	{
		if (Time.time > this.statusEndTime)
		{
			this.EndStatusEffect(this.currentStatus);
		}
	}

	// Token: 0x060023CD RID: 9165 RVA: 0x00048386 File Offset: 0x00046586
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

	// Token: 0x060023CE RID: 9166 RVA: 0x000483B5 File Offset: 0x000465B5
	private float CalcSlideControl(float fps)
	{
		return Mathf.Pow(Mathf.Pow(1f - this.baseSlideControl, 120f), 1f / fps);
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000483D9 File Offset: 0x000465D9
	public static void OnPlayerSpawned(Action action)
	{
		if (GorillaTagger._instance)
		{
			action();
			return;
		}
		GorillaTagger.onPlayerSpawnedRootCallback = (Action)Delegate.Combine(GorillaTagger.onPlayerSpawnedRootCallback, action);
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000FFA84 File Offset: 0x000FDC84
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

	// Token: 0x060023D1 RID: 9169 RVA: 0x000FFDC4 File Offset: 0x000FDFC4
	public void DebugDrawTagCasts(Color color)
	{
		float num = this.sphereCastRadius * GTPlayer.Instance.scale;
		this.DrawSphereCast(this.lastLeftHandPositionForTag, this.leftRaycastSweep.normalized, num, Mathf.Max(this.leftRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.headCollider.transform.position, this.leftHeadRaycastSweep.normalized, num, Mathf.Max(this.leftHeadRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.lastRightHandPositionForTag, this.rightRaycastSweep.normalized, num, Mathf.Max(this.rightRaycastSweep.magnitude, num), color);
		this.DrawSphereCast(this.headCollider.transform.position, this.rightHeadRaycastSweep.normalized, num, Mathf.Max(this.rightHeadRaycastSweep.magnitude, num), color);
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x00048403 File Offset: 0x00046603
	private void DrawSphereCast(Vector3 start, Vector3 dir, float radius, float dist, Color color)
	{
		DebugUtil.DrawCapsule(start, start + dir * dist, radius, 16, 16, color, true, DebugUtil.Style.Wireframe);
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x00048422 File Offset: 0x00046622
	private void RecoverMissingRefs()
	{
		if (!this.offlineVRRig)
		{
			this.RecoverMissingRefs_Asdf<AudioSource>(ref this.leftHandSlideSource, "leftHandSlideSource", "./**/Left Arm IK/SlideAudio");
			this.RecoverMissingRefs_Asdf<AudioSource>(ref this.rightHandSlideSource, "rightHandSlideSource", "./**/Right Arm IK/SlideAudio");
		}
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000FFEA0 File Offset: 0x000FE0A0
	private void RecoverMissingRefs_Asdf<T>(ref T objRef, string objFieldName, string recoveryPath) where T : UnityEngine.Object
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

	// Token: 0x060023D5 RID: 9173 RVA: 0x0004845D File Offset: 0x0004665D
	public void GuidedRefInitialize()
	{
		GuidedRefHub.RegisterReceiverField<GorillaTagger>(this, "offlineVRRig", ref this.offlineVRRig_gRef);
		GuidedRefHub.ReceiverFullyRegistered<GorillaTagger>(this);
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060023D6 RID: 9174 RVA: 0x00048476 File Offset: 0x00046676
	// (set) Token: 0x060023D7 RID: 9175 RVA: 0x0004847E File Offset: 0x0004667E
	int IGuidedRefReceiverMono.GuidedRefsWaitingToResolveCount { get; set; }

	// Token: 0x060023D8 RID: 9176 RVA: 0x000FFF58 File Offset: 0x000FE158
	bool IGuidedRefReceiverMono.GuidedRefTryResolveReference(GuidedRefTryResolveInfo target)
	{
		if (this.offlineVRRig_gRef.fieldId == target.fieldId && this.offlineVRRig == null)
		{
			this.offlineVRRig = (target.targetMono.GuidedRefTargetObject as VRRig);
			return this.offlineVRRig != null;
		}
		return false;
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x00030607 File Offset: 0x0002E807
	void IGuidedRefReceiverMono.OnAllGuidedRefsResolved()
	{
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x00030607 File Offset: 0x0002E807
	void IGuidedRefReceiverMono.OnGuidedRefTargetDestroyed(int fieldId)
	{
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x00039243 File Offset: 0x00037443
	Transform IGuidedRefMonoBehaviour.get_transform()
	{
		return base.transform;
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x00032CAE File Offset: 0x00030EAE
	int IGuidedRefObject.GetInstanceID()
	{
		return base.GetInstanceID();
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x00100058 File Offset: 0x000FE258
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

	// Token: 0x0400274A RID: 10058
	[OnEnterPlay_SetNull]
	private static GorillaTagger _instance;

	// Token: 0x0400274B RID: 10059
	[OnEnterPlay_Set(false)]
	public static bool hasInstance;

	// Token: 0x0400274C RID: 10060
	public bool inCosmeticsRoom;

	// Token: 0x0400274D RID: 10061
	public SphereCollider headCollider;

	// Token: 0x0400274E RID: 10062
	public CapsuleCollider bodyCollider;

	// Token: 0x0400274F RID: 10063
	private Vector3 lastLeftHandPositionForTag;

	// Token: 0x04002750 RID: 10064
	private Vector3 lastRightHandPositionForTag;

	// Token: 0x04002751 RID: 10065
	private Vector3 lastBodyPositionForTag;

	// Token: 0x04002752 RID: 10066
	private Vector3 lastHeadPositionForTag;

	// Token: 0x04002753 RID: 10067
	public Transform rightHandTransform;

	// Token: 0x04002754 RID: 10068
	public Transform leftHandTransform;

	// Token: 0x04002755 RID: 10069
	public float hapticWaitSeconds = 0.05f;

	// Token: 0x04002756 RID: 10070
	public float handTapVolume = 0.1f;

	// Token: 0x04002757 RID: 10071
	public float tapCoolDown = 0.15f;

	// Token: 0x04002758 RID: 10072
	public float lastLeftTap;

	// Token: 0x04002759 RID: 10073
	public float lastRightTap;

	// Token: 0x0400275A RID: 10074
	public float tapHapticDuration = 0.05f;

	// Token: 0x0400275B RID: 10075
	public float tapHapticStrength = 0.5f;

	// Token: 0x0400275C RID: 10076
	public float tagHapticDuration = 0.15f;

	// Token: 0x0400275D RID: 10077
	public float tagHapticStrength = 1f;

	// Token: 0x0400275E RID: 10078
	public float taggedHapticDuration = 0.35f;

	// Token: 0x0400275F RID: 10079
	public float taggedHapticStrength = 1f;

	// Token: 0x04002760 RID: 10080
	private bool leftHandTouching;

	// Token: 0x04002761 RID: 10081
	private bool rightHandTouching;

	// Token: 0x04002762 RID: 10082
	public float taggedTime;

	// Token: 0x04002763 RID: 10083
	public float tagCooldown;

	// Token: 0x04002764 RID: 10084
	public float slowCooldown = 3f;

	// Token: 0x04002765 RID: 10085
	public VRRig offlineVRRig;

	// Token: 0x04002766 RID: 10086
	[FormerlySerializedAs("offlineVRRig_guidedRef")]
	public GuidedRefReceiverFieldInfo offlineVRRig_gRef = new GuidedRefReceiverFieldInfo(false);

	// Token: 0x04002767 RID: 10087
	public GameObject thirdPersonCamera;

	// Token: 0x04002768 RID: 10088
	public GameObject mainCamera;

	// Token: 0x04002769 RID: 10089
	public bool testTutorial;

	// Token: 0x0400276A RID: 10090
	public bool disableTutorial;

	// Token: 0x0400276B RID: 10091
	public bool frameRateUpdated;

	// Token: 0x0400276C RID: 10092
	public GameObject leftHandTriggerCollider;

	// Token: 0x0400276D RID: 10093
	public GameObject rightHandTriggerCollider;

	// Token: 0x0400276E RID: 10094
	public AudioSource leftHandSlideSource;

	// Token: 0x0400276F RID: 10095
	public AudioSource rightHandSlideSource;

	// Token: 0x04002770 RID: 10096
	public AudioSource bodySlideSource;

	// Token: 0x04002771 RID: 10097
	public bool overrideNotInFocus;

	// Token: 0x04002773 RID: 10099
	private Vector3 leftRaycastSweep;

	// Token: 0x04002774 RID: 10100
	private Vector3 leftHeadRaycastSweep;

	// Token: 0x04002775 RID: 10101
	private Vector3 rightRaycastSweep;

	// Token: 0x04002776 RID: 10102
	private Vector3 rightHeadRaycastSweep;

	// Token: 0x04002777 RID: 10103
	private Vector3 headRaycastSweep;

	// Token: 0x04002778 RID: 10104
	private Vector3 bodyRaycastSweep;

	// Token: 0x04002779 RID: 10105
	private UnityEngine.XR.InputDevice rightDevice;

	// Token: 0x0400277A RID: 10106
	private UnityEngine.XR.InputDevice leftDevice;

	// Token: 0x0400277B RID: 10107
	private bool primaryButtonPressRight;

	// Token: 0x0400277C RID: 10108
	private bool secondaryButtonPressRight;

	// Token: 0x0400277D RID: 10109
	private bool primaryButtonPressLeft;

	// Token: 0x0400277E RID: 10110
	private bool secondaryButtonPressLeft;

	// Token: 0x0400277F RID: 10111
	private RaycastHit hitInfo;

	// Token: 0x04002780 RID: 10112
	public NetPlayer otherPlayer;

	// Token: 0x04002781 RID: 10113
	private NetPlayer tryPlayer;

	// Token: 0x04002782 RID: 10114
	private NetPlayer touchedPlayer;

	// Token: 0x04002783 RID: 10115
	private Vector3 topVector;

	// Token: 0x04002784 RID: 10116
	private Vector3 bottomVector;

	// Token: 0x04002785 RID: 10117
	private Vector3 bodyVector;

	// Token: 0x04002786 RID: 10118
	private Vector3 tempHitDir;

	// Token: 0x04002787 RID: 10119
	private int tempInt;

	// Token: 0x04002788 RID: 10120
	private UnityEngine.XR.InputDevice inputDevice;

	// Token: 0x04002789 RID: 10121
	private bool wasInOverlay;

	// Token: 0x0400278A RID: 10122
	private PhotonView tempView;

	// Token: 0x0400278B RID: 10123
	private NetPlayer tempCreator;

	// Token: 0x0400278C RID: 10124
	private float cacheHandTapVolume;

	// Token: 0x0400278D RID: 10125
	public GorillaTagger.StatusEffect currentStatus;

	// Token: 0x0400278E RID: 10126
	public float statusStartTime;

	// Token: 0x0400278F RID: 10127
	public float statusEndTime;

	// Token: 0x04002790 RID: 10128
	private float refreshRate;

	// Token: 0x04002791 RID: 10129
	private float baseSlideControl;

	// Token: 0x04002792 RID: 10130
	private int gorillaTagColliderLayerMask;

	// Token: 0x04002793 RID: 10131
	private RaycastHit[] nonAllocRaycastHits = new RaycastHit[30];

	// Token: 0x04002794 RID: 10132
	private int nonAllocHits;

	// Token: 0x04002796 RID: 10134
	private bool xrSubsystemIsActive;

	// Token: 0x04002797 RID: 10135
	public string loadedDeviceName = "";

	// Token: 0x04002798 RID: 10136
	[SerializeField]
	private LayerMask BaseMirrorCameraCullingMask;

	// Token: 0x04002799 RID: 10137
	public Watchable<int> MirrorCameraCullingMask;

	// Token: 0x0400279A RID: 10138
	private float[] leftHapticsBuffer;

	// Token: 0x0400279B RID: 10139
	private float[] rightHapticsBuffer;

	// Token: 0x0400279C RID: 10140
	private Coroutine leftHapticsRoutine;

	// Token: 0x0400279D RID: 10141
	private Coroutine rightHapticsRoutine;

	// Token: 0x0400279E RID: 10142
	private Callback<GameOverlayActivated_t> gameOverlayActivatedCb;

	// Token: 0x0400279F RID: 10143
	private bool isGameOverlayActive;

	// Token: 0x040027A0 RID: 10144
	private float? tagRadiusOverride;

	// Token: 0x040027A1 RID: 10145
	private int tagRadiusOverrideFrame = -1;

	// Token: 0x040027A2 RID: 10146
	private static Action onPlayerSpawnedRootCallback;

	// Token: 0x020005A1 RID: 1441
	public enum StatusEffect
	{
		// Token: 0x040027A5 RID: 10149
		None,
		// Token: 0x040027A6 RID: 10150
		Frozen,
		// Token: 0x040027A7 RID: 10151
		Slowed,
		// Token: 0x040027A8 RID: 10152
		Dead,
		// Token: 0x040027A9 RID: 10153
		Infected,
		// Token: 0x040027AA RID: 10154
		It
	}
}
