using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class GorillaPlaySpace : MonoBehaviour
{
	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06001BCB RID: 7115 RVA: 0x00087B38 File Offset: 0x00085D38
	public static GorillaPlaySpace Instance
	{
		get
		{
			return GorillaPlaySpace._instance;
		}
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00087B3F File Offset: 0x00085D3F
	private void Awake()
	{
		if (GorillaPlaySpace._instance != null && GorillaPlaySpace._instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GorillaPlaySpace._instance = this;
	}

	// Token: 0x04001EAC RID: 7852
	[OnEnterPlay_SetNull]
	private static GorillaPlaySpace _instance;

	// Token: 0x04001EAD RID: 7853
	public Collider headCollider;

	// Token: 0x04001EAE RID: 7854
	public Collider bodyCollider;

	// Token: 0x04001EAF RID: 7855
	public Transform rightHandTransform;

	// Token: 0x04001EB0 RID: 7856
	public Transform leftHandTransform;

	// Token: 0x04001EB1 RID: 7857
	public Vector3 headColliderOffset;

	// Token: 0x04001EB2 RID: 7858
	public Vector3 bodyColliderOffset;

	// Token: 0x04001EB3 RID: 7859
	private Vector3 lastLeftHandPosition;

	// Token: 0x04001EB4 RID: 7860
	private Vector3 lastRightHandPosition;

	// Token: 0x04001EB5 RID: 7861
	private Vector3 lastLeftHandPositionForTag;

	// Token: 0x04001EB6 RID: 7862
	private Vector3 lastRightHandPositionForTag;

	// Token: 0x04001EB7 RID: 7863
	private Vector3 lastBodyPositionForTag;

	// Token: 0x04001EB8 RID: 7864
	private Vector3 lastHeadPositionForTag;

	// Token: 0x04001EB9 RID: 7865
	private Rigidbody playspaceRigidbody;

	// Token: 0x04001EBA RID: 7866
	public Transform headsetTransform;

	// Token: 0x04001EBB RID: 7867
	public Vector3 rightHandOffset;

	// Token: 0x04001EBC RID: 7868
	public Vector3 leftHandOffset;

	// Token: 0x04001EBD RID: 7869
	public VRRig vrRig;

	// Token: 0x04001EBE RID: 7870
	public VRRig offlineVRRig;

	// Token: 0x04001EBF RID: 7871
	public float vibrationCooldown = 0.1f;

	// Token: 0x04001EC0 RID: 7872
	public float vibrationDuration = 0.05f;

	// Token: 0x04001EC1 RID: 7873
	private float leftLastTouchedSurface;

	// Token: 0x04001EC2 RID: 7874
	private float rightLastTouchedSurface;

	// Token: 0x04001EC3 RID: 7875
	public VRRig myVRRig;

	// Token: 0x04001EC4 RID: 7876
	private float bodyHeight;

	// Token: 0x04001EC5 RID: 7877
	public float tagCooldown;

	// Token: 0x04001EC6 RID: 7878
	public float taggedTime;

	// Token: 0x04001EC7 RID: 7879
	public float disconnectTime = 60f;

	// Token: 0x04001EC8 RID: 7880
	public float maxStepVelocity = 2f;

	// Token: 0x04001EC9 RID: 7881
	public float hapticWaitSeconds = 0.05f;

	// Token: 0x04001ECA RID: 7882
	public float tapHapticDuration = 0.05f;

	// Token: 0x04001ECB RID: 7883
	public float tapHapticStrength = 0.5f;

	// Token: 0x04001ECC RID: 7884
	public float tagHapticDuration = 0.15f;

	// Token: 0x04001ECD RID: 7885
	public float tagHapticStrength = 1f;

	// Token: 0x04001ECE RID: 7886
	public float taggedHapticDuration = 0.35f;

	// Token: 0x04001ECF RID: 7887
	public float taggedHapticStrength = 1f;
}
