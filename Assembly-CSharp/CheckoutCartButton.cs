using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C0 RID: 960
public class CheckoutCartButton : GorillaPressableButton
{
	// Token: 0x06001735 RID: 5941 RVA: 0x0007185B File Offset: 0x0006FA5B
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x00071870 File Offset: 0x0006FA70
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

	// Token: 0x06001737 RID: 5943 RVA: 0x00071904 File Offset: 0x0006FB04
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressCheckoutCartButton(this, isLeftHand);
	}

	// Token: 0x040019E5 RID: 6629
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x040019E6 RID: 6630
	public Image currentImage;

	// Token: 0x040019E7 RID: 6631
	public MeshRenderer button;

	// Token: 0x040019E8 RID: 6632
	public Material blank;

	// Token: 0x040019E9 RID: 6633
	public string noCosmeticText;

	// Token: 0x040019EA RID: 6634
	public Text buttonText;
}
