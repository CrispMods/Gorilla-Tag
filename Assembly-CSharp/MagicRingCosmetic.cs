using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class MagicRingCosmetic : MonoBehaviour
{
	// Token: 0x0600056D RID: 1389 RVA: 0x0002010B File Offset: 0x0001E30B
	protected void Awake()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		this.defaultEmissiveColor = this.ringRenderer.sharedMaterial.GetColor(this.emissionColorShaderPropID);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00020134 File Offset: 0x0001E334
	protected void LateUpdate()
	{
		float celsius = this.thermalReceiver.celsius;
		if (celsius >= this.fadeInTemperatureThreshold && this.fadeState != MagicRingCosmetic.FadeState.FadedIn)
		{
			this.fadeInSounds.Play();
			this.fadeState = MagicRingCosmetic.FadeState.FadedIn;
		}
		else if (celsius <= this.fadeOutTemperatureThreshold && this.fadeState != MagicRingCosmetic.FadeState.FadedOut)
		{
			this.fadeOutSounds.Play();
			this.fadeState = MagicRingCosmetic.FadeState.FadedOut;
		}
		this.emissiveAmount = Mathf.MoveTowards(this.emissiveAmount, (this.fadeState == MagicRingCosmetic.FadeState.FadedIn) ? 1f : 0f, Time.deltaTime / this.fadeTime);
		this.ringRenderer.GetPropertyBlock(this.materialPropertyBlock);
		this.materialPropertyBlock.SetColor(this.emissionColorShaderPropID, new Color(this.defaultEmissiveColor.r, this.defaultEmissiveColor.g, this.defaultEmissiveColor.b, this.emissiveAmount));
		this.ringRenderer.SetPropertyBlock(this.materialPropertyBlock);
	}

	// Token: 0x0400064A RID: 1610
	[Tooltip("The ring will fade in the emissive texture based on temperature from this ThermalReceiver.")]
	public ThermalReceiver thermalReceiver;

	// Token: 0x0400064B RID: 1611
	public Renderer ringRenderer;

	// Token: 0x0400064C RID: 1612
	public float fadeInTemperatureThreshold = 200f;

	// Token: 0x0400064D RID: 1613
	public float fadeOutTemperatureThreshold = 190f;

	// Token: 0x0400064E RID: 1614
	public float fadeTime = 1.5f;

	// Token: 0x0400064F RID: 1615
	public SoundBankPlayer fadeInSounds;

	// Token: 0x04000650 RID: 1616
	public SoundBankPlayer fadeOutSounds;

	// Token: 0x04000651 RID: 1617
	private MagicRingCosmetic.FadeState fadeState;

	// Token: 0x04000652 RID: 1618
	private int emissionColorShaderPropID = Shader.PropertyToID("_EmissionColor");

	// Token: 0x04000653 RID: 1619
	private Color defaultEmissiveColor;

	// Token: 0x04000654 RID: 1620
	private float emissiveAmount;

	// Token: 0x04000655 RID: 1621
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x020000D2 RID: 210
	private enum FadeState
	{
		// Token: 0x04000657 RID: 1623
		FadedOut,
		// Token: 0x04000658 RID: 1624
		FadedIn
	}
}
