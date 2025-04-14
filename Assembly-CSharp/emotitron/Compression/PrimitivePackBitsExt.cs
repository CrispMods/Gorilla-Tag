using System;

namespace emotitron.Compression
{
	// Token: 0x02000C69 RID: 3177
	public static class PrimitivePackBitsExt
	{
		// Token: 0x06004FAB RID: 20395 RVA: 0x00185B70 File Offset: 0x00183D70
		public static ulong WritePackedBits(this ulong buffer, uint value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x00185BA4 File Offset: 0x00183DA4
		public static uint WritePackedBits(this uint buffer, ushort value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x00185BD8 File Offset: 0x00183DD8
		public static ushort WritePackedBits(this ushort buffer, byte value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00185C0C File Offset: 0x00183E0C
		public static ulong ReadPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x00185C34 File Offset: 0x00183E34
		public static ulong ReadPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00185C5C File Offset: 0x00183E5C
		public static ulong ReadPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00185C84 File Offset: 0x00183E84
		public static ulong WriteSignedPackedBits(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint value2 = (uint)(value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits(value2, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x00185CA8 File Offset: 0x00183EA8
		public static uint WriteSignedPackedBits(this uint buffer, short value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((ushort)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00185CCC File Offset: 0x00183ECC
		public static ushort WriteSignedPackedBits(this ushort buffer, sbyte value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((byte)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x00185CF0 File Offset: 0x00183EF0
		public static int ReadSignedPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x00185D14 File Offset: 0x00183F14
		public static short ReadSignedPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (short)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x00185D38 File Offset: 0x00183F38
		public static sbyte ReadSignedPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (sbyte)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}
	}
}
