using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaTag.Scripts.Utilities
{
	// Token: 0x02000BBF RID: 3007
	public static class GTStr
	{
		// Token: 0x06004BF8 RID: 19448 RVA: 0x001718CC File Offset: 0x0016FACC
		public static void Bullet(StringBuilder builder, IList<string> strings, string bulletStr = "- ")
		{
			for (int i = 0; i < strings.Count; i++)
			{
				builder.Append(bulletStr).Append(strings[i]).Append("\n");
			}
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x00171908 File Offset: 0x0016FB08
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
