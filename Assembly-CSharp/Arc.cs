using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
[Serializable]
public struct Arc
{
	// Token: 0x06002AD9 RID: 10969 RVA: 0x0004CE48 File Offset: 0x0004B048
	public Vector3[] GetArcPoints(int count = 12)
	{
		return Arc.ComputeArcPoints(this.start, this.end, new Vector3?(this.control), count);
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	public void DrawGizmo()
	{
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x0011F474 File Offset: 0x0011D674
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

	// Token: 0x06002ADC RID: 10972 RVA: 0x0011F4BC File Offset: 0x0011D6BC
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

	// Token: 0x06002ADD RID: 10973 RVA: 0x0011F55C File Offset: 0x0011D75C
	public static Vector3 BezierLerp(Vector3 a, Vector3 b, Vector3 c, float t)
	{
		Vector3 a2 = Vector3.Lerp(a, c, t);
		Vector3 b2 = Vector3.Lerp(c, b, t);
		return Vector3.Lerp(a2, b2, t);
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x0011F584 File Offset: 0x0011D784
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

	// Token: 0x04003063 RID: 12387
	public Vector3 start;

	// Token: 0x04003064 RID: 12388
	public Vector3 end;

	// Token: 0x04003065 RID: 12389
	public Vector3 control;
}
