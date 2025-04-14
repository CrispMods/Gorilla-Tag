using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class HandFXModifier : FXModifier
{
	// Token: 0x06000A86 RID: 2694 RVA: 0x000394F7 File Offset: 0x000376F7
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0003950A File Offset: 0x0003770A
	private void OnDisable()
	{
		base.transform.localScale = this.originalScale;
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0003951D File Offset: 0x0003771D
	public override void UpdateScale(float scale)
	{
		scale = Mathf.Clamp(scale, this.minScale, this.maxScale);
		base.transform.localScale = this.originalScale * scale;
	}

	// Token: 0x04000CD3 RID: 3283
	private Vector3 originalScale;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	private float minScale;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private float maxScale;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private ParticleSystem dustBurst;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private ParticleSystem dustLinger;
}
