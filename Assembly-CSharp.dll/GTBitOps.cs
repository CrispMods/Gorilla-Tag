using System;
using System.Runtime.CompilerServices;

// Token: 0x020001B1 RID: 433
public static class GTBitOps
{
	// Token: 0x06000A29 RID: 2601 RVA: 0x00036275 File Offset: 0x00034475
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetValueMask(int count)
	{
		return (1 << count) - 1;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0003627F File Offset: 0x0003447F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMask(int index, int valueMask)
	{
		return ~(valueMask << index);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00036288 File Offset: 0x00034488
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int GetClearMaskByCount(int index, int count)
	{
		return ~((1 << count) - 1 << index);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00036298 File Offset: 0x00034498
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, int index, int valueMask)
	{
		return bits >> index & valueMask;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000362A2 File Offset: 0x000344A2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBits(int bits, GTBitOps.BitWriteInfo info)
	{
		return bits >> info.index & info.valueMask;
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x000362B6 File Offset: 0x000344B6
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadBitsByCount(int bits, int index, int count)
	{
		return bits >> index & (1 << count) - 1;
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000362C7 File Offset: 0x000344C7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ReadBit(int bits, int index)
	{
		return (bits >> index & 1) == 1;
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x000362D4 File Offset: 0x000344D4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, GTBitOps.BitWriteInfo info, int value)
	{
		bits = ((bits & info.clearMask) | (value & info.valueMask) << info.index);
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x000362F4 File Offset: 0x000344F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, GTBitOps.BitWriteInfo info, int value)
	{
		GTBitOps.WriteBits(ref bits, info, value);
		return bits;
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00036300 File Offset: 0x00034500
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBits(ref int bits, int index, int valueMask, int clearMask, int value)
	{
		bits = ((bits & clearMask) | (value & valueMask) << index);
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x00036312 File Offset: 0x00034512
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBits(int bits, int index, int valueMask, int clearMask, int value)
	{
		GTBitOps.WriteBits(ref bits, index, valueMask, clearMask, value);
		return bits;
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00036321 File Offset: 0x00034521
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBitsByCount(ref int bits, int index, int count, int value)
	{
		bits = ((bits & ~((1 << count) - 1 << index)) | (value & (1 << count) - 1) << index);
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x00036346 File Offset: 0x00034546
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBitsByCount(int bits, int index, int count, int value)
	{
		GTBitOps.WriteBitsByCount(ref bits, index, count, value);
		return bits;
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x00036353 File Offset: 0x00034553
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteBit(ref int bits, int index, bool value)
	{
		bits = ((bits & ~(1 << index)) | (value ? 1 : 0) << index);
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0003636E File Offset: 0x0003456E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int WriteBit(int bits, int index, bool value)
	{
		GTBitOps.WriteBit(ref bits, index, value);
		return bits;
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0003637A File Offset: 0x0003457A
	public static string ToBinaryString(int number)
	{
		return Convert.ToString(number, 2).PadLeft(32, '0');
	}

	// Token: 0x020001B2 RID: 434
	public readonly struct BitWriteInfo
	{
		// Token: 0x06000A39 RID: 2617 RVA: 0x0003638C File Offset: 0x0003458C
		public BitWriteInfo(int index, int count)
		{
			this.index = index;
			this.valueMask = GTBitOps.GetValueMask(count);
			this.clearMask = GTBitOps.GetClearMask(index, this.valueMask);
		}

		// Token: 0x04000C89 RID: 3209
		public readonly int index;

		// Token: 0x04000C8A RID: 3210
		public readonly int valueMask;

		// Token: 0x04000C8B RID: 3211
		public readonly int clearMask;
	}
}
