using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D1B RID: 3355
	public class MathUtil
	{
		// Token: 0x060054B5 RID: 21685 RVA: 0x00065A84 File Offset: 0x00063C84
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x00065A9B File Offset: 0x00063C9B
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x00067011 File Offset: 0x00065211
		public static float InvSafe(float x)
		{
			return 1f / Mathf.Max(MathUtil.Epsilon, x);
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x001CFCA8 File Offset: 0x001CDEA8
		public static float PointLineDist(Vector2 point, Vector2 linePos, Vector2 lineDir)
		{
			Vector2 vector = point - linePos;
			return (vector - Vector2.Dot(vector, lineDir) * lineDir).magnitude;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001CFCD8 File Offset: 0x001CDED8
		public static float PointSegmentDist(Vector2 point, Vector2 segmentPosA, Vector2 segmentPosB)
		{
			Vector2 a = segmentPosB - segmentPosA;
			float num = 1f / a.magnitude;
			Vector2 rhs = a * num;
			float value = Vector2.Dot(point - segmentPosA, rhs) * num;
			return (segmentPosA + Mathf.Clamp(value, 0f, 1f) * a - point).magnitude;
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001CFD40 File Offset: 0x001CDF40
		public static float Seek(float current, float target, float maxDelta)
		{
			float num = target - current;
			num = Mathf.Sign(num) * Mathf.Min(maxDelta, Mathf.Abs(num));
			return current + num;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001CFD68 File Offset: 0x001CDF68
		public static Vector2 Seek(Vector2 current, Vector2 target, float maxDelta)
		{
			Vector2 b = target - current;
			float magnitude = b.magnitude;
			if (magnitude < MathUtil.Epsilon)
			{
				return target;
			}
			b = Mathf.Min(maxDelta, magnitude) * b.normalized;
			return current + b;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x00067024 File Offset: 0x00065224
		public static float Remainder(float a, float b)
		{
			return a - a / b * b;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x00067024 File Offset: 0x00065224
		public static int Remainder(int a, int b)
		{
			return a - a / b * b;
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x0006702D File Offset: 0x0006522D
		public static float Modulo(float a, float b)
		{
			return Mathf.Repeat(a, b);
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001CFDAC File Offset: 0x001CDFAC
		public static int Modulo(int a, int b)
		{
			int num = a % b;
			if (num < 0)
			{
				return num + b;
			}
			return num;
		}

		// Token: 0x040056C7 RID: 22215
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040056C8 RID: 22216
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040056C9 RID: 22217
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040056CA RID: 22218
		public static readonly float QuaterPi = 0.7853982f;

		// Token: 0x040056CB RID: 22219
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040056CC RID: 22220
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040056CD RID: 22221
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040056CE RID: 22222
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040056CF RID: 22223
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040056D0 RID: 22224
		public static readonly float Epsilon = 1E-06f;

		// Token: 0x040056D1 RID: 22225
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040056D2 RID: 22226
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
