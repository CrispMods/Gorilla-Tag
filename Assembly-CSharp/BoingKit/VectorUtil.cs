﻿using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CF2 RID: 3314
	public class VectorUtil
	{
		// Token: 0x0600539D RID: 21405 RVA: 0x0019B8B8 File Offset: 0x00199AB8
		public static Vector3 Rotate2D(Vector3 v, float angle)
		{
			Vector3 result = v;
			float num = Mathf.Cos(angle);
			float num2 = Mathf.Sin(angle);
			result.x = num * v.x - num2 * v.y;
			result.y = num2 * v.x + num * v.y;
			return result;
		}

		// Token: 0x0600539E RID: 21406 RVA: 0x0019B906 File Offset: 0x00199B06
		public static Vector4 NormalizeSafe(Vector4 v, Vector4 fallback)
		{
			if (v.sqrMagnitude <= MathUtil.Epsilon)
			{
				return fallback;
			}
			return v.normalized;
		}

		// Token: 0x0600539F RID: 21407 RVA: 0x0019B91F File Offset: 0x00199B1F
		public static Vector3 FindOrthogonal(Vector3 v)
		{
			if (v.x >= MathUtil.Sqrt3Inv)
			{
				return new Vector3(v.y, -v.x, 0f);
			}
			return new Vector3(0f, v.z, -v.y);
		}

		// Token: 0x060053A0 RID: 21408 RVA: 0x0019B95D File Offset: 0x00199B5D
		public static void FormOrthogonalBasis(Vector3 v, out Vector3 a, out Vector3 b)
		{
			a = VectorUtil.FindOrthogonal(v);
			b = Vector3.Cross(a, v);
		}

		// Token: 0x060053A1 RID: 21409 RVA: 0x0019B980 File Offset: 0x00199B80
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

		// Token: 0x060053A2 RID: 21410 RVA: 0x0019BA04 File Offset: 0x00199C04
		public static Vector3 GetClosestPointOnSegment(Vector3 p, Vector3 segA, Vector3 segB)
		{
			Vector3 a = segB - segA;
			if (a.sqrMagnitude < MathUtil.Epsilon)
			{
				return 0.5f * (segA + segB);
			}
			float d = Mathf.Clamp01(Vector3.Dot(p - segA, a.normalized) / a.magnitude);
			return segA + d * a;
		}

		// Token: 0x060053A3 RID: 21411 RVA: 0x0019BA68 File Offset: 0x00199C68
		public static Vector3 TriLerp(ref Vector3 v000, ref Vector3 v001, ref Vector3 v010, ref Vector3 v011, ref Vector3 v100, ref Vector3 v101, ref Vector3 v110, ref Vector3 v111, float tx, float ty, float tz)
		{
			Vector3 a = Vector3.Lerp(v000, v001, tx);
			Vector3 b = Vector3.Lerp(v010, v011, tx);
			Vector3 a2 = Vector3.Lerp(v100, v101, tx);
			Vector3 b2 = Vector3.Lerp(v110, v111, tx);
			Vector3 a3 = Vector3.Lerp(a, b, ty);
			Vector3 b3 = Vector3.Lerp(a2, b2, ty);
			return Vector3.Lerp(a3, b3, tz);
		}

		// Token: 0x060053A4 RID: 21412 RVA: 0x0019BAE4 File Offset: 0x00199CE4
		public static Vector3 TriLerp(ref Vector3 v000, ref Vector3 v001, ref Vector3 v010, ref Vector3 v011, ref Vector3 v100, ref Vector3 v101, ref Vector3 v110, ref Vector3 v111, bool lerpX, bool lerpY, bool lerpZ, float tx, float ty, float tz)
		{
			Vector3 vector = lerpX ? Vector3.Lerp(v000, v001, tx) : v000;
			Vector3 b = lerpX ? Vector3.Lerp(v010, v011, tx) : v010;
			Vector3 vector2 = lerpX ? Vector3.Lerp(v100, v101, tx) : v100;
			Vector3 b2 = lerpX ? Vector3.Lerp(v110, v111, tx) : v110;
			Vector3 vector3 = lerpY ? Vector3.Lerp(vector, b, ty) : vector;
			Vector3 b3 = lerpY ? Vector3.Lerp(vector2, b2, ty) : vector2;
			if (!lerpZ)
			{
				return vector3;
			}
			return Vector3.Lerp(vector3, b3, tz);
		}

		// Token: 0x060053A5 RID: 21413 RVA: 0x0019BBB0 File Offset: 0x00199DB0
		public static Vector3 TriLerp(ref Vector3 min, ref Vector3 max, bool lerpX, bool lerpY, bool lerpZ, float tx, float ty, float tz)
		{
			Vector3 vector = lerpX ? Vector3.Lerp(new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z), tx) : new Vector3(min.x, min.y, min.z);
			Vector3 b = lerpX ? Vector3.Lerp(new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z), tx) : new Vector3(min.x, max.y, min.z);
			Vector3 vector2 = lerpX ? Vector3.Lerp(new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z), tx) : new Vector3(min.x, min.y, max.z);
			Vector3 b2 = lerpX ? Vector3.Lerp(new Vector3(min.x, max.y, max.z), new Vector3(max.x, max.y, max.z), tx) : new Vector3(min.x, max.y, max.z);
			Vector3 vector3 = lerpY ? Vector3.Lerp(vector, b, ty) : vector;
			Vector3 b3 = lerpY ? Vector3.Lerp(vector2, b2, ty) : vector2;
			if (!lerpZ)
			{
				return vector3;
			}
			return Vector3.Lerp(vector3, b3, tz);
		}

		// Token: 0x060053A6 RID: 21414 RVA: 0x0019BD3C File Offset: 0x00199F3C
		public static Vector4 TriLerp(ref Vector4 v000, ref Vector4 v001, ref Vector4 v010, ref Vector4 v011, ref Vector4 v100, ref Vector4 v101, ref Vector4 v110, ref Vector4 v111, bool lerpX, bool lerpY, bool lerpZ, float tx, float ty, float tz)
		{
			Vector4 vector = lerpX ? Vector4.Lerp(v000, v001, tx) : v000;
			Vector4 b = lerpX ? Vector4.Lerp(v010, v011, tx) : v010;
			Vector4 vector2 = lerpX ? Vector4.Lerp(v100, v101, tx) : v100;
			Vector4 b2 = lerpX ? Vector4.Lerp(v110, v111, tx) : v110;
			Vector4 vector3 = lerpY ? Vector4.Lerp(vector, b, ty) : vector;
			Vector4 b3 = lerpY ? Vector4.Lerp(vector2, b2, ty) : vector2;
			if (!lerpZ)
			{
				return vector3;
			}
			return Vector4.Lerp(vector3, b3, tz);
		}

		// Token: 0x060053A7 RID: 21415 RVA: 0x0019BE08 File Offset: 0x0019A008
		public static Vector4 TriLerp(ref Vector4 min, ref Vector4 max, bool lerpX, bool lerpY, bool lerpZ, float tx, float ty, float tz)
		{
			Vector4 vector = lerpX ? Vector4.Lerp(new Vector4(min.x, min.y, min.z), new Vector4(max.x, min.y, min.z), tx) : new Vector4(min.x, min.y, min.z);
			Vector4 b = lerpX ? Vector4.Lerp(new Vector4(min.x, max.y, min.z), new Vector4(max.x, max.y, min.z), tx) : new Vector4(min.x, max.y, min.z);
			Vector4 vector2 = lerpX ? Vector4.Lerp(new Vector4(min.x, min.y, max.z), new Vector4(max.x, min.y, max.z), tx) : new Vector4(min.x, min.y, max.z);
			Vector4 b2 = lerpX ? Vector4.Lerp(new Vector4(min.x, max.y, max.z), new Vector4(max.x, max.y, max.z), tx) : new Vector4(min.x, max.y, max.z);
			Vector4 vector3 = lerpY ? Vector4.Lerp(vector, b, ty) : vector;
			Vector4 b3 = lerpY ? Vector4.Lerp(vector2, b2, ty) : vector2;
			if (!lerpZ)
			{
				return vector3;
			}
			return Vector4.Lerp(vector3, b3, tz);
		}

		// Token: 0x060053A8 RID: 21416 RVA: 0x0019BF94 File Offset: 0x0019A194
		public static Vector3 ClampLength(Vector3 v, float minLen, float maxLen)
		{
			float sqrMagnitude = v.sqrMagnitude;
			if (sqrMagnitude < MathUtil.Epsilon)
			{
				return v;
			}
			float num = Mathf.Sqrt(sqrMagnitude);
			return v * (Mathf.Clamp(num, minLen, maxLen) / num);
		}

		// Token: 0x060053A9 RID: 21417 RVA: 0x0019BFCA File Offset: 0x0019A1CA
		public static float MinComponent(Vector3 v)
		{
			return Mathf.Min(v.x, Mathf.Min(v.y, v.z));
		}

		// Token: 0x060053AA RID: 21418 RVA: 0x0019BFE8 File Offset: 0x0019A1E8
		public static float MaxComponent(Vector3 v)
		{
			return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
		}

		// Token: 0x060053AB RID: 21419 RVA: 0x0019C006 File Offset: 0x0019A206
		public static Vector3 ComponentWiseAbs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x060053AC RID: 21420 RVA: 0x0019C02E File Offset: 0x0019A22E
		public static Vector3 ComponentWiseMult(Vector3 a, Vector3 b)
		{
			return Vector3.Scale(a, b);
		}

		// Token: 0x060053AD RID: 21421 RVA: 0x0019C037 File Offset: 0x0019A237
		public static Vector3 ComponentWiseDiv(Vector3 num, Vector3 den)
		{
			return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);
		}

		// Token: 0x060053AE RID: 21422 RVA: 0x0019C065 File Offset: 0x0019A265
		public static Vector3 ComponentWiseDivSafe(Vector3 num, Vector3 den)
		{
			return new Vector3(num.x * MathUtil.InvSafe(den.x), num.y * MathUtil.InvSafe(den.y), num.z * MathUtil.InvSafe(den.z));
		}

		// Token: 0x060053AF RID: 21423 RVA: 0x0019C0A4 File Offset: 0x0019A2A4
		public static Vector3 ClampBend(Vector3 vector, Vector3 reference, float maxBendAngle)
		{
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < MathUtil.Epsilon)
			{
				return vector;
			}
			float sqrMagnitude2 = reference.sqrMagnitude;
			if (sqrMagnitude2 < MathUtil.Epsilon)
			{
				return vector;
			}
			Vector3 rhs = vector / Mathf.Sqrt(sqrMagnitude);
			Vector3 vector2 = reference / Mathf.Sqrt(sqrMagnitude2);
			Vector3 vector3 = Vector3.Cross(vector2, rhs);
			float value = Vector3.Dot(vector2, rhs);
			Vector3 axis = (vector3.sqrMagnitude > MathUtil.Epsilon) ? vector3.normalized : VectorUtil.FindOrthogonal(vector2);
			if (Mathf.Acos(Mathf.Clamp01(value)) <= maxBendAngle)
			{
				return vector;
			}
			return QuaternionUtil.AxisAngle(axis, maxBendAngle) * reference * (Mathf.Sqrt(sqrMagnitude) / Mathf.Sqrt(sqrMagnitude2));
		}

		// Token: 0x040055DB RID: 21979
		public static readonly Vector3 Min = new Vector3(float.MinValue, float.MinValue, float.MinValue);

		// Token: 0x040055DC RID: 21980
		public static readonly Vector3 Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
	}
}
