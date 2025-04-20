using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000843 RID: 2115
public static class AssetUtils
{
	// Token: 0x060033D0 RID: 13264 RVA: 0x00030607 File Offset: 0x0002E807
	[Conditional("UNITY_EDITOR")]
	public static void ExecAndUnloadUnused(Action action)
	{
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x0005211B File Offset: 0x0005031B
	[Conditional("UNITY_EDITOR")]
	public static void LoadAssetOfType<T>(ref T result, ref string resultPath) where T : UnityEngine.Object
	{
		result = default(T);
		resultPath = null;
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x00052127 File Offset: 0x00050327
	[Conditional("UNITY_EDITOR")]
	public static void FindAllAssetsOfType<T>(ref T[] results, ref string[] assetPaths) where T : UnityEngine.Object
	{
		results = Array.Empty<T>();
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave<T>(this IList<T> assets, Action<T> onPreSave = null, bool unloadUnusedAfter = false) where T : UnityEngine.Object
	{
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x00030607 File Offset: 0x0002E807
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave(this UnityEngine.Object asset)
	{
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x00052130 File Offset: 0x00050330
	public static long ComputeAssetId(this UnityEngine.Object asset, bool unsigned = false)
	{
		return 0L;
	}
}
