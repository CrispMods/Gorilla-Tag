using System;

// Token: 0x020006AD RID: 1709
public static class EnumUtilExt
{
	// Token: 0x06002A6A RID: 10858 RVA: 0x000D3533 File Offset: 0x000D1733
	public static string GetName<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToName[e];
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x000D3557 File Offset: 0x000D1757
	public static int GetIndex<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToIndex[e];
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000D357B File Offset: 0x000D177B
	public static long GetLongValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		return EnumData<TEnum>.Shared.EnumToLong[e];
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000D367C File Offset: 0x000D187C
	public static TEnum GetNextValue<TEnum>(this TEnum e) where TEnum : struct, Enum
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		return shared.Values[shared.EnumToIndex[e] + 1 % shared.Values.Length];
	}
}
