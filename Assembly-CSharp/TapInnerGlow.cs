using System;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class TapInnerGlow : MonoBehaviour
{
	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06002594 RID: 9620 RVA: 0x000B9AF4 File Offset: 0x000B7CF4
	private Material targetMaterial
	{
		get
		{
			if (this._instance.AsNull<Material>() == null)
			{
				return this._instance = this._renderer.material;
			}
			return this._instance;
		}
	}

	// Token: 0x06002595 RID: 9621 RVA: 0x000B9B30 File Offset: 0x000B7D30
	public void Tap()
	{
		if (!this._renderer)
		{
			return;
		}
		Material targetMaterial = this.targetMaterial;
		float value = this.tapLength;
		float time = GTShaderGlobals.Time;
		UberShader.InnerGlowSinePeriod.SetValue<float>(targetMaterial, value);
		UberShader.InnerGlowSinePhaseShift.SetValue<float>(targetMaterial, time);
	}

	// Token: 0x040029CC RID: 10700
	public Renderer _renderer;

	// Token: 0x040029CD RID: 10701
	public float tapLength = 1f;

	// Token: 0x040029CE RID: 10702
	[Space]
	private Material _instance;
}
