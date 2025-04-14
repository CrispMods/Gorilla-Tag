using System;
using UnityEngine;

// Token: 0x0200081B RID: 2075
public class FPSController : MonoBehaviour
{
	// Token: 0x14000061 RID: 97
	// (add) Token: 0x060032E9 RID: 13033 RVA: 0x000F3E78 File Offset: 0x000F2078
	// (remove) Token: 0x060032EA RID: 13034 RVA: 0x000F3EB0 File Offset: 0x000F20B0
	[HideInInspector]
	public event FPSController.OnStateChangeEventHandler OnStartEvent;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x060032EB RID: 13035 RVA: 0x000F3EE8 File Offset: 0x000F20E8
	// (remove) Token: 0x060032EC RID: 13036 RVA: 0x000F3F20 File Offset: 0x000F2120
	public event FPSController.OnStateChangeEventHandler OnStopEvent;

	// Token: 0x0400365B RID: 13915
	public float baseMoveSpeed = 4f;

	// Token: 0x0400365C RID: 13916
	public float shiftMoveSpeed = 8f;

	// Token: 0x0400365D RID: 13917
	public float ctrlMoveSpeed = 1f;

	// Token: 0x0400365E RID: 13918
	public float lookHorizontal = 0.4f;

	// Token: 0x0400365F RID: 13919
	public float lookVertical = 0.25f;

	// Token: 0x04003660 RID: 13920
	[SerializeField]
	private Vector3 leftControllerPosOffset = new Vector3(-0.2f, -0.25f, 0.3f);

	// Token: 0x04003661 RID: 13921
	[SerializeField]
	private Vector3 leftControllerRotationOffset = new Vector3(265f, -82f, 28f);

	// Token: 0x04003662 RID: 13922
	[SerializeField]
	private Vector3 rightControllerPosOffset = new Vector3(0.2f, -0.25f, 0.3f);

	// Token: 0x04003663 RID: 13923
	[SerializeField]
	private Vector3 rightControllerRotationOffset = new Vector3(263f, 318f, 485f);

	// Token: 0x04003664 RID: 13924
	[SerializeField]
	private bool toggleGrab;

	// Token: 0x04003667 RID: 13927
	private bool controlRightHand;

	// Token: 0x04003668 RID: 13928
	public LayerMask HandMask;

	// Token: 0x0200081C RID: 2076
	// (Invoke) Token: 0x060032EF RID: 13039
	public delegate void OnStateChangeEventHandler();
}
