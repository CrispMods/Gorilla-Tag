using System;

namespace GorillaTagScripts
{
	// Token: 0x0200099F RID: 2463
	public struct BuilderPrivatePlotData
	{
		// Token: 0x06003CE4 RID: 15588 RVA: 0x0011E523 File Offset: 0x0011C723
		public BuilderPrivatePlotData(BuilderPiecePrivatePlot plot)
		{
			this.plotState = plot.plotState;
			this.ownerActorNumber = plot.GetOwnerActorNumber();
			this.isUnderCapacityLeft = false;
			this.isUnderCapacityRight = false;
		}

		// Token: 0x04003E33 RID: 15923
		public BuilderPiecePrivatePlot.PlotState plotState;

		// Token: 0x04003E34 RID: 15924
		public int ownerActorNumber;

		// Token: 0x04003E35 RID: 15925
		public bool isUnderCapacityLeft;

		// Token: 0x04003E36 RID: 15926
		public bool isUnderCapacityRight;
	}
}
