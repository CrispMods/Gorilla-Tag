using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaTag.Scripts.Utilities
{
	// Token: 0x02000BED RID: 3053
	public static class GTStr
	{
		// Token: 0x06004D44 RID: 19780 RVA: 0x001AA434 File Offset: 0x001A8634
		public static void Bullet(StringBuilder builder, IList<string> strings, string bulletStr = "- ")
		{
			for (int i = 0; i < strings.Count; i++)
			{
				builder.Append(bulletStr).Append(strings[i]).Append("\n");
			}
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x001AA470 File Offset: 0x001A8670
		public static string Bullet(IList<string> strings, string bulletStr = "- ")
		{
			if (strings == null || strings.Count == 0)
			{
				return string.Empty;
			}
			int num = strings.Count * (bulletStr.Length + 1);
			for (int i = 0; i < strings.Count; i++)
			{
				num += strings[i].Length;
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			GTStr.Bullet(stringBuilder, strings, bulletStr);
			return stringBuilder.ToString();
		}
	}
}
