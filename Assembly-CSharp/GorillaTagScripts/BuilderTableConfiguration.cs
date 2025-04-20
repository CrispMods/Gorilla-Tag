using System;

namespace GorillaTagScripts
{
	// Token: 0x020009B6 RID: 2486
	[Serializable]
	public class BuilderTableConfiguration
	{
		// Token: 0x06003D14 RID: 15636 RVA: 0x00057D6A File Offset: 0x00055F6A
		public BuilderTableConfiguration()
		{
			this.version = 0;
			this.TableResourceLimits = new int[3];
			this.PlotResourceLimits = new int[3];
			this.updateCountdownDate = string.Empty;
		}

		// Token: 0x04003E04 RID: 15876
		public const int CONFIGURATION_VERSION = 0;

		// Token: 0x04003E05 RID: 15877
		public int version;

		// Token: 0x04003E06 RID: 15878
		public int[] TableResourceLimits;

		// Token: 0x04003E07 RID: 15879
		public int[] PlotResourceLimits;

		// Token: 0x04003E08 RID: 15880
		public int DroppedPieceLimit;

		// Token: 0x04003E09 RID: 15881
		public string updateCountdownDate;
	}
}
