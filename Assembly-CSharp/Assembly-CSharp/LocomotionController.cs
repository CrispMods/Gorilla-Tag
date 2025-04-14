using System;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class LocomotionController : MonoBehaviour
{
	// Token: 0x06001100 RID: 4352 RVA: 0x00052640 File Offset: 0x00050840
	private void Start()
	{
		if (this.CameraRig == null)
		{
			this.CameraRig = Object.FindObjectOfType<OVRCameraRig>();
		}
	}

	// Token: 0x04001300 RID: 4864
	public OVRCameraRig CameraRig;

	// Token: 0x04001301 RID: 4865
	public CapsuleCollider CharacterController;

	// Token: 0x04001302 RID: 4866
	public SimpleCapsuleWithStickMovement PlayerController;
}
