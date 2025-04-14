using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A3 RID: 2467
	public struct BuilderPotentialPlacementData
	{
		// Token: 0x06003CF1 RID: 15601 RVA: 0x0011EB14 File Offset: 0x0011CD14
		public BuilderPotentialPlacement ToPotentialPlacement(BuilderTable table)
		{
			BuilderPotentialPlacement builderPotentialPlacement = new BuilderPotentialPlacement
			{
				attachPiece = table.GetPiece(this.pieceId),
				parentPiece = table.GetPiece(this.parentPieceId),
				score = this.score,
				localPosition = this.localPosition,
				localRotation = this.localRotation,
				attachIndex = this.attachIndex,
				parentAttachIndex = this.parentAttachIndex,
				attachDistance = this.attachDistance,
				attachPlaneNormal = this.attachPlaneNormal,
				attachBounds = this.attachBounds,
				parentAttachBounds = this.parentAttachBounds,
				twist = this.twist,
				bumpOffsetX = this.bumpOffsetX,
				bumpOffsetZ = this.bumpOffsetZ
			};
			if (builderPotentialPlacement.parentPiece != null)
			{
				BuilderAttachGridPlane builderAttachGridPlane = builderPotentialPlacement.parentPiece.gridPlanes[builderPotentialPlacement.parentAttachIndex];
				builderPotentialPlacement.localPosition = builderAttachGridPlane.transform.InverseTransformPoint(builderPotentialPlacement.localPosition);
				builderPotentialPlacement.localRotation = Quaternion.Inverse(builderAttachGridPlane.transform.rotation) * builderPotentialPlacement.localRotation;
			}
			return builderPotentialPlacement;
		}

		// Token: 0x04003E49 RID: 15945
		public int pieceId;

		// Token: 0x04003E4A RID: 15946
		public int parentPieceId;

		// Token: 0x04003E4B RID: 15947
		public float score;

		// Token: 0x04003E4C RID: 15948
		public Vector3 localPosition;

		// Token: 0x04003E4D RID: 15949
		public Quaternion localRotation;

		// Token: 0x04003E4E RID: 15950
		public int attachIndex;

		// Token: 0x04003E4F RID: 15951
		public int parentAttachIndex;

		// Token: 0x04003E50 RID: 15952
		public float attachDistance;

		// Token: 0x04003E51 RID: 15953
		public Vector3 attachPlaneNormal;

		// Token: 0x04003E52 RID: 15954
		public SnapBounds attachBounds;

		// Token: 0x04003E53 RID: 15955
		public SnapBounds parentAttachBounds;

		// Token: 0x04003E54 RID: 15956
		public byte twist;

		// Token: 0x04003E55 RID: 15957
		public sbyte bumpOffsetX;

		// Token: 0x04003E56 RID: 15958
		public sbyte bumpOffsetZ;
	}
}
