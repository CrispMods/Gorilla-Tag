using System;

namespace GorillaTagScripts
{
	// Token: 0x020009C3 RID: 2499
	public struct BuilderPieceData
	{
		// Token: 0x06003DFB RID: 15867 RVA: 0x001608D8 File Offset: 0x0015EAD8
		public BuilderPieceData(BuilderPiece piece)
		{
			this.pieceId = piece.pieceId;
			this.pieceIndex = piece.pieceDataIndex;
			BuilderPiece parentPiece = piece.parentPiece;
			this.parentPieceIndex = ((parentPiece == null) ? -1 : parentPiece.pieceDataIndex);
			BuilderPiece requestedParentPiece = piece.requestedParentPiece;
			this.requestedParentPieceIndex = ((requestedParentPiece == null) ? -1 : requestedParentPiece.pieceDataIndex);
			this.preventSnapUntilMoved = piece.preventSnapUntilMoved;
			this.isBuiltIntoTable = piece.isBuiltIntoTable;
			this.state = piece.state;
			this.privatePlotIndex = piece.privatePlotIndex;
			this.isArmPiece = piece.isArmShelf;
			this.heldByActorNumber = piece.heldByPlayerActorNumber;
		}

		// Token: 0x04003F01 RID: 16129
		public int pieceId;

		// Token: 0x04003F02 RID: 16130
		public int pieceIndex;

		// Token: 0x04003F03 RID: 16131
		public int parentPieceIndex;

		// Token: 0x04003F04 RID: 16132
		public int requestedParentPieceIndex;

		// Token: 0x04003F05 RID: 16133
		public int heldByActorNumber;

		// Token: 0x04003F06 RID: 16134
		public int preventSnapUntilMoved;

		// Token: 0x04003F07 RID: 16135
		public bool isBuiltIntoTable;

		// Token: 0x04003F08 RID: 16136
		public BuilderPiece.State state;

		// Token: 0x04003F09 RID: 16137
		public int privatePlotIndex;

		// Token: 0x04003F0A RID: 16138
		public bool isArmPiece;
	}
}
