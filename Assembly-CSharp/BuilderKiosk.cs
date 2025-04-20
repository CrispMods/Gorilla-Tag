using System;
using System.Collections;
using System.Collections.Generic;
using GameObjectScheduling;
using GorillaNetworking;
using GorillaTagScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004CF RID: 1231
public class BuilderKiosk : MonoBehaviour
{
	// Token: 0x06001DE4 RID: 7652 RVA: 0x000E317C File Offset: 0x000E137C
	private void Awake()
	{
		BuilderKiosk.nullItem = new BuilderSetManager.BuilderSetStoreItem
		{
			displayName = "NOTHING",
			playfabID = "NULL",
			isNullItem = true
		};
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000E31B8 File Offset: 0x000E13B8
	private void Start()
	{
		this.purchaseParticles.Stop();
		BuilderSetManager.instance.OnOwnedSetsUpdated.AddListener(new UnityAction(this.OnOwnedSetsUpdated));
		CosmeticsController instance = CosmeticsController.instance;
		instance.OnGetCurrency = (Action)Delegate.Combine(instance.OnGetCurrency, new Action(this.OnUpdateCurrencyBalance));
		this.leftPurchaseButton.onPressed += this.PressLeftPurchaseItemButton;
		this.rightPurchaseButton.onPressed += this.PressRightPurchaseItemButton;
		BuilderTable.instance.OnTableConfigurationUpdated.AddListener(new UnityAction(this.UpdateCountdown));
		this.UpdateCountdown();
		this.availableItems.Clear();
		if (this.isMiniKiosk)
		{
			this.availableItems.Add(this.pieceSetForSale);
		}
		else
		{
			this.availableItems.AddRange(BuilderSetManager.instance.GetPermanentSetsForSale());
			this.availableItems.AddRange(BuilderSetManager.instance.GetSeasonalSetsForSale());
		}
		if (!this.isMiniKiosk)
		{
			this.SetupSetButtons();
		}
		if (this.availableItems.Count <= 0 || !BuilderSetManager.instance.pulledStoreItems)
		{
			this.itemToBuy = BuilderKiosk.nullItem;
			return;
		}
		this.hasInitFromPlayfab = true;
		if (this.pieceSetForSale != null)
		{
			this.itemToBuy = BuilderSetManager.instance.GetStoreItemFromSetID(this.pieceSetForSale.GetIntIdentifier());
			this.UpdateLabels();
			this.UpdateDiorama();
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
			this.ProcessPurchaseItemState(null, false);
			return;
		}
		this.itemToBuy = BuilderKiosk.nullItem;
		this.UpdateLabels();
		this.UpdateDiorama();
		this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
		this.ProcessPurchaseItemState(null, false);
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000E3368 File Offset: 0x000E1568
	private void UpdateCountdown()
	{
		if (!this.useTitleCountDown)
		{
			return;
		}
		if (!string.IsNullOrEmpty(BuilderTable.nextUpdateOverride) && !BuilderTable.nextUpdateOverride.Equals(this.countdownOverride))
		{
			this.countdownOverride = BuilderTable.nextUpdateOverride;
			CountdownTextDate countdown = this.countdownText.Countdown;
			countdown.CountdownTo = this.countdownOverride;
			this.countdownText.Countdown = countdown;
		}
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000E33CC File Offset: 0x000E15CC
	private void SetupSetButtons()
	{
		this.setsPerPage = this.setButtons.Length;
		this.totalPages = this.availableItems.Count / this.setsPerPage;
		if (this.availableItems.Count % this.setsPerPage > 0)
		{
			this.totalPages++;
		}
		this.previousPageButton.gameObject.SetActive(this.totalPages > 1);
		this.nextPageButton.gameObject.SetActive(this.totalPages > 1);
		this.previousPageButton.myTmpText.enabled = (this.totalPages > 1);
		this.nextPageButton.myTmpText.enabled = (this.totalPages > 1);
		this.previousPageButton.onPressButton.AddListener(new UnityAction(this.OnPreviousPageClicked));
		this.nextPageButton.onPressButton.AddListener(new UnityAction(this.OnNextPageClicked));
		GorillaPressableButton[] array = this.setButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].onPressed += this.OnSetButtonPressed;
		}
		this.UpdateLabels();
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000E34F0 File Offset: 0x000E16F0
	private void OnDestroy()
	{
		if (this.leftPurchaseButton != null)
		{
			this.leftPurchaseButton.onPressed -= this.PressLeftPurchaseItemButton;
		}
		if (this.rightPurchaseButton != null)
		{
			this.rightPurchaseButton.onPressed -= this.PressRightPurchaseItemButton;
		}
		if (BuilderSetManager.instance != null)
		{
			BuilderSetManager.instance.OnOwnedSetsUpdated.RemoveListener(new UnityAction(this.OnOwnedSetsUpdated));
		}
		if (CosmeticsController.instance != null)
		{
			CosmeticsController instance = CosmeticsController.instance;
			instance.OnGetCurrency = (Action)Delegate.Remove(instance.OnGetCurrency, new Action(this.OnUpdateCurrencyBalance));
		}
		if (!this.isMiniKiosk)
		{
			foreach (GorillaPressableButton gorillaPressableButton in this.setButtons)
			{
				if (gorillaPressableButton != null)
				{
					gorillaPressableButton.onPressed -= this.OnSetButtonPressed;
				}
			}
			if (this.previousPageButton != null)
			{
				this.previousPageButton.onPressButton.RemoveListener(new UnityAction(this.OnPreviousPageClicked));
			}
			if (this.nextPageButton != null)
			{
				this.nextPageButton.onPressButton.RemoveListener(new UnityAction(this.OnNextPageClicked));
			}
		}
		if (this.currentDiorama != null)
		{
			UnityEngine.Object.Destroy(this.currentDiorama);
			this.currentDiorama = null;
		}
		if (this.nextDiorama != null)
		{
			UnityEngine.Object.Destroy(this.nextDiorama);
			this.nextDiorama = null;
		}
		if (BuilderTable.instance != null)
		{
			BuilderTable.instance.OnTableConfigurationUpdated.RemoveListener(new UnityAction(this.UpdateCountdown));
		}
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x000E36A8 File Offset: 0x000E18A8
	private void OnOwnedSetsUpdated()
	{
		if (this.hasInitFromPlayfab || !BuilderSetManager.instance.pulledStoreItems)
		{
			if (this.currentPurchaseItemStage == CosmeticsController.PurchaseItemStages.Start || this.currentPurchaseItemStage == CosmeticsController.PurchaseItemStages.CheckoutButtonPressed)
			{
				this.ProcessPurchaseItemState(null, false);
			}
			return;
		}
		this.hasInitFromPlayfab = true;
		this.availableItems.Clear();
		if (this.isMiniKiosk)
		{
			this.availableItems.Add(this.pieceSetForSale);
		}
		else
		{
			this.availableItems.AddRange(BuilderSetManager.instance.GetPermanentSetsForSale());
			this.availableItems.AddRange(BuilderSetManager.instance.GetSeasonalSetsForSale());
		}
		if (this.pieceSetForSale != null)
		{
			this.itemToBuy = BuilderSetManager.instance.GetStoreItemFromSetID(this.pieceSetForSale.GetIntIdentifier());
			this.UpdateLabels();
			this.UpdateDiorama();
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
			this.ProcessPurchaseItemState(null, false);
			return;
		}
		this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
		this.ProcessPurchaseItemState(null, false);
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x000E379C File Offset: 0x000E199C
	private void OnSetButtonPressed(GorillaPressableButton button, bool isLeft)
	{
		if (this.currentPurchaseItemStage != CosmeticsController.PurchaseItemStages.Buying && !this.animating)
		{
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
			int num = 0;
			for (int i = 0; i < this.setButtons.Length; i++)
			{
				if (button.Equals(this.setButtons[i]))
				{
					num = i;
					break;
				}
			}
			int num2 = this.pageIndex * this.setsPerPage + num;
			if (num2 < this.availableItems.Count)
			{
				BuilderPieceSet builderPieceSet = this.availableItems[num2];
				if (builderPieceSet.setName.Equals(this.itemToBuy.displayName))
				{
					this.itemToBuy = BuilderKiosk.nullItem;
					this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
				}
				else
				{
					this.itemToBuy = BuilderSetManager.instance.GetStoreItemFromSetID(builderPieceSet.GetIntIdentifier());
					this.UpdateLabels();
					this.UpdateDiorama();
				}
				this.ProcessPurchaseItemState(null, isLeft);
			}
		}
	}

	// Token: 0x06001DEB RID: 7659 RVA: 0x000E3874 File Offset: 0x000E1A74
	private void OnPreviousPageClicked()
	{
		int num = Mathf.Clamp(this.pageIndex - 1, 0, this.totalPages - 1);
		if (num != this.pageIndex)
		{
			this.pageIndex = num;
			this.UpdateLabels();
		}
	}

	// Token: 0x06001DEC RID: 7660 RVA: 0x000E38B0 File Offset: 0x000E1AB0
	private void OnNextPageClicked()
	{
		int num = Mathf.Clamp(this.pageIndex + 1, 0, this.totalPages - 1);
		if (num != this.pageIndex)
		{
			this.pageIndex = num;
			this.UpdateLabels();
		}
	}

	// Token: 0x06001DED RID: 7661 RVA: 0x000E38EC File Offset: 0x000E1AEC
	private void UpdateLabels()
	{
		if (this.isMiniKiosk)
		{
			return;
		}
		for (int i = 0; i < this.setButtons.Length; i++)
		{
			int num = this.pageIndex * this.setsPerPage + i;
			if (num < this.availableItems.Count && this.availableItems[num] != null)
			{
				if (!this.setButtons[i].gameObject.activeSelf)
				{
					this.setButtons[i].gameObject.SetActive(true);
					this.setButtons[i].myTmpText.gameObject.SetActive(true);
				}
				if (this.setButtons[i].myTmpText.text != this.availableItems[num].setName.ToUpper())
				{
					this.setButtons[i].myTmpText.text = this.availableItems[num].setName.ToUpper();
				}
				bool flag = !this.itemToBuy.isNullItem && this.availableItems[num].playfabID == this.itemToBuy.playfabID;
				if (flag != this.setButtons[i].isOn || !this.setButtons[i].enabled)
				{
					this.setButtons[i].isOn = flag;
					this.setButtons[i].buttonRenderer.material = (flag ? this.setButtons[i].pressedMaterial : this.setButtons[i].unpressedMaterial);
				}
				this.setButtons[i].enabled = true;
			}
			else
			{
				if (this.setButtons[i].gameObject.activeSelf)
				{
					this.setButtons[i].gameObject.SetActive(false);
					this.setButtons[i].myTmpText.gameObject.SetActive(false);
				}
				if (this.setButtons[i].isOn || this.setButtons[i].enabled)
				{
					this.setButtons[i].isOn = false;
					this.setButtons[i].enabled = false;
				}
			}
		}
		bool flag2 = this.pageIndex > 0 && this.totalPages > 1;
		bool flag3 = this.pageIndex < this.totalPages - 1 && this.totalPages > 1;
		if (this.previousPageButton.myTmpText.enabled != flag2)
		{
			this.previousPageButton.myTmpText.enabled = flag2;
		}
		if (this.nextPageButton.myTmpText.enabled != flag3)
		{
			this.nextPageButton.myTmpText.enabled = flag3;
		}
	}

	// Token: 0x06001DEE RID: 7662 RVA: 0x000E3B84 File Offset: 0x000E1D84
	public void UpdateDiorama()
	{
		if (this.isMiniKiosk)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.itemToBuy.isNullItem)
		{
			this.countdownText.gameObject.SetActive(false);
		}
		else
		{
			this.countdownText.gameObject.SetActive(BuilderSetManager.instance.IsSetSeasonal(this.itemToBuy.playfabID));
		}
		if (this.animating)
		{
			base.StopCoroutine(this.PlaySwapAnimation());
			if (this.currentDiorama != null)
			{
				UnityEngine.Object.Destroy(this.currentDiorama);
				this.currentDiorama = null;
			}
			this.currentDiorama = this.nextDiorama;
			this.nextDiorama = null;
		}
		this.animating = true;
		if (this.nextDiorama != null)
		{
			UnityEngine.Object.Destroy(this.nextDiorama);
			this.nextDiorama = null;
		}
		if (!this.itemToBuy.isNullItem && this.itemToBuy.displayModel != null)
		{
			this.nextDiorama = UnityEngine.Object.Instantiate<GameObject>(this.itemToBuy.displayModel, this.nextItemDisplayPos);
		}
		else
		{
			this.nextDiorama = UnityEngine.Object.Instantiate<GameObject>(this.emptyDisplay, this.nextItemDisplayPos);
		}
		this.itemDisplayAnimation.Rewind();
		if (this.currentDiorama != null)
		{
			this.currentDiorama.transform.SetParent(this.itemDisplayPos, false);
		}
		base.StartCoroutine(this.PlaySwapAnimation());
	}

	// Token: 0x06001DEF RID: 7663 RVA: 0x000446A6 File Offset: 0x000428A6
	private IEnumerator PlaySwapAnimation()
	{
		this.itemDisplayAnimation.Play();
		yield return new WaitForSeconds(this.itemDisplayAnimation.clip.length);
		if (this.currentDiorama != null)
		{
			UnityEngine.Object.Destroy(this.currentDiorama);
			this.currentDiorama = null;
		}
		this.currentDiorama = this.nextDiorama;
		this.nextDiorama = null;
		this.animating = false;
		yield break;
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000446B5 File Offset: 0x000428B5
	public void PressLeftPurchaseItemButton(GorillaPressableButton pressedPurchaseItemButton, bool isLeftHand)
	{
		if (this.currentPurchaseItemStage != CosmeticsController.PurchaseItemStages.Start && !this.animating)
		{
			this.ProcessPurchaseItemState("left", isLeftHand);
		}
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000446D3 File Offset: 0x000428D3
	public void PressRightPurchaseItemButton(GorillaPressableButton pressedPurchaseItemButton, bool isLeftHand)
	{
		if (this.currentPurchaseItemStage != CosmeticsController.PurchaseItemStages.Start && !this.animating)
		{
			this.ProcessPurchaseItemState("right", isLeftHand);
		}
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000446F1 File Offset: 0x000428F1
	public void OnUpdateCurrencyBalance()
	{
		if (this.currentPurchaseItemStage == CosmeticsController.PurchaseItemStages.Start || this.currentPurchaseItemStage == CosmeticsController.PurchaseItemStages.CheckoutButtonPressed || this.currentPurchaseItemStage == CosmeticsController.PurchaseItemStages.ItemOwned)
		{
			this.ProcessPurchaseItemState(null, false);
		}
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x00044715 File Offset: 0x00042915
	public void ClearCheckout()
	{
		GorillaTelemetry.PostBuilderKioskEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_cancel, this.itemToBuy);
		this.itemToBuy = BuilderKiosk.nullItem;
		this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x000E3CF0 File Offset: 0x000E1EF0
	public void ProcessPurchaseItemState(string buttonSide, bool isLeftHand)
	{
		switch (this.currentPurchaseItemStage)
		{
		case CosmeticsController.PurchaseItemStages.Start:
			this.itemToBuy = BuilderKiosk.nullItem;
			this.FormattedPurchaseText("SELECT AN ITEM TO PURCHASE!");
			this.leftPurchaseButton.myTmpText.text = "-";
			this.rightPurchaseButton.myTmpText.text = "-";
			this.UpdateLabels();
			this.UpdateDiorama();
			return;
		case CosmeticsController.PurchaseItemStages.CheckoutButtonPressed:
			if (this.availableItems.Count > 1)
			{
				GorillaTelemetry.PostBuilderKioskEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.checkout_start, this.itemToBuy);
			}
			if (BuilderSetManager.instance.IsPieceSetOwnedLocally(this.itemToBuy.setID))
			{
				this.FormattedPurchaseText("YOU ALREADY OWN THIS ITEM!");
				this.leftPurchaseButton.myTmpText.text = "-";
				this.rightPurchaseButton.myTmpText.text = "-";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.ItemOwned;
				return;
			}
			if ((ulong)this.itemToBuy.cost <= (ulong)((long)CosmeticsController.instance.currencyBalance))
			{
				this.FormattedPurchaseText("DO YOU WANT TO BUY THIS ITEM?");
				this.leftPurchaseButton.myTmpText.text = "NO!";
				this.rightPurchaseButton.myTmpText.text = "YES!";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.unpressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.unpressedMaterial;
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.ItemSelected;
				return;
			}
			this.FormattedPurchaseText("INSUFFICIENT SHINY ROCKS FOR THIS ITEM!");
			this.leftPurchaseButton.myTmpText.text = "-";
			this.rightPurchaseButton.myTmpText.text = "-";
			this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
			this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
			if (!this.isMiniKiosk)
			{
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Start;
				return;
			}
			break;
		case CosmeticsController.PurchaseItemStages.ItemSelected:
			if (buttonSide == "right")
			{
				GorillaTelemetry.PostBuilderKioskEvent(GorillaTagger.Instance.offlineVRRig, GTShopEventType.item_select, this.itemToBuy);
				this.FormattedPurchaseText("ARE YOU REALLY SURE?");
				this.leftPurchaseButton.myTmpText.text = "YES! I NEED IT!";
				this.rightPurchaseButton.myTmpText.text = "LET ME THINK ABOUT IT";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.unpressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.unpressedMaterial;
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.FinalPurchaseAcknowledgement;
				return;
			}
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
			this.ProcessPurchaseItemState(null, isLeftHand);
			return;
		case CosmeticsController.PurchaseItemStages.ItemOwned:
		case CosmeticsController.PurchaseItemStages.Buying:
			break;
		case CosmeticsController.PurchaseItemStages.FinalPurchaseAcknowledgement:
			if (buttonSide == "left")
			{
				this.FormattedPurchaseText("PURCHASING ITEM...");
				this.leftPurchaseButton.myTmpText.text = "-";
				this.rightPurchaseButton.myTmpText.text = "-";
				this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
				this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Buying;
				this.isLastHandTouchedLeft = isLeftHand;
				this.PurchaseItem();
				return;
			}
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.CheckoutButtonPressed;
			this.ProcessPurchaseItemState(null, isLeftHand);
			return;
		case CosmeticsController.PurchaseItemStages.Success:
		{
			this.FormattedPurchaseText("SUCCESS! YOU CAN NOW SELECT " + this.itemToBuy.displayName.ToUpper() + " AT SHELVES.");
			this.audioSource.GTPlayOneShot(this.purchaseSetAudioClip, 1f);
			this.purchaseParticles.Play();
			VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			offlineVRRig.concatStringOfCosmeticsAllowed += this.itemToBuy.playfabID;
			this.leftPurchaseButton.myTmpText.text = "-";
			this.rightPurchaseButton.myTmpText.text = "-";
			this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
			this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
			break;
		}
		case CosmeticsController.PurchaseItemStages.Failure:
			this.FormattedPurchaseText("ERROR IN PURCHASING ITEM! NO MONEY WAS SPENT. SELECT ANOTHER ITEM.");
			this.leftPurchaseButton.myTmpText.text = "-";
			this.rightPurchaseButton.myTmpText.text = "-";
			this.leftPurchaseButton.buttonRenderer.material = this.leftPurchaseButton.pressedMaterial;
			this.rightPurchaseButton.buttonRenderer.material = this.rightPurchaseButton.pressedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000E41B4 File Offset: 0x000E23B4
	public void FormattedPurchaseText(string finalLineVar)
	{
		this.finalLine = finalLineVar;
		this.purchaseText.text = string.Concat(new string[]
		{
			"ITEM: ",
			this.itemToBuy.displayName.ToUpper(),
			"\nITEM COST: ",
			this.itemToBuy.cost.ToString(),
			"\nYOU HAVE: ",
			CosmeticsController.instance.currencyBalance.ToString(),
			"\n\n",
			this.finalLine
		});
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x0004473F File Offset: 0x0004293F
	public void PurchaseItem()
	{
		BuilderSetManager.instance.TryPurchaseItem(this.itemToBuy.setID, delegate(bool result)
		{
			if (result)
			{
				this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Success;
				CosmeticsController.instance.currencyBalance -= (int)this.itemToBuy.cost;
				this.ProcessPurchaseItemState(null, this.isLastHandTouchedLeft);
				return;
			}
			this.currentPurchaseItemStage = CosmeticsController.PurchaseItemStages.Failure;
			this.ProcessPurchaseItemState(null, false);
		});
	}

	// Token: 0x040020FA RID: 8442
	public BuilderPieceSet pieceSetForSale;

	// Token: 0x040020FB RID: 8443
	public GorillaPressableButton leftPurchaseButton;

	// Token: 0x040020FC RID: 8444
	public GorillaPressableButton rightPurchaseButton;

	// Token: 0x040020FD RID: 8445
	public TMP_Text purchaseText;

	// Token: 0x040020FE RID: 8446
	[SerializeField]
	private bool isMiniKiosk;

	// Token: 0x040020FF RID: 8447
	[SerializeField]
	private bool useTitleCountDown = true;

	// Token: 0x04002100 RID: 8448
	[Header("Buttons")]
	[SerializeField]
	private GorillaPressableButton[] setButtons;

	// Token: 0x04002101 RID: 8449
	[SerializeField]
	private GorillaPressableButton previousPageButton;

	// Token: 0x04002102 RID: 8450
	[SerializeField]
	private GorillaPressableButton nextPageButton;

	// Token: 0x04002103 RID: 8451
	private BuilderPieceSet currentSet;

	// Token: 0x04002104 RID: 8452
	private int pageIndex;

	// Token: 0x04002105 RID: 8453
	private int setsPerPage = 3;

	// Token: 0x04002106 RID: 8454
	private int totalPages = 1;

	// Token: 0x04002107 RID: 8455
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04002108 RID: 8456
	[SerializeField]
	private AudioClip purchaseSetAudioClip;

	// Token: 0x04002109 RID: 8457
	[SerializeField]
	private ParticleSystem purchaseParticles;

	// Token: 0x0400210A RID: 8458
	[SerializeField]
	private GameObject emptyDisplay;

	// Token: 0x0400210B RID: 8459
	private List<BuilderPieceSet> availableItems = new List<BuilderPieceSet>(10);

	// Token: 0x0400210C RID: 8460
	internal CosmeticsController.PurchaseItemStages currentPurchaseItemStage;

	// Token: 0x0400210D RID: 8461
	private bool hasInitFromPlayfab;

	// Token: 0x0400210E RID: 8462
	internal BuilderSetManager.BuilderSetStoreItem itemToBuy;

	// Token: 0x0400210F RID: 8463
	public static BuilderSetManager.BuilderSetStoreItem nullItem;

	// Token: 0x04002110 RID: 8464
	private GameObject currentDiorama;

	// Token: 0x04002111 RID: 8465
	private GameObject nextDiorama;

	// Token: 0x04002112 RID: 8466
	private bool animating;

	// Token: 0x04002113 RID: 8467
	[SerializeField]
	private Transform itemDisplayPos;

	// Token: 0x04002114 RID: 8468
	[SerializeField]
	private Transform nextItemDisplayPos;

	// Token: 0x04002115 RID: 8469
	[SerializeField]
	private Animation itemDisplayAnimation;

	// Token: 0x04002116 RID: 8470
	[SerializeField]
	private CountdownText countdownText;

	// Token: 0x04002117 RID: 8471
	private string countdownOverride = string.Empty;

	// Token: 0x04002118 RID: 8472
	private bool isLastHandTouchedLeft;

	// Token: 0x04002119 RID: 8473
	private string finalLine;
}
