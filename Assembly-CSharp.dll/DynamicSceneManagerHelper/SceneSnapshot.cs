using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A38 RID: 2616
	internal class SceneSnapshot
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x00059D05 File Offset: 0x00057F05
		public Dictionary<OVRAnchor, SceneSnapshot.Data> Anchors { get; } = new Dictionary<OVRAnchor, SceneSnapshot.Data>();

		// Token: 0x06004149 RID: 16713 RVA: 0x00059D0D File Offset: 0x00057F0D
		public bool Contains(OVRAnchor anchor)
		{
			return this.Anchors.ContainsKey(anchor);
		}

		// Token: 0x02000A39 RID: 2617
		public class Data
		{
			// Token: 0x04004275 RID: 17013
			public List<OVRAnchor> Children;

			// Token: 0x04004276 RID: 17014
			public Rect? Rect;

			// Token: 0x04004277 RID: 17015
			public Bounds? Bounds;
		}
	}
}
