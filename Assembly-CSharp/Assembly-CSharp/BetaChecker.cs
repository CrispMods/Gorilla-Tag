using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class BetaChecker : MonoBehaviour
{
	// Token: 0x06001735 RID: 5941 RVA: 0x00071B3B File Offset: 0x0006FD3B
	private void Start()
	{
		if (PlayerPrefs.GetString("CheckedBox2") == "true")
		{
			this.doNotEnable = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x00071B68 File Offset: 0x0006FD68
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

	// Token: 0x040019E4 RID: 6628
	public GameObject[] objectsToEnable;

	// Token: 0x040019E5 RID: 6629
	public bool doNotEnable;
}
