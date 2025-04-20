using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200047E RID: 1150
public class GorillaIKHandTarget : MonoBehaviour
{
	// Token: 0x06001C1B RID: 7195 RVA: 0x00043506 File Offset: 0x00041706
	private void Start()
	{
		this.thisRigidbody = base.gameObject.GetComponent<Rigidbody>();
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00043519 File Offset: 0x00041719
	private void FixedUpdate()
	{
		this.thisRigidbody.MovePosition(this.handToStickTo.transform.position);
		base.transform.rotation = this.handToStickTo.transform.rotation;
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x04001EF6 RID: 7926
	public GameObject handToStickTo;

	// Token: 0x04001EF7 RID: 7927
	public bool isLeftHand;

	// Token: 0x04001EF8 RID: 7928
	public float hapticStrength;

	// Token: 0x04001EF9 RID: 7929
	private Rigidbody thisRigidbody;

	// Token: 0x04001EFA RID: 7930
	private XRController controllerReference;
}
