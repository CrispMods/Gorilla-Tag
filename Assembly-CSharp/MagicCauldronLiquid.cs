using System;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class MagicCauldronLiquid : MonoBehaviour
{
	// Token: 0x06002507 RID: 9479 RVA: 0x000B7D1B File Offset: 0x000B5F1B
	private void Test()
	{
		this._animProgress = 0f;
		this._animating = true;
		base.enabled = true;
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x000B7D36 File Offset: 0x000B5F36
	public void AnimateColorFromTo(Color a, Color b, float length = 1f)
	{
		this._colorStart = a;
		this._colorEnd = b;
		this._animProgress = 0f;
		this._animating = true;
		this.animLength = length;
		base.enabled = true;
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000B7D66 File Offset: 0x000B5F66
	private void ApplyColor(Color color)
	{
		if (!this._applyMaterial)
		{
			return;
		}
		this._applyMaterial.SetColor("_BaseColor", color);
		this._applyMaterial.Apply();
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000B7D94 File Offset: 0x000B5F94
	private void ApplyWaveParams(float amplitude, float frequency, float scale, float rotation)
	{
		if (!this._applyMaterial)
		{
			return;
		}
		this._applyMaterial.SetFloat("_WaveAmplitude", amplitude);
		this._applyMaterial.SetFloat("_WaveFrequency", frequency);
		this._applyMaterial.SetFloat("_WaveScale", scale);
		this._applyMaterial.Apply();
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000B7DED File Offset: 0x000B5FED
	private void OnEnable()
	{
		if (this._applyMaterial)
		{
			this._applyMaterial.mode = ApplyMaterialProperty.ApplyMode.MaterialPropertyBlock;
		}
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000B7E08 File Offset: 0x000B6008
	private void OnDisable()
	{
		this._animating = false;
		this._animProgress = 0f;
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000B7E1C File Offset: 0x000B601C
	private void Update()
	{
		if (!this._animating)
		{
			return;
		}
		float num = this._animationCurve.Evaluate(this._animProgress / this.animLength);
		float t = this._waveCurve.Evaluate(this._animProgress / this.animLength);
		if (num >= 1f)
		{
			this.ApplyColor(this._colorEnd);
			this._animating = false;
			base.enabled = false;
			return;
		}
		Color color = Color.Lerp(this._colorStart, this._colorEnd, num);
		Mathf.Lerp(this.waveNormal.frequency, this.waveAnimating.frequency, t);
		Mathf.Lerp(this.waveNormal.amplitude, this.waveAnimating.amplitude, t);
		Mathf.Lerp(this.waveNormal.scale, this.waveAnimating.scale, t);
		Mathf.Lerp(this.waveNormal.rotation, this.waveAnimating.rotation, t);
		this.ApplyColor(color);
		this._animProgress += Time.deltaTime;
	}

	// Token: 0x0400292B RID: 10539
	[SerializeField]
	private ApplyMaterialProperty _applyMaterial;

	// Token: 0x0400292C RID: 10540
	[SerializeField]
	private Color _colorStart;

	// Token: 0x0400292D RID: 10541
	[SerializeField]
	private Color _colorEnd;

	// Token: 0x0400292E RID: 10542
	[SerializeField]
	private bool _animating;

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	private float _animProgress;

	// Token: 0x04002930 RID: 10544
	[SerializeField]
	private AnimationCurve _animationCurve = AnimationCurves.EaseOutCubic;

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	private AnimationCurve _waveCurve = AnimationCurves.EaseInElastic;

	// Token: 0x04002932 RID: 10546
	public float animLength = 1f;

	// Token: 0x04002933 RID: 10547
	public MagicCauldronLiquid.WaveParams waveNormal;

	// Token: 0x04002934 RID: 10548
	public MagicCauldronLiquid.WaveParams waveAnimating;

	// Token: 0x020005D5 RID: 1493
	[Serializable]
	public struct WaveParams
	{
		// Token: 0x04002935 RID: 10549
		public float amplitude;

		// Token: 0x04002936 RID: 10550
		public float frequency;

		// Token: 0x04002937 RID: 10551
		public float scale;

		// Token: 0x04002938 RID: 10552
		public float rotation;
	}
}
