using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class LocomotionController : MonoBehaviour
{
	// Token: 0x06001149 RID: 4425 RVA: 0x0003BD0C File Offset: 0x00039F0C
	private void Start()
	{
		if (this.CameraRig == null)
		{
			this.CameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
		}
	}

	// Token: 0x04001347 RID: 4935
	public OVRCameraRig CameraRig;

	// Token: 0x04001348 RID: 4936
	public CapsuleCollider CharacterController;

	// Token: 0x04001349 RID: 4937
	public SimpleCapsuleWithStickMovement PlayerController;
}
