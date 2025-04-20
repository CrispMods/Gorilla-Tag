using System;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A5E RID: 2654
	public static class SceneViewUtils
	{
		// Token: 0x06004273 RID: 17011 RVA: 0x0005B6DE File Offset: 0x000598DE
		private static bool RaycastWorldSafe(Vector2 screenPos, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			return false;
		}

		// Token: 0x0400435A RID: 17242
		public static readonly SceneViewUtils.FuncRaycastWorld RaycastWorld = new SceneViewUtils.FuncRaycastWorld(SceneViewUtils.RaycastWorldSafe);

		// Token: 0x02000A5F RID: 2655
		// (Invoke) Token: 0x06004276 RID: 17014
		public delegate bool FuncRaycastWorld(Vector2 screenPos, out RaycastHit hit);

		// Token: 0x02000A60 RID: 2656
		// (Invoke) Token: 0x0600427A RID: 17018
		public delegate GameObject FuncPickClosestGameObject(Camera cam, int layers, Vector2 position, GameObject[] ignore, GameObject[] filter, out int materialIndex);
	}
}
