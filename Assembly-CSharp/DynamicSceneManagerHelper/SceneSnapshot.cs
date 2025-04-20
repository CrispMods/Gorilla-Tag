using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A62 RID: 2658
	internal class SceneSnapshot
	{
		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x06004281 RID: 17025 RVA: 0x0005B707 File Offset: 0x00059907
		public Dictionary<OVRAnchor, SceneSnapshot.Data> Anchors { get; } = new Dictionary<OVRAnchor, SceneSnapshot.Data>();

		// Token: 0x06004282 RID: 17026 RVA: 0x0005B70F File Offset: 0x0005990F
		public bool Contains(OVRAnchor anchor)
		{
			return this.Anchors.ContainsKey(anchor);
		}

		// Token: 0x02000A63 RID: 2659
		public class Data
		{
			// Token: 0x0400435D RID: 17245
			public List<OVRAnchor> Children;

			// Token: 0x0400435E RID: 17246
			public Rect? Rect;

			// Token: 0x0400435F RID: 17247
			public Bounds? Bounds;
		}
	}
}
