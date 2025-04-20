using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class MagicRingCosmetic : MonoBehaviour
{
	// Token: 0x060005AE RID: 1454 RVA: 0x0003428D File Offset: 0x0003248D
	protected void Awake()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		this.defaultEmissiveColor = this.ringRenderer.sharedMaterial.GetColor(this.emissionColorShaderPropID);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0008301C File Offset: 0x0008121C
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

	// Token: 0x0400068B RID: 1675
	[Tooltip("The ring will fade in the emissive texture based on temperature from this ThermalReceiver.")]
	public ThermalReceiver thermalReceiver;

	// Token: 0x0400068C RID: 1676
	public Renderer ringRenderer;

	// Token: 0x0400068D RID: 1677
	public float fadeInTemperatureThreshold = 200f;

	// Token: 0x0400068E RID: 1678
	public float fadeOutTemperatureThreshold = 190f;

	// Token: 0x0400068F RID: 1679
	public float fadeTime = 1.5f;

	// Token: 0x04000690 RID: 1680
	public SoundBankPlayer fadeInSounds;

	// Token: 0x04000691 RID: 1681
	public SoundBankPlayer fadeOutSounds;

	// Token: 0x04000692 RID: 1682
	private MagicRingCosmetic.FadeState fadeState;

	// Token: 0x04000693 RID: 1683
	private int emissionColorShaderPropID = Shader.PropertyToID("_EmissionColor");

	// Token: 0x04000694 RID: 1684
	private Color defaultEmissiveColor;

	// Token: 0x04000695 RID: 1685
	private float emissiveAmount;

	// Token: 0x04000696 RID: 1686
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x020000DC RID: 220
	private enum FadeState
	{
		// Token: 0x04000698 RID: 1688
		FadedOut,
		// Token: 0x04000699 RID: 1689
		FadedIn
	}
}
