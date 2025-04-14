using System;

namespace emotitron.Compression
{
	// Token: 0x02000C72 RID: 3186
	public static class ZigZagExt
	{
		// Token: 0x06005044 RID: 20548 RVA: 0x00186D1D File Offset: 0x00184F1D
		public static ulong ZigZag(this long s)
		{
			return (ulong)(s << 1 ^ s >> 63);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x00186D27 File Offset: 0x00184F27
		public static long UnZigZag(this ulong u)
		{
			return (long)(u >> 1 ^ -(long)(u & 1UL));
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x00186D32 File Offset: 0x00184F32
		public static uint ZigZag(this int s)
		{
			return (uint)(s << 1 ^ s >> 31);
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x00186D3C File Offset: 0x00184F3C
		public static int UnZigZag(this uint u)
		{
			return (int)((ulong)(u >> 1) ^ (ulong)((long)(-(long)(u & 1U))));
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x00186D49 File Offset: 0x00184F49
		public static ushort ZigZag(this short s)
		{
			return (ushort)((int)s << 1 ^ s >> 15);
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x00186D54 File Offset: 0x00184F54
		public static short UnZigZag(this ushort u)
		{
			return (short)(u >> 1 ^ (int)(-(int)((short)(u & 1))));
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x00186D60 File Offset: 0x00184F60
		public static byte ZigZag(this sbyte s)
		{
			return (byte)((int)s << 1 ^ s >> 7);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x00186D6A File Offset: 0x00184F6A
		public static sbyte UnZigZag(this byte u)
		{
			return (sbyte)(u >> 1 ^ (int)(-(int)((sbyte)(u & 1))));
		}
	}
}
