using System;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class FlagForLighting : MonoBehaviour
{
	// Token: 0x040036E1 RID: 14049
	public FlagForLighting.TimeOfDay myTimeOfDay;

	// Token: 0x0200084C RID: 2124
	public enum TimeOfDay
	{
		// Token: 0x040036E3 RID: 14051
		Sunrise,
		// Token: 0x040036E4 RID: 14052
		TenAM,
		// Token: 0x040036E5 RID: 14053
		Noon,
		// Token: 0x040036E6 RID: 14054
		ThreePM,
		// Token: 0x040036E7 RID: 14055
		Sunset,
		// Token: 0x040036E8 RID: 14056
		Night,
		// Token: 0x040036E9 RID: 14057
		RainingDay,
		// Token: 0x040036EA RID: 14058
		RainingNight,
		// Token: 0x040036EB RID: 14059
		None
	}
}
