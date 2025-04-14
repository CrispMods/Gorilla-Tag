using System;
using System.Collections.Generic;
using System.Linq;
using GorillaNetworking;
using GorillaNetworking.Store;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000424 RID: 1060
public class TryOnBundlesStand : MonoBehaviour
{
	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00080B9C File Offset: 0x0007ED9C
	private string SelectedBundlePlayFabID
	{
		get
		{
			return this.TryOnBundleButtons[this.SelectedButtonIndex].playfabBundleID;
		}
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x00080BB0 File Offset: 0x0007EDB0
	public static string CleanUpTitleDataValues(string titleDataResult)
	{
		string text = titleDataResult.Replace("\\r", "\r").Replace("\\n", "\n");
		if (text[0] == '"' && text[text.Length - 1] == '"')
		{
			text = text.Substring(1, text.Length - 2);
		}
		return text;
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x00080C0C File Offset: 0x0007EE0C
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

	// Token: 0x06001A2C RID: 6700 RVA: 0x00080C60 File Offset: 0x0007EE60
	private void Start()
	{
		PlayFabTitleDataCache.Instance.GetTitleData(this.ComputerDefaultTextTitleDataKey, new Action<string>(this.OnComputerDefaultTextTitleDataSuccess), new Action<PlayFabError>(this.OnComputerDefaultTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.ComputerAlreadyOwnTextTitleDataKey, new Action<string>(this.OnComputerAlreadyOwnTextTitleDataSuccess), new Action<PlayFabError>(this.OnComputerAlreadyOwnTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.PurchaseButtonDefaultTextTitleDataKey, new Action<string>(this.OnPurchaseButtonDefaultTextTitleDataSuccess), new Action<PlayFabError>(this.OnPurchaseButtonDefaultTextTitleDataFailure));
		PlayFabTitleDataCache.Instance.GetTitleData(this.PurchaseButtonAlreadyOwnTextTitleDataKey, new Action<string>(this.OnPurchaseButtonAlreadyOwnTextTitleDataSuccess), new Action<PlayFabError>(this.OnPurchaseButtonAlreadyOwnTextTitleDataFailure));
		this.InitalizeButtons();
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x00080D13 File Offset: 0x0007EF13
	private void OnComputerDefaultTextTitleDataSuccess(string data)
	{
		this.ComputerDefaultTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.computerScreenText.text = this.ComputerDefaultTextTitleDataValue;
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x00080D32 File Offset: 0x0007EF32
	private void OnComputerDefaultTextTitleDataFailure(PlayFabError error)
	{
		this.ComputerDefaultTextTitleDataValue = "Failed to get TD Key : " + this.ComputerDefaultTextTitleDataKey;
		this.computerScreenText.text = this.ComputerDefaultTextTitleDataValue;
		Debug.LogError(string.Format("Error getting Computer Screen Title Data: {0}", error));
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x00080D6B File Offset: 0x0007EF6B
	private void OnComputerAlreadyOwnTextTitleDataSuccess(string data)
	{
		this.ComputerAlreadyOwnTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x00080D79 File Offset: 0x0007EF79
	private void OnComputerAlreadyOwnTextTitleDataFailure(PlayFabError error)
	{
		this.ComputerAlreadyOwnTextTitleDataValue = "Failed to get TD Key : " + this.ComputerAlreadyOwnTextTitleDataKey;
		Debug.LogError(string.Format("Error getting Computer Already Screen Title Data: {0}", error));
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x00080DA1 File Offset: 0x0007EFA1
	private void OnPurchaseButtonDefaultTextTitleDataSuccess(string data)
	{
		this.PurchaseButtonDefaultTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
		this.purchaseButton.UpdateColor();
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x00080DCC File Offset: 0x0007EFCC
	private void OnPurchaseButtonDefaultTextTitleDataFailure(PlayFabError error)
	{
		this.PurchaseButtonDefaultTextTitleDataValue = "Failed to get TD Key : " + this.PurchaseButtonDefaultTextTitleDataKey;
		this.purchaseButton.offText = this.PurchaseButtonDefaultTextTitleDataValue;
		this.purchaseButton.UpdateColor();
		Debug.LogError(string.Format("Error getting Tryon Purchase Button Default Text Title Data: {0}", error));
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x00080E1B File Offset: 0x0007F01B
	private void OnPurchaseButtonAlreadyOwnTextTitleDataSuccess(string data)
	{
		this.PurchaseButtonAlreadyOwnTextTitleDataValue = TryOnBundlesStand.CleanUpTitleDataValues(data);
		this.purchaseButton.AlreadyOwnText = this.PurchaseButtonAlreadyOwnTextTitleDataValue;
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x00080E3A File Offset: 0x0007F03A
	private void OnPurchaseButtonAlreadyOwnTextTitleDataFailure(PlayFabError error)
	{
		this.PurchaseButtonAlreadyOwnTextTitleDataValue = "Failed to get TD Key : " + this.PurchaseButtonAlreadyOwnTextTitleDataKey;
		this.purchaseButton.AlreadyOwnText = this.PurchaseButtonAlreadyOwnTextTitleDataValue;
		Debug.LogError(string.Format("Error getting Tryon Purchase Button Already Own Text Title Data: {0}", error));
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x00080E74 File Offset: 0x0007F074
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

	// Token: 0x06001A36 RID: 6710 RVA: 0x00080F4C File Offset: 0x0007F14C
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

	// Token: 0x06001A37 RID: 6711 RVA: 0x00080FA4 File Offset: 0x0007F1A4
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

	// Token: 0x06001A38 RID: 6712 RVA: 0x0008107C File Offset: 0x0007F27C
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

	// Token: 0x06001A39 RID: 6713 RVA: 0x00081274 File Offset: 0x0007F474
	private string GetComputerScreenText(string playfabBundleID)
	{
		return BundleManager.instance.storeBundlesById[playfabBundleID].bundleDescriptionText;
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x0008128D File Offset: 0x0007F48D
	private string GetPurchaseButtonText(string playfabBundleID)
	{
		return BundleManager.instance.storeBundlesById[playfabBundleID].purchaseButtonText;
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000812A6 File Offset: 0x0007F4A6
	public void PurchaseButtonPressed()
	{
		if (this.SelectedButtonIndex == -1)
		{
			return;
		}
		CosmeticsController.instance.PurchaseBundle(BundleManager.instance.storeBundlesById[this.SelectedBundlePlayFabID]);
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000812D8 File Offset: 0x0007F4D8
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

	// Token: 0x06001A3D RID: 6717 RVA: 0x0008135C File Offset: 0x0007F55C
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

	// Token: 0x06001A3E RID: 6718 RVA: 0x0008143F File Offset: 0x0007F63F
	public void UpdateBundles(StoreBundleData[] Bundles)
	{
		Debug.LogWarning("TryOnBundlesStand - UpdateBundles is an editor only function!");
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x0008144C File Offset: 0x0007F64C
	private string GetBundleComputerText(string PlayFabID)
	{
		StoreBundle storeBundle;
		if (BundleManager.instance.storeBundlesById.TryGetValue(PlayFabID, out storeBundle))
		{
			return storeBundle.bundleDescriptionText;
		}
		return "ERROR THIS DOES NOT EXIST YET";
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0008147B File Offset: 0x0007F67B
	public void ErrorCompleting()
	{
		this.bError = true;
		this.purchaseButton.ErrorHappened();
		this.computerScreenText.text = this.computerScreeErrorText;
	}

	// Token: 0x04001CFC RID: 7420
	[SerializeField]
	private TryOnBundleButton[] TryOnBundleButtons;

	// Token: 0x04001CFD RID: 7421
	[SerializeField]
	private Image[] BundleIcons;

	// Token: 0x04001CFE RID: 7422
	[Header("The Index of the Selected Bundle from CosmeticsBundle Array in CosmeticsController")]
	private int SelectedButtonIndex = -1;

	// Token: 0x04001CFF RID: 7423
	public TryOnPurchaseButton purchaseButton;

	// Token: 0x04001D00 RID: 7424
	public Image selectedBundleImage;

	// Token: 0x04001D01 RID: 7425
	public Text computerScreenText;

	// Token: 0x04001D02 RID: 7426
	public string ComputerDefaultTextTitleDataKey;

	// Token: 0x04001D03 RID: 7427
	[SerializeField]
	private string ComputerDefaultTextTitleDataValue = "";

	// Token: 0x04001D04 RID: 7428
	public string ComputerAlreadyOwnTextTitleDataKey;

	// Token: 0x04001D05 RID: 7429
	[SerializeField]
	private string ComputerAlreadyOwnTextTitleDataValue = "";

	// Token: 0x04001D06 RID: 7430
	public string PurchaseButtonDefaultTextTitleDataKey;

	// Token: 0x04001D07 RID: 7431
	[SerializeField]
	private string PurchaseButtonDefaultTextTitleDataValue = "";

	// Token: 0x04001D08 RID: 7432
	public string PurchaseButtonAlreadyOwnTextTitleDataKey;

	// Token: 0x04001D09 RID: 7433
	[SerializeField]
	private string PurchaseButtonAlreadyOwnTextTitleDataValue = "";

	// Token: 0x04001D0A RID: 7434
	private bool bError;

	// Token: 0x04001D0B RID: 7435
	[Header("Error Text for Computer Screen")]
	public string computerScreeErrorText = "ERROR COMPLETING PURCHASE! PLEASE RESTART THE GAME, AND MAKE SURE YOU HAVE A STABLE INTERNET CONNECTION. ";

	// Token: 0x04001D0C RID: 7436
	private List<StoreBundle> storeBundles = new List<StoreBundle>();
}
