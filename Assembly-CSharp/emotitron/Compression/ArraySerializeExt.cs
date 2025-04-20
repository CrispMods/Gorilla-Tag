using System;

namespace emotitron.Compression
{
	// Token: 0x02000C95 RID: 3221
	public static class ArraySerializeExt
	{
		// Token: 0x0600508C RID: 20620 RVA: 0x001BC358 File Offset: 0x001BA558
		public static void Zero(this byte[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001BC378 File Offset: 0x001BA578
		public static void Zero(this byte[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x001BC39C File Offset: 0x001BA59C
		public static void Zero(this byte[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x001BC3C0 File Offset: 0x001BA5C0
		public static void Zero(this ushort[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x001BC3E0 File Offset: 0x001BA5E0
		public static void Zero(this ushort[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x001BC404 File Offset: 0x001BA604
		public static void Zero(this ushort[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x001BC428 File Offset: 0x001BA628
		public static void Zero(this uint[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x001BC448 File Offset: 0x001BA648
		public static void Zero(this uint[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x001BC46C File Offset: 0x001BA66C
		public static void Zero(this uint[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001BC490 File Offset: 0x001BA690
		public static void Zero(this ulong[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x001BC4B0 File Offset: 0x001BA6B0
		public static void Zero(this ulong[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x001BC4D4 File Offset: 0x001BA6D4
		public static void Zero(this ulong[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x001BC4F8 File Offset: 0x001BA6F8
		public static void WriteSigned(this byte[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x001BC518 File Offset: 0x001BA718
		public static void WriteSigned(this uint[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x001BC538 File Offset: 0x001BA738
		public static void WriteSigned(this ulong[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x001BC558 File Offset: 0x001BA758
		public static void WriteSigned(this byte[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x001BC578 File Offset: 0x001BA778
		public static void WriteSigned(this uint[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x001BC598 File Offset: 0x001BA798
		public static void WriteSigned(this ulong[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x001BC5B8 File Offset: 0x001BA7B8
		public static int ReadSigned(this byte[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001BC5DC File Offset: 0x001BA7DC
		public static int ReadSigned(this uint[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001BC600 File Offset: 0x001BA800
		public static int ReadSigned(this ulong[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001BC624 File Offset: 0x001BA824
		public static long ReadSigned64(this byte[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x001BC644 File Offset: 0x001BA844
		public static long ReadSigned64(this uint[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x001BC664 File Offset: 0x001BA864
		public static long ReadSigned64(this ulong[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x000648BA File Offset: 0x00062ABA
		public static void WriteFloat(this byte[] buffer, float value, ref int bitposition)
		{
			buffer.Write((ulong)value.uint32, ref bitposition, 32);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x000648D1 File Offset: 0x00062AD1
		public static float ReadFloat(this byte[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 32);
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x001BC684 File Offset: 0x001BA884
		public static void Append(this byte[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int i = bitposition & 7;
			int num = bitposition >> 3;
			ulong num2 = (1UL << i) - 1UL;
			ulong num3 = ((ulong)buffer[num] & num2) | value << i;
			buffer[num] = (byte)num3;
			for (i = 8 - i; i < bits; i += 8)
			{
				num++;
				buffer[num] = (byte)(value >> i);
			}
			bitposition += bits;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x001BC6E0 File Offset: 0x001BA8E0
		public static void Append(this uint[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int i = bitposition & 31;
			int num = bitposition >> 5;
			ulong num2 = (1UL << i) - 1UL;
			ulong num3 = ((ulong)buffer[num] & num2) | value << i;
			buffer[num] = (uint)num3;
			for (i = 32 - i; i < bits; i += 32)
			{
				num++;
				buffer[num] = (uint)(value >> i);
			}
			bitposition += bits;
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x001BC740 File Offset: 0x001BA940
		public static void Append(this uint[] buffer, uint value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 31;
			int num2 = bitposition >> 5;
			ulong num3 = (1UL << num) - 1UL;
			ulong num4 = ((ulong)buffer[num2] & num3) | (ulong)value << num;
			buffer[num2] = (uint)num4;
			buffer[num2 + 1] = (uint)(num4 >> 32);
			bitposition += bits;
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x001BC78C File Offset: 0x001BA98C
		public static void Append(this ulong[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 63;
			int num2 = bitposition >> 6;
			ulong num3 = (1UL << num) - 1UL;
			ulong num4 = (buffer[num2] & num3) | value << num;
			buffer[num2] = num4;
			buffer[num2 + 1] = value >> 64 - num;
			bitposition += bits;
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001BC7D8 File Offset: 0x001BA9D8
		public static void Write(this byte[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 7;
			int num2 = bitposition >> 3;
			int i = num + bits;
			ulong num3 = ulong.MaxValue >> 64 - bits;
			ulong num4 = num3 << num;
			ulong num5 = value << num;
			buffer[num2] = (byte)(((ulong)buffer[num2] & ~num4) | (num5 & num4));
			num = 8 - num;
			for (i -= 8; i > 8; i -= 8)
			{
				num2++;
				num5 = value >> num;
				buffer[num2] = (byte)num5;
				num += 8;
			}
			if (i > 0)
			{
				num2++;
				num4 = num3 >> num;
				num5 = value >> num;
				buffer[num2] = (byte)(((ulong)buffer[num2] & ~num4) | (num5 & num4));
			}
			bitposition += bits;
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001BC87C File Offset: 0x001BAA7C
		public static void Write(this uint[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 31;
			int num2 = bitposition >> 5;
			int i = num + bits;
			ulong num3 = ulong.MaxValue >> 64 - bits;
			ulong num4 = num3 << num;
			ulong num5 = value << num;
			buffer[num2] = (uint)(((ulong)buffer[num2] & ~num4) | (num5 & num4));
			num = 32 - num;
			for (i -= 32; i > 32; i -= 32)
			{
				num2++;
				num4 = num3 >> num;
				num5 = value >> num;
				buffer[num2] = (uint)(((ulong)buffer[num2] & ~num4) | (num5 & num4));
				num += 32;
			}
			bitposition += bits;
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001BC910 File Offset: 0x001BAB10
		public static void Write(this ulong[] buffer, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 63;
			int num2 = bitposition >> 6;
			int i = num + bits;
			ulong num3 = ulong.MaxValue >> 64 - bits;
			ulong num4 = num3 << num;
			ulong num5 = value << num;
			buffer[num2] = ((buffer[num2] & ~num4) | (num5 & num4));
			num = 64 - num;
			for (i -= 64; i > 64; i -= 64)
			{
				num2++;
				num4 = num3 >> num;
				num5 = value >> num;
				buffer[num2] = ((buffer[num2] & ~num4) | (num5 & num4));
				num += 64;
			}
			bitposition += bits;
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x000648E6 File Offset: 0x00062AE6
		public static void WriteBool(this ulong[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x000648F8 File Offset: 0x00062AF8
		public static void WriteBool(this uint[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x0006490A File Offset: 0x00062B0A
		public static void WriteBool(this byte[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x001BC9A0 File Offset: 0x001BABA0
		public static ulong Read(this byte[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int i = bitposition & 7;
			int num = bitposition >> 3;
			ulong num2 = ulong.MaxValue >> 64 - bits;
			ulong num3 = (ulong)buffer[num] >> i;
			for (i = 8 - i; i < bits; i += 8)
			{
				num++;
				num3 |= (ulong)buffer[num] << i;
			}
			bitposition += bits;
			return num3 & num2;
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x001BC9FC File Offset: 0x001BABFC
		public static ulong Read(this uint[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int i = bitposition & 31;
			int num = bitposition >> 5;
			ulong num2 = ulong.MaxValue >> 64 - bits;
			ulong num3 = (ulong)buffer[num] >> i;
			for (i = 32 - i; i < bits; i += 32)
			{
				num++;
				num3 |= (ulong)buffer[num] << i;
			}
			bitposition += bits;
			return num3 & num2;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001BCA58 File Offset: 0x001BAC58
		public static ulong Read(this ulong[] buffer, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int i = bitposition & 63;
			int num = bitposition >> 6;
			ulong num2 = ulong.MaxValue >> 64 - bits;
			ulong num3 = buffer[num] >> i;
			for (i = 64 - i; i < bits; i += 64)
			{
				num++;
				num3 |= buffer[num] << i;
			}
			bitposition += bits;
			return num3 & num2;
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x0006491C File Offset: 0x00062B1C
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this byte[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x00064926 File Offset: 0x00062B26
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this uint[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x00064930 File Offset: 0x00062B30
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this ulong[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x0006493A File Offset: 0x00062B3A
		public static uint ReadUInt32(this byte[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x00064945 File Offset: 0x00062B45
		public static uint ReadUInt32(this uint[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x00064950 File Offset: 0x00062B50
		public static uint ReadUInt32(this ulong[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0006495B File Offset: 0x00062B5B
		public static ushort ReadUInt16(this byte[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x00064966 File Offset: 0x00062B66
		public static ushort ReadUInt16(this uint[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x00064971 File Offset: 0x00062B71
		public static ushort ReadUInt16(this ulong[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0006497C File Offset: 0x00062B7C
		public static byte ReadByte(this byte[] buffer, ref int bitposition, int bits = 8)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x00064987 File Offset: 0x00062B87
		public static byte ReadByte(this uint[] buffer, ref int bitposition, int bits = 32)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x00064992 File Offset: 0x00062B92
		public static byte ReadByte(this ulong[] buffer, ref int bitposition, int bits)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x0006499D File Offset: 0x00062B9D
		public static bool ReadBool(this ulong[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x000649AE File Offset: 0x00062BAE
		public static bool ReadBool(this uint[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x000649BF File Offset: 0x00062BBF
		public static bool ReadBool(this byte[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x000649D0 File Offset: 0x00062BD0
		public static char ReadChar(this ulong[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x000649DC File Offset: 0x00062BDC
		public static char ReadChar(this uint[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x000649E8 File Offset: 0x00062BE8
		public static char ReadChar(this byte[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x001BCAB4 File Offset: 0x001BACB4
		public static void ReadOutSafe(this ulong[] source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = srcStartPos;
			int num2;
			for (int i = bits; i > 0; i -= num2)
			{
				num2 = ((i > 64) ? 64 : i);
				ulong value = source.Read(ref num, num2);
				target.Write(value, ref bitposition, num2);
			}
			bitposition += bits;
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x001BCAFC File Offset: 0x001BACFC
		public static void ReadOutSafe(this ulong[] source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = srcStartPos;
			int num2;
			for (int i = bits; i > 0; i -= num2)
			{
				num2 = ((i > 64) ? 64 : i);
				ulong value = source.Read(ref num, num2);
				target.Write(value, ref bitposition, num2);
			}
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x001BCB3C File Offset: 0x001BAD3C
		public static void ReadOutSafe(this byte[] source, int srcStartPos, ulong[] target, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = srcStartPos;
			int num2;
			for (int i = bits; i > 0; i -= num2)
			{
				num2 = ((i > 8) ? 8 : i);
				ulong value = source.Read(ref num, num2);
				target.Write(value, ref bitposition, num2);
			}
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x001BCB7C File Offset: 0x001BAD7C
		public static void ReadOutSafe(this byte[] source, int srcStartPos, byte[] target, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = srcStartPos;
			int num2;
			for (int i = bits; i > 0; i -= num2)
			{
				num2 = ((i > 8) ? 8 : i);
				ulong value = source.Read(ref num, num2);
				target.Write(value, ref bitposition, num2);
			}
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x001BCBBC File Offset: 0x001BADBC
		public static ulong IndexAsUInt64(this byte[] buffer, int index)
		{
			int num = index << 3;
			return (ulong)buffer[num] | (ulong)buffer[num + 1] << 8 | (ulong)buffer[num + 2] << 16 | (ulong)buffer[num + 3] << 24 | (ulong)buffer[num + 4] << 32 | (ulong)buffer[num + 5] << 40 | (ulong)buffer[num + 6] << 48 | (ulong)buffer[num + 7] << 56;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x001BCC18 File Offset: 0x001BAE18
		public static ulong IndexAsUInt64(this uint[] buffer, int index)
		{
			int num = index << 1;
			return (ulong)buffer[num] | (ulong)buffer[num + 1] << 32;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x001BCC38 File Offset: 0x001BAE38
		public static uint IndexAsUInt32(this byte[] buffer, int index)
		{
			int num = index << 3;
			return (uint)((int)buffer[num] | (int)buffer[num + 1] << 8 | (int)buffer[num + 2] << 16 | (int)buffer[num + 3] << 24);
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001BCC68 File Offset: 0x001BAE68
		public static uint IndexAsUInt32(this ulong[] buffer, int index)
		{
			int num = index >> 1;
			int num2 = (index & 1) << 5;
			return (uint)((byte)(buffer[num] >> num2));
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x001BCC88 File Offset: 0x001BAE88
		public static byte IndexAsUInt8(this ulong[] buffer, int index)
		{
			int num = index >> 3;
			int num2 = (index & 7) << 3;
			return (byte)(buffer[num] >> num2);
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x001BCCA8 File Offset: 0x001BAEA8
		public static byte IndexAsUInt8(this uint[] buffer, int index)
		{
			int num = index >> 3;
			int num2 = (index & 3) << 3;
			return (byte)((ulong)buffer[num] >> num2);
		}

		// Token: 0x040053C7 RID: 21447
		private const string bufferOverrunMsg = "Byte buffer length exceeded by write or read. Dataloss will occur. Likely due to a Read/Write mismatch.";
	}
}
