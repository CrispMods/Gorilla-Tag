using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
[Serializable]
public class VoiceLoudnessReactorRendererColorTarget
{
	// Token: 0x060036BF RID: 14015 RVA: 0x00103B0C File Offset: 0x00101D0C
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

	// Token: 0x060036C0 RID: 14016 RVA: 0x00103B70 File Offset: 0x00101D70
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

	// Token: 0x040039A9 RID: 14761
	[SerializeField]
	private string colorProperty = "_BaseColor";

	// Token: 0x040039AA RID: 14762
	public Renderer renderer;

	// Token: 0x040039AB RID: 14763
	public int materialIndex;

	// Token: 0x040039AC RID: 14764
	public Gradient gradient;

	// Token: 0x040039AD RID: 14765
	public bool useSmoothedLoudness;

	// Token: 0x040039AE RID: 14766
	public float scale = 1f;

	// Token: 0x040039AF RID: 14767
	private List<Material> _materials;

	// Token: 0x040039B0 RID: 14768
	private Color _lastColor = Color.white;
}
