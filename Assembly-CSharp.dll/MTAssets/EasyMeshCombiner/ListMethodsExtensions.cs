using System;
using System.Collections.Generic;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B1C RID: 2844
	public static class ListMethodsExtensions
	{
		// Token: 0x06004707 RID: 18183 RVA: 0x00186564 File Offset: 0x00184764
		public static void RemoveAllNullItems<T>(this List<T> list)
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (list[i] == null)
				{
					list.RemoveAt(i);
				}
			}
		}
	}
}
