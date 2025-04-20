using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000607 RID: 1543
public class ProgressBar : MonoBehaviour
{
	// Token: 0x0600266A RID: 9834 RVA: 0x00108734 File Offset: 0x00106934
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

	// Token: 0x04002A64 RID: 10852
	[SerializeField]
	private Image fillImage;

	// Token: 0x04002A65 RID: 10853
	[SerializeField]
	private bool useColors;

	// Token: 0x04002A66 RID: 10854
	[SerializeField]
	private Color underCapacity = Color.green;

	// Token: 0x04002A67 RID: 10855
	[SerializeField]
	private Color overCapacity = Color.red;

	// Token: 0x04002A68 RID: 10856
	[SerializeField]
	private Color atCapacity = Color.yellow;

	// Token: 0x04002A69 RID: 10857
	private float _fillAmount;
}
