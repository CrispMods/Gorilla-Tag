using System;

namespace emotitron.Compression
{
	// Token: 0x02000C9A RID: 3226
	public static class PrimitivePackBitsExt
	{
		// Token: 0x0600510B RID: 20747 RVA: 0x001BD5D4 File Offset: 0x001BB7D4
		public static ulong WritePackedBits(this ulong buffer, uint value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x001BD608 File Offset: 0x001BB808
		public static uint WritePackedBits(this uint buffer, ushort value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x001BD63C File Offset: 0x001BB83C
		public static ushort WritePackedBits(this ushort buffer, byte value, ref int bitposition, int bits)
		{
			int bits2 = ((uint)bits).UsedBitCount();
			int num = value.UsedBitCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num);
			return buffer;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x001BD670 File Offset: 0x001BB870
		public static ulong ReadPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x001BD698 File Offset: 0x001BB898
		public static ulong ReadPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x001BD6C0 File Offset: 0x001BB8C0
		public static ulong ReadPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			int bits2 = bits.UsedBitCount();
			int bits3 = (int)buffer.Read(ref bitposition, bits2);
			return (ulong)buffer.Read(ref bitposition, bits3);
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x001BD6E8 File Offset: 0x001BB8E8
		public static ulong WriteSignedPackedBits(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint value2 = (uint)(value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits(value2, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x001BD70C File Offset: 0x001BB90C
		public static uint WriteSignedPackedBits(this uint buffer, short value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((ushort)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x001BD730 File Offset: 0x001BB930
		public static ushort WriteSignedPackedBits(this ushort buffer, sbyte value, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			buffer = buffer.WritePackedBits((byte)num, ref bitposition, bits);
			return buffer;
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x001BD754 File Offset: 0x001BB954
		public static int ReadSignedPackedBits(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x001BD778 File Offset: 0x001BB978
		public static short ReadSignedPackedBits(this uint buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (short)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x001BD79C File Offset: 0x001BB99C
		public static sbyte ReadSignedPackedBits(this ushort buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBits(ref bitposition, bits);
			return (sbyte)((int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U)))));
		}
	}
}
