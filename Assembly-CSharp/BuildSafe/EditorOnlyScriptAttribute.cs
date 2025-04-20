using System;
using System.Diagnostics;

namespace BuildSafe
{
	// Token: 0x02000A52 RID: 2642
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Class)]
	public class EditorOnlyScriptAttribute : Attribute
	{
	}
}
