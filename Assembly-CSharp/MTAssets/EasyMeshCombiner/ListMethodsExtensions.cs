using System;
using System.Collections.Generic;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B19 RID: 2841
	public static class ListMethodsExtensions
	{
		// Token: 0x060046FB RID: 18171 RVA: 0x00150D04 File Offset: 0x0014EF04
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
