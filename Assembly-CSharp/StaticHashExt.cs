using System;

// Token: 0x02000883 RID: 2179
public static class StaticHashExt
{
	// Token: 0x060034CA RID: 13514 RVA: 0x000FC649 File Offset: 0x000FA849
	public static int GetStaticHash(this int i)
	{
		return StaticHash.Compute(i);
	}

	// Token: 0x060034CB RID: 13515 RVA: 0x000FC651 File Offset: 0x000FA851
	public static int GetStaticHash(this uint u)
	{
		return StaticHash.Compute(u);
	}

	// Token: 0x060034CC RID: 13516 RVA: 0x000FC659 File Offset: 0x000FA859
	public static int GetStaticHash(this float f)
	{
		return StaticHash.Compute(f);
	}

	// Token: 0x060034CD RID: 13517 RVA: 0x000FC661 File Offset: 0x000FA861
	public static int GetStaticHash(this long l)
	{
		return StaticHash.Compute(l);
	}

	// Token: 0x060034CE RID: 13518 RVA: 0x000FC669 File Offset: 0x000FA869
	public static int GetStaticHash(this double d)
	{
		return StaticHash.Compute(d);
	}

	// Token: 0x060034CF RID: 13519 RVA: 0x000FC671 File Offset: 0x000FA871
	public static int GetStaticHash(this bool b)
	{
		return StaticHash.Compute(b);
	}

	// Token: 0x060034D0 RID: 13520 RVA: 0x000FC679 File Offset: 0x000FA879
	public static int GetStaticHash(this DateTime dt)
	{
		return StaticHash.Compute(dt);
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x000FC681 File Offset: 0x000FA881
	public static int GetStaticHash(this string s)
	{
		return StaticHash.Compute(s);
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x000FC689 File Offset: 0x000FA889
	public static int GetStaticHash(this byte[] bytes)
	{
		return StaticHash.Compute(bytes);
	}
}
