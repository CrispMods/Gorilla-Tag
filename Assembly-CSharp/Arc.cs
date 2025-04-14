using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006A3 RID: 1699
[Serializable]
public struct Arc
{
	// Token: 0x06002A43 RID: 10819 RVA: 0x000D2E9B File Offset: 0x000D109B
	public Vector3[] GetArcPoints(int count = 12)
	{
		return Arc.ComputeArcPoints(this.start, this.end, new Vector3?(this.control), count);
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public void DrawGizmo()
	{
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000D2EBC File Offset: 0x000D10BC
	public static Arc From(Vector3 start, Vector3 end)
	{
		Vector3 vector = Arc.DeriveArcControlPoint(start, end, null, null);
		return new Arc
		{
			start = start,
			end = end,
			control = vector
		};
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000D2F04 File Offset: 0x000D1104
	public static Vector3[] ComputeArcPoints(Vector3 a, Vector3 b, Vector3? c = null, int count = 12)
	{
		Vector3[] array = new Vector3[count];
		float num = 1f / (float)count;
		Vector3 value = c.GetValueOrDefault();
		if (c == null)
		{
			value = Arc.DeriveArcControlPoint(a, b, null, null);
			c = new Vector3?(value);
		}
		for (int i = 0; i < count; i++)
		{
			float t;
			if (i == 0)
			{
				t = 0f;
			}
			else if (i == count - 1)
			{
				t = 1f;
			}
			else
			{
				t = num * (float)i;
			}
			array[i] = Arc.BezierLerp(a, b, c.Value, t);
		}
		return array;
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000D2FA4 File Offset: 0x000D11A4
	public static Vector3 BezierLerp(Vector3 a, Vector3 b, Vector3 c, float t)
	{
		Vector3 a2 = Vector3.Lerp(a, c, t);
		Vector3 b2 = Vector3.Lerp(c, b, t);
		return Vector3.Lerp(a2, b2, t);
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000D2FCC File Offset: 0x000D11CC
	public static Vector3 DeriveArcControlPoint(Vector3 a, Vector3 b, Vector3? dir = null, float? height = null)
	{
		Vector3 b2 = (b - a) * 0.5f;
		Vector3 normalized = b2.normalized;
		float value = height.GetValueOrDefault();
		if (height == null)
		{
			value = b2.magnitude;
			height = new float?(value);
		}
		if (dir == null)
		{
			Vector3 rhs = Vector3.Cross(normalized, Vector3.up);
			dir = new Vector3?(Vector3.Cross(normalized, rhs));
		}
		Vector3 b3 = dir.Value * -height.Value;
		return a + b2 + b3;
	}

	// Token: 0x04002FC6 RID: 12230
	public Vector3 start;

	// Token: 0x04002FC7 RID: 12231
	public Vector3 end;

	// Token: 0x04002FC8 RID: 12232
	public Vector3 control;
}
