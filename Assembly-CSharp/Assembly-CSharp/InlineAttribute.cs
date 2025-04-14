using System;
using System.Diagnostics;

// Token: 0x02000139 RID: 313
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All)]
public class InlineAttribute : Attribute
{
	// Token: 0x0600082E RID: 2094 RVA: 0x0002D1FC File Offset: 0x0002B3FC
	public InlineAttribute(bool keepLabel = false, bool asGroup = false)
	{
		this.keepLabel = keepLabel;
		this.asGroup = asGroup;
	}

	// Token: 0x04000992 RID: 2450
	public readonly bool keepLabel;

	// Token: 0x04000993 RID: 2451
	public readonly bool asGroup;
}
