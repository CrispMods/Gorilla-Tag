using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B8B RID: 2955
	public static class GTColor
	{
		// Token: 0x06004ABB RID: 19131 RVA: 0x0019C278 File Offset: 0x0019A478
		public static Color RandomHSV(GTColor.HSVRanges ranges)
		{
			return Color.HSVToRGB(UnityEngine.Random.Range(ranges.h.x, ranges.h.y), UnityEngine.Random.Range(ranges.s.x, ranges.s.y), UnityEngine.Random.Range(ranges.v.x, ranges.v.y));
		}

		// Token: 0x02000B8C RID: 2956
		[Serializable]
		public struct HSVRanges
		{
			// Token: 0x06004ABC RID: 19132 RVA: 0x000604CA File Offset: 0x0005E6CA
			public HSVRanges(float hMin = 0f, float hMax = 1f, float sMin = 0f, float sMax = 1f, float vMin = 0f, float vMax = 1f)
			{
				this.h = new Vector2(hMin, hMax);
				this.s = new Vector2(sMin, sMax);
				this.v = new Vector2(vMin, vMax);
			}

			// Token: 0x04004C38 RID: 19512
			public Vector2 h;

			// Token: 0x04004C39 RID: 19513
			public Vector2 s;

			// Token: 0x04004C3A RID: 19514
			public Vector2 v;
		}
	}
}
