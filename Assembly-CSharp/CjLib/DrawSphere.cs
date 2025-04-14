using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C95 RID: 3221
	[ExecuteInEditMode]
	public class DrawSphere : DrawBase
	{
		// Token: 0x0600511B RID: 20763 RVA: 0x001895CB File Offset: 0x001877CB
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001895F8 File Offset: 0x001877F8
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005379 RID: 21369
		public float Radius = 1f;

		// Token: 0x0400537A RID: 21370
		public int NumSegments = 64;

		// Token: 0x0400537B RID: 21371
		public float StartAngle;

		// Token: 0x0400537C RID: 21372
		public float ArcAngle = 60f;
	}
}
