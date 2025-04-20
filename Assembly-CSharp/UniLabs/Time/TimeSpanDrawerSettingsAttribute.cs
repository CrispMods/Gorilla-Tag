using System;
using System.Diagnostics;

namespace UniLabs.Time
{
	// Token: 0x0200097B RID: 2427
	[Conditional("UNITY_EDITOR")]
	public class TimeSpanDrawerSettingsAttribute : Attribute
	{
		// Token: 0x06003B41 RID: 15169 RVA: 0x00056AEB File Offset: 0x00054CEB
		public TimeSpanDrawerSettingsAttribute()
		{
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x00056B01 File Offset: 0x00054D01
		public TimeSpanDrawerSettingsAttribute(TimeUnit highestUnit, TimeUnit lowestUnit)
		{
			this.HighestUnit = highestUnit;
			this.LowestUnit = lowestUnit;
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x00056B25 File Offset: 0x00054D25
		public TimeSpanDrawerSettingsAttribute(TimeUnit highestUnit, bool drawMilliseconds = false)
		{
			this.HighestUnit = highestUnit;
			this.LowestUnit = (drawMilliseconds ? TimeUnit.Milliseconds : TimeUnit.Seconds);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x00056B4F File Offset: 0x00054D4F
		public TimeSpanDrawerSettingsAttribute(bool drawMilliseconds)
		{
			this.HighestUnit = TimeUnit.Days;
			this.LowestUnit = (drawMilliseconds ? TimeUnit.Milliseconds : TimeUnit.Seconds);
		}

		// Token: 0x04003C73 RID: 15475
		public TimeUnit HighestUnit = TimeUnit.Days;

		// Token: 0x04003C74 RID: 15476
		public TimeUnit LowestUnit = TimeUnit.Seconds;
	}
}
