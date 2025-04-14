using System;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class RequestCaptureFlow : MonoBehaviour
{
	// Token: 0x060013AA RID: 5034 RVA: 0x0006084A File Offset: 0x0005EA4A
	private void Start()
	{
		this._sceneManager = Object.FindObjectOfType<OVRSceneManager>();
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x00060857 File Offset: 0x0005EA57
	private void Update()
	{
		if (OVRInput.GetUp(this.RequestCaptureBtn, OVRInput.Controller.Active))
		{
			this._sceneManager.RequestSceneCapture();
		}
	}

	// Token: 0x040015D5 RID: 5589
	public OVRInput.Button RequestCaptureBtn = OVRInput.Button.Two;

	// Token: 0x040015D6 RID: 5590
	private OVRSceneManager _sceneManager;
}
