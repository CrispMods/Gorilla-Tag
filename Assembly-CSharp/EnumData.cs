using System;
using System.Collections.Generic;

// Token: 0x020006C0 RID: 1728
public class EnumData<TEnum> where TEnum : struct, Enum
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x0004CEE2 File Offset: 0x0004B0E2
	public static EnumData<TEnum> Shared { get; } = new EnumData<TEnum>();

	// Token: 0x06002AED RID: 10989 RVA: 0x0011F8A0 File Offset: 0x0011DAA0
	private EnumData()
	{
		this.Names = Enum.GetNames(typeof(TEnum));
		int num = this.Names.Length;
		this.Values = new TEnum[num];
		this.LongValues = new long[num];
		this.EnumToName = new Dictionary<TEnum, string>(num);
		this.NameToEnum = new Dictionary<string, TEnum>(num);
		this.EnumToIndex = new Dictionary<TEnum, int>(num);
		this.IndexToEnum = new Dictionary<int, TEnum>(num);
		this.EnumToLong = new Dictionary<TEnum, long>(num);
		this.LongToEnum = new Dictionary<long, TEnum>(num);
		for (int i = 0; i < this.Names.Length; i++)
		{
			string text = this.Names[i];
			TEnum tenum = Enum.Parse<TEnum>(text);
			long num2 = Convert.ToInt64(tenum);
			this.Values[i] = tenum;
			this.LongValues[i] = num2;
			this.EnumToName[tenum] = text;
			this.NameToEnum[text] = tenum;
			this.EnumToIndex[tenum] = i;
			this.IndexToEnum[i] = tenum;
			this.EnumToLong[tenum] = num2;
			this.LongToEnum[num2] = tenum;
		}
		long num3 = 0L;
		bool isBitMaskCompatible = true;
		foreach (long num4 in this.LongValues)
		{
			if (num4 != 0L && (num4 & num4 - 1L) != 0L && (num3 & num4) != num4)
			{
				isBitMaskCompatible = false;
				break;
			}
			num3 |= num4;
		}
		this.IsBitMaskCompatible = isBitMaskCompatible;
	}

	// Token: 0x0400306F RID: 12399
	public readonly string[] Names;

	// Token: 0x04003070 RID: 12400
	public readonly TEnum[] Values;

	// Token: 0x04003071 RID: 12401
	public readonly long[] LongValues;

	// Token: 0x04003072 RID: 12402
	public readonly bool IsBitMaskCompatible;

	// Token: 0x04003073 RID: 12403
	public readonly Dictionary<TEnum, string> EnumToName;

	// Token: 0x04003074 RID: 12404
	public readonly Dictionary<string, TEnum> NameToEnum;

	// Token: 0x04003075 RID: 12405
	public readonly Dictionary<TEnum, int> EnumToIndex;

	// Token: 0x04003076 RID: 12406
	public readonly Dictionary<int, TEnum> IndexToEnum;

	// Token: 0x04003077 RID: 12407
	public readonly Dictionary<TEnum, long> EnumToLong;

	// Token: 0x04003078 RID: 12408
	public readonly Dictionary<long, TEnum> LongToEnum;
}
