using System;

namespace emotitron.Compression
{
	// Token: 0x02000C9E RID: 3230
	[Serializable]
	public abstract class LiteCrusher<T> : LiteCrusher where T : struct
	{
		// Token: 0x0600518C RID: 20876
		public abstract ulong Encode(T val);

		// Token: 0x0600518D RID: 20877
		public abstract T Decode(uint val);

		// Token: 0x0600518E RID: 20878
		public abstract ulong WriteValue(T val, byte[] buffer, ref int bitposition);

		// Token: 0x0600518F RID: 20879
		public abstract void WriteCValue(uint val, byte[] buffer, ref int bitposition);

		// Token: 0x06005190 RID: 20880
		public abstract T ReadValue(byte[] buffer, ref int bitposition);
	}
}
