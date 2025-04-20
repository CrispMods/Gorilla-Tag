using System;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class LowEffortZone : GorillaTriggerBox
{
	// Token: 0x06000C94 RID: 3220 RVA: 0x00038D1F File Offset: 0x00036F1F
	private void Awake()
	{
		if (this.triggerOnAwake)
		{
			this.OnBoxTriggered();
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0009FD3C File Offset: 0x0009DF3C
	public override void OnBoxTriggered()
	{
		for (int i = 0; i < this.objectsToEnable.Length; i++)
		{
			if (this.objectsToEnable[i] != null)
			{
				this.objectsToEnable[i].SetActive(true);
			}
		}
		for (int j = 0; j < this.objectsToDisable.Length; j++)
		{
			if (this.objectsToDisable[j] != null)
			{
				this.objectsToDisable[j].SetActive(false);
			}
		}
	}

	// Token: 0x04000FD3 RID: 4051
	public GameObject[] objectsToEnable;

	// Token: 0x04000FD4 RID: 4052
	public GameObject[] objectsToDisable;

	// Token: 0x04000FD5 RID: 4053
	public bool triggerOnAwake;
}
