using System;
using UnityEngine;

// Token: 0x02000865 RID: 2149
public class FlagForLighting : MonoBehaviour
{
	// Token: 0x0400379D RID: 14237
	public FlagForLighting.TimeOfDay myTimeOfDay;

	// Token: 0x02000866 RID: 2150
	public enum TimeOfDay
	{
		// Token: 0x0400379F RID: 14239
		Sunrise,
		// Token: 0x040037A0 RID: 14240
		TenAM,
		// Token: 0x040037A1 RID: 14241
		Noon,
		// Token: 0x040037A2 RID: 14242
		ThreePM,
		// Token: 0x040037A3 RID: 14243
		Sunset,
		// Token: 0x040037A4 RID: 14244
		Night,
		// Token: 0x040037A5 RID: 14245
		RainingDay,
		// Token: 0x040037A6 RID: 14246
		RainingNight,
		// Token: 0x040037A7 RID: 14247
		None
	}
}
