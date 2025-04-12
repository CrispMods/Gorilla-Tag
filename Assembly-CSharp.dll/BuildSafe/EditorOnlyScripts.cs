using System;
using System.Diagnostics;
using UnityEngine;

namespace BuildSafe
{
	// Token: 0x02000A29 RID: 2601
	internal static class EditorOnlyScripts
	{
		// Token: 0x0600410D RID: 16653 RVA: 0x0002F75F File Offset: 0x0002D95F
		[Conditional("UNITY_EDITOR")]
		public static void Cleanup(GameObject[] rootObjects, bool force = false)
		{
		}
	}
}
