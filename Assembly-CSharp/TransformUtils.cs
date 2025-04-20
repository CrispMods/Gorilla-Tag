using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A5 RID: 2213
public static class TransformUtils
{
	// Token: 0x060035BC RID: 13756 RVA: 0x0014377C File Offset: 0x0014197C
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

	// Token: 0x060035BD RID: 13757 RVA: 0x001437B8 File Offset: 0x001419B8
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

	// Token: 0x060035BE RID: 13758 RVA: 0x0014380C File Offset: 0x00141A0C
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

	// Token: 0x060035BF RID: 13759 RVA: 0x00143858 File Offset: 0x00141A58
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

	// Token: 0x0400384E RID: 14414
	private const string kFwdSlash = "/";
}
