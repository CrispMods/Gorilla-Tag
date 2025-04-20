using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

// Token: 0x020008BD RID: 2237
public static class GTSceneUtils
{
	// Token: 0x0600363E RID: 13886 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	public static void AddToBuild(GTScene scene)
	{
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x00053B93 File Offset: 0x00051D93
	public static bool Equals(GTScene x, Scene y)
	{
		return !(x == null) && y.IsValid() && x.Equals(y);
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x00053BB7 File Offset: 0x00051DB7
	public static GTScene[] ScenesInBuild()
	{
		return Array.Empty<GTScene>();
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	public static void SyncBuildScenes()
	{
	}
}
