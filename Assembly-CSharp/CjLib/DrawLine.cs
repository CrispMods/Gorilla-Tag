using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000CC5 RID: 3269
	[ExecuteInEditMode]
	public class DrawLine : DrawBase
	{
		// Token: 0x0600527A RID: 21114 RVA: 0x000658C9 File Offset: 0x00063AC9
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x000658D9 File Offset: 0x00063AD9
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawLine(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), color, depthTest);
		}

		// Token: 0x04005484 RID: 21636
		public Vector3 LocalEndVector = Vector3.right;
	}
}
