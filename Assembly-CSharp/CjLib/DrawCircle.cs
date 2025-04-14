using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C93 RID: 3219
	[ExecuteInEditMode]
	public class DrawCircle : DrawBase
	{
		// Token: 0x06005115 RID: 20757 RVA: 0x001894F8 File Offset: 0x001876F8
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00189522 File Offset: 0x00187722
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawCircle(base.transform.position, base.transform.rotation * Vector3.back, this.Radius, this.NumSegments, color, depthTest, style);
		}

		// Token: 0x04005376 RID: 21366
		public float Radius = 1f;

		// Token: 0x04005377 RID: 21367
		public int NumSegments = 64;
	}
}
