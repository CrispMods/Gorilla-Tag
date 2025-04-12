using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class BuilderPieceInteractorFindNearby : MonoBehaviour
{
	// Token: 0x06001D19 RID: 7449 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Awake()
	{
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x00042EC5 File Offset: 0x000410C5
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
