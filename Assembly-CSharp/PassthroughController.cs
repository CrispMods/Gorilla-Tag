using System;
using UnityEngine;

// Token: 0x02000327 RID: 807
public class PassthroughController : MonoBehaviour
{
	// Token: 0x0600132F RID: 4911 RVA: 0x000B661C File Offset: 0x000B481C
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

	// Token: 0x06001330 RID: 4912 RVA: 0x000B666C File Offset: 0x000B486C
	private void Update()
	{
		Color edgeColor = Color.HSVToRGB(Time.time * 0.1f % 1f, 1f, 1f);
		this.passthroughLayer.edgeColor = edgeColor;
		float contrast = Mathf.Sin(Time.time);
		this.passthroughLayer.SetColorMapControls(contrast, 0f, 0f, null, OVRPassthroughLayer.ColorMapEditorType.GrayscaleToColor);
		base.transform.position = Camera.main.transform.position;
		base.transform.rotation = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized);
	}

	// Token: 0x04001530 RID: 5424
	private OVRPassthroughLayer passthroughLayer;
}
