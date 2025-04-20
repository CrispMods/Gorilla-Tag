using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class FakeWheelDriver : MonoBehaviour
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x000365FE File Offset: 0x000347FE
	// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00036606 File Offset: 0x00034806
	public bool hasCollision { get; private set; }

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003660F File Offset: 0x0003480F
	public void SetThrust(Vector3 thrust)
	{
		this.thrust = thrust;
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x000903E0 File Offset: 0x0008E5E0
	private void OnCollisionStay(Collision collision)
	{
		int num = 0;
		Vector3 a = Vector3.zero;
		foreach (ContactPoint contactPoint in collision.contacts)
		{
			if (contactPoint.thisCollider == this.wheelCollider)
			{
				a += contactPoint.point;
				num++;
			}
		}
		if (num > 0)
		{
			this.collisionNormal = collision.contacts[0].normal;
			this.collisionPoint = a / (float)num;
			this.hasCollision = true;
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0009046C File Offset: 0x0008E66C
	private void FixedUpdate()
	{
		if (this.hasCollision)
		{
			Vector3 vector = base.transform.rotation * this.thrust;
			if (this.myRigidBody.velocity.IsShorterThan(this.maxSpeed))
			{
				vector = vector.ProjectOntoPlane(this.collisionNormal).normalized * this.thrust.magnitude;
				this.myRigidBody.AddForceAtPosition(vector, this.collisionPoint);
			}
			Vector3 vector2 = this.myRigidBody.velocity.ProjectOntoPlane(this.collisionNormal).ProjectOntoPlane(vector.normalized);
			if (vector2.IsLongerThan(this.lateralFrictionForce))
			{
				this.myRigidBody.AddForceAtPosition(-vector2.normalized * this.lateralFrictionForce, this.collisionPoint);
			}
			else
			{
				this.myRigidBody.AddForceAtPosition(-vector2, this.collisionPoint);
			}
		}
		this.hasCollision = false;
	}

	// Token: 0x04000AB0 RID: 2736
	[SerializeField]
	private Rigidbody myRigidBody;

	// Token: 0x04000AB1 RID: 2737
	[SerializeField]
	private Vector3 thrust;

	// Token: 0x04000AB2 RID: 2738
	[SerializeField]
	private Collider wheelCollider;

	// Token: 0x04000AB3 RID: 2739
	[SerializeField]
	private float maxSpeed;

	// Token: 0x04000AB4 RID: 2740
	[SerializeField]
	private float lateralFrictionForce;

	// Token: 0x04000AB6 RID: 2742
	private Vector3 collisionPoint;

	// Token: 0x04000AB7 RID: 2743
	private Vector3 collisionNormal;
}
