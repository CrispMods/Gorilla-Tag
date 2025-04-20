using System;

// Token: 0x020001A3 RID: 419
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DevInspectorColor : Attribute
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003724B File Offset: 0x0003544B
	public string Color { get; }

	// Token: 0x06000A30 RID: 2608 RVA: 0x00037253 File Offset: 0x00035453
	public DevInspectorColor(string color)
	{
		this.Color = color;
	}
}
