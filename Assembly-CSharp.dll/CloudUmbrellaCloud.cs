﻿using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class CloudUmbrellaCloud : MonoBehaviour
{
	// Token: 0x06000557 RID: 1367 RVA: 0x00032EB6 File Offset: 0x000310B6
	protected void Awake()
	{
		this.umbrellaXform = this.umbrella.transform;
		this.cloudScaleXform = this.cloudRenderer.transform;
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x00080274 File Offset: 0x0007E474
	protected void LateUpdate()
	{
		float time = Vector3.Dot(this.umbrellaXform.up, Vector3.up);
		float num = Mathf.Clamp01(this.scaleCurve.Evaluate(time));
		this.rendererOn = ((num > 0.09f && num < 0.1f) ? this.rendererOn : (num > 0.1f));
		this.cloudRenderer.enabled = this.rendererOn;
		this.cloudScaleXform.localScale = new Vector3(num, num, num);
		this.cloudRotateXform.up = Vector3.up;
	}

	// Token: 0x04000624 RID: 1572
	public UmbrellaItem umbrella;

	// Token: 0x04000625 RID: 1573
	public Transform cloudRotateXform;

	// Token: 0x04000626 RID: 1574
	public Renderer cloudRenderer;

	// Token: 0x04000627 RID: 1575
	public AnimationCurve scaleCurve;

	// Token: 0x04000628 RID: 1576
	private const float kHideAtScale = 0.1f;

	// Token: 0x04000629 RID: 1577
	private const float kHideAtScaleTolerance = 0.01f;

	// Token: 0x0400062A RID: 1578
	private bool rendererOn;

	// Token: 0x0400062B RID: 1579
	private Transform umbrellaXform;

	// Token: 0x0400062C RID: 1580
	private Transform cloudScaleXform;
}
