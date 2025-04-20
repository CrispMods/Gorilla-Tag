using System;
using GorillaExtensions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020008DD RID: 2269
public class ThrowableBug : TransferrableObject
{
	// Token: 0x060036E9 RID: 14057 RVA: 0x00145CE0 File Offset: 0x00143EE0
	protected override void Start()
	{
		base.Start();
		float f = UnityEngine.Random.Range(0f, 6.2831855f);
		this.targetVelocity = new Vector3(Mathf.Sin(f) * this.maxNaturalSpeed, 0f, Mathf.Cos(f) * this.maxNaturalSpeed);
		this.currentState = TransferrableObject.PositionState.Dropped;
		this.rayCastNonAllocColliders = new RaycastHit[5];
		this.rayCastNonAllocColliders2 = new RaycastHit[5];
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x00145D5C File Offset: 0x00143F5C
	internal override void OnEnable()
	{
		base.OnEnable();
		ThrowableBugBeacon.OnCall += this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss += this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock += this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock += this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier += this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x00145DC4 File Offset: 0x00143FC4
	internal override void OnDisable()
	{
		base.OnDisable();
		ThrowableBugBeacon.OnCall -= this.ThrowableBugBeacon_OnCall;
		ThrowableBugBeacon.OnDismiss -= this.ThrowableBugBeacon_OnDismiss;
		ThrowableBugBeacon.OnLock -= this.ThrowableBugBeacon_OnLock;
		ThrowableBugBeacon.OnUnlock -= this.ThrowableBugBeacon_OnUnlock;
		ThrowableBugBeacon.OnChangeSpeedMultiplier -= this.ThrowableBugBeacon_OnChangeSpeedMultiplier;
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x00145E2C File Offset: 0x0014402C
	private bool isValid(ThrowableBugBeacon tbb)
	{
		return tbb.BugName == this.bugName && (tbb.Range <= 0f || Vector3.Distance(tbb.transform.position, base.transform.position) <= tbb.Range);
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x00054463 File Offset: 0x00052663
	private void ThrowableBugBeacon_OnCall(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
		}
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x00145E80 File Offset: 0x00144080
	private void ThrowableBugBeacon_OnLock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = tbb.transform.position - base.transform.position;
			this.lockedTarget = tbb.transform;
			this.locked = true;
		}
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x00054494 File Offset: 0x00052694
	private void ThrowableBugBeacon_OnDismiss(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.reliableState.travelingDirection = base.transform.position - tbb.transform.position;
			this.locked = false;
		}
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x000544CC File Offset: 0x000526CC
	private void ThrowableBugBeacon_OnUnlock(ThrowableBugBeacon tbb)
	{
		if (this.isValid(tbb))
		{
			this.locked = false;
		}
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x000544DE File Offset: 0x000526DE
	private void ThrowableBugBeacon_OnChangeSpeedMultiplier(ThrowableBugBeacon tbb, float f)
	{
		if (this.isValid(tbb))
		{
			this.speedMultiplier = f;
		}
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool ShouldBeKinematic()
	{
		return true;
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00145ED0 File Offset: 0x001440D0
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

	// Token: 0x060036F4 RID: 14068 RVA: 0x0014608C File Offset: 0x0014428C
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

	// Token: 0x060036F5 RID: 14069 RVA: 0x000544F0 File Offset: 0x000526F0
	private float RandomizeBobingFrequency()
	{
		return UnityEngine.Random.Range(this.minRandFrequency, this.maxRandFrequency);
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x0014660C File Offset: 0x0014480C
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

	// Token: 0x060036F7 RID: 14071 RVA: 0x00054503 File Offset: 0x00052703
	public void OnCollisionEnter(Collision collision)
	{
		this.reliableState.travelingDirection *= -1f;
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x001466A0 File Offset: 0x001448A0
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

	// Token: 0x0400391A RID: 14618
	public ThrowableBugReliableState reliableState;

	// Token: 0x0400391B RID: 14619
	public float slowingDownProgress;

	// Token: 0x0400391C RID: 14620
	public float startingSpeed;

	// Token: 0x0400391D RID: 14621
	public float bobingSpeed = 1f;

	// Token: 0x0400391E RID: 14622
	public float bobMagnintude = 0.1f;

	// Token: 0x0400391F RID: 14623
	public bool shouldRandomizeFrequency;

	// Token: 0x04003920 RID: 14624
	public float minRandFrequency = 0.008f;

	// Token: 0x04003921 RID: 14625
	public float maxRandFrequency = 1f;

	// Token: 0x04003922 RID: 14626
	public float bobingFrequency = 1f;

	// Token: 0x04003923 RID: 14627
	public float bobingState;

	// Token: 0x04003924 RID: 14628
	public float thrownYVelocity;

	// Token: 0x04003925 RID: 14629
	public float collisionHitRadius;

	// Token: 0x04003926 RID: 14630
	public LayerMask collisionCheckMask;

	// Token: 0x04003927 RID: 14631
	public Vector3 thrownVeloicity;

	// Token: 0x04003928 RID: 14632
	public Vector3 targetVelocity;

	// Token: 0x04003929 RID: 14633
	public Quaternion bugRotationalVelocity;

	// Token: 0x0400392A RID: 14634
	private RaycastHit[] rayCastNonAllocColliders;

	// Token: 0x0400392B RID: 14635
	private RaycastHit[] rayCastNonAllocColliders2;

	// Token: 0x0400392C RID: 14636
	public VRRig followingRig;

	// Token: 0x0400392D RID: 14637
	public bool isTooHighTravelingDown;

	// Token: 0x0400392E RID: 14638
	public float descentSlerp;

	// Token: 0x0400392F RID: 14639
	public float ascentSlerp;

	// Token: 0x04003930 RID: 14640
	public float maxNaturalSpeed;

	// Token: 0x04003931 RID: 14641
	public float slowdownAcceleration;

	// Token: 0x04003932 RID: 14642
	public float maximumHeightOffOfTheGroundBeforeStartingDescent = 5f;

	// Token: 0x04003933 RID: 14643
	public float minimumHeightOffOfTheGroundBeforeStoppingDescent = 3f;

	// Token: 0x04003934 RID: 14644
	public float descentRate = 0.2f;

	// Token: 0x04003935 RID: 14645
	public float descentSlerpRate = 0.2f;

	// Token: 0x04003936 RID: 14646
	public float minimumHeightOffOfTheGroundBeforeStartingAscent = 0.5f;

	// Token: 0x04003937 RID: 14647
	public float maximumHeightOffOfTheGroundBeforeStoppingAscent = 0.75f;

	// Token: 0x04003938 RID: 14648
	public float ascentRate = 0.4f;

	// Token: 0x04003939 RID: 14649
	public float ascentSlerpRate = 1f;

	// Token: 0x0400393A RID: 14650
	private bool isTooLowTravelingUp;

	// Token: 0x0400393B RID: 14651
	public Animator animator;

	// Token: 0x0400393C RID: 14652
	[FormerlySerializedAs("grabBugAudioSource")]
	public AudioClip grabBugAudioClip;

	// Token: 0x0400393D RID: 14653
	[FormerlySerializedAs("releaseBugAudioSource")]
	public AudioClip releaseBugAudioClip;

	// Token: 0x0400393E RID: 14654
	[FormerlySerializedAs("flyingBugAudioSource")]
	public AudioClip flyingBugAudioClip;

	// Token: 0x0400393F RID: 14655
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04003940 RID: 14656
	private float bobbingDefaultFrequency = 1f;

	// Token: 0x04003941 RID: 14657
	public int updateMultiplier;

	// Token: 0x04003942 RID: 14658
	private ThrowableBug.AudioState currentAudioState;

	// Token: 0x04003943 RID: 14659
	private float speedMultiplier = 1f;

	// Token: 0x04003944 RID: 14660
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04003945 RID: 14661
	[SerializeField]
	private ThrowableBug.BugName bugName;

	// Token: 0x04003946 RID: 14662
	private Transform lockedTarget;

	// Token: 0x04003947 RID: 14663
	private bool locked;

	// Token: 0x04003948 RID: 14664
	private static readonly int _g_IsHeld = Animator.StringToHash("isHeld");

	// Token: 0x020008DE RID: 2270
	public enum BugName
	{
		// Token: 0x0400394A RID: 14666
		NONE,
		// Token: 0x0400394B RID: 14667
		DougTheBug,
		// Token: 0x0400394C RID: 14668
		MattTheBat
	}

	// Token: 0x020008DF RID: 2271
	private enum AudioState
	{
		// Token: 0x0400394E RID: 14670
		JustGrabbed,
		// Token: 0x0400394F RID: 14671
		ContinuallyGrabbed,
		// Token: 0x04003950 RID: 14672
		JustReleased,
		// Token: 0x04003951 RID: 14673
		NotHeld
	}
}
