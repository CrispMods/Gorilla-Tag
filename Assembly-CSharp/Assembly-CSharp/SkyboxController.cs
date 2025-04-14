using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200009F RID: 159
public class SkyboxController : MonoBehaviour
{
	// Token: 0x06000428 RID: 1064 RVA: 0x000192B8 File Offset: 0x000174B8
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

	// Token: 0x06000429 RID: 1065 RVA: 0x00019346 File Offset: 0x00017546
	private void Update()
	{
		if (!this.lastUpdate.HasElapsed(1f, true))
		{
			return;
		}
		this.UpdateTime();
		this.UpdateSky();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00019368 File Offset: 0x00017568
	private void OnValidate()
	{
		this.UpdateSky();
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00019370 File Offset: 0x00017570
	private void UpdateTime()
	{
		this._currentSeconds = ((ITimeOfDaySystem)this._dayNightManager).currentTimeInSeconds;
		this._currentSeconds = Math.Floor(this._currentSeconds);
		this._currentTime = (float)(this._currentSeconds / this._totalSecondsInRange);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000193A8 File Offset: 0x000175A8
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

	// Token: 0x0600042D RID: 1069 RVA: 0x0001948C File Offset: 0x0001768C
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

	// Token: 0x0600042E RID: 1070 RVA: 0x0001954C File Offset: 0x0001774C
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

	// Token: 0x0600042F RID: 1071 RVA: 0x0001960C File Offset: 0x0001780C
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

	// Token: 0x040004C6 RID: 1222
	public MeshRenderer skyFront;

	// Token: 0x040004C7 RID: 1223
	public MeshRenderer skyBack;

	// Token: 0x040004C8 RID: 1224
	public Material[] skyMaterials = new Material[0];

	// Token: 0x040004C9 RID: 1225
	[Range(0f, 1f)]
	public float lerpValue;

	// Token: 0x040004CA RID: 1226
	[NonSerialized]
	private Material _currentSky;

	// Token: 0x040004CB RID: 1227
	[NonSerialized]
	private Material _nextSky;

	// Token: 0x040004CC RID: 1228
	private TimeSince lastUpdate = TimeSince.Now();

	// Token: 0x040004CD RID: 1229
	[Space]
	private BetterDayNightManager _dayNightManager;

	// Token: 0x040004CE RID: 1230
	private double _currentSeconds = -1.0;

	// Token: 0x040004CF RID: 1231
	private double _totalSecondsInRange = -1.0;

	// Token: 0x040004D0 RID: 1232
	private float _currentTime = -1f;

	// Token: 0x040004D1 RID: 1233
	private static ShaderHashId _SkyAlpha = "_SkyAlpha";
}
