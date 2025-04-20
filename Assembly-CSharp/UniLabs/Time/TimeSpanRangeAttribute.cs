using System;
using System.Diagnostics;

namespace UniLabs.Time
{
	// Token: 0x0200097C RID: 2428
	[AttributeUsage(AttributeTargets.All)]
	[Conditional("UNITY_EDITOR")]
	public class TimeSpanRangeAttribute : Attribute
	{
		// Token: 0x06003B45 RID: 15173 RVA: 0x00056B79 File Offset: 0x00054D79
		public TimeSpanRangeAttribute(string maxGetter, bool inline = false, TimeUnit snappingUnit = TimeUnit.Seconds)
		{
			this.MaxGetter = maxGetter;
			this.SnappingUnit = snappingUnit;
			this.Inline = inline;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x00056B96 File Offset: 0x00054D96
		public TimeSpanRangeAttribute(string minGetter, string maxGetter, bool inline = false, TimeUnit snappingUnit = TimeUnit.Seconds)
		{
			this.MinGetter = minGetter;
			this.MaxGetter = maxGetter;
			this.SnappingUnit = snappingUnit;
			this.Inline = inline;
		}

		// Token: 0x04003C75 RID: 15477
		public string MinGetter;

		// Token: 0x04003C76 RID: 15478
		public string MaxGetter;

		// Token: 0x04003C77 RID: 15479
		public TimeUnit SnappingUnit;

		// Token: 0x04003C78 RID: 15480
		public bool Inline;

		// Token: 0x04003C79 RID: 15481
		public string DisableMinMaxIf;
	}
}
