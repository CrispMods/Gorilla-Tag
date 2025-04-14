using System;
using UnityEngine;

// Token: 0x020006D3 RID: 1747
[Serializable]
public struct StringEnum<TEnum> where TEnum : struct, Enum
{
	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06002B71 RID: 11121 RVA: 0x000D6368 File Offset: 0x000D4568
	public TEnum Value
	{
		get
		{
			return this.m_EnumValue;
		}
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x000D6370 File Offset: 0x000D4570
	public static implicit operator StringEnum<TEnum>(TEnum e)
	{
		return new StringEnum<TEnum>
		{
			m_EnumValue = e
		};
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x000D6368 File Offset: 0x000D4568
	public static implicit operator TEnum(StringEnum<TEnum> se)
	{
		return se.m_EnumValue;
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x000D638E File Offset: 0x000D458E
	public static bool operator ==(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return left.m_EnumValue.Equals(right.m_EnumValue);
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x000D63AD File Offset: 0x000D45AD
	public static bool operator !=(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return !(left == right);
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x000D63BC File Offset: 0x000D45BC
	public override bool Equals(object obj)
	{
		if (obj is StringEnum<TEnum>)
		{
			StringEnum<TEnum> stringEnum = (StringEnum<TEnum>)obj;
			return this.m_EnumValue.Equals(stringEnum.m_EnumValue);
		}
		return false;
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x000D63F6 File Offset: 0x000D45F6
	public override int GetHashCode()
	{
		return this.m_EnumValue.GetHashCode();
	}

	// Token: 0x040030B1 RID: 12465
	[SerializeField]
	private TEnum m_EnumValue;
}
