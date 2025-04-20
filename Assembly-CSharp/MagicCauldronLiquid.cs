using System;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class MagicCauldronLiquid : MonoBehaviour
{
	// Token: 0x06002569 RID: 9577 RVA: 0x00049535 File Offset: 0x00047735
	private void Test()
	{
		this._animProgress = 0f;
		this._animating = true;
		base.enabled = true;
	}

	// Token: 0x0600256A RID: 9578 RVA: 0x00049550 File Offset: 0x00047750
	public void AnimateColorFromTo(Color a, Color b, float length = 1f)
	{
		this._colorStart = a;
		this._colorEnd = b;
		this._animProgress = 0f;
		this._animating = true;
		this.animLength = length;
		base.enabled = true;
	}

	// Token: 0x0600256B RID: 9579 RVA: 0x00049580 File Offset: 0x00047780
	private void ApplyColor(Color color)
	{
		if (!this._applyMaterial)
		{
			return;
		}
		this._applyMaterial.SetColor("_BaseColor", color);
		this._applyMaterial.Apply();
	}

	// Token: 0x0600256C RID: 9580 RVA: 0x0010602C File Offset: 0x0010422C
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

	// Token: 0x0600256D RID: 9581 RVA: 0x000495AC File Offset: 0x000477AC
	private void OnEnable()
	{
		if (this._applyMaterial)
		{
			this._applyMaterial.mode = ApplyMaterialProperty.ApplyMode.MaterialPropertyBlock;
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x000495C7 File Offset: 0x000477C7
	private void OnDisable()
	{
		this._animating = false;
		this._animProgress = 0f;
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x00106088 File Offset: 0x00104288
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

	// Token: 0x0400298A RID: 10634
	[SerializeField]
	private ApplyMaterialProperty _applyMaterial;

	// Token: 0x0400298B RID: 10635
	[SerializeField]
	private Color _colorStart;

	// Token: 0x0400298C RID: 10636
	[SerializeField]
	private Color _colorEnd;

	// Token: 0x0400298D RID: 10637
	[SerializeField]
	private bool _animating;

	// Token: 0x0400298E RID: 10638
	[SerializeField]
	private float _animProgress;

	// Token: 0x0400298F RID: 10639
	[SerializeField]
	private AnimationCurve _animationCurve = AnimationCurves.EaseOutCubic;

	// Token: 0x04002990 RID: 10640
	[SerializeField]
	private AnimationCurve _waveCurve = AnimationCurves.EaseInElastic;

	// Token: 0x04002991 RID: 10641
	public float animLength = 1f;

	// Token: 0x04002992 RID: 10642
	public MagicCauldronLiquid.WaveParams waveNormal;

	// Token: 0x04002993 RID: 10643
	public MagicCauldronLiquid.WaveParams waveAnimating;

	// Token: 0x020005E3 RID: 1507
	[Serializable]
	public struct WaveParams
	{
		// Token: 0x04002994 RID: 10644
		public float amplitude;

		// Token: 0x04002995 RID: 10645
		public float frequency;

		// Token: 0x04002996 RID: 10646
		public float scale;

		// Token: 0x04002997 RID: 10647
		public float rotation;
	}
}
