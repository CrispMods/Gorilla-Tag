using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public class LowEffortZone : GorillaTriggerBox
{
	// Token: 0x06000C49 RID: 3145 RVA: 0x000418E0 File Offset: 0x0003FAE0
	private void Awake()
	{
		if (this.triggerOnAwake)
		{
			this.OnBoxTriggered();
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000418F0 File Offset: 0x0003FAF0
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

	// Token: 0x04000F8D RID: 3981
	public GameObject[] objectsToEnable;

	// Token: 0x04000F8E RID: 3982
	public GameObject[] objectsToDisable;

	// Token: 0x04000F8F RID: 3983
	public bool triggerOnAwake;
}
