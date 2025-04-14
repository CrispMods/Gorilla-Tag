using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C92 RID: 3218
	[ExecuteInEditMode]
	public class DrawArc : DrawBase
	{
		// Token: 0x06005115 RID: 20757 RVA: 0x0018979F File Offset: 0x0018799F
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001897D8 File Offset: 0x001879D8
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005376 RID: 21366
		public float Radius = 1f;

		// Token: 0x04005377 RID: 21367
		public int NumSegments = 64;

		// Token: 0x04005378 RID: 21368
		public float StartAngle;

		// Token: 0x04005379 RID: 21369
		public float ArcAngle = 60f;
	}
}
