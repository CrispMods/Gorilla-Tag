using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000A9 RID: 169
public class SkyboxController : MonoBehaviour
{
	// Token: 0x06000462 RID: 1122 RVA: 0x0007CB64 File Offset: 0x0007AD64
	private void Start()
	{
		if (this._dayNightManager.AsNull<BetterDayNightManager>() == null)
		{
			this._dayNightManager = BetterDayNightManager.instance;
		}
		if (this._dayNightManager.AsNull<BetterDayNightManager>() == null)
		{
			return;
		}
		for (int i = 0; i < this._dayNightManager.timeOfDayRange.Length; i++)
		{
			this._totalSecondsInRange += this._dayNightManager.timeOfDayRange[i] * 3600.0;
		}
		this._totalSecondsInRange = Math.Floor(this._totalSecondsInRange);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000334C5 File Offset: 0x000316C5
	private void Update()
	{
		if (!this.lastUpdate.HasElapsed(1f, true))
		{
			return;
		}
		this.UpdateTime();
		this.UpdateSky();
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000334E7 File Offset: 0x000316E7
	private void OnValidate()
	{
		this.UpdateSky();
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000334EF File Offset: 0x000316EF
	private void UpdateTime()
	{
		this._currentSeconds = ((ITimeOfDaySystem)this._dayNightManager).currentTimeInSeconds;
		this._currentSeconds = Math.Floor(this._currentSeconds);
		this._currentTime = (float)(this._currentSeconds / this._totalSecondsInRange);
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0007CBF4 File Offset: 0x0007ADF4
	private void UpdateSky()
	{
		if (this.skyMaterials == null || this.skyMaterials.Length == 0)
		{
			return;
		}
		int num = this.skyMaterials.Length;
		float num2 = Mathf.Clamp(this._currentTime, 0f, 1f);
		float num3 = 1f / (float)num;
		int num4 = (int)(num2 / num3);
		float num5 = (num2 - (float)num4 * num3) / num3;
		this._currentSky = this.skyMaterials[num4];
		this._nextSky = this.skyMaterials[(num4 + 1) % num];
		this.skyFront.sharedMaterial = this._currentSky;
		this.skyBack.sharedMaterial = this._nextSky;
		if (this._currentSky.renderQueue != 3000)
		{
			this.SetFrontToTransparent();
		}
		if (this._nextSky.renderQueue == 3000)
		{
			this.SetBackToOpaque();
		}
		this._currentSky.SetFloat(SkyboxController._SkyAlpha, 1f - num5);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0007CCD8 File Offset: 0x0007AED8
	private void SetFrontToTransparent()
	{
		bool flag = false;
		bool flag2 = false;
		string val = "Transparent";
		int renderQueue = 3000;
		BlendMode blendMode = BlendMode.SrcAlpha;
		BlendMode blendMode2 = BlendMode.OneMinusSrcAlpha;
		BlendMode blendMode3 = BlendMode.One;
		BlendMode blendMode4 = BlendMode.OneMinusSrcAlpha;
		Material sharedMaterial = this.skyFront.sharedMaterial;
		sharedMaterial.SetFloat("_ZWrite", flag ? 1f : 0f);
		sharedMaterial.SetShaderPassEnabled("DepthOnly", flag);
		sharedMaterial.SetFloat("_AlphaToMask", flag2 ? 1f : 0f);
		sharedMaterial.SetOverrideTag("RenderType", val);
		sharedMaterial.renderQueue = renderQueue;
		sharedMaterial.SetFloat("_SrcBlend", (float)blendMode);
		sharedMaterial.SetFloat("_DstBlend", (float)blendMode2);
		sharedMaterial.SetFloat("_SrcBlendAlpha", (float)blendMode3);
		sharedMaterial.SetFloat("_DstBlendAlpha", (float)blendMode4);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0007CD98 File Offset: 0x0007AF98
	private void SetFrontToOpaque()
	{
		bool flag = false;
		bool flag2 = true;
		string val = "Opaque";
		int renderQueue = 2000;
		BlendMode blendMode = BlendMode.One;
		BlendMode blendMode2 = BlendMode.Zero;
		BlendMode blendMode3 = BlendMode.One;
		BlendMode blendMode4 = BlendMode.Zero;
		Material sharedMaterial = this.skyFront.sharedMaterial;
		sharedMaterial.SetFloat("_ZWrite", flag2 ? 1f : 0f);
		sharedMaterial.SetShaderPassEnabled("DepthOnly", flag2);
		sharedMaterial.SetFloat("_AlphaToMask", flag ? 1f : 0f);
		sharedMaterial.SetOverrideTag("RenderType", val);
		sharedMaterial.renderQueue = renderQueue;
		sharedMaterial.SetFloat("_SrcBlend", (float)blendMode);
		sharedMaterial.SetFloat("_DstBlend", (float)blendMode2);
		sharedMaterial.SetFloat("_SrcBlendAlpha", (float)blendMode3);
		sharedMaterial.SetFloat("_DstBlendAlpha", (float)blendMode4);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0007CE58 File Offset: 0x0007B058
	private void SetBackToOpaque()
	{
		bool flag = false;
		bool flag2 = true;
		string val = "Opaque";
		int renderQueue = 2000;
		BlendMode blendMode = BlendMode.One;
		BlendMode blendMode2 = BlendMode.Zero;
		BlendMode blendMode3 = BlendMode.One;
		BlendMode blendMode4 = BlendMode.Zero;
		Material sharedMaterial = this.skyBack.sharedMaterial;
		sharedMaterial.SetFloat("_ZWrite", flag2 ? 1f : 0f);
		sharedMaterial.SetShaderPassEnabled("DepthOnly", flag2);
		sharedMaterial.SetFloat("_AlphaToMask", flag ? 1f : 0f);
		sharedMaterial.SetOverrideTag("RenderType", val);
		sharedMaterial.renderQueue = renderQueue;
		sharedMaterial.SetFloat("_SrcBlend", (float)blendMode);
		sharedMaterial.SetFloat("_DstBlend", (float)blendMode2);
		sharedMaterial.SetFloat("_SrcBlendAlpha", (float)blendMode3);
		sharedMaterial.SetFloat("_DstBlendAlpha", (float)blendMode4);
	}

	// Token: 0x04000505 RID: 1285
	public MeshRenderer skyFront;

	// Token: 0x04000506 RID: 1286
	public MeshRenderer skyBack;

	// Token: 0x04000507 RID: 1287
	public Material[] skyMaterials = new Material[0];

	// Token: 0x04000508 RID: 1288
	[Range(0f, 1f)]
	public float lerpValue;

	// Token: 0x04000509 RID: 1289
	[NonSerialized]
	private Material _currentSky;

	// Token: 0x0400050A RID: 1290
	[NonSerialized]
	private Material _nextSky;

	// Token: 0x0400050B RID: 1291
	private TimeSince lastUpdate = TimeSince.Now();

	// Token: 0x0400050C RID: 1292
	[Space]
	private BetterDayNightManager _dayNightManager;

	// Token: 0x0400050D RID: 1293
	private double _currentSeconds = -1.0;

	// Token: 0x0400050E RID: 1294
	private double _totalSecondsInRange = -1.0;

	// Token: 0x0400050F RID: 1295
	private float _currentTime = -1f;

	// Token: 0x04000510 RID: 1296
	private static ShaderHashId _SkyAlpha = "_SkyAlpha";
}
