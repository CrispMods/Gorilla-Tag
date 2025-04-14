using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

// Token: 0x020008A4 RID: 2212
public static class GTSceneUtils
{
	// Token: 0x06003582 RID: 13698 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void AddToBuild(GTScene scene)
	{
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000FEA21 File Offset: 0x000FCC21
	public static bool Equals(GTScene x, Scene y)
	{
		return !(x == null) && y.IsValid() && x.Equals(y);
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000FEA45 File Offset: 0x000FCC45
	public static GTScene[] ScenesInBuild()
	{
		return Array.Empty<GTScene>();
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void SyncBuildScenes()
	{
	}
}
