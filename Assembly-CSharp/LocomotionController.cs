using System;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class LocomotionController : MonoBehaviour
{
	// Token: 0x060010FD RID: 4349 RVA: 0x000522BC File Offset: 0x000504BC
	private void Start()
	{
		if (this.CameraRig == null)
		{
			this.CameraRig = Object.FindObjectOfType<OVRCameraRig>();
		}
	}

	// Token: 0x040012FF RID: 4863
	public OVRCameraRig CameraRig;

	// Token: 0x04001300 RID: 4864
	public CapsuleCollider CharacterController;

	// Token: 0x04001301 RID: 4865
	public SimpleCapsuleWithStickMovement PlayerController;
}
