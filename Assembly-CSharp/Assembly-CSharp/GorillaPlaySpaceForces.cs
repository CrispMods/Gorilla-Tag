using System;
using UnityEngine;

// Token: 0x02000474 RID: 1140
public class GorillaPlaySpaceForces : MonoBehaviour
{
	// Token: 0x06001BD1 RID: 7121 RVA: 0x00087F80 File Offset: 0x00086180
	private void Start()
	{
		this.playspaceRigidbody = base.GetComponent<Rigidbody>();
		this.leftHandRigidbody = this.leftHand.GetComponent<Rigidbody>();
		this.leftHandCollider = this.leftHand.GetComponent<Collider>();
		this.rightHandRigidbody = this.rightHand.GetComponent<Rigidbody>();
		this.rightHandCollider = this.rightHand.GetComponent<Collider>();
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x00087FDD File Offset: 0x000861DD
	private void FixedUpdate()
	{
		if (Time.time >= 0.1f)
		{
			this.bodyCollider.transform.position = this.headsetTransform.position + this.bodyColliderOffset;
		}
	}

	// Token: 0x04001ED1 RID: 7889
	public GameObject rightHand;

	// Token: 0x04001ED2 RID: 7890
	public GameObject leftHand;

	// Token: 0x04001ED3 RID: 7891
	public Collider bodyCollider;

	// Token: 0x04001ED4 RID: 7892
	private Collider leftHandCollider;

	// Token: 0x04001ED5 RID: 7893
	private Collider rightHandCollider;

	// Token: 0x04001ED6 RID: 7894
	public Transform rightHandTransform;

	// Token: 0x04001ED7 RID: 7895
	public Transform leftHandTransform;

	// Token: 0x04001ED8 RID: 7896
	private Rigidbody leftHandRigidbody;

	// Token: 0x04001ED9 RID: 7897
	private Rigidbody rightHandRigidbody;

	// Token: 0x04001EDA RID: 7898
	public Vector3 bodyColliderOffset;

	// Token: 0x04001EDB RID: 7899
	public float forceConstant;

	// Token: 0x04001EDC RID: 7900
	private Vector3 lastLeftHandPosition;

	// Token: 0x04001EDD RID: 7901
	private Vector3 lastRightHandPosition;

	// Token: 0x04001EDE RID: 7902
	private Rigidbody playspaceRigidbody;

	// Token: 0x04001EDF RID: 7903
	public Transform headsetTransform;
}
