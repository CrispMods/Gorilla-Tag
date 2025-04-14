using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CEA RID: 3306
	public class MathUtil
	{
		// Token: 0x06005353 RID: 21331 RVA: 0x0018F7D0 File Offset: 0x0018D9D0
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x0018F7E7 File Offset: 0x0018D9E7
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x0019A89C File Offset: 0x00198A9C
		public static float InvSafe(float x)
		{
			return 1f / Mathf.Max(MathUtil.Epsilon, x);
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x0019A8B0 File Offset: 0x00198AB0
		public static float PointLineDist(Vector2 point, Vector2 linePos, Vector2 lineDir)
		{
			Vector2 vector = point - linePos;
			return (vector - Vector2.Dot(vector, lineDir) * lineDir).magnitude;
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0019A8E0 File Offset: 0x00198AE0
		public static float PointSegmentDist(Vector2 point, Vector2 segmentPosA, Vector2 segmentPosB)
		{
			Vector2 a = segmentPosB - segmentPosA;
			float num = 1f / a.magnitude;
			Vector2 rhs = a * num;
			float value = Vector2.Dot(point - segmentPosA, rhs) * num;
			return (segmentPosA + Mathf.Clamp(value, 0f, 1f) * a - point).magnitude;
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0019A948 File Offset: 0x00198B48
		public static float Seek(float current, float target, float maxDelta)
		{
			float num = target - current;
			num = Mathf.Sign(num) * Mathf.Min(maxDelta, Mathf.Abs(num));
			return current + num;
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x0019A970 File Offset: 0x00198B70
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

		// Token: 0x0600535A RID: 21338 RVA: 0x0019A9B2 File Offset: 0x00198BB2
		public static float Remainder(float a, float b)
		{
			return a - a / b * b;
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0019A9B2 File Offset: 0x00198BB2
		public static int Remainder(int a, int b)
		{
			return a - a / b * b;
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0019A9BB File Offset: 0x00198BBB
		public static float Modulo(float a, float b)
		{
			return Mathf.Repeat(a, b);
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0019A9C4 File Offset: 0x00198BC4
		public static int Modulo(int a, int b)
		{
			int num = a % b;
			if (num < 0)
			{
				return num + b;
			}
			return num;
		}

		// Token: 0x040055BB RID: 21947
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040055BC RID: 21948
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040055BD RID: 21949
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040055BE RID: 21950
		public static readonly float QuaterPi = 0.7853982f;

		// Token: 0x040055BF RID: 21951
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040055C0 RID: 21952
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040055C1 RID: 21953
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040055C2 RID: 21954
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040055C3 RID: 21955
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040055C4 RID: 21956
		public static readonly float Epsilon = 1E-06f;

		// Token: 0x040055C5 RID: 21957
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040055C6 RID: 21958
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
