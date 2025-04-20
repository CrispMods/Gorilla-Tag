using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class GrowingSnowballThrowable : SnowballThrowable
{
	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600054E RID: 1358 RVA: 0x00033F49 File Offset: 0x00032149
	public int SizeLevel
	{
		get
		{
			return this.sizeLevel;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600054F RID: 1359 RVA: 0x00033F51 File Offset: 0x00032151
	public int MaxSizeLevel
	{
		get
		{
			return Mathf.Max(this.snowballSizeLevels.Count - 1, 0);
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000550 RID: 1360 RVA: 0x00081064 File Offset: 0x0007F264
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

	// Token: 0x06000551 RID: 1361 RVA: 0x000810E4 File Offset: 0x0007F2E4
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

	// Token: 0x06000552 RID: 1362 RVA: 0x00081148 File Offset: 0x0007F348
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

	// Token: 0x06000553 RID: 1363 RVA: 0x00033F66 File Offset: 0x00032166
	public override void OnDisable()
	{
		base.OnDisable();
		this.resetSizeOnNextEnable = true;
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00033F75 File Offset: 0x00032175
	protected override void OnDestroy()
	{
		this.DestroyPhotonEvents();
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x000811C4 File Offset: 0x0007F3C4
	private void VRRigActivated(RigContainer rigContainer)
	{
		this.targetRig = base.GetComponentInParent<VRRig>(true);
		this.isOfflineRig = (this.targetRig != null && this.targetRig.isOfflineVRRig);
		if (rigContainer.Rig == this.targetRig)
		{
			this.CreatePhotonEventsIfNull();
		}
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00033F7D File Offset: 0x0003217D
	private void VRRigDeactivated(RigContainer rigContainer)
	{
		if (rigContainer.Rig == this.targetRig)
		{
			this.DestroyPhotonEvents();
		}
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0008121C File Offset: 0x0007F41C
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

	// Token: 0x06000558 RID: 1368 RVA: 0x0008126C File Offset: 0x0007F46C
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

	// Token: 0x06000559 RID: 1369 RVA: 0x00081440 File Offset: 0x0007F640
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

	// Token: 0x0600055A RID: 1370 RVA: 0x00033F98 File Offset: 0x00032198
	public void IncreaseSize(int increase)
	{
		this.SetSizeLevelAuthority(this.sizeLevel + increase);
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x000814C8 File Offset: 0x0007F6C8
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

	// Token: 0x0600055C RID: 1372 RVA: 0x00081544 File Offset: 0x0007F744
	private int GetValidSizeLevel(int inputSizeLevel)
	{
		int max = Mathf.Max(this.snowballSizeLevels.Count - 1, 0);
		return Mathf.Clamp(inputSizeLevel, 0, max);
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00081570 File Offset: 0x0007F770
	private void SetSizeLevelLocal(int sizeLevel)
	{
		int validSizeLevel = this.GetValidSizeLevel(sizeLevel);
		if (validSizeLevel >= 0 && validSizeLevel != this.sizeLevel)
		{
			this.sizeLevel = validSizeLevel;
			this.snowballModelParentTransform.localScale = Vector3.one * this.snowballSizeLevels[this.sizeLevel].snowballScale;
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000815C4 File Offset: 0x0007F7C4
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

	// Token: 0x0600055F RID: 1375 RVA: 0x00081694 File Offset: 0x0007F894
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
		if (this.targetRig.IsNull() || !this.targetRig.gameObject.activeSelf)
		{
			return;
		}
		NetPlayer creator = this.targetRig.creator;
		if (info.senderID != this.targetRig.creator.ActorNumber)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "SnowballThrowEventReceiver");
		if (!this.snowballThrowCallLimit.CheckCallTime(Time.time))
		{
			return;
		}
		object obj = args[0];
		if (obj is Vector3)
		{
			Vector3 vector = (Vector3)obj;
			obj = args[1];
			if (obj is Vector3)
			{
				Vector3 inVel = (Vector3)obj;
				obj = args[2];
				if (obj is int)
				{
					int index = (int)obj;
					Vector3 velocity = this.targetRig.ClampVelocityRelativeToPlayerSafe(inVel, 50f);
					float x = this.snowballModelTransform.lossyScale.x;
					float num = 10000f;
					if (!vector.IsValid(num) || !this.targetRig.IsPositionInRange(vector, 4f))
					{
						return;
					}
					this.LaunchSnowballRemote(vector, velocity, x, index, info);
					return;
				}
			}
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x000817B4 File Offset: 0x0007F9B4
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

	// Token: 0x06000561 RID: 1377 RVA: 0x00033FA8 File Offset: 0x000321A8
	protected override void OnSnowballRelease()
	{
		if (base.isActiveAndEnabled)
		{
			this.PerformSnowballThrowAuthority();
		}
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x000819A4 File Offset: 0x0007FBA4
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
			slingshotProjectile.myProjectileCount
		});
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00033FB8 File Offset: 0x000321B8
	protected virtual SlingshotProjectile LaunchSnowballLocal(Vector3 location, Vector3 velocity, float scale)
	{
		return this.LaunchSnowballLocal(location, velocity, scale, false, Color.white);
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00081B08 File Offset: 0x0007FD08
	protected override SlingshotProjectile LaunchSnowballLocal(Vector3 location, Vector3 velocity, float scale, bool randomizeColour, Color colour)
	{
		SlingshotProjectile slingshotProjectile = this.SpawnGrowingSnowball(ref velocity, scale);
		int projectileCount = ProjectileTracker.AddAndIncrementLocalProjectile(slingshotProjectile, velocity, location, scale);
		slingshotProjectile.Launch(location, velocity, NetworkSystem.Instance.LocalPlayer, false, false, projectileCount, scale, randomizeColour, colour);
		if (string.IsNullOrEmpty(this.throwEventName))
		{
			PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
		}
		else
		{
			PlayerGameEvents.LaunchedProjectile(this.throwEventName);
		}
		slingshotProjectile.OnImpact += this.OnProjectileImpact;
		return slingshotProjectile;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00033FC9 File Offset: 0x000321C9
	protected virtual SlingshotProjectile LaunchSnowballRemote(Vector3 location, Vector3 velocity, float scale, int index, PhotonMessageInfoWrapped info)
	{
		return this.LaunchSnowballRemote(location, velocity, scale, index, false, Color.white, info);
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00081B80 File Offset: 0x0007FD80
	protected virtual SlingshotProjectile LaunchSnowballRemote(Vector3 location, Vector3 velocity, float scale, int index, bool randomizeColour, Color colour, PhotonMessageInfoWrapped info)
	{
		SlingshotProjectile slingshotProjectile = this.SpawnGrowingSnowball(ref velocity, scale);
		ProjectileTracker.AddRemotePlayerProjectile(this.targetRig.creator, slingshotProjectile, index, info.SentServerTime, velocity, location, scale);
		slingshotProjectile.Launch(location, velocity, NetworkSystem.Instance.LocalPlayer, false, false, index, scale, randomizeColour, Color.white);
		if (string.IsNullOrEmpty(this.throwEventName))
		{
			PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
		}
		else
		{
			PlayerGameEvents.LaunchedProjectile(this.throwEventName);
		}
		slingshotProjectile.OnImpact += this.OnProjectileImpact;
		return slingshotProjectile;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00081C10 File Offset: 0x0007FE10
	private SlingshotProjectile SpawnGrowingSnowball(ref Vector3 velocity, float scale)
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
		return component;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00081D94 File Offset: 0x0007FF94
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

	// Token: 0x04000626 RID: 1574
	public Transform snowballModelParentTransform;

	// Token: 0x04000627 RID: 1575
	public Transform snowballModelTransform;

	// Token: 0x04000628 RID: 1576
	public Vector3 modelParentOffset = Vector3.zero;

	// Token: 0x04000629 RID: 1577
	public Vector3 modelOffset = Vector3.zero;

	// Token: 0x0400062A RID: 1578
	public float modelRadius = 0.055f;

	// Token: 0x0400062B RID: 1579
	[Tooltip("Snowballs will combine into the larger snowball unless they are moving faster than this threshold.Then the faster moving snowball will go in to the more stationary hand")]
	public float combineBasedOnSpeedThreshold = 0.5f;

	// Token: 0x0400062C RID: 1580
	public SoundBankPlayer sizeIncreaseSoundBankPlayer;

	// Token: 0x0400062D RID: 1581
	public List<GrowingSnowballThrowable.SizeParameters> snowballSizeLevels = new List<GrowingSnowballThrowable.SizeParameters>();

	// Token: 0x0400062E RID: 1582
	private int sizeLevel;

	// Token: 0x0400062F RID: 1583
	private bool resetSizeOnNextEnable;

	// Token: 0x04000630 RID: 1584
	private PhotonEvent changeSizeEvent;

	// Token: 0x04000631 RID: 1585
	private PhotonEvent snowballThrowEvent;

	// Token: 0x04000632 RID: 1586
	private CallLimiter snowballThrowCallLimit = new CallLimiter(10, 2f, 0.5f);

	// Token: 0x04000633 RID: 1587
	[HideInInspector]
	public static bool debugDrawAOERange = false;

	// Token: 0x04000634 RID: 1588
	[HideInInspector]
	public static bool twoHandedSnowballGrowing = true;

	// Token: 0x04000635 RID: 1589
	private Queue<GrowingSnowballThrowable.AOERangeDebugDraw> aoeRangeDebugDrawQueue = new Queue<GrowingSnowballThrowable.AOERangeDebugDraw>();

	// Token: 0x04000636 RID: 1590
	private GrowingSnowballThrowable otherHandSnowball;

	// Token: 0x04000637 RID: 1591
	private float debugDrawAOERangeTime = 1.5f;

	// Token: 0x020000D0 RID: 208
	[Serializable]
	public struct SizeParameters
	{
		// Token: 0x04000638 RID: 1592
		public float snowballScale;

		// Token: 0x04000639 RID: 1593
		public float impactEffectScale;

		// Token: 0x0400063A RID: 1594
		public float impactSoundVolume;

		// Token: 0x0400063B RID: 1595
		public float impactSoundPitch;

		// Token: 0x0400063C RID: 1596
		public float throwSpeedMultiplier;

		// Token: 0x0400063D RID: 1597
		public float gravityMultiplier;

		// Token: 0x0400063E RID: 1598
		public SlingshotProjectile.AOEKnockbackConfig aoeKnockbackConfig;
	}

	// Token: 0x020000D1 RID: 209
	private struct AOERangeDebugDraw
	{
		// Token: 0x0400063F RID: 1599
		public float impactTime;

		// Token: 0x04000640 RID: 1600
		public Vector3 position;

		// Token: 0x04000641 RID: 1601
		public float innerRadius;

		// Token: 0x04000642 RID: 1602
		public float outerRadius;
	}
}
