using System;
using UnityEngine;

// Token: 0x02000358 RID: 856
public class RequestCaptureFlow : MonoBehaviour
{
	// Token: 0x060013F6 RID: 5110 RVA: 0x0003D708 File Offset: 0x0003B908
	private void Start()
	{
		this._sceneManager = UnityEngine.Object.FindObjectOfType<OVRSceneManager>();
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0003D715 File Offset: 0x0003B915
	private void Update()
	{
		if (OVRInput.GetUp(this.RequestCaptureBtn, OVRInput.Controller.Active))
		{
			this._sceneManager.RequestSceneCapture();
		}
	}

	// Token: 0x0400161D RID: 5661
	public OVRInput.Button RequestCaptureBtn = OVRInput.Button.Two;

	// Token: 0x0400161E RID: 5662
	private OVRSceneManager _sceneManager;
}
