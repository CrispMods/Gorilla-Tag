﻿using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CA4 RID: 3236
	public class VectorUtil
	{
		// Token: 0x060051B0 RID: 20912 RVA: 0x0019072C File Offset: 0x0018E92C
		public static Vector3 Rotate2D(Vector3 v, float deg)
		{
			Vector3 result = v;
			float num = Mathf.Cos(MathUtil.Deg2Rad * deg);
			float num2 = Mathf.Sin(MathUtil.Deg2Rad * deg);
			result.x = num * v.x - num2 * v.y;
			result.y = num2 * v.x + num * v.y;
			return result;
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x00190786 File Offset: 0x0018E986
		public static Vector3 NormalizeSafe(Vector3 v, Vector3 fallback)
		{
			if (v.sqrMagnitude <= MathUtil.Epsilon)
			{
				return fallback;
			}
			return v.normalized;
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001907A0 File Offset: 0x0018E9A0
		public static Vector3 FindOrthogonal(Vector3 v)
		{
			if (Mathf.Abs(v.x) >= MathUtil.Sqrt3Inv)
			{
				return Vector3.Normalize(new Vector3(v.y, -v.x, 0f));
			}
			return Vector3.Normalize(new Vector3(0f, v.z, -v.y));
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001907F8 File Offset: 0x0018E9F8
		public static void FormOrthogonalBasis(Vector3 v, out Vector3 a, out Vector3 b)
		{
			a = VectorUtil.FindOrthogonal(v);
			b = Vector3.Cross(a, v);
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x00190818 File Offset: 0x0018EA18
		public static Vector3 Integrate(Vector3 x, Vector3 v, float dt)
		{
			return x + v * dt;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x00190828 File Offset: 0x0018EA28
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

		// Token: 0x060051B6 RID: 20918 RVA: 0x001908AC File Offset: 0x0018EAAC
		public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			float d = t * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * d + (-p0 + 3f * p1 - 3f * p2 + p3) * d * t);
		}
	}
}
