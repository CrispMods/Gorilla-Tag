using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C95 RID: 3221
	[ExecuteInEditMode]
	public class DrawBox : DrawBase
	{
		// Token: 0x0600511E RID: 20766 RVA: 0x001899F0 File Offset: 0x00187BF0
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x00189A1C File Offset: 0x00187C1C
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x04005384 RID: 21380
		public float Radius = 1f;

		// Token: 0x04005385 RID: 21381
		public int NumSegments = 64;

		// Token: 0x04005386 RID: 21382
		public float StartAngle;

		// Token: 0x04005387 RID: 21383
		public float ArcAngle = 60f;
	}
}
