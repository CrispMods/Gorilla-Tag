using System;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
[Serializable]
public struct StringEnum<TEnum> where TEnum : struct, Enum
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000D67E8 File Offset: 0x000D49E8
	public TEnum Value
	{
		get
		{
			return this.m_EnumValue;
		}
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000D67F0 File Offset: 0x000D49F0
	public static implicit operator StringEnum<TEnum>(TEnum e)
	{
		return new StringEnum<TEnum>
		{
			m_EnumValue = e
		};
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x000D67E8 File Offset: 0x000D49E8
	public static implicit operator TEnum(StringEnum<TEnum> se)
	{
		return se.m_EnumValue;
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000D680E File Offset: 0x000D4A0E
	public static bool operator ==(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return left.m_EnumValue.Equals(right.m_EnumValue);
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x000D682D File Offset: 0x000D4A2D
	public static bool operator !=(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return !(left == right);
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000D683C File Offset: 0x000D4A3C
	public override bool Equals(object obj)
	{
		if (obj is StringEnum<TEnum>)
		{
			StringEnum<TEnum> stringEnum = (StringEnum<TEnum>)obj;
			return this.m_EnumValue.Equals(stringEnum.m_EnumValue);
		}
		return false;
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x000D6876 File Offset: 0x000D4A76
	public override int GetHashCode()
	{
		return this.m_EnumValue.GetHashCode();
	}

	// Token: 0x040030B7 RID: 12471
	[SerializeField]
	private TEnum m_EnumValue;
}
