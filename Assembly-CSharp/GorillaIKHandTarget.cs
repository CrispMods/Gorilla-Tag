using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x02000472 RID: 1138
public class GorillaIKHandTarget : MonoBehaviour
{
	// Token: 0x06001BC7 RID: 7111 RVA: 0x00087AED File Offset: 0x00085CED
	private void Start()
	{
		this.thisRigidbody = base.gameObject.GetComponent<Rigidbody>();
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x00087B00 File Offset: 0x00085D00
	private void FixedUpdate()
	{
		this.thisRigidbody.MovePosition(this.handToStickTo.transform.position);
		base.transform.rotation = this.handToStickTo.transform.rotation;
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x04001EA7 RID: 7847
	public GameObject handToStickTo;

	// Token: 0x04001EA8 RID: 7848
	public bool isLeftHand;

	// Token: 0x04001EA9 RID: 7849
	public float hapticStrength;

	// Token: 0x04001EAA RID: 7850
	private Rigidbody thisRigidbody;

	// Token: 0x04001EAB RID: 7851
	private XRController controllerReference;
}
