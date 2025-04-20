using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class BetaChecker : MonoBehaviour
{
	// Token: 0x0600177F RID: 6015 RVA: 0x0003FE7A File Offset: 0x0003E07A
	private void Start()
	{
		if (PlayerPrefs.GetString("CheckedBox2") == "true")
		{
			this.doNotEnable = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000C89E0 File Offset: 0x000C6BE0
	private void Update()
	{
		if (!this.doNotEnable)
		{
			if (CosmeticsController.instance.confirmedDidntPlayInBeta)
			{
				PlayerPrefs.SetString("CheckedBox2", "true");
				PlayerPrefs.Save();
				base.gameObject.SetActive(false);
				return;
			}
			if (CosmeticsController.instance.playedInBeta)
			{
				GameObject[] array = this.objectsToEnable;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(true);
				}
				this.doNotEnable = true;
			}
		}
	}

	// Token: 0x04001A2C RID: 6700
	public GameObject[] objectsToEnable;

	// Token: 0x04001A2D RID: 6701
	public bool doNotEnable;
}
