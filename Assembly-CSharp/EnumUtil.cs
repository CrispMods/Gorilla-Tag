using System;
using System.Collections.Generic;

// Token: 0x020006C1 RID: 1729
public static class EnumUtil
{
	// Token: 0x06002AEF RID: 10991 RVA: 0x0004CEF5 File Offset: 0x0004B0F5
	public static string[] GetNames<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<string>(EnumData<TEnum>.Shared.Names);
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x0004CF06 File Offset: 0x0004B106
	public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<TEnum>(EnumData<TEnum>.Shared.Values);
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x0004CF17 File Offset: 0x0004B117
	public static long[] GetLongValues<TEnum>() where TEnum : struct, Enum
	{
		return ArrayUtils.Clone<long>(EnumData<TEnum>.Shared.LongValues);
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x0004CF28 File Offset: 0x0004B128
	public static string EnumToName<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x0004CF3A File Offset: 0x0004B13A
	public static TEnum NameToEnum<TEnum>(string n) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[n];
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x0004CF4C File Offset: 0x0004B14C
	public static int EnumToIndex<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x0004CF5E File Offset: 0x0004B15E
	public static TEnum IndexToEnum<TEnum>(int i) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.IndexToEnum[i];
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x0004CF70 File Offset: 0x0004B170
	public static long EnumToLong<TEnum>(TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002AF7 RID: 10999 RVA: 0x0004CF82 File Offset: 0x0004B182
	public static TEnum LongToEnum<TEnum>(long l) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[l];
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x0004CF94 File Offset: 0x0004B194
	public static TEnum GetValue<TEnum>(int index) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.Values[index];
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x0004CF4C File Offset: 0x0004B14C
	public static int GetIndex<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[value];
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x0004CF28 File Offset: 0x0004B128
	public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[value];
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x0004CF3A File Offset: 0x0004B13A
	public static TEnum GetValue<TEnum>(string name) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.NameToEnum[name];
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x0004CF70 File Offset: 0x0004B170
	public static long GetLongValue<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[value];
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x0004CF82 File Offset: 0x0004B182
	public static TEnum GetValue<TEnum>(long longValue) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.LongToEnum[longValue];
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x0004CFA6 File Offset: 0x0004B1A6
	public static TEnum[] SplitBitmask<TEnum>(TEnum bitmask) where TEnum : struct, Enum
	{
		return EnumUtil.SplitBitmask<TEnum>(Convert.ToInt64(bitmask));
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x0011FA28 File Offset: 0x0011DC28
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
