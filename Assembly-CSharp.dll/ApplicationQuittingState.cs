using System;
using UnityEngine;

// Token: 0x02000132 RID: 306
public static class ApplicationQuittingState
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600081D RID: 2077 RVA: 0x00034C62 File Offset: 0x00032E62
	// (set) Token: 0x0600081E RID: 2078 RVA: 0x00034C69 File Offset: 0x00032E69
	public static bool IsQuitting { get; private set; }

	// Token: 0x0600081F RID: 2079 RVA: 0x00034C71 File Offset: 0x00032E71
	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		Application.quitting += ApplicationQuittingState.HandleApplicationQuitting;
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00034C84 File Offset: 0x00032E84
	private static void HandleApplicationQuitting()
	{
		ApplicationQuittingState.IsQuitting = true;
	}
}
