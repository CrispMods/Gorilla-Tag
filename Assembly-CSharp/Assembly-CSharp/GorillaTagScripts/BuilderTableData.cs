using System;
using System.Collections.Generic;

namespace GorillaTagScripts
{
	// Token: 0x020009AA RID: 2474
	[Serializable]
	public class BuilderTableData
	{
		// Token: 0x06003D40 RID: 15680 RVA: 0x00121878 File Offset: 0x0011FA78
		public BuilderTableData()
		{
			this.version = 4;
			this.numEdits = 0;
			this.numPieces = 0;
			this.pieceType = new List<int>(1024);
			this.pieceId = new List<int>(1024);
			this.parentId = new List<int>(1024);
			this.attachIndex = new List<int>(1024);
			this.parentAttachIndex = new List<int>(1024);
			this.placement = new List<int>(1024);
			this.materialType = new List<int>(1024);
			this.overlapingPieces = new List<int>(1024);
			this.overlappedPieces = new List<int>(1024);
			this.overlapInfo = new List<long>(1024);
			this.timeOffset = new List<int>(1024);
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x00121950 File Offset: 0x0011FB50
		public void Clear()
		{
			this.numPieces = 0;
			this.pieceType.Clear();
			this.pieceId.Clear();
			this.parentId.Clear();
			this.attachIndex.Clear();
			this.parentAttachIndex.Clear();
			this.placement.Clear();
			this.materialType.Clear();
			this.overlapingPieces.Clear();
			this.overlappedPieces.Clear();
			this.overlapInfo.Clear();
			this.timeOffset.Clear();
		}

		// Token: 0x04003EA1 RID: 16033
		public const int BUILDER_TABLE_DATA_VERSION = 4;

		// Token: 0x04003EA2 RID: 16034
		public int version;

		// Token: 0x04003EA3 RID: 16035
		public int numEdits;

		// Token: 0x04003EA4 RID: 16036
		public int numPieces;

		// Token: 0x04003EA5 RID: 16037
		public List<int> pieceType;

		// Token: 0x04003EA6 RID: 16038
		public List<int> pieceId;

		// Token: 0x04003EA7 RID: 16039
		public List<int> parentId;

		// Token: 0x04003EA8 RID: 16040
		public List<int> attachIndex;

		// Token: 0x04003EA9 RID: 16041
		public List<int> parentAttachIndex;

		// Token: 0x04003EAA RID: 16042
		public List<int> placement;

		// Token: 0x04003EAB RID: 16043
		public List<int> materialType;

		// Token: 0x04003EAC RID: 16044
		public List<int> overlapingPieces;

		// Token: 0x04003EAD RID: 16045
		public List<int> overlappedPieces;

		// Token: 0x04003EAE RID: 16046
		public List<long> overlapInfo;

		// Token: 0x04003EAF RID: 16047
		public List<int> timeOffset;
	}
}
