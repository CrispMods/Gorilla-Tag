using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006CC RID: 1740
[Serializable]
public struct GTSturdyEnum<TEnum> : ISerializationCallbackReceiver where TEnum : struct, Enum
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002B1D RID: 11037 RVA: 0x0004D179 File Offset: 0x0004B379
	// (set) Token: 0x06002B1E RID: 11038 RVA: 0x0004D181 File Offset: 0x0004B381
	public TEnum Value { readonly get; private set; }

	// Token: 0x06002B1F RID: 11039 RVA: 0x0011FF28 File Offset: 0x0011E128
	public static implicit operator GTSturdyEnum<TEnum>(TEnum value)
	{
		return new GTSturdyEnum<TEnum>
		{
			Value = value
		};
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x0004D18A File Offset: 0x0004B38A
	public static implicit operator TEnum(GTSturdyEnum<TEnum> sturdyEnum)
	{
		return sturdyEnum.Value;
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x0011FF48 File Offset: 0x0011E148
	public void OnBeforeSerialize()
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		if (!shared.IsBitMaskCompatible)
		{
			this.m_stringValuePairs = new GTSturdyEnum<TEnum>.EnumPair[1];
			GTSturdyEnum<TEnum>.EnumPair[] stringValuePairs = this.m_stringValuePairs;
			int num = 0;
			GTSturdyEnum<TEnum>.EnumPair enumPair = default(GTSturdyEnum<TEnum>.EnumPair);
			TEnum value = this.Value;
			enumPair.Name = value.ToString();
			enumPair.FallbackValue = this.Value;
			stringValuePairs[num] = enumPair;
			return;
		}
		long num2 = Convert.ToInt64(this.Value);
		if (num2 == 0L)
		{
			GTSturdyEnum<TEnum>.EnumPair[] array = new GTSturdyEnum<TEnum>.EnumPair[1];
			int num3 = 0;
			GTSturdyEnum<TEnum>.EnumPair enumPair = default(GTSturdyEnum<TEnum>.EnumPair);
			TEnum value = this.Value;
			enumPair.Name = value.ToString();
			enumPair.FallbackValue = this.Value;
			array[num3] = enumPair;
			this.m_stringValuePairs = array;
			return;
		}
		List<GTSturdyEnum<TEnum>.EnumPair> list = new List<GTSturdyEnum<TEnum>.EnumPair>(shared.Values.Length);
		for (int i = 0; i < shared.Values.Length; i++)
		{
			long num4 = shared.LongValues[i];
			if (num4 != 0L && (num2 & num4) == num4)
			{
				TEnum fallbackValue = shared.Values[i];
				list.Add(new GTSturdyEnum<TEnum>.EnumPair
				{
					Name = fallbackValue.ToString(),
					FallbackValue = fallbackValue
				});
			}
		}
		this.m_stringValuePairs = list.ToArray();
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x00120090 File Offset: 0x0011E290
	public void OnAfterDeserialize()
	{
		EnumData<TEnum> shared = EnumData<TEnum>.Shared;
		if (this.m_stringValuePairs == null || this.m_stringValuePairs.Length == 0)
		{
			if (shared.IsBitMaskCompatible)
			{
				this.Value = (TEnum)((object)Enum.ToObject(typeof(TEnum), 0L));
				return;
			}
			this.Value = default(TEnum);
			return;
		}
		else
		{
			if (shared.IsBitMaskCompatible)
			{
				long num = 0L;
				foreach (GTSturdyEnum<TEnum>.EnumPair enumPair in this.m_stringValuePairs)
				{
					TEnum key;
					long num2;
					if (shared.NameToEnum.TryGetValue(enumPair.Name, out key))
					{
						num |= shared.EnumToLong[key];
					}
					else if (shared.EnumToLong.TryGetValue(enumPair.FallbackValue, out num2))
					{
						num |= num2;
					}
				}
				this.Value = (TEnum)((object)Enum.ToObject(typeof(TEnum), num));
				return;
			}
			GTSturdyEnum<TEnum>.EnumPair enumPair2 = this.m_stringValuePairs[0];
			TEnum value;
			if (shared.NameToEnum.TryGetValue(enumPair2.Name, out value))
			{
				this.Value = value;
				return;
			}
			this.Value = enumPair2.FallbackValue;
			return;
		}
	}

	// Token: 0x040030B2 RID: 12466
	[SerializeField]
	private GTSturdyEnum<TEnum>.EnumPair[] m_stringValuePairs;

	// Token: 0x020006CD RID: 1741
	[Serializable]
	private struct EnumPair
	{
		// Token: 0x040030B3 RID: 12467
		public string Name;

		// Token: 0x040030B4 RID: 12468
		public TEnum FallbackValue;
	}
}
