﻿using System;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public class GorillaCameraFollow : MonoBehaviour
{
	// Token: 0x06001BB8 RID: 7096 RVA: 0x00087888 File Offset: 0x00085A88
	private void Start()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			this.cameraParent.SetActive(false);
		}
		if (this.cinemachineCamera != null)
		{
			this.cinemachineFollow = this.cinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
			this.baseCameraRadius = this.cinemachineFollow.CameraRadius;
			this.baseFollowDistance = this.cinemachineFollow.CameraDistance;
			this.baseVerticalArmLength = this.cinemachineFollow.VerticalArmLength;
			this.baseShoulderOffset = this.cinemachineFollow.ShoulderOffset;
		}
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00087910 File Offset: 0x00085B10
	private void LateUpdate()
	{
		if (this.cinemachineFollow != null)
		{
			float scale = GTPlayer.Instance.scale;
			this.cinemachineFollow.CameraRadius = this.baseCameraRadius * scale;
			this.cinemachineFollow.CameraDistance = this.baseFollowDistance * scale;
			this.cinemachineFollow.VerticalArmLength = this.baseVerticalArmLength * scale;
			this.cinemachineFollow.ShoulderOffset = this.baseShoulderOffset * scale;
		}
	}

	// Token: 0x04001E95 RID: 7829
	public Transform playerHead;

	// Token: 0x04001E96 RID: 7830
	public GameObject cameraParent;

	// Token: 0x04001E97 RID: 7831
	public Vector3 headOffset;

	// Token: 0x04001E98 RID: 7832
	public Vector3 eulerRotationOffset;

	// Token: 0x04001E99 RID: 7833
	public CinemachineVirtualCamera cinemachineCamera;

	// Token: 0x04001E9A RID: 7834
	private Cinemachine3rdPersonFollow cinemachineFollow;

	// Token: 0x04001E9B RID: 7835
	private float baseCameraRadius = 0.2f;

	// Token: 0x04001E9C RID: 7836
	private float baseFollowDistance = 2f;

	// Token: 0x04001E9D RID: 7837
	private float baseVerticalArmLength = 0.4f;

	// Token: 0x04001E9E RID: 7838
	private Vector3 baseShoulderOffset = new Vector3(0.5f, -0.4f, 0f);
}
