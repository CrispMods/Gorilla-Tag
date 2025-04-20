using System;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003D3 RID: 979
public class CosmeticStand : GorillaPressableButton
{
	// Token: 0x06001796 RID: 6038 RVA: 0x000C8F14 File Offset: 0x000C7114
	public void InitializeCosmetic()
	{
		this.thisCosmeticItem = CosmeticsController.instance.allCosmetics.Find((CosmeticsController.CosmeticItem x) => this.thisCosmeticName == x.displayName || this.thisCosmeticName == x.overrideDisplayName || this.thisCosmeticName == x.itemName);
		if (this.slotPriceText != null)
		{
			this.slotPriceText.text = this.thisCosmeticItem.itemCategory.ToString().ToUpper() + " " + this.thisCosmeticItem.cost.ToString();
		}
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x0003FF3F File Offset: 0x0003E13F
	public override void ButtonActivation()
	{
		base.ButtonActivation();
		CosmeticsController.instance.PressCosmeticStandButton(this);
	}

	// Token: 0x04001A4E RID: 6734
	public CosmeticsController.CosmeticItem thisCosmeticItem;

	// Token: 0x04001A4F RID: 6735
	public string thisCosmeticName;

	// Token: 0x04001A50 RID: 6736
	public HeadModel thisHeadModel;

	// Token: 0x04001A51 RID: 6737
	public Text slotPriceText;

	// Token: 0x04001A52 RID: 6738
	public Text addToCartText;

	// Token: 0x04001A53 RID: 6739
	[Tooltip("If this is true then this cosmetic stand should have already been updated when the 'Update Cosmetic Stands' button was pressed in the CosmeticsController inspector.")]
	public bool skipMe;
}
