using System;
using System.Collections.Generic;

// Token: 0x02000847 RID: 2119
public static class DictArrayUtils
{
	// Token: 0x06003398 RID: 13208 RVA: 0x000F65A6 File Offset: 0x000F47A6
	public static void TryGetOrAddList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, out List<TValue> list, int capacity)
	{
		if (dict.TryGetValue(key, out list) && list != null)
		{
			return;
		}
		list = new List<TValue>(capacity);
		dict.Add(key, list);
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x000F65C8 File Offset: 0x000F47C8
	public static void TryGetOrAddArray<TKey, TValue>(this Dictionary<TKey, TValue[]> dict, TKey key, out TValue[] array, int size)
	{
		if (dict.TryGetValue(key, out array) && array != null)
		{
			return;
		}
		array = new TValue[size];
		dict.Add(key, array);
	}
}
