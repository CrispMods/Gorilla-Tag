using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public class FPSController : MonoBehaviour
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x060032F5 RID: 13045 RVA: 0x001368A4 File Offset: 0x00134AA4
	// (remove) Token: 0x060032F6 RID: 13046 RVA: 0x001368DC File Offset: 0x00134ADC
	[HideInInspector]
	public event FPSController.OnStateChangeEventHandler OnStartEvent;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x060032F7 RID: 13047 RVA: 0x00136914 File Offset: 0x00134B14
	// (remove) Token: 0x060032F8 RID: 13048 RVA: 0x0013694C File Offset: 0x00134B4C
	public event FPSController.OnStateChangeEventHandler OnStopEvent;

	// Token: 0x0400366D RID: 13933
	public float baseMoveSpeed = 4f;

	// Token: 0x0400366E RID: 13934
	public float shiftMoveSpeed = 8f;

	// Token: 0x0400366F RID: 13935
	public float ctrlMoveSpeed = 1f;

	// Token: 0x04003670 RID: 13936
	public float lookHorizontal = 0.4f;

	// Token: 0x04003671 RID: 13937
	public float lookVertical = 0.25f;

	// Token: 0x04003672 RID: 13938
	[SerializeField]
	private Vector3 leftControllerPosOffset = new Vector3(-0.2f, -0.25f, 0.3f);

	// Token: 0x04003673 RID: 13939
	[SerializeField]
	private Vector3 leftControllerRotationOffset = new Vector3(265f, -82f, 28f);

	// Token: 0x04003674 RID: 13940
	[SerializeField]
	private Vector3 rightControllerPosOffset = new Vector3(0.2f, -0.25f, 0.3f);

	// Token: 0x04003675 RID: 13941
	[SerializeField]
	private Vector3 rightControllerRotationOffset = new Vector3(263f, 318f, 485f);

	// Token: 0x04003676 RID: 13942
	[SerializeField]
	private bool toggleGrab;

	// Token: 0x04003679 RID: 13945
	private bool controlRightHand;

	// Token: 0x0400367A RID: 13946
	public LayerMask HandMask;

	// Token: 0x0200081F RID: 2079
	// (Invoke) Token: 0x060032FB RID: 13051
	public delegate void OnStateChangeEventHandler();
}
