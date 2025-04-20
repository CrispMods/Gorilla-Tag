using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB5 RID: 2997
	public static class GTColor
	{
		// Token: 0x06004BFA RID: 19450 RVA: 0x001A3290 File Offset: 0x001A1490
		public static Color RandomHSV(GTColor.HSVRanges ranges)
		{
			return Color.HSVToRGB(UnityEngine.Random.Range(ranges.h.x, ranges.h.y), UnityEngine.Random.Range(ranges.s.x, ranges.s.y), UnityEngine.Random.Range(ranges.v.x, ranges.v.y));
		}

		// Token: 0x02000BB6 RID: 2998
		[Serializable]
		public struct HSVRanges
		{
			// Token: 0x06004BFB RID: 19451 RVA: 0x00061F02 File Offset: 0x00060102
			public HSVRanges(float hMin = 0f, float hMax = 1f, float sMin = 0f, float sMax = 1f, float vMin = 0f, float vMax = 1f)
			{
				this.h = new Vector2(hMin, hMax);
				this.s = new Vector2(sMin, sMax);
				this.v = new Vector2(vMin, vMax);
			}

			// Token: 0x04004D1C RID: 19740
			public Vector2 h;

			// Token: 0x04004D1D RID: 19741
			public Vector2 s;

			// Token: 0x04004D1E RID: 19742
			public Vector2 v;
		}
	}
}
