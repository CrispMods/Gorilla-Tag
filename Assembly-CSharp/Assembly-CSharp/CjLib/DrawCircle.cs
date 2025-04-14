using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C96 RID: 3222
	[ExecuteInEditMode]
	public class DrawCircle : DrawBase
	{
		// Token: 0x06005121 RID: 20769 RVA: 0x00189AC0 File Offset: 0x00187CC0
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x00189AEA File Offset: 0x00187CEA
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawCircle(base.transform.position, base.transform.rotation * Vector3.back, this.Radius, this.NumSegments, color, depthTest, style);
		}

		// Token: 0x04005388 RID: 21384
		public float Radius = 1f;

		// Token: 0x04005389 RID: 21385
		public int NumSegments = 64;
	}
}
