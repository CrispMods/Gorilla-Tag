using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class BetaChecker : MonoBehaviour
{
	// Token: 0x06001732 RID: 5938 RVA: 0x000717B7 File Offset: 0x0006F9B7
	private void Start()
	{
		if (PlayerPrefs.GetString("CheckedBox2") == "true")
		{
			this.doNotEnable = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x000717E4 File Offset: 0x0006F9E4
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

	// Token: 0x040019E3 RID: 6627
	public GameObject[] objectsToEnable;

	// Token: 0x040019E4 RID: 6628
	public bool doNotEnable;
}
