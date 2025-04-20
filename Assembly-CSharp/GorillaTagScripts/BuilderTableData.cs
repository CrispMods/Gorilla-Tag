using System;
using System.Collections.Generic;

namespace GorillaTagScripts
{
	// Token: 0x020009CD RID: 2509
	[Serializable]
	public class BuilderTableData
	{
		// Token: 0x06003E4C RID: 15948 RVA: 0x00163528 File Offset: 0x00161728
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

		// Token: 0x06003E4D RID: 15949 RVA: 0x00163600 File Offset: 0x00161800
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

		// Token: 0x04003F69 RID: 16233
		public const int BUILDER_TABLE_DATA_VERSION = 4;

		// Token: 0x04003F6A RID: 16234
		public int version;

		// Token: 0x04003F6B RID: 16235
		public int numEdits;

		// Token: 0x04003F6C RID: 16236
		public int numPieces;

		// Token: 0x04003F6D RID: 16237
		public List<int> pieceType;

		// Token: 0x04003F6E RID: 16238
		public List<int> pieceId;

		// Token: 0x04003F6F RID: 16239
		public List<int> parentId;

		// Token: 0x04003F70 RID: 16240
		public List<int> attachIndex;

		// Token: 0x04003F71 RID: 16241
		public List<int> parentAttachIndex;

		// Token: 0x04003F72 RID: 16242
		public List<int> placement;

		// Token: 0x04003F73 RID: 16243
		public List<int> materialType;

		// Token: 0x04003F74 RID: 16244
		public List<int> overlapingPieces;

		// Token: 0x04003F75 RID: 16245
		public List<int> overlappedPieces;

		// Token: 0x04003F76 RID: 16246
		public List<long> overlapInfo;

		// Token: 0x04003F77 RID: 16247
		public List<int> timeOffset;
	}
}
