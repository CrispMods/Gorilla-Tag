using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class PurchaseCurrencyButton : GorillaPressableButton
{
	// Token: 0x060019DF RID: 6623 RVA: 0x0007F83C File Offset: 0x0007DA3C
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		ATM_Manager.instance.PressCurrencyPurchaseButton(this.purchaseCurrencySize);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x0007F863 File Offset: 0x0007DA63
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001CC4 RID: 7364
	public string purchaseCurrencySize;

	// Token: 0x04001CC5 RID: 7365
	public float buttonFadeTime = 0.25f;
}
