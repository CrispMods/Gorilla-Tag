using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
[ExecuteAlways]
public class WaterSurfaceMaterialController : MonoBehaviour
{
	// Token: 0x060003D4 RID: 980 RVA: 0x00016EE9 File Offset: 0x000150E9
	protected void OnEnable()
	{
		this.renderer = base.GetComponent<Renderer>();
		this.matPropBlock = new MaterialPropertyBlock();
		this.ApplyProperties();
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00016F08 File Offset: 0x00015108
	private void ApplyProperties()
	{
		this.matPropBlock.SetVector(WaterSurfaceMaterialController.shaderProp_ScrollSpeedAndScale, new Vector4(this.ScrollX, this.ScrollY, this.Scale, 0f));
		if (this.renderer)
		{
			this.renderer.SetPropertyBlock(this.matPropBlock);
		}
	}

	// Token: 0x04000455 RID: 1109
	public float ScrollX = 0.6f;

	// Token: 0x04000456 RID: 1110
	public float ScrollY = 0.6f;

	// Token: 0x04000457 RID: 1111
	public float Scale = 1f;

	// Token: 0x04000458 RID: 1112
	private Renderer renderer;

	// Token: 0x04000459 RID: 1113
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x0400045A RID: 1114
	private static readonly int shaderProp_ScrollSpeedAndScale = Shader.PropertyToID("_ScrollSpeedAndScale");
}
