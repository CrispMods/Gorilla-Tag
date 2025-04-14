using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003C8 RID: 968
public class CosmeticStand : GorillaPressableButton
{
	// Token: 0x0600174C RID: 5964 RVA: 0x00072138 File Offset: 0x00070338
	public void InitializeCosmetic()
	{
		this.thisCosmeticItem = CosmeticsController.instance.allCosmetics.Find((CosmeticsController.CosmeticItem x) => this.thisCosmeticName == x.displayName || this.thisCosmeticName == x.overrideDisplayName || this.thisCosmeticName == x.itemName);
		if (this.slotPriceText != null)
		{
			this.slotPriceText.text = this.thisCosmeticItem.itemCategory.ToString().ToUpper() + " " + this.thisCosmeticItem.cost.ToString();
		}
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x000721B6 File Offset: 0x000703B6
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressCosmeticStandButton(this);
	}

	// Token: 0x04001A06 RID: 6662
	public CosmeticsController.CosmeticItem thisCosmeticItem;

	// Token: 0x04001A07 RID: 6663
	public string thisCosmeticName;

	// Token: 0x04001A08 RID: 6664
	public HeadModel thisHeadModel;

	// Token: 0x04001A09 RID: 6665
	public Text slotPriceText;

	// Token: 0x04001A0A RID: 6666
	public Text addToCartText;

	// Token: 0x04001A0B RID: 6667
	[Tooltip("If this is true then this cosmetic stand should have already been updated when the 'Update Cosmetic Stands' button was pressed in the CosmeticsController inspector.")]
	public bool skipMe;
}
