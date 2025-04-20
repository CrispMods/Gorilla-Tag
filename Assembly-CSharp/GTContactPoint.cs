using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020001B7 RID: 439
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public class GTContactPoint
{
	// Token: 0x04000CAA RID: 3242
	[NonSerialized]
	[FieldOffset(0)]
	public Matrix4x4 data;

	// Token: 0x04000CAB RID: 3243
	[NonSerialized]
	[FieldOffset(0)]
	public Vector4 data0;

	// Token: 0x04000CAC RID: 3244
	[NonSerialized]
	[FieldOffset(16)]
	public Vector4 data1;

	// Token: 0x04000CAD RID: 3245
	[NonSerialized]
	[FieldOffset(32)]
	public Vector4 data2;

	// Token: 0x04000CAE RID: 3246
	[NonSerialized]
	[FieldOffset(48)]
	public Vector4 data3;

	// Token: 0x04000CAF RID: 3247
	[FieldOffset(0)]
	public Vector3 contactPoint;

	// Token: 0x04000CB0 RID: 3248
	[FieldOffset(12)]
	public float radius;

	// Token: 0x04000CB1 RID: 3249
	[FieldOffset(16)]
	public Vector3 counterVelocity;

	// Token: 0x04000CB2 RID: 3250
	[FieldOffset(28)]
	public float timestamp;

	// Token: 0x04000CB3 RID: 3251
	[FieldOffset(32)]
	public Color color;

	// Token: 0x04000CB4 RID: 3252
	[FieldOffset(48)]
	public GTContactType contactType;

	// Token: 0x04000CB5 RID: 3253
	[FieldOffset(52)]
	public float lifetime = 1f;

	// Token: 0x04000CB6 RID: 3254
	[FieldOffset(56)]
	public uint free = 1U;
}
