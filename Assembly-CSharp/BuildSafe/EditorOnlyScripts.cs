using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A26 RID: 2598
	internal static class EditorOnlyScripts
	{
		// Token: 0x06004101 RID: 16641 RVA: 0x000023F4 File Offset: 0x000005F4
		[Conditional("UNITY_EDITOR")]
		public static void Cleanup(GameObject[] rootObjects, bool force = false)
		{
		}
	}
}
