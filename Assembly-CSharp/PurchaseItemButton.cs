using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class PurchaseItemButton : GorillaPressableButton
{
	// Token: 0x060019E5 RID: 6629 RVA: 0x0007F587 File Offset: 0x0007D787
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressPurchaseItemButton(this, isLeftHand);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x0007F5AA File Offset: 0x0007D7AA
	private IEnumerator ButtonColorUpdate()
	{
		Debug.Log("did this happen?");
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x04001CC8 RID: 7368
	public string buttonSide;
}
