using System;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class PassthroughController : MonoBehaviour
{
	// Token: 0x060012E6 RID: 4838 RVA: 0x000B3D84 File Offset: 0x000B1F84
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

	// Token: 0x060012E7 RID: 4839 RVA: 0x000B3DD4 File Offset: 0x000B1FD4
	private void Update()
	{
		Color edgeColor = Color.HSVToRGB(Time.time * 0.1f % 1f, 1f, 1f);
		this.passthroughLayer.edgeColor = edgeColor;
		float contrast = Mathf.Sin(Time.time);
		this.passthroughLayer.SetColorMapControls(contrast, 0f, 0f, null, OVRPassthroughLayer.ColorMapEditorType.GrayscaleToColor);
		base.transform.position = Camera.main.transform.position;
		base.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized);
	}

	// Token: 0x040014E9 RID: 5353
	private OVRPassthroughLayer passthroughLayer;
}
