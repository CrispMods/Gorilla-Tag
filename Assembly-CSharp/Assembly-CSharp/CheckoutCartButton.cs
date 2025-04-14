using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C0 RID: 960
public class CheckoutCartButton : GorillaPressableButton
{
	// Token: 0x06001738 RID: 5944 RVA: 0x00071BDF File Offset: 0x0006FDDF
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x00071BF4 File Offset: 0x0006FDF4
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

	// Token: 0x0600173A RID: 5946 RVA: 0x00071C88 File Offset: 0x0006FE88
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressCheckoutCartButton(this, isLeftHand);
	}

	// Token: 0x040019E6 RID: 6630
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x040019E7 RID: 6631
	public Image currentImage;

	// Token: 0x040019E8 RID: 6632
	public MeshRenderer button;

	// Token: 0x040019E9 RID: 6633
	public Material blank;

	// Token: 0x040019EA RID: 6634
	public string noCosmeticText;

	// Token: 0x040019EB RID: 6635
	public Text buttonText;
}
