using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011E RID: 286
public class ProgressDisplay : MonoBehaviour
{
	// Token: 0x060007BF RID: 1983 RVA: 0x00035789 File Offset: 0x00033989
	private void Reset()
	{
		this.root = base.gameObject;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00035797 File Offset: 0x00033997
	public void SetVisible(bool visible)
	{
		this.root.SetActive(visible);
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0008B7F8 File Offset: 0x000899F8
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

	// Token: 0x060007C2 RID: 1986 RVA: 0x000357A5 File Offset: 0x000339A5
	public void SetProgress(float progress)
	{
		this.progressImage.fillAmount = progress;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000357B3 File Offset: 0x000339B3
	private void SetTextVisible(bool visible)
	{
		if (this.text.gameObject.activeSelf == visible)
		{
			return;
		}
		this.text.gameObject.SetActive(visible);
	}

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private GameObject root;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private TMP_Text text;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private Image progressImage;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private int largestNumberToShow = 99;
}
