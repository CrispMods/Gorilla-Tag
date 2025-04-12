using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C93 RID: 3219
	[ExecuteInEditMode]
	public class DrawArrow : DrawBase
	{
		// Token: 0x06005118 RID: 20760 RVA: 0x001B7F6C File Offset: 0x001B616C
		private void OnValidate()
		{
			this.ConeRadius = Mathf.Max(0f, this.ConeRadius);
			this.ConeHeight = Mathf.Max(0f, this.ConeHeight);
			this.StemThickness = Mathf.Max(0f, this.StemThickness);
			this.NumSegments = Mathf.Max(4, this.NumSegments);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001B7FD0 File Offset: 0x001B61D0
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawArrow(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), this.ConeRadius, this.ConeHeight, this.NumSegments, this.StemThickness, color, depthTest, style);
		}

		// Token: 0x0400537A RID: 21370
		public Vector3 LocalEndVector = Vector3.right;

		// Token: 0x0400537B RID: 21371
		public float ConeRadius = 0.05f;

		// Token: 0x0400537C RID: 21372
		public float ConeHeight = 0.1f;

		// Token: 0x0400537D RID: 21373
		public float StemThickness = 0.05f;

		// Token: 0x0400537E RID: 21374
		public int NumSegments = 8;
	}
}
