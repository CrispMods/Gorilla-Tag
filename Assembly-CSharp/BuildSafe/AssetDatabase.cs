using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A4D RID: 2637
	public static class AssetDatabase
	{
		// Token: 0x06004238 RID: 16952 RVA: 0x0017501C File Offset: 0x0017321C
		public static T LoadAssetAtPath<T>(string assetPath) where T : UnityEngine.Object
		{
			return default(T);
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x0005B53D File Offset: 0x0005973D
		public static T[] LoadAssetsOfType<T>() where T : UnityEngine.Object
		{
			return Array.Empty<T>();
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x0005B544 File Offset: 0x00059744
		public static string[] FindAssetsOfType<T>() where T : UnityEngine.Object
		{
			return Array.Empty<string>();
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x0005B54B File Offset: 0x0005974B
		[Conditional("UNITY_EDITOR")]
		public static void SaveToDisk(params UnityEngine.Object[] assetsToSave)
		{
			AssetDatabase.SaveAssetsToDisk(assetsToSave, true);
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00030607 File Offset: 0x0002E807
		public static void SaveAssetsToDisk(UnityEngine.Object[] assetsToSave, bool saveProject = true)
		{
		}
	}
}
