﻿using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA7 RID: 3239
	public class VectorUtil
	{
		// Token: 0x060051BC RID: 20924 RVA: 0x001BEDF4 File Offset: 0x001BCFF4
		public static Vector3 Rotate2D(Vector3 v, float deg)
		{
			Vector3 result = v;
			float num = Mathf.Cos(MathUtil.Deg2Rad * deg);
			float num2 = Mathf.Sin(MathUtil.Deg2Rad * deg);
			result.x = num * v.x - num2 * v.y;
			result.y = num2 * v.x + num * v.y;
			return result;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x00064315 File Offset: 0x00062515
		public static Vector3 NormalizeSafe(Vector3 v, Vector3 fallback)
		{
			if (v.sqrMagnitude <= MathUtil.Epsilon)
			{
				return fallback;
			}
			return v.normalized;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x001BEE50 File Offset: 0x001BD050
		public static Vector3 FindOrthogonal(Vector3 v)
		{
			if (Mathf.Abs(v.x) >= MathUtil.Sqrt3Inv)
			{
				return Vector3.Normalize(new Vector3(v.y, -v.x, 0f));
			}
			return Vector3.Normalize(new Vector3(0f, v.z, -v.y));
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x0006432E File Offset: 0x0006252E
		public static void FormOrthogonalBasis(Vector3 v, out Vector3 a, out Vector3 b)
		{
			a = VectorUtil.FindOrthogonal(v);
			b = Vector3.Cross(a, v);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x0006434E File Offset: 0x0006254E
		public static Vector3 Integrate(Vector3 x, Vector3 v, float dt)
		{
			return x + v * dt;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x001BEEA8 File Offset: 0x001BD0A8
		public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
		{
			float num = Vector3.Dot(a, b);
			if (num > 0.99999f)
			{
				return Vector3.Lerp(a, b, t);
			}
			if (num < -0.99999f)
			{
				Vector3 axis = VectorUtil.FindOrthogonal(a);
				return Quaternion.AngleAxis(180f * t, axis) * a;
			}
			float num2 = MathUtil.AcosSafe(num);
			return (Mathf.Sin((1f - t) * num2) * a + Mathf.Sin(t * num2) * b) / Mathf.Sin(num2);
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x001BEF2C File Offset: 0x001BD12C
		public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float d = t * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * d + (-p0 + 3f * p1 - 3f * p2 + p3) * d * t);
		}
	}
}
