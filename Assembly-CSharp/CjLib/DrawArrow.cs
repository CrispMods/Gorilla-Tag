using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC1 RID: 3265
	[ExecuteInEditMode]
	public class DrawArrow : DrawBase
	{
		// Token: 0x0600526E RID: 21102 RVA: 0x001C0050 File Offset: 0x001BE250
		private void OnValidate()
		{
			this.ConeRadius = Mathf.Max(0f, this.ConeRadius);
			this.ConeHeight = Mathf.Max(0f, this.ConeHeight);
			this.StemThickness = Mathf.Max(0f, this.StemThickness);
			this.NumSegments = Mathf.Max(4, this.NumSegments);
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x001C00B4 File Offset: 0x001BE2B4
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawArrow(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), this.ConeRadius, this.ConeHeight, this.NumSegments, this.StemThickness, color, depthTest, style);
		}

		// Token: 0x04005474 RID: 21620
		public Vector3 LocalEndVector = Vector3.right;

		// Token: 0x04005475 RID: 21621
		public float ConeRadius = 0.05f;

		// Token: 0x04005476 RID: 21622
		public float ConeHeight = 0.1f;

		// Token: 0x04005477 RID: 21623
		public float StemThickness = 0.05f;

		// Token: 0x04005478 RID: 21624
		public int NumSegments = 8;
	}
}
