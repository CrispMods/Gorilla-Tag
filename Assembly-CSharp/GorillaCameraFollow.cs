using System;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class GorillaCameraFollow : MonoBehaviour
{
	// Token: 0x06001C0C RID: 7180 RVA: 0x000DB8C8 File Offset: 0x000D9AC8
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

	// Token: 0x06001C0D RID: 7181 RVA: 0x000DB950 File Offset: 0x000D9B50
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

	// Token: 0x04001EE4 RID: 7908
	public Transform playerHead;

	// Token: 0x04001EE5 RID: 7909
	public GameObject cameraParent;

	// Token: 0x04001EE6 RID: 7910
	public Vector3 headOffset;

	// Token: 0x04001EE7 RID: 7911
	public Vector3 eulerRotationOffset;

	// Token: 0x04001EE8 RID: 7912
	public CinemachineVirtualCamera cinemachineCamera;

	// Token: 0x04001EE9 RID: 7913
	private Cinemachine3rdPersonFollow cinemachineFollow;

	// Token: 0x04001EEA RID: 7914
	private float baseCameraRadius = 0.2f;

	// Token: 0x04001EEB RID: 7915
	private float baseFollowDistance = 2f;

	// Token: 0x04001EEC RID: 7916
	private float baseVerticalArmLength = 0.4f;

	// Token: 0x04001EED RID: 7917
	private Vector3 baseShoulderOffset = new Vector3(0.5f, -0.4f, 0f);
}
