using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC6 RID: 3270
	[ExecuteInEditMode]
	public class DrawSphere : DrawBase
	{
		// Token: 0x0600527D RID: 21117 RVA: 0x00065921 File Offset: 0x00063B21
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x0600527E RID: 21118 RVA: 0x001C01E0 File Offset: 0x001BE3E0
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005485 RID: 21637
		public float Radius = 1f;

		// Token: 0x04005486 RID: 21638
		public int NumSegments = 64;

		// Token: 0x04005487 RID: 21639
		public float StartAngle;

		// Token: 0x04005488 RID: 21640
		public float ArcAngle = 60f;
	}
}
