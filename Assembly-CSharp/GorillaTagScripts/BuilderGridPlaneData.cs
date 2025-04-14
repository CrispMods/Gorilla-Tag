using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200099C RID: 2460
	public struct BuilderGridPlaneData
	{
		// Token: 0x06003CE2 RID: 15586 RVA: 0x0011E3E0 File Offset: 0x0011C5E0
		public BuilderGridPlaneData(BuilderAttachGridPlane gridPlane, int pieceIndex)
		{
			gridPlane.center.transform.GetPositionAndRotation(out this.position, out this.rotation);
			this.localPosition = gridPlane.pieceToGridPosition;
			this.localRotation = gridPlane.pieceToGridRotation;
			this.width = gridPlane.width;
			this.length = gridPlane.length;
			this.male = gridPlane.male;
			this.pieceId = gridPlane.piece.pieceId;
			this.pieceIndex = pieceIndex;
			this.boundingRadius = gridPlane.boundingRadius;
			this.attachIndex = gridPlane.attachIndex;
		}

		// Token: 0x04003E1C RID: 15900
		public int width;

		// Token: 0x04003E1D RID: 15901
		public int length;

		// Token: 0x04003E1E RID: 15902
		public bool male;

		// Token: 0x04003E1F RID: 15903
		public int pieceId;

		// Token: 0x04003E20 RID: 15904
		public int pieceIndex;

		// Token: 0x04003E21 RID: 15905
		public float boundingRadius;

		// Token: 0x04003E22 RID: 15906
		public int attachIndex;

		// Token: 0x04003E23 RID: 15907
		public Vector3 position;

		// Token: 0x04003E24 RID: 15908
		public Quaternion rotation;

		// Token: 0x04003E25 RID: 15909
		public Vector3 localPosition;

		// Token: 0x04003E26 RID: 15910
		public Quaternion localRotation;
	}
}
