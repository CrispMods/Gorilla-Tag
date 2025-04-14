using System;
using System.Diagnostics;

// Token: 0x02000139 RID: 313
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All)]
public class InlineAttribute : Attribute
{
	// Token: 0x0600082C RID: 2092 RVA: 0x0002CED8 File Offset: 0x0002B0D8
	public InlineAttribute(bool keepLabel = false, bool asGroup = false)
	{
		this.keepLabel = keepLabel;
		this.asGroup = asGroup;
	}

	// Token: 0x04000991 RID: 2449
	public readonly bool keepLabel;

	// Token: 0x04000992 RID: 2450
	public readonly bool asGroup;
}
