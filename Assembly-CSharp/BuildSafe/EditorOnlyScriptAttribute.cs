using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A25 RID: 2597
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class)]
	public class EditorOnlyScriptAttribute : Attribute
	{
	}
}
