using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public static class UnityEngineUtils
{
	// Token: 0x0600350E RID: 13582 RVA: 0x000FD216 File Offset: 0x000FB416
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool EqualsColor(this Color32 c, Color32 other)
	{
		return c.r == other.r && c.g == other.g && c.b == other.b && c.a == other.a;
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x000FD254 File Offset: 0x000FB454
	public static Color32 IdToColor32(this Object obj, int alpha = -1, bool distinct = true)
	{
		if (!(obj == null))
		{
			return obj.GetInstanceID().IdToColor32(alpha, distinct);
		}
		return default(Color32);
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x000FD284 File Offset: 0x000FB484
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

	// Token: 0x06003511 RID: 13585 RVA: 0x000FD2C4 File Offset: 0x000FB4C4
	public static Color32 ToHighViz(this Color32 c)
	{
		float h;
		float num;
		float num2;
		Color.RGBToHSV(c, out h, out num, out num2);
		return Color.HSVToRGB(h, 1f, 1f);
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x000FD2F8 File Offset: 0x000FB4F8
	public unsafe static int Color32ToId(this Color32 c, bool distinct = true)
	{
		int num = *Unsafe.As<Color32, int>(ref c);
		if (distinct)
		{
			num = StaticHash.ReverseTriple32(num);
		}
		return num;
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x000FD31C File Offset: 0x000FB51C
	public static Hash128 QuantizedHash128(this Matrix4x4 m)
	{
		Hash128 result = default(Hash128);
		HashUtilities.QuantisedMatrixHash(ref m, ref result);
		return result;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x000FD33C File Offset: 0x000FB53C
	public static Hash128 QuantizedHash128(this Vector3 v)
	{
		Hash128 result = default(Hash128);
		HashUtilities.QuantisedVectorHash(ref v, ref result);
		return result;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x000FD35B File Offset: 0x000FB55B
	public static Id128 QuantizedId128(this Vector3 v)
	{
		return v.QuantizedHash128();
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000FD368 File Offset: 0x000FB568
	public static Id128 QuantizedId128(this Matrix4x4 m)
	{
		return m.QuantizedHash128();
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000FD378 File Offset: 0x000FB578
	public static Id128 QuantizedId128(this Quaternion q)
	{
		int a = (int)((double)q.x * 1000.0 + 0.5);
		int b = (int)((double)q.y * 1000.0 + 0.5);
		int c = (int)((double)q.z * 1000.0 + 0.5);
		int d = (int)((double)q.w * 1000.0 + 0.5);
		return new Id128(a, b, c, d);
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x000FD400 File Offset: 0x000FB600
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

	// Token: 0x06003519 RID: 13593 RVA: 0x000FD494 File Offset: 0x000FB694
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

	// Token: 0x0600351A RID: 13594 RVA: 0x000FD4F4 File Offset: 0x000FB6F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong MergeTo64(int a, int b)
	{
		return (ulong)b << 32 | (ulong)a;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x000FD50B File Offset: 0x000FB70B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Vector4 ToVector(this Quaternion q)
	{
		return *Unsafe.As<Quaternion, Vector4>(ref q);
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x000FD519 File Offset: 0x000FB719
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CopyTo(this Quaternion q, ref Vector4 v)
	{
		v.x = q.x;
		v.y = q.y;
		v.z = q.z;
		v.w = q.w;
	}
}
