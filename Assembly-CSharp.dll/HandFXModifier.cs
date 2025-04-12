using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class HandFXModifier : FXModifier
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x000366ED File Offset: 0x000348ED
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00036700 File Offset: 0x00034900
	private void OnDisable()
	{
		base.transform.localScale = this.originalScale;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00036713 File Offset: 0x00034913
	public override void UpdateScale(float scale)
	{
		scale = Mathf.Clamp(scale, this.minScale, this.maxScale);
		base.transform.localScale = this.originalScale * scale;
	}

	// Token: 0x04000CD4 RID: 3284
	private Vector3 originalScale;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private float minScale;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private float maxScale;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private ParticleSystem dustBurst;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	private ParticleSystem dustLinger;
}
