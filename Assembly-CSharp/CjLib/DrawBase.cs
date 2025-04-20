using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC2 RID: 3266
	public abstract class DrawBase : MonoBehaviour
	{
		// Token: 0x06005271 RID: 21105 RVA: 0x001C0110 File Offset: 0x001BE310
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

		// Token: 0x06005272 RID: 21106
		protected abstract void Draw(Color color, DebugUtil.Style style, bool depthTest);

		// Token: 0x04005479 RID: 21625
		public Color WireframeColor = Color.white;

		// Token: 0x0400547A RID: 21626
		public Color ShadededColor = Color.gray;

		// Token: 0x0400547B RID: 21627
		public bool Wireframe;

		// Token: 0x0400547C RID: 21628
		public DebugUtil.Style Style = DebugUtil.Style.FlatShaded;

		// Token: 0x0400547D RID: 21629
		public bool DepthTest = true;
	}
}
