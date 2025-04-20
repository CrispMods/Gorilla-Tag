using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000897 RID: 2199
public class LinearSpline : MonoBehaviour
{
	// Token: 0x06003562 RID: 13666 RVA: 0x00141FD8 File Offset: 0x001401D8
	private void RefreshControlPoints()
	{
		this.controlPoints.Clear();
		for (int i = 0; i < this.controlPointTransforms.Length; i++)
		{
			this.controlPoints.Add(this.controlPointTransforms[i].position);
		}
		this.totalDistance = 0f;
		this.distances.Clear();
		for (int j = 1; j < this.controlPoints.Count; j++)
		{
			float num = Vector3.Distance(this.controlPoints[j - 1], this.controlPoints[j]);
			this.distances.Add(num);
			this.totalDistance += num;
		}
		float num2 = Vector3.Distance(this.controlPoints[this.controlPoints.Count - 1], this.controlPoints[0]);
		this.distances.Add(num2);
		if (this.looping)
		{
			this.totalDistance += num2;
		}
		this.curveBoundaries.Clear();
		if (this.roundCorners)
		{
			for (int k = 0; k < this.controlPoints.Count; k++)
			{
				int num3 = (k > 0) ? (k - 1) : (this.controlPoints.Count - 1);
				int index = (k + 1) % this.controlPoints.Count;
				float num4 = Mathf.Min(Mathf.Min(this.cornerRadius, this.distances[num3 % this.distances.Count] * 0.5f), this.distances[k % this.distances.Count] * 0.5f);
				this.curveBoundaries.Add(new LinearSpline.CurveBoundary
				{
					start = Vector3.Lerp(this.controlPoints[num3], this.controlPoints[k], 1f - num4 / this.distances[num3 % this.distances.Count]),
					end = Vector3.Lerp(this.controlPoints[k], this.controlPoints[index], num4 / this.distances[k])
				});
			}
		}
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x0005325A File Offset: 0x0005145A
	private void Awake()
	{
		this.RefreshControlPoints();
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x00142214 File Offset: 0x00140414
	public Vector3 Evaluate(float t)
	{
		if (this.controlPoints.Count < 1)
		{
			return Vector3.zero;
		}
		if (this.controlPoints.Count < 2)
		{
			return this.controlPoints[0];
		}
		if (this.controlPoints.Count < 3)
		{
			return Vector3.Lerp(this.controlPoints[0], this.controlPoints[1], t);
		}
		float num = Mathf.Clamp01(t) * this.totalDistance;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < this.distances.Count; i++)
		{
			if (this.looping || i != this.distances.Count - 1)
			{
				num2 = i;
				if (num - num4 <= this.distances[i])
				{
					num3 = Mathf.Clamp01((num - num4) / this.distances[i]);
					break;
				}
				num3 = 1f;
				num4 += this.distances[i];
			}
		}
		num2 %= this.controlPoints.Count;
		int num5 = (num2 + 1) % this.controlPoints.Count;
		if (this.roundCorners)
		{
			if (num3 > 0.5f && (this.looping || num2 < this.controlPoints.Count - 2))
			{
				int num6 = (num5 + 1) % this.controlPoints.Count;
				float num7 = Mathf.Min(Mathf.Min(this.cornerRadius, this.distances[num2] * 0.5f), this.distances[num5 % this.distances.Count] * 0.5f);
				float num8 = 1f - num7 / this.distances[num2];
				if (num3 > num8)
				{
					Vector3 start = this.curveBoundaries[num5].start;
					Vector3 end = this.curveBoundaries[num5].end;
					float t2 = 0.5f * Mathf.Clamp01((num3 - num8) / (1f - num8));
					Vector3 a = Vector3.Lerp(start, this.controlPoints[num5], t2);
					Vector3 b = Vector3.Lerp(this.controlPoints[num5], end, t2);
					return Vector3.Lerp(a, b, t2);
				}
			}
			else if (num3 <= 0.5f && (this.looping || num2 > 0))
			{
				int num9 = (num2 > 0) ? (num2 - 1) : (this.controlPoints.Count - 1);
				float num10 = Mathf.Min(Mathf.Min(this.cornerRadius, this.distances[num2] * 0.5f), this.distances[num9 % this.distances.Count] * 0.5f) / this.distances[num2];
				if (num3 < num10)
				{
					Vector3 start2 = this.curveBoundaries[num2].start;
					Vector3 end2 = this.curveBoundaries[num2].end;
					float t3 = 0.5f + 0.5f * Mathf.Clamp01(num3 / num10);
					Vector3 a2 = Vector3.Lerp(start2, this.controlPoints[num2], t3);
					Vector3 b2 = Vector3.Lerp(this.controlPoints[num2], end2, t3);
					return Vector3.Lerp(a2, b2, t3);
				}
			}
		}
		return Vector3.Lerp(this.controlPoints[num2], this.controlPoints[num5], num3);
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x00142560 File Offset: 0x00140760
	public Vector3 GetForwardTangent(float t, float step = 0.01f)
	{
		t = Mathf.Clamp(t, 0f, 1f - step - Mathf.Epsilon);
		Vector3 b = this.Evaluate(t);
		return (this.Evaluate(t + step) - b).normalized;
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x001425A8 File Offset: 0x001407A8
	private void OnDrawGizmosSelected()
	{
		this.RefreshControlPoints();
		Gizmos.color = Color.yellow;
		int num = this.gizmoResolution;
		Vector3 from = this.Evaluate(0f);
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector = this.Evaluate(t);
			Gizmos.DrawLine(from, vector);
			from = vector;
		}
		Vector3 to = this.Evaluate(1f);
		Gizmos.DrawLine(from, to);
	}

	// Token: 0x04003814 RID: 14356
	public Transform[] controlPointTransforms = new Transform[0];

	// Token: 0x04003815 RID: 14357
	public Transform debugTransform;

	// Token: 0x04003816 RID: 14358
	public List<Vector3> controlPoints = new List<Vector3>();

	// Token: 0x04003817 RID: 14359
	public List<float> distances = new List<float>();

	// Token: 0x04003818 RID: 14360
	public List<LinearSpline.CurveBoundary> curveBoundaries = new List<LinearSpline.CurveBoundary>();

	// Token: 0x04003819 RID: 14361
	public bool roundCorners;

	// Token: 0x0400381A RID: 14362
	public float cornerRadius = 1f;

	// Token: 0x0400381B RID: 14363
	public bool looping;

	// Token: 0x0400381C RID: 14364
	public float testFloat;

	// Token: 0x0400381D RID: 14365
	public int gizmoResolution = 128;

	// Token: 0x0400381E RID: 14366
	public float totalDistance;

	// Token: 0x02000898 RID: 2200
	public struct CurveBoundary
	{
		// Token: 0x0400381F RID: 14367
		public Vector3 start;

		// Token: 0x04003820 RID: 14368
		public Vector3 end;
	}
}
