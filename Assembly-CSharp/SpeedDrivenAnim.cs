using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class SpeedDrivenAnim : MonoBehaviour
{
	// Token: 0x0600096A RID: 2410 RVA: 0x000323D4 File Offset: 0x000305D4
	private void Start()
	{
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
		this.animator = base.GetComponent<Animator>();
		this.keyHash = Animator.StringToHash(this.animKey);
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x00032400 File Offset: 0x00030600
	private void Update()
	{
		float target = Mathf.InverseLerp(this.speed0, this.speed1, this.velocityEstimator.linearVelocity.magnitude);
		this.currentBlend = Mathf.MoveTowards(this.currentBlend, target, this.maxChangePerSecond * Time.deltaTime);
		this.animator.SetFloat(this.keyHash, this.currentBlend);
	}

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private float speed0;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private float speed1 = 1f;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private float maxChangePerSecond = 1f;

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private string animKey = "speed";

	// Token: 0x04000B5E RID: 2910
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000B5F RID: 2911
	private Animator animator;

	// Token: 0x04000B60 RID: 2912
	private int keyHash;

	// Token: 0x04000B61 RID: 2913
	private float currentBlend;
}
