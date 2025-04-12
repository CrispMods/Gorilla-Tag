using System;

namespace GorillaTagScripts
{
	// Token: 0x020009A2 RID: 2466
	public struct BuilderPrivatePlotData
	{
		// Token: 0x06003CF0 RID: 15600 RVA: 0x00056D94 File Offset: 0x00054F94
		public BuilderPrivatePlotData(BuilderPiecePrivatePlot plot)
		{
			this.plotState = plot.plotState;
			this.ownerActorNumber = plot.GetOwnerActorNumber();
			this.isUnderCapacityLeft = false;
			this.isUnderCapacityRight = false;
		}

		// Token: 0x04003E45 RID: 15941
		public BuilderPiecePrivatePlot.PlotState plotState;

		// Token: 0x04003E46 RID: 15942
		public int ownerActorNumber;

		// Token: 0x04003E47 RID: 15943
		public bool isUnderCapacityLeft;

		// Token: 0x04003E48 RID: 15944
		public bool isUnderCapacityRight;
	}
}
