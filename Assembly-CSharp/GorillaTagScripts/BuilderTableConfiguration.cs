using System;

namespace GorillaTagScripts
{
	// Token: 0x02000990 RID: 2448
	[Serializable]
	public class BuilderTableConfiguration
	{
		// Token: 0x06003BFC RID: 15356 RVA: 0x00114BB0 File Offset: 0x00112DB0
		public BuilderTableConfiguration()
		{
			this.version = 0;
			this.TableResourceLimits = new int[3];
			this.PlotResourceLimits = new int[3];
			this.updateCountdownDate = string.Empty;
		}

		// Token: 0x04003D2A RID: 15658
		public const int CONFIGURATION_VERSION = 0;

		// Token: 0x04003D2B RID: 15659
		public int version;

		// Token: 0x04003D2C RID: 15660
		public int[] TableResourceLimits;

		// Token: 0x04003D2D RID: 15661
		public int[] PlotResourceLimits;

		// Token: 0x04003D2E RID: 15662
		public int DroppedPieceLimit;

		// Token: 0x04003D2F RID: 15663
		public string updateCountdownDate;
	}
}
