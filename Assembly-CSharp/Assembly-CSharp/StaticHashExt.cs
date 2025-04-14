using System;

// Token: 0x02000886 RID: 2182
public static class StaticHashExt
{
	// Token: 0x060034D6 RID: 13526 RVA: 0x000FCC11 File Offset: 0x000FAE11
	public static int GetStaticHash(this int i)
	{
		return StaticHash.Compute(i);
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x000FCC19 File Offset: 0x000FAE19
	public static int GetStaticHash(this uint u)
	{
		return StaticHash.Compute(u);
	}

	// Token: 0x060034D8 RID: 13528 RVA: 0x000FCC21 File Offset: 0x000FAE21
	public static int GetStaticHash(this float f)
	{
		return StaticHash.Compute(f);
	}

	// Token: 0x060034D9 RID: 13529 RVA: 0x000FCC29 File Offset: 0x000FAE29
	public static int GetStaticHash(this long l)
	{
		return StaticHash.Compute(l);
	}

	// Token: 0x060034DA RID: 13530 RVA: 0x000FCC31 File Offset: 0x000FAE31
	public static int GetStaticHash(this double d)
	{
		return StaticHash.Compute(d);
	}

	// Token: 0x060034DB RID: 13531 RVA: 0x000FCC39 File Offset: 0x000FAE39
	public static int GetStaticHash(this bool b)
	{
		return StaticHash.Compute(b);
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x000FCC41 File Offset: 0x000FAE41
	public static int GetStaticHash(this DateTime dt)
	{
		return StaticHash.Compute(dt);
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x000FCC49 File Offset: 0x000FAE49
	public static int GetStaticHash(this string s)
	{
		return StaticHash.Compute(s);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x000FCC51 File Offset: 0x000FAE51
	public static int GetStaticHash(this byte[] bytes)
	{
		return StaticHash.Compute(bytes);
	}
}
