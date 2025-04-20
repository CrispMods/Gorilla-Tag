using System;
using Fusion;
using GorillaExtensions;
using GorillaGameModes;
using GorillaLocomotion;
using GorillaTagScripts;
using Photon.Pun;
using Photon.Voice.Fusion;
using Photon.Voice.PUN;
using UnityEngine;

// Token: 0x02000557 RID: 1367
[NetworkBehaviourWeaved(35)]
internal class VRRigSerializer : GorillaWrappedSerializer, IFXContextParems<HandTapArgs>, IFXContextParems<GeoSoundArg>
{
	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06002156 RID: 8534 RVA: 0x000469C1 File Offset: 0x00044BC1
	// (set) Token: 0x06002157 RID: 8535 RVA: 0x000469EB File Offset: 0x00044BEB
	[Networked]
	[NetworkedWeaved(0, 17)]
	public unsafe NetworkString<_16> nickName
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.nickName. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(NetworkString<_16>*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.nickName. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(NetworkString<_16>*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06002158 RID: 8536 RVA: 0x00046A16 File Offset: 0x00044C16
	// (set) Token: 0x06002159 RID: 8537 RVA: 0x00046A44 File Offset: 0x00044C44
	[Networked]
	[NetworkedWeaved(17, 17)]
	public unsafe NetworkString<_16> defaultName
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.defaultName. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(NetworkString<_16>*)(this.Ptr + 17);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.defaultName. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(NetworkString<_16>*)(this.Ptr + 17) = value;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x0600215A RID: 8538 RVA: 0x00046A73 File Offset: 0x00044C73
	// (set) Token: 0x0600215B RID: 8539 RVA: 0x00046AA1 File Offset: 0x00044CA1
	[Networked]
	[NetworkedWeaved(34, 1)]
	public bool tutorialComplete
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.tutorialComplete. Networked properties can only be accessed when Spawned() has been called.");
			}
			return ReadWriteUtilsForWeaver.ReadBoolean(this.Ptr + 34);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing VRRigSerializer.tutorialComplete. Networked properties can only be accessed when Spawned() has been called.");
			}
			ReadWriteUtilsForWeaver.WriteBoolean(this.Ptr + 34, value);
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x0600215C RID: 8540 RVA: 0x00046AD0 File Offset: 0x00044CD0
	private PhotonVoiceView Voice
	{
		get
		{
			return this.voiceView;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x0600215D RID: 8541 RVA: 0x00046AD8 File Offset: 0x00044CD8
	public VRRig VRRig
	{
		get
		{
			return this.vrrig;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x0600215E RID: 8542 RVA: 0x00046AE0 File Offset: 0x00044CE0
	public FXSystemSettings settings
	{
		get
		{
			return this.vrrig.fxSettings;
		}
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x000F57E8 File Offset: 0x000F39E8
	protected override bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetObject = null;
		outTargetType = null;
		NetPlayer player = NetworkSystem.Instance.GetPlayer(wrappedInfo.senderID);
		if (this.netView.IsRoomView)
		{
			if (player != null)
			{
				GorillaNot.instance.SendReport("creating rigs as room objects", player.UserId, player.NickName);
			}
			return false;
		}
		if (NetworkSystem.Instance.IsObjectRoomObject(base.gameObject))
		{
			NetPlayer player2 = NetworkSystem.Instance.GetPlayer(wrappedInfo.senderID);
			if (player2 != null)
			{
				Debug.LogWarning("creating rigs as room objects " + player2.UserId + " " + player2.NickName);
				GorillaNot.instance.SendReport("creating rigs as room objects", player2.UserId, player2.NickName);
			}
			return false;
		}
		if (player != this.netView.Owner)
		{
			GorillaNot.instance.SendReport("creating rigs for someone else", player.UserId, player.NickName);
			return false;
		}
		if (VRRigCache.Instance.TryGetVrrig(player, out this.rigContainer))
		{
			outTargetObject = this.rigContainer.gameObject;
			outTargetType = typeof(VRRig);
			this.vrrig = this.rigContainer.Rig;
			return true;
		}
		return false;
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000F5910 File Offset: 0x000F3B10
	protected override void OnSuccesfullySpawned(PhotonMessageInfoWrapped info)
	{
		bool initialized = this.rigContainer.Initialized;
		this.rigContainer.InitializeNetwork(this.netView, this.Voice, this);
		this.networkSpeaker.SetParent(this.rigContainer.SpeakerHead, false);
		base.transform.SetParent(VRRigCache.Instance.NetworkParent, true);
		this.netView.GetView.AddCallbackTarget(this);
		if (!initialized)
		{
			object[] instantiationData = info.punInfo.photonView.InstantiationData;
			float red = 0f;
			float green = 0f;
			float blue = 0f;
			if (instantiationData != null && instantiationData.Length == 3)
			{
				object obj = instantiationData[0];
				if (obj is float)
				{
					float value = (float)obj;
					obj = instantiationData[1];
					if (obj is float)
					{
						float value2 = (float)obj;
						obj = instantiationData[2];
						if (obj is float)
						{
							float value3 = (float)obj;
							red = value.ClampSafe(0f, 1f);
							green = value2.ClampSafe(0f, 1f);
							blue = value3.ClampSafe(0f, 1f);
						}
					}
				}
			}
			this.vrrig.InitializeNoobMaterialLocal(red, green, blue);
		}
		NetworkSystem.Instance.IsObjectLocallyOwned(base.gameObject);
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x00030607 File Offset: 0x0002E807
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x00046AED File Offset: 0x00044CED
	protected override void OnBeforeDespawn()
	{
		this.CleanUp(true);
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000F5A4C File Offset: 0x000F3C4C
	private void CleanUp(bool netDestroy)
	{
		if (!this.successfullInstantiate)
		{
			return;
		}
		this.successfullInstantiate = false;
		if (this.vrrig != null)
		{
			if (!NetworkSystem.Instance.InRoom)
			{
				if (this.vrrig.isOfflineVRRig)
				{
					this.vrrig.ChangeMaterialLocal(0);
				}
			}
			else if (this.vrrig.isOfflineVRRig)
			{
				NetworkSystem.Instance.NetDestroy(base.gameObject);
			}
			if (this.vrrig.netView == this.netView)
			{
				this.vrrig.netView = null;
			}
			if (this.vrrig.rigSerializer == this)
			{
				this.vrrig.rigSerializer = null;
			}
		}
		if (this.networkSpeaker != null)
		{
			if (netDestroy)
			{
				this.networkSpeaker.SetParent(base.transform, false);
			}
			else
			{
				this.networkSpeaker.SetParent(null);
			}
			this.networkSpeaker.gameObject.SetActive(false);
		}
		this.vrrig = null;
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x00046AF6 File Offset: 0x00044CF6
	private void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		this.CleanUp(false);
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x00046B05 File Offset: 0x00044D05
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		if (this.networkSpeaker != null && this.networkSpeaker.parent != base.transform)
		{
			UnityEngine.Object.Destroy(this.networkSpeaker.gameObject);
		}
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x00046B43 File Offset: 0x00044D43
	[PunRPC]
	public void RPC_InitializeNoobMaterial(float red, float green, float blue, PhotonMessageInfo info)
	{
		this.InitializeNoobMaterialShared(red, green, blue, info);
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x00046B55 File Offset: 0x00044D55
	[PunRPC]
	public void RPC_RequestCosmetics(PhotonMessageInfo info)
	{
		this.RequestCosmeticsShared(info);
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x00046B63 File Offset: 0x00044D63
	[PunRPC]
	public void RPC_PlayDrum(int drumIndex, float drumVolume, PhotonMessageInfo info)
	{
		this.PlayDrumShared(drumIndex, drumVolume, info);
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x00046B73 File Offset: 0x00044D73
	[PunRPC]
	public void RPC_PlaySelfOnlyInstrument(int selfOnlyIndex, int noteIndex, float instrumentVol, PhotonMessageInfo info)
	{
		this.PlaySelfOnlyInstrumentShared(selfOnlyIndex, noteIndex, instrumentVol, info);
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x00046B85 File Offset: 0x00044D85
	[PunRPC]
	public void RPC_PlayHandTap(int soundIndex, bool isLeftHand, float tapVolume, PhotonMessageInfo info = default(PhotonMessageInfo))
	{
		this.PlayHandTapShared(soundIndex, isLeftHand, tapVolume, info);
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x00046B97 File Offset: 0x00044D97
	public void RPC_UpdateNativeSize(float value, PhotonMessageInfo info = default(PhotonMessageInfo))
	{
		this.UpdateNativeSizeShared(value, info);
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x00030607 File Offset: 0x0002E807
	public void RPC_UpdateCosmetics(string[] currentItems, PhotonMessageInfo info)
	{
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x00030607 File Offset: 0x0002E807
	public void RPC_UpdateCosmeticsWithTryon(string[] currentItems, string[] tryOnItems, PhotonMessageInfo info)
	{
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x00046BA6 File Offset: 0x00044DA6
	[PunRPC]
	public void RPC_UpdateCosmeticsWithTryonPacked(int[] currentItemsPacked, int[] tryOnItemsPacked, PhotonMessageInfo info)
	{
		this.UpdateCosmeticsWithTryonShared(currentItemsPacked, tryOnItemsPacked, info);
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x00046BB6 File Offset: 0x00044DB6
	[PunRPC]
	public void RPC_HideAllCosmetics(PhotonMessageInfo info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.HideAllCosmetics(info);
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x00046BC9 File Offset: 0x00044DC9
	[PunRPC]
	public void RPC_PlaySplashEffect(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, PhotonMessageInfo info)
	{
		this.PlaySplashEffectShared(splashPosition, splashRotation, splashScale, boundingRadius, bigSplash, enteringWater, info);
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x00046BE1 File Offset: 0x00044DE1
	[PunRPC]
	public void RPC_PlayGeodeEffect(Vector3 hitPosition, PhotonMessageInfo info)
	{
		this.PlayGeodeEffectShared(hitPosition, info);
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x00046BF0 File Offset: 0x00044DF0
	[PunRPC]
	public void EnableNonCosmeticHandItemRPC(bool enable, bool isLeftHand, PhotonMessageInfo info)
	{
		this.EnableNonCosmeticHandItemShared(enable, isLeftHand, info);
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x00046C00 File Offset: 0x00044E00
	[PunRPC]
	public void OnHandTapRPC(int surfaceIndex, bool leftHanded, float handSpeed, long packedDir, PhotonMessageInfo info)
	{
		this.OnHandTapRPCShared(surfaceIndex, leftHanded, handSpeed, packedDir, info);
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x00046C14 File Offset: 0x00044E14
	[PunRPC]
	public void RPC_UpdateQuestScore(int score, PhotonMessageInfo info)
	{
		this.UpdateQuestScore(score, info);
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x00046C23 File Offset: 0x00044E23
	[PunRPC]
	public void RPC_RequestQuestScore(PhotonMessageInfo info)
	{
		this.RequestQuestScore(info);
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000F5B40 File Offset: 0x000F3D40
	[PunRPC]
	public void GrabbedByPlayer(bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand, PhotonMessageInfo info)
	{
		GorillaGuardianManager gorillaGuardianManager = GorillaGameModes.GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager == null || !gorillaGuardianManager.IsPlayerGuardian(info.Sender))
		{
			return;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			return;
		}
		this.vrrig.GrabbedByPlayer(rigContainer.Rig, grabbedBody, grabbedLeftHand, grabbedWithLeftHand);
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000F5B9C File Offset: 0x000F3D9C
	[PunRPC]
	public void DroppedByPlayer(Vector3 throwVelocity, PhotonMessageInfo info)
	{
		RigContainer rigContainer;
		if (VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer))
		{
			float num = 10000f;
			if (throwVelocity.IsValid(num))
			{
				this.vrrig.DroppedByPlayer(rigContainer.Rig, throwVelocity);
				return;
			}
		}
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x00046C31 File Offset: 0x00044E31
	void IFXContextParems<HandTapArgs>.OnPlayFX(HandTapArgs parems)
	{
		this.vrrig.PlayHandTapLocal(parems.soundIndex, parems.isLeftHand, parems.tapVolume);
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x00046C50 File Offset: 0x00044E50
	void IFXContextParems<GeoSoundArg>.OnPlayFX(GeoSoundArg parems)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlayGeodeEffect(parems.position);
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x000F5BE4 File Offset: 0x000F3DE4
	private void OnHandTapRPCShared(int surfaceIndex, bool leftHanded, float handSpeed, long packedDir, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "OnHandTapRPCShared");
		if (info.Sender != this.netView.Owner)
		{
			return;
		}
		if (surfaceIndex < 0 || surfaceIndex > GTPlayer.Instance.materialData.Count - 1)
		{
			return;
		}
		Vector3 vector = Utils.UnpackVector3FromLong(packedDir);
		ref this.tempVector.SetValueSafe(vector);
		if (this.tempVector.sqrMagnitude != 1f)
		{
			this.tempVector = this.tempVector.normalized;
		}
		float max = GorillaTagger.Instance.DefaultHandTapVolume;
		GorillaAmbushManager gorillaAmbushManager = GorillaGameModes.GameMode.ActiveGameMode as GorillaAmbushManager;
		if (gorillaAmbushManager != null && gorillaAmbushManager.IsInfected(this.rigContainer.Creator))
		{
			max = gorillaAmbushManager.crawlingSpeedForMaxVolume;
		}
		OnHandTapFX onHandTapFX = new OnHandTapFX
		{
			rig = this.vrrig,
			surfaceIndex = surfaceIndex,
			isLeftHand = leftHanded,
			volume = handSpeed.ClampSafe(0f, max),
			tapDir = this.tempVector
		};
		if (leftHanded)
		{
			if (CrittersManager.instance.IsNotNull() && CrittersManager.instance.LocalAuthority() && CrittersManager.instance.rigSetupByRig[this.vrrig].IsNotNull())
			{
				CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.rigSetupByRig[this.vrrig].rigActors[0].actorSet;
				if (crittersLoudNoise.IsNotNull())
				{
					crittersLoudNoise.PlayHandTapRemote(info.SentServerTime, true);
				}
			}
		}
		else if (CrittersManager.instance.IsNotNull() && CrittersManager.instance.LocalAuthority() && CrittersManager.instance.rigSetupByRig[this.vrrig].IsNotNull())
		{
			CrittersLoudNoise crittersLoudNoise2 = (CrittersLoudNoise)CrittersManager.instance.rigSetupByRig[this.vrrig].rigActors[2].actorSet;
			if (crittersLoudNoise2.IsNotNull())
			{
				crittersLoudNoise2.PlayHandTapRemote(info.SentServerTime, false);
			}
		}
		FXSystem.PlayFXForRig<HandEffectContext>(FXType.OnHandTap, onHandTapFX, info);
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000F5E04 File Offset: 0x000F4004
	private void PlayHandTapShared(int soundIndex, bool isLeftHand, float tapVolume, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		GorillaNot.IncrementRPCCall(info, "PlayHandTapShared");
		NetPlayer sender = info.Sender;
		if (info.Sender == this.netView.Owner && float.IsFinite(tapVolume))
		{
			this.handTapArgs.soundIndex = soundIndex;
			this.handTapArgs.isLeftHand = isLeftHand;
			this.handTapArgs.tapVolume = Mathf.Clamp(tapVolume, 0f, 0.1f);
			FXSystem.PlayFX<HandTapArgs>(FXType.PlayHandTap, this, this.handTapArgs, info);
			return;
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent hand tap", sender.UserId, sender.NickName);
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000F5EA4 File Offset: 0x000F40A4
	private void UpdateNativeSizeShared(float value, PhotonMessageInfoWrapped info = default(PhotonMessageInfoWrapped))
	{
		GorillaNot.IncrementRPCCall(info, "UpdateNativeSizeShared");
		NetPlayer sender = info.Sender;
		if (info.Sender == this.netView.Owner && RPCUtil.SafeValue(value, 0.1f, 10f) && RPCUtil.NotSpam("UpdateNativeSizeShared", info, 1f))
		{
			if (this.vrrig != null)
			{
				this.vrrig.NativeScale = value;
				return;
			}
		}
		else
		{
			GorillaNot.instance.SendReport("inappropriate tag data being sent native size", sender.UserId, sender.NickName);
		}
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000F5F34 File Offset: 0x000F4134
	private void PlayGeodeEffectShared(Vector3 hitPosition, PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "PlayGeodeEffectShared");
		if (info.Sender == this.netView.Owner)
		{
			float num = 10000f;
			if (hitPosition.IsValid(num))
			{
				this.geoSoundArg.position = hitPosition;
				FXSystem.PlayFX<GeoSoundArg>(FXType.PlayHandTap, this, this.geoSoundArg, info);
				return;
			}
		}
		GorillaNot.instance.SendReport("inappropriate tag data being sent geode effect", info.Sender.UserId, info.Sender.NickName);
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x00046C68 File Offset: 0x00044E68
	private void InitializeNoobMaterialShared(float red, float green, float blue, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.InitializeNoobMaterial(red, green, blue, info);
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x00046C7F File Offset: 0x00044E7F
	private void RequestMaterialColorShared(int askingPlayerID, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.RequestMaterialColor(askingPlayerID, info);
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000F5FB8 File Offset: 0x000F41B8
	private void RequestCosmeticsShared(PhotonMessageInfoWrapped info)
	{
		GorillaNot.IncrementRPCCall(info, "RequestCosmetics");
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(info.Sender, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[9].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.RequestCosmetics(info);
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x00046C93 File Offset: 0x00044E93
	private void PlayDrumShared(int drumIndex, float drumVolume, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlayDrum(drumIndex, drumVolume, info);
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x00046CA8 File Offset: 0x00044EA8
	private void PlaySelfOnlyInstrumentShared(int selfOnlyIndex, int noteIndex, float instrumentVol, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlaySelfOnlyInstrument(selfOnlyIndex, noteIndex, instrumentVol, info);
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x00046CBF File Offset: 0x00044EBF
	private void UpdateCosmeticsWithTryonShared(int[] currentItems, int[] tryOnItems, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.UpdateCosmeticsWithTryon(currentItems, tryOnItems, info);
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x00046CD4 File Offset: 0x00044ED4
	private void PlaySplashEffectShared(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlaySplashEffect(splashPosition, splashRotation, splashScale, boundingRadius, bigSplash, enteringWater, info);
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x00046CF1 File Offset: 0x00044EF1
	private void EnableNonCosmeticHandItemShared(bool enable, bool isLeftHand, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.EnableNonCosmeticHandItemRPC(enable, isLeftHand, info);
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x00046D06 File Offset: 0x00044F06
	public void UpdateQuestScore(int score, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.UpdateQuestScore(score, info);
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x00046D1A File Offset: 0x00044F1A
	public void RequestQuestScore(PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.RequestQuestScore(info);
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x00046D56 File Offset: 0x00044F56
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.nickName = this._nickName;
		this.defaultName = this._defaultName;
		this.tutorialComplete = this._tutorialComplete;
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x00046D86 File Offset: 0x00044F86
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._nickName = this.nickName;
		this._defaultName = this.defaultName;
		this._tutorialComplete = this.tutorialComplete;
	}

	// Token: 0x04002516 RID: 9494
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("nickName", 0, 17)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkString<_16> _nickName;

	// Token: 0x04002517 RID: 9495
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("defaultName", 17, 17)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkString<_16> _defaultName;

	// Token: 0x04002518 RID: 9496
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("tutorialComplete", 34, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private bool _tutorialComplete;

	// Token: 0x04002519 RID: 9497
	[SerializeField]
	private PhotonVoiceView voiceView;

	// Token: 0x0400251A RID: 9498
	[SerializeField]
	private VoiceNetworkObject fusionVoiceView;

	// Token: 0x0400251B RID: 9499
	public Transform networkSpeaker;

	// Token: 0x0400251C RID: 9500
	[SerializeField]
	private VRRig vrrig;

	// Token: 0x0400251D RID: 9501
	private RigContainer rigContainer;

	// Token: 0x0400251E RID: 9502
	private HandTapArgs handTapArgs = new HandTapArgs();

	// Token: 0x0400251F RID: 9503
	private GeoSoundArg geoSoundArg = new GeoSoundArg();

	// Token: 0x04002520 RID: 9504
	private Vector3 tempVector = Vector3.zero;
}
