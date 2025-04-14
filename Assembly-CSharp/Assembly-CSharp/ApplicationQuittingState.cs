using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public static class ApplicationQuittingState
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x0002CE17 File Offset: 0x0002B017
	// (set) Token: 0x0600081E RID: 2078 RVA: 0x0002CE1E File Offset: 0x0002B01E
	public static bool IsQuitting { get; private set; }

	// Token: 0x0600081F RID: 2079 RVA: 0x0002CE26 File Offset: 0x0002B026
	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		Application.quitting += ApplicationQuittingState.HandleApplicationQuitting;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0002CE39 File Offset: 0x0002B039
	private static void HandleApplicationQuitting()
	{
		ApplicationQuittingState.IsQuitting = true;
	}
}
