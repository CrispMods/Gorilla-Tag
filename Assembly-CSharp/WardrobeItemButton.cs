using System;
using GorillaNetworking;

// Token: 0x02000433 RID: 1075
public class WardrobeItemButton : GorillaPressableButton
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x00082EA9 File Offset: 0x000810A9
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressWardrobeItemButton(this.currentCosmeticItem, isLeftHand);
	}

	// Token: 0x04001D50 RID: 7504
	public HeadModel controlledModel;

	// Token: 0x04001D51 RID: 7505
	public CosmeticsController.CosmeticItem currentCosmeticItem;
}
