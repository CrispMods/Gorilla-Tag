using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class BuilderPieceInteractorFindNearby : MonoBehaviour
{
	// Token: 0x06001D19 RID: 7449 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0008E12A File Offset: 0x0008C32A
	private void LateUpdate()
	{
		if (this.pieceInteractor != null)
		{
			this.pieceInteractor.StartFindNearbyPieces();
		}
	}

	// Token: 0x0400201D RID: 8221
	public BuilderPieceInteractor pieceInteractor;
}
