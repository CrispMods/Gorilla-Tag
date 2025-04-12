using System;

namespace emotitron.Compression
{
	// Token: 0x02000C75 RID: 3189
	public static class ZigZagExt
	{
		// Token: 0x06005050 RID: 20560 RVA: 0x00063515 File Offset: 0x00061715
		public static ulong ZigZag(this long s)
		{
			return (ulong)(s << 1 ^ s >> 63);
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x0006351F File Offset: 0x0006171F
		public static long UnZigZag(this ulong u)
		{
			return (long)(u >> 1 ^ -(long)(u & 1UL));
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0006352A File Offset: 0x0006172A
		public static uint ZigZag(this int s)
		{
			return (uint)(s << 1 ^ s >> 31);
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x00063534 File Offset: 0x00061734
		public static int UnZigZag(this uint u)
		{
			return (int)((ulong)(u >> 1) ^ (ulong)((long)(-(long)(u & 1U))));
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x00063541 File Offset: 0x00061741
		public static ushort ZigZag(this short s)
		{
			return (ushort)((int)s << 1 ^ s >> 15);
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0006354C File Offset: 0x0006174C
		public static short UnZigZag(this ushort u)
		{
			return (short)(u >> 1 ^ (int)(-(int)((short)(u & 1))));
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x00063558 File Offset: 0x00061758
		public static byte ZigZag(this sbyte s)
		{
			return (byte)((int)s << 1 ^ s >> 7);
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x00063562 File Offset: 0x00061762
		public static sbyte UnZigZag(this byte u)
		{
			return (sbyte)(u >> 1 ^ (int)(-(int)((sbyte)(u & 1))));
		}
	}
}
