using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003CB RID: 971
public class FittingRoomButton : GorillaPressableButton
{
	// Token: 0x06001759 RID: 5977 RVA: 0x00071FDB File Offset: 0x000701DB
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x00071FF0 File Offset: 0x000701F0
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

	// Token: 0x0600175B RID: 5979 RVA: 0x00072084 File Offset: 0x00070284
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressFittingRoomButton(this, isLeftHand);
	}

	// Token: 0x04001A0E RID: 6670
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x04001A0F RID: 6671
	public Image currentImage;

	// Token: 0x04001A10 RID: 6672
	public MeshRenderer button;

	// Token: 0x04001A11 RID: 6673
	public Material blank;

	// Token: 0x04001A12 RID: 6674
	public string noCosmeticText;

	// Token: 0x04001A13 RID: 6675
	public Text buttonText;
}
