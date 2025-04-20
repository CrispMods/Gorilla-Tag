using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

// Token: 0x02000717 RID: 1815
public class PreGameMessage : MonoBehaviour
{
	// Token: 0x06002D1F RID: 11551 RVA: 0x0004EADB File Offset: 0x0004CCDB
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002D20 RID: 11552 RVA: 0x0004EAEA File Offset: 0x0004CCEA
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x0004EB03 File Offset: 0x0004CD03
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x001265C8 File Offset: 0x001247C8
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

	// Token: 0x06002D23 RID: 11555 RVA: 0x0012666C File Offset: 0x0012486C
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

	// Token: 0x06002D24 RID: 11556 RVA: 0x00126744 File Offset: 0x00124944
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

	// Token: 0x06002D25 RID: 11557 RVA: 0x001267B4 File Offset: 0x001249B4
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

	// Token: 0x06002D26 RID: 11558 RVA: 0x0004EB1C File Offset: 0x0004CD1C
	public void CloseMessage()
	{
		PrivateUIRoom.RemoveUI(this._uiParent.transform);
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x0012680C File Offset: 0x00124A0C
	private Task WaitForCompletion()
	{
		PreGameMessage.<WaitForCompletion>d__25 <WaitForCompletion>d__;
		<WaitForCompletion>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<WaitForCompletion>d__.<>4__this = this;
		<WaitForCompletion>d__.<>1__state = -1;
		<WaitForCompletion>d__.<>t__builder.Start<PreGameMessage.<WaitForCompletion>d__25>(ref <WaitForCompletion>d__);
		return <WaitForCompletion>d__.<>t__builder.Task;
	}

	// Token: 0x06002D28 RID: 11560 RVA: 0x00126850 File Offset: 0x00124A50
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

	// Token: 0x06002D29 RID: 11561 RVA: 0x0004EB2E File Offset: 0x0004CD2E
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

	// Token: 0x06002D2A RID: 11562 RVA: 0x0004EB57 File Offset: 0x0004CD57
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

	// Token: 0x0400326D RID: 12909
	[SerializeField]
	private GameObject _uiParent;

	// Token: 0x0400326E RID: 12910
	[SerializeField]
	private TMP_Text _messageTitleTxt;

	// Token: 0x0400326F RID: 12911
	[SerializeField]
	private TMP_Text _messageBodyTxt;

	// Token: 0x04003270 RID: 12912
	[SerializeField]
	private GameObject _confirmButtonRoot;

	// Token: 0x04003271 RID: 12913
	[SerializeField]
	private GameObject _multiButtonRoot;

	// Token: 0x04003272 RID: 12914
	[SerializeField]
	private TMP_Text _messageConfirmationTxt;

	// Token: 0x04003273 RID: 12915
	[SerializeField]
	private TMP_Text _messageAlternativeConfirmationTxt;

	// Token: 0x04003274 RID: 12916
	[SerializeField]
	private TMP_Text _messageAlternativeButtonTxt;

	// Token: 0x04003275 RID: 12917
	private Action _confirmationAction;

	// Token: 0x04003276 RID: 12918
	private Action _alternativeAction;

	// Token: 0x04003277 RID: 12919
	private bool _hasCompleted;

	// Token: 0x04003278 RID: 12920
	private float progress;

	// Token: 0x04003279 RID: 12921
	[SerializeField]
	private float holdTime;

	// Token: 0x0400327A RID: 12922
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x0400327B RID: 12923
	[SerializeField]
	private LineRenderer progressBarL;

	// Token: 0x0400327C RID: 12924
	[SerializeField]
	private LineRenderer progressBarR;

	// Token: 0x0400327D RID: 12925
	private ControllerBehaviour controllerBehaviour;
}
