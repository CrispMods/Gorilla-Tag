using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009C2 RID: 2498
	public struct BuilderGridPlaneData
	{
		// Token: 0x06003DFA RID: 15866 RVA: 0x00160840 File Offset: 0x0015EA40
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

		// Token: 0x04003EF6 RID: 16118
		public int width;

		// Token: 0x04003EF7 RID: 16119
		public int length;

		// Token: 0x04003EF8 RID: 16120
		public bool male;

		// Token: 0x04003EF9 RID: 16121
		public int pieceId;

		// Token: 0x04003EFA RID: 16122
		public int pieceIndex;

		// Token: 0x04003EFB RID: 16123
		public float boundingRadius;

		// Token: 0x04003EFC RID: 16124
		public int attachIndex;

		// Token: 0x04003EFD RID: 16125
		public Vector3 position;

		// Token: 0x04003EFE RID: 16126
		public Quaternion rotation;

		// Token: 0x04003EFF RID: 16127
		public Vector3 localPosition;

		// Token: 0x04003F00 RID: 16128
		public Quaternion localRotation;
	}
}
