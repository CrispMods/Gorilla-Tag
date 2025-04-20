using System;

namespace emotitron.Compression
{
	// Token: 0x02000C9B RID: 3227
	public static class PrimitivePackBytesExt
	{
		// Token: 0x06005117 RID: 20759 RVA: 0x001BD7C0 File Offset: 0x001BB9C0
		public static ulong WritePackedBytes(this ulong buffer, ulong value, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write(value, ref bitposition, num << 3);
			return buffer;
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001BD7FC File Offset: 0x001BB9FC
		public static uint WritePackedBytes(this uint buffer, uint value, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num << 3);
			return buffer;
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001BD838 File Offset: 0x001BBA38
		public static void InjectPackedBytes(this ulong value, ref ulong buffer, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write(value, ref bitposition, num << 3);
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001BD874 File Offset: 0x001BBA74
		public static void InjectPackedBytes(this uint value, ref uint buffer, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = value.UsedByteCount();
			buffer = buffer.Write((ulong)num, ref bitposition, bits2);
			buffer = buffer.Write((ulong)value, ref bitposition, num << 3);
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001BD8B0 File Offset: 0x001BBAB0
		public static ulong ReadPackedBytes(this ulong buffer, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, num << 3);
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x001BD8DC File Offset: 0x001BBADC
		public static uint ReadPackedBytes(this uint buffer, ref int bitposition, int bits)
		{
			int bits2 = (bits + 7 >> 3).UsedBitCount();
			int num = (int)buffer.Read(ref bitposition, bits2);
			return buffer.Read(ref bitposition, num << 3);
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x001BD908 File Offset: 0x001BBB08
		public static ulong WriteSignedPackedBytes(this ulong buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			return buffer.WritePackedBytes((ulong)num, ref bitposition, bits);
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x001BD928 File Offset: 0x001BBB28
		public static int ReadSignedPackedBytes(this ulong buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.ReadPackedBytes(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}
	}
}
