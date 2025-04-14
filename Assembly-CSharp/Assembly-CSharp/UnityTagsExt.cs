using System;
using System.ComponentModel;
using UnityEngine;

// Token: 0x02000207 RID: 519
public static class UnityTagsExt
{
	// Token: 0x06000C1B RID: 3099 RVA: 0x000402D0 File Offset: 0x0003E4D0
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

	// Token: 0x06000C1C RID: 3100 RVA: 0x000402F9 File Offset: 0x0003E4F9
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

	// Token: 0x06000C1D RID: 3101 RVA: 0x00040321 File Offset: 0x0003E521
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

	// Token: 0x06000C1E RID: 3102 RVA: 0x00040349 File Offset: 0x0003E549
	public static bool TryGetTag(this GameObject g, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(g == null) && UnityTags.StringToTag.TryGetValue(g.tag, out tag);
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0004036A File Offset: 0x0003E56A
	public static bool TryGetTag(this Component c, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(c == null) && UnityTags.StringToTag.TryGetValue(c.tag, out tag);
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0004038B File Offset: 0x0003E58B
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

	// Token: 0x06000C21 RID: 3105 RVA: 0x000403B4 File Offset: 0x0003E5B4
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
