using System;
using System.Diagnostics;

// Token: 0x02000137 RID: 311
[Conditional("UNITY_EDITOR")]
public class DarkBoxAttribute : Attribute
{
	// Token: 0x06000829 RID: 2089 RVA: 0x0000224E File Offset: 0x0000044E
	public DarkBoxAttribute()
	{
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0002CEC9 File Offset: 0x0002B0C9
	public DarkBoxAttribute(bool withBorders)
	{
		this.withBorders = withBorders;
	}

	// Token: 0x04000990 RID: 2448
	public readonly bool withBorders;
}
