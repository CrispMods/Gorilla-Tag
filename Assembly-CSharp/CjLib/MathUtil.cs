using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CCD RID: 3277
	public class MathUtil
	{
		// Token: 0x060052D2 RID: 21202 RVA: 0x00065A84 File Offset: 0x00063C84
		public static float AsinSafe(float x)
		{
			return Mathf.Asin(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x00065A9B File Offset: 0x00063C9B
		public static float AcosSafe(float x)
		{
			return Mathf.Acos(Mathf.Clamp(x, -1f, 1f));
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001C6284 File Offset: 0x001C4484
		public static float CatmullRom(float p0, float p1, float p2, float p3, float t)
		{
			float num = t * t;
			return 0.5f * (2f * p1 + (-p0 + p2) * t + (2f * p0 - 5f * p1 + 4f * p2 - p3) * num + (-p0 + 3f * p1 - 3f * p2 + p3) * num * t);
		}

		// Token: 0x040054BB RID: 21691
		public static readonly float Pi = 3.1415927f;

		// Token: 0x040054BC RID: 21692
		public static readonly float TwoPi = 6.2831855f;

		// Token: 0x040054BD RID: 21693
		public static readonly float HalfPi = 1.5707964f;

		// Token: 0x040054BE RID: 21694
		public static readonly float ThirdPi = 1.0471976f;

		// Token: 0x040054BF RID: 21695
		public static readonly float QuarterPi = 0.7853982f;

		// Token: 0x040054C0 RID: 21696
		public static readonly float FifthPi = 0.62831855f;

		// Token: 0x040054C1 RID: 21697
		public static readonly float SixthPi = 0.5235988f;

		// Token: 0x040054C2 RID: 21698
		public static readonly float Sqrt2 = Mathf.Sqrt(2f);

		// Token: 0x040054C3 RID: 21699
		public static readonly float Sqrt2Inv = 1f / Mathf.Sqrt(2f);

		// Token: 0x040054C4 RID: 21700
		public static readonly float Sqrt3 = Mathf.Sqrt(3f);

		// Token: 0x040054C5 RID: 21701
		public static readonly float Sqrt3Inv = 1f / Mathf.Sqrt(3f);

		// Token: 0x040054C6 RID: 21702
		public static readonly float Epsilon = 1E-09f;

		// Token: 0x040054C7 RID: 21703
		public static readonly float EpsilonComp = 1f - MathUtil.Epsilon;

		// Token: 0x040054C8 RID: 21704
		public static readonly float Rad2Deg = 57.295776f;

		// Token: 0x040054C9 RID: 21705
		public static readonly float Deg2Rad = 0.017453292f;
	}
}
