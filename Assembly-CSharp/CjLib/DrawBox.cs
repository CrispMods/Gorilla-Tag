using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C92 RID: 3218
	[ExecuteInEditMode]
	public class DrawBox : DrawBase
	{
		// Token: 0x06005112 RID: 20754 RVA: 0x00189428 File Offset: 0x00187628
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x00189454 File Offset: 0x00187654
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005372 RID: 21362
		public float Radius = 1f;

		// Token: 0x04005373 RID: 21363
		public int NumSegments = 64;

		// Token: 0x04005374 RID: 21364
		public float StartAngle;

		// Token: 0x04005375 RID: 21365
		public float ArcAngle = 60f;
	}
}
