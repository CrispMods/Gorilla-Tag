using System;

namespace LitJson
{
	// Token: 0x0200096E RID: 2414
	public enum JsonToken
	{
		// Token: 0x04003C0E RID: 15374
		None,
		// Token: 0x04003C0F RID: 15375
		ObjectStart,
		// Token: 0x04003C10 RID: 15376
		PropertyName,
		// Token: 0x04003C11 RID: 15377
		ObjectEnd,
		// Token: 0x04003C12 RID: 15378
		ArrayStart,
		// Token: 0x04003C13 RID: 15379
		ArrayEnd,
		// Token: 0x04003C14 RID: 15380
		Int,
		// Token: 0x04003C15 RID: 15381
		Long,
		// Token: 0x04003C16 RID: 15382
		Double,
		// Token: 0x04003C17 RID: 15383
		String,
		// Token: 0x04003C18 RID: 15384
		Boolean,
		// Token: 0x04003C19 RID: 15385
		Null
	}
}
