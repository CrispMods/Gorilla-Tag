using System;
using System.Collections.Generic;
using System.Linq;
using GorillaNetworking;
using GorillaNetworking.Store;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000430 RID: 1072
public class TryOnBundlesStand : MonoBehaviour
{
	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001A7D RID: 6781 RVA: 0x00041DD5 File Offset: 0x0003FFD5
	private string SelectedBundlePlayFabID
	{
		get
		{
			return this.TryOnBundleButtons[this.SelectedButtonIndex].playfabBundleID;
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000D62A8 File Offset: 0x000D44A8
	public static string CleanUpTitleDataValues(string titleDataResult)
	{
		string text = titleDataResult.Replace("\\r", "\r").Replace("\\n", "\n");
		if (text[0] == '"' && text[text.Length - 1] == '"')
		{
			text = text.Substring(1, text.Length - 2);
		}
		return text;
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000D6304 File Offset: 0x000D4504
	private void InitalizeButtons()
	{
		this.GetTryOnButtons();
		for (int i = 0; i < this.TryOnBundleButtons.Length; i++)
		{
			if (!CosmeticsController.instance.GetItemFromDict(this.TryOnBundleButtons[i].playfabBundleID).isNullItem)
			{
				this.TryOnBundleButtons[i].UpdateColor();
			}
		}
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000D6358 File Offset: 0x000D4558
	private void Start()
	{
		PlayFabTitleDataCache.Instance.GetTitleData(this.ComputerDefaultTextTitleDataKey, new Action<string>(this.OnComputerDefaultTextTitleDataSuccess), new Action<PlayFabError>(this.OnComputerDefaultTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.ComputerAlreadyOwnTextTitleDataKey, new Action<string>(this.OnComputerAlreadyOwnTextTitleDataSuccess), new Action<PlayFabError>(this.OnComputerAlreadyOwnTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.PurchaseButtonDefaultTextTitleDataKey, new Action<string>(this.OnPurchaseButtonDefaultTextTitleDataSuccess), new Action<PlayFabError>(this.OnPurchaseButtonDefaultTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.PurchaseButtonAlreadyOwnTextTitleDataKey, new Action<string>(this.OnPurchaseButtonAlreadyOwnTextTitleDataSuccess), new Action<PlayFabError>(this.OnPurchaseButtonAlreadyOwnTextTitleDataFailure));
		this.InitalizeButtons();
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x00041DE9 File Offset: 0x0003FFE9
	private void OnComputerDefaultTextTitleDataSuccess(string data)
	{
		this.ComputerDefaultTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.computerScreenText.text = this.ComputerDefaultTextTitleDataValue;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x00041E08 File Offset: 0x00040008
	private void OnComputerDefaultTextTitleDataFailure(PlayFabError error)
	{
		this.ComputerDefaultTextTitleDataValue = "Failed to get TD Key : " + this.ComputerDefaultTextTitleDataKey;
		this.computerScreenText.text = this.ComputerDefaultTextTitleDataValue;
		Debug.LogError(string.Format("Error getting Computer Screen Title Data: {0}", error));
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x00041E41 File Offset: 0x00040041
	private void OnComputerAlreadyOwnTextTitleDataSuccess(string data)
	{
		this.ComputerAlreadyOwnTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x00041E4F File Offset: 0x0004004F
	private void OnComputerAlreadyOwnTextTitleDataFailure(PlayFabError error)
	{
		this.ComputerAlreadyOwnTextTitleDataValue = "Failed to get TD Key : " + this.ComputerAlreadyOwnTextTitleDataKey;
		Debug.LogError(string.Format("Error getting Computer Already Screen Title Data: {0}", error));
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x00041E77 File Offset: 0x00040077
	private void OnPurchaseButtonDefaultTextTitleDataSuccess(string data)
	{
		this.PurchaseButtonDefaultTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
		this.purchaseButton.UpdateColor();
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000D640C File Offset: 0x000D460C
	private void OnPurchaseButtonDefaultTextTitleDataFailure(PlayFabError error)
	{
		this.PurchaseButtonDefaultTextTitleDataValue = "Failed to get TD Key : " + this.PurchaseButtonDefaultTextTitleDataKey;
		this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
		this.purchaseButton.UpdateColor();
		Debug.LogError(string.Format("Error getting Tryon Purchase Button Default Text Title Data: {0}", error));
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x00041EA1 File Offset: 0x000400A1
	private void OnPurchaseButtonAlreadyOwnTextTitleDataSuccess(string data)
	{
		this.PurchaseButtonAlreadyOwnTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.purchaseButton.AlreadyOwnText = this.PurchaseButtonAlreadyOwnTextTitleDataValue;
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x00041EC0 File Offset: 0x000400C0
	private void OnPurchaseButtonAlreadyOwnTextTitleDataFailure(PlayFabError error)
	{
		this.PurchaseButtonAlreadyOwnTextTitleDataValue = "Failed to get TD Key : " + this.PurchaseButtonAlreadyOwnTextTitleDataKey;
		this.purchaseButton.AlreadyOwnText = this.PurchaseButtonAlreadyOwnTextTitleDataValue;
		Debug.LogError(string.Format("Error getting Tryon Purchase Button Already Own Text Title Data: {0}", error));
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x000D645C File Offset: 0x000D465C
	public void ClearSelectedBundle()
	{
		if (this.SelectedButtonIndex != -1)
		{
			this.TryOnBundleButtons[this.SelectedButtonIndex].isOn = false;
			if (this.TryOnBundleButtons[this.SelectedButtonIndex].playfabBundleID != "NULL" || this.TryOnBundleButtons[this.SelectedButtonIndex].playfabBundleID != "")
			{
				this.RemoveBundle(this.SelectedBundlePlayFabID);
				this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
				this.purchaseButton.ResetButton();
				this.selectedBundleImage.sprite = null;
				this.TryOnBundleButtons[this.SelectedButtonIndex].UpdateColor();
				this.SelectedButtonIndex = -1;
			}
		}
		this.computerScreenText.text = (this.bError ? this.computerScreeErrorText : this.ComputerDefaultTextTitleDataValue);
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000D6534 File Offset: 0x000D4734
	private void RemoveBundle(string BundleID)
	{
		CosmeticsController.CosmeticItem itemFromDict = CosmeticsController.instance.GetItemFromDict(BundleID);
		if (itemFromDict.isNullItem)
		{
			return;
		}
		foreach (string itemName in itemFromDict.bundledItems)
		{
			CosmeticsController.instance.RemoveCosmeticItemFromSet(CosmeticsController.instance.tryOnSet, itemName, false);
		}
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000D658C File Offset: 0x000D478C
	private void TryOnBundle(string BundleID)
	{
		CosmeticsController.CosmeticItem itemFromDict = CosmeticsController.instance.GetItemFromDict(BundleID);
		if (itemFromDict.isNullItem)
		{
			return;
		}
		foreach (CosmeticsController.CosmeticItem cosmeticItem in CosmeticsController.instance.tryOnSet.items)
		{
			if (!itemFromDict.bundledItems.Contains(cosmeticItem.itemName))
			{
				CosmeticsController.instance.RemoveCosmeticItemFromSet(CosmeticsController.instance.tryOnSet, cosmeticItem.itemName, false);
			}
		}
		foreach (string text in itemFromDict.bundledItems)
		{
			if (!CosmeticsController.instance.tryOnSet.HasItem(text))
			{
				CosmeticsController.instance.ApplyCosmeticItemToSet(CosmeticsController.instance.tryOnSet, CosmeticsController.instance.GetItemFromDict(text), false, false);
			}
		}
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000D6664 File Offset: 0x000D4864
	public void PressTryOnBundleButton(TryOnBundleButton pressedTryOnBundleButton, bool isLeftHand)
	{
		if (pressedTryOnBundleButton.playfabBundleID == "NULL")
		{
			Debug.LogError("TryOnBundlesStand - PressTryOnBundleButton - Invalid bundle ID");
			return;
		}
		if (CosmeticsController.instance.GetItemFromDict(pressedTryOnBundleButton.playfabBundleID).isNullItem)
		{
			Debug.LogError("TryOnBundlesStand - PressTryOnBundleButton - Bundle is Null + " + pressedTryOnBundleButton.playfabBundleID);
			return;
		}
		if (this.SelectedButtonIndex != pressedTryOnBundleButton.buttonIndex)
		{
			this.ClearSelectedBundle();
		}
		switch (CosmeticsController.instance.CheckIfCosmeticSetMatchesItemSet(CosmeticsController.instance.tryOnSet, pressedTryOnBundleButton.playfabBundleID))
		{
		case CosmeticsController.EWearingCosmeticSet.NotASet:
			Debug.LogError("TryOnBundlesStand - PressTryOnBundleButton - Item is Not A Set");
			break;
		case CosmeticsController.EWearingCosmeticSet.NotWearing:
			this.TryOnBundle(pressedTryOnBundleButton.playfabBundleID);
			this.SelectedButtonIndex = pressedTryOnBundleButton.buttonIndex;
			break;
		case CosmeticsController.EWearingCosmeticSet.Partial:
			if (pressedTryOnBundleButton.isOn)
			{
				this.ClearSelectedBundle();
			}
			else
			{
				this.TryOnBundle(pressedTryOnBundleButton.playfabBundleID);
				this.SelectedButtonIndex = pressedTryOnBundleButton.buttonIndex;
			}
			break;
		case CosmeticsController.EWearingCosmeticSet.Complete:
			this.ClearSelectedBundle();
			break;
		}
		if (this.SelectedButtonIndex != -1)
		{
			if (!this.bError)
			{
				this.selectedBundleImage.sprite = BundleManager.instance.storeBundlesById[pressedTryOnBundleButton.playfabBundleID].bundleImage;
				pressedTryOnBundleButton.isOn = true;
				this.purchaseButton.offText = this.GetPurchaseButtonText(pressedTryOnBundleButton.playfabBundleID);
				this.computerScreenText.text = this.GetComputerScreenText(pressedTryOnBundleButton.playfabBundleID);
				this.AlreadyOwnCheck();
			}
			pressedTryOnBundleButton.UpdateColor();
		}
		else
		{
			if (!this.bError)
			{
				this.computerScreenText.text = this.ComputerDefaultTextTitleDataValue;
				this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
			}
			pressedTryOnBundleButton.isOn = false;
			this.selectedBundleImage.sprite = null;
			this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
			this.purchaseButton.ResetButton();
			this.purchaseButton.UpdateColor();
		}
		CosmeticsController.instance.UpdateShoppingCart();
		CosmeticsController.instance.UpdateWornCosmetics(true);
		pressedTryOnBundleButton.UpdateColor();
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x00041EF9 File Offset: 0x000400F9
	private string GetComputerScreenText(string playfabBundleID)
	{
		return BundleManager.instance.storeBundlesById[playfabBundleID].bundleDescriptionText;
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00041F12 File Offset: 0x00040112
	private string GetPurchaseButtonText(string playfabBundleID)
	{
		return BundleManager.instance.storeBundlesById[playfabBundleID].purchaseButtonText;
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x00041F2B File Offset: 0x0004012B
	public void PurchaseButtonPressed()
	{
		if (this.SelectedButtonIndex == -1)
		{
			return;
		}
		CosmeticsController.instance.PurchaseBundle(BundleManager.instance.storeBundlesById[this.SelectedBundlePlayFabID]);
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000D685C File Offset: 0x000D4A5C
	public void AlreadyOwnCheck()
	{
		if (this.SelectedButtonIndex == -1)
		{
			return;
		}
		if (BundleManager.instance.storeBundlesById[this.SelectedBundlePlayFabID].isOwned)
		{
			this.purchaseButton.AlreadyOwn();
			if (!this.bError)
			{
				this.computerScreenText.text = this.ComputerAlreadyOwnTextTitleDataValue;
				return;
			}
		}
		else
		{
			if (!this.bError)
			{
				this.computerScreenText.text = this.GetBundleComputerText(this.SelectedBundlePlayFabID);
			}
			this.purchaseButton.UpdateColor();
		}
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000D68E0 File Offset: 0x000D4AE0
	public void GetTryOnButtons()
	{
		StoreBundleData[] tryOnButtons = BundleManager.instance.GetTryOnButtons();
		for (int i = 0; i < this.TryOnBundleButtons.Length; i++)
		{
			if (i < tryOnButtons.Length)
			{
				if (tryOnButtons[i] != null && tryOnButtons[i].playfabBundleID != "NULL" && tryOnButtons[i].bundleImage != null)
				{
					this.TryOnBundleButtons[i].playfabBundleID = tryOnButtons[i].playfabBundleID;
					this.BundleIcons[i].sprite = tryOnButtons[i].bundleImage;
				}
				else
				{
					this.TryOnBundleButtons[i].playfabBundleID = "NULL";
					this.BundleIcons[i].sprite = null;
				}
			}
			else
			{
				this.TryOnBundleButtons[i].playfabBundleID = "NULL";
				this.BundleIcons[i].sprite = null;
			}
			this.TryOnBundleButtons[i].UpdateColor();
		}
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x00041F5A File Offset: 0x0004015A
	public void UpdateBundles(StoreBundleData[] Bundles)
	{
		Debug.LogWarning("TryOnBundlesStand - UpdateBundles is an editor only function!");
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000D69C4 File Offset: 0x000D4BC4
	private string GetBundleComputerText(string PlayFabID)
	{
		StoreBundle storeBundle;
		if (BundleManager.instance.storeBundlesById.TryGetValue(PlayFabID, out storeBundle))
		{
			return storeBundle.bundleDescriptionText;
		}
		return "ERROR THIS DOES NOT EXIST YET";
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x00041F66 File Offset: 0x00040166
	public void ErrorCompleting()
	{
		this.bError = true;
		this.purchaseButton.ErrorHappened();
		this.computerScreenText.text = this.computerScreeErrorText;
	}

	// Token: 0x04001D4B RID: 7499
	[SerializeField]
	private TryOnBundleButton[] TryOnBundleButtons;

	// Token: 0x04001D4C RID: 7500
	[SerializeField]
	private Image[] BundleIcons;

	// Token: 0x04001D4D RID: 7501
	[Header("The Index of the Selected Bundle from CosmeticsBundle Array in CosmeticsController")]
	private int SelectedButtonIndex = -1;

	// Token: 0x04001D4E RID: 7502
	public TryOnPurchaseButton purchaseButton;

	// Token: 0x04001D4F RID: 7503
	public Image selectedBundleImage;

	// Token: 0x04001D50 RID: 7504
	public Text computerScreenText;

	// Token: 0x04001D51 RID: 7505
	public string ComputerDefaultTextTitleDataKey;

	// Token: 0x04001D52 RID: 7506
	[SerializeField]
	private string ComputerDefaultTextTitleDataValue = "";

	// Token: 0x04001D53 RID: 7507
	public string ComputerAlreadyOwnTextTitleDataKey;

	// Token: 0x04001D54 RID: 7508
	[SerializeField]
	private string ComputerAlreadyOwnTextTitleDataValue = "";

	// Token: 0x04001D55 RID: 7509
	public string PurchaseButtonDefaultTextTitleDataKey;

	// Token: 0x04001D56 RID: 7510
	[SerializeField]
	private string PurchaseButtonDefaultTextTitleDataValue = "";

	// Token: 0x04001D57 RID: 7511
	public string PurchaseButtonAlreadyOwnTextTitleDataKey;

	// Token: 0x04001D58 RID: 7512
	[SerializeField]
	private string PurchaseButtonAlreadyOwnTextTitleDataValue = "";

	// Token: 0x04001D59 RID: 7513
	private bool bError;

	// Token: 0x04001D5A RID: 7514
	[Header("Error Text for Computer Screen")]
	public string computerScreeErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME, AND MAKE SURE YOU HAVE A STABLE INTERNET CONNECTION. ";

	// Token: 0x04001D5B RID: 7515
	private List<StoreBundle> storeBundles = new List<StoreBundle>();
}
