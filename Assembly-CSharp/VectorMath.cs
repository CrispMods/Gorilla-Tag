using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000898 RID: 2200
public static class VectorMath
{
	// Token: 0x06003534 RID: 13620 RVA: 0x000FD84C File Offset: 0x000FBA4C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3Int Clamped(this Vector3Int v, int min, int max)
	{
		v.x = Math.Clamp(v.x, min, max);
		v.y = Math.Clamp(v.y, min, max);
		v.z = Math.Clamp(v.z, min, max);
		return v;
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x000FD899 File Offset: 0x000FBA99
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetXYZ(this Vector3 v, float f)
	{
		v.x = f;
		v.y = f;
		v.z = f;
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x000FD8B0 File Offset: 0x000FBAB0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3Int Abs(this Vector3Int v)
	{
		v.x = Math.Abs(v.x);
		v.y = Math.Abs(v.y);
		v.z = Math.Abs(v.z);
		return v;
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x000FD8EC File Offset: 0x000FBAEC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Abs(this Vector3 v)
	{
		v.x = Math.Abs(v.x);
		v.y = Math.Abs(v.y);
		v.z = Math.Abs(v.z);
		return v;
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x000FD925 File Offset: 0x000FBB25
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Min(this Vector3 v, Vector3 other)
	{
		return new Vector3(Math.Min(v.x, other.x), Math.Min(v.y, other.y), Math.Min(v.z, other.z));
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x000FD95F File Offset: 0x000FBB5F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v, Vector3 other)
	{
		return new Vector3(Math.Max(v.x, other.x), Math.Max(v.y, other.y), Math.Max(v.z, other.z));
	}

	// Token: 0x0600353A RID: 13626 RVA: 0x000FD999 File Offset: 0x000FBB99
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Add(this Vector3 v, float amount)
	{
		v.x += amount;
		v.y += amount;
		v.z += amount;
		return v;
	}

	// Token: 0x0600353B RID: 13627 RVA: 0x000FD9C0 File Offset: 0x000FBBC0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Sub(this Vector3 v, float amount)
	{
		v.x -= amount;
		v.y -= amount;
		v.z -= amount;
		return v;
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x000FD9E7 File Offset: 0x000FBBE7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Mul(this Vector3 v, float amount)
	{
		v.x *= amount;
		v.y *= amount;
		v.z *= amount;
		return v;
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x000FDA10 File Offset: 0x000FBC10
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Div(this Vector3 v, float amount)
	{
		float num = 1f / amount;
		v.x *= num;
		v.y *= num;
		v.z *= num;
		return v;
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x000FDA4C File Offset: 0x000FBC4C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v)
	{
		float num = Math.Max(Math.Max(v.x, v.y), v.z);
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x000FDA90 File Offset: 0x000FBC90
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v, float max)
	{
		float num = Math.Max(Math.Max(Math.Max(v.x, v.y), v.z), max);
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x000FDADC File Offset: 0x000FBCDC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 Max(this float3 v)
	{
		float num = Math.Max(v.x, Math.Max(v.y, v.z));
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x000FDB1F File Offset: 0x000FBD1F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsFinite(this Vector3 v)
	{
		return float.IsFinite(v.x) && float.IsFinite(v.y) && float.IsFinite(v.z);
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x000FDB48 File Offset: 0x000FBD48
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max)
	{
		v.x = Math.Clamp(v.x, min.x, max.x);
		v.y = Math.Clamp(v.y, min.y, max.y);
		v.z = Math.Clamp(v.z, min.z, max.z);
		return v;
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x000FDBB0 File Offset: 0x000FBDB0
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx0(this Vector3 v, float epsilon = 1E-05f)
	{
		float x = v.x;
		float y = v.y;
		float z = v.z;
		return x * x + y * y + z * z <= epsilon * epsilon;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x000FDBE4 File Offset: 0x000FBDE4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx1(this Vector3 v, float epsilon = 1E-05f)
	{
		float num = v.x - 1f;
		float num2 = v.y - 1f;
		float num3 = v.z - 1f;
		return num * num + num2 * num2 + num3 * num3 <= epsilon * epsilon;
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x000FDC2C File Offset: 0x000FBE2C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
	{
		float num = a.x - b.x;
		float num2 = a.y - b.y;
		float num3 = a.z - b.z;
		return num * num + num2 * num2 + num3 * num3 <= epsilon * epsilon;
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x000FDC74 File Offset: 0x000FBE74
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx(this Vector4 a, Vector4 b, float epsilon = 1E-05f)
	{
		float num = a.x - b.x;
		float num2 = a.y - b.y;
		float num3 = a.z - b.z;
		float num4 = a.w - b.w;
		return num * num + num2 * num2 + num3 * num3 + num4 * num4 <= epsilon * epsilon;
	}
}
