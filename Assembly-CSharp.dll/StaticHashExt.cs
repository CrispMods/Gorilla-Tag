using System;

// Token: 0x02000886 RID: 2182
public static class StaticHashExt
{
	// Token: 0x060034D6 RID: 13526 RVA: 0x00051E00 File Offset: 0x00050000
	public static int GetStaticHash(this int i)
	{
		return StaticHash.Compute(i);
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x00051E08 File Offset: 0x00050008
	public static int GetStaticHash(this uint u)
	{
		return StaticHash.Compute(u);
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x00051E10 File Offset: 0x00050010
	public static int GetStaticHash(this float f)
	{
		return StaticHash.Compute(f);
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x00051E18 File Offset: 0x00050018
	public static int GetStaticHash(this long l)
	{
		return StaticHash.Compute(l);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x00051E20 File Offset: 0x00050020
	public static int GetStaticHash(this double d)
	{
		return StaticHash.Compute(d);
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x00051E28 File Offset: 0x00050028
	public static int GetStaticHash(this bool b)
	{
		return StaticHash.Compute(b);
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x00051E30 File Offset: 0x00050030
	public static int GetStaticHash(this DateTime dt)
	{
		return StaticHash.Compute(dt);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x00051E38 File Offset: 0x00050038
	public static int GetStaticHash(this string s)
	{
		return StaticHash.Compute(s);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x00051E40 File Offset: 0x00050040
	public static int GetStaticHash(this byte[] bytes)
	{
		return StaticHash.Compute(bytes);
	}
}
