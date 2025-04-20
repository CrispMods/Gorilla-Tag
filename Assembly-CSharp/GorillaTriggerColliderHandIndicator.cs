using System;
using UnityEngine;

// Token: 0x02000651 RID: 1617
public class GorillaTriggerColliderHandIndicator : MonoBehaviour
{
	// Token: 0x06002811 RID: 10257 RVA: 0x0004B41F File Offset: 0x0004961F
	private void Update()
	{
		this.currentVelocity = (base.transform.position - this.lastPosition) / Time.deltaTime;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x06002812 RID: 10258 RVA: 0x0004B458 File Offset: 0x00049658
	private void OnTriggerEnter(Collider other)
	{
		if (this.throwableController != null)
		{
			this.throwableController.GrabbableObjectHover(this.isLeftHand);
		}
	}

	// Token: 0x04002D5A RID: 11610
	public Vector3 currentVelocity;

	// Token: 0x04002D5B RID: 11611
	public Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002D5C RID: 11612
	public bool isLeftHand;

	// Token: 0x04002D5D RID: 11613
	public GorillaThrowableController throwableController;
}
