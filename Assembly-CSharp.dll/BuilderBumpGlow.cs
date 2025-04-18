﻿using System;
using UnityEngine;

// Token: 0x020004B9 RID: 1209
public class BuilderBumpGlow : MonoBehaviour
{
	// Token: 0x06001D3D RID: 7485 RVA: 0x0004300D File Offset: 0x0004120D
	public void Awake()
	{
		this.blendIn = 1f;
		this.intensity = 0f;
		this.UpdateRender();
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0004302B File Offset: 0x0004122B
	public void SetIntensity(float intensity)
	{
		this.intensity = intensity;
		this.UpdateRender();
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0004303A File Offset: 0x0004123A
	public void SetBlendIn(float blendIn)
	{
		this.blendIn = blendIn;
		this.UpdateRender();
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void UpdateRender()
	{
	}

	// Token: 0x0400204F RID: 8271
	public MeshRenderer glowRenderer;

	// Token: 0x04002050 RID: 8272
	private float blendIn;

	// Token: 0x04002051 RID: 8273
	private float intensity;
}
