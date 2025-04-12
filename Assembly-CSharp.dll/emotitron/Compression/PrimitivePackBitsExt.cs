using System;

namespace emotitron.Compression
{
	// Token: 0x02000C6C RID: 3180
	public static class PrimitivePackBitsExt
	{
		// Token: 0x06004FB7 RID: 20407 RVA: 0x001B54F0 File Offset: 0x001B36F0
		public static ulong WritePackedBits(this ulong buffer, uint value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001B5524 File Offset: 0x001B3724
		public static uint WritePackedBits(this uint buffer, ushort value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x001B5558 File Offset: 0x001B3758
		public static ushort WritePackedBits(this ushort buffer, byte value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x001B558C File Offset: 0x001B378C
		public static ulong ReadPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x001B55B4 File Offset: 0x001B37B4
		public static ulong ReadPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x001B55DC File Offset: 0x001B37DC
		public static ulong ReadPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x001B5604 File Offset: 0x001B3804
		public static ulong WriteSignedPackedBits(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint value2 = (uint)(value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits(value2, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x001B5628 File Offset: 0x001B3828
		public static uint WriteSignedPackedBits(this uint buffer, short value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((ushort)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001B564C File Offset: 0x001B384C
		public static ushort WriteSignedPackedBits(this ushort buffer, sbyte value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((byte)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x001B5670 File Offset: 0x001B3870
		public static int ReadSignedPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001B5694 File Offset: 0x001B3894
		public static short ReadSignedPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (short)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001B56B8 File Offset: 0x001B38B8
		public static sbyte ReadSignedPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (sbyte)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}
	}
}
