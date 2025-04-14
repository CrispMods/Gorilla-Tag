using System;

namespace LitJson
{
	// Token: 0x02000959 RID: 2393
	internal enum ParserToken
	{
		// Token: 0x04003B8C RID: 15244
		None = 65536,
		// Token: 0x04003B8D RID: 15245
		Number,
		// Token: 0x04003B8E RID: 15246
		True,
		// Token: 0x04003B8F RID: 15247
		False,
		// Token: 0x04003B90 RID: 15248
		Null,
		// Token: 0x04003B91 RID: 15249
		CharSeq,
		// Token: 0x04003B92 RID: 15250
		Char,
		// Token: 0x04003B93 RID: 15251
		Text,
		// Token: 0x04003B94 RID: 15252
		Object,
		// Token: 0x04003B95 RID: 15253
		ObjectPrime,
		// Token: 0x04003B96 RID: 15254
		Pair,
		// Token: 0x04003B97 RID: 15255
		PairRest,
		// Token: 0x04003B98 RID: 15256
		Array,
		// Token: 0x04003B99 RID: 15257
		ArrayPrime,
		// Token: 0x04003B9A RID: 15258
		Value,
		// Token: 0x04003B9B RID: 15259
		ValueRest,
		// Token: 0x04003B9C RID: 15260
		String,
		// Token: 0x04003B9D RID: 15261
		End,
		// Token: 0x04003B9E RID: 15262
		Epsilon
	}
}
