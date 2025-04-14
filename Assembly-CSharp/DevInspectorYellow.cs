using System;

// Token: 0x0200019B RID: 411
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DevInspectorYellow : DevInspectorColor
{
	// Token: 0x060009E7 RID: 2535 RVA: 0x0003720F File Offset: 0x0003540F
	public DevInspectorYellow() : base("#ff5")
	{
	}
}
