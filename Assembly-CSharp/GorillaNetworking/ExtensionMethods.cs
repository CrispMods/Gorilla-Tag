using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000B10 RID: 2832
	public static class ExtensionMethods
	{
		// Token: 0x06004705 RID: 18181 RVA: 0x0018887C File Offset: 0x00186A7C
		public static void SafeInvoke<T>(this Action<T> action, T data)
		{
			try
			{
				if (action != null)
				{
					action(data);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("Failure invoking action: {0}", arg));
			}
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0005E44F File Offset: 0x0005C64F
		public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			if (dict.ContainsKey(key))
			{
				dict[key] = value;
				return;
			}
			dict.Add(key, value);
		}
	}
}
