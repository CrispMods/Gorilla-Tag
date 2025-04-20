using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
[ExecuteAlways]
public class WaterSurfaceMaterialController : MonoBehaviour
{
	// Token: 0x06000410 RID: 1040 RVA: 0x000331A2 File Offset: 0x000313A2
	protected void OnEnable()
	{
		this.renderer = base.GetComponent<Renderer>();
		this.matPropBlock = new MaterialPropertyBlock();
		this.ApplyProperties();
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0007ADDC File Offset: 0x00078FDC
	private void ApplyProperties()
	{
		this.matPropBlock.SetVector(WaterSurfaceMaterialController.shaderProp_ScrollSpeedAndScale, new Vector4(this.ScrollX, this.ScrollY, this.Scale, 0f));
		if (this.renderer)
		{
			this.renderer.SetPropertyBlock(this.matPropBlock);
		}
	}

	// Token: 0x04000495 RID: 1173
	public float ScrollX = 0.6f;

	// Token: 0x04000496 RID: 1174
	public float ScrollY = 0.6f;

	// Token: 0x04000497 RID: 1175
	public float Scale = 1f;

	// Token: 0x04000498 RID: 1176
	private Renderer renderer;

	// Token: 0x04000499 RID: 1177
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x0400049A RID: 1178
	private static readonly int shaderProp_ScrollSpeedAndScale = Shader.PropertyToID("_ScrollSpeedAndScale");
}
