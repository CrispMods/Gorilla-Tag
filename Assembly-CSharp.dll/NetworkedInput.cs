using System;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

// Token: 0x02000264 RID: 612
[NetworkInputWeaved(35)]
[StructLayout(LayoutKind.Explicit, Size = 140)]
public struct NetworkedInput : INetworkInput
{
	// Token: 0x04001101 RID: 4353
	[FieldOffset(0)]
	public Quaternion headRot_LS;

	// Token: 0x04001102 RID: 4354
	[FieldOffset(16)]
	public Vector3 rightHandPos_LS;

	// Token: 0x04001103 RID: 4355
	[FieldOffset(28)]
	public Quaternion rightHandRot_LS;

	// Token: 0x04001104 RID: 4356
	[FieldOffset(44)]
	public Vector3 leftHandPos_LS;

	// Token: 0x04001105 RID: 4357
	[FieldOffset(56)]
	public Quaternion leftHandRot_LS;

	// Token: 0x04001106 RID: 4358
	[FieldOffset(72)]
	public Vector3 rootPosition;

	// Token: 0x04001107 RID: 4359
	[FieldOffset(84)]
	public Quaternion rootRotation;

	// Token: 0x04001108 RID: 4360
	[FieldOffset(100)]
	public bool leftThumbTouch;

	// Token: 0x04001109 RID: 4361
	[FieldOffset(104)]
	public bool leftThumbPress;

	// Token: 0x0400110A RID: 4362
	[FieldOffset(108)]
	public float leftIndexValue;

	// Token: 0x0400110B RID: 4363
	[FieldOffset(112)]
	public float leftMiddleValue;

	// Token: 0x0400110C RID: 4364
	[FieldOffset(116)]
	public bool rightThumbTouch;

	// Token: 0x0400110D RID: 4365
	[FieldOffset(120)]
	public bool rightThumbPress;

	// Token: 0x0400110E RID: 4366
	[FieldOffset(124)]
	public float rightIndexValue;

	// Token: 0x0400110F RID: 4367
	[FieldOffset(128)]
	public float rightMiddleValue;

	// Token: 0x04001110 RID: 4368
	[FieldOffset(132)]
	public float scale;

	// Token: 0x04001111 RID: 4369
	[FieldOffset(136)]
	public int handPoseData;
}
