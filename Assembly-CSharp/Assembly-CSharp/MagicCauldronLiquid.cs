using System;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class MagicCauldronLiquid : MonoBehaviour
{
	// Token: 0x0600250F RID: 9487 RVA: 0x000B819B File Offset: 0x000B639B
	private void Test()
	{
		this._animProgress = 0f;
		this._animating = true;
		base.enabled = true;
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000B81B6 File Offset: 0x000B63B6
	public void AnimateColorFromTo(Color a, Color b, float length = 1f)
	{
		this._colorStart = a;
		this._colorEnd = b;
		this._animProgress = 0f;
		this._animating = true;
		this.animLength = length;
		base.enabled = true;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000B81E6 File Offset: 0x000B63E6
	private void ApplyColor(Color color)
	{
		if (!this._applyMaterial)
		{
			return;
		}
		this._applyMaterial.SetColor("_BaseColor", color);
		this._applyMaterial.Apply();
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000B8214 File Offset: 0x000B6414
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

	// Token: 0x06002513 RID: 9491 RVA: 0x000B826D File Offset: 0x000B646D
	private void OnEnable()
	{
		if (this._applyMaterial)
		{
			this._applyMaterial.mode = ApplyMaterialProperty.ApplyMode.MaterialPropertyBlock;
		}
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000B8288 File Offset: 0x000B6488
	private void OnDisable()
	{
		this._animating = false;
		this._animProgress = 0f;
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000B829C File Offset: 0x000B649C
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

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	private ApplyMaterialProperty _applyMaterial;

	// Token: 0x04002932 RID: 10546
	[SerializeField]
	private Color _colorStart;

	// Token: 0x04002933 RID: 10547
	[SerializeField]
	private Color _colorEnd;

	// Token: 0x04002934 RID: 10548
	[SerializeField]
	private bool _animating;

	// Token: 0x04002935 RID: 10549
	[SerializeField]
	private float _animProgress;

	// Token: 0x04002936 RID: 10550
	[SerializeField]
	private AnimationCurve _animationCurve = AnimationCurves.EaseOutCubic;

	// Token: 0x04002937 RID: 10551
	[SerializeField]
	private AnimationCurve _waveCurve = AnimationCurves.EaseInElastic;

	// Token: 0x04002938 RID: 10552
	public float animLength = 1f;

	// Token: 0x04002939 RID: 10553
	public MagicCauldronLiquid.WaveParams waveNormal;

	// Token: 0x0400293A RID: 10554
	public MagicCauldronLiquid.WaveParams waveAnimating;

	// Token: 0x020005D6 RID: 1494
	[Serializable]
	public struct WaveParams
	{
		// Token: 0x0400293B RID: 10555
		public float amplitude;

		// Token: 0x0400293C RID: 10556
		public float frequency;

		// Token: 0x0400293D RID: 10557
		public float scale;

		// Token: 0x0400293E RID: 10558
		public float rotation;
	}
}
