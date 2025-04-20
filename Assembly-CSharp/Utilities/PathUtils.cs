using System;

namespace Utilities
{
	// Token: 0x0200098A RID: 2442
	public static class PathUtils
	{
		// Token: 0x06003BB5 RID: 15285 RVA: 0x001516E4 File Offset: 0x0014F8E4
		public static string Resolve(params string[] subPaths)
		{
			if (subPaths == null || subPaths.Length == 0)
			{
				return null;
			}
			string[] value = string.Concat(subPaths).Split(PathUtils.kPathSeps, StringSplitOptions.RemoveEmptyEntries);
			return Uri.UnescapeDataString(new Uri(string.Join("/", value)).AbsolutePath);
		}

		// Token: 0x04003C8B RID: 15499
		private static readonly char[] kPathSeps = new char[]
		{
			'\\',
			'/'
		};

		// Token: 0x04003C8C RID: 15500
		private const string kFwdSlash = "/";
	}
}
