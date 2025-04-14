using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AE3 RID: 2787
	public static class ExtensionMethods
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x0014B548 File Offset: 0x00149748
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

		// Token: 0x060045BE RID: 17854 RVA: 0x0014B584 File Offset: 0x00149784
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
