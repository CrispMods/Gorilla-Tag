using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC3 RID: 3267
	[ExecuteInEditMode]
	public class DrawBox : DrawBase
	{
		// Token: 0x06005274 RID: 21108 RVA: 0x000657FE File Offset: 0x000639FE
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005275 RID: 21109 RVA: 0x001C0160 File Offset: 0x001BE360
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x0400547E RID: 21630
		public float Radius = 1f;

		// Token: 0x0400547F RID: 21631
		public int NumSegments = 64;

		// Token: 0x04005480 RID: 21632
		public float StartAngle;

		// Token: 0x04005481 RID: 21633
		public float ArcAngle = 60f;
	}
}
