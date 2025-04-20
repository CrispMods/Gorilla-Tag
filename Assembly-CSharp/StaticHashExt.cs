using System;

// Token: 0x0200089F RID: 2207
public static class StaticHashExt
{
	// Token: 0x06003596 RID: 13718 RVA: 0x0005330D File Offset: 0x0005150D
	public static int GetStaticHash(this int i)
	{
		return StaticHash.Compute(i);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00053315 File Offset: 0x00051515
	public static int GetStaticHash(this uint u)
	{
		return StaticHash.Compute(u);
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x0005331D File Offset: 0x0005151D
	public static int GetStaticHash(this float f)
	{
		return StaticHash.Compute(f);
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x00053325 File Offset: 0x00051525
	public static int GetStaticHash(this long l)
	{
		return StaticHash.Compute(l);
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x0005332D File Offset: 0x0005152D
	public static int GetStaticHash(this double d)
	{
		return StaticHash.Compute(d);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x00053335 File Offset: 0x00051535
	public static int GetStaticHash(this bool b)
	{
		return StaticHash.Compute(b);
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x0005333D File Offset: 0x0005153D
	public static int GetStaticHash(this DateTime dt)
	{
		return StaticHash.Compute(dt);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x00053345 File Offset: 0x00051545
	public static int GetStaticHash(this string s)
	{
		return StaticHash.Compute(s);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x0005334D File Offset: 0x0005154D
	public static int GetStaticHash(this byte[] bytes)
	{
		return StaticHash.Compute(bytes);
	}
}
