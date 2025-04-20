using System;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
[Serializable]
public struct StringEnum<TEnum> where TEnum : struct, Enum
{
	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06002C07 RID: 11271 RVA: 0x0004DD4E File Offset: 0x0004BF4E
	public TEnum Value
	{
		get
		{
			return this.m_EnumValue;
		}
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x00121A44 File Offset: 0x0011FC44
	public static implicit operator StringEnum<TEnum>(TEnum e)
	{
		return new StringEnum<TEnum>
		{
			m_EnumValue = e
		};
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x0004DD4E File Offset: 0x0004BF4E
	public static implicit operator TEnum(StringEnum<TEnum> se)
	{
		return se.m_EnumValue;
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x0004DD56 File Offset: 0x0004BF56
	public static bool operator ==(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return left.m_EnumValue.Equals(right.m_EnumValue);
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x0004DD75 File Offset: 0x0004BF75
	public static bool operator !=(StringEnum<TEnum> left, StringEnum<TEnum> right)
	{
		return !(left == right);
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x00121A64 File Offset: 0x0011FC64
	public override bool Equals(object obj)
	{
		if (obj is StringEnum<TEnum>)
		{
			StringEnum<TEnum> stringEnum = (StringEnum<TEnum>)obj;
			return this.m_EnumValue.Equals(stringEnum.m_EnumValue);
		}
		return false;
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x0004DD81 File Offset: 0x0004BF81
	public override int GetHashCode()
	{
		return this.m_EnumValue.GetHashCode();
	}

	// Token: 0x0400314E RID: 12622
	[SerializeField]
	private TEnum m_EnumValue;
}
