using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class DevInspectorManager : MonoBehaviour
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000A3C RID: 2620 RVA: 0x000372E2 File Offset: 0x000354E2
	public static DevInspectorManager instance
	{
		get
		{
			if (DevInspectorManager._instance == null)
			{
				DevInspectorManager._instance = UnityEngine.Object.FindObjectOfType<DevInspectorManager>();
			}
			return DevInspectorManager._instance;
		}
	}

	// Token: 0x04000C92 RID: 3218
	private static DevInspectorManager _instance;
}
