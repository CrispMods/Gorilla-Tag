using System;
using System.Runtime.CompilerServices;

// Token: 0x020001B1 RID: 433
public static class GTBitOps
{
	// Token: 0x06000A27 RID: 2599 RVA: 0x0003794A File Offset: 0x00035B4A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetValueMask(int count)
	{
		return (1 << count) - 1;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00037954 File Offset: 0x00035B54
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMask(int index, int valueMask)
	{
		return ~(valueMask << index);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0003795D File Offset: 0x00035B5D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMaskByCount(int index, int count)
	{
		return ~((1 << count) - 1 << index);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0003796D File Offset: 0x00035B6D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, int index, int valueMask)
	{
		return bits >> index & valueMask;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00037977 File Offset: 0x00035B77
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, GTBitOps.BitWriteInfo info)
	{
		return bits >> info.index & info.valueMask;
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0003798B File Offset: 0x00035B8B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBitsByCount(int bits, int index, int count)
	{
		return bits >> index & (1 << count) - 1;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0003799C File Offset: 0x00035B9C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ReadBit(int bits, int index)
	{
		return (bits >> index & 1) == 1;
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x000379A9 File Offset: 0x00035BA9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, GTBitOps.BitWriteInfo info, int value)
	{
		bits = ((bits & info.clearMask) | (value & info.valueMask) << info.index);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000379C9 File Offset: 0x00035BC9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, GTBitOps.BitWriteInfo info, int value)
	{
		GTBitOps.WriteBits(ref bits, info, value);
		return bits;
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x000379D5 File Offset: 0x00035BD5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, int index, int valueMask, int clearMask, int value)
	{
		bits = ((bits & clearMask) | (value & valueMask) << index);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x000379E7 File Offset: 0x00035BE7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, int index, int valueMask, int clearMask, int value)
	{
		GTBitOps.WriteBits(ref bits, index, valueMask, clearMask, value);
		return bits;
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x000379F6 File Offset: 0x00035BF6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBitsByCount(ref int bits, int index, int count, int value)
	{
		bits = ((bits & ~((1 << count) - 1 << index)) | (value & (1 << count) - 1) << index);
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00037A1B File Offset: 0x00035C1B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBitsByCount(int bits, int index, int count, int value)
	{
		GTBitOps.WriteBitsByCount(ref bits, index, count, value);
		return bits;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00037A28 File Offset: 0x00035C28
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBit(ref int bits, int index, bool value)
	{
		bits = ((bits & ~(1 << index)) | (value ? 1 : 0) << index);
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00037A43 File Offset: 0x00035C43
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBit(int bits, int index, bool value)
	{
		GTBitOps.WriteBit(ref bits, index, value);
		return bits;
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00037A4F File Offset: 0x00035C4F
	public static string ToBinaryString(int number)
	{
		return Convert.ToString(number, 2).PadLeft(32, '0');
	}

	// Token: 0x020001B2 RID: 434
	public readonly struct BitWriteInfo
	{
		// Token: 0x06000A37 RID: 2615 RVA: 0x00037A61 File Offset: 0x00035C61
		public BitWriteInfo(int index, int count)
		{
			this.index = index;
			this.valueMask = GTBitOps.GetValueMask(count);
			this.clearMask = GTBitOps.GetClearMask(index, this.valueMask);
		}

		// Token: 0x04000C88 RID: 3208
		public readonly int index;

		// Token: 0x04000C89 RID: 3209
		public readonly int valueMask;

		// Token: 0x04000C8A RID: 3210
		public readonly int clearMask;
	}
}
