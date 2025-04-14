using System;
using GorillaExtensions;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200044B RID: 1099
public class RubberDuck : TransferrableObject
{
	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001B08 RID: 6920 RVA: 0x00085423 File Offset: 0x00083623
	// (set) Token: 0x06001B09 RID: 6921 RVA: 0x00085435 File Offset: 0x00083635
	public bool fxActive
	{
		get
		{
			return this.hasParticleFX && this._fxActive;
		}
		set
		{
			if (!this.hasParticleFX)
			{
				return;
			}
			this.pFXEmissionModule.enabled = value;
			this._fxActive = value;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00085453 File Offset: 0x00083653
	public int SqueezeSound
	{
		get
		{
			if (this.squeezeSoundBank.Length > 1)
			{
				return this.squeezeSoundBank[Random.Range(0, this.squeezeSoundBank.Length)];
			}
			if (this.squeezeSoundBank.Length == 1)
			{
				return this.squeezeSoundBank[0];
			}
			return this.squeezeSound;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001B0B RID: 6923 RVA: 0x00085490 File Offset: 0x00083690
	public int SqueezeReleaseSound
	{
		get
		{
			if (this.squeezeReleaseSoundBank.Length > 1)
			{
				return this.squeezeReleaseSoundBank[Random.Range(0, this.squeezeReleaseSoundBank.Length)];
			}
			if (this.squeezeReleaseSoundBank.Length == 1)
			{
				return this.squeezeReleaseSoundBank[0];
			}
			return this.squeezeReleaseSound;
		}
	}

	// Token: 0x06001B0C RID: 6924 RVA: 0x000854D0 File Offset: 0x000836D0
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		if (this.skinRenderer == null)
		{
			this.skinRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>(true);
		}
		this.hasSkinRenderer = (this.skinRenderer != null);
		this.myThreshold = 0.7f;
		this.hysterisis = 0.3f;
		this.hasParticleFX = (this.particleFX != null);
		if (this.hasParticleFX)
		{
			this.pFXEmissionModule = this.particleFX.emission;
			this.pFXEmissionModule.rateOverTime = this.particleFXEmissionIdle;
		}
		this.fxActive = false;
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x00085570 File Offset: 0x00083770
	internal override void OnEnable()
	{
		base.OnEnable();
		if (this._events == null)
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
			NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
			if (netPlayer != null)
			{
				this._events.Init(netPlayer);
			}
			else
			{
				Debug.LogError("Failed to get a reference to the Photon Player needed to hook up the cosmetic event");
			}
		}
		if (this._events != null)
		{
			this._events.Activate += this.OnSqueezeActivate;
			this._events.Deactivate += this.OnSqueezeDeactivate;
		}
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x00085660 File Offset: 0x00083860
	internal override void OnDisable()
	{
		base.OnDisable();
		if (this._events != null)
		{
			this._events.Activate -= this.OnSqueezeActivate;
			this._events.Deactivate -= this.OnSqueezeDeactivate;
			this._events.Dispose();
			this._events = null;
		}
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x000856D7 File Offset: 0x000838D7
	private void OnSqueezeActivate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (sender != target)
		{
			return;
		}
		if (info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		this.SqueezeActivateLocal();
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000856FE File Offset: 0x000838FE
	private void SqueezeActivateLocal()
	{
		this.PlayParticleFX(this.particleFXEmissionSqueeze);
		if (this._sfxActivate && !this._sfxActivate.isPlaying)
		{
			this._sfxActivate.PlayNext(0f, 1f);
		}
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x0008573B File Offset: 0x0008393B
	private void OnSqueezeDeactivate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (sender != target)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "OnSqueezeDeactivate");
		if (info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		this.SqueezeDeactivateLocal();
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x0008576E File Offset: 0x0008396E
	private void SqueezeDeactivateLocal()
	{
		this.PlayParticleFX(this.particleFXEmissionIdle);
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x0008577C File Offset: 0x0008397C
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		float num = 0f;
		if (base.InHand())
		{
			this.tempHandPos = ((base.myOnlineRig != null) ? base.myOnlineRig.ReturnHandPosition() : base.myRig.ReturnHandPosition());
			if (this.currentState == TransferrableObject.PositionState.InLeftHand)
			{
				num = (float)Mathf.FloorToInt((float)(this.tempHandPos % 10000) / 1000f);
			}
			else
			{
				num = (float)Mathf.FloorToInt((float)(this.tempHandPos % 10) / 1f);
			}
		}
		if (this.hasSkinRenderer)
		{
			this.skinRenderer.SetBlendShapeWeight(0, Mathf.Lerp(this.skinRenderer.GetBlendShapeWeight(0), num * 11.1f, this.blendShapeMaxWeight));
		}
		if (this.fxActive)
		{
			this.squeezeTimeElapsed += Time.deltaTime;
			this.pFXEmissionModule.rateOverTime = Mathf.Lerp(this.particleFXEmissionIdle, this.particleFXEmissionSqueeze, this.particleFXEmissionCooldownCurve.Evaluate(this.squeezeTimeElapsed));
			if (this.squeezeTimeElapsed > this.particleFXEmissionSqueeze)
			{
				this.fxActive = false;
			}
		}
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x00085898 File Offset: 0x00083A98
	public override void OnActivate()
	{
		base.OnActivate();
		if (this.IsMyItem())
		{
			bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
			RigContainer localRig = VRRigCache.Instance.localRig;
			int num = this.SqueezeSound;
			localRig.Rig.PlayHandTapLocal(num, flag, 0.33f);
			if (localRig.netView)
			{
				localRig.netView.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
				{
					num,
					flag,
					0.33f
				});
			}
			GorillaTagger.Instance.StartVibration(flag, this.squeezeStrength, Time.deltaTime);
		}
		if (this._raiseActivate)
		{
			if (RoomSystem.JoinedRoom)
			{
				RubberDuckEvents events = this._events;
				if (events == null)
				{
					return;
				}
				PhotonEvent activate = events.Activate;
				if (activate == null)
				{
					return;
				}
				activate.RaiseAll(Array.Empty<object>());
				return;
			}
			else
			{
				this.SqueezeActivateLocal();
			}
		}
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x00085974 File Offset: 0x00083B74
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (this.IsMyItem())
		{
			bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand;
			int num = this.SqueezeReleaseSound;
			Debug.Log("Squeezy Deactivate: " + num.ToString());
			VRRigCache.Instance.localRig.Rig.PlayHandTapLocal(num, flag, 0.33f);
			RigContainer rigContainer;
			if (GorillaGameManager.instance && VRRigCache.Instance.TryGetVrrig(NetworkSystem.Instance.LocalPlayer, out rigContainer))
			{
				rigContainer.Rig.netView.SendRPC("RPC_PlayHandTap", RpcTarget.Others, new object[]
				{
					num,
					flag,
					0.33f
				});
			}
			GorillaTagger.Instance.StartVibration(flag, this.releaseStrength, Time.deltaTime);
		}
		if (this._raiseDeactivate)
		{
			if (RoomSystem.JoinedRoom)
			{
				RubberDuckEvents events = this._events;
				if (events == null)
				{
					return;
				}
				PhotonEvent deactivate = events.Deactivate;
				if (deactivate == null)
				{
					return;
				}
				deactivate.RaiseAll(Array.Empty<object>());
				return;
			}
			else
			{
				this.SqueezeDeactivateLocal();
			}
		}
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x00085A80 File Offset: 0x00083C80
	public void PlayParticleFX(float rate)
	{
		if (!this.hasParticleFX)
		{
			return;
		}
		if (this.currentState != TransferrableObject.PositionState.InLeftHand && this.currentState != TransferrableObject.PositionState.InRightHand)
		{
			return;
		}
		if (!this.fxActive)
		{
			this.fxActive = true;
		}
		this.squeezeTimeElapsed = 0f;
		this.pFXEmissionModule.rateOverTime = rate;
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x00085AD4 File Offset: 0x00083CD4
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x00085ADF File Offset: 0x00083CDF
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001DF6 RID: 7670
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001DF7 RID: 7671
	[DebugOption]
	public bool disableDeactivation;

	// Token: 0x04001DF8 RID: 7672
	private SkinnedMeshRenderer skinRenderer;

	// Token: 0x04001DF9 RID: 7673
	[FormerlySerializedAs("duckieLerp")]
	public float blendShapeMaxWeight = 1f;

	// Token: 0x04001DFA RID: 7674
	private int tempHandPos;

	// Token: 0x04001DFB RID: 7675
	[GorillaSoundLookup]
	[SerializeField]
	private int squeezeSound = 75;

	// Token: 0x04001DFC RID: 7676
	[GorillaSoundLookup]
	[SerializeField]
	private int squeezeReleaseSound = 76;

	// Token: 0x04001DFD RID: 7677
	[GorillaSoundLookup]
	public int[] squeezeSoundBank;

	// Token: 0x04001DFE RID: 7678
	[GorillaSoundLookup]
	public int[] squeezeReleaseSoundBank;

	// Token: 0x04001DFF RID: 7679
	public float squeezeStrength = 0.05f;

	// Token: 0x04001E00 RID: 7680
	public float releaseStrength = 0.03f;

	// Token: 0x04001E01 RID: 7681
	public ParticleSystem particleFX;

	// Token: 0x04001E02 RID: 7682
	[Tooltip("The emission rate of the particle effect when not squeezed.")]
	public float particleFXEmissionIdle = 0.8f;

	// Token: 0x04001E03 RID: 7683
	[Tooltip("The emission rate of the particle effect when squeezed.")]
	public float particleFXEmissionSqueeze = 10f;

	// Token: 0x04001E04 RID: 7684
	[Tooltip("The animation of the particle effect returning to the idle emission rate. X axis is time, Y axis is the emission lerp value where 0 is idle, 1 is squeezed.")]
	public AnimationCurve particleFXEmissionCooldownCurve;

	// Token: 0x04001E05 RID: 7685
	private bool hasSkinRenderer;

	// Token: 0x04001E06 RID: 7686
	private ParticleSystem.EmissionModule pFXEmissionModule;

	// Token: 0x04001E07 RID: 7687
	private bool hasParticleFX;

	// Token: 0x04001E08 RID: 7688
	private float squeezeTimeElapsed;

	// Token: 0x04001E09 RID: 7689
	[SerializeField]
	private RubberDuckEvents _events;

	// Token: 0x04001E0A RID: 7690
	[SerializeField]
	private bool _raiseActivate = true;

	// Token: 0x04001E0B RID: 7691
	[SerializeField]
	private bool _raiseDeactivate = true;

	// Token: 0x04001E0C RID: 7692
	[SerializeField]
	private SoundEffects _sfxActivate;

	// Token: 0x04001E0D RID: 7693
	[SerializeField]
	private bool _fxActive;
}
