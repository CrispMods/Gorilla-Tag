using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008E7 RID: 2279
[Serializable]
public class VoiceLoudnessReactorRendererColorTarget
{
	// Token: 0x060036CB RID: 14027 RVA: 0x001437C4 File Offset: 0x001419C4
	public void Inititialize()
	{
		if (this._materials == null)
		{
			this._materials = new List<Material>(this.renderer.materials);
			this._materials[this.materialIndex].EnableKeyword(this.colorProperty);
			this.renderer.SetMaterials(this._materials);
			this.UpdateMaterialColor(0f);
		}
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00143828 File Offset: 0x00141A28
	public void UpdateMaterialColor(float level)
	{
		Color color = this.gradient.Evaluate(level);
		if (this._lastColor == color)
		{
			return;
		}
		this._materials[this.materialIndex].SetColor(this.colorProperty, color);
		this._lastColor = color;
	}

	// Token: 0x040039BB RID: 14779
	[SerializeField]
	private string colorProperty = "_BaseColor";

	// Token: 0x040039BC RID: 14780
	public Renderer renderer;

	// Token: 0x040039BD RID: 14781
	public int materialIndex;

	// Token: 0x040039BE RID: 14782
	public Gradient gradient;

	// Token: 0x040039BF RID: 14783
	public bool useSmoothedLoudness;

	// Token: 0x040039C0 RID: 14784
	public float scale = 1f;

	// Token: 0x040039C1 RID: 14785
	private List<Material> _materials;

	// Token: 0x040039C2 RID: 14786
	private Color _lastColor = Color.white;
}
