using System;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
public class TapInnerGlow : MonoBehaviour
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060025F6 RID: 9718 RVA: 0x001076A0 File Offset: 0x001058A0
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

	// Token: 0x060025F7 RID: 9719 RVA: 0x001076DC File Offset: 0x001058DC
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

	// Token: 0x04002A2B RID: 10795
	public Renderer _renderer;

	// Token: 0x04002A2C RID: 10796
	public float tapLength = 1f;

	// Token: 0x04002A2D RID: 10797
	[Space]
	private Material _instance;
}
