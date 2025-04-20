using System;
using System.Collections.Generic;

// Token: 0x02000861 RID: 2145
public static class DictArrayUtils
{
	// Token: 0x06003453 RID: 13395 RVA: 0x00052772 File Offset: 0x00050972
	public static void TryGetOrAddList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, out List<TValue> list, int capacity)
	{
		if (dict.TryGetValue(key, out list) && list != null)
		{
			return;
		}
		list = new List<TValue>(capacity);
		dict.Add(key, list);
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x00052794 File Offset: 0x00050994
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
