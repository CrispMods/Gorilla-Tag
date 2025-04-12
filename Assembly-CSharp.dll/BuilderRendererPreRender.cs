using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class BuilderRendererPreRender : MonoBehaviour
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Awake()
	{
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x00042EE0 File Offset: 0x000410E0
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
