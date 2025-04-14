using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A20 RID: 2592
	public static class AssetDatabase
	{
		// Token: 0x060040F3 RID: 16627 RVA: 0x00134D10 File Offset: 0x00132F10
		public static T LoadAssetAtPath<T>(string assetPath) where T : Object
		{
			return default(T);
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00134D26 File Offset: 0x00132F26
		public static T[] LoadAssetsOfType<T>() where T : Object
		{
			return Array.Empty<T>();
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00134D2D File Offset: 0x00132F2D
		public static string[] FindAssetsOfType<T>() where T : Object
		{
			return Array.Empty<string>();
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00134D34 File Offset: 0x00132F34
		[Conditional("UNITY_EDITOR")]
		public static void SaveToDisk(params Object[] assetsToSave)
		{
			AssetDatabase.SaveAssetsToDisk(assetsToSave, true);
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void SaveAssetsToDisk(Object[] assetsToSave, bool saveProject = true)
		{
		}
	}
}
