using System;
using System.Diagnostics;

// Token: 0x02000137 RID: 311
[Conditional("UNITY_EDITOR")]
public class DarkBoxAttribute : Attribute
{
	// Token: 0x0600082B RID: 2091 RVA: 0x0002F68A File Offset: 0x0002D88A
	public DarkBoxAttribute()
	{
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x00034CC8 File Offset: 0x00032EC8
	public DarkBoxAttribute(bool withBorders)
	{
		this.withBorders = withBorders;
	}

	// Token: 0x04000991 RID: 2449
	public readonly bool withBorders;
}
