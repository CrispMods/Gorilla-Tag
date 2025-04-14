using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000417 RID: 1047
public class PurchaseItemButton : GorillaPressableButton
{
	// Token: 0x060019E8 RID: 6632 RVA: 0x0007F90B File Offset: 0x0007DB0B
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressPurchaseItemButton(this, isLeftHand);
		base.StartCoroutine(this.ButtonColorUpdate());
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x0007F92E File Offset: 0x0007DB2E
	private IEnumerator ButtonColorUpdate()
	{
		Debug.Log("did this happen?");
		this.buttonRenderer.material = this.pressedMaterial;
		yield return new WaitForSeconds(this.debounceTime);
		this.buttonRenderer.material = (this.isOn ? this.pressedMaterial : this.unpressedMaterial);
		yield break;
	}

	// Token: 0x04001CC9 RID: 7369
	public string buttonSide;
}
