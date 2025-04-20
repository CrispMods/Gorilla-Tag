using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class BuilderArmShelf : MonoBehaviour
{
	// Token: 0x06001D8E RID: 7566 RVA: 0x0004435A File Offset: 0x0004255A
	private void Start()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x00044368 File Offset: 0x00042568
	public bool IsOwnedLocally()
	{
		return this.ownerRig != null && this.ownerRig.isLocal;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x00044385 File Offset: 0x00042585
	public bool CanAttachToArmPiece()
	{
		return this.ownerRig != null && this.ownerRig.scaleFactor >= 1f;
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x000E1784 File Offset: 0x000DF984
	public void DropAttachedPieces()
	{
		if (this.ownerRig != null && this.piece != null)
		{
			Vector3 velocity = Vector3.zero;
			if (this.piece.firstChildPiece == null)
			{
				return;
			}
			Vector3 point = BuilderTable.instance.roomCenter.position - this.piece.transform.position;
			point.Normalize();
			Vector3 a = Quaternion.Euler(0f, 180f, 0f) * point;
			velocity = BuilderTable.DROP_ZONE_REPEL * a;
			BuilderPiece builderPiece = this.piece.firstChildPiece;
			while (builderPiece != null)
			{
				BuilderTable.instance.RequestDropPiece(builderPiece, builderPiece.transform.position + a * 0.1f, builderPiece.transform.rotation, velocity, Vector3.zero);
				builderPiece = builderPiece.nextSiblingPiece;
			}
		}
	}

	// Token: 0x0400209E RID: 8350
	[HideInInspector]
	public BuilderPiece piece;

	// Token: 0x0400209F RID: 8351
	public Transform pieceAnchor;

	// Token: 0x040020A0 RID: 8352
	private VRRig ownerRig;
}
