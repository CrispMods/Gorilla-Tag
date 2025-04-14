using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct m4x4
{
	// Token: 0x06002ADF RID: 10975 RVA: 0x000D4B4C File Offset: 0x000D2D4C
	public m4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
	{
		this = default(m4x4);
		this.m00 = m00;
		this.m01 = m01;
		this.m02 = m02;
		this.m03 = m03;
		this.m10 = m10;
		this.m11 = m11;
		this.m12 = m12;
		this.m13 = m13;
		this.m20 = m20;
		this.m21 = m21;
		this.m22 = m22;
		this.m23 = m23;
		this.m30 = m30;
		this.m31 = m31;
		this.m32 = m32;
		this.m33 = m33;
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000D4BDD File Offset: 0x000D2DDD
	public m4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
	{
		this = default(m4x4);
		this.r0 = row0;
		this.r1 = row1;
		this.r2 = row2;
		this.r3 = row3;
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x000D4C04 File Offset: 0x000D2E04
	public void Clear()
	{
		this.m00 = 0f;
		this.m01 = 0f;
		this.m02 = 0f;
		this.m03 = 0f;
		this.m10 = 0f;
		this.m11 = 0f;
		this.m12 = 0f;
		this.m13 = 0f;
		this.m20 = 0f;
		this.m21 = 0f;
		this.m22 = 0f;
		this.m23 = 0f;
		this.m30 = 0f;
		this.m31 = 0f;
		this.m32 = 0f;
		this.m33 = 0f;
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x000D4CC1 File Offset: 0x000D2EC1
	public void SetRow0(ref Vector4 v)
	{
		this.m00 = v.x;
		this.m01 = v.y;
		this.m02 = v.z;
		this.m03 = v.w;
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000D4CF3 File Offset: 0x000D2EF3
	public void SetRow1(ref Vector4 v)
	{
		this.m10 = v.x;
		this.m11 = v.y;
		this.m12 = v.z;
		this.m13 = v.w;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x000D4D25 File Offset: 0x000D2F25
	public void SetRow2(ref Vector4 v)
	{
		this.m20 = v.x;
		this.m21 = v.y;
		this.m22 = v.z;
		this.m23 = v.w;
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000D4D57 File Offset: 0x000D2F57
	public void SetRow3(ref Vector4 v)
	{
		this.m30 = v.x;
		this.m31 = v.y;
		this.m32 = v.z;
		this.m33 = v.w;
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000D4D8C File Offset: 0x000D2F8C
	public void Transpose()
	{
		float num = this.m01;
		float num2 = this.m02;
		float num3 = this.m03;
		float num4 = this.m10;
		float num5 = this.m12;
		float num6 = this.m13;
		float num7 = this.m20;
		float num8 = this.m21;
		float num9 = this.m23;
		float num10 = this.m30;
		float num11 = this.m31;
		float num12 = this.m32;
		this.m01 = num4;
		this.m02 = num7;
		this.m03 = num10;
		this.m10 = num;
		this.m12 = num8;
		this.m13 = num11;
		this.m20 = num2;
		this.m21 = num5;
		this.m23 = num12;
		this.m30 = num3;
		this.m31 = num6;
		this.m32 = num9;
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x000D4E51 File Offset: 0x000D3051
	public void Set(ref Vector4 row0, ref Vector4 row1, ref Vector4 row2, ref Vector4 row3)
	{
		this.r0 = row0;
		this.r1 = row1;
		this.r2 = row2;
		this.r3 = row3;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000D4E84 File Offset: 0x000D3084
	public void SetTransposed(ref Vector4 row0, ref Vector4 row1, ref Vector4 row2, ref Vector4 row3)
	{
		this.m00 = row0.x;
		this.m01 = row1.x;
		this.m02 = row2.x;
		this.m03 = row3.x;
		this.m10 = row0.y;
		this.m11 = row1.y;
		this.m12 = row2.y;
		this.m13 = row3.y;
		this.m20 = row0.z;
		this.m21 = row1.z;
		this.m22 = row2.z;
		this.m23 = row3.z;
		this.m30 = row0.w;
		this.m31 = row1.w;
		this.m32 = row2.w;
		this.m33 = row3.w;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000D4F58 File Offset: 0x000D3158
	public void Set(ref Matrix4x4 x)
	{
		this.m00 = x.m00;
		this.m01 = x.m01;
		this.m02 = x.m02;
		this.m03 = x.m03;
		this.m10 = x.m10;
		this.m11 = x.m11;
		this.m12 = x.m12;
		this.m13 = x.m13;
		this.m20 = x.m20;
		this.m21 = x.m21;
		this.m22 = x.m22;
		this.m23 = x.m23;
		this.m30 = x.m30;
		this.m31 = x.m31;
		this.m32 = x.m32;
		this.m33 = x.m33;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x000D5028 File Offset: 0x000D3228
	public void SetTransposed(ref Matrix4x4 x)
	{
		this.m00 = x.m00;
		this.m01 = x.m10;
		this.m02 = x.m20;
		this.m03 = x.m30;
		this.m10 = x.m01;
		this.m11 = x.m11;
		this.m12 = x.m21;
		this.m13 = x.m31;
		this.m20 = x.m02;
		this.m21 = x.m12;
		this.m22 = x.m22;
		this.m23 = x.m32;
		this.m30 = x.m03;
		this.m31 = x.m13;
		this.m32 = x.m23;
		this.m33 = x.m33;
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000D50F8 File Offset: 0x000D32F8
	public void Push(ref Matrix4x4 x)
	{
		x.m00 = this.m00;
		x.m01 = this.m01;
		x.m02 = this.m02;
		x.m03 = this.m03;
		x.m10 = this.m10;
		x.m11 = this.m11;
		x.m12 = this.m12;
		x.m13 = this.m13;
		x.m20 = this.m20;
		x.m21 = this.m21;
		x.m22 = this.m22;
		x.m23 = this.m23;
		x.m30 = this.m30;
		x.m31 = this.m31;
		x.m32 = this.m32;
		x.m33 = this.m33;
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x000D51C8 File Offset: 0x000D33C8
	public void PushTransposed(ref Matrix4x4 x)
	{
		x.m00 = this.m00;
		x.m01 = this.m10;
		x.m02 = this.m20;
		x.m03 = this.m30;
		x.m10 = this.m01;
		x.m11 = this.m11;
		x.m12 = this.m21;
		x.m13 = this.m31;
		x.m20 = this.m02;
		x.m21 = this.m12;
		x.m22 = this.m22;
		x.m23 = this.m32;
		x.m30 = this.m03;
		x.m31 = this.m13;
		x.m32 = this.m23;
		x.m33 = this.m33;
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x000D5295 File Offset: 0x000D3495
	public static ref m4x4 From(ref Matrix4x4 src)
	{
		return Unsafe.As<Matrix4x4, m4x4>(ref src);
	}

	// Token: 0x0400303C RID: 12348
	[FixedBuffer(typeof(float), 16)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public m4x4.<data_f>e__FixedBuffer data_f;

	// Token: 0x0400303D RID: 12349
	[FixedBuffer(typeof(int), 16)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public m4x4.<data_i>e__FixedBuffer data_i;

	// Token: 0x0400303E RID: 12350
	[FixedBuffer(typeof(ushort), 32)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public m4x4.<data_h>e__FixedBuffer data_h;

	// Token: 0x0400303F RID: 12351
	[NonSerialized]
	[FieldOffset(0)]
	public Vector4 r0;

	// Token: 0x04003040 RID: 12352
	[NonSerialized]
	[FieldOffset(16)]
	public Vector4 r1;

	// Token: 0x04003041 RID: 12353
	[NonSerialized]
	[FieldOffset(32)]
	public Vector4 r2;

	// Token: 0x04003042 RID: 12354
	[NonSerialized]
	[FieldOffset(48)]
	public Vector4 r3;

	// Token: 0x04003043 RID: 12355
	[NonSerialized]
	[FieldOffset(0)]
	public float m00;

	// Token: 0x04003044 RID: 12356
	[NonSerialized]
	[FieldOffset(4)]
	public float m01;

	// Token: 0x04003045 RID: 12357
	[NonSerialized]
	[FieldOffset(8)]
	public float m02;

	// Token: 0x04003046 RID: 12358
	[NonSerialized]
	[FieldOffset(12)]
	public float m03;

	// Token: 0x04003047 RID: 12359
	[NonSerialized]
	[FieldOffset(16)]
	public float m10;

	// Token: 0x04003048 RID: 12360
	[NonSerialized]
	[FieldOffset(20)]
	public float m11;

	// Token: 0x04003049 RID: 12361
	[NonSerialized]
	[FieldOffset(24)]
	public float m12;

	// Token: 0x0400304A RID: 12362
	[NonSerialized]
	[FieldOffset(28)]
	public float m13;

	// Token: 0x0400304B RID: 12363
	[NonSerialized]
	[FieldOffset(32)]
	public float m20;

	// Token: 0x0400304C RID: 12364
	[NonSerialized]
	[FieldOffset(36)]
	public float m21;

	// Token: 0x0400304D RID: 12365
	[NonSerialized]
	[FieldOffset(40)]
	public float m22;

	// Token: 0x0400304E RID: 12366
	[NonSerialized]
	[FieldOffset(44)]
	public float m23;

	// Token: 0x0400304F RID: 12367
	[NonSerialized]
	[FieldOffset(48)]
	public float m30;

	// Token: 0x04003050 RID: 12368
	[NonSerialized]
	[FieldOffset(52)]
	public float m31;

	// Token: 0x04003051 RID: 12369
	[NonSerialized]
	[FieldOffset(56)]
	public float m32;

	// Token: 0x04003052 RID: 12370
	[NonSerialized]
	[FieldOffset(60)]
	public float m33;

	// Token: 0x04003053 RID: 12371
	[HideInInspector]
	[FieldOffset(0)]
	public int i00;

	// Token: 0x04003054 RID: 12372
	[HideInInspector]
	[FieldOffset(4)]
	public int i01;

	// Token: 0x04003055 RID: 12373
	[HideInInspector]
	[FieldOffset(8)]
	public int i02;

	// Token: 0x04003056 RID: 12374
	[HideInInspector]
	[FieldOffset(12)]
	public int i03;

	// Token: 0x04003057 RID: 12375
	[HideInInspector]
	[FieldOffset(16)]
	public int i10;

	// Token: 0x04003058 RID: 12376
	[HideInInspector]
	[FieldOffset(20)]
	public int i11;

	// Token: 0x04003059 RID: 12377
	[HideInInspector]
	[FieldOffset(24)]
	public int i12;

	// Token: 0x0400305A RID: 12378
	[HideInInspector]
	[FieldOffset(28)]
	public int i13;

	// Token: 0x0400305B RID: 12379
	[HideInInspector]
	[FieldOffset(32)]
	public int i20;

	// Token: 0x0400305C RID: 12380
	[HideInInspector]
	[FieldOffset(36)]
	public int i21;

	// Token: 0x0400305D RID: 12381
	[HideInInspector]
	[FieldOffset(40)]
	public int i22;

	// Token: 0x0400305E RID: 12382
	[HideInInspector]
	[FieldOffset(44)]
	public int i23;

	// Token: 0x0400305F RID: 12383
	[HideInInspector]
	[FieldOffset(48)]
	public int i30;

	// Token: 0x04003060 RID: 12384
	[HideInInspector]
	[FieldOffset(52)]
	public int i31;

	// Token: 0x04003061 RID: 12385
	[HideInInspector]
	[FieldOffset(56)]
	public int i32;

	// Token: 0x04003062 RID: 12386
	[HideInInspector]
	[FieldOffset(60)]
	public int i33;

	// Token: 0x04003063 RID: 12387
	[NonSerialized]
	[FieldOffset(0)]
	public ushort h00_a;

	// Token: 0x04003064 RID: 12388
	[NonSerialized]
	[FieldOffset(2)]
	public ushort h00_b;

	// Token: 0x04003065 RID: 12389
	[NonSerialized]
	[FieldOffset(4)]
	public ushort h01_a;

	// Token: 0x04003066 RID: 12390
	[NonSerialized]
	[FieldOffset(6)]
	public ushort h01_b;

	// Token: 0x04003067 RID: 12391
	[NonSerialized]
	[FieldOffset(8)]
	public ushort h02_a;

	// Token: 0x04003068 RID: 12392
	[NonSerialized]
	[FieldOffset(10)]
	public ushort h02_b;

	// Token: 0x04003069 RID: 12393
	[NonSerialized]
	[FieldOffset(12)]
	public ushort h03_a;

	// Token: 0x0400306A RID: 12394
	[NonSerialized]
	[FieldOffset(14)]
	public ushort h03_b;

	// Token: 0x0400306B RID: 12395
	[NonSerialized]
	[FieldOffset(16)]
	public ushort h10_a;

	// Token: 0x0400306C RID: 12396
	[NonSerialized]
	[FieldOffset(18)]
	public ushort h10_b;

	// Token: 0x0400306D RID: 12397
	[NonSerialized]
	[FieldOffset(20)]
	public ushort h11_a;

	// Token: 0x0400306E RID: 12398
	[NonSerialized]
	[FieldOffset(22)]
	public ushort h11_b;

	// Token: 0x0400306F RID: 12399
	[NonSerialized]
	[FieldOffset(24)]
	public ushort h12_a;

	// Token: 0x04003070 RID: 12400
	[NonSerialized]
	[FieldOffset(26)]
	public ushort h12_b;

	// Token: 0x04003071 RID: 12401
	[NonSerialized]
	[FieldOffset(28)]
	public ushort h13_a;

	// Token: 0x04003072 RID: 12402
	[NonSerialized]
	[FieldOffset(30)]
	public ushort h13_b;

	// Token: 0x04003073 RID: 12403
	[NonSerialized]
	[FieldOffset(32)]
	public ushort h20_a;

	// Token: 0x04003074 RID: 12404
	[NonSerialized]
	[FieldOffset(34)]
	public ushort h20_b;

	// Token: 0x04003075 RID: 12405
	[NonSerialized]
	[FieldOffset(36)]
	public ushort h21_a;

	// Token: 0x04003076 RID: 12406
	[NonSerialized]
	[FieldOffset(38)]
	public ushort h21_b;

	// Token: 0x04003077 RID: 12407
	[NonSerialized]
	[FieldOffset(40)]
	public ushort h22_a;

	// Token: 0x04003078 RID: 12408
	[NonSerialized]
	[FieldOffset(42)]
	public ushort h22_b;

	// Token: 0x04003079 RID: 12409
	[NonSerialized]
	[FieldOffset(44)]
	public ushort h23_a;

	// Token: 0x0400307A RID: 12410
	[NonSerialized]
	[FieldOffset(46)]
	public ushort h23_b;

	// Token: 0x0400307B RID: 12411
	[NonSerialized]
	[FieldOffset(48)]
	public ushort h30_a;

	// Token: 0x0400307C RID: 12412
	[NonSerialized]
	[FieldOffset(50)]
	public ushort h30_b;

	// Token: 0x0400307D RID: 12413
	[NonSerialized]
	[FieldOffset(52)]
	public ushort h31_a;

	// Token: 0x0400307E RID: 12414
	[NonSerialized]
	[FieldOffset(54)]
	public ushort h31_b;

	// Token: 0x0400307F RID: 12415
	[NonSerialized]
	[FieldOffset(56)]
	public ushort h32_a;

	// Token: 0x04003080 RID: 12416
	[NonSerialized]
	[FieldOffset(58)]
	public ushort h32_b;

	// Token: 0x04003081 RID: 12417
	[NonSerialized]
	[FieldOffset(60)]
	public ushort h33_a;

	// Token: 0x04003082 RID: 12418
	[NonSerialized]
	[FieldOffset(62)]
	public ushort h33_b;

	// Token: 0x020006C4 RID: 1732
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_f>e__FixedBuffer
	{
		// Token: 0x04003083 RID: 12419
		public float FixedElementField;
	}

	// Token: 0x020006C5 RID: 1733
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_h>e__FixedBuffer
	{
		// Token: 0x04003084 RID: 12420
		public ushort FixedElementField;
	}

	// Token: 0x020006C6 RID: 1734
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_i>e__FixedBuffer
	{
		// Token: 0x04003085 RID: 12421
		public int FixedElementField;
	}
}
