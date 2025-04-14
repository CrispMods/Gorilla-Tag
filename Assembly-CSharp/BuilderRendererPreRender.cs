using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class BuilderRendererPreRender : MonoBehaviour
{
	// Token: 0x06001D1C RID: 7452 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Awake()
	{
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0008DDC1 File Offset: 0x0008BFC1
	private void LateUpdate()
	{
		if (this.builderRenderer != null)
		{
			this.builderRenderer.PreRenderIndirect();
		}
	}

	// Token: 0x0400201E RID: 8222
	public BuilderRenderer builderRenderer;
}
