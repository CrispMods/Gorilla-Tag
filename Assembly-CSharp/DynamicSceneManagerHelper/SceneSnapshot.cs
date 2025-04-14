using System;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A35 RID: 2613
	internal class SceneSnapshot
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x001350BE File Offset: 0x001332BE
		public Dictionary<OVRAnchor, SceneSnapshot.Data> Anchors { get; } = new Dictionary<OVRAnchor, SceneSnapshot.Data>();

		// Token: 0x0600413D RID: 16701 RVA: 0x001350C6 File Offset: 0x001332C6
		public bool Contains(OVRAnchor anchor)
		{
			return this.Anchors.ContainsKey(anchor);
		}

		// Token: 0x02000A36 RID: 2614
		public class Data
		{
			// Token: 0x04004263 RID: 16995
			public List<OVRAnchor> Children;

			// Token: 0x04004264 RID: 16996
			public Rect? Rect;

			// Token: 0x04004265 RID: 16997
			public Bounds? Bounds;
		}
	}
}
