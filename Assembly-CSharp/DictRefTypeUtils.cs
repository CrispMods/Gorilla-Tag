using System;
using System.Collections.Generic;

// Token: 0x0200085F RID: 2143
public static class DictRefTypeUtils
{
	// Token: 0x06003451 RID: 13393 RVA: 0x0005271F File Offset: 0x0005091F
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
