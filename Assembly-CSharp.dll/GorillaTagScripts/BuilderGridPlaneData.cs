using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200099F RID: 2463
	public struct BuilderGridPlaneData
	{
		// Token: 0x06003CEE RID: 15598 RVA: 0x0015A858 File Offset: 0x00158A58
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

		// Token: 0x04003E2E RID: 15918
		public int width;

		// Token: 0x04003E2F RID: 15919
		public int length;

		// Token: 0x04003E30 RID: 15920
		public bool male;

		// Token: 0x04003E31 RID: 15921
		public int pieceId;

		// Token: 0x04003E32 RID: 15922
		public int pieceIndex;

		// Token: 0x04003E33 RID: 15923
		public float boundingRadius;

		// Token: 0x04003E34 RID: 15924
		public int attachIndex;

		// Token: 0x04003E35 RID: 15925
		public Vector3 position;

		// Token: 0x04003E36 RID: 15926
		public Quaternion rotation;

		// Token: 0x04003E37 RID: 15927
		public Vector3 localPosition;

		// Token: 0x04003E38 RID: 15928
		public Quaternion localRotation;
	}
}
