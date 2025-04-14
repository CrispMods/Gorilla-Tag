using System;

namespace Utilities
{
	// Token: 0x02000964 RID: 2404
	public static class PathUtils
	{
		// Token: 0x06003A9D RID: 15005 RVA: 0x0010DBF4 File Offset: 0x0010BDF4
		public static string Resolve(params string[] subPaths)
		{
			if (subPaths == null || subPaths.Length == 0)
			{
				return null;
			}
			string[] value = string.Concat(subPaths).Split(PathUtils.kPathSeps, StringSplitOptions.RemoveEmptyEntries);
			return Uri.UnescapeDataString(new Uri(string.Join("/", value)).AbsolutePath);
		}

		// Token: 0x04003BB1 RID: 15281
		private static readonly char[] kPathSeps = new char[]
		{
			'\\',
			'/'
		};

		// Token: 0x04003BB2 RID: 15282
		private const string kFwdSlash = "/";
	}
}
