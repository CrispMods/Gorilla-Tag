using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C6 RID: 2502
	public struct BuilderPotentialPlacementData
	{
		// Token: 0x06003DFD RID: 15869 RVA: 0x00160984 File Offset: 0x0015EB84
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

		// Token: 0x04003F11 RID: 16145
		public int pieceId;

		// Token: 0x04003F12 RID: 16146
		public int parentPieceId;

		// Token: 0x04003F13 RID: 16147
		public float score;

		// Token: 0x04003F14 RID: 16148
		public Vector3 localPosition;

		// Token: 0x04003F15 RID: 16149
		public Quaternion localRotation;

		// Token: 0x04003F16 RID: 16150
		public int attachIndex;

		// Token: 0x04003F17 RID: 16151
		public int parentAttachIndex;

		// Token: 0x04003F18 RID: 16152
		public float attachDistance;

		// Token: 0x04003F19 RID: 16153
		public Vector3 attachPlaneNormal;

		// Token: 0x04003F1A RID: 16154
		public SnapBounds attachBounds;

		// Token: 0x04003F1B RID: 16155
		public SnapBounds parentAttachBounds;

		// Token: 0x04003F1C RID: 16156
		public byte twist;

		// Token: 0x04003F1D RID: 16157
		public sbyte bumpOffsetX;

		// Token: 0x04003F1E RID: 16158
		public sbyte bumpOffsetZ;
	}
}
