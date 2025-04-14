using System;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class BuilderBumpGlow : MonoBehaviour
{
	// Token: 0x06001D3A RID: 7482 RVA: 0x0008E6A5 File Offset: 0x0008C8A5
	public void Awake()
	{
		this.blendIn = 1f;
		this.intensity = 0f;
		this.UpdateRender();
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x0008E6C3 File Offset: 0x0008C8C3
	public void SetIntensity(float intensity)
	{
		this.intensity = intensity;
		this.UpdateRender();
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x0008E6D2 File Offset: 0x0008C8D2
	public void SetBlendIn(float blendIn)
	{
		this.blendIn = blendIn;
		this.UpdateRender();
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x000023F4 File Offset: 0x000005F4
	private void UpdateRender()
	{
	}

	// Token: 0x0400204E RID: 8270
	public MeshRenderer glowRenderer;

	// Token: 0x0400204F RID: 8271
	private float blendIn;

	// Token: 0x04002050 RID: 8272
	private float intensity;
}
