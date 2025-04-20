using System;
using GorillaExtensions;
using GorillaTag;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000457 RID: 1111
public class RubberDuck : TransferrableObject
{
	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001B5C RID: 7004 RVA: 0x00042972 File Offset: 0x00040B72
	// (set) Token: 0x06001B5D RID: 7005 RVA: 0x00042984 File Offset: 0x00040B84
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

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000429A2 File Offset: 0x00040BA2
	public int SqueezeSound
	{
		get
		{
			if (this.squeezeSoundBank.Length > 1)
			{
				return this.squeezeSoundBank[UnityEngine.Random.Range(0, this.squeezeSoundBank.Length)];
			}
			if (this.squeezeSoundBank.Length == 1)
			{
				return this.squeezeSoundBank[0];
			}
			return this.squeezeSound;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000429DF File Offset: 0x00040BDF
	public int SqueezeReleaseSound
	{
		get
		{
			if (this.squeezeReleaseSoundBank.Length > 1)
			{
				return this.squeezeReleaseSoundBank[UnityEngine.Random.Range(0, this.squeezeReleaseSoundBank.Length)];
			}
			if (this.squeezeReleaseSoundBank.Length == 1)
			{
				return this.squeezeReleaseSoundBank[0];
			}
			return this.squeezeReleaseSound;
		}
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x000D9F80 File Offset: 0x000D8180
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

	// Token: 0x06001B61 RID: 7009 RVA: 0x000DA020 File Offset: 0x000D8220
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

	// Token: 0x06001B62 RID: 7010 RVA: 0x000DA110 File Offset: 0x000D8310
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

	// Token: 0x06001B63 RID: 7011 RVA: 0x00042A1C File Offset: 0x00040C1C
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

	// Token: 0x06001B64 RID: 7012 RVA: 0x00042A43 File Offset: 0x00040C43
	private void SqueezeActivateLocal()
	{
		this.PlayParticleFX(this.particleFXEmissionSqueeze);
		if (this._sfxActivate && !this._sfxActivate.isPlaying)
		{
			this._sfxActivate.PlayNext(0f, 1f);
		}
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x00042A80 File Offset: 0x00040C80
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

	// Token: 0x06001B66 RID: 7014 RVA: 0x00042AB3 File Offset: 0x00040CB3
	private void SqueezeDeactivateLocal()
	{
		this.PlayParticleFX(this.particleFXEmissionIdle);
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000DA188 File Offset: 0x000D8388
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

	// Token: 0x06001B68 RID: 7016 RVA: 0x000DA2A4 File Offset: 0x000D84A4
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

	// Token: 0x06001B69 RID: 7017 RVA: 0x000DA380 File Offset: 0x000D8580
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

	// Token: 0x06001B6A RID: 7018 RVA: 0x000DA48C File Offset: 0x000D868C
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

	// Token: 0x06001B6B RID: 7019 RVA: 0x00042AC1 File Offset: 0x00040CC1
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x00042ACC File Offset: 0x00040CCC
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001E45 RID: 7749
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001E46 RID: 7750
	[DebugOption]
	public bool disableDeactivation;

	// Token: 0x04001E47 RID: 7751
	private SkinnedMeshRenderer skinRenderer;

	// Token: 0x04001E48 RID: 7752
	[FormerlySerializedAs("duckieLerp")]
	public float blendShapeMaxWeight = 1f;

	// Token: 0x04001E49 RID: 7753
	private int tempHandPos;

	// Token: 0x04001E4A RID: 7754
	[GorillaSoundLookup]
	[SerializeField]
	private int squeezeSound = 75;

	// Token: 0x04001E4B RID: 7755
	[GorillaSoundLookup]
	[SerializeField]
	private int squeezeReleaseSound = 76;

	// Token: 0x04001E4C RID: 7756
	[GorillaSoundLookup]
	public int[] squeezeSoundBank;

	// Token: 0x04001E4D RID: 7757
	[GorillaSoundLookup]
	public int[] squeezeReleaseSoundBank;

	// Token: 0x04001E4E RID: 7758
	public float squeezeStrength = 0.05f;

	// Token: 0x04001E4F RID: 7759
	public float releaseStrength = 0.03f;

	// Token: 0x04001E50 RID: 7760
	public ParticleSystem particleFX;

	// Token: 0x04001E51 RID: 7761
	[Tooltip("The emission rate of the particle effect when not squeezed.")]
	public float particleFXEmissionIdle = 0.8f;

	// Token: 0x04001E52 RID: 7762
	[Tooltip("The emission rate of the particle effect when squeezed.")]
	public float particleFXEmissionSqueeze = 10f;

	// Token: 0x04001E53 RID: 7763
	[Tooltip("The animation of the particle effect returning to the idle emission rate. X axis is time, Y axis is the emission lerp value where 0 is idle, 1 is squeezed.")]
	public AnimationCurve particleFXEmissionCooldownCurve;

	// Token: 0x04001E54 RID: 7764
	private bool hasSkinRenderer;

	// Token: 0x04001E55 RID: 7765
	private ParticleSystem.EmissionModule pFXEmissionModule;

	// Token: 0x04001E56 RID: 7766
	private bool hasParticleFX;

	// Token: 0x04001E57 RID: 7767
	private float squeezeTimeElapsed;

	// Token: 0x04001E58 RID: 7768
	[SerializeField]
	private RubberDuckEvents _events;

	// Token: 0x04001E59 RID: 7769
	[SerializeField]
	private bool _raiseActivate = true;

	// Token: 0x04001E5A RID: 7770
	[SerializeField]
	private bool _raiseDeactivate = true;

	// Token: 0x04001E5B RID: 7771
	[SerializeField]
	private SoundEffects _sfxActivate;

	// Token: 0x04001E5C RID: 7772
	[SerializeField]
	private bool _fxActive;
}
