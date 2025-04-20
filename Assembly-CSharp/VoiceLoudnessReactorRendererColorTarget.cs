using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000900 RID: 2304
[Serializable]
public class VoiceLoudnessReactorRendererColorTarget
{
	// Token: 0x06003787 RID: 14215 RVA: 0x00148D84 File Offset: 0x00146F84
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

	// Token: 0x06003788 RID: 14216 RVA: 0x00148DE8 File Offset: 0x00146FE8
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

	// Token: 0x04003A6A RID: 14954
	[SerializeField]
	private string colorProperty = "_BaseColor";

	// Token: 0x04003A6B RID: 14955
	public Renderer renderer;

	// Token: 0x04003A6C RID: 14956
	public int materialIndex;

	// Token: 0x04003A6D RID: 14957
	public Gradient gradient;

	// Token: 0x04003A6E RID: 14958
	public bool useSmoothedLoudness;

	// Token: 0x04003A6F RID: 14959
	public float scale = 1f;

	// Token: 0x04003A70 RID: 14960
	private List<Material> _materials;

	// Token: 0x04003A71 RID: 14961
	private Color _lastColor = Color.white;
}
