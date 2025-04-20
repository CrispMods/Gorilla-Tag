using System;

namespace emotitron.Compression
{
	// Token: 0x02000C92 RID: 3218
	public static class ArrayPackBitsExt
	{
		// Token: 0x06005057 RID: 20567 RVA: 0x001BBAF8 File Offset: 0x001B9CF8
		public unsafe static void WritePackedBits(ulong* uPtr, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = value.UsedBitCount();
			int bits2 = bits.UsedBitCount();
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits2);
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, num);
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x001BBB2C File Offset: 0x001B9D2C
		public static void WritePackedBits(this ulong[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = value.UsedBitCount();
			int bits2 = bits.UsedBitCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num);
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x001BBB60 File Offset: 0x001B9D60
		public static void WritePackedBits(this uint[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = value.UsedBitCount();
			int bits2 = bits.UsedBitCount();
			buffer.Write((ulong)((long)num), ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num);
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001BBB94 File Offset: 0x001B9D94
		public static void WritePackedBits(this byte[] buffer, ulong value, ref int bitposition, int bits)
		{
			int num = value.UsedBitCount();
			int bits2 = bits.UsedBitCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x001BBBC4 File Offset: 0x001B9DC4
		public unsafe static ulong ReadPackedBits(ulong* uPtr, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits2);
			return ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits3);
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x001BBBF0 File Offset: 0x001B9DF0
		public static ulong ReadPackedBits(this ulong[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x001BBC1C File Offset: 0x001B9E1C
		public static ulong ReadPackedBits(this uint[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x001BBC48 File Offset: 0x001B9E48
		public static ulong ReadPackedBits(this byte[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x001BBC74 File Offset: 0x001B9E74
		public unsafe static void WriteSignedPackedBits(ulong* uPtr, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArrayPackBitsExt.WritePackedBits(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x001BBC94 File Offset: 0x001B9E94
		public unsafe static int ReadSignedPackedBits(ulong* buffer, ref int bitposition, int bits)
		{
			uint num = (uint)ArrayPackBitsExt.ReadPackedBits(buffer, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001BBCB8 File Offset: 0x001B9EB8
		public static void WriteSignedPackedBits(this ulong[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x001BBCD8 File Offset: 0x001B9ED8
		public static int ReadSignedPackedBits(this ulong[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001BBCFC File Offset: 0x001B9EFC
		public static void WriteSignedPackedBits(this uint[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001BBD1C File Offset: 0x001B9F1C
		public static int ReadSignedPackedBits(this uint[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001BBD40 File Offset: 0x001B9F40
		public static void WriteSignedPackedBits(this byte[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x001BBD60 File Offset: 0x001B9F60
		public static int ReadSignedPackedBits(this byte[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x001BBD84 File Offset: 0x001B9F84
		public static void WriteSignedPackedBits64(this byte[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.WritePackedBits(value2, ref bitposition, bits);
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x001BBDA4 File Offset: 0x001B9FA4
		public static long ReadSignedPackedBits64(this byte[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.ReadPackedBits(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}
	}
}
