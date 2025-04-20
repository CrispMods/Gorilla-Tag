using System;
using System.Runtime.CompilerServices;

// Token: 0x020001BC RID: 444
public static class GTBitOps
{
	// Token: 0x06000A73 RID: 2675 RVA: 0x00037535 File Offset: 0x00035735
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetValueMask(int count)
	{
		return (1 << count) - 1;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0003753F File Offset: 0x0003573F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMask(int index, int valueMask)
	{
		return ~(valueMask << index);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00037548 File Offset: 0x00035748
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMaskByCount(int index, int count)
	{
		return ~((1 << count) - 1 << index);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00037558 File Offset: 0x00035758
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, int index, int valueMask)
	{
		return bits >> index & valueMask;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00037562 File Offset: 0x00035762
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, GTBitOps.BitWriteInfo info)
	{
		return bits >> info.index & info.valueMask;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00037576 File Offset: 0x00035776
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBitsByCount(int bits, int index, int count)
	{
		return bits >> index & (1 << count) - 1;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00037587 File Offset: 0x00035787
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ReadBit(int bits, int index)
	{
		return (bits >> index & 1) == 1;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00037594 File Offset: 0x00035794
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, GTBitOps.BitWriteInfo info, int value)
	{
		bits = ((bits & info.clearMask) | (value & info.valueMask) << info.index);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x000375B4 File Offset: 0x000357B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, GTBitOps.BitWriteInfo info, int value)
	{
		GTBitOps.WriteBits(ref bits, info, value);
		return bits;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x000375C0 File Offset: 0x000357C0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, int index, int valueMask, int clearMask, int value)
	{
		bits = ((bits & clearMask) | (value & valueMask) << index);
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x000375D2 File Offset: 0x000357D2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, int index, int valueMask, int clearMask, int value)
	{
		GTBitOps.WriteBits(ref bits, index, valueMask, clearMask, value);
		return bits;
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x000375E1 File Offset: 0x000357E1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBitsByCount(ref int bits, int index, int count, int value)
	{
		bits = ((bits & ~((1 << count) - 1 << index)) | (value & (1 << count) - 1) << index);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00037606 File Offset: 0x00035806
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBitsByCount(int bits, int index, int count, int value)
	{
		GTBitOps.WriteBitsByCount(ref bits, index, count, value);
		return bits;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00037613 File Offset: 0x00035813
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBit(ref int bits, int index, bool value)
	{
		bits = ((bits & ~(1 << index)) | (value ? 1 : 0) << index);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0003762E File Offset: 0x0003582E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBit(int bits, int index, bool value)
	{
		GTBitOps.WriteBit(ref bits, index, value);
		return bits;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0003763A File Offset: 0x0003583A
	public static string ToBinaryString(int number)
	{
		return Convert.ToString(number, 2).PadLeft(32, '0');
	}

	// Token: 0x020001BD RID: 445
	public readonly struct BitWriteInfo
	{
		// Token: 0x06000A83 RID: 2691 RVA: 0x0003764C File Offset: 0x0003584C
		public BitWriteInfo(int index, int count)
		{
			this.index = index;
			this.valueMask = GTBitOps.GetValueMask(count);
			this.clearMask = GTBitOps.GetClearMask(index, this.valueMask);
		}

		// Token: 0x04000CCE RID: 3278
		public readonly int index;

		// Token: 0x04000CCF RID: 3279
		public readonly int valueMask;

		// Token: 0x04000CD0 RID: 3280
		public readonly int clearMask;
	}
}
