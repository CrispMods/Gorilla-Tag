using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002C8 RID: 712
public class HandedInputSelector : MonoBehaviour
{
	// Token: 0x06001131 RID: 4401 RVA: 0x0003BB25 File Offset: 0x00039D25
	private void Start()
	{
		this.m_CameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
		this.m_InputModule = UnityEngine.Object.FindObjectOfType<OVRInputModule>();
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x0003BB3D File Offset: 0x00039D3D
	private void Update()
	{
		if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
		{
			this.SetActiveController(OVRInput.Controller.LTouch);
			return;
		}
		this.SetActiveController(OVRInput.Controller.RTouch);
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x000AD118 File Offset: 0x000AB318
	private void SetActiveController(OVRInput.Controller c)
	{
		Transform rayTransform;
		if (c == OVRInput.Controller.LTouch)
		{
			rayTransform = this.m_CameraRig.leftHandAnchor;
		}
		else
		{
			rayTransform = this.m_CameraRig.rightHandAnchor;
		}
		this.m_InputModule.rayTransform = rayTransform;
	}

	// Token: 0x0400132E RID: 4910
	private OVRCameraRig m_CameraRig;

	// Token: 0x0400132F RID: 4911
	private OVRInputModule m_InputModule;
}
