using System;
using System.Collections.Generic;

// Token: 0x020006AB RID: 1707
public class EnumData<TEnum> where TEnum : struct, Enum
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x06002A56 RID: 10838 RVA: 0x000D3365 File Offset: 0x000D1565
	public static EnumData<TEnum> Shared { get; } = new EnumData<TEnum>();

	// Token: 0x06002A57 RID: 10839 RVA: 0x000D336C File Offset: 0x000D156C
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

	// Token: 0x04002FD2 RID: 12242
	public readonly string[] Names;

	// Token: 0x04002FD3 RID: 12243
	public readonly TEnum[] Values;

	// Token: 0x04002FD4 RID: 12244
	public readonly long[] LongValues;

	// Token: 0x04002FD5 RID: 12245
	public readonly bool IsBitMaskCompatible;

	// Token: 0x04002FD6 RID: 12246
	public readonly Dictionary<TEnum, string> EnumToName;

	// Token: 0x04002FD7 RID: 12247
	public readonly Dictionary<string, TEnum> NameToEnum;

	// Token: 0x04002FD8 RID: 12248
	public readonly Dictionary<TEnum, int> EnumToIndex;

	// Token: 0x04002FD9 RID: 12249
	public readonly Dictionary<int, TEnum> IndexToEnum;

	// Token: 0x04002FDA RID: 12250
	public readonly Dictionary<TEnum, long> EnumToLong;

	// Token: 0x04002FDB RID: 12251
	public readonly Dictionary<long, TEnum> LongToEnum;
}
