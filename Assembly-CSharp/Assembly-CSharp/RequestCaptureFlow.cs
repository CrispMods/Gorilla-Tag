using System;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class RequestCaptureFlow : MonoBehaviour
{
	// Token: 0x060013AD RID: 5037 RVA: 0x00060BCE File Offset: 0x0005EDCE
	private void Start()
	{
		this._sceneManager = Object.FindObjectOfType<OVRSceneManager>();
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x00060BDB File Offset: 0x0005EDDB
	private void Update()
	{
		if (OVRInput.GetUp(this.RequestCaptureBtn, OVRInput.Controller.Active))
		{
			this._sceneManager.RequestSceneCapture();
		}
	}

	// Token: 0x040015D6 RID: 5590
	public OVRInput.Button RequestCaptureBtn = OVRInput.Button.Two;

	// Token: 0x040015D7 RID: 5591
	private OVRSceneManager _sceneManager;
}
