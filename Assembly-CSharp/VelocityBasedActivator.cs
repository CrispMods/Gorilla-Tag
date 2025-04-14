using System;
using UnityEngine;

// Token: 0x020008E2 RID: 2274
[RequireComponent(typeof(GorillaVelocityEstimator))]
public class VelocityBasedActivator : MonoBehaviour
{
	// Token: 0x060036B6 RID: 14006 RVA: 0x0010313A File Offset: 0x0010133A
	private void Start()
	{
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x00103148 File Offset: 0x00101348
	private void Update()
	{
		this.k += this.velocityEstimator.linearVelocity.sqrMagnitude;
		this.k = Mathf.Max(this.k - Time.deltaTime * this.decay, 0f);
		if (!this.active && this.k > this.threshold)
		{
			this.activate(true);
		}
		if (this.active && this.k < this.threshold)
		{
			this.activate(false);
		}
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x001031D4 File Offset: 0x001013D4
	private void activate(bool v)
	{
		this.active = v;
		for (int i = 0; i < this.activationTargets.Length; i++)
		{
			this.activationTargets[i].SetActive(v);
		}
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x00103209 File Offset: 0x00101409
	private void OnDisable()
	{
		if (this.active)
		{
			this.activate(false);
		}
	}

	// Token: 0x04003996 RID: 14742
	[SerializeField]
	private GameObject[] activationTargets;

	// Token: 0x04003997 RID: 14743
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04003998 RID: 14744
	private float k;

	// Token: 0x04003999 RID: 14745
	private bool active;

	// Token: 0x0400399A RID: 14746
	[SerializeField]
	private float decay = 1f;

	// Token: 0x0400399B RID: 14747
	[SerializeField]
	private float threshold = 1f;
}
