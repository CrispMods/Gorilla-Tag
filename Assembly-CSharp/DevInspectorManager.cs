using System;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class DevInspectorManager : MonoBehaviour
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0003728F File Offset: 0x0003548F
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

	// Token: 0x04000C4C RID: 3148
	private static DevInspectorManager _instance;
}
