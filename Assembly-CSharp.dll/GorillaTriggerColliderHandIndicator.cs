using System;
using UnityEngine;

// Token: 0x02000673 RID: 1651
public class GorillaTriggerColliderHandIndicator : MonoBehaviour
{
	// Token: 0x060028EE RID: 10478 RVA: 0x0004AE82 File Offset: 0x00049082
	private void Update()
	{
		this.currentVelocity = (base.transform.position - this.lastPosition) / Time.deltaTime;
		this.lastPosition = base.transform.position;
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x0004AEBB File Offset: 0x000490BB
	private void OnTriggerEnter(Collider other)
	{
		if (this.throwableController != null)
		{
			this.throwableController.GrabbableObjectHover(this.isLeftHand);
		}
	}

	// Token: 0x04002DFA RID: 11770
	public Vector3 currentVelocity;

	// Token: 0x04002DFB RID: 11771
	public Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002DFC RID: 11772
	public bool isLeftHand;

	// Token: 0x04002DFD RID: 11773
	public GorillaThrowableController throwableController;
}
