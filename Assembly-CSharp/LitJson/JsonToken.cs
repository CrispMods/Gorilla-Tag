using System;

namespace LitJson
{
	// Token: 0x02000951 RID: 2385
	public enum JsonToken
	{
		// Token: 0x04003B49 RID: 15177
		None,
		// Token: 0x04003B4A RID: 15178
		ObjectStart,
		// Token: 0x04003B4B RID: 15179
		PropertyName,
		// Token: 0x04003B4C RID: 15180
		ObjectEnd,
		// Token: 0x04003B4D RID: 15181
		ArrayStart,
		// Token: 0x04003B4E RID: 15182
		ArrayEnd,
		// Token: 0x04003B4F RID: 15183
		Int,
		// Token: 0x04003B50 RID: 15184
		Long,
		// Token: 0x04003B51 RID: 15185
		Double,
		// Token: 0x04003B52 RID: 15186
		String,
		// Token: 0x04003B53 RID: 15187
		Boolean,
		// Token: 0x04003B54 RID: 15188
		Null
	}
}
