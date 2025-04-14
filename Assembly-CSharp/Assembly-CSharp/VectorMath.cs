using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x0200089B RID: 2203
public static class VectorMath
{
	// Token: 0x06003540 RID: 13632 RVA: 0x000FDE14 File Offset: 0x000FC014
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3Int Clamped(this Vector3Int v, int min, int max)
	{
		v.x = Math.Clamp(v.x, min, max);
		v.y = Math.Clamp(v.y, min, max);
		v.z = Math.Clamp(v.z, min, max);
		return v;
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x000FDE61 File Offset: 0x000FC061
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetXYZ(this Vector3 v, float f)
	{
		v.x = f;
		v.y = f;
		v.z = f;
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x000FDE78 File Offset: 0x000FC078
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3Int Abs(this Vector3Int v)
	{
		v.x = Math.Abs(v.x);
		v.y = Math.Abs(v.y);
		v.z = Math.Abs(v.z);
		return v;
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x000FDEB4 File Offset: 0x000FC0B4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Abs(this Vector3 v)
	{
		v.x = Math.Abs(v.x);
		v.y = Math.Abs(v.y);
		v.z = Math.Abs(v.z);
		return v;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x000FDEED File Offset: 0x000FC0ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Min(this Vector3 v, Vector3 other)
	{
		return new Vector3(Math.Min(v.x, other.x), Math.Min(v.y, other.y), Math.Min(v.z, other.z));
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x000FDF27 File Offset: 0x000FC127
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v, Vector3 other)
	{
		return new Vector3(Math.Max(v.x, other.x), Math.Max(v.y, other.y), Math.Max(v.z, other.z));
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x000FDF61 File Offset: 0x000FC161
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Add(this Vector3 v, float amount)
	{
		v.x += amount;
		v.y += amount;
		v.z += amount;
		return v;
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x000FDF88 File Offset: 0x000FC188
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Sub(this Vector3 v, float amount)
	{
		v.x -= amount;
		v.y -= amount;
		v.z -= amount;
		return v;
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x000FDFAF File Offset: 0x000FC1AF
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Mul(this Vector3 v, float amount)
	{
		v.x *= amount;
		v.y *= amount;
		v.z *= amount;
		return v;
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x000FDFD8 File Offset: 0x000FC1D8
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Div(this Vector3 v, float amount)
	{
		float num = 1f / amount;
		v.x *= num;
		v.y *= num;
		v.z *= num;
		return v;
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x000FE014 File Offset: 0x000FC214
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v)
	{
		float num = Math.Max(Math.Max(v.x, v.y), v.z);
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x000FE058 File Offset: 0x000FC258
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Max(this Vector3 v, float max)
	{
		float num = Math.Max(Math.Max(Math.Max(v.x, v.y), v.z), max);
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x000FE0A4 File Offset: 0x000FC2A4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float3 Max(this float3 v)
	{
		float num = Math.Max(v.x, Math.Max(v.y, v.z));
		v.x = num;
		v.y = num;
		v.z = num;
		return v;
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x000FE0E7 File Offset: 0x000FC2E7
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsFinite(this Vector3 v)
	{
		return float.IsFinite(v.x) && float.IsFinite(v.y) && float.IsFinite(v.z);
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x000FE110 File Offset: 0x000FC310
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max)
	{
		v.x = Math.Clamp(v.x, min.x, max.x);
		v.y = Math.Clamp(v.y, min.y, max.y);
		v.z = Math.Clamp(v.z, min.z, max.z);
		return v;
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x000FE178 File Offset: 0x000FC378
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx0(this Vector3 v, float epsilon = 1E-05f)
	{
		float x = v.x;
		float y = v.y;
		float z = v.z;
		return x * x + y * y + z * z <= epsilon * epsilon;
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x000FE1AC File Offset: 0x000FC3AC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx1(this Vector3 v, float epsilon = 1E-05f)
	{
		float num = v.x - 1f;
		float num2 = v.y - 1f;
		float num3 = v.z - 1f;
		return num * num + num2 * num2 + num3 * num3 <= epsilon * epsilon;
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x000FE1F4 File Offset: 0x000FC3F4
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool Approx(this Vector3 a, Vector3 b, float epsilon = 1E-05f)
	{
		float num = a.x - b.x;
		float num2 = a.y - b.y;
		float num3 = a.z - b.z;
		return num * num + num2 * num2 + num3 * num3 <= epsilon * epsilon;
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x000FE23C File Offset: 0x000FC43C
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
