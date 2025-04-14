using System;
using System.Diagnostics;
using UnityEngine.SceneManagement;

// Token: 0x020008A1 RID: 2209
public static class GTSceneUtils
{
	// Token: 0x06003576 RID: 13686 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void AddToBuild(GTScene scene)
	{
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x000FE459 File Offset: 0x000FC659
	public static bool Equals(GTScene x, Scene y)
	{
		return !(x == null) && y.IsValid() && x.Equals(y);
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x000FE47D File Offset: 0x000FC67D
	public static GTScene[] ScenesInBuild()
	{
		return Array.Empty<GTScene>();
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void SyncBuildScenes()
	{
	}
}
