using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008C1 RID: 2241
public class ThrowableBug : TransferrableObject
{
	// Token: 0x06003621 RID: 13857 RVA: 0x0010042C File Offset: 0x000FE62C
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

	// Token: 0x06003622 RID: 13858 RVA: 0x001004A8 File Offset: 0x000FE6A8
	internal override void OnEnable()
	{
		base.OnEnable();
		ThrowableBugBeacon.OnCall += this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss += this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock += this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock += this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier += this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00100510 File Offset: 0x000FE710
	internal override void OnDisable()
	{
		base.OnDisable();
		ThrowableBugBeacon.OnCall -= this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss -= this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock -= this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock -= this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier -= this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x00100578 File Offset: 0x000FE778
	private bool isValid(ThrowableBugBeacon tbb)
	{
		return tbb.BugName == this.bugName && (tbb.Range <= 0f || Vector3.Distance(tbb.transform.position, base.transform.position) <= tbb.Range);
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x001005CA File Offset: 0x000FE7CA
	private void ThrowableBugBeacon_OnCall(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
		}
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x001005FC File Offset: 0x000FE7FC
	private void ThrowableBugBeacon_OnLock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
			this.lockedTarget = tbb.transform;
			this.locked = true;
		}
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x0010064B File Offset: 0x000FE84B
	private void ThrowableBugBeacon_OnDismiss(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = base.transform.position - tbb.transform.position;
			this.locked = false;
		}
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00100683 File Offset: 0x000FE883
	private void ThrowableBugBeacon_OnUnlock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.locked = false;
		}
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x00100695 File Offset: 0x000FE895
	private void ThrowableBugBeacon_OnChangeSpeedMultiplier(ThrowableBugBeacon tbb, float f)
	{
		if (this.isValid(tbb))
		{
			this.speedMultiplier = f;
		}
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x000444E2 File Offset: 0x000426E2
	public override bool ShouldBeKinematic()
	{
		return true;
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x001006A8 File Offset: 0x000FE8A8
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

	// Token: 0x0600362C RID: 13868 RVA: 0x00100864 File Offset: 0x000FEA64
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

	// Token: 0x0600362D RID: 13869 RVA: 0x00100DE1 File Offset: 0x000FEFE1
	private float RandomizeBobingFrequency()
	{
		return Random.Range(this.minRandFrequency, this.maxRandFrequency);
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x00100DF4 File Offset: 0x000FEFF4
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

	// Token: 0x0600362F RID: 13871 RVA: 0x00100E86 File Offset: 0x000FF086
	public void OnCollisionEnter(Collision collision)
	{
		this.reliableState.travelingDirection *= -1f;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x00100EA4 File Offset: 0x000FF0A4
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

	// Token: 0x04003859 RID: 14425
	public ThrowableBugReliableState reliableState;

	// Token: 0x0400385A RID: 14426
	public float slowingDownProgress;

	// Token: 0x0400385B RID: 14427
	public float startingSpeed;

	// Token: 0x0400385C RID: 14428
	public float bobingSpeed = 1f;

	// Token: 0x0400385D RID: 14429
	public float bobMagnintude = 0.1f;

	// Token: 0x0400385E RID: 14430
	public bool shouldRandomizeFrequency;

	// Token: 0x0400385F RID: 14431
	public float minRandFrequency = 0.008f;

	// Token: 0x04003860 RID: 14432
	public float maxRandFrequency = 1f;

	// Token: 0x04003861 RID: 14433
	public float bobingFrequency = 1f;

	// Token: 0x04003862 RID: 14434
	public float bobingState;

	// Token: 0x04003863 RID: 14435
	public float thrownYVelocity;

	// Token: 0x04003864 RID: 14436
	public float collisionHitRadius;

	// Token: 0x04003865 RID: 14437
	public LayerMask collisionCheckMask;

	// Token: 0x04003866 RID: 14438
	public Vector3 thrownVeloicity;

	// Token: 0x04003867 RID: 14439
	public Vector3 targetVelocity;

	// Token: 0x04003868 RID: 14440
	public Quaternion bugRotationalVelocity;

	// Token: 0x04003869 RID: 14441
	private RaycastHit[] rayCastNonAllocColliders;

	// Token: 0x0400386A RID: 14442
	private RaycastHit[] rayCastNonAllocColliders2;

	// Token: 0x0400386B RID: 14443
	public VRRig followingRig;

	// Token: 0x0400386C RID: 14444
	public bool isTooHighTravelingDown;

	// Token: 0x0400386D RID: 14445
	public float descentSlerp;

	// Token: 0x0400386E RID: 14446
	public float ascentSlerp;

	// Token: 0x0400386F RID: 14447
	public float maxNaturalSpeed;

	// Token: 0x04003870 RID: 14448
	public float slowdownAcceleration;

	// Token: 0x04003871 RID: 14449
	public float maximumHeightOffOfTheGroundBeforeStartingDescent = 5f;

	// Token: 0x04003872 RID: 14450
	public float minimumHeightOffOfTheGroundBeforeStoppingDescent = 3f;

	// Token: 0x04003873 RID: 14451
	public float descentRate = 0.2f;

	// Token: 0x04003874 RID: 14452
	public float descentSlerpRate = 0.2f;

	// Token: 0x04003875 RID: 14453
	public float minimumHeightOffOfTheGroundBeforeStartingAscent = 0.5f;

	// Token: 0x04003876 RID: 14454
	public float maximumHeightOffOfTheGroundBeforeStoppingAscent = 0.75f;

	// Token: 0x04003877 RID: 14455
	public float ascentRate = 0.4f;

	// Token: 0x04003878 RID: 14456
	public float ascentSlerpRate = 1f;

	// Token: 0x04003879 RID: 14457
	private bool isTooLowTravelingUp;

	// Token: 0x0400387A RID: 14458
	public Animator animator;

	// Token: 0x0400387B RID: 14459
	[FormerlySerializedAs("grabBugAudioSource")]
	public AudioClip grabBugAudioClip;

	// Token: 0x0400387C RID: 14460
	[FormerlySerializedAs("releaseBugAudioSource")]
	public AudioClip releaseBugAudioClip;

	// Token: 0x0400387D RID: 14461
	[FormerlySerializedAs("flyingBugAudioSource")]
	public AudioClip flyingBugAudioClip;

	// Token: 0x0400387E RID: 14462
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x0400387F RID: 14463
	private float bobbingDefaultFrequency = 1f;

	// Token: 0x04003880 RID: 14464
	public int updateMultiplier;

	// Token: 0x04003881 RID: 14465
	private ThrowableBug.AudioState currentAudioState;

	// Token: 0x04003882 RID: 14466
	private float speedMultiplier = 1f;

	// Token: 0x04003883 RID: 14467
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04003884 RID: 14468
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x04003885 RID: 14469
	private Transform lockedTarget;

	// Token: 0x04003886 RID: 14470
	private bool locked;

	// Token: 0x04003887 RID: 14471
	private static readonly int _g_IsHeld = Animator.StringToHash("isHeld");

	// Token: 0x020008C2 RID: 2242
	public enum BugName
	{
		// Token: 0x04003889 RID: 14473
		NONE,
		// Token: 0x0400388A RID: 14474
		DougTheBug,
		// Token: 0x0400388B RID: 14475
		MattTheBat
	}

	// Token: 0x020008C3 RID: 2243
	private enum AudioState
	{
		// Token: 0x0400388D RID: 14477
		JustGrabbed,
		// Token: 0x0400388E RID: 14478
		ContinuallyGrabbed,
		// Token: 0x0400388F RID: 14479
		JustReleased,
		// Token: 0x04003890 RID: 14480
		NotHeld
	}
}
