using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AE6 RID: 2790
	public static class ExtensionMethods
	{
		// Token: 0x060045C9 RID: 17865 RVA: 0x0014BB10 File Offset: 0x00149D10
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

		// Token: 0x060045CA RID: 17866 RVA: 0x0014BB4C File Offset: 0x00149D4C
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
