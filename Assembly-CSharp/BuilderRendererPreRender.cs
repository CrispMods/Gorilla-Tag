using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class BuilderRendererPreRender : MonoBehaviour
{
	// Token: 0x06001D75 RID: 7541 RVA: 0x00030607 File Offset: 0x0002E807
	private void Awake()
	{
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x0004427F File Offset: 0x0004247F
	private void LateUpdate()
	{
		if (this.builderRenderer != null)
		{
			this.builderRenderer.PreRenderIndirect();
		}
	}

	// Token: 0x04002071 RID: 8305
	public BuilderRenderer builderRenderer;
}
