using System;
using System.Collections.Generic;

// Token: 0x02000860 RID: 2144
public static class DictValueTypeUtils
{
	// Token: 0x06003452 RID: 13394 RVA: 0x00052751 File Offset: 0x00050951
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
