using System;

// Token: 0x020006C2 RID: 1730
public static class EnumUtilExt
{
	// Token: 0x06002B00 RID: 11008 RVA: 0x0004CF28 File Offset: 0x0004B128
	public static string GetName<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x0004CF4C File Offset: 0x0004B14C
	public static int GetIndex<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x0004CF70 File Offset: 0x0004B170
	public static long GetLongValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x0011FAE0 File Offset: 0x0011DCE0
	public static TEnum GetNextValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		return shared.Values[shared.EnumToIndex[e] + 1 % shared.Values.Length];
	}
}
