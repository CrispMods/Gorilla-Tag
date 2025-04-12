using System;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class GorillaWalkingGrab : MonoBehaviour
{
	// Token: 0x06001BE7 RID: 7143 RVA: 0x00042382 File Offset: 0x00040582
	private void Start()
	{
		this.thisRigidbody = base.gameObject.GetComponent<Rigidbody>();
		this.positionHistory = new Vector3[this.historySteps];
		this.historyIndex = 0;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000D8F28 File Offset: 0x000D7128
	private void FixedUpdate()
	{
		this.historyIndex++;
		if (this.historyIndex >= this.historySteps)
		{
			this.historyIndex = 0;
		}
		this.positionHistory[this.historyIndex] = this.handToStickTo.transform.position;
		this.thisRigidbody.MovePosition(this.handToStickTo.transform.position);
		base.transform.rotation = this.handToStickTo.transform.rotation;
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x0002F5F0 File Offset: 0x0002D7F0
	private bool MakeJump()
	{
		return false;
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x000D8FB0 File Offset: 0x000D71B0
	private void OnCollisionStay(Collision collision)
	{
		if (!this.MakeJump())
		{
			Vector3 b = Vector3.ProjectOnPlane(this.positionHistory[(this.historyIndex != 0) ? (this.historyIndex - 1) : (this.historySteps - 1)] - this.handToStickTo.transform.position, collision.GetContact(0).normal);
			Vector3 b2 = this.thisRigidbody.transform.position - this.handToStickTo.transform.position;
			this.playspaceRigidbody.MovePosition(this.playspaceRigidbody.transform.position + b - b2);
		}
	}

	// Token: 0x04001EF1 RID: 7921
	public GameObject handToStickTo;

	// Token: 0x04001EF2 RID: 7922
	public float ratioToUse;

	// Token: 0x04001EF3 RID: 7923
	public float forceMultiplier;

	// Token: 0x04001EF4 RID: 7924
	public int historySteps;

	// Token: 0x04001EF5 RID: 7925
	public Rigidbody playspaceRigidbody;

	// Token: 0x04001EF6 RID: 7926
	private Rigidbody thisRigidbody;

	// Token: 0x04001EF7 RID: 7927
	private Vector3 lastPosition;

	// Token: 0x04001EF8 RID: 7928
	private Vector3 maybeLastPositionIDK;

	// Token: 0x04001EF9 RID: 7929
	private Vector3[] positionHistory;

	// Token: 0x04001EFA RID: 7930
	private int historyIndex;
}
