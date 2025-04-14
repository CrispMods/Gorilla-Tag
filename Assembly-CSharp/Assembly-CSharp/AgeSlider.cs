using System;
using TMPro;
using UnityEngine;

// Token: 0x020006DE RID: 1758
public class AgeSlider : MonoBehaviour, IBuildValidation
{
	// Token: 0x06002BCE RID: 11214 RVA: 0x000D75F9 File Offset: 0x000D57F9
	private void Awake()
	{
		this.controllerBehaviour = base.GetComponentInChildren<ControllerBehaviour>(true);
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x000D7608 File Offset: 0x000D5808
	private void OnEnable()
	{
		this.controllerBehaviour.OnAction += this.PostUpdate;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000D7621 File Offset: 0x000D5821
	private void OnDisable()
	{
		this.controllerBehaviour.OnAction -= this.PostUpdate;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000D763C File Offset: 0x000D583C
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

	// Token: 0x06002BD2 RID: 11218 RVA: 0x000D7740 File Offset: 0x000D5940
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

	// Token: 0x06002BD3 RID: 11219 RVA: 0x000D7831 File Offset: 0x000D5A31
	public static void ToggleAgeGate(bool state)
	{
		AgeSlider._ageGateActive = state;
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x000D7839 File Offset: 0x000D5A39
	public bool BuildValidationCheck()
	{
		if (this._confirmButton == null)
		{
			Debug.LogError("[KID] Object [_confirmButton] is NULL. Must be assigned in editor");
			return false;
		}
		return true;
	}

	// Token: 0x040030F7 RID: 12535
	private const int MIN_AGE = 13;

	// Token: 0x040030F8 RID: 12536
	[SerializeField]
	private int _maxAge = 99;

	// Token: 0x040030F9 RID: 12537
	[SerializeField]
	private TMP_Text _ageValueTxt;

	// Token: 0x040030FA RID: 12538
	[SerializeField]
	private GameObject _confirmButton;

	// Token: 0x040030FB RID: 12539
	[SerializeField]
	private float holdTime = 5f;

	// Token: 0x040030FC RID: 12540
	[SerializeField]
	private LineRenderer progressBar;

	// Token: 0x040030FD RID: 12541
	private int _currentAge;

	// Token: 0x040030FE RID: 12542
	private static bool _ageGateActive;

	// Token: 0x040030FF RID: 12543
	private float progress;

	// Token: 0x04003100 RID: 12544
	private ControllerBehaviour controllerBehaviour;
}
