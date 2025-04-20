using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020006D7 RID: 1751
[Serializable]
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct m4x4
{
	// Token: 0x06002B6D RID: 11117 RVA: 0x00120618 File Offset: 0x0011E818
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

	// Token: 0x06002B6E RID: 11118 RVA: 0x0004D4D0 File Offset: 0x0004B6D0
	public m4x4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
	{
		this = default(m4x4);
		this.r0 = row0;
		this.r1 = row1;
		this.r2 = row2;
		this.r3 = row3;
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x001206AC File Offset: 0x0011E8AC
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

	// Token: 0x06002B70 RID: 11120 RVA: 0x0004D4F6 File Offset: 0x0004B6F6
	public void SetRow0(ref Vector4 v)
	{
		this.m00 = v.x;
		this.m01 = v.y;
		this.m02 = v.z;
		this.m03 = v.w;
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x0004D528 File Offset: 0x0004B728
	public void SetRow1(ref Vector4 v)
	{
		this.m10 = v.x;
		this.m11 = v.y;
		this.m12 = v.z;
		this.m13 = v.w;
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x0004D55A File Offset: 0x0004B75A
	public void SetRow2(ref Vector4 v)
	{
		this.m20 = v.x;
		this.m21 = v.y;
		this.m22 = v.z;
		this.m23 = v.w;
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x0004D58C File Offset: 0x0004B78C
	public void SetRow3(ref Vector4 v)
	{
		this.m30 = v.x;
		this.m31 = v.y;
		this.m32 = v.z;
		this.m33 = v.w;
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x0012076C File Offset: 0x0011E96C
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

	// Token: 0x06002B75 RID: 11125 RVA: 0x0004D5BE File Offset: 0x0004B7BE
	public void Set(ref Vector4 row0, ref Vector4 row1, ref Vector4 row2, ref Vector4 row3)
	{
		this.r0 = row0;
		this.r1 = row1;
		this.r2 = row2;
		this.r3 = row3;
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x00120834 File Offset: 0x0011EA34
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

	// Token: 0x06002B77 RID: 11127 RVA: 0x00120908 File Offset: 0x0011EB08
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

	// Token: 0x06002B78 RID: 11128 RVA: 0x001209D8 File Offset: 0x0011EBD8
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

	// Token: 0x06002B79 RID: 11129 RVA: 0x00120AA8 File Offset: 0x0011ECA8
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

	// Token: 0x06002B7A RID: 11130 RVA: 0x00120B78 File Offset: 0x0011ED78
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

	// Token: 0x06002B7B RID: 11131 RVA: 0x0004D5F1 File Offset: 0x0004B7F1
	public static ref m4x4 From(ref Matrix4x4 src)
	{
		return Unsafe.As<Matrix4x4, m4x4>(ref src);
	}

	// Token: 0x040030D3 RID: 12499
	[FixedBuffer(typeof(float), 16)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public m4x4.<data_f>e__FixedBuffer data_f;

	// Token: 0x040030D4 RID: 12500
	[FixedBuffer(typeof(int), 16)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public m4x4.<data_i>e__FixedBuffer data_i;

	// Token: 0x040030D5 RID: 12501
	[FixedBuffer(typeof(ushort), 32)]
	[NonSerialized]
	[FieldOffset(0)]
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public m4x4.<data_h>e__FixedBuffer data_h;

	// Token: 0x040030D6 RID: 12502
	[NonSerialized]
	[FieldOffset(0)]
	public Vector4 r0;

	// Token: 0x040030D7 RID: 12503
	[NonSerialized]
	[FieldOffset(16)]
	public Vector4 r1;

	// Token: 0x040030D8 RID: 12504
	[NonSerialized]
	[FieldOffset(32)]
	public Vector4 r2;

	// Token: 0x040030D9 RID: 12505
	[NonSerialized]
	[FieldOffset(48)]
	public Vector4 r3;

	// Token: 0x040030DA RID: 12506
	[NonSerialized]
	[FieldOffset(0)]
	public float m00;

	// Token: 0x040030DB RID: 12507
	[NonSerialized]
	[FieldOffset(4)]
	public float m01;

	// Token: 0x040030DC RID: 12508
	[NonSerialized]
	[FieldOffset(8)]
	public float m02;

	// Token: 0x040030DD RID: 12509
	[NonSerialized]
	[FieldOffset(12)]
	public float m03;

	// Token: 0x040030DE RID: 12510
	[NonSerialized]
	[FieldOffset(16)]
	public float m10;

	// Token: 0x040030DF RID: 12511
	[NonSerialized]
	[FieldOffset(20)]
	public float m11;

	// Token: 0x040030E0 RID: 12512
	[NonSerialized]
	[FieldOffset(24)]
	public float m12;

	// Token: 0x040030E1 RID: 12513
	[NonSerialized]
	[FieldOffset(28)]
	public float m13;

	// Token: 0x040030E2 RID: 12514
	[NonSerialized]
	[FieldOffset(32)]
	public float m20;

	// Token: 0x040030E3 RID: 12515
	[NonSerialized]
	[FieldOffset(36)]
	public float m21;

	// Token: 0x040030E4 RID: 12516
	[NonSerialized]
	[FieldOffset(40)]
	public float m22;

	// Token: 0x040030E5 RID: 12517
	[NonSerialized]
	[FieldOffset(44)]
	public float m23;

	// Token: 0x040030E6 RID: 12518
	[NonSerialized]
	[FieldOffset(48)]
	public float m30;

	// Token: 0x040030E7 RID: 12519
	[NonSerialized]
	[FieldOffset(52)]
	public float m31;

	// Token: 0x040030E8 RID: 12520
	[NonSerialized]
	[FieldOffset(56)]
	public float m32;

	// Token: 0x040030E9 RID: 12521
	[NonSerialized]
	[FieldOffset(60)]
	public float m33;

	// Token: 0x040030EA RID: 12522
	[HideInInspector]
	[FieldOffset(0)]
	public int i00;

	// Token: 0x040030EB RID: 12523
	[HideInInspector]
	[FieldOffset(4)]
	public int i01;

	// Token: 0x040030EC RID: 12524
	[HideInInspector]
	[FieldOffset(8)]
	public int i02;

	// Token: 0x040030ED RID: 12525
	[HideInInspector]
	[FieldOffset(12)]
	public int i03;

	// Token: 0x040030EE RID: 12526
	[HideInInspector]
	[FieldOffset(16)]
	public int i10;

	// Token: 0x040030EF RID: 12527
	[HideInInspector]
	[FieldOffset(20)]
	public int i11;

	// Token: 0x040030F0 RID: 12528
	[HideInInspector]
	[FieldOffset(24)]
	public int i12;

	// Token: 0x040030F1 RID: 12529
	[HideInInspector]
	[FieldOffset(28)]
	public int i13;

	// Token: 0x040030F2 RID: 12530
	[HideInInspector]
	[FieldOffset(32)]
	public int i20;

	// Token: 0x040030F3 RID: 12531
	[HideInInspector]
	[FieldOffset(36)]
	public int i21;

	// Token: 0x040030F4 RID: 12532
	[HideInInspector]
	[FieldOffset(40)]
	public int i22;

	// Token: 0x040030F5 RID: 12533
	[HideInInspector]
	[FieldOffset(44)]
	public int i23;

	// Token: 0x040030F6 RID: 12534
	[HideInInspector]
	[FieldOffset(48)]
	public int i30;

	// Token: 0x040030F7 RID: 12535
	[HideInInspector]
	[FieldOffset(52)]
	public int i31;

	// Token: 0x040030F8 RID: 12536
	[HideInInspector]
	[FieldOffset(56)]
	public int i32;

	// Token: 0x040030F9 RID: 12537
	[HideInInspector]
	[FieldOffset(60)]
	public int i33;

	// Token: 0x040030FA RID: 12538
	[NonSerialized]
	[FieldOffset(0)]
	public ushort h00_a;

	// Token: 0x040030FB RID: 12539
	[NonSerialized]
	[FieldOffset(2)]
	public ushort h00_b;

	// Token: 0x040030FC RID: 12540
	[NonSerialized]
	[FieldOffset(4)]
	public ushort h01_a;

	// Token: 0x040030FD RID: 12541
	[NonSerialized]
	[FieldOffset(6)]
	public ushort h01_b;

	// Token: 0x040030FE RID: 12542
	[NonSerialized]
	[FieldOffset(8)]
	public ushort h02_a;

	// Token: 0x040030FF RID: 12543
	[NonSerialized]
	[FieldOffset(10)]
	public ushort h02_b;

	// Token: 0x04003100 RID: 12544
	[NonSerialized]
	[FieldOffset(12)]
	public ushort h03_a;

	// Token: 0x04003101 RID: 12545
	[NonSerialized]
	[FieldOffset(14)]
	public ushort h03_b;

	// Token: 0x04003102 RID: 12546
	[NonSerialized]
	[FieldOffset(16)]
	public ushort h10_a;

	// Token: 0x04003103 RID: 12547
	[NonSerialized]
	[FieldOffset(18)]
	public ushort h10_b;

	// Token: 0x04003104 RID: 12548
	[NonSerialized]
	[FieldOffset(20)]
	public ushort h11_a;

	// Token: 0x04003105 RID: 12549
	[NonSerialized]
	[FieldOffset(22)]
	public ushort h11_b;

	// Token: 0x04003106 RID: 12550
	[NonSerialized]
	[FieldOffset(24)]
	public ushort h12_a;

	// Token: 0x04003107 RID: 12551
	[NonSerialized]
	[FieldOffset(26)]
	public ushort h12_b;

	// Token: 0x04003108 RID: 12552
	[NonSerialized]
	[FieldOffset(28)]
	public ushort h13_a;

	// Token: 0x04003109 RID: 12553
	[NonSerialized]
	[FieldOffset(30)]
	public ushort h13_b;

	// Token: 0x0400310A RID: 12554
	[NonSerialized]
	[FieldOffset(32)]
	public ushort h20_a;

	// Token: 0x0400310B RID: 12555
	[NonSerialized]
	[FieldOffset(34)]
	public ushort h20_b;

	// Token: 0x0400310C RID: 12556
	[NonSerialized]
	[FieldOffset(36)]
	public ushort h21_a;

	// Token: 0x0400310D RID: 12557
	[NonSerialized]
	[FieldOffset(38)]
	public ushort h21_b;

	// Token: 0x0400310E RID: 12558
	[NonSerialized]
	[FieldOffset(40)]
	public ushort h22_a;

	// Token: 0x0400310F RID: 12559
	[NonSerialized]
	[FieldOffset(42)]
	public ushort h22_b;

	// Token: 0x04003110 RID: 12560
	[NonSerialized]
	[FieldOffset(44)]
	public ushort h23_a;

	// Token: 0x04003111 RID: 12561
	[NonSerialized]
	[FieldOffset(46)]
	public ushort h23_b;

	// Token: 0x04003112 RID: 12562
	[NonSerialized]
	[FieldOffset(48)]
	public ushort h30_a;

	// Token: 0x04003113 RID: 12563
	[NonSerialized]
	[FieldOffset(50)]
	public ushort h30_b;

	// Token: 0x04003114 RID: 12564
	[NonSerialized]
	[FieldOffset(52)]
	public ushort h31_a;

	// Token: 0x04003115 RID: 12565
	[NonSerialized]
	[FieldOffset(54)]
	public ushort h31_b;

	// Token: 0x04003116 RID: 12566
	[NonSerialized]
	[FieldOffset(56)]
	public ushort h32_a;

	// Token: 0x04003117 RID: 12567
	[NonSerialized]
	[FieldOffset(58)]
	public ushort h32_b;

	// Token: 0x04003118 RID: 12568
	[NonSerialized]
	[FieldOffset(60)]
	public ushort h33_a;

	// Token: 0x04003119 RID: 12569
	[NonSerialized]
	[FieldOffset(62)]
	public ushort h33_b;

	// Token: 0x020006D8 RID: 1752
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_f>e__FixedBuffer
	{
		// Token: 0x0400311A RID: 12570
		public float FixedElementField;
	}

	// Token: 0x020006D9 RID: 1753
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_h>e__FixedBuffer
	{
		// Token: 0x0400311B RID: 12571
		public ushort FixedElementField;
	}

	// Token: 0x020006DA RID: 1754
	[CompilerGenerated]
	[UnsafeValueType]
	[StructLayout(LayoutKind.Sequential, Size = 64)]
	public struct <data_i>e__FixedBuffer
	{
		// Token: 0x0400311C RID: 12572
		public int FixedElementField;
	}
}
