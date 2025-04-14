using System;

// Token: 0x020006AE RID: 1710
public static class EnumUtilExt
{
	// Token: 0x06002A72 RID: 10866 RVA: 0x000D39B3 File Offset: 0x000D1BB3
	public static string GetName<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000D39D7 File Offset: 0x000D1BD7
	public static int GetIndex<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000D39FB File Offset: 0x000D1BFB
	public static long GetLongValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000D3AFC File Offset: 0x000D1CFC
	public static TEnum GetNextValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		return shared.Values[shared.EnumToIndex[e] + 1 % shared.Values.Length];
	}
}
