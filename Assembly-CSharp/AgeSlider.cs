using System;
using TMPro;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
public class AgeSlider : MonoBehaviour, IBuildValidation
{
	// Token: 0x06002C5C RID: 11356 RVA: 0x0004E247 File Offset: 0x0004C447
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x0004E256 File Offset: 0x0004C456
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x0004E26F File Offset: 0x0004C46F
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x00122310 File Offset: 0x00120510
	protected void Update()
	{
		if (!AgeSlider._ageGateActive)
		{
			return;
		}
		if (this.controllerBehaviour.ButtonDown && this._confirmButton.activeInHierarchy)
		{
			this.progress += Time.deltaTime / this.holdTime;
			this.progressBar.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
			this.progressBar.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
			if (this.progress >= 1f)
			{
				KIDAgeGate.OnConfirmAgePressed(this._currentAge);
				return;
			}
		}
		else
		{
			this.progress = 0f;
			this.progressBar.transform.localScale = new Vector3(Mathf.Clamp01(this.progress), 1f, 1f);
			this.progressBar.textureScale = new Vector2(Mathf.Clamp01(this.progress), -1f);
		}
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x00122414 File Offset: 0x00120614
	private void PostUpdate()
	{
		if (!AgeSlider._ageGateActive)
		{
			return;
		}
		if (this.controllerBehaviour.IsLeftStick || this.controllerBehaviour.IsUpStick)
		{
			this._currentAge = Mathf.Clamp(this._currentAge - 1, 0, this._maxAge);
			this._ageValueTxt.text = ((this._currentAge > 0) ? this._currentAge.ToString() : "?");
			this._confirmButton.SetActive(this._currentAge > 0);
		}
		if (this.controllerBehaviour.IsRightStick || this.controllerBehaviour.IsDownStick)
		{
			this._currentAge = Mathf.Clamp(this._currentAge + 1, 0, this._maxAge);
			this._ageValueTxt.text = ((this._currentAge > 0) ? this._currentAge.ToString() : "?");
			this._confirmButton.SetActive(this._currentAge > 0);
		}
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x0004E288 File Offset: 0x0004C488
	public static void ToggleAgeGate(bool state)
	{
		AgeSlider._ageGateActive = state;
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x0004E290 File Offset: 0x0004C490
	public bool BuildValidationCheck()
	{
		if (this._confirmButton == null)
		{
			Debug.LogError("[KID] Object [_confirmButton] is NULL. Must be assigned in editor");
			return false;
		}
		return true;
	}

	// Token: 0x0400318E RID: 12686
	private const int MIN_AGE = 13;

	// Token: 0x0400318F RID: 12687
	[SerializeField]
	private int _maxAge = 99;

	// Token: 0x04003190 RID: 12688
	[SerializeField]
	private TMP_Text _ageValueTxt;

	// Token: 0x04003191 RID: 12689
	[SerializeField]
	private GameObject _confirmButton;

	// Token: 0x04003192 RID: 12690
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x04003193 RID: 12691
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x04003194 RID: 12692
	private int _currentAge;

	// Token: 0x04003195 RID: 12693
	private static bool _ageGateActive;

	// Token: 0x04003196 RID: 12694
	private float progress;

	// Token: 0x04003197 RID: 12695
	private ControllerBehaviour controllerBehaviour;
}
