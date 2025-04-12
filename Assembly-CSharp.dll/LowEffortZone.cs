using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class LowEffortZone : GorillaTriggerBox
{
	// Token: 0x06000C4B RID: 3147 RVA: 0x00037A5F File Offset: 0x00035C5F
	private void Awake()
	{
		if (this.triggerOnAwake)
		{
			this.OnBoxTriggered();
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0009D4B0 File Offset: 0x0009B6B0
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

	// Token: 0x04000F8E RID: 3982
	public GameObject[] objectsToEnable;

	// Token: 0x04000F8F RID: 3983
	public GameObject[] objectsToDisable;

	// Token: 0x04000F90 RID: 3984
	public bool triggerOnAwake;
}
