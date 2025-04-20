using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class BeeAvoiderTest : MonoBehaviour
{
	// Token: 0x06000617 RID: 1559 RVA: 0x00085700 File Offset: 0x00083900
	public void Update()
	{
		Vector3 position = this.patrolPoints[this.nextPatrolPoint].transform.position;
		Vector3 position2 = base.transform.position;
		Vector3 target = (position - position2).normalized * this.speed;
		this.velocity = Vector3.MoveTowards(this.velocity * this.drag, target, this.acceleration);
		if ((position2 - position).IsLongerThan(this.instabilityOffRadius))
		{
			this.velocity += UnityEngine.Random.insideUnitSphere * this.instability * Time.deltaTime;
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

	// Token: 0x0400074A RID: 1866
	public GameObject[] patrolPoints;

	// Token: 0x0400074B RID: 1867
	public GameObject[] avoidancePoints;

	// Token: 0x0400074C RID: 1868
	public float speed;

	// Token: 0x0400074D RID: 1869
	public float acceleration;

	// Token: 0x0400074E RID: 1870
	public float instability;

	// Token: 0x0400074F RID: 1871
	public float instabilityOffRadius;

	// Token: 0x04000750 RID: 1872
	public float drag;

	// Token: 0x04000751 RID: 1873
	public float avoidRadius;

	// Token: 0x04000752 RID: 1874
	public float patrolArrivedRadius;

	// Token: 0x04000753 RID: 1875
	private int nextPatrolPoint;

	// Token: 0x04000754 RID: 1876
	private Vector3 velocity;
}
