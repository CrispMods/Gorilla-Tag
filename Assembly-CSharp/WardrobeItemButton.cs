using System;
using GorillaNetworking;

// Token: 0x0200043F RID: 1087
public class WardrobeItemButton : GorillaPressableButton
{
	// Token: 0x06001ADC RID: 6876 RVA: 0x000422F9 File Offset: 0x000404F9
	public override void ButtonActivationWithHand(bool isLeftHand)
	{
		base.ButtonActivationWithHand(isLeftHand);
		CosmeticsController.instance.PressWardrobeItemButton(this.currentCosmeticItem, isLeftHand);
	}

	// Token: 0x04001D9F RID: 7583
	public HeadModel controlledModel;

	// Token: 0x04001DA0 RID: 7584
	public CosmeticsController.CosmeticItem currentCosmeticItem;
}
