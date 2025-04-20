using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000842 RID: 2114
public static class ArrayUtils
{
	// Token: 0x060033C7 RID: 13255 RVA: 0x000520D7 File Offset: 0x000502D7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int BinarySearch<T>(this T[] array, T value) where T : IComparable<T>
	{
		return Array.BinarySearch<T>(array, 0, array.Length, value);
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x000520E4 File Offset: 0x000502E4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty<T>(this T[] array)
	{
		return array == null || array.Length == 0;
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x000520F0 File Offset: 0x000502F0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty<T>(this List<T> list)
	{
		return list == null || list.Count == 0;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x0013C0B4 File Offset: 0x0013A2B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(this T[] array, int from, int to)
	{
		T t = array[from];
		T t2 = array[to];
		array[to] = t;
		array[from] = t2;
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x0013C0EC File Offset: 0x0013A2EC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(this List<T> list, int from, int to)
	{
		T value = list[from];
		T value2 = list[to];
		list[to] = value;
		list[from] = value2;
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0013C128 File Offset: 0x0013A328
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T[] Clone<T>(T[] source)
	{
		if (source == null)
		{
			return null;
		}
		if (source.Length == 0)
		{
			return Array.Empty<T>();
		}
		T[] array = new T[source.Length];
		for (int i = 0; i < source.Length; i++)
		{
			array[i] = source[i];
		}
		return array;
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x00052100 File Offset: 0x00050300
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static List<T> Clone<T>(List<T> source)
	{
		if (source == null)
		{
			return null;
		}
		if (source.Count == 0)
		{
			return new List<T>();
		}
		return new List<T>(source);
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0013C16C File Offset: 0x0013A36C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int IndexOfRef<T>(this T[] array, T value) where T : class
	{
		if (array == null || array.Length == 0)
		{
			return -1;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == value)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x0013C1A8 File Offset: 0x0013A3A8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int IndexOfRef<T>(this List<T> list, T value) where T : class
	{
		if (list == null || list.Count == 0)
		{
			return -1;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == value)
			{
				return i;
			}
		}
		return -1;
	}
}
