using System;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A31 RID: 2609
	public static class SceneViewUtils
	{
		// Token: 0x0600412E RID: 16686 RVA: 0x00135095 File Offset: 0x00133295
		private static bool RaycastWorldSafe(Vector2 screenPos, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x04004260 RID: 16992
		public static readonly SceneViewUtils.FuncRaycastWorld RaycastWorld = new SceneViewUtils.FuncRaycastWorld(SceneViewUtils.RaycastWorldSafe);

		// Token: 0x02000A32 RID: 2610
		// (Invoke) Token: 0x06004131 RID: 16689
		public delegate bool FuncRaycastWorld(Vector2 screenPos, out RaycastHit hit);

		// Token: 0x02000A33 RID: 2611
		// (Invoke) Token: 0x06004135 RID: 16693
		public delegate GameObject FuncPickClosestGameObject(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex);
	}
}
