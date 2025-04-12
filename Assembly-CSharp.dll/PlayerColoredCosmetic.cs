using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class PlayerColoredCosmetic : MonoBehaviour
{
	// Token: 0x06001AE0 RID: 6880 RVA: 0x000D64E8 File Offset: 0x000D46E8
	public void Awake()
	{
		for (int i = 0; i < this.coloringRules.Length; i++)
		{
			this.coloringRules[i].Init();
		}
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000D651C File Offset: 0x000D471C
	private void OnEnable()
	{
		if (!this.didInit)
		{
			this.didInit = true;
			this.rig = base.GetComponentInParent<VRRig>();
			if (this.rig == null && GorillaTagger.Instance != null)
			{
				this.rig = GorillaTagger.Instance.offlineVRRig;
			}
		}
		if (this.rig != null)
		{
			this.rig.OnColorChanged += this.UpdateColor;
			this.UpdateColor(this.rig.playerColor);
		}
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x00041469 File Offset: 0x0003F669
	private void OnDisable()
	{
		if (this.rig != null)
		{
			this.rig.OnColorChanged -= this.UpdateColor;
		}
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000D65A8 File Offset: 0x000D47A8
	private void UpdateColor(Color color)
	{
		foreach (PlayerColoredCosmetic.ColoringRule coloringRule in this.coloringRules)
		{
			coloringRule.Apply(color);
		}
	}

	// Token: 0x04001DBB RID: 7611
	private bool didInit;

	// Token: 0x04001DBC RID: 7612
	private VRRig rig;

	// Token: 0x04001DBD RID: 7613
	[SerializeField]
	private PlayerColoredCosmetic.ColoringRule[] coloringRules;

	// Token: 0x02000443 RID: 1091
	[Serializable]
	private struct ColoringRule
	{
		// Token: 0x06001AE5 RID: 6885 RVA: 0x000D65DC File Offset: 0x000D47DC
		public void Init()
		{
			this.hashId = new ShaderHashId(this.shaderColorProperty);
			Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
			this.instancedMaterial = new Material(sharedMaterials[this.materialIndex]);
			sharedMaterials[this.materialIndex] = this.instancedMaterial;
			this.meshRenderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x00041490 File Offset: 0x0003F690
		public void Apply(Color color)
		{
			this.instancedMaterial.SetColor(this.hashId, color);
		}

		// Token: 0x04001DBE RID: 7614
		[SerializeField]
		private string shaderColorProperty;

		// Token: 0x04001DBF RID: 7615
		private ShaderHashId hashId;

		// Token: 0x04001DC0 RID: 7616
		[SerializeField]
		private Renderer meshRenderer;

		// Token: 0x04001DC1 RID: 7617
		[SerializeField]
		private int materialIndex;

		// Token: 0x04001DC2 RID: 7618
		private Material instancedMaterial;
	}
}
