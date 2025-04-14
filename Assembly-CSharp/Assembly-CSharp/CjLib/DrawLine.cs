﻿using System;
using UnityEngine;

namespace CjLib
{
	// Token: 0x02000C97 RID: 3223
	[ExecuteInEditMode]
	public class DrawLine : DrawBase
	{
		// Token: 0x06005124 RID: 20772 RVA: 0x00189B3B File Offset: 0x00187D3B
		private void OnValidate()
		{
			this.Wireframe = true;
			this.Style = DebugUtil.Style.Wireframe;
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x00189B4B File Offset: 0x00187D4B
		protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
		{
			DebugUtil.DrawLine(base.transform.position, base.transform.position + base.transform.TransformVector(this.LocalEndVector), color, depthTest);
		}

		// Token: 0x0400538A RID: 21386
		public Vector3 LocalEndVector = Vector3.right;
	}
}
