using System;
using UnityEngine;

// Token: 0x02000672 RID: 1650
public class GorillaTriggerColliderHandIndicator : MonoBehaviour
{
	// Token: 0x060028E6 RID: 10470 RVA: 0x000C8F50 File Offset: 0x000C7150
	private void Update()
	{
		this.currentVelocity = (base.transform.position - this.lastPosition) / Time.deltaTime;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000C8F89 File Offset: 0x000C7189
	private void OnTriggerEnter(Collider other)
	{
		if (this.throwableController != null)
		{
			this.throwableController.GrabbableObjectHover(this.isLeftHand);
		}
	}

	// Token: 0x04002DF4 RID: 11764
	public Vector3 currentVelocity;

	// Token: 0x04002DF5 RID: 11765
	public Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002DF6 RID: 11766
	public bool isLeftHand;

	// Token: 0x04002DF7 RID: 11767
	public GorillaThrowableController throwableController;
}
