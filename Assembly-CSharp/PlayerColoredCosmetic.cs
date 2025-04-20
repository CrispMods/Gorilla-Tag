using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class PlayerColoredCosmetic : MonoBehaviour
{
	// Token: 0x06001B31 RID: 6961 RVA: 0x000D9188 File Offset: 0x000D7388
	public void Awake()
	{
		for (int i = 0; i < this.coloringRules.Length; i++)
		{
			this.coloringRules[i].Init();
		}
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000D91BC File Offset: 0x000D73BC
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

	// Token: 0x06001B33 RID: 6963 RVA: 0x000427A2 File Offset: 0x000409A2
	private void OnDisable()
	{
		if (this.rig != null)
		{
			this.rig.OnColorChanged -= this.UpdateColor;
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000D9248 File Offset: 0x000D7448
	private void UpdateColor(Color color)
	{
		foreach (PlayerColoredCosmetic.ColoringRule coloringRule in this.coloringRules)
		{
			coloringRule.Apply(color);
		}
	}

	// Token: 0x04001E09 RID: 7689
	private bool didInit;

	// Token: 0x04001E0A RID: 7690
	private VRRig rig;

	// Token: 0x04001E0B RID: 7691
	[SerializeField]
	private PlayerColoredCosmetic.ColoringRule[] coloringRules;

	// Token: 0x0200044F RID: 1103
	[Serializable]
	private struct ColoringRule
	{
		// Token: 0x06001B36 RID: 6966 RVA: 0x000D927C File Offset: 0x000D747C
		public void Init()
		{
			this.hashId = new ShaderHashId(this.shaderColorProperty);
			Material[] sharedMaterials = this.meshRenderer.sharedMaterials;
			this.instancedMaterial = new Material(sharedMaterials[this.materialIndex]);
			sharedMaterials[this.materialIndex] = this.instancedMaterial;
			this.meshRenderer.sharedMaterials = sharedMaterials;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000427C9 File Offset: 0x000409C9
		public void Apply(Color color)
		{
			this.instancedMaterial.SetColor(this.hashId, color);
		}

		// Token: 0x04001E0C RID: 7692
		[SerializeField]
		private string shaderColorProperty;

		// Token: 0x04001E0D RID: 7693
		private ShaderHashId hashId;

		// Token: 0x04001E0E RID: 7694
		[SerializeField]
		private Renderer meshRenderer;

		// Token: 0x04001E0F RID: 7695
		[SerializeField]
		private int materialIndex;

		// Token: 0x04001E10 RID: 7696
		private Material instancedMaterial;
	}
}
