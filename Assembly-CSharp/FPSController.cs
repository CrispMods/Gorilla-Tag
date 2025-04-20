using System;
using UnityEngine;

// Token: 0x02000835 RID: 2101
public class FPSController : MonoBehaviour
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x060033A4 RID: 13220 RVA: 0x0013BDFC File Offset: 0x00139FFC
	// (remove) Token: 0x060033A5 RID: 13221 RVA: 0x0013BE34 File Offset: 0x0013A034
	[HideInInspector]
	public event FPSController.OnStateChangeEventHandler OnStartEvent;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x060033A6 RID: 13222 RVA: 0x0013BE6C File Offset: 0x0013A06C
	// (remove) Token: 0x060033A7 RID: 13223 RVA: 0x0013BEA4 File Offset: 0x0013A0A4
	public event FPSController.OnStateChangeEventHandler OnStopEvent;

	// Token: 0x04003717 RID: 14103
	public float baseMoveSpeed = 4f;

	// Token: 0x04003718 RID: 14104
	public float shiftMoveSpeed = 8f;

	// Token: 0x04003719 RID: 14105
	public float ctrlMoveSpeed = 1f;

	// Token: 0x0400371A RID: 14106
	public float lookHorizontal = 0.4f;

	// Token: 0x0400371B RID: 14107
	public float lookVertical = 0.25f;

	// Token: 0x0400371C RID: 14108
	[SerializeField]
	private Vector3 leftControllerPosOffset = new Vector3(-0.2f, -0.25f, 0.3f);

	// Token: 0x0400371D RID: 14109
	[SerializeField]
	private Vector3 leftControllerRotationOffset = new Vector3(265f, -82f, 28f);

	// Token: 0x0400371E RID: 14110
	[SerializeField]
	private Vector3 rightControllerPosOffset = new Vector3(0.2f, -0.25f, 0.3f);

	// Token: 0x0400371F RID: 14111
	[SerializeField]
	private Vector3 rightControllerRotationOffset = new Vector3(263f, 318f, 485f);

	// Token: 0x04003720 RID: 14112
	[SerializeField]
	private bool toggleGrab;

	// Token: 0x04003723 RID: 14115
	private bool controlRightHand;

	// Token: 0x04003724 RID: 14116
	public LayerMask HandMask;

	// Token: 0x02000836 RID: 2102
	// (Invoke) Token: 0x060033AA RID: 13226
	public delegate void OnStateChangeEventHandler();
}
