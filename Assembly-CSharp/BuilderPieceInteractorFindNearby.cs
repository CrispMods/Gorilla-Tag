using System;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class BuilderPieceInteractorFindNearby : MonoBehaviour
{
	// Token: 0x06001D6A RID: 7530 RVA: 0x00030607 File Offset: 0x0002E807
	private void Awake()
	{
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x000441FE File Offset: 0x000423FE
	private void LateUpdate()
	{
		if (this.pieceInteractor != null)
		{
			this.pieceInteractor.StartFindNearbyPieces();
		}
	}

	// Token: 0x0400206B RID: 8299
	public BuilderPieceInteractor pieceInteractor;
}
