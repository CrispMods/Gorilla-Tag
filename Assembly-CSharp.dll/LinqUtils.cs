using System;
using System.Collections.Generic;

// Token: 0x02000856 RID: 2134
public static class LinqUtils
{
	// Token: 0x060033BB RID: 13243 RVA: 0x00051442 File Offset: 0x0004F642
	public static IEnumerable<TResult> SelectManyNullSafe<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, IEnumerable<TResult>> selector)
	{
		if (sources == null)
		{
			yield break;
		}
		if (selector == null)
		{
			yield break;
		}
		foreach (TSource tsource in sources)
		{
			if (tsource != null)
			{
				IEnumerable<TResult> enumerable = selector(tsource);
				foreach (TResult tresult in enumerable)
				{
					if (tresult != null)
					{
						yield return tresult;
					}
				}
				IEnumerator<TResult> enumerator2 = null;
			}
		}
		IEnumerator<TSource> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x00051459 File Offset: 0x0004F659
	public static IEnumerable<TSource> DistinctBy<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		HashSet<TResult> set = new HashSet<TResult>();
		foreach (TSource tsource in source)
		{
			TResult item = selector(tsource);
			if (set.Add(item))
			{
				yield return tsource;
			}
		}
		IEnumerator<TSource> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x00138AA4 File Offset: 0x00136CA4
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T obj in source)
		{
			action(obj);
		}
		return source;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x00051470 File Offset: 0x0004F670
	public static T[] AsArray<T>(this IEnumerable<T> source)
	{
		return (T[])source;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x00051478 File Offset: 0x0004F678
	public static List<T> AsList<T>(this IEnumerable<T> source)
	{
		return (List<T>)source;
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x00138AF0 File Offset: 0x00136CF0
	public static IList<T> Transform<T>(this IList<T> list, Func<T, T> action)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = action(list[i]);
		}
		return list;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x00051480 File Offset: 0x0004F680
	public static IEnumerable<T> Self<T>(this T value)
	{
		yield return value;
		yield break;
	}
}
