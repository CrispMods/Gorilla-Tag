using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CED RID: 3309
	public class MathUtil
	{
		// Token: 0x0600535F RID: 21343 RVA: 0x0018FD98 File Offset: 0x0018DF98
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x0018FDAF File Offset: 0x0018DFAF
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0019AE64 File Offset: 0x00199064
		public static float InvSafe(float x)
		{
			return 1f / Mathf.Max(MathUtil.Epsilon, x);
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0019AE78 File Offset: 0x00199078
		public static float PointLineDist(Vector2 point, Vector2 linePos, Vector2 lineDir)
		{
			Vector2 vector = point - linePos;
			return (vector - Vector2.Dot(vector, lineDir) * lineDir).magnitude;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0019AEA8 File Offset: 0x001990A8
		public static float PointSegmentDist(Vector2 point, Vector2 segmentPosA, Vector2 segmentPosB)
		{
			Vector2 a = segmentPosB - segmentPosA;
			float num = 1f / a.magnitude;
			Vector2 rhs = a * num;
			float value = Vector2.Dot(point - segmentPosA, rhs) * num;
			return (segmentPosA + Mathf.Clamp(value, 0f, 1f) * a - point).magnitude;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0019AF10 File Offset: 0x00199110
		public static float Seek(float current, float target, float maxDelta)
		{
			float num = target - current;
			num = Mathf.Sign(num) * Mathf.Min(maxDelta, Mathf.Abs(num));
			return current + num;
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x0019AF38 File Offset: 0x00199138
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

		// Token: 0x06005366 RID: 21350 RVA: 0x0019AF7A File Offset: 0x0019917A
		public static float Remainder(float a, float b)
		{
			return a - a / b * b;
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x0019AF7A File Offset: 0x0019917A
		public static int Remainder(int a, int b)
		{
			return a - a / b * b;
		}

		// Token: 0x06005368 RID: 21352 RVA: 0x0019AF83 File Offset: 0x00199183
		public static float Modulo(float a, float b)
		{
			return Mathf.Repeat(a, b);
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x0019AF8C File Offset: 0x0019918C
		public static int Modulo(int a, int b)
		{
			int num = a % b;
			if (num < 0)
			{
				return num + b;
			}
			return num;
		}

		// Token: 0x040055CD RID: 21965
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040055CE RID: 21966
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040055CF RID: 21967
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040055D0 RID: 21968
		public static readonly float QuaterPi = 0.7853982f;

		// Token: 0x040055D1 RID: 21969
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040055D2 RID: 21970
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040055D3 RID: 21971
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040055D4 RID: 21972
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040055D5 RID: 21973
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040055D6 RID: 21974
		public static readonly float Epsilon = 1E-06f;

		// Token: 0x040055D7 RID: 21975
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040055D8 RID: 21976
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
