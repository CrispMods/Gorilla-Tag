using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003CB RID: 971
public class CheckoutCartButton : GorillaPressableButton
{
	// Token: 0x06001782 RID: 6018 RVA: 0x0003FEA5 File Offset: 0x0003E0A5
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000C8A58 File Offset: 0x000C6C58
	public override void UpdateColor()
	{
		if (this.currentCosmeticItem.itemName == "null")
		{
			this.button.material = this.unpressedMaterial;
			this.buttonText.text = this.noCosmeticText;
			return;
		}
		if (this.isOn)
		{
			this.button.material = this.pressedMaterial;
			this.buttonText.text = this.onText;
			return;
		}
		this.button.material = this.unpressedMaterial;
		this.buttonText.text = this.offText;
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x0003FEB9 File Offset: 0x0003E0B9
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressCheckoutCartButton(this, isLeftHand);
	}

	// Token: 0x04001A2E RID: 6702
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x04001A2F RID: 6703
	public Image currentImage;

	// Token: 0x04001A30 RID: 6704
	public MeshRenderer button;

	// Token: 0x04001A31 RID: 6705
	public Material blank;

	// Token: 0x04001A32 RID: 6706
	public string noCosmeticText;

	// Token: 0x04001A33 RID: 6707
	public Text buttonText;
}
