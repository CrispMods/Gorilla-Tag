using System;
using Cysharp.Text;
using TMPro;

namespace GorillaExtensions
{
	// Token: 0x02000B6B RID: 2923
	public static class GTTextMeshProExtensions
	{
		// Token: 0x06004920 RID: 18720 RVA: 0x0016366C File Offset: 0x0016186C
		public static void SetTextToZString(this TMP_Text textMono, Utf16ValueStringBuilder zStringBuilder)
		{
			ArraySegment<char> arraySegment = zStringBuilder.AsArraySegment();
			textMono.SetCharArray(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}
	}
}
