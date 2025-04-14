using System;

// Token: 0x02000198 RID: 408
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DevInspectorColor : Attribute
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0003751C File Offset: 0x0003571C
	public string Color { get; }

	// Token: 0x060009E6 RID: 2534 RVA: 0x00037524 File Offset: 0x00035724
	public DevInspectorColor(string color)
	{
		this.Color = color;
	}
}
