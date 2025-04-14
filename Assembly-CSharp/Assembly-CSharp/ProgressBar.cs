using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000629 RID: 1577
public class ProgressBar : MonoBehaviour
{
	// Token: 0x06002747 RID: 10055 RVA: 0x000C0FF8 File Offset: 0x000BF1F8
	public void UpdateProgress(float newFill)
	{
		bool flag = newFill > 1f;
		this._fillAmount = Mathf.Clamp(newFill, 0f, 1f);
		this.fillImage.fillAmount = this._fillAmount;
		if (this.useColors)
		{
			if (flag)
			{
				this.fillImage.color = this.overCapacity;
				return;
			}
			if (Mathf.Approximately(this._fillAmount, 1f))
			{
				this.fillImage.color = this.atCapacity;
				return;
			}
			this.fillImage.color = this.underCapacity;
		}
	}

	// Token: 0x04002B04 RID: 11012
	[SerializeField]
	private Image fillImage;

	// Token: 0x04002B05 RID: 11013
	[SerializeField]
	private bool useColors;

	// Token: 0x04002B06 RID: 11014
	[SerializeField]
	private Color underCapacity = Color.green;

	// Token: 0x04002B07 RID: 11015
	[SerializeField]
	private Color overCapacity = Color.red;

	// Token: 0x04002B08 RID: 11016
	[SerializeField]
	private Color atCapacity = Color.yellow;

	// Token: 0x04002B09 RID: 11017
	private float _fillAmount;
}
