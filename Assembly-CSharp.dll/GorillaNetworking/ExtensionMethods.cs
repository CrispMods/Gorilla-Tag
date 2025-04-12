using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AE6 RID: 2790
	public static class ExtensionMethods
	{
		// Token: 0x060045C9 RID: 17865 RVA: 0x00181984 File Offset: 0x0017FB84
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

		// Token: 0x060045CA RID: 17866 RVA: 0x0005CA50 File Offset: 0x0005AC50
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
