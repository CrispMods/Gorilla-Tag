using System;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class BuilderPieceInteractorFindNearby : MonoBehaviour
{
	// Token: 0x06001D16 RID: 7446 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0008DDA6 File Offset: 0x0008BFA6
	private void LateUpdate()
	{
		if (this.pieceInteractor != null)
		{
			this.pieceInteractor.StartFindNearbyPieces();
		}
	}

	// Token: 0x0400201C RID: 8220
	public BuilderPieceInteractor pieceInteractor;
}
