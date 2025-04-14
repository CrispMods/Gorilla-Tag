using System;

namespace GorillaTagScripts
{
	// Token: 0x02000993 RID: 2451
	[Serializable]
	public class BuilderTableConfiguration
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x00115178 File Offset: 0x00113378
		public BuilderTableConfiguration()
		{
			this.version = 0;
			this.TableResourceLimits = new int[3];
			this.PlotResourceLimits = new int[3];
			this.updateCountdownDate = string.Empty;
		}

		// Token: 0x04003D3C RID: 15676
		public const int CONFIGURATION_VERSION = 0;

		// Token: 0x04003D3D RID: 15677
		public int version;

		// Token: 0x04003D3E RID: 15678
		public int[] TableResourceLimits;

		// Token: 0x04003D3F RID: 15679
		public int[] PlotResourceLimits;

		// Token: 0x04003D40 RID: 15680
		public int DroppedPieceLimit;

		// Token: 0x04003D41 RID: 15681
		public string updateCountdownDate;
	}
}
