using System;

namespace GorillaTagScripts
{
	// Token: 0x0200099D RID: 2461
	public struct BuilderPieceData
	{
		// Token: 0x06003CE3 RID: 15587 RVA: 0x0011E478 File Offset: 0x0011C678
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

		// Token: 0x04003E27 RID: 15911
		public int pieceId;

		// Token: 0x04003E28 RID: 15912
		public int pieceIndex;

		// Token: 0x04003E29 RID: 15913
		public int parentPieceIndex;

		// Token: 0x04003E2A RID: 15914
		public int requestedParentPieceIndex;

		// Token: 0x04003E2B RID: 15915
		public int heldByActorNumber;

		// Token: 0x04003E2C RID: 15916
		public int preventSnapUntilMoved;

		// Token: 0x04003E2D RID: 15917
		public bool isBuiltIntoTable;

		// Token: 0x04003E2E RID: 15918
		public BuilderPiece.State state;

		// Token: 0x04003E2F RID: 15919
		public int privatePlotIndex;

		// Token: 0x04003E30 RID: 15920
		public bool isArmPiece;
	}
}
