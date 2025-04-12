using System;

namespace GorillaTagScripts
{
	// Token: 0x020009A0 RID: 2464
	public struct BuilderPieceData
	{
		// Token: 0x06003CEF RID: 15599 RVA: 0x0015A8F0 File Offset: 0x00158AF0
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

		// Token: 0x04003E39 RID: 15929
		public int pieceId;

		// Token: 0x04003E3A RID: 15930
		public int pieceIndex;

		// Token: 0x04003E3B RID: 15931
		public int parentPieceIndex;

		// Token: 0x04003E3C RID: 15932
		public int requestedParentPieceIndex;

		// Token: 0x04003E3D RID: 15933
		public int heldByActorNumber;

		// Token: 0x04003E3E RID: 15934
		public int preventSnapUntilMoved;

		// Token: 0x04003E3F RID: 15935
		public bool isBuiltIntoTable;

		// Token: 0x04003E40 RID: 15936
		public BuilderPiece.State state;

		// Token: 0x04003E41 RID: 15937
		public int privatePlotIndex;

		// Token: 0x04003E42 RID: 15938
		public bool isArmPiece;
	}
}
