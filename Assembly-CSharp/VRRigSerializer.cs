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

// Token: 0x02000549 RID: 1353
[NetworkBehaviourWeaved(35)]
internal class VRRigSerializer : GorillaWrappedSerializer, IFXContextParems<HandTapArgs>, IFXContextParems<GeoSoundArg>
{
	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000A4B35 File Offset: 0x000A2D35
	// (set) Token: 0x060020F9 RID: 8441 RVA: 0x000A4B5F File Offset: 0x000A2D5F
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

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060020FA RID: 8442 RVA: 0x000A4B8A File Offset: 0x000A2D8A
	// (set) Token: 0x060020FB RID: 8443 RVA: 0x000A4BB8 File Offset: 0x000A2DB8
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

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060020FC RID: 8444 RVA: 0x000A4BE7 File Offset: 0x000A2DE7
	// (set) Token: 0x060020FD RID: 8445 RVA: 0x000A4C15 File Offset: 0x000A2E15
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

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060020FE RID: 8446 RVA: 0x000A4C44 File Offset: 0x000A2E44
	private PhotonVoiceView Voice
	{
		get
		{
			return this.voiceView;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060020FF RID: 8447 RVA: 0x000A4C4C File Offset: 0x000A2E4C
	public VRRig VRRig
	{
		get
		{
			return this.vrrig;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06002100 RID: 8448 RVA: 0x000A4C54 File Offset: 0x000A2E54
	public FXSystemSettings settings
	{
		get
		{
			return this.vrrig.fxSettings;
		}
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000A4C64 File Offset: 0x000A2E64
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

	// Token: 0x06002102 RID: 8450 RVA: 0x000A4D8C File Offset: 0x000A2F8C
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

	// Token: 0x06002103 RID: 8451 RVA: 0x000023F4 File Offset: 0x000005F4
	protected override void OnFailedSpawn()
	{
	}

	// Token: 0x06002104 RID: 8452 RVA: 0x000A4EC8 File Offset: 0x000A30C8
	protected override void OnBeforeDespawn()
	{
		this.CleanUp(true);
	}

	// Token: 0x06002105 RID: 8453 RVA: 0x000A4ED4 File Offset: 0x000A30D4
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

	// Token: 0x06002106 RID: 8454 RVA: 0x000A4FC5 File Offset: 0x000A31C5
	private void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		this.CleanUp(false);
	}

	// Token: 0x06002107 RID: 8455 RVA: 0x000A4FD4 File Offset: 0x000A31D4
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		if (this.networkSpeaker != null && this.networkSpeaker.parent != base.transform)
		{
			UnityEngine.Object.Destroy(this.networkSpeaker.gameObject);
		}
	}

	// Token: 0x06002108 RID: 8456 RVA: 0x000A5012 File Offset: 0x000A3212
	[PunRPC]
	public void RPC_InitializeNoobMaterial(float red, float green, float blue, PhotonMessageInfo info)
	{
		this.InitializeNoobMaterialShared(red, green, blue, info);
	}

	// Token: 0x06002109 RID: 8457 RVA: 0x000A5024 File Offset: 0x000A3224
	[PunRPC]
	public void RPC_RequestCosmetics(PhotonMessageInfo info)
	{
		this.RequestCosmeticsShared(info);
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000A5032 File Offset: 0x000A3232
	[PunRPC]
	public void RPC_PlayDrum(int drumIndex, float drumVolume, PhotonMessageInfo info)
	{
		this.PlayDrumShared(drumIndex, drumVolume, info);
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000A5042 File Offset: 0x000A3242
	[PunRPC]
	public void RPC_PlaySelfOnlyInstrument(int selfOnlyIndex, int noteIndex, float instrumentVol, PhotonMessageInfo info)
	{
		this.PlaySelfOnlyInstrumentShared(selfOnlyIndex, noteIndex, instrumentVol, info);
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x000A5054 File Offset: 0x000A3254
	[PunRPC]
	public void RPC_PlayHandTap(int soundIndex, bool isLeftHand, float tapVolume, PhotonMessageInfo info = default(PhotonMessageInfo))
	{
		this.PlayHandTapShared(soundIndex, isLeftHand, tapVolume, info);
	}

	// Token: 0x0600210D RID: 8461 RVA: 0x000A5066 File Offset: 0x000A3266
	public void RPC_UpdateNativeSize(float value, PhotonMessageInfo info = default(PhotonMessageInfo))
	{
		this.UpdateNativeSizeShared(value, info);
	}

	// Token: 0x0600210E RID: 8462 RVA: 0x000023F4 File Offset: 0x000005F4
	public void RPC_UpdateCosmetics(string[] currentItems, PhotonMessageInfo info)
	{
	}

	// Token: 0x0600210F RID: 8463 RVA: 0x000023F4 File Offset: 0x000005F4
	public void RPC_UpdateCosmeticsWithTryon(string[] currentItems, string[] tryOnItems, PhotonMessageInfo info)
	{
	}

	// Token: 0x06002110 RID: 8464 RVA: 0x000A5075 File Offset: 0x000A3275
	[PunRPC]
	public void RPC_UpdateCosmeticsWithTryonPacked(int[] currentItemsPacked, int[] tryOnItemsPacked, PhotonMessageInfo info)
	{
		this.UpdateCosmeticsWithTryonShared(currentItemsPacked, tryOnItemsPacked, info);
	}

	// Token: 0x06002111 RID: 8465 RVA: 0x000A5085 File Offset: 0x000A3285
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

	// Token: 0x06002112 RID: 8466 RVA: 0x000A5098 File Offset: 0x000A3298
	[PunRPC]
	public void RPC_PlaySplashEffect(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, PhotonMessageInfo info)
	{
		this.PlaySplashEffectShared(splashPosition, splashRotation, splashScale, boundingRadius, bigSplash, enteringWater, info);
	}

	// Token: 0x06002113 RID: 8467 RVA: 0x000A50B0 File Offset: 0x000A32B0
	[PunRPC]
	public void RPC_PlayGeodeEffect(Vector3 hitPosition, PhotonMessageInfo info)
	{
		this.PlayGeodeEffectShared(hitPosition, info);
	}

	// Token: 0x06002114 RID: 8468 RVA: 0x000A50BF File Offset: 0x000A32BF
	[PunRPC]
	public void EnableNonCosmeticHandItemRPC(bool enable, bool isLeftHand, PhotonMessageInfo info)
	{
		this.EnableNonCosmeticHandItemShared(enable, isLeftHand, info);
	}

	// Token: 0x06002115 RID: 8469 RVA: 0x000A50CF File Offset: 0x000A32CF
	[PunRPC]
	public void OnHandTapRPC(int surfaceIndex, bool leftHanded, float handSpeed, long packedDir, PhotonMessageInfo info)
	{
		this.OnHandTapRPCShared(surfaceIndex, leftHanded, handSpeed, packedDir, info);
	}

	// Token: 0x06002116 RID: 8470 RVA: 0x000A50E3 File Offset: 0x000A32E3
	[PunRPC]
	public void RPC_UpdateQuestScore(int score, PhotonMessageInfo info)
	{
		this.UpdateQuestScore(score, info);
	}

	// Token: 0x06002117 RID: 8471 RVA: 0x000A50F2 File Offset: 0x000A32F2
	[PunRPC]
	public void RPC_RequestQuestScore(PhotonMessageInfo info)
	{
		this.RequestQuestScore(info);
	}

	// Token: 0x06002118 RID: 8472 RVA: 0x000A5100 File Offset: 0x000A3300
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

	// Token: 0x06002119 RID: 8473 RVA: 0x000A515C File Offset: 0x000A335C
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

	// Token: 0x0600211A RID: 8474 RVA: 0x000A51A1 File Offset: 0x000A33A1
	void IFXContextParems<HandTapArgs>.OnPlayFX(HandTapArgs parems)
	{
		this.vrrig.PlayHandTapLocal(parems.soundIndex, parems.isLeftHand, parems.tapVolume);
	}

	// Token: 0x0600211B RID: 8475 RVA: 0x000A51C0 File Offset: 0x000A33C0
	void IFXContextParems<GeoSoundArg>.OnPlayFX(GeoSoundArg parems)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlayGeodeEffect(parems.position);
	}

	// Token: 0x0600211C RID: 8476 RVA: 0x000A51D8 File Offset: 0x000A33D8
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

	// Token: 0x0600211D RID: 8477 RVA: 0x000A53F8 File Offset: 0x000A35F8
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

	// Token: 0x0600211E RID: 8478 RVA: 0x000A5498 File Offset: 0x000A3698
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

	// Token: 0x0600211F RID: 8479 RVA: 0x000A5528 File Offset: 0x000A3728
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

	// Token: 0x06002120 RID: 8480 RVA: 0x000A55A9 File Offset: 0x000A37A9
	private void InitializeNoobMaterialShared(float red, float green, float blue, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.InitializeNoobMaterial(red, green, blue, info);
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x000A55C0 File Offset: 0x000A37C0
	private void RequestMaterialColorShared(int askingPlayerID, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.RequestMaterialColor(askingPlayerID, info);
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000A55D4 File Offset: 0x000A37D4
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

	// Token: 0x06002123 RID: 8483 RVA: 0x000A5637 File Offset: 0x000A3837
	private void PlayDrumShared(int drumIndex, float drumVolume, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlayDrum(drumIndex, drumVolume, info);
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000A564C File Offset: 0x000A384C
	private void PlaySelfOnlyInstrumentShared(int selfOnlyIndex, int noteIndex, float instrumentVol, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlaySelfOnlyInstrument(selfOnlyIndex, noteIndex, instrumentVol, info);
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000A5663 File Offset: 0x000A3863
	private void UpdateCosmeticsWithTryonShared(int[] currentItems, int[] tryOnItems, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.UpdateCosmeticsWithTryon(currentItems, tryOnItems, info);
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000A5678 File Offset: 0x000A3878
	private void PlaySplashEffectShared(Vector3 splashPosition, Quaternion splashRotation, float splashScale, float boundingRadius, bool bigSplash, bool enteringWater, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.PlaySplashEffect(splashPosition, splashRotation, splashScale, boundingRadius, bigSplash, enteringWater, info);
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000A5695 File Offset: 0x000A3895
	private void EnableNonCosmeticHandItemShared(bool enable, bool isLeftHand, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.EnableNonCosmeticHandItemRPC(enable, isLeftHand, info);
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000A56AA File Offset: 0x000A38AA
	public void UpdateQuestScore(int score, PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.UpdateQuestScore(score, info);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000A56BE File Offset: 0x000A38BE
	public void RequestQuestScore(PhotonMessageInfoWrapped info)
	{
		VRRig vrrig = this.vrrig;
		if (vrrig == null)
		{
			return;
		}
		vrrig.RequestQuestScore(info);
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x000A56FA File Offset: 0x000A38FA
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.nickName = this._nickName;
		this.defaultName = this._defaultName;
		this.tutorialComplete = this._tutorialComplete;
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000A572A File Offset: 0x000A392A
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._nickName = this.nickName;
		this._defaultName = this.defaultName;
		this._tutorialComplete = this.tutorialComplete;
	}

	// Token: 0x040024BE RID: 9406
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("nickName", 0, 17)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkString<_16> _nickName;

	// Token: 0x040024BF RID: 9407
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("defaultName", 17, 17)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private NetworkString<_16> _defaultName;

	// Token: 0x040024C0 RID: 9408
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("tutorialComplete", 34, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private bool _tutorialComplete;

	// Token: 0x040024C1 RID: 9409
	[SerializeField]
	private PhotonVoiceView voiceView;

	// Token: 0x040024C2 RID: 9410
	[SerializeField]
	private VoiceNetworkObject fusionVoiceView;

	// Token: 0x040024C3 RID: 9411
	public Transform networkSpeaker;

	// Token: 0x040024C4 RID: 9412
	[SerializeField]
	private VRRig vrrig;

	// Token: 0x040024C5 RID: 9413
	private RigContainer rigContainer;

	// Token: 0x040024C6 RID: 9414
	private HandTapArgs handTapArgs = new HandTapArgs();

	// Token: 0x040024C7 RID: 9415
	private GeoSoundArg geoSoundArg = new GeoSoundArg();

	// Token: 0x040024C8 RID: 9416
	private Vector3 tempVector = Vector3.zero;
}
