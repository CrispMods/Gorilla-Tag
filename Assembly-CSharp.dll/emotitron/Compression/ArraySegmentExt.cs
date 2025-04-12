using System;

namespace emotitron.Compression
{
	// Token: 0x02000C66 RID: 3174
	public static class ArraySegmentExt
	{
		// Token: 0x06004F27 RID: 20263 RVA: 0x00062E56 File Offset: 0x00061056
		public static ArraySegment<byte> ExtractArraySegment(byte[] buffer, ref int bitposition)
		{
			return new ArraySegment<byte>(buffer, 0, bitposition + 7 >> 3);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00062E65 File Offset: 0x00061065
		public static ArraySegment<ushort> ExtractArraySegment(ushort[] buffer, ref int bitposition)
		{
			return new ArraySegment<ushort>(buffer, 0, bitposition + 15 >> 4);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00062E75 File Offset: 0x00061075
		public static ArraySegment<uint> ExtractArraySegment(uint[] buffer, ref int bitposition)
		{
			return new ArraySegment<uint>(buffer, 0, bitposition + 31 >> 5);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x00062E85 File Offset: 0x00061085
		public static ArraySegment<ulong> ExtractArraySegment(ulong[] buffer, ref int bitposition)
		{
			return new ArraySegment<ulong>(buffer, 0, bitposition + 63 >> 6);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x001B3FE0 File Offset: 0x001B21E0
		public static void Append(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001B4014 File Offset: 0x001B2214
		public static void Append(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001B4048 File Offset: 0x001B2248
		public static void Append(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x001B407C File Offset: 0x001B227C
		public static void Write(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001B40B0 File Offset: 0x001B22B0
		public static void Write(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001B40E4 File Offset: 0x001B22E4
		public static void Write(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001B4118 File Offset: 0x001B2318
		public static ulong Read(this ArraySegment<byte> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x001B414C File Offset: 0x001B234C
		public static ulong Read(this ArraySegment<uint> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x001B4180 File Offset: 0x001B2380
		public static ulong Read(this ArraySegment<ulong> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001B41B4 File Offset: 0x001B23B4
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x001B41E4 File Offset: 0x001B23E4
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x001B4214 File Offset: 0x001B2414
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x001B4244 File Offset: 0x001B2444
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}
	}
}
