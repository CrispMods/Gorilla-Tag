using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class GorillaWalkingGrab : MonoBehaviour
{
	// Token: 0x06001C38 RID: 7224 RVA: 0x000436BB File Offset: 0x000418BB
	private void Start()
	{
		this.thisRigidbody = base.gameObject.GetComponent<Rigidbody>();
		this.positionHistory = new Vector3[this.historySteps];
		this.historyIndex = 0;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x000DBBD8 File Offset: 0x000D9DD8
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

	// Token: 0x06001C3A RID: 7226 RVA: 0x00030498 File Offset: 0x0002E698
	private bool MakeJump()
	{
		return false;
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x000DBC60 File Offset: 0x000D9E60
	private void OnCollisionStay(Collision collision)
	{
		if (!this.MakeJump())
		{
			Vector3 b = Vector3.ProjectOnPlane(this.positionHistory[(this.historyIndex != 0) ? (this.historyIndex - 1) : (this.historySteps - 1)] - this.handToStickTo.transform.position, collision.GetContact(0).normal);
			Vector3 b2 = this.thisRigidbody.transform.position - this.handToStickTo.transform.position;
			this.playspaceRigidbody.MovePosition(this.playspaceRigidbody.transform.position + b - b2);
		}
	}

	// Token: 0x04001F3F RID: 7999
	public GameObject handToStickTo;

	// Token: 0x04001F40 RID: 8000
	public float ratioToUse;

	// Token: 0x04001F41 RID: 8001
	public float forceMultiplier;

	// Token: 0x04001F42 RID: 8002
	public int historySteps;

	// Token: 0x04001F43 RID: 8003
	public Rigidbody playspaceRigidbody;

	// Token: 0x04001F44 RID: 8004
	private Rigidbody thisRigidbody;

	// Token: 0x04001F45 RID: 8005
	private Vector3 lastPosition;

	// Token: 0x04001F46 RID: 8006
	private Vector3 maybeLastPositionIDK;

	// Token: 0x04001F47 RID: 8007
	private Vector3[] positionHistory;

	// Token: 0x04001F48 RID: 8008
	private int historyIndex;
}
