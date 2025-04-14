using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008C4 RID: 2244
public class ThrowableBug : TransferrableObject
{
	// Token: 0x0600362D RID: 13869 RVA: 0x001009F4 File Offset: 0x000FEBF4
	protected override void Start()
	{
		base.Start();
		float f = Random.Range(0f, 6.2831855f);
		this.targetVelocity = new Vector3(Mathf.Sin(f) * this.maxNaturalSpeed, 0f, Mathf.Cos(f) * this.maxNaturalSpeed);
		this.currentState = TransferrableObject.PositionState.Dropped;
		this.rayCastNonAllocColliders = new RaycastHit[5];
		this.rayCastNonAllocColliders2 = new RaycastHit[5];
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00100A70 File Offset: 0x000FEC70
	internal override void OnEnable()
	{
		base.OnEnable();
		ThrowableBugBeacon.OnCall += this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss += this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock += this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock += this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier += this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x00100AD8 File Offset: 0x000FECD8
	internal override void OnDisable()
	{
		base.OnDisable();
		ThrowableBugBeacon.OnCall -= this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss -= this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock -= this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock -= this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier -= this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x00100B40 File Offset: 0x000FED40
	private bool isValid(ThrowableBugBeacon tbb)
	{
		return tbb.BugName == this.bugName && (tbb.Range <= 0f || Vector3.Distance(tbb.transform.position, base.transform.position) <= tbb.Range);
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x00100B92 File Offset: 0x000FED92
	private void ThrowableBugBeacon_OnCall(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
		}
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x00100BC4 File Offset: 0x000FEDC4
	private void ThrowableBugBeacon_OnLock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
			this.lockedTarget = tbb.transform;
			this.locked = true;
		}
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00100C13 File Offset: 0x000FEE13
	private void ThrowableBugBeacon_OnDismiss(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = base.transform.position - tbb.transform.position;
			this.locked = false;
		}
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x00100C4B File Offset: 0x000FEE4B
	private void ThrowableBugBeacon_OnUnlock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.locked = false;
		}
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x00100C5D File Offset: 0x000FEE5D
	private void ThrowableBugBeacon_OnChangeSpeedMultiplier(ThrowableBugBeacon tbb, float f)
	{
		if (this.isValid(tbb))
		{
			this.speedMultiplier = f;
		}
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00044826 File Offset: 0x00042A26
	public override bool ShouldBeKinematic()
	{
		return true;
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x00100C70 File Offset: 0x000FEE70
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		bool flag = this.currentState == TransferrableObject.PositionState.InLeftHand || this.currentState == TransferrableObject.PositionState.InRightHand;
		if (this.animator.enabled)
		{
			this.animator.SetBool(ThrowableBug._g_IsHeld, flag);
		}
		if (!this.audioSource)
		{
			return;
		}
		switch (this.currentAudioState)
		{
		case ThrowableBug.AudioState.JustGrabbed:
			if (!flag)
			{
				this.currentAudioState = ThrowableBug.AudioState.JustReleased;
				return;
			}
			if (this.grabBugAudioClip && this.audioSource.clip != this.grabBugAudioClip)
			{
				this.audioSource.clip = this.grabBugAudioClip;
				this.audioSource.time = 0f;
				this.audioSource.GTPlay();
				return;
			}
			if (!this.audioSource.isPlaying)
			{
				this.currentAudioState = ThrowableBug.AudioState.ContinuallyGrabbed;
				return;
			}
			break;
		case ThrowableBug.AudioState.ContinuallyGrabbed:
			if (!flag)
			{
				this.currentAudioState = ThrowableBug.AudioState.JustReleased;
				return;
			}
			break;
		case ThrowableBug.AudioState.JustReleased:
			if (!flag)
			{
				if (this.releaseBugAudioClip && this.audioSource.clip != this.releaseBugAudioClip)
				{
					this.audioSource.clip = this.releaseBugAudioClip;
					this.audioSource.time = 0f;
					this.audioSource.GTPlay();
					return;
				}
				if (!this.audioSource.isPlaying)
				{
					this.currentAudioState = ThrowableBug.AudioState.NotHeld;
					return;
				}
			}
			else
			{
				this.currentAudioState = ThrowableBug.AudioState.JustGrabbed;
			}
			break;
		case ThrowableBug.AudioState.NotHeld:
			if (flag)
			{
				this.currentAudioState = ThrowableBug.AudioState.JustGrabbed;
				return;
			}
			if (this.flyingBugAudioClip && !this.audioSource.isPlaying)
			{
				this.audioSource.clip = this.flyingBugAudioClip;
				this.audioSource.time = 0f;
				this.audioSource.GTPlay();
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x00100E2C File Offset: 0x000FF02C
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!this.reliableState)
		{
			return;
		}
		if (this.currentState != TransferrableObject.PositionState.InLeftHand)
		{
			bool flag = this.currentState == TransferrableObject.PositionState.InRightHand;
		}
		if (this.currentState == TransferrableObject.PositionState.Dropped)
		{
			if (this.locked && Vector3.Distance(this.lockedTarget.position, base.transform.position) > 0.1f)
			{
				this.reliableState.travelingDirection = this.lockedTarget.position - base.transform.position;
			}
			if (this.slowingDownProgress < 1f)
			{
				this.slowingDownProgress += this.slowdownAcceleration * Time.deltaTime;
				float t = Mathf.SmoothStep(0f, 1f, this.slowingDownProgress);
				this.reliableState.travelingDirection = Vector3.Slerp(this.thrownVeloicity, this.targetVelocity, t);
			}
			else
			{
				this.reliableState.travelingDirection = this.reliableState.travelingDirection.normalized * this.maxNaturalSpeed;
			}
			this.bobingFrequency = (this.shouldRandomizeFrequency ? this.RandomizeBobingFrequency() : this.bobbingDefaultFrequency);
			float num = this.bobingState + this.bobingSpeed * Time.deltaTime;
			float num2 = Mathf.Sin(num / this.bobingFrequency) - Mathf.Sin(this.bobingState / this.bobingFrequency);
			Vector3 vector = Vector3.up * (num2 * this.bobMagnintude);
			this.bobingState = num;
			if (this.bobingState > 6.2831855f)
			{
				this.bobingState -= 6.2831855f;
			}
			vector += this.reliableState.travelingDirection * Time.deltaTime;
			int num3 = Physics.SphereCastNonAlloc(base.transform.position, this.collisionHitRadius, vector.normalized, this.rayCastNonAllocColliders, vector.magnitude, this.collisionCheckMask);
			float maxDistance = this.maximumHeightOffOfTheGroundBeforeStartingDescent;
			if (this.isTooHighTravelingDown)
			{
				maxDistance = this.minimumHeightOffOfTheGroundBeforeStoppingDescent;
			}
			float num4 = this.minimumHeightOffOfTheGroundBeforeStartingAscent;
			if (this.isTooLowTravelingUp)
			{
				num4 = this.maximumHeightOffOfTheGroundBeforeStoppingAscent;
			}
			if (Physics.RaycastNonAlloc(base.transform.position, Vector3.down, this.rayCastNonAllocColliders2, maxDistance, this.collisionCheckMask) > 0)
			{
				this.isTooHighTravelingDown = false;
				if (this.descentSlerp > 0f)
				{
					this.descentSlerp = Mathf.Clamp01(this.descentSlerp - this.descentSlerpRate * Time.deltaTime);
				}
				RaycastHit raycastHit = this.rayCastNonAllocColliders2[0];
				this.isTooLowTravelingUp = (raycastHit.distance < num4);
				if (this.isTooLowTravelingUp)
				{
					if (this.ascentSlerp < 1f)
					{
						this.ascentSlerp = Mathf.Clamp01(this.ascentSlerp + this.ascentSlerpRate * Time.deltaTime);
					}
				}
				else if (this.ascentSlerp > 0f)
				{
					this.ascentSlerp = Mathf.Clamp01(this.ascentSlerp - this.ascentSlerpRate * Time.deltaTime);
				}
			}
			else
			{
				this.isTooHighTravelingDown = true;
				if (this.descentSlerp < 1f)
				{
					this.descentSlerp = Mathf.Clamp01(this.descentSlerp + this.descentSlerpRate * Time.deltaTime);
				}
			}
			vector += Time.deltaTime * Mathf.SmoothStep(0f, 1f, this.descentSlerp) * this.descentRate * Vector3.down;
			vector += Time.deltaTime * Mathf.SmoothStep(0f, 1f, this.ascentSlerp) * this.ascentRate * Vector3.up;
			float num5;
			Vector3 axis;
			Quaternion.FromToRotation(base.transform.rotation * Vector3.up, Quaternion.identity * Vector3.up).ToAngleAxis(out num5, out axis);
			Quaternion quaternion = Quaternion.AngleAxis(num5 * 0.02f, axis);
			float num6;
			Vector3 axis2;
			Quaternion.FromToRotation(base.transform.rotation * Vector3.forward, this.reliableState.travelingDirection.normalized).ToAngleAxis(out num6, out axis2);
			Quaternion lhs = Quaternion.AngleAxis(num6 * 0.005f, axis2);
			quaternion = lhs * quaternion;
			vector = quaternion * quaternion * quaternion * quaternion * vector;
			vector *= this.speedMultiplier;
			if (this.speedMultiplier != 1f)
			{
				this.speedMultiplier = Mathf.MoveTowards(this.speedMultiplier, 1f, Time.deltaTime);
			}
			if (num3 > 0)
			{
				Vector3 normal = this.rayCastNonAllocColliders[0].normal;
				this.reliableState.travelingDirection = Vector3.Reflect(this.reliableState.travelingDirection, normal).x0z();
				base.transform.position += Vector3.Reflect(vector, normal);
				this.thrownVeloicity = Vector3.Reflect(this.thrownVeloicity, normal);
				this.targetVelocity = Vector3.Reflect(this.targetVelocity, normal).x0z();
			}
			else
			{
				base.transform.position += vector;
			}
			this.bugRotationalVelocity = quaternion * this.bugRotationalVelocity;
			float num7;
			Vector3 axis3;
			this.bugRotationalVelocity.ToAngleAxis(out num7, out axis3);
			this.bugRotationalVelocity = Quaternion.AngleAxis(num7 * 0.9f, axis3);
			base.transform.rotation = this.bugRotationalVelocity * base.transform.rotation;
		}
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x001013A9 File Offset: 0x000FF5A9
	private float RandomizeBobingFrequency()
	{
		return Random.Range(this.minRandFrequency, this.maxRandFrequency);
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x001013BC File Offset: 0x000FF5BC
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.slowingDownProgress = 0f;
		Vector3 linearVelocity = this.velocityEstimator.linearVelocity;
		this.thrownVeloicity = linearVelocity;
		this.reliableState.travelingDirection = linearVelocity;
		this.bugRotationalVelocity = Quaternion.Euler(this.velocityEstimator.angularVelocity);
		this.startingSpeed = linearVelocity.magnitude;
		Vector3 normalized = this.reliableState.travelingDirection.x0z().normalized;
		this.targetVelocity = normalized * this.maxNaturalSpeed;
		return true;
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x0010144E File Offset: 0x000FF64E
	public void OnCollisionEnter(Collision collision)
	{
		this.reliableState.travelingDirection *= -1f;
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x0010146C File Offset: 0x000FF66C
	private void Update()
	{
		if (this.updateMultiplier > 0)
		{
			for (int i = 0; i < this.updateMultiplier; i++)
			{
				this.LateUpdateLocal();
			}
		}
	}

	// Token: 0x0400386B RID: 14443
	public ThrowableBugReliableState reliableState;

	// Token: 0x0400386C RID: 14444
	public float slowingDownProgress;

	// Token: 0x0400386D RID: 14445
	public float startingSpeed;

	// Token: 0x0400386E RID: 14446
	public float bobingSpeed = 1f;

	// Token: 0x0400386F RID: 14447
	public float bobMagnintude = 0.1f;

	// Token: 0x04003870 RID: 14448
	public bool shouldRandomizeFrequency;

	// Token: 0x04003871 RID: 14449
	public float minRandFrequency = 0.008f;

	// Token: 0x04003872 RID: 14450
	public float maxRandFrequency = 1f;

	// Token: 0x04003873 RID: 14451
	public float bobingFrequency = 1f;

	// Token: 0x04003874 RID: 14452
	public float bobingState;

	// Token: 0x04003875 RID: 14453
	public float thrownYVelocity;

	// Token: 0x04003876 RID: 14454
	public float collisionHitRadius;

	// Token: 0x04003877 RID: 14455
	public LayerMask collisionCheckMask;

	// Token: 0x04003878 RID: 14456
	public Vector3 thrownVeloicity;

	// Token: 0x04003879 RID: 14457
	public Vector3 targetVelocity;

	// Token: 0x0400387A RID: 14458
	public Quaternion bugRotationalVelocity;

	// Token: 0x0400387B RID: 14459
	private RaycastHit[] rayCastNonAllocColliders;

	// Token: 0x0400387C RID: 14460
	private RaycastHit[] rayCastNonAllocColliders2;

	// Token: 0x0400387D RID: 14461
	public VRRig followingRig;

	// Token: 0x0400387E RID: 14462
	public bool isTooHighTravelingDown;

	// Token: 0x0400387F RID: 14463
	public float descentSlerp;

	// Token: 0x04003880 RID: 14464
	public float ascentSlerp;

	// Token: 0x04003881 RID: 14465
	public float maxNaturalSpeed;

	// Token: 0x04003882 RID: 14466
	public float slowdownAcceleration;

	// Token: 0x04003883 RID: 14467
	public float maximumHeightOffOfTheGroundBeforeStartingDescent = 5f;

	// Token: 0x04003884 RID: 14468
	public float minimumHeightOffOfTheGroundBeforeStoppingDescent = 3f;

	// Token: 0x04003885 RID: 14469
	public float descentRate = 0.2f;

	// Token: 0x04003886 RID: 14470
	public float descentSlerpRate = 0.2f;

	// Token: 0x04003887 RID: 14471
	public float minimumHeightOffOfTheGroundBeforeStartingAscent = 0.5f;

	// Token: 0x04003888 RID: 14472
	public float maximumHeightOffOfTheGroundBeforeStoppingAscent = 0.75f;

	// Token: 0x04003889 RID: 14473
	public float ascentRate = 0.4f;

	// Token: 0x0400388A RID: 14474
	public float ascentSlerpRate = 1f;

	// Token: 0x0400388B RID: 14475
	private bool isTooLowTravelingUp;

	// Token: 0x0400388C RID: 14476
	public Animator animator;

	// Token: 0x0400388D RID: 14477
	[FormerlySerializedAs("grabBugAudioSource")]
	public AudioClip grabBugAudioClip;

	// Token: 0x0400388E RID: 14478
	[FormerlySerializedAs("releaseBugAudioSource")]
	public AudioClip releaseBugAudioClip;

	// Token: 0x0400388F RID: 14479
	[FormerlySerializedAs("flyingBugAudioSource")]
	public AudioClip flyingBugAudioClip;

	// Token: 0x04003890 RID: 14480
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04003891 RID: 14481
	private float bobbingDefaultFrequency = 1f;

	// Token: 0x04003892 RID: 14482
	public int updateMultiplier;

	// Token: 0x04003893 RID: 14483
	private ThrowableBug.AudioState currentAudioState;

	// Token: 0x04003894 RID: 14484
	private float speedMultiplier = 1f;

	// Token: 0x04003895 RID: 14485
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04003896 RID: 14486
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x04003897 RID: 14487
	private Transform lockedTarget;

	// Token: 0x04003898 RID: 14488
	private bool locked;

	// Token: 0x04003899 RID: 14489
	private static readonly int _g_IsHeld = Animator.StringToHash("isHeld");

	// Token: 0x020008C5 RID: 2245
	public enum BugName
	{
		// Token: 0x0400389B RID: 14491
		NONE,
		// Token: 0x0400389C RID: 14492
		DougTheBug,
		// Token: 0x0400389D RID: 14493
		MattTheBat
	}

	// Token: 0x020008C6 RID: 2246
	private enum AudioState
	{
		// Token: 0x0400389F RID: 14495
		JustGrabbed,
		// Token: 0x040038A0 RID: 14496
		ContinuallyGrabbed,
		// Token: 0x040038A1 RID: 14497
		JustReleased,
		// Token: 0x040038A2 RID: 14498
		NotHeld
	}
}
