using System;
using UnityEngine;

// Token: 0x02000183 RID: 387
public class SpeedDrivenAnim : MonoBehaviour
{
	// Token: 0x060009B6 RID: 2486 RVA: 0x00036DB4 File Offset: 0x00034FB4
	private void Start()
	{
		this.velocityEstimator = base.GetComponent<GorillaVelocityEstimator>();
		this.animator = base.GetComponent<Animator>();
		this.keyHash = Animator.StringToHash(this.animKey);
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x000927EC File Offset: 0x000909EC
	private void Update()
	{
		float target = Mathf.InverseLerp(this.speed0, this.speed1, this.velocityEstimator.linearVelocity.magnitude);
		this.currentBlend = Mathf.MoveTowards(this.currentBlend, target, this.maxChangePerSecond * Time.deltaTime);
		this.animator.SetFloat(this.keyHash, this.currentBlend);
	}

	// Token: 0x04000BA0 RID: 2976
	[SerializeField]
	private float speed0;

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private float speed1 = 1f;

	// Token: 0x04000BA2 RID: 2978
	[SerializeField]
	private float maxChangePerSecond = 1f;

	// Token: 0x04000BA3 RID: 2979
	[SerializeField]
	private string animKey = "speed";

	// Token: 0x04000BA4 RID: 2980
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x04000BA5 RID: 2981
	private Animator animator;

	// Token: 0x04000BA6 RID: 2982
	private int keyHash;

	// Token: 0x04000BA7 RID: 2983
	private float currentBlend;
}
