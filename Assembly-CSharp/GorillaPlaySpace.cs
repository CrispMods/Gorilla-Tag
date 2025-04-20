using System;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class GorillaPlaySpace : MonoBehaviour
{
	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001C1F RID: 7199 RVA: 0x00043551 File Offset: 0x00041751
	public static GorillaPlaySpace Instance
	{
		get
		{
			return GorillaPlaySpace._instance;
		}
	}

	// Token: 0x06001C20 RID: 7200 RVA: 0x00043558 File Offset: 0x00041758
	private void Awake()
	{
		if (GorillaPlaySpace._instance != null && GorillaPlaySpace._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GorillaPlaySpace._instance = this;
	}

	// Token: 0x04001EFB RID: 7931
	[OnEnterPlay_SetNull]
	private static GorillaPlaySpace _instance;

	// Token: 0x04001EFC RID: 7932
	public Collider headCollider;

	// Token: 0x04001EFD RID: 7933
	public Collider bodyCollider;

	// Token: 0x04001EFE RID: 7934
	public Transform rightHandTransform;

	// Token: 0x04001EFF RID: 7935
	public Transform leftHandTransform;

	// Token: 0x04001F00 RID: 7936
	public Vector3 headColliderOffset;

	// Token: 0x04001F01 RID: 7937
	public Vector3 bodyColliderOffset;

	// Token: 0x04001F02 RID: 7938
	private Vector3 lastLeftHandPosition;

	// Token: 0x04001F03 RID: 7939
	private Vector3 lastRightHandPosition;

	// Token: 0x04001F04 RID: 7940
	private Vector3 lastLeftHandPositionForTag;

	// Token: 0x04001F05 RID: 7941
	private Vector3 lastRightHandPositionForTag;

	// Token: 0x04001F06 RID: 7942
	private Vector3 lastBodyPositionForTag;

	// Token: 0x04001F07 RID: 7943
	private Vector3 lastHeadPositionForTag;

	// Token: 0x04001F08 RID: 7944
	private Rigidbody playspaceRigidbody;

	// Token: 0x04001F09 RID: 7945
	public Transform headsetTransform;

	// Token: 0x04001F0A RID: 7946
	public Vector3 rightHandOffset;

	// Token: 0x04001F0B RID: 7947
	public Vector3 leftHandOffset;

	// Token: 0x04001F0C RID: 7948
	public VRRig vrRig;

	// Token: 0x04001F0D RID: 7949
	public VRRig offlineVRRig;

	// Token: 0x04001F0E RID: 7950
	public float vibrationCooldown = 0.1f;

	// Token: 0x04001F0F RID: 7951
	public float vibrationDuration = 0.05f;

	// Token: 0x04001F10 RID: 7952
	private float leftLastTouchedSurface;

	// Token: 0x04001F11 RID: 7953
	private float rightLastTouchedSurface;

	// Token: 0x04001F12 RID: 7954
	public VRRig myVRRig;

	// Token: 0x04001F13 RID: 7955
	private float bodyHeight;

	// Token: 0x04001F14 RID: 7956
	public float tagCooldown;

	// Token: 0x04001F15 RID: 7957
	public float taggedTime;

	// Token: 0x04001F16 RID: 7958
	public float disconnectTime = 60f;

	// Token: 0x04001F17 RID: 7959
	public float maxStepVelocity = 2f;

	// Token: 0x04001F18 RID: 7960
	public float hapticWaitSeconds = 0.05f;

	// Token: 0x04001F19 RID: 7961
	public float tapHapticDuration = 0.05f;

	// Token: 0x04001F1A RID: 7962
	public float tapHapticStrength = 0.5f;

	// Token: 0x04001F1B RID: 7963
	public float tagHapticDuration = 0.15f;

	// Token: 0x04001F1C RID: 7964
	public float tagHapticStrength = 1f;

	// Token: 0x04001F1D RID: 7965
	public float taggedHapticDuration = 0.35f;

	// Token: 0x04001F1E RID: 7966
	public float taggedHapticStrength = 1f;
}
