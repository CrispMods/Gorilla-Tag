using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class GiantSnowflakeAudio : MonoBehaviour
{
	// Token: 0x0600207B RID: 8315 RVA: 0x000F3098 File Offset: 0x000F1298
	private void Start()
	{
		foreach (GiantSnowflakeAudio.SnowflakeScaleOverride snowflakeScaleOverride in this.audioOverrides)
		{
			if (base.transform.lossyScale.x < snowflakeScaleOverride.scaleMax)
			{
				base.GetComponent<GorillaSurfaceOverride>().overrideIndex = snowflakeScaleOverride.newOverrideIndex;
			}
		}
	}

	// Token: 0x040024A0 RID: 9376
	public List<GiantSnowflakeAudio.SnowflakeScaleOverride> audioOverrides;

	// Token: 0x0200053F RID: 1343
	[Serializable]
	public struct SnowflakeScaleOverride
	{
		// Token: 0x040024A1 RID: 9377
		public float scaleMax;

		// Token: 0x040024A2 RID: 9378
		[GorillaSoundLookup]
		public int newOverrideIndex;
	}
}
