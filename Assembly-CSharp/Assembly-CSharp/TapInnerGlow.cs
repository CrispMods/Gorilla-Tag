using System;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
public class TapInnerGlow : MonoBehaviour
{
	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x0600259C RID: 9628 RVA: 0x000B9F74 File Offset: 0x000B8174
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

	// Token: 0x0600259D RID: 9629 RVA: 0x000B9FB0 File Offset: 0x000B81B0
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

	// Token: 0x040029D2 RID: 10706
	public Renderer _renderer;

	// Token: 0x040029D3 RID: 10707
	public float tapLength = 1f;

	// Token: 0x040029D4 RID: 10708
	[Space]
	private Material _instance;
}
