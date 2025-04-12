using System;

namespace emotitron.Compression
{
	// Token: 0x02000C68 RID: 3176
	public static class ArraySerializeUnsafe
	{
		// Token: 0x06004F7B RID: 20347 RVA: 0x001B4BE8 File Offset: 0x001B2DE8
		public unsafe static void WriteSigned(ulong* buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(buffer, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x001B4C08 File Offset: 0x001B2E08
		public unsafe static void AppendSigned(ulong* buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(buffer, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001B4C28 File Offset: 0x001B2E28
		public unsafe static void AddSigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001B4C28 File Offset: 0x001B2E28
		public unsafe static void AddSigned(this short value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x001B4C28 File Offset: 0x001B2E28
		public unsafe static void AddSigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001B4C48 File Offset: 0x001B2E48
		public unsafe static void InjectSigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001B4C48 File Offset: 0x001B2E48
		public unsafe static void InjectSigned(this short value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001B4C48 File Offset: 0x001B2E48
		public unsafe static void InjectSigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x001B4C68 File Offset: 0x001B2E68
		public unsafe static void PokeSigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001B4C68 File Offset: 0x001B2E68
		public unsafe static void PokeSigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x001B4C68 File Offset: 0x001B2E68
		public unsafe static void PokeSigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x001B4C8C File Offset: 0x001B2E8C
		public unsafe static int ReadSigned(ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x001B4CB0 File Offset: 0x001B2EB0
		public unsafe static int PeekSigned(ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x001B4CD4 File Offset: 0x001B2ED4
		public unsafe static void Append(ulong* uPtr, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 63;
			int num2 = bitposition >> 6;
			ulong num3 = (1UL << num) - 1UL;
			ulong num4 = (uPtr[num2] & num3) | value << num;
			uPtr[num2] = num4;
			uPtr[num2 + 1] = num4 >> 64 - num;
			bitposition += bits;
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x001B4D2C File Offset: 0x001B2F2C
		public unsafe static void Write(ulong* uPtr, ulong value, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = bitposition & 63;
			int num2 = bitposition >> 6;
			ulong num3 = ulong.MaxValue >> 64 - bits;
			ulong num4 = num3 << num;
			ulong num5 = value << num;
			uPtr[num2] = ((uPtr[num2] & ~num4) | (num5 & num4));
			num = 64 - num;
			if (num < bits)
			{
				num4 = num3 >> num;
				num5 = value >> num;
				num2++;
				uPtr[num2] = ((uPtr[num2] & ~num4) | (num5 & num4));
			}
			bitposition += bits;
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x001B4DB0 File Offset: 0x001B2FB0
		public unsafe static ulong Read(ulong* uPtr, ref int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int i = bitposition & 63;
			int num = bitposition >> 6;
			ulong num2 = ulong.MaxValue >> 64 - bits;
			ulong num3 = uPtr[num] >> i;
			for (i = 64 - i; i < bits; i += 64)
			{
				num++;
				num3 |= uPtr[num] << i;
			}
			bitposition += bits;
			return num3 & num2;
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x001B4E14 File Offset: 0x001B3014
		public unsafe static ulong Read(ulong* uPtr, int bitposition, int bits)
		{
			if (bits == 0)
			{
				return 0UL;
			}
			int i = bitposition & 63;
			int num = bitposition >> 6;
			ulong num2 = ulong.MaxValue >> 64 - bits;
			ulong num3 = uPtr[num] >> i;
			for (i = 64 - i; i < bits; i += 64)
			{
				num++;
				num3 |= uPtr[num] << i;
			}
			bitposition += bits;
			return num3 & num2;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00062FCF File Offset: 0x000611CF
		public unsafe static void Add(this ulong value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x00062FDB File Offset: 0x000611DB
		public unsafe static void Add(this uint value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x00062FDB File Offset: 0x000611DB
		public unsafe static void Add(this ushort value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x00062FDB File Offset: 0x000611DB
		public unsafe static void Add(this byte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00062FCF File Offset: 0x000611CF
		public unsafe static void AddUnsigned(this long value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00062FE8 File Offset: 0x000611E8
		public unsafe static void AddUnsigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00062FE8 File Offset: 0x000611E8
		public unsafe static void AddUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00062FE8 File Offset: 0x000611E8
		public unsafe static void AddUnsigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00062FF5 File Offset: 0x000611F5
		public unsafe static void Inject(this ulong value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00063000 File Offset: 0x00061200
		public unsafe static void Inject(this uint value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00063000 File Offset: 0x00061200
		public unsafe static void Inject(this ushort value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x00063000 File Offset: 0x00061200
		public unsafe static void Inject(this byte value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00062FF5 File Offset: 0x000611F5
		public unsafe static void InjectUnsigned(this long value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x0006300C File Offset: 0x0006120C
		public unsafe static void InjectUnsigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00063018 File Offset: 0x00061218
		public unsafe static void InjectUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x0006300C File Offset: 0x0006120C
		public unsafe static void InjectUnsigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x00063025 File Offset: 0x00061225
		public unsafe static void Poke(this ulong value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x00063031 File Offset: 0x00061231
		public unsafe static void Poke(this uint value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x00063031 File Offset: 0x00061231
		public unsafe static void Poke(this ushort value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00063031 File Offset: 0x00061231
		public unsafe static void Poke(this byte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00063025 File Offset: 0x00061225
		public unsafe static void InjectUnsigned(this long value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00063018 File Offset: 0x00061218
		public unsafe static void InjectUnsigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x00063018 File Offset: 0x00061218
		public unsafe static void PokeUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x00063018 File Offset: 0x00061218
		public unsafe static void PokeUnsigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001B4E74 File Offset: 0x001B3074
		public unsafe static void ReadOutUnsafe(ulong* sourcePtr, int sourcePos, ulong* targetPtr, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int num2;
			for (int i = bits; i > 0; i -= num2)
			{
				num2 = ((i > 64) ? 64 : i);
				ulong value = ArraySerializeUnsafe.Read(sourcePtr, ref num, num2);
				ArraySerializeUnsafe.Write(targetPtr, value, ref targetPos, num2);
			}
			targetPos += bits;
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001B4EBC File Offset: 0x001B30BC
		public unsafe static void ReadOutUnsafe(this ulong[] source, int sourcePos, byte[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (ulong[] array = source)
			{
				ulong* uPtr;
				if (source == null || array.Length == 0)
				{
					uPtr = null;
				}
				else
				{
					uPtr = &array[0];
				}
				fixed (byte[] array2 = target)
				{
					byte* ptr;
					if (target == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					ulong* uPtr2 = (ulong*)ptr;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x001B4F48 File Offset: 0x001B3148
		public unsafe static void ReadOutUnsafe(this ulong[] source, int sourcePos, uint[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (ulong[] array = source)
			{
				ulong* uPtr;
				if (source == null || array.Length == 0)
				{
					uPtr = null;
				}
				else
				{
					uPtr = &array[0];
				}
				fixed (uint[] array2 = target)
				{
					uint* ptr;
					if (target == null || array2.Length == 0)
					{
						ptr = null;
					}
					else
					{
						ptr = &array2[0];
					}
					ulong* uPtr2 = (ulong*)ptr;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x001B4FD4 File Offset: 0x001B31D4
		public unsafe static void ReadOutUnsafe(this ulong[] source, int sourcePos, ulong[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (ulong[] array = source)
			{
				ulong* uPtr;
				if (source == null || array.Length == 0)
				{
					uPtr = null;
				}
				else
				{
					uPtr = &array[0];
				}
				fixed (ulong[] array2 = target)
				{
					ulong* uPtr2;
					if (target == null || array2.Length == 0)
					{
						uPtr2 = null;
					}
					else
					{
						uPtr2 = &array2[0];
					}
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x001B505C File Offset: 0x001B325C
		public unsafe static void ReadOutUnsafe(this uint[] source, int sourcePos, byte[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (uint[] array = source)
			{
				uint* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (byte[] array2 = target)
				{
					byte* ptr2;
					if (target == null || array2.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array2[0];
					}
					ulong* uPtr = (ulong*)ptr;
					ulong* uPtr2 = (ulong*)ptr2;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x001B50EC File Offset: 0x001B32EC
		public unsafe static void ReadOutUnsafe(this uint[] source, int sourcePos, uint[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (uint[] array = source)
			{
				uint* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (uint[] array2 = target)
				{
					uint* ptr2;
					if (target == null || array2.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array2[0];
					}
					ulong* uPtr = (ulong*)ptr;
					ulong* uPtr2 = (ulong*)ptr2;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x001B517C File Offset: 0x001B337C
		public unsafe static void ReadOutUnsafe(this uint[] source, int sourcePos, ulong[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (uint[] array = source)
			{
				uint* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (ulong[] array2 = target)
				{
					ulong* uPtr;
					if (target == null || array2.Length == 0)
					{
						uPtr = null;
					}
					else
					{
						uPtr = &array2[0];
					}
					ulong* uPtr2 = (ulong*)ptr;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr2, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x001B5208 File Offset: 0x001B3408
		public unsafe static void ReadOutUnsafe(this byte[] source, int sourcePos, ulong[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (byte[] array = source)
			{
				byte* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (ulong[] array2 = target)
				{
					ulong* uPtr;
					if (target == null || array2.Length == 0)
					{
						uPtr = null;
					}
					else
					{
						uPtr = &array2[0];
					}
					ulong* uPtr2 = (ulong*)ptr;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr2, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x001B5294 File Offset: 0x001B3494
		public unsafe static void ReadOutUnsafe(this byte[] source, int sourcePos, uint[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (byte[] array = source)
			{
				byte* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (uint[] array2 = target)
				{
					uint* ptr2;
					if (target == null || array2.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array2[0];
					}
					ulong* uPtr = (ulong*)ptr;
					ulong* uPtr2 = (ulong*)ptr2;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x001B5324 File Offset: 0x001B3524
		public unsafe static void ReadOutUnsafe(this byte[] source, int sourcePos, byte[] target, ref int targetPos, int bits)
		{
			if (bits == 0)
			{
				return;
			}
			int num = sourcePos;
			int i = bits;
			fixed (byte[] array = source)
			{
				byte* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (byte[] array2 = target)
				{
					byte* ptr2;
					if (target == null || array2.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array2[0];
					}
					ulong* uPtr = (ulong*)ptr;
					ulong* uPtr2 = (ulong*)ptr2;
					while (i > 0)
					{
						int num2 = (i > 64) ? 64 : i;
						ulong value = ArraySerializeUnsafe.Read(uPtr, ref num, num2);
						ArraySerializeUnsafe.Write(uPtr2, value, ref targetPos, num2);
						i -= num2;
					}
				}
			}
			targetPos += bits;
		}

		// Token: 0x040052CE RID: 21198
		private const string bufferOverrunMsg = "Byte buffer overrun. Dataloss will occur.";
	}
}
