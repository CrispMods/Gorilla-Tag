using System;
using Cysharp.Text;
using TMPro;

namespace GorillaExtensions
{
	// Token: 0x02000B98 RID: 2968
	public static class GTTextMeshProExtensions
	{
		// Token: 0x06004A6B RID: 19051 RVA: 0x0019E800 File Offset: 0x0019CA00
		public static void SetTextToZString(this TMP_Text textMono, Utf16ValueStringBuilder zStringBuilder)
		{
			ArraySegment<char> arraySegment = zStringBuilder.AsArraySegment();
			textMono.SetCharArray(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}
	}
}
