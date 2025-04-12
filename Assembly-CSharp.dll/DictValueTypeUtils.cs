using System;
using System.Collections.Generic;

// Token: 0x02000849 RID: 2121
public static class DictValueTypeUtils
{
	// Token: 0x060033A3 RID: 13219 RVA: 0x00051343 File Offset: 0x0004F543
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
