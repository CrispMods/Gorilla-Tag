using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class PassthroughStyler : MonoBehaviour
{
	// Token: 0x06001335 RID: 4917 RVA: 0x000B6854 File Offset: 0x000B4A54
	private void Start()
	{
		GrabObject grabObject;
		if (base.TryGetComponent<GrabObject>(out grabObject))
		{
			GrabObject grabObject2 = grabObject;
			grabObject2.GrabbedObjectDelegate = (GrabObject.GrabbedObject)Delegate.Combine(grabObject2.GrabbedObjectDelegate, new GrabObject.GrabbedObject(this.Grab));
			GrabObject grabObject3 = grabObject;
			grabObject3.ReleasedObjectDelegate = (GrabObject.ReleasedObject)Delegate.Combine(grabObject3.ReleasedObjectDelegate, new GrabObject.ReleasedObject(this.Release));
			GrabObject grabObject4 = grabObject;
			grabObject4.CursorPositionDelegate = (GrabObject.SetCursorPosition)Delegate.Combine(grabObject4.CursorPositionDelegate, new GrabObject.SetCursorPosition(this.Cursor));
		}
		this._savedColor = new Color(1f, 1f, 1f, 0f);
		this.ShowFullMenu(false);
		this._mainCanvas.interactable = false;
		this._passthroughColorLut = new OVRPassthroughColorLut(this._colorLutTexture, true);
		if (!OVRManager.GetPassthroughCapabilities().SupportsColorPassthrough && this._objectsToHideForColorPassthrough != null)
		{
			for (int i = 0; i < this._objectsToHideForColorPassthrough.Length; i++)
			{
				this._objectsToHideForColorPassthrough[i].SetActive(false);
			}
		}
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0003D0BB File Offset: 0x0003B2BB
	private void Update()
	{
		if (this._controllerHand == OVRInput.Controller.None)
		{
			return;
		}
		if (this._settingColor)
		{
			this.GetColorFromWheel();
		}
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0003D0D4 File Offset: 0x0003B2D4
	public void OnBrightnessChanged(float newValue)
	{
		this._savedBrightness = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x0003D0E3 File Offset: 0x0003B2E3
	public void OnContrastChanged(float newValue)
	{
		this._savedContrast = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0003D0F2 File Offset: 0x0003B2F2
	public void OnSaturationChanged(float newValue)
	{
		this._savedSaturation = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0003D101 File Offset: 0x0003B301
	public void OnAlphaChanged(float newValue)
	{
		this._savedColor = new Color(this._savedColor.r, this._savedColor.g, this._savedColor.b, newValue);
		this._passthroughLayer.edgeColor = this._savedColor;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0003D141 File Offset: 0x0003B341
	public void OnBlendChange(float newValue)
	{
		this._savedBlend = newValue;
		this._passthroughLayer.SetColorLut(this._passthroughColorLut, this._savedBlend);
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0003D161 File Offset: 0x0003B361
	public void DoColorDrag(bool doDrag)
	{
		this._settingColor = doDrag;
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0003D16A File Offset: 0x0003B36A
	public void SetPassthroughStyleToColorAdjustment(bool isOn)
	{
		if (isOn)
		{
			this.SetPassthroughStyle(OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment);
		}
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0003D176 File Offset: 0x0003B376
	public void SetPassthroughStyleToColorLut(bool isOn)
	{
		if (isOn)
		{
			this.SetPassthroughStyle(OVRPassthroughLayer.ColorMapEditorType.ColorLut);
		}
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x000B694C File Offset: 0x000B4B4C
	private void Grab(OVRInput.Controller grabHand)
	{
		this._controllerHand = grabHand;
		this.ShowFullMenu(true);
		if (this._mainCanvas)
		{
			this._mainCanvas.interactable = true;
		}
		if (this._fade != null)
		{
			base.StopCoroutine(this._fade);
		}
		this._fade = this.FadeToCurrentStyle(0.2f);
		base.StartCoroutine(this._fade);
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000B69B4 File Offset: 0x000B4BB4
	private void Release()
	{
		this._controllerHand = OVRInput.Controller.None;
		this.ShowFullMenu(false);
		if (this._mainCanvas)
		{
			this._mainCanvas.interactable = false;
		}
		if (this._fade != null)
		{
			base.StopCoroutine(this._fade);
		}
		this._fade = this.FadeToDefaultPassthrough(0.2f);
		base.StartCoroutine(this._fade);
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0003D182 File Offset: 0x0003B382
	private IEnumerator FadeToCurrentStyle(float fadeTime)
	{
		this._passthroughLayer.edgeRenderingEnabled = true;
		yield return this.FadeTo(1f, fadeTime);
		yield break;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0003D198 File Offset: 0x0003B398
	private IEnumerator FadeToDefaultPassthrough(float fadeTime)
	{
		yield return this.FadeTo(0f, fadeTime);
		this._passthroughLayer.edgeRenderingEnabled = false;
		yield break;
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0003D1AE File Offset: 0x0003B3AE
	private IEnumerator FadeTo(float styleValueMultiplier, float duration)
	{
		float timer = 0f;
		float brightness = this._passthroughLayer.colorMapEditorBrightness;
		float contrast = this._passthroughLayer.colorMapEditorContrast;
		float saturation = this._passthroughLayer.colorMapEditorSaturation;
		Color edgeCol = this._passthroughLayer.edgeColor;
		float blend = this._savedBlend;
		while (timer <= duration)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / duration);
			if (this._currentStyle == OVRPassthroughLayer.ColorMapEditorType.ColorLut)
			{
				this._passthroughLayer.SetColorLut(this._passthroughColorLut, Mathf.Lerp(blend, this._savedBlend * styleValueMultiplier, t));
			}
			else
			{
				this._passthroughLayer.SetBrightnessContrastSaturation(Mathf.Lerp(brightness, this._savedBrightness * styleValueMultiplier, t), Mathf.Lerp(contrast, this._savedContrast * styleValueMultiplier, t), Mathf.Lerp(saturation, this._savedSaturation * styleValueMultiplier, t));
			}
			this._passthroughLayer.edgeColor = Color.Lerp(edgeCol, new Color(this._savedColor.r, this._savedColor.g, this._savedColor.b, this._savedColor.a * styleValueMultiplier), t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0003D1CB File Offset: 0x0003B3CB
	private void UpdateBrighnessContrastSaturation()
	{
		this._passthroughLayer.SetBrightnessContrastSaturation(this._savedBrightness, this._savedContrast, this._savedSaturation);
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x000B6A1C File Offset: 0x000B4C1C
	private void ShowFullMenu(bool doShow)
	{
		GameObject[] compactObjects = this._compactObjects;
		for (int i = 0; i < compactObjects.Length; i++)
		{
			compactObjects[i].SetActive(doShow);
		}
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0003D1EA File Offset: 0x0003B3EA
	private void Cursor(Vector3 cP)
	{
		this._cursorPosition = cP;
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x000B6A48 File Offset: 0x000B4C48
	private void GetColorFromWheel()
	{
		Vector3 vector = this._colorWheel.transform.InverseTransformPoint(this._cursorPosition);
		Vector2 vector2 = new Vector2(vector.x / this._colorWheel.sizeDelta.x + 0.5f, vector.y / this._colorWheel.sizeDelta.y + 0.5f);
		Debug.Log("Sanctuary: " + vector2.x.ToString() + ", " + vector2.y.ToString());
		Color color = Color.black;
		if ((double)vector2.x < 1.0 && vector2.x > 0f && (double)vector2.y < 1.0 && vector2.y > 0f)
		{
			int x = Mathf.RoundToInt(vector2.x * (float)this._colorTexture.width);
			int y = Mathf.RoundToInt(vector2.y * (float)this._colorTexture.height);
			color = this._colorTexture.GetPixel(x, y);
		}
		this._savedColor = new Color(color.r, color.g, color.b, this._savedColor.a);
		this._passthroughLayer.edgeColor = this._savedColor;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x0003D1F3 File Offset: 0x0003B3F3
	private void SetPassthroughStyle(OVRPassthroughLayer.ColorMapEditorType passthroughStyle)
	{
		this._currentStyle = passthroughStyle;
		if (this._currentStyle == OVRPassthroughLayer.ColorMapEditorType.ColorLut)
		{
			this._passthroughLayer.SetColorLut(this._passthroughColorLut, this._savedBlend);
			return;
		}
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x04001534 RID: 5428
	private const float FadeDuration = 0.2f;

	// Token: 0x04001535 RID: 5429
	[SerializeField]
	private OVRInput.Controller _controllerHand;

	// Token: 0x04001536 RID: 5430
	[SerializeField]
	private OVRPassthroughLayer _passthroughLayer;

	// Token: 0x04001537 RID: 5431
	[SerializeField]
	private RectTransform _colorWheel;

	// Token: 0x04001538 RID: 5432
	[SerializeField]
	private Texture2D _colorTexture;

	// Token: 0x04001539 RID: 5433
	[SerializeField]
	private Texture2D _colorLutTexture;

	// Token: 0x0400153A RID: 5434
	[SerializeField]
	private CanvasGroup _mainCanvas;

	// Token: 0x0400153B RID: 5435
	[SerializeField]
	private GameObject[] _compactObjects;

	// Token: 0x0400153C RID: 5436
	[SerializeField]
	private GameObject[] _objectsToHideForColorPassthrough;

	// Token: 0x0400153D RID: 5437
	private Vector3 _cursorPosition = Vector3.zero;

	// Token: 0x0400153E RID: 5438
	private bool _settingColor;

	// Token: 0x0400153F RID: 5439
	private Color _savedColor = Color.white;

	// Token: 0x04001540 RID: 5440
	private float _savedBrightness;

	// Token: 0x04001541 RID: 5441
	private float _savedContrast;

	// Token: 0x04001542 RID: 5442
	private float _savedSaturation;

	// Token: 0x04001543 RID: 5443
	private OVRPassthroughLayer.ColorMapEditorType _currentStyle = OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment;

	// Token: 0x04001544 RID: 5444
	private float _savedBlend = 1f;

	// Token: 0x04001545 RID: 5445
	private OVRPassthroughColorLut _passthroughColorLut;

	// Token: 0x04001546 RID: 5446
	private IEnumerator _fade;
}
