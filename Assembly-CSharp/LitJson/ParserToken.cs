using System;

namespace LitJson
{
	// Token: 0x02000976 RID: 2422
	internal enum ParserToken
	{
		// Token: 0x04003C51 RID: 15441
		None = 65536,
		// Token: 0x04003C52 RID: 15442
		Number,
		// Token: 0x04003C53 RID: 15443
		True,
		// Token: 0x04003C54 RID: 15444
		False,
		// Token: 0x04003C55 RID: 15445
		Null,
		// Token: 0x04003C56 RID: 15446
		CharSeq,
		// Token: 0x04003C57 RID: 15447
		Char,
		// Token: 0x04003C58 RID: 15448
		Text,
		// Token: 0x04003C59 RID: 15449
		Object,
		// Token: 0x04003C5A RID: 15450
		ObjectPrime,
		// Token: 0x04003C5B RID: 15451
		Pair,
		// Token: 0x04003C5C RID: 15452
		PairRest,
		// Token: 0x04003C5D RID: 15453
		Array,
		// Token: 0x04003C5E RID: 15454
		ArrayPrime,
		// Token: 0x04003C5F RID: 15455
		Value,
		// Token: 0x04003C60 RID: 15456
		ValueRest,
		// Token: 0x04003C61 RID: 15457
		String,
		// Token: 0x04003C62 RID: 15458
		End,
		// Token: 0x04003C63 RID: 15459
		Epsilon
	}
}
