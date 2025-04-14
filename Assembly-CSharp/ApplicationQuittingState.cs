using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public static class ApplicationQuittingState
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600081B RID: 2075 RVA: 0x0002CAF3 File Offset: 0x0002ACF3
	// (set) Token: 0x0600081C RID: 2076 RVA: 0x0002CAFA File Offset: 0x0002ACFA
	public static bool IsQuitting { get; private set; }

	// Token: 0x0600081D RID: 2077 RVA: 0x0002CB02 File Offset: 0x0002AD02
	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		Application.quitting += ApplicationQuittingState.HandleApplicationQuitting;
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0002CB15 File Offset: 0x0002AD15
	private static void HandleApplicationQuitting()
	{
		ApplicationQuittingState.IsQuitting = true;
	}
}
