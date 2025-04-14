using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class PassthroughStyler : MonoBehaviour
{
	// Token: 0x060012EC RID: 4844 RVA: 0x0005CB6C File Offset: 0x0005AD6C
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

	// Token: 0x060012ED RID: 4845 RVA: 0x0005CC62 File Offset: 0x0005AE62
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

	// Token: 0x060012EE RID: 4846 RVA: 0x0005CC7B File Offset: 0x0005AE7B
	public void OnBrightnessChanged(float newValue)
	{
		this._savedBrightness = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0005CC8A File Offset: 0x0005AE8A
	public void OnContrastChanged(float newValue)
	{
		this._savedContrast = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0005CC99 File Offset: 0x0005AE99
	public void OnSaturationChanged(float newValue)
	{
		this._savedSaturation = newValue;
		this.UpdateBrighnessContrastSaturation();
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0005CCA8 File Offset: 0x0005AEA8
	public void OnAlphaChanged(float newValue)
	{
		this._savedColor = new Color(this._savedColor.r, this._savedColor.g, this._savedColor.b, newValue);
		this._passthroughLayer.edgeColor = this._savedColor;
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0005CCE8 File Offset: 0x0005AEE8
	public void OnBlendChange(float newValue)
	{
		this._savedBlend = newValue;
		this._passthroughLayer.SetColorLut(this._passthroughColorLut, this._savedBlend);
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0005CD08 File Offset: 0x0005AF08
	public void DoColorDrag(bool doDrag)
	{
		this._settingColor = doDrag;
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0005CD11 File Offset: 0x0005AF11
	public void SetPassthroughStyleToColorAdjustment(bool isOn)
	{
		if (isOn)
		{
			this.SetPassthroughStyle(OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment);
		}
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0005CD1D File Offset: 0x0005AF1D
	public void SetPassthroughStyleToColorLut(bool isOn)
	{
		if (isOn)
		{
			this.SetPassthroughStyle(OVRPassthroughLayer.ColorMapEditorType.ColorLut);
		}
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0005CD2C File Offset: 0x0005AF2C
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

	// Token: 0x060012F7 RID: 4855 RVA: 0x0005CD94 File Offset: 0x0005AF94
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

	// Token: 0x060012F8 RID: 4856 RVA: 0x0005CDFA File Offset: 0x0005AFFA
	private IEnumerator FadeToCurrentStyle(float fadeTime)
	{
		this._passthroughLayer.edgeRenderingEnabled = true;
		yield return this.FadeTo(1f, fadeTime);
		yield break;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0005CE10 File Offset: 0x0005B010
	private IEnumerator FadeToDefaultPassthrough(float fadeTime)
	{
		yield return this.FadeTo(0f, fadeTime);
		this._passthroughLayer.edgeRenderingEnabled = false;
		yield break;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0005CE26 File Offset: 0x0005B026
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

	// Token: 0x060012FB RID: 4859 RVA: 0x0005CE43 File Offset: 0x0005B043
	private void UpdateBrighnessContrastSaturation()
	{
		this._passthroughLayer.SetBrightnessContrastSaturation(this._savedBrightness, this._savedContrast, this._savedSaturation);
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0005CE64 File Offset: 0x0005B064
	private void ShowFullMenu(bool doShow)
	{
		GameObject[] compactObjects = this._compactObjects;
		for (int i = 0; i < compactObjects.Length; i++)
		{
			compactObjects[i].SetActive(doShow);
		}
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0005CE8F File Offset: 0x0005B08F
	private void Cursor(Vector3 cP)
	{
		this._cursorPosition = cP;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0005CE98 File Offset: 0x0005B098
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

	// Token: 0x060012FF RID: 4863 RVA: 0x0005CFE8 File Offset: 0x0005B1E8
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

	// Token: 0x040014ED RID: 5357
	private const float FadeDuration = 0.2f;

	// Token: 0x040014EE RID: 5358
	[SerializeField]
	private OVRInput.Controller _controllerHand;

	// Token: 0x040014EF RID: 5359
	[SerializeField]
	private OVRPassthroughLayer _passthroughLayer;

	// Token: 0x040014F0 RID: 5360
	[SerializeField]
	private RectTransform _colorWheel;

	// Token: 0x040014F1 RID: 5361
	[SerializeField]
	private Texture2D _colorTexture;

	// Token: 0x040014F2 RID: 5362
	[SerializeField]
	private Texture2D _colorLutTexture;

	// Token: 0x040014F3 RID: 5363
	[SerializeField]
	private CanvasGroup _mainCanvas;

	// Token: 0x040014F4 RID: 5364
	[SerializeField]
	private GameObject[] _compactObjects;

	// Token: 0x040014F5 RID: 5365
	[SerializeField]
	private GameObject[] _objectsToHideForColorPassthrough;

	// Token: 0x040014F6 RID: 5366
	private Vector3 _cursorPosition = Vector3.zero;

	// Token: 0x040014F7 RID: 5367
	private bool _settingColor;

	// Token: 0x040014F8 RID: 5368
	private Color _savedColor = Color.white;

	// Token: 0x040014F9 RID: 5369
	private float _savedBrightness;

	// Token: 0x040014FA RID: 5370
	private float _savedContrast;

	// Token: 0x040014FB RID: 5371
	private float _savedSaturation;

	// Token: 0x040014FC RID: 5372
	private OVRPassthroughLayer.ColorMapEditorType _currentStyle = OVRPassthroughLayer.ColorMapEditorType.ColorAdjustment;

	// Token: 0x040014FD RID: 5373
	private float _savedBlend = 1f;

	// Token: 0x040014FE RID: 5374
	private OVRPassthroughColorLut _passthroughColorLut;

	// Token: 0x040014FF RID: 5375
	private IEnumerator _fade;
}
