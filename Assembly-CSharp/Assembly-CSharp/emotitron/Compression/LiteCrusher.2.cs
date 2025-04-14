using System;

namespace emotitron.Compression
{
	// Token: 0x02000C70 RID: 3184
	[Serializable]
	public abstract class LiteCrusher<T> : LiteCrusher where T : struct
	{
		// Token: 0x06005038 RID: 20536
		public abstract ulong Encode(T val);

		// Token: 0x06005039 RID: 20537
		public abstract T Decode(uint val);

		// Token: 0x0600503A RID: 20538
		public abstract ulong WriteValue(T val, byte[] buffer, ref int bitposition);

		// Token: 0x0600503B RID: 20539
		public abstract void WriteCValue(uint val, byte[] buffer, ref int bitposition);

		// Token: 0x0600503C RID: 20540
		public abstract T ReadValue(byte[] buffer, ref int bitposition);
	}
}
