using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class GiantSnowflakeAudio : MonoBehaviour
{
	// Token: 0x06002022 RID: 8226 RVA: 0x000A1CFC File Offset: 0x0009FEFC
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

	// Token: 0x0400244D RID: 9293
	public List<GiantSnowflakeAudio.SnowflakeScaleOverride> audioOverrides;

	// Token: 0x02000532 RID: 1330
	[Serializable]
	public struct SnowflakeScaleOverride
	{
		// Token: 0x0400244E RID: 9294
		public float scaleMax;

		// Token: 0x0400244F RID: 9295
		[GorillaSoundLookup]
		public int newOverrideIndex;
	}
}
