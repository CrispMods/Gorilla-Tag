using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B88 RID: 2952
	public static class GTColor
	{
		// Token: 0x06004AAF RID: 19119 RVA: 0x00169984 File Offset: 0x00167B84
		public static Color RandomHSV(GTColor.HSVRanges ranges)
		{
			return Color.HSVToRGB(Random.Range(ranges.h.x, ranges.h.y), Random.Range(ranges.s.x, ranges.s.y), Random.Range(ranges.v.x, ranges.v.y));
		}

		// Token: 0x02000B89 RID: 2953
		[Serializable]
		public struct HSVRanges
		{
			// Token: 0x06004AB0 RID: 19120 RVA: 0x001699E7 File Offset: 0x00167BE7
			public HSVRanges(float hMin = 0f, float hMax = 1f, float sMin = 0f, float sMax = 1f, float vMin = 0f, float vMax = 1f)
			{
				this.h = new Vector2(hMin, hMax);
				this.s = new Vector2(sMin, sMax);
				this.v = new Vector2(vMin, vMax);
			}

			// Token: 0x04004C26 RID: 19494
			public Vector2 h;

			// Token: 0x04004C27 RID: 19495
			public Vector2 s;

			// Token: 0x04004C28 RID: 19496
			public Vector2 v;
		}
	}
}
