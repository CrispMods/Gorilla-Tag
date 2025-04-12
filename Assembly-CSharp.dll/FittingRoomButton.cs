using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003CB RID: 971
public class FittingRoomButton : GorillaPressableButton
{
	// Token: 0x0600175C RID: 5980 RVA: 0x0003ED22 File Offset: 0x0003CF22
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000C6848 File Offset: 0x000C4A48
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

	// Token: 0x0600175E RID: 5982 RVA: 0x0003ED36 File Offset: 0x0003CF36
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressFittingRoomButton(this, isLeftHand);
	}

	// Token: 0x04001A0F RID: 6671
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x04001A10 RID: 6672
	public Image currentImage;

	// Token: 0x04001A11 RID: 6673
	public MeshRenderer button;

	// Token: 0x04001A12 RID: 6674
	public Material blank;

	// Token: 0x04001A13 RID: 6675
	public string noCosmeticText;

	// Token: 0x04001A14 RID: 6676
	public Text buttonText;
}
