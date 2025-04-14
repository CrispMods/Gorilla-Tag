using System;
using System.Collections.Generic;

// Token: 0x02000846 RID: 2118
public static class DictValueTypeUtils
{
	// Token: 0x06003397 RID: 13207 RVA: 0x000F6585 File Offset: 0x000F4785
	public static void TryGetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, out TValue value) where TValue : struct
	{
		if (dict.TryGetValue(key, out value))
		{
			return;
		}
		value = default(TValue);
		dict.Add(key, value);
	}
}
