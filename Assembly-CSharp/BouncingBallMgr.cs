using System;
using UnityEngine;

// Token: 0x02000333 RID: 819
public class BouncingBallMgr : MonoBehaviour
{
	// Token: 0x06001375 RID: 4981 RVA: 0x000B71E0 File Offset: 0x000B53E0
	private void Update()
	{
		if (!this.ballGrabbed && OVRInput.GetDown(this.actionBtn, OVRInput.Controller.Active))
		{
			this.currentBall = UnityEngine.Object.Instantiate<GameObject>(this.ball, this.rightControllerPivot.transform.position, Quaternion.identity);
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

	// Token: 0x04001573 RID: 5491
	[SerializeField]
	private Transform trackingspace;

	// Token: 0x04001574 RID: 5492
	[SerializeField]
	private GameObject rightControllerPivot;

	// Token: 0x04001575 RID: 5493
	[SerializeField]
	private OVRInput.RawButton actionBtn;

	// Token: 0x04001576 RID: 5494
	[SerializeField]
	private GameObject ball;

	// Token: 0x04001577 RID: 5495
	private GameObject currentBall;

	// Token: 0x04001578 RID: 5496
	private bool ballGrabbed;
}
