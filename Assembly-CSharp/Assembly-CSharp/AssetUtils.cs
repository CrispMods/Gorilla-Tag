using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200082C RID: 2092
public static class AssetUtils
{
	// Token: 0x06003321 RID: 13089 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void ExecAndUnloadUnused(Action action)
	{
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000F498A File Offset: 0x000F2B8A
	[Conditional("UNITY_EDITOR")]
	public static void LoadAssetOfType<T>(ref T result, ref string resultPath) where T : Object
	{
		result = default(T);
		resultPath = null;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x000F4996 File Offset: 0x000F2B96
	[Conditional("UNITY_EDITOR")]
	public static void FindAllAssetsOfType<T>(ref T[] results, ref string[] assetPaths) where T : Object
	{
		results = Array.Empty<T>();
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave<T>(this IList<T> assets, Action<T> onPreSave = null, bool unloadUnusedAfter = false) where T : Object
	{
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave(this Object asset)
	{
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x000F499F File Offset: 0x000F2B9F
	public static long ComputeAssetId(this Object asset, bool unsigned = false)
	{
		return 0L;
	}
}
