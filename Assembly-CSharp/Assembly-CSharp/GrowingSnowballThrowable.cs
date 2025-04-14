using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class GrowingSnowballThrowable : SnowballThrowable
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001E236 File Offset: 0x0001C436
	public int SizeLevel
	{
		get
		{
			return this.sizeLevel;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001E23E File Offset: 0x0001C43E
	public int MaxSizeLevel
	{
		get
		{
			return Mathf.Max(this.snowballSizeLevels.Count - 1, 0);
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001E254 File Offset: 0x0001C454
	public float CurrentSnowballRadius
	{
		get
		{
			if (this.snowballSizeLevels.Count > 0 && this.sizeLevel > -1 && this.sizeLevel < this.snowballSizeLevels.Count)
			{
				return this.snowballSizeLevels[this.sizeLevel].snowballScale * this.modelRadius * base.transform.lossyScale.x;
			}
			return this.modelRadius * base.transform.lossyScale.x;
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
	protected override void Awake()
	{
		base.Awake();
		if (NetworkSystem.Instance != null)
		{
			NetworkSystem.Instance.OnMultiplayerStarted += this.StartedMultiplayerSession;
		}
		else
		{
			Debug.LogError("NetworkSystem.Instance was null in SnowballThrowable Awake");
		}
		VRRigCache.OnRigActivated += this.VRRigActivated;
		VRRigCache.OnRigDeactivated += this.VRRigDeactivated;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0001E338 File Offset: 0x0001C538
	public override void OnEnable()
	{
		base.OnEnable();
		this.snowballModelParentTransform.localPosition = this.modelParentOffset;
		this.snowballModelTransform.localPosition = this.modelOffset;
		this.otherHandSnowball = (this.isLeftHanded ? (EquipmentInteractor.instance.rightHandHeldEquipment as GrowingSnowballThrowable) : (EquipmentInteractor.instance.leftHandHeldEquipment as GrowingSnowballThrowable));
		if (this.resetSizeOnNextEnable)
		{
			this.SetSizeLevelLocal(0);
		}
		this.CreatePhotonEventsIfNull();
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0001E3B4 File Offset: 0x0001C5B4
	public override void OnDisable()
	{
		base.OnDisable();
		this.resetSizeOnNextEnable = true;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001E3C3 File Offset: 0x0001C5C3
	protected override void OnDestroy()
	{
		this.DestroyPhotonEvents();
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001E3CC File Offset: 0x0001C5CC
	private void VRRigActivated(RigContainer rigContainer)
	{
		this.targetRig = base.GetComponentInParent<VRRig>(true);
		this.isOfflineRig = (this.targetRig != null && this.targetRig.isOfflineVRRig);
		if (rigContainer.Rig == this.targetRig)
		{
			this.CreatePhotonEventsIfNull();
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001E421 File Offset: 0x0001C621
	private void VRRigDeactivated(RigContainer rigContainer)
	{
		if (rigContainer.Rig == this.targetRig)
		{
			this.DestroyPhotonEvents();
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x0001E43C File Offset: 0x0001C63C
	private void StartedMultiplayerSession()
	{
		this.targetRig = base.GetComponentInParent<VRRig>(true);
		this.isOfflineRig = (this.targetRig != null && this.targetRig.isOfflineVRRig);
		if (this.isOfflineRig)
		{
			this.DestroyPhotonEvents();
			this.CreatePhotonEventsIfNull();
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001E48C File Offset: 0x0001C68C
	private void CreatePhotonEventsIfNull()
	{
		if (this.targetRig == null)
		{
			this.targetRig = base.GetComponentInParent<VRRig>(true);
			this.isOfflineRig = (this.targetRig != null && this.targetRig.isOfflineVRRig);
		}
		if (this.targetRig == null || this.targetRig.netView == null)
		{
			return;
		}
		if (this.changeSizeEvent == null)
		{
			"SnowballThrowable" + (this.isLeftHanded ? "ChangeSizeEventLeft" : "ChangeSizeEventRight") + this.targetRig.netView.ViewID.ToString();
			int eventId = StaticHash.Compute("SnowballThrowable", this.isLeftHanded ? "ChangeSizeEventLeft" : "ChangeSizeEventRight", this.targetRig.netView.ViewID.ToString());
			this.changeSizeEvent = new PhotonEvent(eventId);
			this.changeSizeEvent.reliable = true;
			this.changeSizeEvent += new Action<int, int, object[], PhotonMessageInfoWrapped>(this.ChangeSizeEventReceiver);
		}
		if (this.snowballThrowEvent == null)
		{
			"SnowballThrowable" + (this.isLeftHanded ? "SnowballThrowEventLeft" : "SnowballThrowEventRight") + this.targetRig.netView.ViewID.ToString();
			int eventId2 = StaticHash.Compute("SnowballThrowable", this.isLeftHanded ? "SnowballThrowEventLeft" : "SnowballThrowEventRight", this.targetRig.netView.ViewID.ToString());
			this.snowballThrowEvent = new PhotonEvent(eventId2);
			this.snowballThrowEvent.reliable = true;
			this.snowballThrowEvent += new Action<int, int, object[], PhotonMessageInfoWrapped>(this.SnowballThrowEventReceiver);
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001E660 File Offset: 0x0001C860
	private void DestroyPhotonEvents()
	{
		if (this.changeSizeEvent != null)
		{
			this.changeSizeEvent -= new Action<int, int, object[], PhotonMessageInfoWrapped>(this.ChangeSizeEventReceiver);
			this.changeSizeEvent.Dispose();
			this.changeSizeEvent = null;
		}
		if (this.snowballThrowEvent != null)
		{
			this.snowballThrowEvent -= new Action<int, int, object[], PhotonMessageInfoWrapped>(this.SnowballThrowEventReceiver);
			this.snowballThrowEvent.Dispose();
			this.snowballThrowEvent = null;
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0001E6E7 File Offset: 0x0001C8E7
	public void IncreaseSize(int increase)
	{
		this.SetSizeLevelAuthority(this.sizeLevel + increase);
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001E6F8 File Offset: 0x0001C8F8
	private void SetSizeLevelAuthority(int sizeLevel)
	{
		if (this.targetRig != null && this.targetRig.creator != null && this.targetRig.creator.IsLocal)
		{
			int validSizeLevel = this.GetValidSizeLevel(sizeLevel);
			if (validSizeLevel > this.sizeLevel)
			{
				this.sizeIncreaseSoundBankPlayer.Play();
			}
			this.SetSizeLevelLocal(validSizeLevel);
			PhotonEvent photonEvent = this.changeSizeEvent;
			if (photonEvent == null)
			{
				return;
			}
			photonEvent.RaiseOthers(new object[]
			{
				validSizeLevel
			});
		}
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001E774 File Offset: 0x0001C974
	private int GetValidSizeLevel(int inputSizeLevel)
	{
		int max = Mathf.Max(this.snowballSizeLevels.Count - 1, 0);
		return Mathf.Clamp(inputSizeLevel, 0, max);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
	private void SetSizeLevelLocal(int sizeLevel)
	{
		int validSizeLevel = this.GetValidSizeLevel(sizeLevel);
		if (validSizeLevel >= 0 && validSizeLevel != this.sizeLevel)
		{
			this.sizeLevel = validSizeLevel;
			this.snowballModelParentTransform.localScale = Vector3.one * this.snowballSizeLevels[this.sizeLevel].snowballScale;
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
	private void ChangeSizeEventReceiver(int sender, int receiver, object[] args, PhotonMessageInfoWrapped info)
	{
		if (sender != receiver)
		{
			return;
		}
		if (args == null || args.Length < 1)
		{
			return;
		}
		int num = (this.targetRig != null && this.targetRig.gameObject.activeInHierarchy && this.targetRig.netView != null && this.targetRig.netView.Owner != null) ? this.targetRig.netView.Owner.ActorNumber : -1;
		if (info.senderID != num)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "ChangeSizeEventReceiver");
		int num2 = (int)args[0];
		if (this.GetValidSizeLevel(num2) > this.sizeLevel)
		{
			this.sizeIncreaseSoundBankPlayer.Play();
		}
		this.SetSizeLevelLocal(num2);
		if (!base.gameObject.activeSelf)
		{
			this.resetSizeOnNextEnable = false;
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0001E8C4 File Offset: 0x0001CAC4
	private void SnowballThrowEventReceiver(int sender, int receiver, object[] args, PhotonMessageInfoWrapped info)
	{
		if (sender != receiver)
		{
			return;
		}
		if (args == null || args.Length < 3)
		{
			return;
		}
		int num = (this.targetRig != null && this.targetRig.gameObject.activeInHierarchy && this.targetRig.netView != null && this.targetRig.netView.Owner != null) ? this.targetRig.netView.Owner.ActorNumber : -1;
		if (info.senderID != num)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SnowballThrowEventReceiver");
		if (!this.snowballThrowCallLimit.CheckCallTime(Time.time))
		{
			return;
		}
		Vector3 vector = (Vector3)args[0];
		Vector3 velocity = this.targetRig.ClampVelocityRelativeToPlayerSafe((Vector3)args[1], 50f);
		float x = this.snowballModelTransform.lossyScale.x;
		float num2 = 10000f;
		if (!vector.IsValid(num2) || !this.targetRig.IsPositionInRange(vector, 4f))
		{
			return;
		}
		this.LaunchSnowballLocal(vector, velocity, x);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001E9CC File Offset: 0x0001CBCC
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (GrowingSnowballThrowable.twoHandedSnowballGrowing && this.otherHandSnowball != null)
		{
			IHoldableObject holdableObject = this.isLeftHanded ? EquipmentInteractor.instance.rightHandHeldEquipment : EquipmentInteractor.instance.leftHandHeldEquipment;
			if (holdableObject != null && this.otherHandSnowball != (GrowingSnowballThrowable)holdableObject)
			{
				this.otherHandSnowball = null;
				return;
			}
			float num = this.otherHandSnowball.CurrentSnowballRadius + this.CurrentSnowballRadius;
			if (this.SizeLevel < this.MaxSizeLevel && this.otherHandSnowball.SizeLevel < this.otherHandSnowball.MaxSizeLevel && (this.otherHandSnowball.snowballModelTransform.position - this.snowballModelTransform.position).sqrMagnitude < num * num)
			{
				int num2 = this.SizeLevel - this.otherHandSnowball.SizeLevel;
				float magnitude = this.velocityEstimator.linearVelocity.magnitude;
				float magnitude2 = this.otherHandSnowball.velocityEstimator.linearVelocity.magnitude;
				bool flag;
				if (Mathf.Abs(magnitude - magnitude2) > this.combineBasedOnSpeedThreshold || num2 == 0)
				{
					flag = (magnitude > magnitude2);
				}
				else
				{
					flag = (num2 < 0);
				}
				if (flag)
				{
					this.otherHandSnowball.IncreaseSize(this.sizeLevel + 1);
					GorillaTagger.Instance.StartVibration(!this.isLeftHanded, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
					base.SetSnowballActiveLocal(false);
					return;
				}
				this.IncreaseSize(this.otherHandSnowball.SizeLevel + 1);
				GorillaTagger.Instance.StartVibration(this.isLeftHanded, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
				this.otherHandSnowball.SetSnowballActiveLocal(false);
			}
		}
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001EBBA File Offset: 0x0001CDBA
	protected override void OnSnowballRelease()
	{
		if (base.isActiveAndEnabled)
		{
			this.PerformSnowballThrowAuthority();
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001EBCC File Offset: 0x0001CDCC
	protected override void PerformSnowballThrowAuthority()
	{
		if (!(this.targetRig != null) || this.targetRig.creator == null || !this.targetRig.creator.IsLocal)
		{
			return;
		}
		Vector3 b = Vector3.zero;
		Rigidbody component = GorillaTagger.Instance.GetComponent<Rigidbody>();
		if (component != null)
		{
			b = component.velocity;
		}
		Vector3 a = this.velocityEstimator.linearVelocity - b;
		float magnitude = a.magnitude;
		if (magnitude > 0.001f)
		{
			float num = Mathf.Clamp(magnitude * this.linSpeedMultiplier, 0f, this.maxLinSpeed);
			a *= num / magnitude;
		}
		Vector3 vector = a + b;
		this.targetRig.GetThrowableProjectileColor(this.isLeftHanded);
		Transform transform = this.snowballModelTransform;
		Vector3 position = transform.position;
		float x = transform.lossyScale.x;
		SlingshotProjectile slingshotProjectile = this.LaunchSnowballLocal(position, vector, x);
		base.SetSnowballActiveLocal(false);
		if (this.randModelIndex > -1 && this.randModelIndex < this.localModels.Count && this.localModels[this.randModelIndex].destroyAfterRelease)
		{
			slingshotProjectile.DestroyAfterRelease();
		}
		PhotonEvent photonEvent = this.snowballThrowEvent;
		if (photonEvent == null)
		{
			return;
		}
		photonEvent.RaiseOthers(new object[]
		{
			position,
			vector,
			x
		});
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001ED2C File Offset: 0x0001CF2C
	protected override SlingshotProjectile LaunchSnowballLocal(Vector3 location, Vector3 velocity, float scale)
	{
		SlingshotProjectile component = ObjectPools.instance.Instantiate(this.randomModelSelection ? this.localModels[this.randModelIndex].projectilePrefab : this.projectilePrefab).GetComponent<SlingshotProjectile>();
		if (this.snowballSizeLevels.Count > 0 && this.sizeLevel >= 0 && this.sizeLevel < this.snowballSizeLevels.Count)
		{
			float num = scale / this.snowballSizeLevels[this.sizeLevel].snowballScale;
			SlingshotProjectile.AOEKnockbackConfig aoeKnockbackConfig = this.snowballSizeLevels[this.sizeLevel].aoeKnockbackConfig;
			aoeKnockbackConfig.aeoInnerRadius *= num;
			aoeKnockbackConfig.aeoOuterRadius *= num;
			aoeKnockbackConfig.knockbackVelocity *= num;
			aoeKnockbackConfig.impactVelocityThreshold *= num;
			velocity *= this.snowballSizeLevels[this.sizeLevel].throwSpeedMultiplier;
			component.gravityMultiplier = this.snowballSizeLevels[this.sizeLevel].gravityMultiplier;
			component.impactEffectScaleMultiplier = this.snowballSizeLevels[this.sizeLevel].impactEffectScale;
			component.aoeKnockbackConfig = new SlingshotProjectile.AOEKnockbackConfig?(aoeKnockbackConfig);
			component.impactSoundVolumeOverride = new float?(this.snowballSizeLevels[this.sizeLevel].impactSoundVolume);
			component.impactSoundPitchOverride = new float?(this.snowballSizeLevels[this.sizeLevel].impactSoundPitch);
		}
		component.Launch(location, velocity, NetworkSystem.Instance.LocalPlayer, false, false, 0, scale, false, Color.white);
		if (string.IsNullOrEmpty(this.throwEventName))
		{
			PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
		}
		else
		{
			PlayerGameEvents.LaunchedProjectile(this.throwEventName);
		}
		component.OnImpact += this.OnProjectileImpact;
		return component;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001EF00 File Offset: 0x0001D100
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!(this.targetRig != null) || this.targetRig.creator == null || !this.targetRig.creator.IsLocal)
		{
			return;
		}
		SnowballThrowable snowballThrowable;
		if (((this.isLeftHanded && grabbingHand == EquipmentInteractor.instance.rightHand && EquipmentInteractor.instance.rightHandHeldEquipment == null) || (!this.isLeftHanded && grabbingHand == EquipmentInteractor.instance.leftHand && EquipmentInteractor.instance.leftHandHeldEquipment == null)) && (this.isLeftHanded ? SnowballMaker.rightHandInstance : SnowballMaker.leftHandInstance).TryCreateSnowball(this.matDataIndexes[0], out snowballThrowable))
		{
			GrowingSnowballThrowable growingSnowballThrowable = snowballThrowable as GrowingSnowballThrowable;
			if (growingSnowballThrowable != null)
			{
				growingSnowballThrowable.IncreaseSize(this.sizeLevel);
				GorillaTagger.Instance.StartVibration(!this.isLeftHanded, GorillaTagger.Instance.tapHapticStrength * 0.5f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
				base.SetSnowballActiveLocal(false);
			}
		}
	}

	// Token: 0x040005E7 RID: 1511
	public Transform snowballModelParentTransform;

	// Token: 0x040005E8 RID: 1512
	public Transform snowballModelTransform;

	// Token: 0x040005E9 RID: 1513
	public Vector3 modelParentOffset = Vector3.zero;

	// Token: 0x040005EA RID: 1514
	public Vector3 modelOffset = Vector3.zero;

	// Token: 0x040005EB RID: 1515
	public float modelRadius = 0.055f;

	// Token: 0x040005EC RID: 1516
	[Tooltip("Snowballs will combine into the larger snowball unless they are moving faster than this threshold.Then the faster moving snowball will go in to the more stationary hand")]
	public float combineBasedOnSpeedThreshold = 0.5f;

	// Token: 0x040005ED RID: 1517
	public SoundBankPlayer sizeIncreaseSoundBankPlayer;

	// Token: 0x040005EE RID: 1518
	public List<GrowingSnowballThrowable.SizeParameters> snowballSizeLevels = new List<GrowingSnowballThrowable.SizeParameters>();

	// Token: 0x040005EF RID: 1519
	private int sizeLevel;

	// Token: 0x040005F0 RID: 1520
	private bool resetSizeOnNextEnable;

	// Token: 0x040005F1 RID: 1521
	private PhotonEvent changeSizeEvent;

	// Token: 0x040005F2 RID: 1522
	private PhotonEvent snowballThrowEvent;

	// Token: 0x040005F3 RID: 1523
	private CallLimiter snowballThrowCallLimit = new CallLimiter(10, 2f, 0.5f);

	// Token: 0x040005F4 RID: 1524
	[HideInInspector]
	public static bool debugDrawAOERange = false;

	// Token: 0x040005F5 RID: 1525
	[HideInInspector]
	public static bool twoHandedSnowballGrowing = true;

	// Token: 0x040005F6 RID: 1526
	private Queue<GrowingSnowballThrowable.AOERangeDebugDraw> aoeRangeDebugDrawQueue = new Queue<GrowingSnowballThrowable.AOERangeDebugDraw>();

	// Token: 0x040005F7 RID: 1527
	private GrowingSnowballThrowable otherHandSnowball;

	// Token: 0x040005F8 RID: 1528
	private float debugDrawAOERangeTime = 1.5f;

	// Token: 0x020000C6 RID: 198
	[Serializable]
	public struct SizeParameters
	{
		// Token: 0x040005F9 RID: 1529
		public float snowballScale;

		// Token: 0x040005FA RID: 1530
		public float impactEffectScale;

		// Token: 0x040005FB RID: 1531
		public float impactSoundVolume;

		// Token: 0x040005FC RID: 1532
		public float impactSoundPitch;

		// Token: 0x040005FD RID: 1533
		public float throwSpeedMultiplier;

		// Token: 0x040005FE RID: 1534
		public float gravityMultiplier;

		// Token: 0x040005FF RID: 1535
		public SlingshotProjectile.AOEKnockbackConfig aoeKnockbackConfig;
	}

	// Token: 0x020000C7 RID: 199
	private struct AOERangeDebugDraw
	{
		// Token: 0x04000600 RID: 1536
		public float impactTime;

		// Token: 0x04000601 RID: 1537
		public Vector3 position;

		// Token: 0x04000602 RID: 1538
		public float innerRadius;

		// Token: 0x04000603 RID: 1539
		public float outerRadius;
	}
}
