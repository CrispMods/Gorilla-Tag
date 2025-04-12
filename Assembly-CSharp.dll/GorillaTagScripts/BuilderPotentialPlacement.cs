using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000994 RID: 2452
	public struct BuilderPotentialPlacement
	{
		// Token: 0x06003C09 RID: 15369 RVA: 0x001518F0 File Offset: 0x0014FAF0
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

		// Token: 0x04003D42 RID: 15682
		public BuilderPiece attachPiece;

		// Token: 0x04003D43 RID: 15683
		public BuilderPiece parentPiece;

		// Token: 0x04003D44 RID: 15684
		public int attachIndex;

		// Token: 0x04003D45 RID: 15685
		public int parentAttachIndex;

		// Token: 0x04003D46 RID: 15686
		public Vector3 localPosition;

		// Token: 0x04003D47 RID: 15687
		public Quaternion localRotation;

		// Token: 0x04003D48 RID: 15688
		public Vector3 attachPlaneNormal;

		// Token: 0x04003D49 RID: 15689
		public float attachDistance;

		// Token: 0x04003D4A RID: 15690
		public float score;

		// Token: 0x04003D4B RID: 15691
		public SnapBounds attachBounds;

		// Token: 0x04003D4C RID: 15692
		public SnapBounds parentAttachBounds;

		// Token: 0x04003D4D RID: 15693
		public byte twist;

		// Token: 0x04003D4E RID: 15694
		public sbyte bumpOffsetX;

		// Token: 0x04003D4F RID: 15695
		public sbyte bumpOffsetZ;
	}
}
