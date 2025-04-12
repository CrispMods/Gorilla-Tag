using System;
using System.Collections;
using System.Collections.Generic;
using GorillaNetworking;
using GorillaNetworking.Store;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200041E RID: 1054
public class ATM_Manager : MonoBehaviour
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0004097A File Offset: 0x0003EB7A
	// (set) Token: 0x06001A0F RID: 6671 RVA: 0x00040982 File Offset: 0x0003EB82
	public string ValidatedCreatorCode { get; set; }

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0004098B File Offset: 0x0003EB8B
	public ATM_Manager.ATMStages CurrentATMStage
	{
		get
		{
			return this.currentATMStage;
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x000D24D4 File Offset: 0x000D06D4
	public void Awake()
	{
		if (ATM_Manager.instance)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			ATM_Manager.instance = this;
		}
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeTitle.text = "CREATOR CODE: ";
		}
		this.SwitchToStage(ATM_Manager.ATMStages.Unavailable);
		this.smallDisplays = new List<CreatorCodeSmallDisplay>();
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x000D2538 File Offset: 0x000D0738
	public void Start()
	{
		Debug.Log("ATM COUNT: " + this.atmUIs.Length.ToString());
		Debug.Log("SMALL DISPLAY COUNT: " + this.smallDisplays.Count.ToString());
		GameEvents.OnGorrillaATMKeyButtonPressedEvent.AddListener(new UnityAction<GorillaATMKeyBindings>(this.PressButton));
		this.currentCreatorCode = "";
		if (PlayerPrefs.HasKey("CodeUsedTime"))
		{
			this.codeFirstUsedTime = PlayerPrefs.GetString("CodeUsedTime");
			DateTime d = DateTime.Parse(this.codeFirstUsedTime);
			if ((DateTime.Now - d).TotalDays > 14.0)
			{
				PlayerPrefs.SetString("CreatorCode", "");
			}
			else
			{
				this.currentCreatorCode = PlayerPrefs.GetString("CreatorCode", "");
				this.initialCode = this.currentCreatorCode;
				Debug.Log("Initial code: " + this.initialCode);
				if (string.IsNullOrEmpty(this.currentCreatorCode))
				{
					this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Empty;
				}
				else
				{
					this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Unchecked;
				}
				foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
				{
					creatorCodeSmallDisplay.SetCode(this.currentCreatorCode);
				}
			}
		}
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeField.text = this.currentCreatorCode;
		}
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x000D26CC File Offset: 0x000D08CC
	public void PressButton(GorillaATMKeyBindings buttonPressed)
	{
		if (this.currentATMStage == ATM_Manager.ATMStages.Confirm && this.creatorCodeStatus != ATM_Manager.CreatorCodeStatus.Validating)
		{
			ATM_UI[] array = this.atmUIs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].creatorCodeTitle.text = "CREATOR CODE:";
			}
			if (buttonPressed == GorillaATMKeyBindings.delete)
			{
				if (this.currentCreatorCode.Length > 0)
				{
					this.currentCreatorCode = this.currentCreatorCode.Substring(0, this.currentCreatorCode.Length - 1);
					if (this.currentCreatorCode.Length == 0)
					{
						this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Empty;
						this.ValidatedCreatorCode = "";
						foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
						{
							creatorCodeSmallDisplay.SetCode("");
						}
						PlayerPrefs.SetString("CreatorCode", "");
						PlayerPrefs.Save();
					}
					else
					{
						this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Unchecked;
					}
				}
			}
			else if (this.currentCreatorCode.Length < 10)
			{
				string str = this.currentCreatorCode;
				string str2;
				if (buttonPressed >= GorillaATMKeyBindings.delete)
				{
					str2 = buttonPressed.ToString();
				}
				else
				{
					int i = (int)buttonPressed;
					str2 = i.ToString();
				}
				this.currentCreatorCode = str + str2;
				this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Unchecked;
			}
			array = this.atmUIs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].creatorCodeField.text = this.currentCreatorCode;
			}
		}
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000D2844 File Offset: 0x000D0A44
	public void ProcessATMState(string currencyButton)
	{
		switch (this.currentATMStage)
		{
		case ATM_Manager.ATMStages.Unavailable:
		case ATM_Manager.ATMStages.Purchasing:
			break;
		case ATM_Manager.ATMStages.Begin:
			this.SwitchToStage(ATM_Manager.ATMStages.Menu);
			return;
		case ATM_Manager.ATMStages.Menu:
			if (PlayFabAuthenticator.instance.GetSafety())
			{
				if (currencyButton == "one")
				{
					this.SwitchToStage(ATM_Manager.ATMStages.Balance);
					return;
				}
				if (!(currencyButton == "four"))
				{
					return;
				}
				this.SwitchToStage(ATM_Manager.ATMStages.Begin);
				return;
			}
			else
			{
				if (currencyButton == "one")
				{
					this.SwitchToStage(ATM_Manager.ATMStages.Balance);
					return;
				}
				if (currencyButton == "two")
				{
					this.SwitchToStage(ATM_Manager.ATMStages.Choose);
					return;
				}
				if (!(currencyButton == "back"))
				{
					return;
				}
				this.SwitchToStage(ATM_Manager.ATMStages.Begin);
				return;
			}
			break;
		case ATM_Manager.ATMStages.Balance:
			if (currencyButton == "back")
			{
				this.SwitchToStage(ATM_Manager.ATMStages.Menu);
				return;
			}
			break;
		case ATM_Manager.ATMStages.Choose:
			if (currencyButton == "one")
			{
				this.numShinyRocksToBuy = 1000;
				this.shinyRocksCost = 4.99f;
				CosmeticsController.instance.itemToPurchase = "1000SHINYROCKS";
				CosmeticsController.instance.buyingBundle = false;
				this.SwitchToStage(ATM_Manager.ATMStages.Confirm);
				return;
			}
			if (currencyButton == "two")
			{
				this.numShinyRocksToBuy = 2200;
				this.shinyRocksCost = 9.99f;
				CosmeticsController.instance.itemToPurchase = "2200SHINYROCKS";
				CosmeticsController.instance.buyingBundle = false;
				this.SwitchToStage(ATM_Manager.ATMStages.Confirm);
				return;
			}
			if (currencyButton == "three")
			{
				this.numShinyRocksToBuy = 5000;
				this.shinyRocksCost = 19.99f;
				CosmeticsController.instance.itemToPurchase = "5000SHINYROCKS";
				CosmeticsController.instance.buyingBundle = false;
				this.SwitchToStage(ATM_Manager.ATMStages.Confirm);
				return;
			}
			if (currencyButton == "four")
			{
				this.numShinyRocksToBuy = 11000;
				this.shinyRocksCost = 39.99f;
				CosmeticsController.instance.itemToPurchase = "11000SHINYROCKS";
				CosmeticsController.instance.buyingBundle = false;
				this.SwitchToStage(ATM_Manager.ATMStages.Confirm);
				return;
			}
			if (!(currencyButton == "back"))
			{
				return;
			}
			this.SwitchToStage(ATM_Manager.ATMStages.Menu);
			return;
		case ATM_Manager.ATMStages.Confirm:
			if (!(currencyButton == "one"))
			{
				if (!(currencyButton == "back"))
				{
					return;
				}
				this.SwitchToStage(ATM_Manager.ATMStages.Choose);
				return;
			}
			else
			{
				if (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Empty)
				{
					CosmeticsController.instance.SteamPurchase();
					this.SwitchToStage(ATM_Manager.ATMStages.Purchasing);
					return;
				}
				base.StartCoroutine(this.CheckValidationCoroutine());
				return;
			}
			break;
		default:
			this.SwitchToStage(ATM_Manager.ATMStages.Menu);
			break;
		}
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x000D2AB0 File Offset: 0x000D0CB0
	private void ResetCreatorCode()
	{
		Debug.Log("Resetting creator code");
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeTitle.text = "CREATOR CODE:";
		}
		foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
		{
			creatorCodeSmallDisplay.SetCode("");
		}
		this.currentCreatorCode = "";
		this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Empty;
		this.supportedMember = default(Member);
		this.ValidatedCreatorCode = "";
		PlayerPrefs.SetString("CreatorCode", "");
		PlayerPrefs.Save();
		array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeField.text = this.currentCreatorCode;
		}
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x00040993 File Offset: 0x0003EB93
	private IEnumerator CheckValidationCoroutine()
	{
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeTitle.text = "CREATOR CODE: VALIDATING";
		}
		this.VerifyCreatorCode();
		while (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Validating)
		{
			yield return new WaitForSeconds(0.5f);
		}
		if (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Valid)
		{
			array = this.atmUIs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].creatorCodeTitle.text = "CREATOR CODE: VALID";
			}
			this.SwitchToStage(ATM_Manager.ATMStages.Purchasing);
			CosmeticsController.instance.SteamPurchase();
		}
		yield break;
	}

	// Token: 0x06001A17 RID: 6679 RVA: 0x000D2B9C File Offset: 0x000D0D9C
	public void SwitchToStage(ATM_Manager.ATMStages newStage)
	{
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			if (!atm_UI.atmText)
			{
				return;
			}
			this.currentATMStage = newStage;
			switch (newStage)
			{
			case ATM_Manager.ATMStages.Unavailable:
				atm_UI.atmText.text = "ATM NOT AVAILABLE! PLEASE TRY AGAIN LATER!";
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Begin:
				atm_UI.atmText.text = "WELCOME! PRESS ANY BUTTON TO BEGIN.";
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "BEGIN";
				atm_UI.ATM_RightColumnArrowText[3].enabled = true;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Menu:
				if (PlayFabAuthenticator.instance.GetSafety())
				{
					atm_UI.atmText.text = "CHECK YOUR BALANCE.";
					atm_UI.ATM_RightColumnButtonText[0].text = "BALANCE";
					atm_UI.ATM_RightColumnArrowText[0].enabled = true;
					atm_UI.ATM_RightColumnButtonText[1].text = "";
					atm_UI.ATM_RightColumnArrowText[1].enabled = false;
					atm_UI.ATM_RightColumnButtonText[2].text = "";
					atm_UI.ATM_RightColumnArrowText[2].enabled = false;
					atm_UI.ATM_RightColumnButtonText[3].text = "";
					atm_UI.ATM_RightColumnArrowText[3].enabled = false;
					atm_UI.creatorCodeObject.SetActive(false);
				}
				else
				{
					atm_UI.atmText.text = "CHECK YOUR BALANCE OR PURCHASE MORE SHINY ROCKS.";
					atm_UI.ATM_RightColumnButtonText[0].text = "BALANCE";
					atm_UI.ATM_RightColumnArrowText[0].enabled = true;
					atm_UI.ATM_RightColumnButtonText[1].text = "PURCHASE";
					atm_UI.ATM_RightColumnArrowText[1].enabled = true;
					atm_UI.ATM_RightColumnButtonText[2].text = "";
					atm_UI.ATM_RightColumnArrowText[2].enabled = false;
					atm_UI.ATM_RightColumnButtonText[3].text = "";
					atm_UI.ATM_RightColumnArrowText[3].enabled = false;
					atm_UI.creatorCodeObject.SetActive(false);
				}
				break;
			case ATM_Manager.ATMStages.Balance:
				atm_UI.atmText.text = "CURRENT BALANCE:\n\n" + CosmeticsController.instance.CurrencyBalance.ToString();
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Choose:
				atm_UI.atmText.text = "CHOOSE AN AMOUNT OF SHINY ROCKS TO PURCHASE.";
				atm_UI.ATM_RightColumnButtonText[0].text = "1000 for $4.99";
				atm_UI.ATM_RightColumnArrowText[0].enabled = true;
				atm_UI.ATM_RightColumnButtonText[1].text = "2200 for $9.99\n(10% BONUS!)";
				atm_UI.ATM_RightColumnArrowText[1].enabled = true;
				atm_UI.ATM_RightColumnButtonText[2].text = "5000 for $19.99\n(25% BONUS!)";
				atm_UI.ATM_RightColumnArrowText[2].enabled = true;
				atm_UI.ATM_RightColumnButtonText[3].text = "11000 for $39.99\n(37% BONUS!)";
				atm_UI.ATM_RightColumnArrowText[3].enabled = true;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Confirm:
				atm_UI.atmText.text = string.Concat(new string[]
				{
					"YOU HAVE CHOSEN TO PURCHASE ",
					this.numShinyRocksToBuy.ToString(),
					" SHINY ROCKS FOR $",
					this.shinyRocksCost.ToString(),
					". CONFIRM TO LAUNCH A STEAM WINDOW TO COMPLETE YOUR PURCHASE."
				});
				atm_UI.ATM_RightColumnButtonText[0].text = "CONFIRM";
				atm_UI.ATM_RightColumnArrowText[0].enabled = true;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(true);
				break;
			case ATM_Manager.ATMStages.Purchasing:
				atm_UI.atmText.text = "PURCHASING IN STEAM...";
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Success:
				atm_UI.atmText.text = "SUCCESS! NEW SHINY ROCKS BALANCE: " + (CosmeticsController.instance.CurrencyBalance + this.numShinyRocksToBuy).ToString();
				if (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Valid)
				{
					string name = this.supportedMember.name;
					if (!string.IsNullOrEmpty(name))
					{
						Text atmText = atm_UI.atmText;
						atmText.text = atmText.text + "\n\nTHIS PURCHASE SUPPORTED\n" + name + "!";
						foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
						{
							creatorCodeSmallDisplay.SuccessfulPurchase(name);
						}
					}
				}
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.Failure:
				atm_UI.atmText.text = "PURCHASE CANCELLED. NO FUNDS WERE SPENT.";
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			case ATM_Manager.ATMStages.SafeAccount:
				atm_UI.atmText.text = "Out Of Order.";
				atm_UI.ATM_RightColumnButtonText[0].text = "";
				atm_UI.ATM_RightColumnArrowText[0].enabled = false;
				atm_UI.ATM_RightColumnButtonText[1].text = "";
				atm_UI.ATM_RightColumnArrowText[1].enabled = false;
				atm_UI.ATM_RightColumnButtonText[2].text = "";
				atm_UI.ATM_RightColumnArrowText[2].enabled = false;
				atm_UI.ATM_RightColumnButtonText[3].text = "";
				atm_UI.ATM_RightColumnArrowText[3].enabled = false;
				atm_UI.creatorCodeObject.SetActive(false);
				break;
			}
		}
	}

	// Token: 0x06001A18 RID: 6680 RVA: 0x000D3384 File Offset: 0x000D1584
	public void SetATMText(string newText)
	{
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].atmText.text = newText;
		}
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x000409A2 File Offset: 0x0003EBA2
	public void PressCurrencyPurchaseButton(string currencyPurchaseSize)
	{
		this.ProcessATMState(currencyPurchaseSize);
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000409AB File Offset: 0x0003EBAB
	public void VerifyCreatorCode()
	{
		this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Validating;
		NexusManager.instance.VerifyCreatorCode(this.currentCreatorCode, new Action<Member>(this.OnCreatorCodeSucess), new Action(this.OnCreatorCodeFailure));
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000D33B4 File Offset: 0x000D15B4
	private void OnCreatorCodeSucess(Member member)
	{
		this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Valid;
		this.supportedMember = member;
		this.ValidatedCreatorCode = this.currentCreatorCode;
		foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
		{
			creatorCodeSmallDisplay.SetCode(this.ValidatedCreatorCode);
		}
		PlayerPrefs.SetString("CreatorCode", this.ValidatedCreatorCode);
		if (this.initialCode != this.ValidatedCreatorCode)
		{
			PlayerPrefs.SetString("CodeUsedTime", DateTime.Now.ToString());
		}
		PlayerPrefs.Save();
		Debug.Log("ATM CODE SUCCESS: " + this.supportedMember.name);
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000D3480 File Offset: 0x000D1680
	private void OnCreatorCodeFailure()
	{
		this.supportedMember = default(Member);
		this.ResetCreatorCode();
		ATM_UI[] array = this.atmUIs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].creatorCodeTitle.text = "CREATOR CODE: INVALID";
		}
		Debug.Log("ATM CODE FAILURE");
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x0002F75F File Offset: 0x0002D95F
	public void LeaveSystemMenu()
	{
	}

	// Token: 0x04001CDA RID: 7386
	[OnEnterPlay_SetNull]
	public static volatile ATM_Manager instance;

	// Token: 0x04001CDB RID: 7387
	public ATM_UI[] atmUIs;

	// Token: 0x04001CDC RID: 7388
	[HideInInspector]
	public List<CreatorCodeSmallDisplay> smallDisplays;

	// Token: 0x04001CDD RID: 7389
	private string currentCreatorCode;

	// Token: 0x04001CDE RID: 7390
	private string codeFirstUsedTime;

	// Token: 0x04001CDF RID: 7391
	private string initialCode;

	// Token: 0x04001CE1 RID: 7393
	private ATM_Manager.CreatorCodeStatus creatorCodeStatus;

	// Token: 0x04001CE2 RID: 7394
	private ATM_Manager.ATMStages currentATMStage;

	// Token: 0x04001CE3 RID: 7395
	public int numShinyRocksToBuy;

	// Token: 0x04001CE4 RID: 7396
	public float shinyRocksCost;

	// Token: 0x04001CE5 RID: 7397
	private Member supportedMember;

	// Token: 0x04001CE6 RID: 7398
	public bool alreadyBegan;

	// Token: 0x0200041F RID: 1055
	public enum CreatorCodeStatus
	{
		// Token: 0x04001CE8 RID: 7400
		Empty,
		// Token: 0x04001CE9 RID: 7401
		Unchecked,
		// Token: 0x04001CEA RID: 7402
		Validating,
		// Token: 0x04001CEB RID: 7403
		Valid
	}

	// Token: 0x02000420 RID: 1056
	public enum ATMStages
	{
		// Token: 0x04001CED RID: 7405
		Unavailable,
		// Token: 0x04001CEE RID: 7406
		Begin,
		// Token: 0x04001CEF RID: 7407
		Menu,
		// Token: 0x04001CF0 RID: 7408
		Balance,
		// Token: 0x04001CF1 RID: 7409
		Choose,
		// Token: 0x04001CF2 RID: 7410
		Confirm,
		// Token: 0x04001CF3 RID: 7411
		Purchasing,
		// Token: 0x04001CF4 RID: 7412
		Success,
		// Token: 0x04001CF5 RID: 7413
		Failure,
		// Token: 0x04001CF6 RID: 7414
		SafeAccount
	}
}
