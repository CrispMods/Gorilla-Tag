using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class PurchaseItemButton : GorillaPressableButton
{
	// Token: 0x06001A32 RID: 6706 RVA: 0x00041AB9 File Offset: 0x0003FCB9
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressPurchaseItemButton(this, isLeftHand);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x00041ADC File Offset: 0x0003FCDC
	private IEnumerator ButtonColorUpdate()
	{
		Debug.Log("did this happen?");
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x04001D11 RID: 7441
	public string buttonSide;
}
