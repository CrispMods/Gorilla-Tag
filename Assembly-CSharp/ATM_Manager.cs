using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GorillaNetworking;
using GorillaNetworking.Store;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000429 RID: 1065
public class ATM_Manager : MonoBehaviour
{
	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001A58 RID: 6744 RVA: 0x00041C64 File Offset: 0x0003FE64
	// (set) Token: 0x06001A59 RID: 6745 RVA: 0x00041C6C File Offset: 0x0003FE6C
	public string ValidatedCreatorCode { get; set; }

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001A5A RID: 6746 RVA: 0x00041C75 File Offset: 0x0003FE75
	public ATM_Manager.ATMStages CurrentATMStage
	{
		get
		{
			return this.currentATMStage;
		}
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000D4CFC File Offset: 0x000D2EFC
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
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.creatorCodeTitle.text = "CREATOR CODE: ";
		}
		this.SwitchToStage(ATM_Manager.ATMStages.Unavailable);
		this.smallDisplays = new List<CreatorCodeSmallDisplay>();
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000D4D88 File Offset: 0x000D2F88
	public void Start()
	{
		Debug.Log("ATM COUNT: " + this.atmUIs.Count.ToString());
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
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.creatorCodeField.text = this.currentCreatorCode;
		}
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000D4F40 File Offset: 0x000D3140
	public void PressButton(GorillaATMKeyBindings buttonPressed)
	{
		if (this.currentATMStage == ATM_Manager.ATMStages.Confirm && this.creatorCodeStatus != ATM_Manager.CreatorCodeStatus.Validating)
		{
			foreach (ATM_UI atm_UI in this.atmUIs)
			{
				atm_UI.creatorCodeTitle.text = "CREATOR CODE:";
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
					int num = (int)buttonPressed;
					str2 = num.ToString();
				}
				this.currentCreatorCode = str + str2;
				this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Unchecked;
			}
			foreach (ATM_UI atm_UI2 in this.atmUIs)
			{
				atm_UI2.creatorCodeField.text = this.currentCreatorCode;
			}
		}
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000D50FC File Offset: 0x000D32FC
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

	// Token: 0x06001A5F RID: 6751 RVA: 0x00041C7D File Offset: 0x0003FE7D
	public void AddATM(ATM_UI newATM)
	{
		this.atmUIs.Add(newATM);
		this.SwitchToStage(this.currentATMStage);
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x00041C97 File Offset: 0x0003FE97
	public void RemoveATM(ATM_UI atmToRemove)
	{
		this.atmUIs.Remove(atmToRemove);
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x000D5368 File Offset: 0x000D3568
	public void SetTemporaryCreatorCode(string creatorCode, bool onlyIfEmpty = true, Action<bool> OnComplete = null)
	{
		if (onlyIfEmpty && (this.creatorCodeStatus != ATM_Manager.CreatorCodeStatus.Empty || !this.currentCreatorCode.IsNullOrEmpty()))
		{
			Action<bool> onComplete = OnComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete(false);
			return;
		}
		else
		{
			string pattern = "^[a-zA-Z0-9]+$";
			if (creatorCode.Length <= 10 && Regex.IsMatch(creatorCode, pattern))
			{
				NexusManager.instance.VerifyCreatorCode(creatorCode, delegate(Member member)
				{
					if (this.currentATMStage > ATM_Manager.ATMStages.Confirm)
					{
						Action<bool> onComplete3 = OnComplete;
						if (onComplete3 == null)
						{
							return;
						}
						onComplete3(false);
						return;
					}
					else if (onlyIfEmpty && (this.creatorCodeStatus != ATM_Manager.CreatorCodeStatus.Empty || !this.currentCreatorCode.IsNullOrEmpty()))
					{
						Action<bool> onComplete4 = OnComplete;
						if (onComplete4 == null)
						{
							return;
						}
						onComplete4(false);
						return;
					}
					else
					{
						this.temporaryOverrideCode = creatorCode;
						this.currentCreatorCode = creatorCode;
						this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Unchecked;
						foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
						{
							creatorCodeSmallDisplay.SetCode(this.currentCreatorCode);
						}
						foreach (ATM_UI atm_UI in this.atmUIs)
						{
							atm_UI.creatorCodeField.text = this.currentCreatorCode;
						}
						Action<bool> onComplete5 = OnComplete;
						if (onComplete5 == null)
						{
							return;
						}
						onComplete5(true);
						return;
					}
				}, delegate
				{
					Action<bool> onComplete3 = OnComplete;
					if (onComplete3 == null)
					{
						return;
					}
					onComplete3(false);
				});
				return;
			}
			Action<bool> onComplete2 = OnComplete;
			if (onComplete2 == null)
			{
				return;
			}
			onComplete2(false);
			return;
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x000D5428 File Offset: 0x000D3628
	public void ResetTemporaryCreatorCode()
	{
		if (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Unchecked && this.currentCreatorCode.Equals(this.temporaryOverrideCode))
		{
			this.currentCreatorCode = "";
			this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Empty;
			foreach (CreatorCodeSmallDisplay creatorCodeSmallDisplay in this.smallDisplays)
			{
				creatorCodeSmallDisplay.SetCode("");
			}
			foreach (ATM_UI atm_UI in this.atmUIs)
			{
				atm_UI.creatorCodeField.text = this.currentCreatorCode;
			}
		}
		this.temporaryOverrideCode = "";
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x000D5508 File Offset: 0x000D3708
	private void ResetCreatorCode()
	{
		Debug.Log("Resetting creator code");
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.creatorCodeTitle.text = "CREATOR CODE:";
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
		foreach (ATM_UI atm_UI2 in this.atmUIs)
		{
			atm_UI2.creatorCodeField.text = this.currentCreatorCode;
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x00041CA6 File Offset: 0x0003FEA6
	private IEnumerator CheckValidationCoroutine()
	{
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.creatorCodeTitle.text = "CREATOR CODE: VALIDATING";
		}
		this.VerifyCreatorCode();
		while (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Validating)
		{
			yield return new WaitForSeconds(0.5f);
		}
		if (this.creatorCodeStatus == ATM_Manager.CreatorCodeStatus.Valid)
		{
			foreach (ATM_UI atm_UI2 in this.atmUIs)
			{
				atm_UI2.creatorCodeTitle.text = "CREATOR CODE: VALID";
			}
			this.SwitchToStage(ATM_Manager.ATMStages.Purchasing);
			CosmeticsController.instance.SteamPurchase();
		}
		yield break;
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000D5638 File Offset: 0x000D3838
	public void SwitchToStage(ATM_Manager.ATMStages newStage)
	{
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			if (!atm_UI.atmText)
			{
				break;
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

	// Token: 0x06001A66 RID: 6758 RVA: 0x000D5E58 File Offset: 0x000D4058
	public void SetATMText(string newText)
	{
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.atmText.text = newText;
		}
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x00041CB5 File Offset: 0x0003FEB5
	public void PressCurrencyPurchaseButton(string currencyPurchaseSize)
	{
		this.ProcessATMState(currencyPurchaseSize);
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x00041CBE File Offset: 0x0003FEBE
	public void VerifyCreatorCode()
	{
		this.creatorCodeStatus = ATM_Manager.CreatorCodeStatus.Validating;
		NexusManager.instance.VerifyCreatorCode(this.currentCreatorCode, new Action<Member>(this.OnCreatorCodeSucess), new Action(this.OnCreatorCodeFailure));
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000D5EB0 File Offset: 0x000D40B0
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

	// Token: 0x06001A6A RID: 6762 RVA: 0x000D5F7C File Offset: 0x000D417C
	private void OnCreatorCodeFailure()
	{
		this.supportedMember = default(Member);
		this.ResetCreatorCode();
		foreach (ATM_UI atm_UI in this.atmUIs)
		{
			atm_UI.creatorCodeTitle.text = "CREATOR CODE: INVALID";
		}
		Debug.Log("ATM CODE FAILURE");
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x00030607 File Offset: 0x0002E807
	public void LeaveSystemMenu()
	{
	}

	// Token: 0x04001D22 RID: 7458
	[OnEnterPlay_SetNull]
	public static volatile ATM_Manager instance;

	// Token: 0x04001D23 RID: 7459
	private const int MAX_CODE_LENGTH = 10;

	// Token: 0x04001D24 RID: 7460
	public List<ATM_UI> atmUIs = new List<ATM_UI>();

	// Token: 0x04001D25 RID: 7461
	[HideInInspector]
	public List<CreatorCodeSmallDisplay> smallDisplays;

	// Token: 0x04001D26 RID: 7462
	private string currentCreatorCode;

	// Token: 0x04001D27 RID: 7463
	private string codeFirstUsedTime;

	// Token: 0x04001D28 RID: 7464
	private string initialCode;

	// Token: 0x04001D29 RID: 7465
	private string temporaryOverrideCode;

	// Token: 0x04001D2B RID: 7467
	private ATM_Manager.CreatorCodeStatus creatorCodeStatus;

	// Token: 0x04001D2C RID: 7468
	private ATM_Manager.ATMStages currentATMStage;

	// Token: 0x04001D2D RID: 7469
	public int numShinyRocksToBuy;

	// Token: 0x04001D2E RID: 7470
	public float shinyRocksCost;

	// Token: 0x04001D2F RID: 7471
	private Member supportedMember;

	// Token: 0x04001D30 RID: 7472
	public bool alreadyBegan;

	// Token: 0x0200042A RID: 1066
	public enum CreatorCodeStatus
	{
		// Token: 0x04001D32 RID: 7474
		Empty,
		// Token: 0x04001D33 RID: 7475
		Unchecked,
		// Token: 0x04001D34 RID: 7476
		Validating,
		// Token: 0x04001D35 RID: 7477
		Valid
	}

	// Token: 0x0200042B RID: 1067
	public enum ATMStages
	{
		// Token: 0x04001D37 RID: 7479
		Unavailable,
		// Token: 0x04001D38 RID: 7480
		Begin,
		// Token: 0x04001D39 RID: 7481
		Menu,
		// Token: 0x04001D3A RID: 7482
		Balance,
		// Token: 0x04001D3B RID: 7483
		Choose,
		// Token: 0x04001D3C RID: 7484
		Confirm,
		// Token: 0x04001D3D RID: 7485
		Purchasing,
		// Token: 0x04001D3E RID: 7486
		Success,
		// Token: 0x04001D3F RID: 7487
		Failure,
		// Token: 0x04001D40 RID: 7488
		SafeAccount
	}
}
