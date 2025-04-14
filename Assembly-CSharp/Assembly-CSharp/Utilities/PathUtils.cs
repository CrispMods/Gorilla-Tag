using System;

namespace Utilities
{
	// Token: 0x02000967 RID: 2407
	public static class PathUtils
	{
		// Token: 0x06003AA9 RID: 15017 RVA: 0x0010E1BC File Offset: 0x0010C3BC
		public static string Resolve(params string[] subPaths)
		{
			if (subPaths == null || subPaths.Length == 0)
			{
				return null;
			}
			string[] value = string.Concat(subPaths).Split(PathUtils.kPathSeps, StringSplitOptions.RemoveEmptyEntries);
			return Uri.UnescapeDataString(new Uri(string.Join("/", value)).AbsolutePath);
		}

		// Token: 0x04003BC3 RID: 15299
		private static readonly char[] kPathSeps = new char[]
		{
			'\\',
			'/'
		};

		// Token: 0x04003BC4 RID: 15300
		private const string kFwdSlash = "/";
	}
}
