using System;

namespace GorillaTagScripts
{
	// Token: 0x020009C5 RID: 2501
	public struct BuilderPrivatePlotData
	{
		// Token: 0x06003DFC RID: 15868 RVA: 0x0005862B File Offset: 0x0005682B
		public BuilderPrivatePlotData(BuilderPiecePrivatePlot plot)
		{
			this.plotState = plot.plotState;
			this.ownerActorNumber = plot.GetOwnerActorNumber();
			this.isUnderCapacityLeft = false;
			this.isUnderCapacityRight = false;
		}

		// Token: 0x04003F0D RID: 16141
		public BuilderPiecePrivatePlot.PlotState plotState;

		// Token: 0x04003F0E RID: 16142
		public int ownerActorNumber;

		// Token: 0x04003F0F RID: 16143
		public bool isUnderCapacityLeft;

		// Token: 0x04003F10 RID: 16144
		public bool isUnderCapacityRight;
	}
}
