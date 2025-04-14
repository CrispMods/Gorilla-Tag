using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
[Serializable]
public struct GTSturdyEnum<TEnum> : ISerializationCallbackReceiver where TEnum : struct, Enum
{
	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000D3C81 File Offset: 0x000D1E81
	// (set) Token: 0x06002A88 RID: 10888 RVA: 0x000D3C89 File Offset: 0x000D1E89
	public TEnum Value { readonly get; private set; }

	// Token: 0x06002A89 RID: 10889 RVA: 0x000D3C94 File Offset: 0x000D1E94
	public static implicit operator GTSturdyEnum<TEnum>(TEnum value)
	{
		return new GTSturdyEnum<TEnum>
		{
			Value = value
		};
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x000D3CB2 File Offset: 0x000D1EB2
	public static implicit operator TEnum(GTSturdyEnum<TEnum> sturdyEnum)
	{
		return sturdyEnum.Value;
	}

	// Token: 0x06002A8B RID: 10891 RVA: 0x000D3CBC File Offset: 0x000D1EBC
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

	// Token: 0x06002A8C RID: 10892 RVA: 0x000D3E04 File Offset: 0x000D2004
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

	// Token: 0x04003015 RID: 12309
	[SerializeField]
	private GTSturdyEnum<TEnum>.EnumPair[] m_stringValuePairs;

	// Token: 0x020006B8 RID: 1720
	[Serializable]
	private struct EnumPair
	{
		// Token: 0x04003016 RID: 12310
		public string Name;

		// Token: 0x04003017 RID: 12311
		public TEnum FallbackValue;
	}
}
