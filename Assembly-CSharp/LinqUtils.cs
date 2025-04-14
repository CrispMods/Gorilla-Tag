using System;
using System.Collections.Generic;

// Token: 0x02000853 RID: 2131
public static class LinqUtils
{
	// Token: 0x060033AF RID: 13231 RVA: 0x000F68FF File Offset: 0x000F4AFF
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

	// Token: 0x060033B0 RID: 13232 RVA: 0x000F6916 File Offset: 0x000F4B16
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

	// Token: 0x060033B1 RID: 13233 RVA: 0x000F6930 File Offset: 0x000F4B30
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T obj in source)
		{
			action(obj);
		}
		return source;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x000F697C File Offset: 0x000F4B7C
	public static T[] AsArray<T>(this IEnumerable<T> source)
	{
		return (T[])source;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x000F6984 File Offset: 0x000F4B84
	public static List<T> AsList<T>(this IEnumerable<T> source)
	{
		return (List<T>)source;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x000F698C File Offset: 0x000F4B8C
	public static IList<T> Transform<T>(this IList<T> list, Func<T, T> action)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = action(list[i]);
		}
		return list;
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x000F69BF File Offset: 0x000F4BBF
	public static IEnumerable<T> Self<T>(this T value)
	{
		yield return value;
		yield break;
	}
}
