using System;
using GorillaTagScripts;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class BuilderArmShelf : MonoBehaviour
{
	// Token: 0x06001D38 RID: 7480 RVA: 0x0008E8E4 File Offset: 0x0008CAE4
	private void Start()
	{
		this.ownerRig = base.GetComponentInParent<VRRig>();
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x0008E8F2 File Offset: 0x0008CAF2
	public bool IsOwnedLocally()
	{
		return this.ownerRig != null && this.ownerRig.isLocal;
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x0008E90F File Offset: 0x0008CB0F
	public bool CanAttachToArmPiece()
	{
		return this.ownerRig != null && this.ownerRig.scaleFactor >= 1f;
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x0008E938 File Offset: 0x0008CB38
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

	// Token: 0x0400204C RID: 8268
	[HideInInspector]
	public BuilderPiece piece;

	// Token: 0x0400204D RID: 8269
	public Transform pieceAnchor;

	// Token: 0x0400204E RID: 8270
	private VRRig ownerRig;
}
