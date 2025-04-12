using System;
using System.Collections.Generic;

// Token: 0x02000848 RID: 2120
public static class DictRefTypeUtils
{
	// Token: 0x060033A2 RID: 13218 RVA: 0x00051311 File Offset: 0x0004F511
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
