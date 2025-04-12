using System;

namespace LitJson
{
	// Token: 0x02000947 RID: 2375
	public class JsonException : ApplicationException
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x00054BB5 File Offset: 0x00052DB5
		public JsonException()
		{
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x00054BBD File Offset: 0x00052DBD
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00054BD5 File Offset: 0x00052DD5
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00054BEE File Offset: 0x00052DEE
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x00054C07 File Offset: 0x00052E07
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x00054C21 File Offset: 0x00052E21
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00054C2A File Offset: 0x00052E2A
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
