using System;

// Token: 0x020004C8 RID: 1224
public interface IBuilderPieceComponent
{
	// Token: 0x06001DB5 RID: 7605
	void OnPieceCreate(int pieceType, int pieceId);

	// Token: 0x06001DB6 RID: 7606
	void OnPieceDestroy();

	// Token: 0x06001DB7 RID: 7607
	void OnPiecePlacementDeserialized();

	// Token: 0x06001DB8 RID: 7608
	void OnPieceActivate();

	// Token: 0x06001DB9 RID: 7609
	void OnPieceDeactivate();
}
