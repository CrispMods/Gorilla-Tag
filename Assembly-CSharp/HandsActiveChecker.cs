using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class HandsActiveChecker : MonoBehaviour
{
	// Token: 0x06001252 RID: 4690 RVA: 0x000575D0 File Offset: 0x000557D0
	private void Awake()
	{
		this._notification = Object.Instantiate<GameObject>(this._notificationPrefab);
		base.StartCoroutine(this.GetCenterEye());
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x000575F0 File Offset: 0x000557F0
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

	// Token: 0x06001254 RID: 4692 RVA: 0x0005767A File Offset: 0x0005587A
	private IEnumerator GetCenterEye()
	{
		if ((this._cameraRig = Object.FindObjectOfType<OVRCameraRig>()) != null)
		{
			while (!this._centerEye)
			{
				this._centerEye = this._cameraRig.centerEyeAnchor;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001443 RID: 5187
	[SerializeField]
	private GameObject _notificationPrefab;

	// Token: 0x04001444 RID: 5188
	private GameObject _notification;

	// Token: 0x04001445 RID: 5189
	private OVRCameraRig _cameraRig;

	// Token: 0x04001446 RID: 5190
	private Transform _centerEye;
}
