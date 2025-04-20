using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003D6 RID: 982
public class FittingRoomButton : GorillaPressableButton
{
	// Token: 0x060017A6 RID: 6054 RVA: 0x0004000C File Offset: 0x0003E20C
	public override void Start()
	{
		this.currentCosmeticItem = CosmeticsController.instance.nullItem;
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x000C9070 File Offset: 0x000C7270
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

	// Token: 0x060017A8 RID: 6056 RVA: 0x00040020 File Offset: 0x0003E220
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressFittingRoomButton(this, isLeftHand);
	}

	// Token: 0x04001A57 RID: 6743
	public CosmeticsController.CosmeticItem currentCosmeticItem;

	// Token: 0x04001A58 RID: 6744
	public Image currentImage;

	// Token: 0x04001A59 RID: 6745
	public MeshRenderer button;

	// Token: 0x04001A5A RID: 6746
	public Material blank;

	// Token: 0x04001A5B RID: 6747
	public string noCosmeticText;

	// Token: 0x04001A5C RID: 6748
	public Text buttonText;
}
