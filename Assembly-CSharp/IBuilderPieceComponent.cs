using System;

// Token: 0x020004D5 RID: 1237
public interface IBuilderPieceComponent
{
	// Token: 0x06001E0B RID: 7691
	void OnPieceCreate(int pieceType, int pieceId);

	// Token: 0x06001E0C RID: 7692
	void OnPieceDestroy();

	// Token: 0x06001E0D RID: 7693
	void OnPiecePlacementDeserialized();

	// Token: 0x06001E0E RID: 7694
	void OnPieceActivate();

	// Token: 0x06001E0F RID: 7695
	void OnPieceDeactivate();
}
