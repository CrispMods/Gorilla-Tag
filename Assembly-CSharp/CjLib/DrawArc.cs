using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC0 RID: 3264
	[ExecuteInEditMode]
	public class DrawArc : DrawBase
	{
		// Token: 0x0600526B RID: 21099 RVA: 0x00065739 File Offset: 0x00063939
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001BFFD0 File Offset: 0x001BE1D0
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005470 RID: 21616
		public float Radius = 1f;

		// Token: 0x04005471 RID: 21617
		public int NumSegments = 64;

		// Token: 0x04005472 RID: 21618
		public float StartAngle;

		// Token: 0x04005473 RID: 21619
		public float ArcAngle = 60f;
	}
}
