using System;
using System.ComponentModel;
using UnityEngine;

// Token: 0x02000212 RID: 530
public static class UnityTagsExt
{
	// Token: 0x06000C64 RID: 3172 RVA: 0x0009E6DC File Offset: 0x0009C8DC
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

	// Token: 0x06000C65 RID: 3173 RVA: 0x00038A3B File Offset: 0x00036C3B
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

	// Token: 0x06000C66 RID: 3174 RVA: 0x00038A63 File Offset: 0x00036C63
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

	// Token: 0x06000C67 RID: 3175 RVA: 0x00038A8B File Offset: 0x00036C8B
	public static bool TryGetTag(this GameObject g, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(g == null) && UnityTags.StringToTag.TryGetValue(g.tag, out tag);
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00038AAC File Offset: 0x00036CAC
	public static bool TryGetTag(this Component c, out UnityTag tag)
	{
		tag = UnityTag.Invalid;
		return !(c == null) && UnityTags.StringToTag.TryGetValue(c.tag, out tag);
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00038ACD File Offset: 0x00036CCD
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

	// Token: 0x06000C6A RID: 3178 RVA: 0x00038AF6 File Offset: 0x00036CF6
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
