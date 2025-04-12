using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002BD RID: 701
public class HandedInputSelector : MonoBehaviour
{
	// Token: 0x060010E8 RID: 4328 RVA: 0x0003A865 File Offset: 0x00038A65
	private void Start()
	{
		this.m_CameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>();
		this.m_InputModule = UnityEngine.Object.FindObjectOfType<OVRInputModule>();
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0003A87D File Offset: 0x00038A7D
	private void Update()
	{
		if (OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
		{
			this.SetActiveController(OVRInput.Controller.LTouch);
			return;
		}
		this.SetActiveController(OVRInput.Controller.RTouch);
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000AA880 File Offset: 0x000A8A80
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
