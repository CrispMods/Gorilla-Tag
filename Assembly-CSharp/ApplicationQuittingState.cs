using System;
using UnityEngine;

// Token: 0x0200013C RID: 316
public static class ApplicationQuittingState
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x0600085F RID: 2143 RVA: 0x00035ED8 File Offset: 0x000340D8
	// (set) Token: 0x06000860 RID: 2144 RVA: 0x00035EDF File Offset: 0x000340DF
	public static bool IsQuitting { get; private set; }

	// Token: 0x06000861 RID: 2145 RVA: 0x00035EE7 File Offset: 0x000340E7
	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		Application.quitting += ApplicationQuittingState.HandleApplicationQuitting;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00035EFA File Offset: 0x000340FA
	private static void HandleApplicationQuitting()
	{
		ApplicationQuittingState.IsQuitting = true;
	}
}
