using System;
using System.Diagnostics;

// Token: 0x02000141 RID: 321
[Conditional("UNITY_EDITOR")]
public class DarkBoxAttribute : Attribute
{
	// Token: 0x0600086D RID: 2157 RVA: 0x00030532 File Offset: 0x0002E732
	public DarkBoxAttribute()
	{
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00035F3E File Offset: 0x0003413E
	public DarkBoxAttribute(bool withBorders)
	{
		this.withBorders = withBorders;
	}

	// Token: 0x040009D3 RID: 2515
	public readonly bool withBorders;
}
