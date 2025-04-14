using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class DevInspectorManager : MonoBehaviour
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x000375B3 File Offset: 0x000357B3
	public static DevInspectorManager instance
	{
		get
		{
			if (DevInspectorManager._instance == null)
			{
				DevInspectorManager._instance = Object.FindObjectOfType<DevInspectorManager>();
			}
			return DevInspectorManager._instance;
		}
	}

	// Token: 0x04000C4D RID: 3149
	private static DevInspectorManager _instance;
}
