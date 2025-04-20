using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class BalloonDynamics : MonoBehaviour, ITetheredObjectBehavior
{
	// Token: 0x060014B2 RID: 5298 RVA: 0x000BD244 File Offset: 0x000BB444
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.knotRb = this.knot.GetComponent<Rigidbody>();
		this.balloonCollider = base.GetComponent<Collider>();
		this.grabPtInitParent = this.grabPt.transform.parent;
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x0003DF2D File Offset: 0x0003C12D
	private void Start()
	{
		this.airResistance = Mathf.Clamp(this.airResistance, 0f, 1f);
		this.balloonCollider.enabled = false;
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x000BD290 File Offset: 0x000BB490
	public void ReParent()
	{
		if (this.grabPt != null)
		{
			this.grabPt.transform.parent = this.grabPtInitParent.transform;
		}
		this.bouyancyActualHeight = UnityEngine.Random.Range(this.bouyancyMinHeight, this.bouyancyMaxHeight);
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000BD2E0 File Offset: 0x000BB4E0
	private void ApplyBouyancyForce()
	{
		float num = this.bouyancyActualHeight + Mathf.Sin(Time.time) * this.varianceMaxheight;
		float num2 = (num - base.transform.position.y) / num;
		float y = this.bouyancyForce * num2 * this.balloonScale;
		this.rb.AddForce(new Vector3(0f, y, 0f), ForceMode.Acceleration);
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000BD348 File Offset: 0x000BB548
	private void ApplyUpRightForce()
	{
		Vector3 torque = Vector3.Cross(base.transform.up, Vector3.up) * this.upRightTorque * this.balloonScale;
		this.rb.AddTorque(torque);
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x000BD390 File Offset: 0x000BB590
	private void ApplyAntiSpinForce()
	{
		Vector3 vector = this.rb.transform.InverseTransformDirection(this.rb.angularVelocity);
		this.rb.AddRelativeTorque(0f, -vector.y * this.antiSpinTorque, 0f);
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x0003DF56 File Offset: 0x0003C156
	private void ApplyAirResistance()
	{
		this.rb.velocity *= 1f - this.airResistance;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x000BD3DC File Offset: 0x000BB5DC
	private void ApplyDistanceConstraint()
	{
		this.knot.transform.position - base.transform.position;
		Vector3 vector = this.grabPt.transform.position - this.knot.transform.position;
		Vector3 normalized = vector.normalized;
		float magnitude = vector.magnitude;
		float num = this.stringLength * this.balloonScale;
		if (magnitude > num)
		{
			Vector3 vector2 = Vector3.Dot(this.knotRb.velocity, normalized) * normalized;
			float num2 = magnitude - num;
			float num3 = num2 / Time.fixedDeltaTime;
			if (vector2.magnitude < num3)
			{
				float b = num3 - vector2.magnitude;
				float num4 = Mathf.Clamp01(num2 / this.stringStretch);
				Vector3 force = Mathf.Lerp(0f, b, num4 * num4) * normalized * this.stringStrength;
				this.rb.AddForceAtPosition(force, this.knot.transform.position, ForceMode.VelocityChange);
			}
		}
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x000BD4E8 File Offset: 0x000BB6E8
	public void EnableDynamics(bool enable, bool collider, bool kinematic)
	{
		bool flag = !this.enableDynamics && enable;
		this.enableDynamics = enable;
		if (this.balloonCollider)
		{
			this.balloonCollider.enabled = collider;
		}
		if (this.rb != null)
		{
			this.rb.isKinematic = kinematic;
			if (!kinematic && flag)
			{
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
			}
		}
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x0003DF7A File Offset: 0x0003C17A
	public void EnableDistanceConstraints(bool enable, float scale = 1f)
	{
		this.enableDistanceConstraints = enable;
		this.balloonScale = scale;
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x060014BC RID: 5308 RVA: 0x0003DF8A File Offset: 0x0003C18A
	public bool ColliderEnabled
	{
		get
		{
			return this.balloonCollider && this.balloonCollider.enabled;
		}
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x000BD564 File Offset: 0x000BB764
	private void FixedUpdate()
	{
		if (this.enableDynamics && !this.rb.isKinematic)
		{
			this.ApplyBouyancyForce();
			if (this.antiSpinTorque > 0f)
			{
				this.ApplyAntiSpinForce();
			}
			this.ApplyUpRightForce();
			this.ApplyAirResistance();
			if (this.enableDistanceConstraints)
			{
				this.ApplyDistanceConstraint();
			}
			Vector3 velocity = this.rb.velocity;
			float magnitude = velocity.magnitude;
			this.rb.velocity = velocity.normalized * Mathf.Min(magnitude, this.maximumVelocity * this.balloonScale);
		}
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000306DC File Offset: 0x0002E8DC
	void ITetheredObjectBehavior.DbgClear()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x0003DFA6 File Offset: 0x0003C1A6
	bool ITetheredObjectBehavior.IsEnabled()
	{
		return base.enabled;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000BD5F8 File Offset: 0x000BB7F8
	void ITetheredObjectBehavior.TriggerEnter(Collider other, ref Vector3 force, ref Vector3 collisionPt, ref bool transferOwnership)
	{
		if (!other.gameObject.IsOnLayer(UnityLayer.GorillaHand))
		{
			return;
		}
		if (!this.rb)
		{
			return;
		}
		transferOwnership = true;
		TransformFollow component = other.gameObject.GetComponent<TransformFollow>();
		if (!component)
		{
			return;
		}
		Vector3 a = (component.transform.position - component.prevPos) / Time.deltaTime;
		force = a * this.bopSpeed;
		force = Mathf.Min(this.maximumVelocity, force.magnitude) * force.normalized * this.balloonScale;
		if (this.bopSpeedCap > 0f && force.IsLongerThan(this.bopSpeedCap))
		{
			force = force.normalized * this.bopSpeedCap;
		}
		collisionPt = other.ClosestPointOnBounds(base.transform.position);
		this.rb.AddForceAtPosition(force, collisionPt, ForceMode.VelocityChange);
		if (this.balloonBopSource != null)
		{
			this.balloonBopSource.GTPlay();
		}
		GorillaTriggerColliderHandIndicator component2 = other.GetComponent<GorillaTriggerColliderHandIndicator>();
		if (component2 != null)
		{
			float amplitude = GorillaTagger.Instance.tapHapticStrength / 4f;
			float fixedDeltaTime = Time.fixedDeltaTime;
			GorillaTagger.Instance.StartVibration(component2.isLeftHand, amplitude, fixedDeltaTime);
		}
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x00039846 File Offset: 0x00037A46
	public bool ReturnStep()
	{
		return true;
	}

	// Token: 0x040016DE RID: 5854
	private Rigidbody rb;

	// Token: 0x040016DF RID: 5855
	private Collider balloonCollider;

	// Token: 0x040016E0 RID: 5856
	private Bounds bounds;

	// Token: 0x040016E1 RID: 5857
	public float bouyancyForce = 1f;

	// Token: 0x040016E2 RID: 5858
	public float bouyancyMinHeight = 10f;

	// Token: 0x040016E3 RID: 5859
	public float bouyancyMaxHeight = 20f;

	// Token: 0x040016E4 RID: 5860
	private float bouyancyActualHeight = 20f;

	// Token: 0x040016E5 RID: 5861
	public float varianceMaxheight = 5f;

	// Token: 0x040016E6 RID: 5862
	public float airResistance = 0.01f;

	// Token: 0x040016E7 RID: 5863
	public GameObject knot;

	// Token: 0x040016E8 RID: 5864
	private Rigidbody knotRb;

	// Token: 0x040016E9 RID: 5865
	public Transform grabPt;

	// Token: 0x040016EA RID: 5866
	private Transform grabPtInitParent;

	// Token: 0x040016EB RID: 5867
	public float stringLength = 2f;

	// Token: 0x040016EC RID: 5868
	public float stringStrength = 0.9f;

	// Token: 0x040016ED RID: 5869
	public float stringStretch = 0.1f;

	// Token: 0x040016EE RID: 5870
	public float maximumVelocity = 2f;

	// Token: 0x040016EF RID: 5871
	public float upRightTorque = 1f;

	// Token: 0x040016F0 RID: 5872
	public float antiSpinTorque;

	// Token: 0x040016F1 RID: 5873
	private bool enableDynamics;

	// Token: 0x040016F2 RID: 5874
	private bool enableDistanceConstraints;

	// Token: 0x040016F3 RID: 5875
	public float balloonScale = 1f;

	// Token: 0x040016F4 RID: 5876
	public float bopSpeed = 1f;

	// Token: 0x040016F5 RID: 5877
	public float bopSpeedCap;

	// Token: 0x040016F6 RID: 5878
	[SerializeField]
	private AudioSource balloonBopSource;
}
