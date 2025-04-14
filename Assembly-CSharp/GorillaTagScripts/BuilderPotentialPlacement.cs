using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000991 RID: 2449
	public struct BuilderPotentialPlacement
	{
		// Token: 0x06003BFD RID: 15357 RVA: 0x00114BE4 File Offset: 0x00112DE4
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

		// Token: 0x04003D30 RID: 15664
		public BuilderPiece attachPiece;

		// Token: 0x04003D31 RID: 15665
		public BuilderPiece parentPiece;

		// Token: 0x04003D32 RID: 15666
		public int attachIndex;

		// Token: 0x04003D33 RID: 15667
		public int parentAttachIndex;

		// Token: 0x04003D34 RID: 15668
		public Vector3 localPosition;

		// Token: 0x04003D35 RID: 15669
		public Quaternion localRotation;

		// Token: 0x04003D36 RID: 15670
		public Vector3 attachPlaneNormal;

		// Token: 0x04003D37 RID: 15671
		public float attachDistance;

		// Token: 0x04003D38 RID: 15672
		public float score;

		// Token: 0x04003D39 RID: 15673
		public SnapBounds attachBounds;

		// Token: 0x04003D3A RID: 15674
		public SnapBounds parentAttachBounds;

		// Token: 0x04003D3B RID: 15675
		public byte twist;

		// Token: 0x04003D3C RID: 15676
		public sbyte bumpOffsetX;

		// Token: 0x04003D3D RID: 15677
		public sbyte bumpOffsetZ;
	}
}
