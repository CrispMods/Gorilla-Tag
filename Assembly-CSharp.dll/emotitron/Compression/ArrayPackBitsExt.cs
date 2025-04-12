using System;

namespace emotitron.Compression
{
	// Token: 0x02000C64 RID: 3172
	public static class ArrayPackBitsExt
	{
		// Token: 0x06004F03 RID: 20227 RVA: 0x001B3A14 File Offset: 0x001B1C14
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

		// Token: 0x06004F04 RID: 20228 RVA: 0x001B3A48 File Offset: 0x001B1C48
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

		// Token: 0x06004F05 RID: 20229 RVA: 0x001B3A7C File Offset: 0x001B1C7C
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

		// Token: 0x06004F06 RID: 20230 RVA: 0x001B3AB0 File Offset: 0x001B1CB0
		public static void WritePackedBits(this byte[] buffer, ulong value, ref int bitposition, int bits)
		{
			int num = value.UsedBitCount();
			int bits2 = bits.UsedBitCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num);
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x001B3AE0 File Offset: 0x001B1CE0
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

		// Token: 0x06004F08 RID: 20232 RVA: 0x001B3B0C File Offset: 0x001B1D0C
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

		// Token: 0x06004F09 RID: 20233 RVA: 0x001B3B38 File Offset: 0x001B1D38
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

		// Token: 0x06004F0A RID: 20234 RVA: 0x001B3B64 File Offset: 0x001B1D64
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

		// Token: 0x06004F0B RID: 20235 RVA: 0x001B3B90 File Offset: 0x001B1D90
		public unsafe static void WriteSignedPackedBits(ulong* uPtr, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArrayPackBitsExt.WritePackedBits(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x001B3BB0 File Offset: 0x001B1DB0
		public unsafe static int ReadSignedPackedBits(ulong* buffer, ref int bitposition, int bits)
		{
			uint num = (uint)ArrayPackBitsExt.ReadPackedBits(buffer, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x001B3BD4 File Offset: 0x001B1DD4
		public static void WriteSignedPackedBits(this ulong[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x001B3BF4 File Offset: 0x001B1DF4
		public static int ReadSignedPackedBits(this ulong[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001B3C18 File Offset: 0x001B1E18
		public static void WriteSignedPackedBits(this uint[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x001B3C38 File Offset: 0x001B1E38
		public static int ReadSignedPackedBits(this uint[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001B3C5C File Offset: 0x001B1E5C
		public static void WriteSignedPackedBits(this byte[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBits((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x001B3C7C File Offset: 0x001B1E7C
		public static int ReadSignedPackedBits(this byte[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x001B3CA0 File Offset: 0x001B1EA0
		public static void WriteSignedPackedBits64(this byte[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.WritePackedBits(value2, ref bitposition, bits);
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x001B3CC0 File Offset: 0x001B1EC0
		public static long ReadSignedPackedBits64(this byte[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.ReadPackedBits(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}
	}
}
