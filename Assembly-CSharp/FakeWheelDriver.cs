using System;
using Cinemachine.Utility;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class FakeWheelDriver : MonoBehaviour
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0002F7C1 File Offset: 0x0002D9C1
	// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0002F7C9 File Offset: 0x0002D9C9
	public bool hasCollision { get; private set; }

	// Token: 0x060008B5 RID: 2229 RVA: 0x0002F7D2 File Offset: 0x0002D9D2
	public void SetThrust(Vector3 thrust)
	{
		this.thrust = thrust;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0002F7DC File Offset: 0x0002D9DC
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

	// Token: 0x060008B7 RID: 2231 RVA: 0x0002F868 File Offset: 0x0002DA68
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

	// Token: 0x04000A6D RID: 2669
	[SerializeField]
	private Rigidbody myRigidBody;

	// Token: 0x04000A6E RID: 2670
	[SerializeField]
	private Vector3 thrust;

	// Token: 0x04000A6F RID: 2671
	[SerializeField]
	private Collider wheelCollider;

	// Token: 0x04000A70 RID: 2672
	[SerializeField]
	private float maxSpeed;

	// Token: 0x04000A71 RID: 2673
	[SerializeField]
	private float lateralFrictionForce;

	// Token: 0x04000A73 RID: 2675
	private Vector3 collisionPoint;

	// Token: 0x04000A74 RID: 2676
	private Vector3 collisionNormal;
}
