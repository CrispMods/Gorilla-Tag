using System;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
[RequireComponent(typeof(GorillaVelocityEstimator))]
public class VelocityBasedActivator : MonoBehaviour
{
	// Token: 0x060036C2 RID: 14018 RVA: 0x0005354C File Offset: 0x0005174C
	private void Start()
	{
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00142E2C File Offset: 0x0014102C
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

	// Token: 0x060036C4 RID: 14020 RVA: 0x00142EB8 File Offset: 0x001410B8
	private void activate(bool v)
	{
		this.active = v;
		for (int i = 0; i < this.activationTargets.Length; i++)
		{
			this.activationTargets[i].SetActive(v);
		}
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0005355A File Offset: 0x0005175A
	private void OnDisable()
	{
		if (this.active)
		{
			this.activate(false);
		}
	}

	// Token: 0x040039A8 RID: 14760
	[SerializeField]
	private GameObject[] activationTargets;

	// Token: 0x040039A9 RID: 14761
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040039AA RID: 14762
	private float k;

	// Token: 0x040039AB RID: 14763
	private bool active;

	// Token: 0x040039AC RID: 14764
	[SerializeField]
	private float decay = 1f;

	// Token: 0x040039AD RID: 14765
	[SerializeField]
	private float threshold = 1f;
}
