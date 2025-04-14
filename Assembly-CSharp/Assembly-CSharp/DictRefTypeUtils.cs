using System;
using System.Collections.Generic;

// Token: 0x02000848 RID: 2120
public static class DictRefTypeUtils
{
	// Token: 0x060033A2 RID: 13218 RVA: 0x000F6B1B File Offset: 0x000F4D1B
	public static void TryGetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, out TValue value) where TValue : class, new()
	{
		if (dict.TryGetValue(key, out value) && value != null)
		{
			return;
		}
		value = Activator.CreateInstance<TValue>();
		dict.Add(key, value);
	}
}
