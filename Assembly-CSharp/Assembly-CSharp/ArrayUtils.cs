using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200082B RID: 2091
public static class ArrayUtils
{
	// Token: 0x06003318 RID: 13080 RVA: 0x000F480E File Offset: 0x000F2A0E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int BinarySearch<T>(this T[] array, T value) where T : IComparable<T>
	{
		return Array.BinarySearch<T>(array, 0, array.Length, value);
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000F481B File Offset: 0x000F2A1B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty<T>(this T[] array)
	{
		return array == null || array.Length == 0;
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x000F4827 File Offset: 0x000F2A27
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty<T>(this List<T> list)
	{
		return list == null || list.Count == 0;
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x000F4838 File Offset: 0x000F2A38
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(this T[] array, int from, int to)
	{
		T t = array[from];
		T t2 = array[to];
		array[to] = t;
		array[from] = t2;
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000F4870 File Offset: 0x000F2A70
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Swap<T>(this List<T> list, int from, int to)
	{
		T value = list[from];
		T value2 = list[to];
		list[to] = value;
		list[from] = value2;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x000F48AC File Offset: 0x000F2AAC
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

	// Token: 0x0600331E RID: 13086 RVA: 0x000F48EE File Offset: 0x000F2AEE
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

	// Token: 0x0600331F RID: 13087 RVA: 0x000F490C File Offset: 0x000F2B0C
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

	// Token: 0x06003320 RID: 13088 RVA: 0x000F4948 File Offset: 0x000F2B48
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
