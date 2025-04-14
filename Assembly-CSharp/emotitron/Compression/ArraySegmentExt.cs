using System;

namespace emotitron.Compression
{
	// Token: 0x02000C63 RID: 3171
	public static class ArraySegmentExt
	{
		// Token: 0x06004F1B RID: 20251 RVA: 0x001842DB File Offset: 0x001824DB
		public static ArraySegment<byte> ExtractArraySegment(byte[] buffer, ref int bitposition)
		{
			return new ArraySegment<byte>(buffer, 0, bitposition + 7 >> 3);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x001842EA File Offset: 0x001824EA
		public static ArraySegment<ushort> ExtractArraySegment(ushort[] buffer, ref int bitposition)
		{
			return new ArraySegment<ushort>(buffer, 0, bitposition + 15 >> 4);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x001842FA File Offset: 0x001824FA
		public static ArraySegment<uint> ExtractArraySegment(uint[] buffer, ref int bitposition)
		{
			return new ArraySegment<uint>(buffer, 0, bitposition + 31 >> 5);
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x0018430A File Offset: 0x0018250A
		public static ArraySegment<ulong> ExtractArraySegment(ulong[] buffer, ref int bitposition)
		{
			return new ArraySegment<ulong>(buffer, 0, bitposition + 63 >> 6);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x0018431C File Offset: 0x0018251C
		public static void Append(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00184350 File Offset: 0x00182550
		public static void Append(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x00184384 File Offset: 0x00182584
		public static void Append(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Append(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x001843B8 File Offset: 0x001825B8
		public static void Write(this ArraySegment<byte> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x001843EC File Offset: 0x001825EC
		public static void Write(this ArraySegment<uint> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x00184420 File Offset: 0x00182620
		public static void Write(this ArraySegment<ulong> buffer, ulong value, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			buffer.Array.Write(value, ref bitposition, bits);
			bitposition -= num;
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00184454 File Offset: 0x00182654
		public static ulong Read(this ArraySegment<byte> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 3;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x00184488 File Offset: 0x00182688
		public static ulong Read(this ArraySegment<uint> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 5;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x001844BC File Offset: 0x001826BC
		public static ulong Read(this ArraySegment<ulong> buffer, ref int bitposition, int bits)
		{
			int num = buffer.Offset << 6;
			bitposition += num;
			ulong result = buffer.Array.Read(ref bitposition, bits);
			bitposition -= num;
			return result;
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x001844F0 File Offset: 0x001826F0
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00184520 File Offset: 0x00182720
		public static void ReadOutSafe(this ArraySegment<byte> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 3;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x00184550 File Offset: 0x00182750
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00184580 File Offset: 0x00182780
		public static void ReadOutSafe(this ArraySegment<ulong> source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			int num = source.Offset << 6;
			srcStartPos += num;
			source.Array.ReadOutSafe(srcStartPos, target, ref bitposition, bits);
		}
	}
}
