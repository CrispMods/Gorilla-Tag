using System;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A34 RID: 2612
	public static class SceneViewUtils
	{
		// Token: 0x0600413A RID: 16698 RVA: 0x0013565D File Offset: 0x0013385D
		private static bool RaycastWorldSafe(Vector2 screenPos, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x04004272 RID: 17010
		public static readonly SceneViewUtils.FuncRaycastWorld RaycastWorld = new SceneViewUtils.FuncRaycastWorld(SceneViewUtils.RaycastWorldSafe);

		// Token: 0x02000A35 RID: 2613
		// (Invoke) Token: 0x0600413D RID: 16701
		public delegate bool FuncRaycastWorld(Vector2 screenPos, out RaycastHit hit);

		// Token: 0x02000A36 RID: 2614
		// (Invoke) Token: 0x06004141 RID: 16705
		public delegate GameObject FuncPickClosestGameObject(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex);
	}
}
