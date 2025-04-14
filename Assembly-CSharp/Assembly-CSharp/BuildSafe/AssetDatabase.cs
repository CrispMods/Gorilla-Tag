using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A23 RID: 2595
	public static class AssetDatabase
	{
		// Token: 0x060040FF RID: 16639 RVA: 0x001352D8 File Offset: 0x001334D8
		public static T LoadAssetAtPath<T>(string assetPath) where T : Object
		{
			return default(T);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x001352EE File Offset: 0x001334EE
		public static T[] LoadAssetsOfType<T>() where T : Object
		{
			return Array.Empty<T>();
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x001352F5 File Offset: 0x001334F5
		public static string[] FindAssetsOfType<T>() where T : Object
		{
			return Array.Empty<string>();
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x001352FC File Offset: 0x001334FC
		[Conditional("UNITY_EDITOR")]
		public static void SaveToDisk(params Object[] assetsToSave)
		{
			AssetDatabase.SaveAssetsToDisk(assetsToSave, true);
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x000023F4 File Offset: 0x000005F4
		public static void SaveAssetsToDisk(Object[] assetsToSave, bool saveProject = true)
		{
		}
	}
}
