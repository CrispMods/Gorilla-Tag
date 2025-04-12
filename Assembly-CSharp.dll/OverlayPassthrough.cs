using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class OverlayPassthrough : MonoBehaviour
{
	// Token: 0x060012DC RID: 4828 RVA: 0x000B3A58 File Offset: 0x000B1C58
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

	// Token: 0x060012DD RID: 4829 RVA: 0x000B3AA8 File Offset: 0x000B1CA8
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
		{
			this.passthroughLayer.hidden = !this.passthroughLayer.hidden;
		}
		float x = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
		this.passthroughLayer.textureOpacity = x * 0.5f + 0.5f;
	}

	// Token: 0x040014DB RID: 5339
	private OVRPassthroughLayer passthroughLayer;
}
