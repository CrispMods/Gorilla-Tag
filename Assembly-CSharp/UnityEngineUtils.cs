using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020008AB RID: 2219
public static class UnityEngineUtils
{
	// Token: 0x060035DA RID: 13786 RVA: 0x000535F0 File Offset: 0x000517F0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsColor(this Color32 c, Color32 other)
	{
		return c.r == other.r && c.g == other.g && c.b == other.b && c.a == other.a;
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00143A70 File Offset: 0x00141C70
	public static Color32 IdToColor32(this UnityEngine.Object obj, int alpha = -1, bool distinct = true)
	{
		if (!(obj == null))
		{
			return obj.GetInstanceID().IdToColor32(alpha, distinct);
		}
		return default(Color32);
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x00143AA0 File Offset: 0x00141CA0
	public unsafe static Color32 IdToColor32(this int id, int alpha = -1, bool distinct = true)
	{
		if (distinct)
		{
			id = StaticHash.ComputeTriple32(id);
		}
		Color32 result = *Unsafe.As<int, Color32>(ref id);
		if (alpha > -1)
		{
			result.a = (byte)Math.Clamp(alpha, 0, 255);
		}
		return result;
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x00143AE0 File Offset: 0x00141CE0
	public static Color32 ToHighViz(this Color32 c)
	{
		float h;
		float num;
		float num2;
		Color.RGBToHSV(c, out h, out num, out num2);
		return Color.HSVToRGB(h, 1f, 1f);
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x00143B14 File Offset: 0x00141D14
	public unsafe static int Color32ToId(this Color32 c, bool distinct = true)
	{
		int num = *Unsafe.As<Color32, int>(ref c);
		if (distinct)
		{
			num = StaticHash.ReverseTriple32(num);
		}
		return num;
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x00143B38 File Offset: 0x00141D38
	public static Hash128 QuantizedHash128(this Matrix4x4 m)
	{
		Hash128 result = default(Hash128);
		HashUtilities.QuantisedMatrixHash(ref m, ref result);
		return result;
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x00143B58 File Offset: 0x00141D58
	public static Hash128 QuantizedHash128(this Vector3 v)
	{
		Hash128 result = default(Hash128);
		HashUtilities.QuantisedVectorHash(ref v, ref result);
		return result;
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x0005362C File Offset: 0x0005182C
	public static Id128 QuantizedId128(this Vector3 v)
	{
		return v.QuantizedHash128();
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00053639 File Offset: 0x00051839
	public static Id128 QuantizedId128(this Matrix4x4 m)
	{
		return m.QuantizedHash128();
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x00143B78 File Offset: 0x00141D78
	public static Id128 QuantizedId128(this Quaternion q)
	{
		int a = (int)((double)q.x * 1000.0 + 0.5);
		int b = (int)((double)q.y * 1000.0 + 0.5);
		int c = (int)((double)q.z * 1000.0 + 0.5);
		int d = (int)((double)q.w * 1000.0 + 0.5);
		return new Id128(a, b, c, d);
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00143C00 File Offset: 0x00141E00
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long QuantizedHash64(this Vector4 v)
	{
		int a = (int)((double)v.x * 1000.0 + 0.5);
		int b = (int)((double)v.y * 1000.0 + 0.5);
		int a2 = (int)((double)v.z * 1000.0 + 0.5);
		int b2 = (int)((double)v.w * 1000.0 + 0.5);
		ulong a3 = UnityEngineUtils.MergeTo64(a, b);
		ulong b3 = UnityEngineUtils.MergeTo64(a2, b2);
		return StaticHash.Compute128To64(a3, b3);
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x00143C94 File Offset: 0x00141E94
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static long QuantizedHash64(this Matrix4x4 m)
	{
		m4x4 m4x = *m4x4.From(ref m);
		long a = m4x.r0.QuantizedHash64();
		long b = m4x.r1.QuantizedHash64();
		long a2 = m4x.r2.QuantizedHash64();
		long b2 = m4x.r3.QuantizedHash64();
		long a3 = StaticHash.Compute128To64(a, b);
		long b3 = StaticHash.Compute128To64(a2, b2);
		return StaticHash.Compute128To64(a3, b3);
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x00143CF4 File Offset: 0x00141EF4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong MergeTo64(int a, int b)
	{
		return (ulong)b << 32 | (ulong)a;
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x00053646 File Offset: 0x00051846
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Vector4 ToVector(this Quaternion q)
	{
		return *Unsafe.As<Quaternion, Vector4>(ref q);
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x00053654 File Offset: 0x00051854
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CopyTo(this Quaternion q, ref Vector4 v)
	{
		v.x = q.x;
		v.y = q.y;
		v.z = q.z;
		v.w = q.w;
	}
}
