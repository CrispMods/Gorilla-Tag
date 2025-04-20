using System;
using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

// Token: 0x02000286 RID: 646
[NetworkStructWeaved(27)]
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 108)]
public struct InputStruct : INetworkStruct
{
	// Token: 0x040011E1 RID: 4577
	[FieldOffset(0)]
	public int headRotation;

	// Token: 0x040011E2 RID: 4578
	[FieldOffset(4)]
	public long rightHandLong;

	// Token: 0x040011E3 RID: 4579
	[FieldOffset(12)]
	public long leftHandLong;

	// Token: 0x040011E4 RID: 4580
	[FieldOffset(20)]
	public long position;

	// Token: 0x040011E5 RID: 4581
	[FieldOffset(28)]
	public int handPosition;

	// Token: 0x040011E6 RID: 4582
	[FieldOffset(32)]
	public int packedFields;

	// Token: 0x040011E7 RID: 4583
	[FieldOffset(36)]
	public short packedCompetitiveData;

	// Token: 0x040011E8 RID: 4584
	[FieldOffset(40)]
	public Vector3 velocity;

	// Token: 0x040011E9 RID: 4585
	[FieldOffset(52)]
	public int grabbedRopeIndex;

	// Token: 0x040011EA RID: 4586
	[FieldOffset(56)]
	public int ropeBoneIndex;

	// Token: 0x040011EB RID: 4587
	[FieldOffset(60)]
	public bool ropeGrabIsLeft;

	// Token: 0x040011EC RID: 4588
	[FieldOffset(64)]
	public bool ropeGrabIsBody;

	// Token: 0x040011ED RID: 4589
	[FieldOffset(68)]
	public Vector3 ropeGrabOffset;

	// Token: 0x040011EE RID: 4590
	[FieldOffset(80)]
	public bool movingSurfaceIsMonkeBlock;

	// Token: 0x040011EF RID: 4591
	[FieldOffset(84)]
	public long hoverboardPosRot;

	// Token: 0x040011F0 RID: 4592
	[FieldOffset(92)]
	public short hoverboardColor;

	// Token: 0x040011F1 RID: 4593
	[FieldOffset(96)]
	public double serverTimeStamp;

	// Token: 0x040011F2 RID: 4594
	[FieldOffset(104)]
	public short taggedById;
}
