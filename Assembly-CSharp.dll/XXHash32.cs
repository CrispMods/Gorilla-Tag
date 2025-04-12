using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x0200089D RID: 2205
public static class XXHash32
{
	// Token: 0x0600355C RID: 13660 RVA: 0x0013ECF4 File Offset: 0x0013CEF4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(byte[] data, int length, uint seed = 0U)
	{
		fixed (byte* ptr = &data[0])
		{
			return XXHash32.ComputeUnsafe(ptr, length, seed);
		}
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x0013ED14 File Offset: 0x0013CF14
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(byte[] data, int offset, int length, uint seed = 0U)
	{
		fixed (byte* ptr = &data[offset])
		{
			return XXHash32.ComputeUnsafe(ptr, length, seed);
		}
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x0013ED34 File Offset: 0x0013CF34
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(Span<byte> data, int length, uint seed = 0U)
	{
		fixed (byte* reference = MemoryMarshal.GetReference<byte>(data))
		{
			return XXHash32.ComputeUnsafe(reference, length, seed);
		}
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x0013ED54 File Offset: 0x0013CF54
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(ReadOnlySpan<byte> data, int length, uint seed = 0U)
	{
		fixed (byte* reference = MemoryMarshal.GetReference<byte>(data))
		{
			return XXHash32.ComputeUnsafe(reference, length, seed);
		}
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x0013ED74 File Offset: 0x0013CF74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static int Compute(string s, uint seed = 0U)
	{
		char* ptr = s;
		if (ptr != null)
		{
			ptr += RuntimeHelpers.OffsetToStringData / 2;
		}
		byte* ptr2 = (byte*)ptr;
		int length = s.Length * 2;
		return XXHash32.ComputeUnsafe(ptr2, length, seed);
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x000524B3 File Offset: 0x000506B3
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static int ComputeUnsafe(byte* ptr, int length, uint seed)
	{
		return (int)XXHash32._XXH32(ptr, length, seed);
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x0013EDA4 File Offset: 0x0013CFA4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static uint _XXH32(byte* input, int len, uint seed)
	{
		uint num9;
		if (len >= 16)
		{
			byte* ptr = input + len - 15;
			uint num = seed + 2654435761U + 2246822519U;
			uint num2 = seed + 2246822519U;
			uint num3 = seed;
			uint num4 = seed - 2654435761U;
			do
			{
				uint num5 = *(uint*)input;
				uint num6 = *(uint*)(input + 4);
				uint num7 = *(uint*)(input + 8);
				uint num8 = *(uint*)(input + 12);
				num += num5 * 2246822519U;
				num = (num << 13 | num >> 19);
				num *= 2654435761U;
				num2 += num6 * 2246822519U;
				num2 = (num2 << 13 | num2 >> 19);
				num2 *= 2654435761U;
				num3 += num7 * 2246822519U;
				num3 = (num3 << 13 | num3 >> 19);
				num3 *= 2654435761U;
				num4 += num8 * 2246822519U;
				num4 = (num4 << 13 | num4 >> 19);
				num4 *= 2654435761U;
				input += 16;
			}
			while (input < ptr);
			num9 = (num << 1 | num >> 31) + (num2 << 7 | num2 >> 25) + (num3 << 12 | num3 >> 20) + (num4 << 18 | num4 >> 14);
		}
		else
		{
			num9 = seed + 374761393U;
		}
		num9 += (uint)len;
		for (len &= 15; len >= 4; len -= 4)
		{
			num9 += *(uint*)input * 3266489917U;
			input += 4;
			num9 = (num9 << 17 | num9 >> 15) * 668265263U;
		}
		while (len > 0)
		{
			num9 += (uint)(*input) * 374761393U;
			input++;
			num9 = (num9 << 11 | num9 >> 21) * 2654435761U;
			len--;
		}
		num9 ^= num9 >> 15;
		num9 *= 2246822519U;
		num9 ^= num9 >> 13;
		num9 *= 3266489917U;
		return num9 ^ num9 >> 16;
	}

	// Token: 0x040037C5 RID: 14277
	private const uint PRIME32_1 = 2654435761U;

	// Token: 0x040037C6 RID: 14278
	private const uint PRIME32_2 = 2246822519U;

	// Token: 0x040037C7 RID: 14279
	private const uint PRIME32_3 = 3266489917U;

	// Token: 0x040037C8 RID: 14280
	private const uint PRIME32_4 = 668265263U;

	// Token: 0x040037C9 RID: 14281
	private const uint PRIME32_5 = 374761393U;
}
