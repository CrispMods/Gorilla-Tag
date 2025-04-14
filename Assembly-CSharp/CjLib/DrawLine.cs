using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C94 RID: 3220
	[ExecuteInEditMode]
	public class DrawLine : DrawBase
	{
		// Token: 0x06005118 RID: 20760 RVA: 0x00189573 File Offset: 0x00187773
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x00189583 File Offset: 0x00187783
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawLine(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), color, depthTest);
		}

		// Token: 0x04005378 RID: 21368
		public Vector3 LocalEndVector = Vector3.right;
	}
}
