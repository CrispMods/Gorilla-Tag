using System;
using TMPro;
using UnityEngine;

// Token: 0x020006DD RID: 1757
public class AgeSlider : MonoBehaviour, IBuildValidation
{
	// Token: 0x06002BC6 RID: 11206 RVA: 0x000D7179 File Offset: 0x000D5379
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x000D7188 File Offset: 0x000D5388
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x000D71A1 File Offset: 0x000D53A1
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000D71BC File Offset: 0x000D53BC
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

	// Token: 0x06002BCA RID: 11210 RVA: 0x000D72C0 File Offset: 0x000D54C0
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

	// Token: 0x06002BCB RID: 11211 RVA: 0x000D73B1 File Offset: 0x000D55B1
	public static void ToggleAgeGate(bool state)
	{
		AgeSlider._ageGateActive = state;
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x000D73B9 File Offset: 0x000D55B9
	public bool BuildValidationCheck()
	{
		if (this._confirmButton == null)
		{
			Debug.LogError("[KID] Object [_confirmButton] is NULL. Must be assigned in editor");
			return false;
		}
		return true;
	}

	// Token: 0x040030F1 RID: 12529
	private const int MIN_AGE = 13;

	// Token: 0x040030F2 RID: 12530
	[SerializeField]
	private int _maxAge = 99;

	// Token: 0x040030F3 RID: 12531
	[SerializeField]
	private TMP_Text _ageValueTxt;

	// Token: 0x040030F4 RID: 12532
	[SerializeField]
	private GameObject _confirmButton;

	// Token: 0x040030F5 RID: 12533
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x040030F6 RID: 12534
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x040030F7 RID: 12535
	private int _currentAge;

	// Token: 0x040030F8 RID: 12536
	private static bool _ageGateActive;

	// Token: 0x040030F9 RID: 12537
	private float progress;

	// Token: 0x040030FA RID: 12538
	private ControllerBehaviour controllerBehaviour;
}
