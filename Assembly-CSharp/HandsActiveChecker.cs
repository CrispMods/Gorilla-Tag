using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200030D RID: 781
public class HandsActiveChecker : MonoBehaviour
{
	// Token: 0x0600129E RID: 4766 RVA: 0x0003CC2E File Offset: 0x0003AE2E
	private void Awake()
	{
		this._notification = UnityEngine.Object.Instantiate<GameObject>(this._notificationPrefab);
		base.StartCoroutine(this.GetCenterEye());
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x000B1AD0 File Offset: 0x000AFCD0
	private void Update()
	{
		if (OVRPlugin.GetHandTrackingEnabled())
		{
			this._notification.SetActive(false);
			return;
		}
		this._notification.SetActive(true);
		if (this._centerEye)
		{
			this._notification.transform.position = this._centerEye.position + this._centerEye.forward * 0.5f;
			this._notification.transform.rotation = this._centerEye.rotation;
		}
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0003CC4E File Offset: 0x0003AE4E
	private IEnumerator GetCenterEye()
	{
		if ((this._cameraRig = UnityEngine.Object.FindObjectOfType<OVRCameraRig>()) != null)
		{
			while (!this._centerEye)
			{
				this._centerEye = this._cameraRig.centerEyeAnchor;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0400148B RID: 5259
	[SerializeField]
	private GameObject _notificationPrefab;

	// Token: 0x0400148C RID: 5260
	private GameObject _notification;

	// Token: 0x0400148D RID: 5261
	private OVRCameraRig _cameraRig;

	// Token: 0x0400148E RID: 5262
	private Transform _centerEye;
}
