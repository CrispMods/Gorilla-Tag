using System;

namespace LitJson
{
	// Token: 0x02000947 RID: 2375
	public class JsonException : ApplicationException
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x00109CC1 File Offset: 0x00107EC1
		public JsonException()
		{
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x00109CC9 File Offset: 0x00107EC9
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00109CE1 File Offset: 0x00107EE1
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00109CFA File Offset: 0x00107EFA
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00109D13 File Offset: 0x00107F13
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x00109D2D File Offset: 0x00107F2D
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00109D36 File Offset: 0x00107F36
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
