using System;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

// Token: 0x0200027B RID: 635
[NetworkStructWeaved(27)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 108)]
public struct InputStruct : INetworkStruct
{
	// Token: 0x0400119B RID: 4507
	[FieldOffset(0)]
	public int headRotation;

	// Token: 0x0400119C RID: 4508
	[FieldOffset(4)]
	public long rightHandLong;

	// Token: 0x0400119D RID: 4509
	[FieldOffset(12)]
	public long leftHandLong;

	// Token: 0x0400119E RID: 4510
	[FieldOffset(20)]
	public long position;

	// Token: 0x0400119F RID: 4511
	[FieldOffset(28)]
	public int handPosition;

	// Token: 0x040011A0 RID: 4512
	[FieldOffset(32)]
	public int packedFields;

	// Token: 0x040011A1 RID: 4513
	[FieldOffset(36)]
	public short packedCompetitiveData;

	// Token: 0x040011A2 RID: 4514
	[FieldOffset(40)]
	public Vector3 velocity;

	// Token: 0x040011A3 RID: 4515
	[FieldOffset(52)]
	public int grabbedRopeIndex;

	// Token: 0x040011A4 RID: 4516
	[FieldOffset(56)]
	public int ropeBoneIndex;

	// Token: 0x040011A5 RID: 4517
	[FieldOffset(60)]
	public bool ropeGrabIsLeft;

	// Token: 0x040011A6 RID: 4518
	[FieldOffset(64)]
	public bool ropeGrabIsBody;

	// Token: 0x040011A7 RID: 4519
	[FieldOffset(68)]
	public Vector3 ropeGrabOffset;

	// Token: 0x040011A8 RID: 4520
	[FieldOffset(80)]
	public bool movingSurfaceIsMonkeBlock;

	// Token: 0x040011A9 RID: 4521
	[FieldOffset(84)]
	public long hoverboardPosRot;

	// Token: 0x040011AA RID: 4522
	[FieldOffset(92)]
	public short hoverboardColor;

	// Token: 0x040011AB RID: 4523
	[FieldOffset(96)]
	public double serverTimeStamp;

	// Token: 0x040011AC RID: 4524
	[FieldOffset(104)]
	public short taggedById;
}
