using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x02000472 RID: 1138
public class GorillaIKHandTarget : MonoBehaviour
{
	// Token: 0x06001BCA RID: 7114 RVA: 0x00087E71 File Offset: 0x00086071
	private void Start()
	{
		this.thisRigidbody = base.gameObject.GetComponent<Rigidbody>();
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x00087E84 File Offset: 0x00086084
	private void FixedUpdate()
	{
		this.thisRigidbody.MovePosition(this.handToStickTo.transform.position);
		base.transform.rotation = this.handToStickTo.transform.rotation;
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnCollisionEnter(Collision collision)
	{
	}

	// Token: 0x04001EA8 RID: 7848
	public GameObject handToStickTo;

	// Token: 0x04001EA9 RID: 7849
	public bool isLeftHand;

	// Token: 0x04001EAA RID: 7850
	public float hapticStrength;

	// Token: 0x04001EAB RID: 7851
	private Rigidbody thisRigidbody;

	// Token: 0x04001EAC RID: 7852
	private XRController controllerReference;
}
