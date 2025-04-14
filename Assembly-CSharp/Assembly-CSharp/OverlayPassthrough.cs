using System;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class OverlayPassthrough : MonoBehaviour
{
	// Token: 0x060012DC RID: 4828 RVA: 0x0005C5CC File Offset: 0x0005A7CC
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

	// Token: 0x060012DD RID: 4829 RVA: 0x0005C61C File Offset: 0x0005A81C
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
