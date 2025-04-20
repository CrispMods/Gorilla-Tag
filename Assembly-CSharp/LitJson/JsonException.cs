using System;

namespace LitJson
{
	// Token: 0x02000961 RID: 2401
	public class JsonException : ApplicationException
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x00056157 File Offset: 0x00054357
		public JsonException()
		{
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x0005615F File Offset: 0x0005435F
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00056177 File Offset: 0x00054377
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00056190 File Offset: 0x00054390
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x000561A9 File Offset: 0x000543A9
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x000561C3 File Offset: 0x000543C3
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x000561CC File Offset: 0x000543CC
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
