using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A53 RID: 2643
	internal static class EditorOnlyScripts
	{
		// Token: 0x06004246 RID: 16966 RVA: 0x00030607 File Offset: 0x0002E807
		[Conditional("UNITY_EDITOR")]
		public static void Cleanup(GameObject[] rootObjects, bool force = false)
		{
		}
	}
}
