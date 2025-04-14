using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000880 RID: 2176
public static class StaticHash
{
	// Token: 0x060034A2 RID: 13474 RVA: 0x000FBC64 File Offset: 0x000F9E64
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(int i)
	{
		uint num = (uint)(i + 2127912214 + (i << 12));
		num = (num ^ 3345072700U ^ num >> 19);
		num = num + 374761393U + (num << 5);
		num = (num + 3550635116U ^ num << 9);
		num = num + 4251993797U + (num << 3);
		return (int)(num ^ 3042594569U ^ num >> 16);
	}

	// Token: 0x060034A3 RID: 13475 RVA: 0x000FBCC0 File Offset: 0x000F9EC0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(uint u)
	{
		uint num = u + 2127912214U + (u << 12);
		num = (num ^ 3345072700U ^ num >> 19);
		num = num + 374761393U + (num << 5);
		num = (num + 3550635116U ^ num << 9);
		num = num + 4251993797U + (num << 3);
		return (int)(num ^ 3042594569U ^ num >> 16);
	}

	// Token: 0x060034A4 RID: 13476 RVA: 0x000FBD1C File Offset: 0x000F9F1C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(float f)
	{
		return StaticHash.Compute(*Unsafe.As<float, int>(ref f));
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x000FBD2C File Offset: 0x000F9F2C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(float f1, float f2)
	{
		int i = StaticHash.Compute(f1);
		int i2 = StaticHash.Compute(f2);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x000FBD4C File Offset: 0x000F9F4C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(float f1, float f2, float f3)
	{
		int i = StaticHash.Compute(f1);
		int i2 = StaticHash.Compute(f2);
		int i3 = StaticHash.Compute(f3);
		return StaticHash.Compute(i, i2, i3);
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x000FBD74 File Offset: 0x000F9F74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(float f1, float f2, float f3, float f4)
	{
		int i = StaticHash.Compute(f1);
		int i2 = StaticHash.Compute(f2);
		int i3 = StaticHash.Compute(f3);
		int i4 = StaticHash.Compute(f4);
		return StaticHash.Compute(i, i2, i3, i4);
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x000FBDA4 File Offset: 0x000F9FA4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(long l)
	{
		ulong num = (ulong)(~(ulong)l + (l << 18));
		num ^= num >> 31;
		num *= 21UL;
		num ^= num >> 11;
		num += num << 6;
		num ^= num >> 22;
		return (int)num;
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x000FBDE0 File Offset: 0x000F9FE0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(long l1, long l2)
	{
		int i = StaticHash.Compute(l1);
		int i2 = StaticHash.Compute(l2);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x000FBE00 File Offset: 0x000FA000
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(long l1, long l2, long l3)
	{
		int i = StaticHash.Compute(l1);
		int i2 = StaticHash.Compute(l2);
		int i3 = StaticHash.Compute(l3);
		return StaticHash.Compute(i, i2, i3);
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x000FBE28 File Offset: 0x000FA028
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(long l1, long l2, long l3, long l4)
	{
		int i = StaticHash.Compute(l1);
		int i2 = StaticHash.Compute(l2);
		int i3 = StaticHash.Compute(l3);
		int i4 = StaticHash.Compute(l4);
		return StaticHash.Compute(i, i2, i3, i4);
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x000FBE58 File Offset: 0x000FA058
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(double d)
	{
		return StaticHash.Compute(*Unsafe.As<double, long>(ref d));
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x000FBE68 File Offset: 0x000FA068
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(double d1, double d2)
	{
		int i = StaticHash.Compute(d1);
		int i2 = StaticHash.Compute(d2);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x000FBE88 File Offset: 0x000FA088
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(double d1, double d2, double d3)
	{
		int i = StaticHash.Compute(d1);
		int i2 = StaticHash.Compute(d2);
		int i3 = StaticHash.Compute(d3);
		return StaticHash.Compute(i, i2, i3);
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x000FBEB0 File Offset: 0x000FA0B0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(double d1, double d2, double d3, double d4)
	{
		int i = StaticHash.Compute(d1);
		int i2 = StaticHash.Compute(d2);
		int i3 = StaticHash.Compute(d3);
		int i4 = StaticHash.Compute(d4);
		return StaticHash.Compute(i, i2, i3, i4);
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x000FBEE0 File Offset: 0x000FA0E0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(bool b)
	{
		if (!b)
		{
			return 1800329511;
		}
		return -1266253386;
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x000FBEF0 File Offset: 0x000FA0F0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(bool b1, bool b2)
	{
		int i = StaticHash.Compute(b1);
		int i2 = StaticHash.Compute(b2);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x000FBF10 File Offset: 0x000FA110
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(bool b1, bool b2, bool b3)
	{
		int i = StaticHash.Compute(b1);
		int i2 = StaticHash.Compute(b2);
		int i3 = StaticHash.Compute(b3);
		return StaticHash.Compute(i, i2, i3);
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x000FBF38 File Offset: 0x000FA138
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(bool b1, bool b2, bool b3, bool b4)
	{
		int i = StaticHash.Compute(b1);
		int i2 = StaticHash.Compute(b2);
		int i3 = StaticHash.Compute(b3);
		int i4 = StaticHash.Compute(b4);
		return StaticHash.Compute(i, i2, i3, i4);
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x000FBF68 File Offset: 0x000FA168
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(DateTime dt)
	{
		return StaticHash.Compute(dt.ToBinary());
	}

	// Token: 0x060034B5 RID: 13493 RVA: 0x000FBF78 File Offset: 0x000FA178
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(string s)
	{
		if (s == null || s.Length == 0)
		{
			return 0;
		}
		int i = s.Length;
		uint num = (uint)i;
		int num2 = i & 1;
		i >>= 1;
		int num3 = 0;
		while (i > 0)
		{
			num += (uint)s[num3];
			uint num4 = (uint)((uint)s[num3 + 1] << 11) ^ num;
			num = (num << 16 ^ num4);
			num3 += 2;
			num += num >> 11;
			i--;
		}
		if (num2 == 1)
		{
			num += (uint)s[num3];
			num ^= num << 11;
			num += num >> 17;
		}
		num ^= num << 3;
		num += num >> 5;
		num ^= num << 4;
		num += num >> 17;
		num ^= num << 25;
		return (int)(num + (num >> 6));
	}

	// Token: 0x060034B6 RID: 13494 RVA: 0x000FC020 File Offset: 0x000FA220
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(string s1, string s2)
	{
		int i = StaticHash.Compute(s1);
		int i2 = StaticHash.Compute(s2);
		return StaticHash.Compute(i, i2);
	}

	// Token: 0x060034B7 RID: 13495 RVA: 0x000FC040 File Offset: 0x000FA240
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(string s1, string s2, string s3)
	{
		int i = StaticHash.Compute(s1);
		int i2 = StaticHash.Compute(s2);
		int i3 = StaticHash.Compute(s3);
		return StaticHash.Compute(i, i2, i3);
	}

	// Token: 0x060034B8 RID: 13496 RVA: 0x000FC068 File Offset: 0x000FA268
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(string s1, string s2, string s3, string s4)
	{
		int i = StaticHash.Compute(s1);
		int i2 = StaticHash.Compute(s2);
		int i3 = StaticHash.Compute(s3);
		int i4 = StaticHash.Compute(s4);
		return StaticHash.Compute(i, i2, i3, i4);
	}

	// Token: 0x060034B9 RID: 13497 RVA: 0x000FC098 File Offset: 0x000FA298
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(byte[] bytes)
	{
		if (bytes == null || bytes.Length == 0)
		{
			return 0;
		}
		int i = bytes.Length;
		uint num = (uint)i;
		int num2 = i & 1;
		i >>= 1;
		int num3 = 0;
		while (i > 0)
		{
			num += (uint)bytes[num3];
			uint num4 = (uint)((int)bytes[num3 + 1] << 11 ^ (int)num);
			num = (num << 16 ^ num4);
			num3 += 2;
			num += num >> 11;
			i--;
		}
		if (num2 == 1)
		{
			num += (uint)bytes[num3];
			num ^= num << 11;
			num += num >> 17;
		}
		num ^= num << 3;
		num += num >> 5;
		num ^= num << 4;
		num += num >> 17;
		num ^= num << 25;
		return (int)(num + (num >> 6));
	}

	// Token: 0x060034BA RID: 13498 RVA: 0x000FC12C File Offset: 0x000FA32C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(int i1, int i2)
	{
		uint num = 3735928567U;
		uint num2 = num;
		uint result = num;
		num += (uint)i1;
		num2 += (uint)i2;
		StaticHash.Finalize(ref num, ref num2, ref result);
		return (int)result;
	}

	// Token: 0x060034BB RID: 13499 RVA: 0x000FC158 File Offset: 0x000FA358
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(int i1, int i2, int i3)
	{
		uint num = 3735928571U;
		uint num2 = num;
		uint num3 = num;
		num += (uint)i1;
		num2 += (uint)i2;
		num3 += (uint)i3;
		StaticHash.Finalize(ref num, ref num2, ref num3);
		return (int)num3;
	}

	// Token: 0x060034BC RID: 13500 RVA: 0x000FC188 File Offset: 0x000FA388
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(int i1, int i2, int i3, int i4)
	{
		uint num = 3735928575U;
		uint num2 = num;
		uint num3 = num;
		num += (uint)i1;
		num2 += (uint)i2;
		num3 += (uint)i3;
		StaticHash.Mix(ref num, ref num2, ref num3);
		num += (uint)i4;
		StaticHash.Finalize(ref num, ref num2, ref num3);
		return (int)num3;
	}

	// Token: 0x060034BD RID: 13501 RVA: 0x000FC1C8 File Offset: 0x000FA3C8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(int[] values)
	{
		if (values == null || values.Length == 0)
		{
			return 224428569;
		}
		int num = values.Length;
		uint num2 = (uint)(-559038737 + (num << 2));
		uint num3 = num2;
		uint num4 = num2;
		int num5 = 0;
		while (num - num5 > 3)
		{
			num2 += (uint)values[num5];
			num3 += (uint)values[num5 + 1];
			num4 += (uint)values[num5 + 2];
			StaticHash.Mix(ref num2, ref num3, ref num4);
			num5 += 3;
		}
		if (num - num5 > 2)
		{
			num4 += (uint)values[num5 + 2];
		}
		if (num - num5 > 1)
		{
			num3 += (uint)values[num5 + 1];
		}
		if (num - num5 > 0)
		{
			num2 += (uint)values[num5];
			StaticHash.Finalize(ref num2, ref num3, ref num4);
		}
		return (int)num4;
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x000FC264 File Offset: 0x000FA464
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(uint[] values)
	{
		if (values == null || values.Length == 0)
		{
			return 224428569;
		}
		int num = values.Length;
		uint num2 = (uint)(-559038737 + (num << 2));
		uint num3 = num2;
		uint num4 = num2;
		int num5 = 0;
		while (num - num5 > 3)
		{
			num2 += values[num5];
			num3 += values[num5 + 1];
			num4 += values[num5 + 2];
			StaticHash.Mix(ref num2, ref num3, ref num4);
			num5 += 3;
		}
		if (num - num5 > 2)
		{
			num4 += values[num5 + 2];
		}
		if (num - num5 > 1)
		{
			num3 += values[num5 + 1];
		}
		if (num - num5 > 0)
		{
			num2 += values[num5];
			StaticHash.Finalize(ref num2, ref num3, ref num4);
		}
		return (int)num4;
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x000FC300 File Offset: 0x000FA500
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(uint u1, uint u2)
	{
		uint num = 3735928567U;
		uint num2 = num;
		uint result = num;
		num += u1;
		num2 += u2;
		StaticHash.Finalize(ref num, ref num2, ref result);
		return (int)result;
	}

	// Token: 0x060034C0 RID: 13504 RVA: 0x000FC32C File Offset: 0x000FA52C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(uint u1, uint u2, uint u3)
	{
		uint num = 3735928571U;
		uint num2 = num;
		uint num3 = num;
		num += u1;
		num2 += u2;
		num3 += u3;
		StaticHash.Finalize(ref num, ref num2, ref num3);
		return (int)num3;
	}

	// Token: 0x060034C1 RID: 13505 RVA: 0x000FC35C File Offset: 0x000FA55C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Compute(uint u1, uint u2, uint u3, uint u4)
	{
		uint num = 3735928575U;
		uint num2 = num;
		uint num3 = num;
		num += u1;
		num2 += u2;
		num3 += u3;
		StaticHash.Mix(ref num, ref num2, ref num3);
		num += u4;
		StaticHash.Finalize(ref num, ref num2, ref num3);
		return (int)num3;
	}

	// Token: 0x060034C2 RID: 13506 RVA: 0x000FC39C File Offset: 0x000FA59C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ComputeOrderAgnostic(int[] values)
	{
		if (values == null || values.Length == 0)
		{
			return 0;
		}
		uint num = (uint)StaticHash.Compute(values[0]);
		if (values.Length == 1)
		{
			return (int)num;
		}
		for (int i = 1; i < values.Length; i++)
		{
			num += (uint)StaticHash.Compute(values[i]);
		}
		return (int)num;
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x000FC3E0 File Offset: 0x000FA5E0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long Compute128To64(long a, long b)
	{
		ulong num = (ulong)((b ^ a) * -7070675565921424023L);
		num ^= num >> 47;
		long num2 = (a ^ (long)num) * -7070675565921424023L;
		return (num2 ^ (long)((ulong)num2 >> 47)) * -7070675565921424023L;
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x000FC420 File Offset: 0x000FA620
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long Compute128To64(ulong a, ulong b)
	{
		ulong num = (b ^ a) * 11376068507788127593UL;
		num ^= num >> 47;
		ulong num2 = (a ^ num) * 11376068507788127593UL;
		return (long)((num2 ^ num2 >> 47) * 11376068507788127593UL);
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x000FC45E File Offset: 0x000FA65E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ComputeTriple32(int i)
	{
		int num = i + 1;
		int num2 = (num ^ (int)((uint)num >> 17)) * -312814405;
		int num3 = (num2 ^ (int)((uint)num2 >> 11)) * -1404298415;
		int num4 = (num3 ^ (int)((uint)num3 >> 15)) * 830770091;
		return num4 ^ (int)((uint)num4 >> 14);
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x000FC48C File Offset: 0x000FA68C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReverseTriple32(int i)
	{
		uint num = (uint)(i ^ (int)((uint)i >> 14 ^ (uint)i >> 28));
		num *= 850532099U;
		num ^= (num >> 15 ^ num >> 30);
		num *= 1184763313U;
		num ^= (num >> 11 ^ num >> 22);
		num *= 2041073779U;
		num ^= num >> 17;
		return (int)(num - 1U);
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x000FC4E4 File Offset: 0x000FA6E4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Mix(ref uint a, ref uint b, ref uint c)
	{
		a -= c;
		a ^= StaticHash.Rotate(c, 4);
		c += b;
		b -= a;
		b ^= StaticHash.Rotate(a, 6);
		a += c;
		c -= b;
		c ^= StaticHash.Rotate(b, 8);
		b += a;
		a -= c;
		a ^= StaticHash.Rotate(c, 16);
		c += b;
		b -= a;
		b ^= StaticHash.Rotate(a, 19);
		a += c;
		c -= b;
		c ^= StaticHash.Rotate(b, 4);
		b += a;
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x000FC598 File Offset: 0x000FA798
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Finalize(ref uint a, ref uint b, ref uint c)
	{
		c ^= b;
		c -= StaticHash.Rotate(b, 14);
		a ^= c;
		a -= StaticHash.Rotate(c, 11);
		b ^= a;
		b -= StaticHash.Rotate(a, 25);
		c ^= b;
		c -= StaticHash.Rotate(b, 16);
		a ^= c;
		a -= StaticHash.Rotate(c, 4);
		b ^= a;
		b -= StaticHash.Rotate(a, 14);
		c ^= b;
		c -= StaticHash.Rotate(b, 24);
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x000FC637 File Offset: 0x000FA837
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static uint Rotate(uint x, int k)
	{
		return x << k | x >> 32 - k;
	}

	// Token: 0x02000881 RID: 2177
	[StructLayout(LayoutKind.Explicit)]
	private struct SingleInt32
	{
		// Token: 0x04003774 RID: 14196
		[FieldOffset(0)]
		public float single;

		// Token: 0x04003775 RID: 14197
		[FieldOffset(0)]
		public int int32;
	}

	// Token: 0x02000882 RID: 2178
	[StructLayout(LayoutKind.Explicit)]
	private struct DoubleInt64
	{
		// Token: 0x04003776 RID: 14198
		[FieldOffset(0)]
		public double @double;

		// Token: 0x04003777 RID: 14199
		[FieldOffset(0)]
		public long int64;
	}
}
