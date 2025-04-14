using System;
using System.Collections.Generic;

// Token: 0x020006AC RID: 1708
public static class EnumUtil
{
	// Token: 0x06002A59 RID: 10841 RVA: 0x000D3500 File Offset: 0x000D1700
	public static string[] GetNames<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<string>(EnumData<TEnum>.Shared.Names);
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000D3511 File Offset: 0x000D1711
	public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<TEnum>(EnumData<TEnum>.Shared.Values);
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000D3522 File Offset: 0x000D1722
	public static long[] GetLongValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<long>(EnumData<TEnum>.Shared.LongValues);
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000D3533 File Offset: 0x000D1733
	public static string EnumToName<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000D3545 File Offset: 0x000D1745
	public static TEnum NameToEnum<TEnum>(string n) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[n];
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000D3557 File Offset: 0x000D1757
	public static int EnumToIndex<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x000D3569 File Offset: 0x000D1769
	public static TEnum IndexToEnum<TEnum>(int i) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.IndexToEnum[i];
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x000D357B File Offset: 0x000D177B
	public static long EnumToLong<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000D358D File Offset: 0x000D178D
	public static TEnum LongToEnum<TEnum>(long l) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[l];
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000D359F File Offset: 0x000D179F
	public static TEnum GetValue<TEnum>(int index) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.Values[index];
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x000D3557 File Offset: 0x000D1757
	public static int GetIndex<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[value];
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x000D3533 File Offset: 0x000D1733
	public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[value];
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000D3545 File Offset: 0x000D1745
	public static TEnum GetValue<TEnum>(string name) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[name];
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x000D357B File Offset: 0x000D177B
	public static long GetLongValue<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[value];
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x000D358D File Offset: 0x000D178D
	public static TEnum GetValue<TEnum>(long longValue) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[longValue];
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x000D35B1 File Offset: 0x000D17B1
	public static TEnum[] SplitBitmask<TEnum>(TEnum bitmask) where TEnum : struct, Enum
	{
		return EnumUtil.SplitBitmask<TEnum>(Convert.ToInt64(bitmask));
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x000D35C4 File Offset: 0x000D17C4
	public static TEnum[] SplitBitmask<TEnum>(long bitmaskLong) where TEnum : struct, Enum
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		if (!shared.IsBitMaskCompatible)
		{
			throw new ArgumentException("The enum type " + typeof(TEnum).Name + " is not bitmask-compatible.");
		}
		if (bitmaskLong == 0L)
		{
			return new TEnum[]
			{
				(TEnum)((object)Enum.ToObject(typeof(TEnum), 0L))
			};
		}
		List<TEnum> list = new List<TEnum>(shared.Values.Length);
		for (int i = 0; i < shared.Values.Length; i++)
		{
			TEnum item = shared.Values[i];
			long num = shared.LongValues[i];
			if (num != 0L && (bitmaskLong & num) == num)
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}
}
