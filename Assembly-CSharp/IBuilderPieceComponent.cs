using System;

// Token: 0x020004C8 RID: 1224
public interface IBuilderPieceComponent
{
	// Token: 0x06001DB2 RID: 7602
	void OnPieceCreate(int pieceType, int pieceId);

	// Token: 0x06001DB3 RID: 7603
	void OnPieceDestroy();

	// Token: 0x06001DB4 RID: 7604
	void OnPiecePlacementDeserialized();

	// Token: 0x06001DB5 RID: 7605
	void OnPieceActivate();

	// Token: 0x06001DB6 RID: 7606
	void OnPieceDeactivate();
}
