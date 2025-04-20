using System;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000677 RID: 1655
public class ModIOAccountLinkingTerminal : MonoBehaviour
{
	// Token: 0x0600290F RID: 10511 RVA: 0x00114720 File Offset: 0x00112920
	public void Start()
	{
		this.modioUsernameLabelText.gameObject.SetActive(false);
		this.modioUsernameText.gameObject.SetActive(false);
		this.loggingInText.gameObject.SetActive(false);
		this.linkAccountPromptText.gameObject.SetActive(true);
		this.linkAccountLabelText.gameObject.SetActive(false);
		this.linkAccountURLLabelText.gameObject.SetActive(false);
		this.linkAccountURLText.gameObject.SetActive(false);
		this.linkAccountCodeLabelText.gameObject.SetActive(false);
		this.linkAccountCodeText.gameObject.SetActive(false);
		this.alreadyLinkedAccountText.gameObject.SetActive(false);
		this.errorText.gameObject.SetActive(false);
		GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<CustomMapsTerminalButton.ModIOKeyboardBindings>(ModIOAccountLinkingTerminal.PressButton));
		GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
		ModIOAccountLinkingTerminal.OnButtonPress.AddListener(new UnityAction(this.ResetScreen));
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x0011482C File Offset: 0x00112A2C
	public void OnDestroy()
	{
		GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<CustomMapsTerminalButton.ModIOKeyboardBindings>(ModIOAccountLinkingTerminal.PressButton));
		GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
		ModIOAccountLinkingTerminal.OnButtonPress.RemoveListener(new UnityAction(this.ResetScreen));
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x0004BD39 File Offset: 0x00049F39
	public void OnModIOLoggedIn()
	{
		if (!this.isLoggedIn)
		{
			ModIOUnity.GetCurrentUser(delegate(ResultAnd<UserProfile> result)
			{
				if (!result.result.Succeeded())
				{
					return;
				}
				this.isLoggedIn = true;
				this.errorText.gameObject.SetActive(false);
				this.loggingInText.gameObject.SetActive(false);
				this.linkAccountLabelText.gameObject.SetActive(false);
				this.linkAccountURLLabelText.gameObject.SetActive(false);
				this.linkAccountURLText.gameObject.SetActive(false);
				this.linkAccountCodeLabelText.gameObject.SetActive(false);
				this.linkAccountCodeText.gameObject.SetActive(false);
				this.linkAccountPromptText.gameObject.SetActive(false);
				this.alreadyLinkedAccountText.gameObject.SetActive(false);
				this.linkAccountCodeText.text = "";
				this.linkAccountURLText.text = "";
				this.errorText.text = "";
				this.modioUsernameText.text = result.value.username;
				this.modioUsernameLabelText.gameObject.SetActive(true);
				this.modioUsernameText.gameObject.SetActive(true);
				if (ModIOManager.GetLastAuthMethod() != ModIOManager.ModIOAuthMethod.LinkedAccount)
				{
					this.linkAccountPromptText.gameObject.SetActive(true);
				}
				else
				{
					this.alreadyLinkedAccountText.gameObject.SetActive(true);
				}
				GameEvents.OnModIOLoggedOut.AddListener(new UnityAction(this.OnModIOLoggedOut));
				this.processingAccountLink = false;
			}, false);
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x0011487C File Offset: 0x00112A7C
	private void OnModIOLoggedOut()
	{
		if (this.isLoggedIn)
		{
			this.isLoggedIn = false;
			this.processingAccountLink = false;
			ModIOManager.CancelExternalAuthentication();
			this.modioUsernameLabelText.gameObject.SetActive(false);
			this.modioUsernameText.gameObject.SetActive(false);
			this.modioUsernameText.text = "";
			this.loggingInText.gameObject.SetActive(false);
			this.errorText.gameObject.SetActive(false);
			this.linkAccountLabelText.gameObject.SetActive(false);
			this.linkAccountURLLabelText.gameObject.SetActive(false);
			this.linkAccountURLText.gameObject.SetActive(false);
			this.linkAccountCodeLabelText.gameObject.SetActive(false);
			this.linkAccountCodeText.gameObject.SetActive(false);
			this.linkAccountPromptText.gameObject.SetActive(false);
			this.alreadyLinkedAccountText.gameObject.SetActive(false);
			this.linkAccountPromptText.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x00114984 File Offset: 0x00112B84
	private static void PressButton(CustomMapsTerminalButton.ModIOKeyboardBindings pressedButton)
	{
		if (pressedButton == CustomMapsTerminalButton.ModIOKeyboardBindings.option2)
		{
			if (ModIOManager.IsLoggedIn())
			{
				ModIOManager.LogoutFromModIO();
			}
			else
			{
				UnityEvent onButtonPress = ModIOAccountLinkingTerminal.OnButtonPress;
				if (onButtonPress != null)
				{
					onButtonPress.Invoke();
				}
			}
		}
		if (pressedButton == CustomMapsTerminalButton.ModIOKeyboardBindings.option4)
		{
			if (!ModIOManager.IsLoggedIn())
			{
				ModIOManager.CancelExternalAuthentication();
				ModIOManager.RequestPlatformLogin(null);
				return;
			}
			UnityEvent onButtonPress2 = ModIOAccountLinkingTerminal.OnButtonPress;
			if (onButtonPress2 == null)
			{
				return;
			}
			onButtonPress2.Invoke();
		}
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x0004BD55 File Offset: 0x00049F55
	public void LinkButtonPressed()
	{
		if (!this.processingAccountLink)
		{
			this.processingAccountLink = true;
			ModIOManager.IsAuthenticated(delegate(Result result)
			{
				if (result.Succeeded())
				{
					if (ModIOManager.GetLastAuthMethod() == ModIOManager.ModIOAuthMethod.LinkedAccount)
					{
						this.processingAccountLink = false;
						this.alreadyLinkedAccountText.gameObject.SetActive(true);
						this.linkAccountPromptText.gameObject.SetActive(false);
						return;
					}
					GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
					ModIOManager.LogoutFromModIO();
					this.isLoggedIn = false;
				}
				this.errorText.gameObject.SetActive(false);
				this.errorText.text = "";
				this.modioUsernameLabelText.gameObject.SetActive(false);
				this.modioUsernameText.gameObject.SetActive(false);
				this.modioUsernameText.text = "";
				ModIOManager.RequestAccountLinkCode(delegate(ModIORequestResult result, string linkURL, string linkCode)
				{
					this.linkAccountPromptText.gameObject.SetActive(false);
					this.linkAccountLabelText.gameObject.SetActive(true);
					this.linkAccountURLLabelText.gameObject.SetActive(true);
					this.linkAccountURLText.text = linkURL;
					this.linkAccountURLText.gameObject.SetActive(true);
					this.linkAccountCodeLabelText.gameObject.SetActive(true);
					this.linkAccountCodeText.text = linkCode;
					this.linkAccountCodeText.gameObject.SetActive(true);
				}, delegate(ModIORequestResult result)
				{
					if (!result.success)
					{
						this.linkAccountLabelText.gameObject.SetActive(false);
						this.linkAccountURLLabelText.gameObject.SetActive(false);
						this.linkAccountURLText.gameObject.SetActive(false);
						this.linkAccountCodeLabelText.gameObject.SetActive(false);
						this.linkAccountCodeText.gameObject.SetActive(false);
						this.linkAccountCodeText.text = "";
						this.linkAccountURLText.text = "";
						this.errorText.text = "Failed to authorize with Mod.io. Error:\n " + result.message + "\n\n Press the LINK button to try again.";
						this.errorText.gameObject.SetActive(true);
						this.processingAccountLink = false;
					}
				});
			});
			return;
		}
		this.ResetScreen();
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x001149DC File Offset: 0x00112BDC
	public void NotifyLoggingIn()
	{
		ModIOManager.CancelExternalAuthentication();
		this.modioUsernameLabelText.gameObject.SetActive(false);
		this.modioUsernameText.gameObject.SetActive(false);
		this.linkAccountPromptText.gameObject.SetActive(false);
		this.linkAccountLabelText.gameObject.SetActive(false);
		this.linkAccountURLLabelText.gameObject.SetActive(false);
		this.linkAccountURLText.gameObject.SetActive(false);
		this.linkAccountCodeLabelText.gameObject.SetActive(false);
		this.linkAccountCodeText.gameObject.SetActive(false);
		this.alreadyLinkedAccountText.gameObject.SetActive(false);
		this.errorText.text = "";
		this.errorText.gameObject.SetActive(false);
		this.loggingInText.gameObject.SetActive(true);
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x00114ABC File Offset: 0x00112CBC
	public void DisplayLoginError(string errorMessage)
	{
		ModIOManager.CancelExternalAuthentication();
		this.modioUsernameLabelText.gameObject.SetActive(false);
		this.modioUsernameText.gameObject.SetActive(false);
		this.loggingInText.gameObject.SetActive(false);
		this.linkAccountPromptText.gameObject.SetActive(false);
		this.linkAccountLabelText.gameObject.SetActive(false);
		this.linkAccountURLLabelText.gameObject.SetActive(false);
		this.linkAccountURLText.gameObject.SetActive(false);
		this.linkAccountCodeLabelText.gameObject.SetActive(false);
		this.linkAccountCodeText.gameObject.SetActive(false);
		this.alreadyLinkedAccountText.gameObject.SetActive(false);
		this.errorText.text = errorMessage;
		this.errorText.gameObject.SetActive(true);
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x00114B98 File Offset: 0x00112D98
	private void ResetScreen()
	{
		this.processingAccountLink = false;
		ModIOManager.CancelExternalAuthentication();
		if (this.isLoggedIn)
		{
			this.modioUsernameLabelText.gameObject.SetActive(true);
			this.modioUsernameText.gameObject.SetActive(true);
		}
		else
		{
			this.modioUsernameLabelText.gameObject.SetActive(false);
			this.modioUsernameText.gameObject.SetActive(false);
			this.modioUsernameText.text = "";
		}
		this.loggingInText.gameObject.SetActive(false);
		this.errorText.gameObject.SetActive(false);
		this.linkAccountLabelText.gameObject.SetActive(false);
		this.linkAccountURLLabelText.gameObject.SetActive(false);
		this.linkAccountURLText.gameObject.SetActive(false);
		this.linkAccountCodeLabelText.gameObject.SetActive(false);
		this.linkAccountCodeText.gameObject.SetActive(false);
		this.linkAccountPromptText.gameObject.SetActive(false);
		this.alreadyLinkedAccountText.gameObject.SetActive(false);
		if (ModIOManager.GetLastAuthMethod() != ModIOManager.ModIOAuthMethod.LinkedAccount)
		{
			this.linkAccountPromptText.gameObject.SetActive(true);
			return;
		}
		this.alreadyLinkedAccountText.gameObject.SetActive(true);
	}

	// Token: 0x04002E6B RID: 11883
	[SerializeField]
	private TMP_Text modioUsernameLabelText;

	// Token: 0x04002E6C RID: 11884
	[SerializeField]
	private TMP_Text modioUsernameText;

	// Token: 0x04002E6D RID: 11885
	[SerializeField]
	private TMP_Text linkAccountPromptText;

	// Token: 0x04002E6E RID: 11886
	[SerializeField]
	private TMP_Text alreadyLinkedAccountText;

	// Token: 0x04002E6F RID: 11887
	[SerializeField]
	private TMP_Text linkAccountLabelText;

	// Token: 0x04002E70 RID: 11888
	[SerializeField]
	private TMP_Text linkAccountURLLabelText;

	// Token: 0x04002E71 RID: 11889
	[SerializeField]
	private TMP_Text linkAccountURLText;

	// Token: 0x04002E72 RID: 11890
	[SerializeField]
	private TMP_Text linkAccountCodeLabelText;

	// Token: 0x04002E73 RID: 11891
	[SerializeField]
	private TMP_Text linkAccountCodeText;

	// Token: 0x04002E74 RID: 11892
	[SerializeField]
	private TMP_Text loggingInText;

	// Token: 0x04002E75 RID: 11893
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002E76 RID: 11894
	private bool processingAccountLink;

	// Token: 0x04002E77 RID: 11895
	private bool isLoggedIn;

	// Token: 0x04002E78 RID: 11896
	private static UnityEvent OnButtonPress = new UnityEvent();
}
