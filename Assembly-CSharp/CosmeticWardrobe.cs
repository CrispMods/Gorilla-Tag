using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class CosmeticWardrobe : MonoBehaviour
{
	// Token: 0x06001A57 RID: 6743 RVA: 0x00081A44 File Offset: 0x0007FC44
	private void Start()
	{
		for (int i = 0; i < this.cosmeticCategoryButtons.Length; i++)
		{
			if (this.cosmeticCategoryButtons[i].category == CosmeticWardrobe.selectedCategory)
			{
				CosmeticWardrobe.selectedCategoryIndex = i;
				break;
			}
		}
		for (int j = 0; j < this.cosmeticCollectionDisplays.Length; j++)
		{
			this.cosmeticCollectionDisplays[j].displayHead.transform.localScale = this.startingHeadSize;
		}
		if (GorillaTagger.Instance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.OnColorChanged += this.HandleLocalColorChanged;
			this.HandleLocalColorChanged(GorillaTagger.Instance.offlineVRRig.playerColor);
		}
		this.nextSelection.onPressed += this.HandlePressedNextSelection;
		this.prevSelection.onPressed += this.HandlePressedPrevSelection;
		for (int k = 0; k < this.cosmeticCollectionDisplays.Length; k++)
		{
			this.cosmeticCollectionDisplays[k].selectButton.onPressed += this.HandlePressedSelectCosmeticButton;
		}
		for (int l = 0; l < this.cosmeticCategoryButtons.Length; l++)
		{
			this.cosmeticCategoryButtons[l].button.onPressed += this.HandleChangeCategory;
			this.cosmeticCategoryButtons[l].slot1RemovedItem = CosmeticsController.instance.nullItem;
			this.cosmeticCategoryButtons[l].slot2RemovedItem = CosmeticsController.instance.nullItem;
		}
		CosmeticsController instance = CosmeticsController.instance;
		instance.OnCosmeticsUpdated = (Action)Delegate.Combine(instance.OnCosmeticsUpdated, new Action(this.HandleCosmeticsUpdated));
		CosmeticWardrobe.OnWardrobeUpdateCategories = (Action)Delegate.Combine(CosmeticWardrobe.OnWardrobeUpdateCategories, new Action(this.UpdateCategoryButtons));
		CosmeticWardrobe.OnWardrobeUpdateDisplays = (Action)Delegate.Combine(CosmeticWardrobe.OnWardrobeUpdateDisplays, new Action(this.UpdateCosmeticDisplays));
		this.HandleCosmeticsUpdated();
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x00081C30 File Offset: 0x0007FE30
	private void OnDestroy()
	{
		if (GorillaTagger.Instance && GorillaTagger.Instance.offlineVRRig)
		{
			GorillaTagger.Instance.offlineVRRig.OnColorChanged -= this.HandleLocalColorChanged;
		}
		this.nextSelection.onPressed -= this.HandlePressedNextSelection;
		this.prevSelection.onPressed -= this.HandlePressedPrevSelection;
		for (int i = 0; i < this.cosmeticCollectionDisplays.Length; i++)
		{
			this.cosmeticCollectionDisplays[i].selectButton.onPressed -= this.HandlePressedSelectCosmeticButton;
		}
		for (int j = 0; j < this.cosmeticCategoryButtons.Length; j++)
		{
			this.cosmeticCategoryButtons[j].button.onPressed -= this.HandleChangeCategory;
		}
		CosmeticsController instance = CosmeticsController.instance;
		instance.OnCosmeticsUpdated = (Action)Delegate.Remove(instance.OnCosmeticsUpdated, new Action(this.HandleCosmeticsUpdated));
		CosmeticWardrobe.OnWardrobeUpdateCategories = (Action)Delegate.Remove(CosmeticWardrobe.OnWardrobeUpdateCategories, new Action(this.UpdateCategoryButtons));
		CosmeticWardrobe.OnWardrobeUpdateDisplays = (Action)Delegate.Remove(CosmeticWardrobe.OnWardrobeUpdateDisplays, new Action(this.UpdateCosmeticDisplays));
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x00081D70 File Offset: 0x0007FF70
	private void HandlePressedNextSelection(GorillaPressableButton button, bool isLeft)
	{
		CosmeticWardrobe.startingDisplayIndex += this.cosmeticCollectionDisplays.Length;
		if (CosmeticWardrobe.startingDisplayIndex >= CosmeticsController.instance.GetCategorySize(CosmeticWardrobe.selectedCategory))
		{
			CosmeticWardrobe.startingDisplayIndex = 0;
		}
		Action onWardrobeUpdateDisplays = CosmeticWardrobe.OnWardrobeUpdateDisplays;
		if (onWardrobeUpdateDisplays == null)
		{
			return;
		}
		onWardrobeUpdateDisplays();
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x00081DC0 File Offset: 0x0007FFC0
	private void HandlePressedPrevSelection(GorillaPressableButton button, bool isLeft)
	{
		CosmeticWardrobe.startingDisplayIndex -= this.cosmeticCollectionDisplays.Length;
		if (CosmeticWardrobe.startingDisplayIndex < 0)
		{
			int categorySize = CosmeticsController.instance.GetCategorySize(CosmeticWardrobe.selectedCategory);
			int num;
			if (categorySize % this.cosmeticCollectionDisplays.Length == 0)
			{
				num = categorySize - this.cosmeticCollectionDisplays.Length;
			}
			else
			{
				num = categorySize / this.cosmeticCollectionDisplays.Length;
				num *= this.cosmeticCollectionDisplays.Length;
			}
			CosmeticWardrobe.startingDisplayIndex = num;
		}
		Action onWardrobeUpdateDisplays = CosmeticWardrobe.OnWardrobeUpdateDisplays;
		if (onWardrobeUpdateDisplays == null)
		{
			return;
		}
		onWardrobeUpdateDisplays();
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x00081E40 File Offset: 0x00080040
	private void HandlePressedSelectCosmeticButton(GorillaPressableButton button, bool isLeft)
	{
		int i = 0;
		while (i < this.cosmeticCollectionDisplays.Length)
		{
			if (this.cosmeticCollectionDisplays[i].selectButton == button)
			{
				CosmeticsController.instance.PressWardrobeItemButton(this.cosmeticCollectionDisplays[i].currentCosmeticItem, isLeft);
				if (isLeft)
				{
					this.cosmeticCategoryButtons[CosmeticWardrobe.selectedCategoryIndex].slot2RemovedItem = CosmeticsController.instance.nullItem;
					return;
				}
				this.cosmeticCategoryButtons[CosmeticWardrobe.selectedCategoryIndex].slot1RemovedItem = CosmeticsController.instance.nullItem;
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x00081ED0 File Offset: 0x000800D0
	private void HandleChangeCategory(GorillaPressableButton button, bool isLeft)
	{
		for (int i = 0; i < this.cosmeticCategoryButtons.Length; i++)
		{
			CosmeticWardrobe.CosmeticWardrobeCategory cosmeticWardrobeCategory = this.cosmeticCategoryButtons[i];
			if (cosmeticWardrobeCategory.button == button)
			{
				if (CosmeticWardrobe.selectedCategory == cosmeticWardrobeCategory.category)
				{
					CosmeticsController.CosmeticItem cosmeticItem = CosmeticsController.instance.nullItem;
					if (cosmeticWardrobeCategory.slot1 != CosmeticsController.CosmeticSlots.Count)
					{
						cosmeticItem = CosmeticsController.instance.GetSlotItem(cosmeticWardrobeCategory.slot1, true);
					}
					CosmeticsController.CosmeticItem cosmeticItem2 = CosmeticsController.instance.nullItem;
					if (cosmeticWardrobeCategory.slot2 != CosmeticsController.CosmeticSlots.Count)
					{
						cosmeticItem2 = CosmeticsController.instance.GetSlotItem(cosmeticWardrobeCategory.slot2, true);
					}
					bool flag = CosmeticWardrobe.selectedCategory == CosmeticsController.CosmeticCategory.Arms;
					if (!cosmeticItem.isNullItem || !cosmeticItem2.isNullItem)
					{
						if (!cosmeticItem.isNullItem)
						{
							cosmeticWardrobeCategory.slot1RemovedItem = cosmeticItem;
							CosmeticsController.instance.PressWardrobeItemButton(cosmeticItem, flag);
						}
						if (!cosmeticItem2.isNullItem)
						{
							cosmeticWardrobeCategory.slot2RemovedItem = cosmeticItem2;
							CosmeticsController.instance.PressWardrobeItemButton(cosmeticItem2, !flag);
						}
						Action onWardrobeUpdateDisplays = CosmeticWardrobe.OnWardrobeUpdateDisplays;
						if (onWardrobeUpdateDisplays != null)
						{
							onWardrobeUpdateDisplays();
						}
						Action onWardrobeUpdateCategories = CosmeticWardrobe.OnWardrobeUpdateCategories;
						if (onWardrobeUpdateCategories == null)
						{
							return;
						}
						onWardrobeUpdateCategories();
						return;
					}
					else if (!cosmeticWardrobeCategory.slot1RemovedItem.isNullItem || !cosmeticWardrobeCategory.slot2RemovedItem.isNullItem)
					{
						if (!cosmeticWardrobeCategory.slot1RemovedItem.isNullItem)
						{
							CosmeticsController.instance.PressWardrobeItemButton(cosmeticWardrobeCategory.slot1RemovedItem, flag);
							cosmeticWardrobeCategory.slot1RemovedItem = CosmeticsController.instance.nullItem;
						}
						if (!cosmeticWardrobeCategory.slot2RemovedItem.isNullItem)
						{
							CosmeticsController.instance.PressWardrobeItemButton(cosmeticWardrobeCategory.slot2RemovedItem, !flag);
							cosmeticWardrobeCategory.slot2RemovedItem = CosmeticsController.instance.nullItem;
						}
						Action onWardrobeUpdateDisplays2 = CosmeticWardrobe.OnWardrobeUpdateDisplays;
						if (onWardrobeUpdateDisplays2 != null)
						{
							onWardrobeUpdateDisplays2();
						}
						Action onWardrobeUpdateCategories2 = CosmeticWardrobe.OnWardrobeUpdateCategories;
						if (onWardrobeUpdateCategories2 == null)
						{
							return;
						}
						onWardrobeUpdateCategories2();
						return;
					}
				}
				else
				{
					CosmeticWardrobe.selectedCategory = cosmeticWardrobeCategory.category;
					CosmeticWardrobe.selectedCategoryIndex = i;
					CosmeticWardrobe.startingDisplayIndex = 0;
					Action onWardrobeUpdateDisplays3 = CosmeticWardrobe.OnWardrobeUpdateDisplays;
					if (onWardrobeUpdateDisplays3 != null)
					{
						onWardrobeUpdateDisplays3();
					}
					Action onWardrobeUpdateCategories3 = CosmeticWardrobe.OnWardrobeUpdateCategories;
					if (onWardrobeUpdateCategories3 == null)
					{
						return;
					}
					onWardrobeUpdateCategories3();
				}
				return;
			}
		}
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000820D0 File Offset: 0x000802D0
	private void HandleCosmeticsUpdated()
	{
		string[] currentlyWornCosmetics = CosmeticsController.instance.GetCurrentlyWornCosmetics();
		bool[] currentRightEquippedSided = CosmeticsController.instance.GetCurrentRightEquippedSided();
		this.currentEquippedDisplay.SetCosmeticActiveArray(currentlyWornCosmetics, currentRightEquippedSided);
		this.UpdateCategoryButtons();
		this.UpdateCosmeticDisplays();
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x00082110 File Offset: 0x00080310
	private void HandleLocalColorChanged(Color newColor)
	{
		MeshRenderer component = this.currentEquippedDisplay.GetComponent<MeshRenderer>();
		if (component != null)
		{
			component.material.color = newColor;
		}
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x00082140 File Offset: 0x00080340
	private void UpdateCosmeticDisplays()
	{
		for (int i = 0; i < this.cosmeticCollectionDisplays.Length; i++)
		{
			CosmeticsController.CosmeticItem cosmetic = CosmeticsController.instance.GetCosmetic(CosmeticWardrobe.selectedCategory, CosmeticWardrobe.startingDisplayIndex + i);
			this.cosmeticCollectionDisplays[i].currentCosmeticItem = cosmetic;
			this.cosmeticCollectionDisplays[i].displayHead.SetCosmeticActive(cosmetic.displayName, false);
			this.cosmeticCollectionDisplays[i].selectButton.enabled = !cosmetic.isNullItem;
			this.cosmeticCollectionDisplays[i].selectButton.isOn = (!cosmetic.isNullItem && CosmeticsController.instance.IsCosmeticEquipped(cosmetic));
			this.cosmeticCollectionDisplays[i].selectButton.UpdateColor();
		}
		int categorySize = CosmeticsController.instance.GetCategorySize(CosmeticWardrobe.selectedCategory);
		this.nextSelection.enabled = (categorySize > this.cosmeticCollectionDisplays.Length);
		this.nextSelection.UpdateColor();
		this.prevSelection.enabled = (categorySize > this.cosmeticCollectionDisplays.Length);
		this.prevSelection.UpdateColor();
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x00082254 File Offset: 0x00080454
	private void UpdateCategoryButtons()
	{
		for (int i = 0; i < this.cosmeticCategoryButtons.Length; i++)
		{
			CosmeticWardrobe.CosmeticWardrobeCategory cosmeticWardrobeCategory = this.cosmeticCategoryButtons[i];
			if (cosmeticWardrobeCategory.slot1 != CosmeticsController.CosmeticSlots.Count)
			{
				CosmeticsController.CosmeticItem slotItem = CosmeticsController.instance.GetSlotItem(cosmeticWardrobeCategory.slot1, false);
				if (cosmeticWardrobeCategory.slot2 != CosmeticsController.CosmeticSlots.Count)
				{
					CosmeticsController.CosmeticItem slotItem2 = CosmeticsController.instance.GetSlotItem(cosmeticWardrobeCategory.slot2, false);
					if (slotItem.bothHandsHoldable)
					{
						cosmeticWardrobeCategory.button.SetIcon(slotItem.isNullItem ? null : slotItem.itemPicture);
					}
					else if (slotItem2.bothHandsHoldable)
					{
						cosmeticWardrobeCategory.button.SetIcon(slotItem2.isNullItem ? null : slotItem2.itemPicture);
					}
					else
					{
						cosmeticWardrobeCategory.button.SetDualIcon(slotItem.isNullItem ? null : slotItem.itemPicture, slotItem2.isNullItem ? null : slotItem2.itemPicture);
					}
				}
				else
				{
					cosmeticWardrobeCategory.button.SetIcon(slotItem.isNullItem ? null : slotItem.itemPicture);
				}
			}
			int categorySize = CosmeticsController.instance.GetCategorySize(cosmeticWardrobeCategory.category);
			cosmeticWardrobeCategory.button.enabled = (categorySize > 0);
			cosmeticWardrobeCategory.button.isOn = (CosmeticWardrobe.selectedCategory == cosmeticWardrobeCategory.category);
			cosmeticWardrobeCategory.button.UpdateColor();
		}
	}

	// Token: 0x04001D1B RID: 7451
	[SerializeField]
	private CosmeticWardrobe.CosmeticWardrobeSelection[] cosmeticCollectionDisplays;

	// Token: 0x04001D1C RID: 7452
	[SerializeField]
	private CosmeticWardrobe.CosmeticWardrobeCategory[] cosmeticCategoryButtons;

	// Token: 0x04001D1D RID: 7453
	[SerializeField]
	private HeadModel currentEquippedDisplay;

	// Token: 0x04001D1E RID: 7454
	[SerializeField]
	private GorillaPressableButton nextSelection;

	// Token: 0x04001D1F RID: 7455
	[SerializeField]
	private GorillaPressableButton prevSelection;

	// Token: 0x04001D20 RID: 7456
	private static int selectedCategoryIndex = 0;

	// Token: 0x04001D21 RID: 7457
	private static CosmeticsController.CosmeticCategory selectedCategory = CosmeticsController.CosmeticCategory.Hat;

	// Token: 0x04001D22 RID: 7458
	private static int startingDisplayIndex = 0;

	// Token: 0x04001D23 RID: 7459
	private static Action OnWardrobeUpdateCategories;

	// Token: 0x04001D24 RID: 7460
	private static Action OnWardrobeUpdateDisplays;

	// Token: 0x04001D25 RID: 7461
	public Vector3 startingHeadSize = new Vector3(0.25f, 0.25f, 0.25f);

	// Token: 0x0200042A RID: 1066
	[Serializable]
	public class CosmeticWardrobeSelection
	{
		// Token: 0x04001D26 RID: 7462
		public HeadModel displayHead;

		// Token: 0x04001D27 RID: 7463
		public CosmeticButton selectButton;

		// Token: 0x04001D28 RID: 7464
		public CosmeticsController.CosmeticItem currentCosmeticItem;
	}

	// Token: 0x0200042B RID: 1067
	[Serializable]
	public class CosmeticWardrobeCategory
	{
		// Token: 0x04001D29 RID: 7465
		public CosmeticCategoryButton button;

		// Token: 0x04001D2A RID: 7466
		public CosmeticsController.CosmeticCategory category;

		// Token: 0x04001D2B RID: 7467
		public CosmeticsController.CosmeticSlots slot1 = CosmeticsController.CosmeticSlots.Count;

		// Token: 0x04001D2C RID: 7468
		public CosmeticsController.CosmeticSlots slot2 = CosmeticsController.CosmeticSlots.Count;

		// Token: 0x04001D2D RID: 7469
		public CosmeticsController.CosmeticItem slot1RemovedItem;

		// Token: 0x04001D2E RID: 7470
		public CosmeticsController.CosmeticItem slot2RemovedItem;
	}
}
