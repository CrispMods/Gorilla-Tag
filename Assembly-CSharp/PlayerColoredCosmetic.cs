using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class PlayerColoredCosmetic : MonoBehaviour
{
	// Token: 0x06001ADD RID: 6877 RVA: 0x00084454 File Offset: 0x00082654
	public void Awake()
	{
		for (int i = 0; i < this.coloringRules.Length; i++)
		{
			this.coloringRules[i].Init();
		}
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x00084488 File Offset: 0x00082688
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

	// Token: 0x06001ADF RID: 6879 RVA: 0x00084511 File Offset: 0x00082711
	private void OnDisable()
	{
		if (this.rig != null)
		{
			this.rig.OnColorChanged -= this.UpdateColor;
		}
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x00084538 File Offset: 0x00082738
	private void UpdateColor(Color color)
	{
		foreach (PlayerColoredCosmetic.ColoringRule coloringRule in this.coloringRules)
		{
			coloringRule.Apply(color);
		}
	}

	// Token: 0x04001DBA RID: 7610
	private bool didInit;

	// Token: 0x04001DBB RID: 7611
	private VRRig rig;

	// Token: 0x04001DBC RID: 7612
	[SerializeField]
	private PlayerColoredCosmetic.ColoringRule[] coloringRules;

	// Token: 0x02000443 RID: 1091
	[Serializable]
	private struct ColoringRule
	{
		// Token: 0x06001AE2 RID: 6882 RVA: 0x0008456C File Offset: 0x0008276C
		public void Init()
		{
			this.hashId = new ShaderHashId(this.shaderColorProperty);
			Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
			this.instancedMaterial = new Material(sharedMaterials[this.materialIndex]);
			sharedMaterials[this.materialIndex] = this.instancedMaterial;
			this.meshRenderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000845C3 File Offset: 0x000827C3
		public void Apply(Color color)
		{
			this.instancedMaterial.SetColor(this.hashId, color);
		}

		// Token: 0x04001DBD RID: 7613
		[SerializeField]
		private string shaderColorProperty;

		// Token: 0x04001DBE RID: 7614
		private ShaderHashId hashId;

		// Token: 0x04001DBF RID: 7615
		[SerializeField]
		private Renderer meshRenderer;

		// Token: 0x04001DC0 RID: 7616
		[SerializeField]
		private int materialIndex;

		// Token: 0x04001DC1 RID: 7617
		private Material instancedMaterial;
	}
}
