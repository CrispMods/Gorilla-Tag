using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class BuilderBumpGlow : MonoBehaviour
{
	// Token: 0x06001D93 RID: 7571 RVA: 0x000443AC File Offset: 0x000425AC
	public void Awake()
	{
		this.blendIn = 1f;
		this.intensity = 0f;
		this.UpdateRender();
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x000443CA File Offset: 0x000425CA
	public void SetIntensity(float intensity)
	{
		this.intensity = intensity;
		this.UpdateRender();
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x000443D9 File Offset: 0x000425D9
	public void SetBlendIn(float blendIn)
	{
		this.blendIn = blendIn;
		this.UpdateRender();
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x00030607 File Offset: 0x0002E807
	private void UpdateRender()
	{
	}

	// Token: 0x040020A1 RID: 8353
	public MeshRenderer glowRenderer;

	// Token: 0x040020A2 RID: 8354
	private float blendIn;

	// Token: 0x040020A3 RID: 8355
	private float intensity;
}
