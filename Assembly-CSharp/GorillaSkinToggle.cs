using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class GorillaSkinToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x0600093F RID: 2367 RVA: 0x00036826 File Offset: 0x00034A26
	public bool applied
	{
		get
		{
			return this._applied;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000940 RID: 2368 RVA: 0x0003682E File Offset: 0x00034A2E
	// (set) Token: 0x06000941 RID: 2369 RVA: 0x00036836 File Offset: 0x00034A36
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000942 RID: 2370 RVA: 0x0003683F File Offset: 0x00034A3F
	// (set) Token: 0x06000943 RID: 2371 RVA: 0x00036847 File Offset: 0x00034A47
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06000944 RID: 2372 RVA: 0x000911EC File Offset: 0x0008F3EC
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this._rig = base.GetComponentInParent<VRRig>(true);
		if (this.coloringRules.Length != 0)
		{
			this._activeSkin = GorillaSkin.CopyWithInstancedMaterials(this._skin);
			for (int i = 0; i < this.coloringRules.Length; i++)
			{
				this.coloringRules[i].Init();
			}
			return;
		}
		this._activeSkin = this._skin;
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x00091254 File Offset: 0x0008F454
	private void OnPlayerColorChanged(Color playerColor)
	{
		foreach (GorillaSkinToggle.ColoringRule coloringRule in this.coloringRules)
		{
			coloringRule.Apply(this._activeSkin, playerColor);
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00036850 File Offset: 0x00034A50
	private void OnEnable()
	{
		if (this.coloringRules.Length != 0)
		{
			this._rig.OnColorChanged += this.OnPlayerColorChanged;
			this.OnPlayerColorChanged(this._rig.playerColor);
		}
		this.Apply();
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00036889 File Offset: 0x00034A89
	private void OnDisable()
	{
		if (ApplicationQuittingState.IsQuitting)
		{
			return;
		}
		this.Remove();
		if (this.coloringRules.Length != 0)
		{
			this._rig.OnColorChanged -= this.OnPlayerColorChanged;
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x000368B9 File Offset: 0x00034AB9
	public void Apply()
	{
		GorillaSkin.ApplyToRig(this._rig, this._activeSkin, GorillaSkin.SkinType.cosmetic);
		this._applied = true;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x000368D4 File Offset: 0x00034AD4
	public void ApplyToMannequin(GameObject mannequin)
	{
		if (this._skin.IsNull())
		{
			Debug.LogError("No skin set on GorillaSkinToggle");
			return;
		}
		if (mannequin.IsNull())
		{
			Debug.LogError("No mannequin set on GorillaSkinToggle");
			return;
		}
		this._skin.ApplySkinToMannequin(mannequin);
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0009128C File Offset: 0x0008F48C
	public void Remove()
	{
		GorillaSkin.ApplyToRig(this._rig, null, GorillaSkin.SkinType.cosmetic);
		float @float = PlayerPrefs.GetFloat("redValue", 0f);
		float float2 = PlayerPrefs.GetFloat("greenValue", 0f);
		float float3 = PlayerPrefs.GetFloat("blueValue", 0f);
		GorillaTagger.Instance.UpdateColor(@float, float2, float3);
		this._applied = false;
	}

	// Token: 0x04000B23 RID: 2851
	private VRRig _rig;

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	private GorillaSkin _skin;

	// Token: 0x04000B25 RID: 2853
	private GorillaSkin _activeSkin;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	private GorillaSkinToggle.ColoringRule[] coloringRules;

	// Token: 0x04000B27 RID: 2855
	[Space]
	[SerializeField]
	private bool _applied;

	// Token: 0x02000172 RID: 370
	[Serializable]
	private struct ColoringRule
	{
		// Token: 0x0600094D RID: 2381 RVA: 0x0003690D File Offset: 0x00034B0D
		public void Init()
		{
			if (this.shaderColorProperty == "")
			{
				this.shaderColorProperty = "_BaseColor";
			}
			this.shaderHashId = new ShaderHashId(this.shaderColorProperty);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000912EC File Offset: 0x0008F4EC
		public void Apply(GorillaSkin skin, Color color)
		{
			if (this.colorMaterials.HasFlag(GorillaSkinMaterials.Body))
			{
				skin.bodyMaterial.SetColor(this.shaderHashId, color);
			}
			if (this.colorMaterials.HasFlag(GorillaSkinMaterials.Face))
			{
				skin.faceMaterial.SetColor(this.shaderHashId, color);
			}
			if (this.colorMaterials.HasFlag(GorillaSkinMaterials.Chest))
			{
				skin.chestMaterial.SetColor(this.shaderHashId, color);
			}
			if (this.colorMaterials.HasFlag(GorillaSkinMaterials.Scoreboard))
			{
				skin.scoreboardMaterial.SetColor(this.shaderHashId, color);
			}
		}

		// Token: 0x04000B2A RID: 2858
		public GorillaSkinMaterials colorMaterials;

		// Token: 0x04000B2B RID: 2859
		public string shaderColorProperty;

		// Token: 0x04000B2C RID: 2860
		private ShaderHashId shaderHashId;
	}
}
