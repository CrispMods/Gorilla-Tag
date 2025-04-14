using System;

namespace emotitron.Compression
{
	// Token: 0x02000C65 RID: 3173
	public static class ArraySerializeUnsafe
	{
		// Token: 0x06004F6F RID: 20335 RVA: 0x00185060 File Offset: 0x00183260
		public unsafe static void WriteSigned(ulong* buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(buffer, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00185080 File Offset: 0x00183280
		public unsafe static void AppendSigned(ulong* buffer, int value, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(buffer, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x001850A0 File Offset: 0x001832A0
		public unsafe static void AddSigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001850C0 File Offset: 0x001832C0
		public unsafe static void AddSigned(this short value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001850E0 File Offset: 0x001832E0
		public unsafe static void AddSigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Append(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x00185100 File Offset: 0x00183300
		public unsafe static void InjectSigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x00185120 File Offset: 0x00183320
		public unsafe static void InjectSigned(this short value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00185140 File Offset: 0x00183340
		public unsafe static void InjectSigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00185160 File Offset: 0x00183360
		public unsafe static void PokeSigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)(value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00185184 File Offset: 0x00183384
		public unsafe static void PokeSigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001851A8 File Offset: 0x001833A8
		public unsafe static void PokeSigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)((int)value << 1 ^ value >> 31);
			ArraySerializeUnsafe.Write(uPtr, (ulong)num, ref bitposition, bits);
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x001851CC File Offset: 0x001833CC
		public unsafe static int ReadSigned(ulong* uPtr, ref int bitposition, int bits)
		{
			uint num = (uint)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x001851F0 File Offset: 0x001833F0
		public unsafe static int PeekSigned(ulong* uPtr, int bitposition, int bits)
		{
			uint num = (uint)ArraySerializeUnsafe.Read(uPtr, ref bitposition, bits);
			return (int)((ulong)(num >> 1) ^ (ulong)((long)(-(long)(num & 1U))));
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00185214 File Offset: 0x00183414
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

		// Token: 0x06004F7D RID: 20349 RVA: 0x0018526C File Offset: 0x0018346C
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

		// Token: 0x06004F7E RID: 20350 RVA: 0x001852F0 File Offset: 0x001834F0
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

		// Token: 0x06004F7F RID: 20351 RVA: 0x00185354 File Offset: 0x00183554
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

		// Token: 0x06004F80 RID: 20352 RVA: 0x001853B3 File Offset: 0x001835B3
		public unsafe static void Add(this ulong value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001853BF File Offset: 0x001835BF
		public unsafe static void Add(this uint value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x001853BF File Offset: 0x001835BF
		public unsafe static void Add(this ushort value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x001853BF File Offset: 0x001835BF
		public unsafe static void Add(this byte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x001853B3 File Offset: 0x001835B3
		public unsafe static void AddUnsigned(this long value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x001853CC File Offset: 0x001835CC
		public unsafe static void AddUnsigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x001853CC File Offset: 0x001835CC
		public unsafe static void AddUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x001853CC File Offset: 0x001835CC
		public unsafe static void AddUnsigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Append(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x001853D9 File Offset: 0x001835D9
		public unsafe static void Inject(this ulong value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x001853E4 File Offset: 0x001835E4
		public unsafe static void Inject(this uint value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x001853E4 File Offset: 0x001835E4
		public unsafe static void Inject(this ushort value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x001853E4 File Offset: 0x001835E4
		public unsafe static void Inject(this byte value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x001853D9 File Offset: 0x001835D9
		public unsafe static void InjectUnsigned(this long value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x001853F0 File Offset: 0x001835F0
		public unsafe static void InjectUnsigned(this int value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x001853FC File Offset: 0x001835FC
		public unsafe static void InjectUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001853F0 File Offset: 0x001835F0
		public unsafe static void InjectUnsigned(this sbyte value, ulong* uPtr, ref int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x00185409 File Offset: 0x00183609
		public unsafe static void Poke(this ulong value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, value, ref bitposition, bits);
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x00185415 File Offset: 0x00183615
		public unsafe static void Poke(this uint value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00185415 File Offset: 0x00183615
		public unsafe static void Poke(this ushort value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00185415 File Offset: 0x00183615
		public unsafe static void Poke(this byte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00185409 File Offset: 0x00183609
		public unsafe static void InjectUnsigned(this long value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)value, ref bitposition, bits);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001853FC File Offset: 0x001835FC
		public unsafe static void InjectUnsigned(this int value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001853FC File Offset: 0x001835FC
		public unsafe static void PokeUnsigned(this short value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001853FC File Offset: 0x001835FC
		public unsafe static void PokeUnsigned(this sbyte value, ulong* uPtr, int bitposition, int bits)
		{
			ArraySerializeUnsafe.Write(uPtr, (ulong)((long)value), ref bitposition, bits);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00185424 File Offset: 0x00183624
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

		// Token: 0x06004F99 RID: 20377 RVA: 0x0018546C File Offset: 0x0018366C
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

		// Token: 0x06004F9A RID: 20378 RVA: 0x001854F8 File Offset: 0x001836F8
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

		// Token: 0x06004F9B RID: 20379 RVA: 0x00185584 File Offset: 0x00183784
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

		// Token: 0x06004F9C RID: 20380 RVA: 0x0018560C File Offset: 0x0018380C
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

		// Token: 0x06004F9D RID: 20381 RVA: 0x0018569C File Offset: 0x0018389C
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

		// Token: 0x06004F9E RID: 20382 RVA: 0x0018572C File Offset: 0x0018392C
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

		// Token: 0x06004F9F RID: 20383 RVA: 0x001857B8 File Offset: 0x001839B8
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

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00185844 File Offset: 0x00183A44
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

		// Token: 0x06004FA1 RID: 20385 RVA: 0x001858D4 File Offset: 0x00183AD4
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

		// Token: 0x040052BC RID: 21180
		private const string bufferOverrunMsg = "Byte buffer overrun. Dataloss will occur.";
	}
}
