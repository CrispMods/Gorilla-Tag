using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class HandFXModifier : FXModifier
{
	// Token: 0x06000AD2 RID: 2770 RVA: 0x000379AD File Offset: 0x00035BAD
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x000379C0 File Offset: 0x00035BC0
	private void OnDisable()
	{
		base.transform.localScale = this.originalScale;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x000379D3 File Offset: 0x00035BD3
	public override void UpdateScale(float scale)
	{
		scale = Mathf.Clamp(scale, this.minScale, this.maxScale);
		base.transform.localScale = this.originalScale * scale;
	}

	// Token: 0x04000D19 RID: 3353
	private Vector3 originalScale;

	// Token: 0x04000D1A RID: 3354
	[SerializeField]
	private float minScale;

	// Token: 0x04000D1B RID: 3355
	[SerializeField]
	private float maxScale;

	// Token: 0x04000D1C RID: 3356
	[SerializeField]
	private ParticleSystem dustBurst;

	// Token: 0x04000D1D RID: 3357
	[SerializeField]
	private ParticleSystem dustLinger;
}
