using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000889 RID: 2185
public static class TransformUtils
{
	// Token: 0x060034F0 RID: 13552 RVA: 0x000FCDC4 File Offset: 0x000FAFC4
	public static int ComputePathHashByInstance(Transform t)
	{
		if (t == null)
		{
			return 0;
		}
		int num = 0;
		Transform transform = t;
		while (transform != null)
		{
			num = StaticHash.Compute(num, transform.GetHashCode());
			transform = transform.parent;
		}
		return num;
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x000FCE00 File Offset: 0x000FB000
	public static Hash128 ComputePathHash(Transform t)
	{
		if (t == null)
		{
			return default(Hash128);
		}
		Hash128 result = default(Hash128);
		Transform transform = t;
		while (transform != null)
		{
			Hash128 hash = Hash128.Compute(transform.name);
			HashUtilities.AppendHash(ref hash, ref result);
			transform = transform.parent;
		}
		return result;
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x000FCE54 File Offset: 0x000FB054
	public static string GetScenePath(Transform t)
	{
		if (t == null)
		{
			return null;
		}
		string text = t.name;
		Transform parent = t.parent;
		while (parent != null)
		{
			text = parent.name + "/" + text;
			parent = parent.parent;
		}
		return text;
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x000FCEA0 File Offset: 0x000FB0A0
	public static string GetScenePathReverse(Transform t)
	{
		if (t == null)
		{
			return null;
		}
		string text = t.name;
		Transform parent = t.parent;
		Queue<string> queue = new Queue<string>(16);
		while (parent != null)
		{
			queue.Enqueue(parent.name);
			parent = parent.parent;
		}
		while (queue.Count > 0)
		{
			text = text + "/" + queue.Dequeue();
		}
		return text;
	}

	// Token: 0x0400378E RID: 14222
	private const string kFwdSlash = "/";
}
