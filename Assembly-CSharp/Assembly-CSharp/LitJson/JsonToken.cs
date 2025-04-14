using System;

namespace LitJson
{
	// Token: 0x02000954 RID: 2388
	public enum JsonToken
	{
		// Token: 0x04003B5B RID: 15195
		None,
		// Token: 0x04003B5C RID: 15196
		ObjectStart,
		// Token: 0x04003B5D RID: 15197
		PropertyName,
		// Token: 0x04003B5E RID: 15198
		ObjectEnd,
		// Token: 0x04003B5F RID: 15199
		ArrayStart,
		// Token: 0x04003B60 RID: 15200
		ArrayEnd,
		// Token: 0x04003B61 RID: 15201
		Int,
		// Token: 0x04003B62 RID: 15202
		Long,
		// Token: 0x04003B63 RID: 15203
		Double,
		// Token: 0x04003B64 RID: 15204
		String,
		// Token: 0x04003B65 RID: 15205
		Boolean,
		// Token: 0x04003B66 RID: 15206
		Null
	}
}
