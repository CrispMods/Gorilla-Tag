using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000415 RID: 1045
public class PurchaseCurrencyButton : GorillaPressableButton
{
	// Token: 0x060019DC RID: 6620 RVA: 0x0007F4B8 File Offset: 0x0007D6B8
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		ATM_Manager.instance.PressCurrencyPurchaseButton(this.purchaseCurrencySize);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x0007F4DF File Offset: 0x0007D6DF
	private IEnumerator ButtonColorUpdate()
	{
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.buttonFadeTime);
		this.buttonRenderer.material = this.unpressedMaterial;
		yield break;
	}

	// Token: 0x04001CC3 RID: 7363
	public string purchaseCurrencySize;

	// Token: 0x04001CC4 RID: 7364
	public float buttonFadeTime = 0.25f;
}
