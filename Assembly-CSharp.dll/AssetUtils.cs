using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200082C RID: 2092
public static class AssetUtils
{
	// Token: 0x06003321 RID: 13089 RVA: 0x0002F75F File Offset: 0x0002D95F
	[Conditional("UNITY_EDITOR")]
	public static void ExecAndUnloadUnused(Action action)
	{
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x00050D0D File Offset: 0x0004EF0D
	[Conditional("UNITY_EDITOR")]
	public static void LoadAssetOfType<T>(ref T result, ref string resultPath) where T : UnityEngine.Object
	{
		result = default(T);
		resultPath = null;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x00050D19 File Offset: 0x0004EF19
	[Conditional("UNITY_EDITOR")]
	public static void FindAllAssetsOfType<T>(ref T[] results, ref string[] assetPaths) where T : UnityEngine.Object
	{
		results = Array.Empty<T>();
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x0002F75F File Offset: 0x0002D95F
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave<T>(this IList<T> assets, Action<T> onPreSave = null, bool unloadUnusedAfter = false) where T : UnityEngine.Object
	{
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x0002F75F File Offset: 0x0002D95F
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave(this UnityEngine.Object asset)
	{
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x00050D22 File Offset: 0x0004EF22
	public static long ComputeAssetId(this UnityEngine.Object asset, bool unsigned = false)
	{
		return 0L;
	}
}
