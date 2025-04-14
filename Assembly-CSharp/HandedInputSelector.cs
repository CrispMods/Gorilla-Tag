using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002BD RID: 701
public class HandedInputSelector : MonoBehaviour
{
	// Token: 0x060010E5 RID: 4325 RVA: 0x00051AB2 File Offset: 0x0004FCB2
	private void Start()
	{
		this.m_CameraRig = Object.FindObjectOfType<OVRCameraRig>();
		this.m_InputModule = Object.FindObjectOfType<OVRInputModule>();
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00051ACA File Offset: 0x0004FCCA
	private void Update()
	{
		if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
		{
			this.SetActiveController(OVRInput.Controller.LTouch);
			return;
		}
		this.SetActiveController(OVRInput.Controller.RTouch);
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00051AE4 File Offset: 0x0004FCE4
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

	// Token: 0x040012E6 RID: 4838
	private OVRCameraRig m_CameraRig;

	// Token: 0x040012E7 RID: 4839
	private OVRInputModule m_InputModule;
}
