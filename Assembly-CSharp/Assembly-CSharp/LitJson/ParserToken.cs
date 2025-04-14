using System;

namespace LitJson
{
	// Token: 0x0200095C RID: 2396
	internal enum ParserToken
	{
		// Token: 0x04003B9E RID: 15262
		None = 65536,
		// Token: 0x04003B9F RID: 15263
		Number,
		// Token: 0x04003BA0 RID: 15264
		True,
		// Token: 0x04003BA1 RID: 15265
		False,
		// Token: 0x04003BA2 RID: 15266
		Null,
		// Token: 0x04003BA3 RID: 15267
		CharSeq,
		// Token: 0x04003BA4 RID: 15268
		Char,
		// Token: 0x04003BA5 RID: 15269
		Text,
		// Token: 0x04003BA6 RID: 15270
		Object,
		// Token: 0x04003BA7 RID: 15271
		ObjectPrime,
		// Token: 0x04003BA8 RID: 15272
		Pair,
		// Token: 0x04003BA9 RID: 15273
		PairRest,
		// Token: 0x04003BAA RID: 15274
		Array,
		// Token: 0x04003BAB RID: 15275
		ArrayPrime,
		// Token: 0x04003BAC RID: 15276
		Value,
		// Token: 0x04003BAD RID: 15277
		ValueRest,
		// Token: 0x04003BAE RID: 15278
		String,
		// Token: 0x04003BAF RID: 15279
		End,
		// Token: 0x04003BB0 RID: 15280
		Epsilon
	}
}
