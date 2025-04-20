using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B7 RID: 2487
	public struct BuilderPotentialPlacement
	{
		// Token: 0x06003D15 RID: 15637 RVA: 0x001578D8 File Offset: 0x00155AD8
		public void Reset()
		{
			this.attachPiece = null;
			this.parentPiece = null;
			this.attachIndex = -1;
			this.parentAttachIndex = -1;
			this.localPosition = Vector3.zero;
			this.localRotation = Quaternion.identity;
			this.attachDistance = float.MaxValue;
			this.attachPlaneNormal = Vector3.zero;
			this.score = float.MinValue;
			this.twist = 0;
			this.bumpOffsetX = 0;
			this.bumpOffsetZ = 0;
		}

		// Token: 0x04003E0A RID: 15882
		public BuilderPiece attachPiece;

		// Token: 0x04003E0B RID: 15883
		public BuilderPiece parentPiece;

		// Token: 0x04003E0C RID: 15884
		public int attachIndex;

		// Token: 0x04003E0D RID: 15885
		public int parentAttachIndex;

		// Token: 0x04003E0E RID: 15886
		public Vector3 localPosition;

		// Token: 0x04003E0F RID: 15887
		public Quaternion localRotation;

		// Token: 0x04003E10 RID: 15888
		public Vector3 attachPlaneNormal;

		// Token: 0x04003E11 RID: 15889
		public float attachDistance;

		// Token: 0x04003E12 RID: 15890
		public float score;

		// Token: 0x04003E13 RID: 15891
		public SnapBounds attachBounds;

		// Token: 0x04003E14 RID: 15892
		public SnapBounds parentAttachBounds;

		// Token: 0x04003E15 RID: 15893
		public byte twist;

		// Token: 0x04003E16 RID: 15894
		public sbyte bumpOffsetX;

		// Token: 0x04003E17 RID: 15895
		public sbyte bumpOffsetZ;
	}
}
