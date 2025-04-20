using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class CloudUmbrellaCloud : MonoBehaviour
{
	// Token: 0x06000596 RID: 1430 RVA: 0x0003411A File Offset: 0x0003231A
	protected void Awake()
	{
		this.umbrellaXform = this.umbrella.transform;
		this.cloudScaleXform = this.cloudRenderer.transform;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x00082B7C File Offset: 0x00080D7C
	protected void LateUpdate()
	{
		float time = Vector3.Dot(this.umbrellaXform.up, Vector3.up);
		float num = Mathf.Clamp01(this.scaleCurve.Evaluate(time));
		this.rendererOn = ((num > 0.09f && num < 0.1f) ? this.rendererOn : (num > 0.1f));
		this.cloudRenderer.enabled = this.rendererOn;
		this.cloudScaleXform.localScale = new Vector3(num, num, num);
		this.cloudRotateXform.up = Vector3.up;
	}

	// Token: 0x04000664 RID: 1636
	public UmbrellaItem umbrella;

	// Token: 0x04000665 RID: 1637
	public Transform cloudRotateXform;

	// Token: 0x04000666 RID: 1638
	public Renderer cloudRenderer;

	// Token: 0x04000667 RID: 1639
	public AnimationCurve scaleCurve;

	// Token: 0x04000668 RID: 1640
	private const float kHideAtScale = 0.1f;

	// Token: 0x04000669 RID: 1641
	private const float kHideAtScaleTolerance = 0.01f;

	// Token: 0x0400066A RID: 1642
	private bool rendererOn;

	// Token: 0x0400066B RID: 1643
	private Transform umbrellaXform;

	// Token: 0x0400066C RID: 1644
	private Transform cloudScaleXform;
}
