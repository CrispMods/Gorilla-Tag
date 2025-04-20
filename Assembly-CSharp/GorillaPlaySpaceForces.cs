using System;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class GorillaPlaySpaceForces : MonoBehaviour
{
	// Token: 0x06001C22 RID: 7202 RVA: 0x000DBB0C File Offset: 0x000D9D0C
	private void Start()
	{
		this.playspaceRigidbody = base.GetComponent<Rigidbody>();
		this.leftHandRigidbody = this.leftHand.GetComponent<Rigidbody>();
		this.leftHandCollider = this.leftHand.GetComponent<Collider>();
		this.rightHandRigidbody = this.rightHand.GetComponent<Rigidbody>();
		this.rightHandCollider = this.rightHand.GetComponent<Collider>();
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x00043586 File Offset: 0x00041786
	private void FixedUpdate()
	{
		if (Time.time >= 0.1f)
		{
			this.bodyCollider.transform.position = this.headsetTransform.position + this.bodyColliderOffset;
		}
	}

	// Token: 0x04001F1F RID: 7967
	public GameObject rightHand;

	// Token: 0x04001F20 RID: 7968
	public GameObject leftHand;

	// Token: 0x04001F21 RID: 7969
	public Collider bodyCollider;

	// Token: 0x04001F22 RID: 7970
	private Collider leftHandCollider;

	// Token: 0x04001F23 RID: 7971
	private Collider rightHandCollider;

	// Token: 0x04001F24 RID: 7972
	public Transform rightHandTransform;

	// Token: 0x04001F25 RID: 7973
	public Transform leftHandTransform;

	// Token: 0x04001F26 RID: 7974
	private Rigidbody leftHandRigidbody;

	// Token: 0x04001F27 RID: 7975
	private Rigidbody rightHandRigidbody;

	// Token: 0x04001F28 RID: 7976
	public Vector3 bodyColliderOffset;

	// Token: 0x04001F29 RID: 7977
	public float forceConstant;

	// Token: 0x04001F2A RID: 7978
	private Vector3 lastLeftHandPosition;

	// Token: 0x04001F2B RID: 7979
	private Vector3 lastRightHandPosition;

	// Token: 0x04001F2C RID: 7980
	private Rigidbody playspaceRigidbody;

	// Token: 0x04001F2D RID: 7981
	public Transform headsetTransform;
}
