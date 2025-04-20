using System;
using System.Diagnostics;

// Token: 0x02000143 RID: 323
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All)]
public class InlineAttribute : Attribute
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00035F4D File Offset: 0x0003414D
	public InlineAttribute(bool keepLabel = false, bool asGroup = false)
	{
		this.keepLabel = keepLabel;
		this.asGroup = asGroup;
	}

	// Token: 0x040009D4 RID: 2516
	public readonly bool keepLabel;

	// Token: 0x040009D5 RID: 2517
	public readonly bool asGroup;
}
