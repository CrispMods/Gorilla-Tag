using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C91 RID: 3217
	public abstract class DrawBase : MonoBehaviour
	{
		// Token: 0x0600510F RID: 20751 RVA: 0x001893AC File Offset: 0x001875AC
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

		// Token: 0x06005110 RID: 20752
		protected abstract void Draw(Color color, DebugUtil.Style style, bool depthTest);

		// Token: 0x0400536D RID: 21357
		public Color WireframeColor = Color.white;

		// Token: 0x0400536E RID: 21358
		public Color ShadededColor = Color.gray;

		// Token: 0x0400536F RID: 21359
		public bool Wireframe;

		// Token: 0x04005370 RID: 21360
		public DebugUtil.Style Style = DebugUtil.Style.FlatShaded;

		// Token: 0x04005371 RID: 21361
		public bool DepthTest = true;
	}
}
