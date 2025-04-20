using System;

namespace emotitron.Compression
{
	// Token: 0x02000CA3 RID: 3235
	public static class ZigZagExt
	{
		// Token: 0x060051A4 RID: 20900 RVA: 0x00064F3A File Offset: 0x0006313A
		public static ulong ZigZag(this long s)
		{
			return (ulong)(s << 1 ^ s >> 63);
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x00064F44 File Offset: 0x00063144
		public static long UnZigZag(this ulong u)
		{
			return (long)(u >> 1 ^ -(long)(u & 1UL));
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x00064F4F File Offset: 0x0006314F
		public static uint ZigZag(this int s)
		{
			return (uint)(s << 1 ^ s >> 31);
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x00064F59 File Offset: 0x00063159
		public static int UnZigZag(this uint u)
		{
			return (int)((ulong)(u >> 1) ^ (ulong)((long)(-(long)(u & 1U))));
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x00064F66 File Offset: 0x00063166
		public static ushort ZigZag(this short s)
		{
			return (ushort)((int)s << 1 ^ s >> 15);
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x00064F71 File Offset: 0x00063171
		public static short UnZigZag(this ushort u)
		{
			return (short)(u >> 1 ^ (int)(-(int)((short)(u & 1))));
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x00064F7D File Offset: 0x0006317D
		public static byte ZigZag(this sbyte s)
		{
			return (byte)((int)s << 1 ^ s >> 7);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x00064F87 File Offset: 0x00063187
		public static sbyte UnZigZag(this byte u)
		{
			return (sbyte)(u >> 1 ^ (int)(-(int)((sbyte)(u & 1))));
		}
	}
}
