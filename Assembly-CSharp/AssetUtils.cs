using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000829 RID: 2089
public static class AssetUtils
{
	// Token: 0x06003315 RID: 13077 RVA: 0x000023F4 File Offset: 0x000005F4
	[Conditional("UNITY_EDITOR")]
	public static void ExecAndUnloadUnused(Action action)
	{
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000F43C2 File Offset: 0x000F25C2
	[Conditional("UNITY_EDITOR")]
	public static void LoadAssetOfType<T>(ref T result, ref string resultPath) where T : Object
	{
		result = default(T);
		resultPath = null;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x000F43CE File Offset: 0x000F25CE
	[Conditional("UNITY_EDITOR")]
	public static void FindAllAssetsOfType<T>(ref T[] results, ref string[] assetPaths) where T : Object
	{
		results = Array.Empty<T>();
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave<T>(this IList<T> assets, Action<T> onPreSave = null, bool unloadUnusedAfter = false) where T : Object
	{
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000023F4 File Offset: 0x000005F4
	[HideInCallstack]
	[Conditional("UNITY_EDITOR")]
	public static void ForceSave(this Object asset)
	{
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x000F43D7 File Offset: 0x000F25D7
	public static long ComputeAssetId(this Object asset, bool unsigned = false)
	{
		return 0L;
	}
}
