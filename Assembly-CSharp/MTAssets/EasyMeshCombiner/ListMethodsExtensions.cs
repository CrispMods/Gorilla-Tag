using System;
using System.Collections.Generic;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B46 RID: 2886
	public static class ListMethodsExtensions
	{
		// Token: 0x06004844 RID: 18500 RVA: 0x0018D4D8 File Offset: 0x0018B6D8
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
