using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class GorillaSkinToggle : MonoBehaviour, ISpawnable
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00030AC5 File Offset: 0x0002ECC5
	public bool applied
	{
		get
		{
			return this._applied;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00030ACD File Offset: 0x0002ECCD
	// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00030AD5 File Offset: 0x0002ECD5
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00030ADE File Offset: 0x0002ECDE
	// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00030AE6 File Offset: 0x0002ECE6
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060008F9 RID: 2297 RVA: 0x00030AF0 File Offset: 0x0002ECF0
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

	// Token: 0x060008FA RID: 2298 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00030B58 File Offset: 0x0002ED58
	private void OnPlayerColorChanged(Color playerColor)
	{
		foreach (GorillaSkinToggle.ColoringRule coloringRule in this.coloringRules)
		{
			coloringRule.Apply(this._activeSkin, playerColor);
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00030B90 File Offset: 0x0002ED90
	private void OnEnable()
	{
		if (this.coloringRules.Length != 0)
		{
			this._rig.OnColorChanged += this.OnPlayerColorChanged;
			this.OnPlayerColorChanged(this._rig.playerColor);
		}
		this.Apply();
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00030BC9 File Offset: 0x0002EDC9
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

	// Token: 0x060008FE RID: 2302 RVA: 0x00030BF9 File Offset: 0x0002EDF9
	public void Apply()
	{
		GorillaSkin.ApplyToRig(this._rig, this._activeSkin, GorillaSkin.SkinType.cosmetic);
		this._applied = true;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00030C14 File Offset: 0x0002EE14
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

	// Token: 0x06000900 RID: 2304 RVA: 0x00030C50 File Offset: 0x0002EE50
	public void Remove()
	{
		GorillaSkin.ApplyToRig(this._rig, null, GorillaSkin.SkinType.cosmetic);
		float @float = PlayerPrefs.GetFloat("redValue", 0f);
		float float2 = PlayerPrefs.GetFloat("greenValue", 0f);
		float float3 = PlayerPrefs.GetFloat("blueValue", 0f);
		GorillaTagger.Instance.UpdateColor(@float, float2, float3);
		this._applied = false;
	}

	// Token: 0x04000ADD RID: 2781
	private VRRig _rig;

	// Token: 0x04000ADE RID: 2782
	[SerializeField]
	private GorillaSkin _skin;

	// Token: 0x04000ADF RID: 2783
	private GorillaSkin _activeSkin;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private GorillaSkinToggle.ColoringRule[] coloringRules;

	// Token: 0x04000AE1 RID: 2785
	[Space]
	[SerializeField]
	private bool _applied;

	// Token: 0x02000167 RID: 359
	[Serializable]
	private struct ColoringRule
	{
		// Token: 0x06000902 RID: 2306 RVA: 0x00030CAE File Offset: 0x0002EEAE
		public void Init()
		{
			if (this.shaderColorProperty == "")
			{
				this.shaderColorProperty = "_BaseColor";
			}
			this.shaderHashId = new ShaderHashId(this.shaderColorProperty);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00030CE0 File Offset: 0x0002EEE0
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

		// Token: 0x04000AE4 RID: 2788
		public GorillaSkinMaterials colorMaterials;

		// Token: 0x04000AE5 RID: 2789
		public string shaderColorProperty;

		// Token: 0x04000AE6 RID: 2790
		private ShaderHashId shaderHashId;
	}
}
