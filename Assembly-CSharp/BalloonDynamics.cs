using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class BalloonDynamics : MonoBehaviour, ITetheredObjectBehavior
{
	// Token: 0x06001466 RID: 5222 RVA: 0x000640D0 File Offset: 0x000622D0
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.knotRb = this.knot.GetComponent<Rigidbody>();
		this.balloonCollider = base.GetComponent<Collider>();
		this.grabPtInitParent = this.grabPt.transform.parent;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x0006411C File Offset: 0x0006231C
	private void Start()
	{
		this.airResistance = Mathf.Clamp(this.airResistance, 0f, 1f);
		this.balloonCollider.enabled = false;
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x00064148 File Offset: 0x00062348
	public void ReParent()
	{
		if (this.grabPt != null)
		{
			this.grabPt.transform.parent = this.grabPtInitParent.transform;
		}
		this.bouyancyActualHeight = Random.Range(this.bouyancyMinHeight, this.bouyancyMaxHeight);
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x00064198 File Offset: 0x00062398
	private void ApplyBouyancyForce()
	{
		float num = this.bouyancyActualHeight + Mathf.Sin(Time.time) * this.varianceMaxheight;
		float num2 = (num - base.transform.position.y) / num;
		float y = this.bouyancyForce * num2 * this.balloonScale;
		this.rb.AddForce(new Vector3(0f, y, 0f), ForceMode.Acceleration);
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x00064200 File Offset: 0x00062400
	private void ApplyUpRightForce()
	{
		Vector3 torque = Vector3.Cross(base.transform.up, Vector3.up) * this.upRightTorque * this.balloonScale;
		this.rb.AddTorque(torque);
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x00064248 File Offset: 0x00062448
	private void ApplyAntiSpinForce()
	{
		Vector3 vector = this.rb.transform.InverseTransformDirection(this.rb.angularVelocity);
		this.rb.AddRelativeTorque(0f, -vector.y * this.antiSpinTorque, 0f);
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x00064294 File Offset: 0x00062494
	private void ApplyAirResistance()
	{
		this.rb.velocity *= 1f - this.airResistance;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000642B8 File Offset: 0x000624B8
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

	// Token: 0x0600146E RID: 5230 RVA: 0x000643C4 File Offset: 0x000625C4
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

	// Token: 0x0600146F RID: 5231 RVA: 0x0006443F File Offset: 0x0006263F
	public void EnableDistanceConstraints(bool enable, float scale = 1f)
	{
		this.enableDistanceConstraints = enable;
		this.balloonScale = scale;
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06001470 RID: 5232 RVA: 0x0006444F File Offset: 0x0006264F
	public bool ColliderEnabled
	{
		get
		{
			return this.balloonCollider && this.balloonCollider.enabled;
		}
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x0006446C File Offset: 0x0006266C
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

	// Token: 0x06001472 RID: 5234 RVA: 0x00002628 File Offset: 0x00000828
	void ITetheredObjectBehavior.DbgClear()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x000644FF File Offset: 0x000626FF
	bool ITetheredObjectBehavior.IsEnabled()
	{
		return base.enabled;
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x00064508 File Offset: 0x00062708
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

	// Token: 0x06001475 RID: 5237 RVA: 0x000444E2 File Offset: 0x000426E2
	public bool ReturnStep()
	{
		return true;
	}

	// Token: 0x04001696 RID: 5782
	private Rigidbody rb;

	// Token: 0x04001697 RID: 5783
	private Collider balloonCollider;

	// Token: 0x04001698 RID: 5784
	private Bounds bounds;

	// Token: 0x04001699 RID: 5785
	public float bouyancyForce = 1f;

	// Token: 0x0400169A RID: 5786
	public float bouyancyMinHeight = 10f;

	// Token: 0x0400169B RID: 5787
	public float bouyancyMaxHeight = 20f;

	// Token: 0x0400169C RID: 5788
	private float bouyancyActualHeight = 20f;

	// Token: 0x0400169D RID: 5789
	public float varianceMaxheight = 5f;

	// Token: 0x0400169E RID: 5790
	public float airResistance = 0.01f;

	// Token: 0x0400169F RID: 5791
	public GameObject knot;

	// Token: 0x040016A0 RID: 5792
	private Rigidbody knotRb;

	// Token: 0x040016A1 RID: 5793
	public Transform grabPt;

	// Token: 0x040016A2 RID: 5794
	private Transform grabPtInitParent;

	// Token: 0x040016A3 RID: 5795
	public float stringLength = 2f;

	// Token: 0x040016A4 RID: 5796
	public float stringStrength = 0.9f;

	// Token: 0x040016A5 RID: 5797
	public float stringStretch = 0.1f;

	// Token: 0x040016A6 RID: 5798
	public float maximumVelocity = 2f;

	// Token: 0x040016A7 RID: 5799
	public float upRightTorque = 1f;

	// Token: 0x040016A8 RID: 5800
	public float antiSpinTorque;

	// Token: 0x040016A9 RID: 5801
	private bool enableDynamics;

	// Token: 0x040016AA RID: 5802
	private bool enableDistanceConstraints;

	// Token: 0x040016AB RID: 5803
	public float balloonScale = 1f;

	// Token: 0x040016AC RID: 5804
	public float bopSpeed = 1f;

	// Token: 0x040016AD RID: 5805
	public float bopSpeedCap;

	// Token: 0x040016AE RID: 5806
	[SerializeField]
	private AudioSource balloonBopSource;
}
