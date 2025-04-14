using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A28 RID: 2600
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class)]
	public class EditorOnlyScriptAttribute : Attribute
	{
	}
}
