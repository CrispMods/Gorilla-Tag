using System;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class Flashlight : MonoBehaviour
{
	// Token: 0x060012E4 RID: 4836 RVA: 0x000B3458 File Offset: 0x000B1658
	private void LateUpdate()
	{
		for (int i = 0; i < this.lightVolume.transform.childCount; i++)
		{
			this.lightVolume.transform.GetChild(i).rotation = Quaternion.LookRotation((this.lightVolume.transform.GetChild(i).position - Camera.main.transform.position).normalized);
		}
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x000B34D0 File Offset: 0x000B16D0
	public void ToggleFlashlight()
	{
		this.lightVolume.SetActive(!this.lightVolume.activeSelf);
		this.spotlight.enabled = !this.spotlight.enabled;
		this.bulbGlow.SetActive(this.lightVolume.activeSelf);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x0003CF27 File Offset: 0x0003B127
	public void EnableFlashlight(bool doEnable)
	{
		this.lightVolume.SetActive(doEnable);
		this.spotlight.enabled = doEnable;
		this.bulbGlow.SetActive(doEnable);
	}

	// Token: 0x040014CC RID: 5324
	public GameObject lightVolume;

	// Token: 0x040014CD RID: 5325
	public Light spotlight;

	// Token: 0x040014CE RID: 5326
	public GameObject bulbGlow;
}
