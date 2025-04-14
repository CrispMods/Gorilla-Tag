using System;
using UnityEngine;

// Token: 0x0200084E RID: 2126
public class FlagForLighting : MonoBehaviour
{
	// Token: 0x040036F3 RID: 14067
	public FlagForLighting.TimeOfDay myTimeOfDay;

	// Token: 0x0200084F RID: 2127
	public enum TimeOfDay
	{
		// Token: 0x040036F5 RID: 14069
		Sunrise,
		// Token: 0x040036F6 RID: 14070
		TenAM,
		// Token: 0x040036F7 RID: 14071
		Noon,
		// Token: 0x040036F8 RID: 14072
		ThreePM,
		// Token: 0x040036F9 RID: 14073
		Sunset,
		// Token: 0x040036FA RID: 14074
		Night,
		// Token: 0x040036FB RID: 14075
		RainingDay,
		// Token: 0x040036FC RID: 14076
		RainingNight,
		// Token: 0x040036FD RID: 14077
		None
	}
}
