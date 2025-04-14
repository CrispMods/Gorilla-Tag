using System;
using GorillaNetworking;

// Token: 0x02000433 RID: 1075
public class WardrobeItemButton : GorillaPressableButton
{
	// Token: 0x06001A8B RID: 6795 RVA: 0x0008322D File Offset: 0x0008142D
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressWardrobeItemButton(this.currentCosmeticItem, isLeftHand);
	}

	// Token: 0x04001D51 RID: 7505
	public HeadModel controlledModel;

	// Token: 0x04001D52 RID: 7506
	public CosmeticsController.CosmeticItem currentCosmeticItem;
}
