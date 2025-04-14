using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class HandFXModifier : FXModifier
{
	// Token: 0x06000A88 RID: 2696 RVA: 0x0003981B File Offset: 0x00037A1B
	private void Awake()
	{
		this.originalScale = base.transform.localScale;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0003982E File Offset: 0x00037A2E
	private void OnDisable()
	{
		base.transform.localScale = this.originalScale;
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x00039841 File Offset: 0x00037A41
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
