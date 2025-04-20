using System;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class PassthroughProjectionSurface : MonoBehaviour
{
	// Token: 0x06001332 RID: 4914 RVA: 0x000B6730 File Offset: 0x000B4930
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
		this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, true);
		this.quadOutline = this.projectionObject.GetComponent<MeshRenderer>();
		this.quadOutline.enabled = false;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x000B67B4 File Offset: 0x000B49B4
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Active))
		{
			this.passthroughLayer.RemoveSurfaceGeometry(this.projectionObject.gameObject);
			this.quadOutline.enabled = true;
		}
		if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.Active))
		{
			OVRInput.Controller controllerType = OVRInput.Controller.RTouch;
			base.transform.position = OVRInput.GetLocalControllerPosition(controllerType);
			base.transform.rotation = OVRInput.GetLocalControllerRotation(controllerType);
		}
		if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.Active))
		{
			this.passthroughLayer.AddSurfaceGeometry(this.projectionObject.gameObject, false);
			this.quadOutline.enabled = false;
		}
	}

	// Token: 0x04001531 RID: 5425
	private OVRPassthroughLayer passthroughLayer;

	// Token: 0x04001532 RID: 5426
	public MeshFilter projectionObject;

	// Token: 0x04001533 RID: 5427
	private MeshRenderer quadOutline;
}
