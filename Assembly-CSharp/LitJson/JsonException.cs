using System;

namespace LitJson
{
	// Token: 0x02000944 RID: 2372
	public class JsonException : ApplicationException
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x001096F9 File Offset: 0x001078F9
		public JsonException()
		{
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x00109701 File Offset: 0x00107901
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00109719 File Offset: 0x00107919
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x00109732 File Offset: 0x00107932
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0010974B File Offset: 0x0010794B
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x00109765 File Offset: 0x00107965
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0010976E File Offset: 0x0010796E
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
