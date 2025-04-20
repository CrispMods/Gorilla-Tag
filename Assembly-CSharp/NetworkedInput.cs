using System;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

// Token: 0x0200026F RID: 623
[NetworkInputWeaved(35)]
[StructLayout(LayoutKind.Explicit, Size = 140)]
public struct NetworkedInput : INetworkInput
{
	// Token: 0x04001146 RID: 4422
	[FieldOffset(0)]
	public Quaternion headRot_LS;

	// Token: 0x04001147 RID: 4423
	[FieldOffset(16)]
	public Vector3 rightHandPos_LS;

	// Token: 0x04001148 RID: 4424
	[FieldOffset(28)]
	public Quaternion rightHandRot_LS;

	// Token: 0x04001149 RID: 4425
	[FieldOffset(44)]
	public Vector3 leftHandPos_LS;

	// Token: 0x0400114A RID: 4426
	[FieldOffset(56)]
	public Quaternion leftHandRot_LS;

	// Token: 0x0400114B RID: 4427
	[FieldOffset(72)]
	public Vector3 rootPosition;

	// Token: 0x0400114C RID: 4428
	[FieldOffset(84)]
	public Quaternion rootRotation;

	// Token: 0x0400114D RID: 4429
	[FieldOffset(100)]
	public bool leftThumbTouch;

	// Token: 0x0400114E RID: 4430
	[FieldOffset(104)]
	public bool leftThumbPress;

	// Token: 0x0400114F RID: 4431
	[FieldOffset(108)]
	public float leftIndexValue;

	// Token: 0x04001150 RID: 4432
	[FieldOffset(112)]
	public float leftMiddleValue;

	// Token: 0x04001151 RID: 4433
	[FieldOffset(116)]
	public bool rightThumbTouch;

	// Token: 0x04001152 RID: 4434
	[FieldOffset(120)]
	public bool rightThumbPress;

	// Token: 0x04001153 RID: 4435
	[FieldOffset(124)]
	public float rightIndexValue;

	// Token: 0x04001154 RID: 4436
	[FieldOffset(128)]
	public float rightMiddleValue;

	// Token: 0x04001155 RID: 4437
	[FieldOffset(132)]
	public float scale;

	// Token: 0x04001156 RID: 4438
	[FieldOffset(136)]
	public int handPoseData;
}
