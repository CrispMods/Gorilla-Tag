using System;

namespace emotitron.Compression
{
	// Token: 0x02000C67 RID: 3175
	public static class ArraySerializeExt
	{
		// Token: 0x06004F38 RID: 20280 RVA: 0x00184B78 File Offset: 0x00182D78
		public static void Zero(this byte[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x00184B98 File Offset: 0x00182D98
		public static void Zero(this byte[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x00184BBC File Offset: 0x00182DBC
		public static void Zero(this byte[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x00184BE0 File Offset: 0x00182DE0
		public static void Zero(this ushort[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x00184C00 File Offset: 0x00182E00
		public static void Zero(this ushort[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x00184C24 File Offset: 0x00182E24
		public static void Zero(this ushort[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00184C48 File Offset: 0x00182E48
		public static void Zero(this uint[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x00184C68 File Offset: 0x00182E68
		public static void Zero(this uint[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00184C8C File Offset: 0x00182E8C
		public static void Zero(this uint[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0U;
			}
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00184CB0 File Offset: 0x00182EB0
		public static void Zero(this ulong[] buffer, int startByte, int endByte)
		{
			for (int i = startByte; i <= endByte; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x00184CD0 File Offset: 0x00182ED0
		public static void Zero(this ulong[] buffer, int startByte)
		{
			int num = buffer.Length;
			for (int i = startByte; i < num; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00184CF4 File Offset: 0x00182EF4
		public static void Zero(this ulong[] buffer)
		{
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = 0UL;
			}
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x00184D18 File Offset: 0x00182F18
		public static void WriteSigned(this byte[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00184D38 File Offset: 0x00182F38
		public static void WriteSigned(this uint[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00184D58 File Offset: 0x00182F58
		public static void WriteSigned(this ulong[] buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			buffer.Write((ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00184D78 File Offset: 0x00182F78
		public static void WriteSigned(this byte[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00184D98 File Offset: 0x00182F98
		public static void WriteSigned(this uint[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x00184DB8 File Offset: 0x00182FB8
		public static void WriteSigned(this ulong[] buffer, long value, ref int bitposition, int bits)
		{
			ulong value2 = (ulong)(value << 1 ^ value >> 63);
			buffer.Write(value2, ref bitposition, bits);
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x00184DD8 File Offset: 0x00182FD8
		public static int ReadSigned(this byte[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00184DFC File Offset: 0x00182FFC
		public static int ReadSigned(this uint[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00184E20 File Offset: 0x00183020
		public static int ReadSigned(this ulong[] buffer, ref int bitposition, int bits)
		{
			uint num = (uint)buffer.Read(ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00184E44 File Offset: 0x00183044
		public static long ReadSigned64(this byte[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00184E64 File Offset: 0x00183064
		public static long ReadSigned64(this uint[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00184E84 File Offset: 0x00183084
		public static long ReadSigned64(this ulong[] buffer, ref int bitposition, int bits)
		{
			ulong num = buffer.Read(ref bitposition, bits);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00184EA3 File Offset: 0x001830A3
		public static void WriteFloat(this byte[] buffer, float value, ref int bitposition)
		{
			buffer.Write((ulong)value.uint32, ref bitposition, 32);
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x00184EBA File Offset: 0x001830BA
		public static float ReadFloat(this byte[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 32);
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x00184ED0 File Offset: 0x001830D0
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

		// Token: 0x06004F53 RID: 20307 RVA: 0x00184F2C File Offset: 0x0018312C
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

		// Token: 0x06004F54 RID: 20308 RVA: 0x00184F8C File Offset: 0x0018318C
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

		// Token: 0x06004F55 RID: 20309 RVA: 0x00184FD8 File Offset: 0x001831D8
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

		// Token: 0x06004F56 RID: 20310 RVA: 0x00185024 File Offset: 0x00183224
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

		// Token: 0x06004F57 RID: 20311 RVA: 0x001850C8 File Offset: 0x001832C8
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

		// Token: 0x06004F58 RID: 20312 RVA: 0x0018515C File Offset: 0x0018335C
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

		// Token: 0x06004F59 RID: 20313 RVA: 0x001851EC File Offset: 0x001833EC
		public static void WriteBool(this ulong[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x001851FE File Offset: 0x001833FE
		public static void WriteBool(this uint[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00185210 File Offset: 0x00183410
		public static void WriteBool(this byte[] buffer, bool b, ref int bitposition)
		{
			buffer.Write((ulong)(b ? 1L : 0L), ref bitposition, 1);
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00185224 File Offset: 0x00183424
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

		// Token: 0x06004F5D RID: 20317 RVA: 0x00185280 File Offset: 0x00183480
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

		// Token: 0x06004F5E RID: 20318 RVA: 0x001852DC File Offset: 0x001834DC
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

		// Token: 0x06004F5F RID: 20319 RVA: 0x00185336 File Offset: 0x00183536
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this byte[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x00185340 File Offset: 0x00183540
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this uint[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x0018534A File Offset: 0x0018354A
		[Obsolete("Just use Read(), it return a ulong already.")]
		public static ulong ReadUInt64(this ulong[] buffer, ref int bitposition, int bits = 64)
		{
			return buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x00185354 File Offset: 0x00183554
		public static uint ReadUInt32(this byte[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0018535F File Offset: 0x0018355F
		public static uint ReadUInt32(this uint[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x0018536A File Offset: 0x0018356A
		public static uint ReadUInt32(this ulong[] buffer, ref int bitposition, int bits = 32)
		{
			return (uint)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00185375 File Offset: 0x00183575
		public static ushort ReadUInt16(this byte[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x00185380 File Offset: 0x00183580
		public static ushort ReadUInt16(this uint[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x0018538B File Offset: 0x0018358B
		public static ushort ReadUInt16(this ulong[] buffer, ref int bitposition, int bits = 16)
		{
			return (ushort)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00185396 File Offset: 0x00183596
		public static byte ReadByte(this byte[] buffer, ref int bitposition, int bits = 8)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x001853A1 File Offset: 0x001835A1
		public static byte ReadByte(this uint[] buffer, ref int bitposition, int bits = 32)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x001853AC File Offset: 0x001835AC
		public static byte ReadByte(this ulong[] buffer, ref int bitposition, int bits)
		{
			return (byte)buffer.Read(ref bitposition, bits);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x001853B7 File Offset: 0x001835B7
		public static bool ReadBool(this ulong[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001853C8 File Offset: 0x001835C8
		public static bool ReadBool(this uint[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x001853D9 File Offset: 0x001835D9
		public static bool ReadBool(this byte[] buffer, ref int bitposition)
		{
			return buffer.Read(ref bitposition, 1) == 1UL;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001853EA File Offset: 0x001835EA
		public static char ReadChar(this ulong[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x001853F6 File Offset: 0x001835F6
		public static char ReadChar(this uint[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00185402 File Offset: 0x00183602
		public static char ReadChar(this byte[] buffer, ref int bitposition)
		{
			return (char)buffer.Read(ref bitposition, 16);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x00185410 File Offset: 0x00183610
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

		// Token: 0x06004F72 RID: 20338 RVA: 0x00185458 File Offset: 0x00183658
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

		// Token: 0x06004F73 RID: 20339 RVA: 0x00185498 File Offset: 0x00183698
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

		// Token: 0x06004F74 RID: 20340 RVA: 0x001854D8 File Offset: 0x001836D8
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

		// Token: 0x06004F75 RID: 20341 RVA: 0x00185518 File Offset: 0x00183718
		public static ulong IndexAsUInt64(this byte[] buffer, int index)
		{
			int num = index << 3;
			return (ulong)buffer[num] | (ulong)buffer[num + 1] << 8 | (ulong)buffer[num + 2] << 16 | (ulong)buffer[num + 3] << 24 | (ulong)buffer[num + 4] << 32 | (ulong)buffer[num + 5] << 40 | (ulong)buffer[num + 6] << 48 | (ulong)buffer[num + 7] << 56;
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00185574 File Offset: 0x00183774
		public static ulong IndexAsUInt64(this uint[] buffer, int index)
		{
			int num = index << 1;
			return (ulong)buffer[num] | (ulong)buffer[num + 1] << 32;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00185594 File Offset: 0x00183794
		public static uint IndexAsUInt32(this byte[] buffer, int index)
		{
			int num = index << 3;
			return (uint)((int)buffer[num] | (int)buffer[num + 1] << 8 | (int)buffer[num + 2] << 16 | (int)buffer[num + 3] << 24);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001855C4 File Offset: 0x001837C4
		public static uint IndexAsUInt32(this ulong[] buffer, int index)
		{
			int num = index >> 1;
			int num2 = (index & 1) << 5;
			return (uint)((byte)(buffer[num] >> num2));
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001855E4 File Offset: 0x001837E4
		public static byte IndexAsUInt8(this ulong[] buffer, int index)
		{
			int num = index >> 3;
			int num2 = (index & 7) << 3;
			return (byte)(buffer[num] >> num2);
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x00185604 File Offset: 0x00183804
		public static byte IndexAsUInt8(this uint[] buffer, int index)
		{
			int num = index >> 3;
			int num2 = (index & 3) << 3;
			return (byte)((ulong)buffer[num] >> num2);
		}

		// Token: 0x040052CD RID: 21197
		private const string bufferOverrunMsg = "Byte buffer length exceeded by write or read. Dataloss will occur. Likely due to a Read/Write mismatch.";
	}
}
