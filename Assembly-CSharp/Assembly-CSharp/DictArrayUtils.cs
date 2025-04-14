using System;
using System.Collections.Generic;

// Token: 0x0200084A RID: 2122
public static class DictArrayUtils
{
	// Token: 0x060033A4 RID: 13220 RVA: 0x000F6B6E File Offset: 0x000F4D6E
	public static void TryGetOrAddList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dict, TKey key, out List<TValue> list, int capacity)
	{
		if (dict.TryGetValue(key, out list) && list != null)
		{
			return;
		}
		list = new List<TValue>(capacity);
		dict.Add(key, list);
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x000F6B90 File Offset: 0x000F4D90
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
