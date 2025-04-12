using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
[ExecuteAlways]
public class WaterSurfaceMaterialController : MonoBehaviour
{
	// Token: 0x060003D6 RID: 982 RVA: 0x00031F9B File Offset: 0x0003019B
	protected void OnEnable()
	{
		this.renderer = base.GetComponent<Renderer>();
		this.matPropBlock = new MaterialPropertyBlock();
		this.ApplyProperties();
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x00078580 File Offset: 0x00076780
	private void ApplyProperties()
	{
		this.matPropBlock.SetVector(WaterSurfaceMaterialController.shaderProp_ScrollSpeedAndScale, new Vector4(this.ScrollX, this.ScrollY, this.Scale, 0f));
		if (this.renderer)
		{
			this.renderer.SetPropertyBlock(this.matPropBlock);
		}
	}

	// Token: 0x04000456 RID: 1110
	public float ScrollX = 0.6f;

	// Token: 0x04000457 RID: 1111
	public float ScrollY = 0.6f;

	// Token: 0x04000458 RID: 1112
	public float Scale = 1f;

	// Token: 0x04000459 RID: 1113
	private Renderer renderer;

	// Token: 0x0400045A RID: 1114
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x0400045B RID: 1115
	private static readonly int shaderProp_ScrollSpeedAndScale = Shader.PropertyToID("_ScrollSpeedAndScale");
}
