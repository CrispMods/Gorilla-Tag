using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B8 RID: 1720
[Serializable]
public struct GTSturdyEnum<TEnum> : ISerializationCallbackReceiver where TEnum : struct, Enum
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06002A8F RID: 10895 RVA: 0x000D4101 File Offset: 0x000D2301
	// (set) Token: 0x06002A90 RID: 10896 RVA: 0x000D4109 File Offset: 0x000D2309
	public TEnum Value { readonly get; private set; }

	// Token: 0x06002A91 RID: 10897 RVA: 0x000D4114 File Offset: 0x000D2314
	public static implicit operator GTSturdyEnum<TEnum>(TEnum value)
	{
		return new GTSturdyEnum<TEnum>
		{
			Value = value
		};
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000D4132 File Offset: 0x000D2332
	public static implicit operator TEnum(GTSturdyEnum<TEnum> sturdyEnum)
	{
		return sturdyEnum.Value;
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x000D413C File Offset: 0x000D233C
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

	// Token: 0x06002A94 RID: 10900 RVA: 0x000D4284 File Offset: 0x000D2484
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

	// Token: 0x0400301B RID: 12315
	[SerializeField]
	private GTSturdyEnum<TEnum>.EnumPair[] m_stringValuePairs;

	// Token: 0x020006B9 RID: 1721
	[Serializable]
	private struct EnumPair
	{
		// Token: 0x0400301C RID: 12316
		public string Name;

		// Token: 0x0400301D RID: 12317
		public TEnum FallbackValue;
	}
}
