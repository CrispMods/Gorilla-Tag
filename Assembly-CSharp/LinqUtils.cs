using System;
using System.Collections.Generic;

// Token: 0x0200086D RID: 2157
public static class LinqUtils
{
	// Token: 0x0600346A RID: 13418 RVA: 0x00052850 File Offset: 0x00050A50
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

	// Token: 0x0600346B RID: 13419 RVA: 0x00052867 File Offset: 0x00050A67
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

	// Token: 0x0600346C RID: 13420 RVA: 0x0013DFFC File Offset: 0x0013C1FC
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T obj in source)
		{
			action(obj);
		}
		return source;
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x0005287E File Offset: 0x00050A7E
	public static T[] AsArray<T>(this IEnumerable<T> source)
	{
		return (T[])source;
	}

	// Token: 0x0600346E RID: 13422 RVA: 0x00052886 File Offset: 0x00050A86
	public static List<T> AsList<T>(this IEnumerable<T> source)
	{
		return (List<T>)source;
	}

	// Token: 0x0600346F RID: 13423 RVA: 0x0013E048 File Offset: 0x0013C248
	public static IList<T> Transform<T>(this IList<T> list, Func<T, T> action)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = action(list[i]);
		}
		return list;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x0005288E File Offset: 0x00050A8E
	public static IEnumerable<T> Self<T>(this T value)
	{
		yield return value;
		yield break;
	}
}
