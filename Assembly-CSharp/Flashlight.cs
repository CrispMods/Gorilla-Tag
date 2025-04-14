using System;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class Flashlight : MonoBehaviour
{
	// Token: 0x06001298 RID: 4760 RVA: 0x00059250 File Offset: 0x00057450
	private void LateUpdate()
	{
		for (int i = 0; i < this.lightVolume.transform.childCount; i++)
		{
			this.lightVolume.transform.GetChild(i).rotation = Quaternion.LookRotation((this.lightVolume.transform.GetChild(i).position - Camera.main.transform.position).normalized);
		}
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x000592C8 File Offset: 0x000574C8
	public void ToggleFlashlight()
	{
		this.lightVolume.SetActive(!this.lightVolume.activeSelf);
		this.spotlight.enabled = !this.spotlight.enabled;
		this.bulbGlow.SetActive(this.lightVolume.activeSelf);
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x0005931D File Offset: 0x0005751D
	public void EnableFlashlight(bool doEnable)
	{
		this.lightVolume.SetActive(doEnable);
		this.spotlight.enabled = doEnable;
		this.bulbGlow.SetActive(doEnable);
	}

	// Token: 0x04001484 RID: 5252
	public GameObject lightVolume;

	// Token: 0x04001485 RID: 5253
	public Light spotlight;

	// Token: 0x04001486 RID: 5254
	public GameObject bulbGlow;
}
