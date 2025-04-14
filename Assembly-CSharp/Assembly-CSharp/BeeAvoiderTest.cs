using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class BeeAvoiderTest : MonoBehaviour
{
	// Token: 0x060005D8 RID: 1496 RVA: 0x0002319C File Offset: 0x0002139C
	public void Update()
	{
		Vector3 position = this.patrolPoints[this.nextPatrolPoint].transform.position;
		Vector3 position2 = base.transform.position;
		Vector3 target = (position - position2).normalized * this.speed;
		this.velocity = Vector3.MoveTowards(this.velocity * this.drag, target, this.acceleration);
		if ((position2 - position).IsLongerThan(this.instabilityOffRadius))
		{
			this.velocity += Random.insideUnitSphere * this.instability * Time.deltaTime;
		}
		Vector3 vector = position2 + this.velocity * Time.deltaTime;
		GameObject[] array = this.avoidancePoints;
		for (int i = 0; i < array.Length; i++)
		{
			Vector3 position3 = array[i].transform.position;
			if ((vector - position3).IsShorterThan(this.avoidRadius))
			{
				Vector3 normalized = Vector3.Cross(position3 - vector, position - vector).normalized;
				Vector3 normalized2 = (position - position3).normalized;
				float num = Vector3.Dot(vector - position3, normalized);
				Vector3 b = (this.avoidRadius - num) * normalized;
				vector += b;
				this.velocity += b;
			}
		}
		base.transform.position = vector;
		base.transform.rotation = Quaternion.LookRotation(position - vector);
		if ((vector - position).IsShorterThan(this.patrolArrivedRadius))
		{
			this.nextPatrolPoint = (this.nextPatrolPoint + 1) % this.patrolPoints.Length;
		}
	}

	// Token: 0x0400070A RID: 1802
	public GameObject[] patrolPoints;

	// Token: 0x0400070B RID: 1803
	public GameObject[] avoidancePoints;

	// Token: 0x0400070C RID: 1804
	public float speed;

	// Token: 0x0400070D RID: 1805
	public float acceleration;

	// Token: 0x0400070E RID: 1806
	public float instability;

	// Token: 0x0400070F RID: 1807
	public float instabilityOffRadius;

	// Token: 0x04000710 RID: 1808
	public float drag;

	// Token: 0x04000711 RID: 1809
	public float avoidRadius;

	// Token: 0x04000712 RID: 1810
	public float patrolArrivedRadius;

	// Token: 0x04000713 RID: 1811
	private int nextPatrolPoint;

	// Token: 0x04000714 RID: 1812
	private Vector3 velocity;
}
