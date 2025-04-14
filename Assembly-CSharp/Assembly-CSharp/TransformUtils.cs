using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200088C RID: 2188
public static class TransformUtils
{
	// Token: 0x060034FC RID: 13564 RVA: 0x000FD38C File Offset: 0x000FB58C
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

	// Token: 0x060034FD RID: 13565 RVA: 0x000FD3C8 File Offset: 0x000FB5C8
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

	// Token: 0x060034FE RID: 13566 RVA: 0x000FD41C File Offset: 0x000FB61C
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

	// Token: 0x060034FF RID: 13567 RVA: 0x000FD468 File Offset: 0x000FB668
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

	// Token: 0x040037A0 RID: 14240
	private const string kFwdSlash = "/";
}
