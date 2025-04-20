using System;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class OverlayPassthrough : MonoBehaviour
{
	// Token: 0x06001325 RID: 4901 RVA: 0x000B62F0 File Offset: 0x000B44F0
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

	// Token: 0x06001326 RID: 4902 RVA: 0x000B6340 File Offset: 0x000B4540
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
		{
			this.passthroughLayer.hidden = !this.passthroughLayer.hidden;
		}
		float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
		this.passthroughLayer.textureOpacity = x * 0.5f + 0.5f;
	}

	// Token: 0x04001522 RID: 5410
	private OVRPassthroughLayer passthroughLayer;
}
