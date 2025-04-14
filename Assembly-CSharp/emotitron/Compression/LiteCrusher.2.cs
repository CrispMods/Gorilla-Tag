using System;

namespace emotitron.Compression
{
	// Token: 0x02000C6D RID: 3181
	[Serializable]
	public abstract class LiteCrusher<T> : LiteCrusher where T : struct
	{
		// Token: 0x0600502C RID: 20524
		public abstract ulong Encode(T val);

		// Token: 0x0600502D RID: 20525
		public abstract T Decode(uint val);

		// Token: 0x0600502E RID: 20526
		public abstract ulong WriteValue(T val, byte[] buffer, ref int bitposition);

		// Token: 0x0600502F RID: 20527
		public abstract void WriteCValue(uint val, byte[] buffer, ref int bitposition);

		// Token: 0x06005030 RID: 20528
		public abstract T ReadValue(byte[] buffer, ref int bitposition);
	}
}
