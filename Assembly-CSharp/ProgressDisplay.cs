using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000114 RID: 276
public class ProgressDisplay : MonoBehaviour
{
	// Token: 0x0600077B RID: 1915 RVA: 0x00029D66 File Offset: 0x00027F66
	private void Reset()
	{
		this.root = base.gameObject;
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00029D74 File Offset: 0x00027F74
	public void SetVisible(bool visible)
	{
		this.root.SetActive(visible);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00029D84 File Offset: 0x00027F84
	public void SetProgress(int progress, int total)
	{
		if (this.text)
		{
			if (total < this.largestNumberToShow)
			{
				this.text.text = ((progress >= total) ? string.Format("{0}", total) : string.Format("{0}/{1}", progress, total));
				this.SetTextVisible(true);
			}
			else
			{
				this.SetTextVisible(false);
			}
		}
		this.progressImage.fillAmount = (float)progress / (float)total;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00029DFE File Offset: 0x00027FFE
	public void SetProgress(float progress)
	{
		this.progressImage.fillAmount = progress;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00029E0C File Offset: 0x0002800C
	private void SetTextVisible(bool visible)
	{
		if (this.text.gameObject.activeSelf == visible)
		{
			return;
		}
		this.text.gameObject.SetActive(visible);
	}

	// Token: 0x040008CF RID: 2255
	[SerializeField]
	private GameObject root;

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private TMP_Text text;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private Image progressImage;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private int largestNumberToShow = 99;
}
