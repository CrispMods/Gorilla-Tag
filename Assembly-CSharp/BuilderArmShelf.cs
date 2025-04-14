using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class BuilderArmShelf : MonoBehaviour
{
	// Token: 0x06001D35 RID: 7477 RVA: 0x0008E560 File Offset: 0x0008C760
	private void Start()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x0008E56E File Offset: 0x0008C76E
	public bool IsOwnedLocally()
	{
		return this.ownerRig != null && this.ownerRig.isLocal;
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x0008E58B File Offset: 0x0008C78B
	public bool CanAttachToArmPiece()
	{
		return this.ownerRig != null && this.ownerRig.scaleFactor >= 1f;
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x0008E5B4 File Offset: 0x0008C7B4
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

	// Token: 0x0400204B RID: 8267
	[HideInInspector]
	public BuilderPiece piece;

	// Token: 0x0400204C RID: 8268
	public Transform pieceAnchor;

	// Token: 0x0400204D RID: 8269
	private VRRig ownerRig;
}
