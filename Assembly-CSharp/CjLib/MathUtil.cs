using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C9C RID: 3228
	public class MathUtil
	{
		// Token: 0x06005170 RID: 20848 RVA: 0x0018F7D0 File Offset: 0x0018D9D0
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x0018F7E7 File Offset: 0x0018D9E7
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x0018F800 File Offset: 0x0018DA00
		public static float CatmullRom(float p0, float p1, float p2, float p3, float t)
		{
			float num = t * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num * t);
		}

		// Token: 0x040053AF RID: 21423
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040053B0 RID: 21424
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040053B1 RID: 21425
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040053B2 RID: 21426
		public static readonly float ThirdPi = 1.0471976f;

		// Token: 0x040053B3 RID: 21427
		public static readonly float QuarterPi = 0.7853982f;

		// Token: 0x040053B4 RID: 21428
		public static readonly float FifthPi = 0.62831855f;

		// Token: 0x040053B5 RID: 21429
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040053B6 RID: 21430
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040053B7 RID: 21431
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040053B8 RID: 21432
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040053B9 RID: 21433
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040053BA RID: 21434
		public static readonly float Epsilon = 1E-09f;

		// Token: 0x040053BB RID: 21435
		public static readonly float EpsilonComp = 1f - MathUtil.Epsilon;

		// Token: 0x040053BC RID: 21436
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040053BD RID: 21437
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
