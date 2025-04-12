using System;

namespace emotitron.Compression
{
	// Token: 0x02000C65 RID: 3173
	public static class ArrayPackBytesExt
	{
		// Token: 0x06004F15 RID: 20245 RVA: 0x001B3CE0 File Offset: 0x001B1EE0
		public unsafe static void WritePackedBytes(ulong* uPtr, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits2);
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, num << 3);
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x001B3D18 File Offset: 0x001B1F18
		public static void WritePackedBytes(this ulong[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num << 3);
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x001B3D50 File Offset: 0x001B1F50
		public static void WritePackedBytes(this uint[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num << 3);
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x001B3D88 File Offset: 0x001B1F88
		public static void WritePackedBytes(this byte[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer.Write((ulong)num, ref bitposition, bits2);
			buffer.Write(value, ref bitposition, num << 3);
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x001B3DC0 File Offset: 0x001B1FC0
		public unsafe static ulong ReadPackedBytes(ulong* uPtr, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int bits3 = (int)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits2) << 3;
			return ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits3);
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x001B3DF4 File Offset: 0x001B1FF4
		public static ulong ReadPackedBytes(this ulong[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2) << 3;
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x001B3E28 File Offset: 0x001B2028
		public static ulong ReadPackedBytes(this uint[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2) << 3;
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001B3E5C File Offset: 0x001B205C
		public static ulong ReadPackedBytes(this byte[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2) << 3;
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x001B3E90 File Offset: 0x001B2090
		public unsafe static void WriteSignedPackedBytes(ulong* uPtr, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArrayPackBytesExt.WritePackedBytes(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x001B3EB0 File Offset: 0x001B20B0
		public unsafe static int ReadSignedPackedBytes(ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)ArrayPackBytesExt.ReadPackedBytes(uPtr, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x001B3ED4 File Offset: 0x001B20D4
		public static void WriteSignedPackedBytes(this ulong[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBytes((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x001B3EF4 File Offset: 0x001B20F4
		public static int ReadSignedPackedBytes(this ulong[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBytes(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x001B3F18 File Offset: 0x001B2118
		public static void WriteSignedPackedBytes(this uint[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBytes((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001B3F38 File Offset: 0x001B2138
		public static int ReadSignedPackedBytes(this uint[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBytes(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x001B3F5C File Offset: 0x001B215C
		public static void WriteSignedPackedBytes(this byte[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.WritePackedBytes((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x001B3F7C File Offset: 0x001B217C
		public static int ReadSignedPackedBytes(this byte[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBytes(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x001B3FA0 File Offset: 0x001B21A0
		public static void WriteSignedPackedBytes64(this byte[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.WritePackedBytes(value2, ref bitposition, bits);
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001B3FC0 File Offset: 0x001B21C0
		public static long ReadSignedPackedBytes64(this byte[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.ReadPackedBytes(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}
	}
}
