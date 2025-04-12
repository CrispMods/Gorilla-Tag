using System;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
[Serializable]
public struct StringEnum<TEnum> where TEnum : struct, Enum
{
	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06002B79 RID: 11129 RVA: 0x0004CA09 File Offset: 0x0004AC09
	public TEnum Value
	{
		get
		{
			return this.m_EnumValue;
		}
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x0011CE8C File Offset: 0x0011B08C
	public static implicit operator StringEnum<TEnum>(TEnum e)
	{
		return new StringEnum<TEnum>
		{
			m_EnumValue = e
		};
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x0004CA09 File Offset: 0x0004AC09
	public static implicit operator TEnum(StringEnum<TEnum> se)
	{
		return se.m_EnumValue;
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x0004CA11 File Offset: 0x0004AC11
	public static bool operator ==(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return left.m_EnumValue.Equals(right.m_EnumValue);
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x0004CA30 File Offset: 0x0004AC30
	public static bool operator !=(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return !(left == right);
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x0011CEAC File Offset: 0x0011B0AC
	public override bool Equals(object obj)
	{
		if (obj is StringEnum<TEnum>)
		{
			StringEnum<TEnum> stringEnum = (StringEnum<TEnum>)obj;
			return this.m_EnumValue.Equals(stringEnum.m_EnumValue);
		}
		return false;
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x0004CA3C File Offset: 0x0004AC3C
	public override int GetHashCode()
	{
		return this.m_EnumValue.GetHashCode();
	}

	// Token: 0x040030B7 RID: 12471
	[SerializeField]
	private TEnum m_EnumValue;
}
