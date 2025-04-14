using System;
using System.ComponentModel;
using UnityEngine;

// Token: 0x02000207 RID: 519
public static class UnityTagsExt
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x0003FF8C File Offset: 0x0003E18C
	public static UnityTag ToTag(this string s)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return UnityTag.Invalid;
		}
		UnityTag result;
		if (!UnityTags.StringToTag.TryGetValue(s, out result))
		{
			return UnityTag.Invalid;
		}
		return result;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0003FFB5 File Offset: 0x0003E1B5
	public static void SetTag(this Component c, UnityTag tag)
	{
		if (c == null)
		{
			return;
		}
		if (tag == UnityTag.Invalid)
		{
			throw new InvalidEnumArgumentException("tag");
		}
		c.tag = UnityTags.StringValues[(int)tag];
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0003FFDD File Offset: 0x0003E1DD
	public static void SetTag(this GameObject g, UnityTag tag)
	{
		if (g == null)
		{
			return;
		}
		if (tag == UnityTag.Invalid)
		{
			throw new InvalidEnumArgumentException("tag");
		}
		g.tag = UnityTags.StringValues[(int)tag];
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00040005 File Offset: 0x0003E205
	public static bool TryGetTag(this GameObject g, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(g == null) && UnityTags.StringToTag.TryGetValue(g.tag, out tag);
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00040026 File Offset: 0x0003E226
	public static bool TryGetTag(this Component c, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(c == null) && UnityTags.StringToTag.TryGetValue(c.tag, out tag);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00040047 File Offset: 0x0003E247
	public static bool CompareTag(this GameObject g, UnityTag tag)
	{
		if (g == null)
		{
			return false;
		}
		if (tag == UnityTag.Invalid)
		{
			throw new InvalidEnumArgumentException("tag");
		}
		return g.CompareTag(UnityTags.StringValues[(int)tag]);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00040070 File Offset: 0x0003E270
	public static bool CompareTag(this Component c, UnityTag tag)
	{
		if (c == null)
		{
			return false;
		}
		if (tag == UnityTag.Invalid)
		{
			throw new InvalidEnumArgumentException("tag");
		}
		return c.CompareTag(UnityTags.StringValues[(int)tag]);
	}
}
