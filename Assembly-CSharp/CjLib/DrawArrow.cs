using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C90 RID: 3216
	[ExecuteInEditMode]
	public class DrawArrow : DrawBase
	{
		// Token: 0x0600510C RID: 20748 RVA: 0x001892B4 File Offset: 0x001874B4
		private void OnValidate()
		{
			this.ConeRadius = Mathf.Max(0f, this.ConeRadius);
			this.ConeHeight = Mathf.Max(0f, this.ConeHeight);
			this.StemThickness = Mathf.Max(0f, this.StemThickness);
			this.NumSegments = Mathf.Max(4, this.NumSegments);
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x00189318 File Offset: 0x00187518
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawArrow(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), this.ConeRadius, this.ConeHeight, this.NumSegments, this.StemThickness, color, depthTest, style);
		}

		// Token: 0x04005368 RID: 21352
		public Vector3 LocalEndVector = Vector3.right;

		// Token: 0x04005369 RID: 21353
		public float ConeRadius = 0.05f;

		// Token: 0x0400536A RID: 21354
		public float ConeHeight = 0.1f;

		// Token: 0x0400536B RID: 21355
		public float StemThickness = 0.05f;

		// Token: 0x0400536C RID: 21356
		public int NumSegments = 8;
	}
}
