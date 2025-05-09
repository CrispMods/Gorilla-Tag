﻿using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class HandsActiveChecker : MonoBehaviour
{
	// Token: 0x06001255 RID: 4693 RVA: 0x0003B96E File Offset: 0x00039B6E
	private void Awake()
	{
		this._notification = UnityEngine.Object.Instantiate<GameObject>(this._notificationPrefab);
		base.StartCoroutine(this.GetCenterEye());
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x000AF238 File Offset: 0x000AD438
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

	// Token: 0x06001257 RID: 4695 RVA: 0x0003B98E File Offset: 0x00039B8E
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

	// Token: 0x04001444 RID: 5188
	[SerializeField]
	private GameObject _notificationPrefab;

	// Token: 0x04001445 RID: 5189
	private GameObject _notification;

	// Token: 0x04001446 RID: 5190
	private OVRCameraRig _cameraRig;

	// Token: 0x04001447 RID: 5191
	private Transform _centerEye;
}
