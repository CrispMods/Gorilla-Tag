using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE8 RID: 3304
	public class Collision
	{
		// Token: 0x0600534A RID: 21322 RVA: 0x0019A3CC File Offset: 0x001985CC
		public static bool SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB, out Vector3 push)
		{
			push = Vector3.zero;
			Vector3 v = centerA - centerB;
			float sqrMagnitude = v.sqrMagnitude;
			float num = radiusA + radiusB;
			if (sqrMagnitude >= num * num)
			{
				return false;
			}
			float num2 = Mathf.Sqrt(sqrMagnitude);
			push = VectorUtil.NormalizeSafe(v, Vector3.zero) * (num - num2);
			return true;
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x0019A434 File Offset: 0x00198634
		public static bool SphereSphereInverse(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB, out Vector3 push)
		{
			push = Vector3.zero;
			Vector3 v = centerB - centerA;
			float sqrMagnitude = v.sqrMagnitude;
			float num = radiusB - radiusA;
			if (sqrMagnitude <= num * num)
			{
				return false;
			}
			float num2 = Mathf.Sqrt(sqrMagnitude);
			push = VectorUtil.NormalizeSafe(v, Vector3.zero) * (num2 - num);
			return true;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x0019A49C File Offset: 0x0019869C
		public static bool SphereCapsule(Vector3 centerA, float radiusA, Vector3 headB, Vector3 tailB, float radiusB, out Vector3 push)
		{
			push = Vector3.zero;
			Vector3 a = tailB - headB;
			float sqrMagnitude = a.sqrMagnitude;
			if (sqrMagnitude < MathUtil.Epsilon)
			{
				return Collision.SphereSphereInverse(centerA, radiusA, 0.5f * (headB + tailB), radiusB, out push);
			}
			float num = 1f / Mathf.Sqrt(sqrMagnitude);
			Vector3 rhs = a * num;
			float t = Mathf.Clamp01(Vector3.Dot(centerA - headB, rhs) * num);
			Vector3 centerB = Vector3.Lerp(headB, tailB, t);
			return Collision.SphereSphere(centerA, radiusA, centerB, radiusB, out push);
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x0019A530 File Offset: 0x00198730
		public static bool SphereCapsuleInverse(Vector3 centerA, float radiusA, Vector3 headB, Vector3 tailB, float radiusB, out Vector3 push)
		{
			push = Vector3.zero;
			Vector3 a = tailB - headB;
			float sqrMagnitude = a.sqrMagnitude;
			if (sqrMagnitude < MathUtil.Epsilon)
			{
				return Collision.SphereSphereInverse(centerA, radiusA, 0.5f * (headB + tailB), radiusB, out push);
			}
			float num = 1f / Mathf.Sqrt(sqrMagnitude);
			Vector3 rhs = a * num;
			float t = Mathf.Clamp01(Vector3.Dot(centerA - headB, rhs) * num);
			Vector3 centerB = Vector3.Lerp(headB, tailB, t);
			return Collision.SphereSphereInverse(centerA, radiusA, centerB, radiusB, out push);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x0019A5C4 File Offset: 0x001987C4
		public static bool SphereBox(Vector3 centerOffsetA, float radiusA, Vector3 halfExtentB, out Vector3 push)
		{
			push = Vector3.zero;
			Vector3 b = new Vector3(Mathf.Clamp(centerOffsetA.x, -halfExtentB.x, halfExtentB.x), Mathf.Clamp(centerOffsetA.y, -halfExtentB.y, halfExtentB.y), Mathf.Clamp(centerOffsetA.z, -halfExtentB.z, halfExtentB.z));
			Vector3 v = centerOffsetA - b;
			float sqrMagnitude = v.sqrMagnitude;
			if (sqrMagnitude > radiusA * radiusA)
			{
				return false;
			}
			int num = ((centerOffsetA.x < -halfExtentB.x || centerOffsetA.x > halfExtentB.x) ? 0 : 1) + ((centerOffsetA.y < -halfExtentB.y || centerOffsetA.y > halfExtentB.y) ? 0 : 1) + ((centerOffsetA.z < -halfExtentB.z || centerOffsetA.z > halfExtentB.z) ? 0 : 1);
			if (num > 2)
			{
				if (num == 3)
				{
					Vector3 vector = new Vector3(halfExtentB.x - Mathf.Abs(centerOffsetA.x) + radiusA, halfExtentB.y - Mathf.Abs(centerOffsetA.y) + radiusA, halfExtentB.z - Mathf.Abs(centerOffsetA.z) + radiusA);
					if (vector.x < vector.y)
					{
						if (vector.x < vector.z)
						{
							push = new Vector3(Mathf.Sign(centerOffsetA.x) * vector.x, 0f, 0f);
						}
						else
						{
							push = new Vector3(0f, 0f, Mathf.Sign(centerOffsetA.z) * vector.z);
						}
					}
					else if (vector.y < vector.z)
					{
						push = new Vector3(0f, Mathf.Sign(centerOffsetA.y) * vector.y, 0f);
					}
					else
					{
						push = new Vector3(0f, 0f, Mathf.Sign(centerOffsetA.z) * vector.z);
					}
				}
			}
			else
			{
				push = VectorUtil.NormalizeSafe(v, Vector3.right) * (radiusA - Mathf.Sqrt(sqrMagnitude));
			}
			return true;
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x0019A805 File Offset: 0x00198A05
		public static bool SphereBoxInverse(Vector3 centerOffsetA, float radiusA, Vector3 halfExtentB, out Vector3 push)
		{
			push = Vector3.zero;
			return false;
		}
	}
}
