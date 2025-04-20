using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC4 RID: 3268
	[ExecuteInEditMode]
	public class DrawCircle : DrawBase
	{
		// Token: 0x06005277 RID: 21111 RVA: 0x0006584E File Offset: 0x00063A4E
		private void OnValidate()
		{
			this.Radius = Mathf.Max(0f, this.Radius);
			this.NumSegments = Mathf.Max(0, this.NumSegments);
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00065878 File Offset: 0x00063A78
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawCircle(base.transform.position, base.transform.rotation * Vector3.back, this.Radius, this.NumSegments, color, depthTest, style);
		}

		// Token: 0x04005482 RID: 21634
		public float Radius = 1f;

		// Token: 0x04005483 RID: 21635
		public int NumSegments = 64;
	}
}
