using System;
using System.Collections.Generic;

namespace GorillaTagScripts
{
	// Token: 0x020009A7 RID: 2471
	[Serializable]
	public class BuilderTableData
	{
		// Token: 0x06003D34 RID: 15668 RVA: 0x001212B0 File Offset: 0x0011F4B0
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

		// Token: 0x06003D35 RID: 15669 RVA: 0x00121388 File Offset: 0x0011F588
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

		// Token: 0x04003E8F RID: 16015
		public const int BUILDER_TABLE_DATA_VERSION = 4;

		// Token: 0x04003E90 RID: 16016
		public int version;

		// Token: 0x04003E91 RID: 16017
		public int numEdits;

		// Token: 0x04003E92 RID: 16018
		public int numPieces;

		// Token: 0x04003E93 RID: 16019
		public List<int> pieceType;

		// Token: 0x04003E94 RID: 16020
		public List<int> pieceId;

		// Token: 0x04003E95 RID: 16021
		public List<int> parentId;

		// Token: 0x04003E96 RID: 16022
		public List<int> attachIndex;

		// Token: 0x04003E97 RID: 16023
		public List<int> parentAttachIndex;

		// Token: 0x04003E98 RID: 16024
		public List<int> placement;

		// Token: 0x04003E99 RID: 16025
		public List<int> materialType;

		// Token: 0x04003E9A RID: 16026
		public List<int> overlapingPieces;

		// Token: 0x04003E9B RID: 16027
		public List<int> overlappedPieces;

		// Token: 0x04003E9C RID: 16028
		public List<long> overlapInfo;

		// Token: 0x04003E9D RID: 16029
		public List<int> timeOffset;
	}
}
