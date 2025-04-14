using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000114 RID: 276
public class ProgressDisplay : MonoBehaviour
{
	// Token: 0x0600077D RID: 1917 RVA: 0x0002A08A File Offset: 0x0002828A
	private void Reset()
	{
		this.root = base.gameObject;
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0002A098 File Offset: 0x00028298
	public void SetVisible(bool visible)
	{
		this.root.SetActive(visible);
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0002A0A8 File Offset: 0x000282A8
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

	// Token: 0x06000780 RID: 1920 RVA: 0x0002A122 File Offset: 0x00028322
	public void SetProgress(float progress)
	{
		this.progressImage.fillAmount = progress;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0002A130 File Offset: 0x00028330
	private void SetTextVisible(bool visible)
	{
		if (this.text.gameObject.activeSelf == visible)
		{
			return;
		}
		this.text.gameObject.SetActive(visible);
	}

	// Token: 0x040008D0 RID: 2256
	[SerializeField]
	private GameObject root;

	// Token: 0x040008D1 RID: 2257
	[SerializeField]
	private TMP_Text text;

	// Token: 0x040008D2 RID: 2258
	[SerializeField]
	private Image progressImage;

	// Token: 0x040008D3 RID: 2259
	[SerializeField]
	private int largestNumberToShow = 99;
}
