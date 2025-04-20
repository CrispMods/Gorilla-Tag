using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class XRaySkeleton : SyncToPlayerColor
{
	// Token: 0x06001B95 RID: 7061 RVA: 0x000DA9E0 File Offset: 0x000D8BE0
	protected override void Awake()
	{
		base.Awake();
		this.target = this.renderer.material;
		Material[] materialsToChangeTo = this.rig.materialsToChangeTo;
		this.tagMaterials = new Material[materialsToChangeTo.Length];
		this.tagMaterials[0] = new Material(this.target);
		for (int i = 1; i < materialsToChangeTo.Length; i++)
		{
			Material material = new Material(materialsToChangeTo[i]);
			this.tagMaterials[i] = material;
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00042E03 File Offset: 0x00041003
	public void SetMaterialIndex(int index)
	{
		this.renderer.sharedMaterial = this.tagMaterials[index];
		this._lastMatIndex = index;
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x00042E1F File Offset: 0x0004101F
	private void Setup()
	{
		this.colorPropertiesToSync = new ShaderHashId[]
		{
			XRaySkeleton._BaseColor,
			XRaySkeleton._EmissionColor
		};
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000DAA54 File Offset: 0x000D8C54
	public override void UpdateColor(Color color)
	{
		if (this._lastMatIndex != 0)
		{
			return;
		}
		Material material = this.tagMaterials[0];
		float h;
		float s;
		float value;
		Color.RGBToHSV(color, out h, out s, out value);
		Color value2 = Color.HSVToRGB(h, s, Mathf.Clamp(value, this.baseValueMinMax.x, this.baseValueMinMax.y));
		material.SetColor(XRaySkeleton._BaseColor, value2);
		float h2;
		float num;
		float num2;
		Color.RGBToHSV(color, out h2, out num, out num2);
		Color color2 = Color.HSVToRGB(h2, 0.82f, 0.9f, true);
		color2 = new Color(color2.r * 1.4f, color2.g * 1.4f, color2.b * 1.4f);
		material.SetColor(XRaySkeleton._EmissionColor, ColorUtils.ComposeHDR(new Color32(36, 191, 136, byte.MaxValue), 2f));
		this.renderer.sharedMaterial = material;
	}

	// Token: 0x04001E7F RID: 7807
	public SkinnedMeshRenderer renderer;

	// Token: 0x04001E80 RID: 7808
	public Vector2 baseValueMinMax = new Vector2(0.69f, 1f);

	// Token: 0x04001E81 RID: 7809
	public Material[] tagMaterials = new Material[0];

	// Token: 0x04001E82 RID: 7810
	private int _lastMatIndex;

	// Token: 0x04001E83 RID: 7811
	private static readonly ShaderHashId _BaseColor = "_BaseColor";

	// Token: 0x04001E84 RID: 7812
	private static readonly ShaderHashId _EmissionColor = "_EmissionColor";
}
