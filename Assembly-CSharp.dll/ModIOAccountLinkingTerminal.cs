using System;
using ModIO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000676 RID: 1654
public class ModIOAccountLinkingTerminal : MonoBehaviour
{
	// Token: 0x060028FE RID: 10494 RVA: 0x00111818 File Offset: 0x0010FA18
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
		GameEvents.OnModIOKeyboardButtonPressedEvent.AddListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(ModIOAccountLinkingTerminal.PressButton));
		GameEvents.OnModIOLoggedIn.AddListener(new UnityAction(this.OnModIOLoggedIn));
		ModIOAccountLinkingTerminal.OnButtonPress.AddListener(new UnityAction(this.ResetScreen));
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x00111924 File Offset: 0x0010FB24
	public void OnDestroy()
	{
		GameEvents.OnModIOKeyboardButtonPressedEvent.RemoveListener(new UnityAction<ModIOKeyboardButton.ModIOKeyboardBindings>(ModIOAccountLinkingTerminal.PressButton));
		GameEvents.OnModIOLoggedIn.RemoveListener(new UnityAction(this.OnModIOLoggedIn));
		ModIOAccountLinkingTerminal.OnButtonPress.RemoveListener(new UnityAction(this.ResetScreen));
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x0004AFFB File Offset: 0x000491FB
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
				if (ModIODataStore.GetLastAuthMethod() != ModIODataStore.ModIOAuthMethod.LinkedAccount)
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

	// Token: 0x06002901 RID: 10497 RVA: 0x00111974 File Offset: 0x0010FB74
	private void OnModIOLoggedOut()
	{
		if (this.isLoggedIn)
		{
			this.isLoggedIn = false;
			this.processingAccountLink = false;
			ModIODataStore.CancelExternalAuthentication();
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

	// Token: 0x06002902 RID: 10498 RVA: 0x00111A7C File Offset: 0x0010FC7C
	private static void PressButton(ModIOKeyboardButton.ModIOKeyboardBindings pressedButton)
	{
		if (pressedButton == ModIOKeyboardButton.ModIOKeyboardBindings.option2)
		{
			if (ModIODataStore.IsLoggedIn())
			{
				ModIODataStore.LogoutFromModIO();
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
		if (pressedButton == ModIOKeyboardButton.ModIOKeyboardBindings.option4)
		{
			if (!ModIODataStore.IsLoggedIn())
			{
				ModIODataStore.CancelExternalAuthentication();
				ModIODataStore.RequestPlatformLogin(null);
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

	// Token: 0x06002903 RID: 10499 RVA: 0x0004B017 File Offset: 0x00049217
	public void LinkButtonPressed()
	{
		if (!this.processingAccountLink)
		{
			this.processingAccountLink = true;
			ModIODataStore.IsAuthenticated(delegate(Result result)
			{
				if (result.Succeeded())
				{
					if (ModIODataStore.GetLastAuthMethod() == ModIODataStore.ModIOAuthMethod.LinkedAccount)
					{
						this.processingAccountLink = false;
						this.alreadyLinkedAccountText.gameObject.SetActive(true);
						this.linkAccountPromptText.gameObject.SetActive(false);
						return;
					}
					GameEvents.OnModIOLoggedOut.RemoveListener(new UnityAction(this.OnModIOLoggedOut));
					ModIODataStore.LogoutFromModIO();
					this.isLoggedIn = false;
				}
				this.errorText.gameObject.SetActive(false);
				this.errorText.text = "";
				this.modioUsernameLabelText.gameObject.SetActive(false);
				this.modioUsernameText.gameObject.SetActive(false);
				this.modioUsernameText.text = "";
				ModIODataStore.RequestAccountLinkCode(delegate(ModIORequestResult result, string linkURL, string linkCode)
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

	// Token: 0x06002904 RID: 10500 RVA: 0x00111AD4 File Offset: 0x0010FCD4
	public void NotifyLoggingIn()
	{
		ModIODataStore.CancelExternalAuthentication();
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

	// Token: 0x06002905 RID: 10501 RVA: 0x00111BB4 File Offset: 0x0010FDB4
	public void DisplayLoginError(string errorMessage)
	{
		ModIODataStore.CancelExternalAuthentication();
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

	// Token: 0x06002906 RID: 10502 RVA: 0x00111C90 File Offset: 0x0010FE90
	private void ResetScreen()
	{
		this.processingAccountLink = false;
		ModIODataStore.CancelExternalAuthentication();
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
		if (ModIODataStore.GetLastAuthMethod() != ModIODataStore.ModIOAuthMethod.LinkedAccount)
		{
			this.linkAccountPromptText.gameObject.SetActive(true);
			return;
		}
		this.alreadyLinkedAccountText.gameObject.SetActive(true);
	}

	// Token: 0x04002E07 RID: 11783
	[SerializeField]
	private TMP_Text modioUsernameLabelText;

	// Token: 0x04002E08 RID: 11784
	[SerializeField]
	private TMP_Text modioUsernameText;

	// Token: 0x04002E09 RID: 11785
	[SerializeField]
	private TMP_Text linkAccountPromptText;

	// Token: 0x04002E0A RID: 11786
	[SerializeField]
	private TMP_Text alreadyLinkedAccountText;

	// Token: 0x04002E0B RID: 11787
	[SerializeField]
	private TMP_Text linkAccountLabelText;

	// Token: 0x04002E0C RID: 11788
	[SerializeField]
	private TMP_Text linkAccountURLLabelText;

	// Token: 0x04002E0D RID: 11789
	[SerializeField]
	private TMP_Text linkAccountURLText;

	// Token: 0x04002E0E RID: 11790
	[SerializeField]
	private TMP_Text linkAccountCodeLabelText;

	// Token: 0x04002E0F RID: 11791
	[SerializeField]
	private TMP_Text linkAccountCodeText;

	// Token: 0x04002E10 RID: 11792
	[SerializeField]
	private TMP_Text loggingInText;

	// Token: 0x04002E11 RID: 11793
	[SerializeField]
	private TMP_Text errorText;

	// Token: 0x04002E12 RID: 11794
	private bool processingAccountLink;

	// Token: 0x04002E13 RID: 11795
	private bool isLoggedIn;

	// Token: 0x04002E14 RID: 11796
	private static UnityEvent OnButtonPress = new UnityEvent();
}
