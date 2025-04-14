using System;
using System.Collections.Generic;

// Token: 0x020006AC RID: 1708
public class EnumData<TEnum> where TEnum : struct, Enum
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06002A5E RID: 10846 RVA: 0x000D37E5 File Offset: 0x000D19E5
	public static EnumData<TEnum> Shared { get; } = new EnumData<TEnum>();

	// Token: 0x06002A5F RID: 10847 RVA: 0x000D37EC File Offset: 0x000D19EC
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

	// Token: 0x04002FD8 RID: 12248
	public readonly string[] Names;

	// Token: 0x04002FD9 RID: 12249
	public readonly TEnum[] Values;

	// Token: 0x04002FDA RID: 12250
	public readonly long[] LongValues;

	// Token: 0x04002FDB RID: 12251
	public readonly bool IsBitMaskCompatible;

	// Token: 0x04002FDC RID: 12252
	public readonly Dictionary<TEnum, string> EnumToName;

	// Token: 0x04002FDD RID: 12253
	public readonly Dictionary<string, TEnum> NameToEnum;

	// Token: 0x04002FDE RID: 12254
	public readonly Dictionary<TEnum, int> EnumToIndex;

	// Token: 0x04002FDF RID: 12255
	public readonly Dictionary<int, TEnum> IndexToEnum;

	// Token: 0x04002FE0 RID: 12256
	public readonly Dictionary<TEnum, long> EnumToLong;

	// Token: 0x04002FE1 RID: 12257
	public readonly Dictionary<long, TEnum> LongToEnum;
}
