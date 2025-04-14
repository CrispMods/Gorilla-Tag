using System;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class XRaySkeleton : SyncToPlayerColor
{
	// Token: 0x06001B41 RID: 6977 RVA: 0x00086318 File Offset: 0x00084518
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

	// Token: 0x06001B42 RID: 6978 RVA: 0x00086389 File Offset: 0x00084589
	public void SetMaterialIndex(int index)
	{
		this.renderer.sharedMaterial = this.tagMaterials[index];
		this._lastMatIndex = index;
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000863A5 File Offset: 0x000845A5
	private void Setup()
	{
		this.colorPropertiesToSync = new ShaderHashId[]
		{
			XRaySkeleton._BaseColor,
			XRaySkeleton._EmissionColor
		};
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x000863CC File Offset: 0x000845CC
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

	// Token: 0x04001E30 RID: 7728
	public SkinnedMeshRenderer renderer;

	// Token: 0x04001E31 RID: 7729
	public Vector2 baseValueMinMax = new Vector2(0.69f, 1f);

	// Token: 0x04001E32 RID: 7730
	public Material[] tagMaterials = new Material[0];

	// Token: 0x04001E33 RID: 7731
	private int _lastMatIndex;

	// Token: 0x04001E34 RID: 7732
	private static readonly ShaderHashId _BaseColor = "_BaseColor";

	// Token: 0x04001E35 RID: 7733
	private static readonly ShaderHashId _EmissionColor = "_EmissionColor";
}
