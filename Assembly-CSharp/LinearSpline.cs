using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200087B RID: 2171
public class LinearSpline : MonoBehaviour
{
	// Token: 0x06003496 RID: 13462 RVA: 0x000FB2E4 File Offset: 0x000F94E4
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

	// Token: 0x06003497 RID: 13463 RVA: 0x000FB51E File Offset: 0x000F971E
	private void Awake()
	{
		this.RefreshControlPoints();
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x000FB528 File Offset: 0x000F9728
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

	// Token: 0x06003499 RID: 13465 RVA: 0x000FB874 File Offset: 0x000F9A74
	public Vector3 GetForwardTangent(float t, float step = 0.01f)
	{
		t = Mathf.Clamp(t, 0f, 1f - step - Mathf.Epsilon);
		Vector3 b = this.Evaluate(t);
		return (this.Evaluate(t + step) - b).normalized;
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x000FB8BC File Offset: 0x000F9ABC
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

	// Token: 0x04003754 RID: 14164
	public Transform[] controlPointTransforms = new Transform[0];

	// Token: 0x04003755 RID: 14165
	public Transform debugTransform;

	// Token: 0x04003756 RID: 14166
	public List<Vector3> controlPoints = new List<Vector3>();

	// Token: 0x04003757 RID: 14167
	public List<float> distances = new List<float>();

	// Token: 0x04003758 RID: 14168
	public List<LinearSpline.CurveBoundary> curveBoundaries = new List<LinearSpline.CurveBoundary>();

	// Token: 0x04003759 RID: 14169
	public bool roundCorners;

	// Token: 0x0400375A RID: 14170
	public float cornerRadius = 1f;

	// Token: 0x0400375B RID: 14171
	public bool looping;

	// Token: 0x0400375C RID: 14172
	public float testFloat;

	// Token: 0x0400375D RID: 14173
	public int gizmoResolution = 128;

	// Token: 0x0400375E RID: 14174
	public float totalDistance;

	// Token: 0x0200087C RID: 2172
	public struct CurveBoundary
	{
		// Token: 0x0400375F RID: 14175
		public Vector3 start;

		// Token: 0x04003760 RID: 14176
		public Vector3 end;
	}
}
