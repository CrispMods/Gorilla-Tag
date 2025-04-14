using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A0 RID: 2464
	public struct BuilderPotentialPlacementData
	{
		// Token: 0x06003CE5 RID: 15589 RVA: 0x0011E54C File Offset: 0x0011C74C
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

		// Token: 0x04003E37 RID: 15927
		public int pieceId;

		// Token: 0x04003E38 RID: 15928
		public int parentPieceId;

		// Token: 0x04003E39 RID: 15929
		public float score;

		// Token: 0x04003E3A RID: 15930
		public Vector3 localPosition;

		// Token: 0x04003E3B RID: 15931
		public Quaternion localRotation;

		// Token: 0x04003E3C RID: 15932
		public int attachIndex;

		// Token: 0x04003E3D RID: 15933
		public int parentAttachIndex;

		// Token: 0x04003E3E RID: 15934
		public float attachDistance;

		// Token: 0x04003E3F RID: 15935
		public Vector3 attachPlaneNormal;

		// Token: 0x04003E40 RID: 15936
		public SnapBounds attachBounds;

		// Token: 0x04003E41 RID: 15937
		public SnapBounds parentAttachBounds;

		// Token: 0x04003E42 RID: 15938
		public byte twist;

		// Token: 0x04003E43 RID: 15939
		public sbyte bumpOffsetX;

		// Token: 0x04003E44 RID: 15940
		public sbyte bumpOffsetZ;
	}
}
