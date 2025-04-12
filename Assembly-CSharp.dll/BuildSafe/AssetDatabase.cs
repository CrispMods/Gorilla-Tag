using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A23 RID: 2595
	public static class AssetDatabase
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x0016E198 File Offset: 0x0016C398
		public static T LoadAssetAtPath<T>(string assetPath) where T : UnityEngine.Object
		{
			return default(T);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00059B3B File Offset: 0x00057D3B
		public static T[] LoadAssetsOfType<T>() where T : UnityEngine.Object
		{
			return Array.Empty<T>();
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00059B42 File Offset: 0x00057D42
		public static string[] FindAssetsOfType<T>() where T : UnityEngine.Object
		{
			return Array.Empty<string>();
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00059B49 File Offset: 0x00057D49
		[Conditional("UNITY_EDITOR")]
		public static void SaveToDisk(params UnityEngine.Object[] assetsToSave)
		{
			AssetDatabase.SaveAssetsToDisk(assetsToSave, true);
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x0002F75F File Offset: 0x0002D95F
		public static void SaveAssetsToDisk(UnityEngine.Object[] assetsToSave, bool saveProject = true)
		{
		}
	}
}
