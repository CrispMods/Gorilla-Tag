using System;
using System.Collections.Generic;

// Token: 0x02000845 RID: 2117
public static class DictRefTypeUtils
{
	// Token: 0x06003396 RID: 13206 RVA: 0x000F6553 File Offset: 0x000F4753
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
