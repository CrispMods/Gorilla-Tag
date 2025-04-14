using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class OverlayPassthrough : MonoBehaviour
{
	// Token: 0x060012D9 RID: 4825 RVA: 0x0005C248 File Offset: 0x0005A448
	private void Start()
	{
		GameObject gameObject = GameObject.Find("OVRCameraRig");
		if (gameObject == null)
		{
			Debug.LogError("Scene does not contain an OVRCameraRig");
			return;
		}
		this.passthroughLayer = gameObject.GetComponent<OVRPassthroughLayer>();
		if (this.passthroughLayer == null)
		{
			Debug.LogError("OVRCameraRig does not contain an OVRPassthroughLayer component");
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0005C298 File Offset: 0x0005A498
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
		{
			this.passthroughLayer.hidden = !this.passthroughLayer.hidden;
		}
		float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
		this.passthroughLayer.textureOpacity = x * 0.5f + 0.5f;
	}

	// Token: 0x040014DA RID: 5338
	private OVRPassthroughLayer passthroughLayer;
}
