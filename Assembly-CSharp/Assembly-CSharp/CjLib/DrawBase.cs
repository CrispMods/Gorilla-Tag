using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C94 RID: 3220
	public abstract class DrawBase : MonoBehaviour
	{
		// Token: 0x0600511B RID: 20763 RVA: 0x00189974 File Offset: 0x00187B74
		private void Update()
		{
			if (this.Style != DebugUtil.Style.Wireframe)
			{
				this.Draw(this.ShadededColor, this.Style, this.DepthTest);
			}
			if (this.Style == DebugUtil.Style.Wireframe || this.Wireframe)
			{
				this.Draw(this.WireframeColor, DebugUtil.Style.Wireframe, this.DepthTest);
			}
		}

		// Token: 0x0600511C RID: 20764
		protected abstract void Draw(Color color, DebugUtil.Style style, bool depthTest);

		// Token: 0x0400537F RID: 21375
		public Color WireframeColor = Color.white;

		// Token: 0x04005380 RID: 21376
		public Color ShadededColor = Color.gray;

		// Token: 0x04005381 RID: 21377
		public bool Wireframe;

		// Token: 0x04005382 RID: 21378
		public DebugUtil.Style Style = DebugUtil.Style.FlatShaded;

		// Token: 0x04005383 RID: 21379
		public bool DepthTest = true;
	}
}
