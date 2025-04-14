using System;

// Token: 0x02000198 RID: 408
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DevInspectorColor : Attribute
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x000371F8 File Offset: 0x000353F8
	public string Color { get; }

	// Token: 0x060009E4 RID: 2532 RVA: 0x00037200 File Offset: 0x00035400
	public DevInspectorColor(string color)
	{
		this.Color = color;
	}
}
