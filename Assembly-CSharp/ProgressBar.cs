using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000628 RID: 1576
public class ProgressBar : MonoBehaviour
{
	// Token: 0x0600273F RID: 10047 RVA: 0x000C0B78 File Offset: 0x000BED78
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

	// Token: 0x04002AFE RID: 11006
	[SerializeField]
	private Image fillImage;

	// Token: 0x04002AFF RID: 11007
	[SerializeField]
	private bool useColors;

	// Token: 0x04002B00 RID: 11008
	[SerializeField]
	private Color underCapacity = Color.green;

	// Token: 0x04002B01 RID: 11009
	[SerializeField]
	private Color overCapacity = Color.red;

	// Token: 0x04002B02 RID: 11010
	[SerializeField]
	private Color atCapacity = Color.yellow;

	// Token: 0x04002B03 RID: 11011
	private float _fillAmount;
}
