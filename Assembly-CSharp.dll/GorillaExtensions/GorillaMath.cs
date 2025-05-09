﻿using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GorillaExtensions
{
	// Token: 0x02000B6F RID: 2927
	public static class GorillaMath
	{
		// Token: 0x0600492D RID: 18733 RVA: 0x00197818 File Offset: 0x00195A18
		public static Vector3 GetAngularVelocity(Quaternion oldRotation, Quaternion newRotation)
		{
			Quaternion quaternion = newRotation * Quaternion.Inverse(oldRotation);
			if (Mathf.Abs(quaternion.w) > 0.9995117f)
			{
				return Vector3.zero;
			}
			float num2;
			if (quaternion.w < 0f)
			{
				float num = Mathf.Acos(-quaternion.w);
				num2 = -2f * num / (Mathf.Sin(num) * Time.deltaTime);
			}
			else
			{
				float num3 = Mathf.Acos(quaternion.w);
				num2 = 2f * num3 / (Mathf.Sin(num3) * Time.deltaTime);
			}
			Vector3 zero = new Vector3(quaternion.x * num2, quaternion.y * num2, quaternion.z * num2);
			if (float.IsNaN(zero.z))
			{
				zero = Vector3.zero;
			}
			return zero;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x001978D4 File Offset: 0x00195AD4
		public static float FastInvSqrt(float z)
		{
			if (z == 0f)
			{
				return 0f;
			}
			GorillaMath.FloatIntUnion floatIntUnion;
			floatIntUnion.tmp = 0;
			float num = 0.5f * z;
			floatIntUnion.f = z;
			floatIntUnion.tmp = 1597463174 - (floatIntUnion.tmp >> 1);
			floatIntUnion.f *= 1.5f - num * floatIntUnion.f * floatIntUnion.f;
			return floatIntUnion.f * z;
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x0005ECF6 File Offset: 0x0005CEF6
		public static float Dot2(in Vector3 v)
		{
			return Vector3.Dot(v, v);
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00197948 File Offset: 0x00195B48
		public static Vector4 RaycastToCappedCone(in Vector3 rayOrigin, in Vector3 rayDirection, in Vector3 coneTip, in Vector3 coneBase, in float coneTipRadius, in float coneBaseRadius)
		{
			Vector3 vector = coneBase - coneTip;
			Vector3 vector2 = rayOrigin - coneTip;
			Vector3 vector3 = rayOrigin - coneBase;
			float num = Vector3.Dot(vector, vector);
			float num2 = Vector3.Dot(vector2, vector);
			float num3 = Vector3.Dot(vector3, vector);
			float num4 = Vector3.Dot(rayDirection, vector);
			if ((double)num2 < 0.0)
			{
				Vector3 vector4 = vector2 * num4 - rayDirection * num2;
				if (GorillaMath.Dot2(vector4) < coneTipRadius * coneTipRadius * num4 * num4)
				{
					Vector3 vector5 = -vector * GorillaMath.FastInvSqrt(num);
					return new Vector4(-num2 / num4, vector5.x, vector5.y, vector5.z);
				}
			}
			else if ((double)num3 > 0.0)
			{
				Vector3 vector4 = vector3 * num4 - rayDirection * num3;
				if (GorillaMath.Dot2(vector4) < coneBaseRadius * coneBaseRadius * num4 * num4)
				{
					Vector3 vector6 = vector * GorillaMath.FastInvSqrt(num);
					return new Vector4(-num3 / num4, vector6.x, vector6.y, vector6.z);
				}
			}
			float num5 = Vector3.Dot(rayDirection, vector2);
			float num6 = Vector3.Dot(vector2, vector2);
			float num7 = coneTipRadius - coneBaseRadius;
			float num8 = num + num7 * num7;
			float num9 = num * num - num4 * num4 * num8;
			float num10 = num * num * num5 - num2 * num4 * num8 + num * coneTipRadius * (num7 * num4 * 1f);
			float num11 = num * num * num6 - num2 * num2 * num8 + num * coneTipRadius * (num7 * num2 * 2f - num * coneTipRadius);
			float num12 = num10 * num10 - num9 * num11;
			if ((double)num12 < 0.0)
			{
				return -Vector4.one;
			}
			float num13 = (-num10 - Mathf.Sqrt(num12)) / num9;
			float num14 = num2 + num13 * num4;
			if ((double)num14 > 0.0 && num14 < num)
			{
				Vector3 vector4 = num * (num * (vector2 + num13 * rayDirection) + num7 * vector * coneTipRadius) - vector * num8 * num14;
				Vector3 normalized = vector4.normalized;
				return new Vector4(num13, normalized.x, normalized.y, normalized.z);
			}
			return -Vector4.one;
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00197BEC File Offset: 0x00195DEC
		public static void LineSegClosestPoints(Vector3 a, Vector3 u, Vector3 b, Vector3 v, out Vector3 lineAPoint, out Vector3 lineBPoint)
		{
			lineAPoint = a;
			lineBPoint = b;
			Vector3 lhs = b - a;
			float num = Vector3.Dot(lhs, u);
			float num2 = Vector3.Dot(lhs, v);
			float num3 = Vector3.Dot(u, u);
			float num4 = Vector3.Dot(u, v);
			float num5 = Vector3.Dot(v, v);
			float num6 = num3 * num5 - num4 * num4;
			if ((double)Mathf.Abs(num6) < 0.001)
			{
				return;
			}
			float num7 = (num * num5 - num2 * num4) / num6;
			float value = (num * num4 - num2 * num3) / num6;
			num7 = Mathf.Clamp(num7, 0f, 1f);
			float num8 = (Mathf.Clamp(value, 0f, 1f) * num4 + num) / num3;
			float num9 = (num7 * num4 - num2) / num5;
			num8 = Mathf.Clamp(num8, 0f, 1f);
			num9 = Mathf.Clamp(num9, 0f, 1f);
			lineAPoint = a + num8 * u;
			lineBPoint = b + num9 * v;
		}

		// Token: 0x02000B70 RID: 2928
		[Serializable]
		public struct RemapFloatInfo
		{
			// Token: 0x06004932 RID: 18738 RVA: 0x0005ED09 File Offset: 0x0005CF09
			public RemapFloatInfo(float fromMin = 0f, float toMin = 0f, float fromMax = 1f, float toMax = 1f)
			{
				this.fromMin = fromMin;
				this.toMin = toMin;
				this.fromMax = fromMax;
				this.toMax = toMax;
			}

			// Token: 0x06004933 RID: 18739 RVA: 0x00197CF4 File Offset: 0x00195EF4
			public void OnValidate()
			{
				if (this.fromMin < this.fromMax)
				{
					this.fromMin = this.fromMax + float.Epsilon;
				}
				if (this.toMin < this.toMax)
				{
					this.toMin = this.toMax + float.Epsilon;
				}
			}

			// Token: 0x06004934 RID: 18740 RVA: 0x0005ED28 File Offset: 0x0005CF28
			public bool IsValid()
			{
				return this.fromMin < this.fromMax && this.toMin < this.toMax;
			}

			// Token: 0x06004935 RID: 18741 RVA: 0x0005ED48 File Offset: 0x0005CF48
			public float Remap(float value)
			{
				return this.toMin + (value - this.fromMin) / (this.fromMax - this.fromMin) * (this.toMax - this.toMin);
			}

			// Token: 0x04004BDB RID: 19419
			public float fromMin;

			// Token: 0x04004BDC RID: 19420
			public float toMin;

			// Token: 0x04004BDD RID: 19421
			public float fromMax;

			// Token: 0x04004BDE RID: 19422
			public float toMax;
		}

		// Token: 0x02000B71 RID: 2929
		[StructLayout(LayoutKind.Explicit)]
		private struct FloatIntUnion
		{
			// Token: 0x04004BDF RID: 19423
			[FieldOffset(0)]
			public float f;

			// Token: 0x04004BE0 RID: 19424
			[FieldOffset(0)]
			public int tmp;
		}
	}
}
