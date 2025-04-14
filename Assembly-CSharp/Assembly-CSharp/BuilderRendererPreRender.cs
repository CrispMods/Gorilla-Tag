using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class BuilderRendererPreRender : MonoBehaviour
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0008E145 File Offset: 0x0008C345
	private void LateUpdate()
	{
		if (this.builderRenderer != null)
		{
			this.builderRenderer.PreRenderIndirect();
		}
	}

	// Token: 0x0400201F RID: 8223
	public BuilderRenderer builderRenderer;
}
