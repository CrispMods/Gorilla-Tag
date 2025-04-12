using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9F RID: 3231
	public class MathUtil
	{
		// Token: 0x0600517C RID: 20860 RVA: 0x0006400E File Offset: 0x0006220E
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x00064025 File Offset: 0x00062225
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001BE1A0 File Offset: 0x001BC3A0
		public static float CatmullRom(float p0, float p1, float p2, float p3, float t)
		{
			float num = t * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num * t);
		}

		// Token: 0x040053C1 RID: 21441
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040053C2 RID: 21442
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040053C3 RID: 21443
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040053C4 RID: 21444
		public static readonly float ThirdPi = 1.0471976f;

		// Token: 0x040053C5 RID: 21445
		public static readonly float QuarterPi = 0.7853982f;

		// Token: 0x040053C6 RID: 21446
		public static readonly float FifthPi = 0.62831855f;

		// Token: 0x040053C7 RID: 21447
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040053C8 RID: 21448
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040053C9 RID: 21449
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040053CA RID: 21450
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040053CB RID: 21451
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040053CC RID: 21452
		public static readonly float Epsilon = 1E-09f;

		// Token: 0x040053CD RID: 21453
		public static readonly float EpsilonComp = 1f - MathUtil.Epsilon;

		// Token: 0x040053CE RID: 21454
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040053CF RID: 21455
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
