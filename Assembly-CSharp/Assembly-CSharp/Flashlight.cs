using System;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class Flashlight : MonoBehaviour
{
	// Token: 0x0600129B RID: 4763 RVA: 0x000595D4 File Offset: 0x000577D4
	private void LateUpdate()
	{
		for (int i = 0; i < this.lightVolume.transform.childCount; i++)
		{
			this.lightVolume.transform.GetChild(i).rotation = Quaternion.LookRotation((this.lightVolume.transform.GetChild(i).position - Camera.main.transform.position).normalized);
		}
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0005964C File Offset: 0x0005784C
	public void ToggleFlashlight()
	{
		this.lightVolume.SetActive(!this.lightVolume.activeSelf);
		this.spotlight.enabled = !this.spotlight.enabled;
		this.bulbGlow.SetActive(this.lightVolume.activeSelf);
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x000596A1 File Offset: 0x000578A1
	public void EnableFlashlight(bool doEnable)
	{
		this.lightVolume.SetActive(doEnable);
		this.spotlight.enabled = doEnable;
		this.bulbGlow.SetActive(doEnable);
	}

	// Token: 0x04001485 RID: 5253
	public GameObject lightVolume;

	// Token: 0x04001486 RID: 5254
	public Light spotlight;

	// Token: 0x04001487 RID: 5255
	public GameObject bulbGlow;
}
