using System;

namespace emotitron.Compression
{
	// Token: 0x02000C94 RID: 3220
	public static class ArraySegmentExt
	{
		// Token: 0x0600507B RID: 20603 RVA: 0x0006487B File Offset: 0x00062A7B
		public static ArraySegment<byte> ExtractArraySegment(byte[] buffer, ref int bitposition)
		{
			return new ArraySegment<byte>(buffer, 0, bitposition + 7 >> 3);
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0006488A File Offset: 0x00062A8A
		public static ArraySegment<ushort> ExtractArraySegment(ushort[] buffer, ref int bitposition)
		{
			return new ArraySegment<ushort>(buffer, 0, bitposition + 15 >> 4);
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x0006489A File Offset: 0x00062A9A
		public static ArraySegment<uint> ExtractArraySegment(uint[] buffer, ref int bitposition)
		{
			return new ArraySegment<uint>(buffer, 0, bitposition + 31 >> 5);
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x000648AA File Offset: 0x00062AAA
		public static ArraySegment<ulong> ExtractArraySegment(ulong[] buffer, ref int bitposition)
		{
			return new ArraySegment<ulong>(buffer, 0, bitposition + 63 >> 6);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001BC0C4 File Offset: 0x001BA2C4
		public static void Append(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001BC0F8 File Offset: 0x001BA2F8
		public static void Append(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x001BC12C File Offset: 0x001BA32C
		public static void Append(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001BC160 File Offset: 0x001BA360
		public static void Write(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001BC194 File Offset: 0x001BA394
		public static void Write(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x001BC1C8 File Offset: 0x001BA3C8
		public static void Write(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001BC1FC File Offset: 0x001BA3FC
		public static ulong Read(this ArraySegment<byte> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001BC230 File Offset: 0x001BA430
		public static ulong Read(this ArraySegment<uint> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001BC264 File Offset: 0x001BA464
		public static ulong Read(this ArraySegment<ulong> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001BC298 File Offset: 0x001BA498
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x001BC2C8 File Offset: 0x001BA4C8
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x001BC2F8 File Offset: 0x001BA4F8
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x001BC328 File Offset: 0x001BA528
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}
	}
}
