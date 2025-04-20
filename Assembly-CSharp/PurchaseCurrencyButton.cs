using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class PurchaseCurrencyButton : GorillaPressableButton
{
	// Token: 0x06001A29 RID: 6697 RVA: 0x00041A59 File Offset: 0x0003FC59
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		ATM_Manager.instance.PressCurrencyPurchaseButton(this.purchaseCurrencySize);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x00041A80 File Offset: 0x0003FC80
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001D0C RID: 7436
	public string purchaseCurrencySize;

	// Token: 0x04001D0D RID: 7437
	public float buttonFadeTime = 0.25f;
}
