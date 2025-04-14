using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002BD RID: 701
public class HandedInputSelector : MonoBehaviour
{
	// Token: 0x060010E8 RID: 4328 RVA: 0x00051E36 File Offset: 0x00050036
	private void Start()
	{
		this.m_CameraRig = Object.FindObjectOfType<OVRCameraRig>();
		this.m_InputModule = Object.FindObjectOfType<OVRInputModule>();
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x00051E4E File Offset: 0x0005004E
	private void Update()
	{
		if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
		{
			this.SetActiveController(OVRInput.Controller.LTouch);
			return;
		}
		this.SetActiveController(OVRInput.Controller.RTouch);
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x00051E68 File Offset: 0x00050068
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

	// Token: 0x040012E7 RID: 4839
	private OVRCameraRig m_CameraRig;

	// Token: 0x040012E8 RID: 4840
	private OVRInputModule m_InputModule;
}
