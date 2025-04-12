using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C98 RID: 3224
	[ExecuteInEditMode]
	public class DrawSphere : DrawBase
	{
		// Token: 0x06005127 RID: 20775 RVA: 0x00063EAB File Offset: 0x000620AB
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001B80FC File Offset: 0x001B62FC
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			Quaternion rhs = QuaternionUtil.AxisAngle(Vector3.forward, this.StartAngle * MathUtil.Deg2Rad);
			DebugUtil.DrawArc(base.transform.position, base.transform.rotation * rhs * Vector3.right, base.transform.rotation * Vector3.forward, this.ArcAngle * MathUtil.Deg2Rad, this.Radius, this.NumSegments, color, depthTest);
		}

		// Token: 0x0400538B RID: 21387
		public float Radius = 1f;

		// Token: 0x0400538C RID: 21388
		public int NumSegments = 64;

		// Token: 0x0400538D RID: 21389
		public float StartAngle;

		// Token: 0x0400538E RID: 21390
		public float ArcAngle = 60f;
	}
}
