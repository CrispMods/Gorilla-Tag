using System;
using System.Collections.Generic;

// Token: 0x020006AD RID: 1709
public static class EnumUtil
{
	// Token: 0x06002A61 RID: 10849 RVA: 0x0004BBB0 File Offset: 0x00049DB0
	public static string[] GetNames<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<string>(EnumData<TEnum>.Shared.Names);
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x0004BBC1 File Offset: 0x00049DC1
	public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<TEnum>(EnumData<TEnum>.Shared.Values);
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x0004BBD2 File Offset: 0x00049DD2
	public static long[] GetLongValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<long>(EnumData<TEnum>.Shared.LongValues);
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x0004BBE3 File Offset: 0x00049DE3
	public static string EnumToName<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x0004BBF5 File Offset: 0x00049DF5
	public static TEnum NameToEnum<TEnum>(string n) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[n];
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x0004BC07 File Offset: 0x00049E07
	public static int EnumToIndex<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x0004BC19 File Offset: 0x00049E19
	public static TEnum IndexToEnum<TEnum>(int i) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.IndexToEnum[i];
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x0004BC2B File Offset: 0x00049E2B
	public static long EnumToLong<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x0004BC3D File Offset: 0x00049E3D
	public static TEnum LongToEnum<TEnum>(long l) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[l];
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x0004BC4F File Offset: 0x00049E4F
	public static TEnum GetValue<TEnum>(int index) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.Values[index];
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x0004BC07 File Offset: 0x00049E07
	public static int GetIndex<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[value];
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x0004BBE3 File Offset: 0x00049DE3
	public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[value];
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x0004BBF5 File Offset: 0x00049DF5
	public static TEnum GetValue<TEnum>(string name) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[name];
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0004BC2B File Offset: 0x00049E2B
	public static long GetLongValue<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[value];
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x0004BC3D File Offset: 0x00049E3D
	public static TEnum GetValue<TEnum>(long longValue) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[longValue];
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x0004BC61 File Offset: 0x00049E61
	public static TEnum[] SplitBitmask<TEnum>(TEnum bitmask) where TEnum : struct, Enum
	{
		return EnumUtil.SplitBitmask<TEnum>(Convert.ToInt64(bitmask));
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x0011AE70 File Offset: 0x00119070
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
