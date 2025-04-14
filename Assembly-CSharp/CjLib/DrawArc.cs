using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C8F RID: 3215
	[ExecuteInEditMode]
	public class DrawArc : DrawBase
	{
		// Token: 0x06005109 RID: 20745 RVA: 0x001891D7 File Offset: 0x001873D7
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x00189210 File Offset: 0x00187410
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005364 RID: 21348
		public float Radius = 1f;

		// Token: 0x04005365 RID: 21349
		public int NumSegments = 64;

		// Token: 0x04005366 RID: 21350
		public float StartAngle;

		// Token: 0x04005367 RID: 21351
		public float ArcAngle = 60f;
	}
}
