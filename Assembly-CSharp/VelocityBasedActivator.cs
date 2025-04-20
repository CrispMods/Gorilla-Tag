using System;
using UnityEngine;

// Token: 0x020008FE RID: 2302
[RequireComponent(typeof(GorillaVelocityEstimator))]
public class VelocityBasedActivator : MonoBehaviour
{
	// Token: 0x0600377E RID: 14206 RVA: 0x00054A69 File Offset: 0x00052C69
	private void Start()
	{
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x001483EC File Offset: 0x001465EC
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

	// Token: 0x06003780 RID: 14208 RVA: 0x00148478 File Offset: 0x00146678
	private void activate(bool v)
	{
		this.active = v;
		for (int i = 0; i < this.activationTargets.Length; i++)
		{
			this.activationTargets[i].SetActive(v);
		}
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x00054A77 File Offset: 0x00052C77
	private void OnDisable()
	{
		if (this.active)
		{
			this.activate(false);
		}
	}

	// Token: 0x04003A57 RID: 14935
	[SerializeField]
	private GameObject[] activationTargets;

	// Token: 0x04003A58 RID: 14936
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04003A59 RID: 14937
	private float k;

	// Token: 0x04003A5A RID: 14938
	private bool active;

	// Token: 0x04003A5B RID: 14939
	[SerializeField]
	private float decay = 1f;

	// Token: 0x04003A5C RID: 14940
	[SerializeField]
	private float threshold = 1f;
}
