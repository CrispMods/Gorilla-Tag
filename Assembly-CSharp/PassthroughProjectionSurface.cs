using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
public class PassthroughProjectionSurface : MonoBehaviour
{
	// Token: 0x060012E6 RID: 4838 RVA: 0x0005C6C4 File Offset: 0x0005A8C4
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

	// Token: 0x060012E7 RID: 4839 RVA: 0x0005C748 File Offset: 0x0005A948
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

	// Token: 0x040014E9 RID: 5353
	private OVRPassthroughLayer passthroughLayer;

	// Token: 0x040014EA RID: 5354
	public MeshFilter projectionObject;

	// Token: 0x040014EB RID: 5355
	private MeshRenderer quadOutline;
}
