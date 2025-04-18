﻿using System;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class BouncingBallMgr : MonoBehaviour
{
	// Token: 0x06001329 RID: 4905 RVA: 0x0005D494 File Offset: 0x0005B694
	private void Update()
	{
		if (!this.ballGrabbed && OVRInput.GetDown(this.actionBtn, OVRInput.Controller.Active))
		{
			this.currentBall = Object.Instantiate<GameObject>(this.ball, this.rightControllerPivot.transform.position, Quaternion.identity);
			this.currentBall.transform.parent = this.rightControllerPivot.transform;
			this.ballGrabbed = true;
		}
		if (this.ballGrabbed && OVRInput.GetUp(this.actionBtn, OVRInput.Controller.Active))
		{
			this.currentBall.transform.parent = null;
			Vector3 position = this.currentBall.transform.position;
			Vector3 vel = this.trackingspace.rotation * OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
			Vector3 localControllerAngularVelocity = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
			this.currentBall.GetComponent<BouncingBallLogic>().Release(position, vel, localControllerAngularVelocity);
			this.ballGrabbed = false;
		}
	}

	// Token: 0x0400152B RID: 5419
	[SerializeField]
	private Transform trackingspace;

	// Token: 0x0400152C RID: 5420
	[SerializeField]
	private GameObject rightControllerPivot;

	// Token: 0x0400152D RID: 5421
	[SerializeField]
	private OVRInput.RawButton actionBtn;

	// Token: 0x0400152E RID: 5422
	[SerializeField]
	private GameObject ball;

	// Token: 0x0400152F RID: 5423
	private GameObject currentBall;

	// Token: 0x04001530 RID: 5424
	private bool ballGrabbed;
}
