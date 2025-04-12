using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

// Token: 0x02000703 RID: 1795
public class PreGameMessage : MonoBehaviour
{
	// Token: 0x06002C91 RID: 11409 RVA: 0x0004D796 File Offset: 0x0004B996
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x0004D7A5 File Offset: 0x0004B9A5
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002C93 RID: 11411 RVA: 0x0004D7BE File Offset: 0x0004B9BE
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002C94 RID: 11412 RVA: 0x00121A10 File Offset: 0x0011FC10
	public void ShowMessage(string messageTitle, string messageBody, string messageConfirmation, Action onConfirmationAction, float bodyFontSize = 0.5f, float buttonHideTimer = 0f)
	{
		this._alternativeAction = null;
		this._multiButtonRoot.SetActive(false);
		this._messageTitleTxt.text = messageTitle;
		this._messageBodyTxt.text = messageBody;
		this._messageConfirmationTxt.text = messageConfirmation;
		this._confirmationAction = onConfirmationAction;
		this._messageBodyTxt.fontSize = bodyFontSize;
		this._hasCompleted = false;
		if (this._confirmationAction == null)
		{
			this._confirmButtonRoot.SetActive(false);
		}
		else if (!string.IsNullOrEmpty(this._messageConfirmationTxt.text))
		{
			this._confirmButtonRoot.SetActive(true);
		}
		PrivateUIRoom.AddUI(this._uiParent.transform);
	}

	// Token: 0x06002C95 RID: 11413 RVA: 0x00121AB4 File Offset: 0x0011FCB4
	public void ShowMessage(string messageTitle, string messageBody, string messageConfirmationButton, string messageAlternativeButton, Action onConfirmationAction, Action onAlternativeAction, float bodyFontSize = 0.5f)
	{
		this._confirmButtonRoot.SetActive(false);
		this._messageTitleTxt.text = messageTitle;
		this._messageBodyTxt.text = messageBody;
		this._messageAlternativeConfirmationTxt.text = messageConfirmationButton;
		this._messageAlternativeButtonTxt.text = messageAlternativeButton;
		this._confirmationAction = onConfirmationAction;
		this._alternativeAction = onAlternativeAction;
		this._messageBodyTxt.fontSize = bodyFontSize;
		this._hasCompleted = false;
		if (this._confirmationAction == null || this._alternativeAction == null)
		{
			Debug.LogError("[KID] Trying to show a mesasge with multiple buttons, but one or both callbacks are null");
			this._multiButtonRoot.SetActive(false);
		}
		else if (!string.IsNullOrEmpty(this._messageAlternativeConfirmationTxt.text) && !string.IsNullOrEmpty(this._messageAlternativeButtonTxt.text))
		{
			this._multiButtonRoot.SetActive(true);
		}
		PrivateUIRoom.AddUI(this._uiParent.transform);
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x00121B8C File Offset: 0x0011FD8C
	public Task ShowMessageWithAwait(string messageTitle, string messageBody, string messageConfirmation, Action onConfirmationAction, float bodyFontSize = 0.5f, float buttonHideTimer = 0f)
	{
		PreGameMessage.<ShowMessageWithAwait>d__22 <ShowMessageWithAwait>d__;
		<ShowMessageWithAwait>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ShowMessageWithAwait>d__.<>4__this = this;
		<ShowMessageWithAwait>d__.messageTitle = messageTitle;
		<ShowMessageWithAwait>d__.messageBody = messageBody;
		<ShowMessageWithAwait>d__.messageConfirmation = messageConfirmation;
		<ShowMessageWithAwait>d__.onConfirmationAction = onConfirmationAction;
		<ShowMessageWithAwait>d__.bodyFontSize = bodyFontSize;
		<ShowMessageWithAwait>d__.<>1__state = -1;
		<ShowMessageWithAwait>d__.<>t__builder.Start<PreGameMessage.<ShowMessageWithAwait>d__22>(ref <ShowMessageWithAwait>d__);
		return <ShowMessageWithAwait>d__.<>t__builder.Task;
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x00121BFC File Offset: 0x0011FDFC
	public void UpdateMessage(string newMessageBody, string newConfirmButton)
	{
		this._messageBodyTxt.text = newMessageBody;
		this._messageConfirmationTxt.text = newConfirmButton;
		if (string.IsNullOrEmpty(this._messageConfirmationTxt.text))
		{
			this._confirmButtonRoot.SetActive(false);
			return;
		}
		if (this._confirmationAction != null)
		{
			this._confirmButtonRoot.SetActive(true);
		}
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x0004D7D7 File Offset: 0x0004B9D7
	public void CloseMessage()
	{
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x00121C54 File Offset: 0x0011FE54
	private Task WaitForCompletion()
	{
		PreGameMessage.<WaitForCompletion>d__25 <WaitForCompletion>d__;
		<WaitForCompletion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForCompletion>d__.<>4__this = this;
		<WaitForCompletion>d__.<>1__state = -1;
		<WaitForCompletion>d__.<>t__builder.Start<PreGameMessage.<WaitForCompletion>d__25>(ref <WaitForCompletion>d__);
		return <WaitForCompletion>d__.<>t__builder.Task;
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x00121C98 File Offset: 0x0011FE98
	private void PostUpdate()
	{
		bool isLeftStick = this.controllerBehaviour.IsLeftStick;
		bool isRightStick = this.controllerBehaviour.IsRightStick;
		bool buttonDown = this.controllerBehaviour.ButtonDown;
		if (this._multiButtonRoot.activeInHierarchy)
		{
			if (isLeftStick)
			{
				this.progress += Time.deltaTime / this.holdTime;
				this.progressBarL.transform.localScale = new Vector3(0f, 1f, 1f);
				this.progressBarR.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
				this.progressBarR.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
				if (this.progress >= 1f)
				{
					this.OnConfirmedPressed();
					return;
				}
			}
			else if (isRightStick)
			{
				this.progress += Time.deltaTime / this.holdTime;
				this.progressBarR.transform.localScale = new Vector3(0f, 1f, 1f);
				this.progressBarL.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
				this.progressBarL.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
				if (this.progress >= 1f)
				{
					this.OnAlternativePressed();
					return;
				}
			}
			else
			{
				this.progress = 0f;
				this.progressBarR.transform.localScale = new Vector3(0f, 1f, 1f);
				this.progressBarL.transform.localScale = new Vector3(0f, 1f, 1f);
				this.progressBarL.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
			}
			return;
		}
		if (this._confirmButtonRoot.activeInHierarchy)
		{
			if (buttonDown)
			{
				this.progress += Time.deltaTime / this.holdTime;
				this.progressBar.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
				this.progressBar.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
				if (this.progress >= 1f)
				{
					this.OnConfirmedPressed();
					return;
				}
			}
			else
			{
				this.progress = 0f;
				this.progressBar.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
				this.progressBar.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
			}
			return;
		}
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x0004D7E9 File Offset: 0x0004B9E9
	private void OnConfirmedPressed()
	{
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
		this._hasCompleted = true;
		Action confirmationAction = this._confirmationAction;
		if (confirmationAction == null)
		{
			return;
		}
		confirmationAction();
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x0004D812 File Offset: 0x0004BA12
	private void OnAlternativePressed()
	{
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
		this._hasCompleted = true;
		Action alternativeAction = this._alternativeAction;
		if (alternativeAction == null)
		{
			return;
		}
		alternativeAction();
	}

	// Token: 0x040031D6 RID: 12758
	[SerializeField]
	private GameObject _uiParent;

	// Token: 0x040031D7 RID: 12759
	[SerializeField]
	private TMP_Text _messageTitleTxt;

	// Token: 0x040031D8 RID: 12760
	[SerializeField]
	private TMP_Text _messageBodyTxt;

	// Token: 0x040031D9 RID: 12761
	[SerializeField]
	private GameObject _confirmButtonRoot;

	// Token: 0x040031DA RID: 12762
	[SerializeField]
	private GameObject _multiButtonRoot;

	// Token: 0x040031DB RID: 12763
	[SerializeField]
	private TMP_Text _messageConfirmationTxt;

	// Token: 0x040031DC RID: 12764
	[SerializeField]
	private TMP_Text _messageAlternativeConfirmationTxt;

	// Token: 0x040031DD RID: 12765
	[SerializeField]
	private TMP_Text _messageAlternativeButtonTxt;

	// Token: 0x040031DE RID: 12766
	private Action _confirmationAction;

	// Token: 0x040031DF RID: 12767
	private Action _alternativeAction;

	// Token: 0x040031E0 RID: 12768
	private bool _hasCompleted;

	// Token: 0x040031E1 RID: 12769
	private float progress;

	// Token: 0x040031E2 RID: 12770
	[SerializeField]
	private float holdTime;

	// Token: 0x040031E3 RID: 12771
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x040031E4 RID: 12772
	[SerializeField]
	private LineRenderer progressBarL;

	// Token: 0x040031E5 RID: 12773
	[SerializeField]
	private LineRenderer progressBarR;

	// Token: 0x040031E6 RID: 12774
	private ControllerBehaviour controllerBehaviour;
}
