using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020001AC RID: 428
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public class GTContactPoint
{
	// Token: 0x04000C64 RID: 3172
	[NonSerialized]
	[FieldOffset(0)]
	public Matrix4x4 data;

	// Token: 0x04000C65 RID: 3173
	[NonSerialized]
	[FieldOffset(0)]
	public Vector4 data0;

	// Token: 0x04000C66 RID: 3174
	[NonSerialized]
	[FieldOffset(16)]
	public Vector4 data1;

	// Token: 0x04000C67 RID: 3175
	[NonSerialized]
	[FieldOffset(32)]
	public Vector4 data2;

	// Token: 0x04000C68 RID: 3176
	[NonSerialized]
	[FieldOffset(48)]
	public Vector4 data3;

	// Token: 0x04000C69 RID: 3177
	[FieldOffset(0)]
	public Vector3 contactPoint;

	// Token: 0x04000C6A RID: 3178
	[FieldOffset(12)]
	public float radius;

	// Token: 0x04000C6B RID: 3179
	[FieldOffset(16)]
	public Vector3 counterVelocity;

	// Token: 0x04000C6C RID: 3180
	[FieldOffset(28)]
	public float timestamp;

	// Token: 0x04000C6D RID: 3181
	[FieldOffset(32)]
	public Color color;

	// Token: 0x04000C6E RID: 3182
	[FieldOffset(48)]
	public GTContactType contactType;

	// Token: 0x04000C6F RID: 3183
	[FieldOffset(52)]
	public float lifetime = 1f;

	// Token: 0x04000C70 RID: 3184
	[FieldOffset(56)]
	public uint free = 1U;
}
