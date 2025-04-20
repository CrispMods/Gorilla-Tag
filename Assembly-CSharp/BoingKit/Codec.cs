using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D11 RID: 3345
	public class Codec
	{
		// Token: 0x06005459 RID: 21593 RVA: 0x00066AC5 File Offset: 0x00064CC5
		public static float PackSaturated(float a, float b)
		{
			a = Mathf.Floor(a * 4095f);
			b = Mathf.Floor(b * 4095f);
			return a * 4096f + b;
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x00066AEC File Offset: 0x00064CEC
		public static float PackSaturated(Vector2 v)
		{
			return Codec.PackSaturated(v.x, v.y);
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x00066AFF File Offset: 0x00064CFF
		public static Vector2 UnpackSaturated(float f)
		{
			return new Vector2(Mathf.Floor(f / 4096f), Mathf.Repeat(f, 4096f)) / 4095f;
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001CF134 File Offset: 0x001CD334
		public static Vector2 OctWrap(Vector2 v)
		{
			return (Vector2.one - new Vector2(Mathf.Abs(v.y), Mathf.Abs(v.x))) * new Vector2(Mathf.Sign(v.x), Mathf.Sign(v.y));
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x001CF188 File Offset: 0x001CD388
		public static float PackNormal(Vector3 n)
		{
			n /= Mathf.Abs(n.x) + Mathf.Abs(n.y) + Mathf.Abs(n.z);
			return Codec.PackSaturated(((n.z >= 0f) ? new Vector2(n.x, n.y) : Codec.OctWrap(new Vector2(n.x, n.y))) * 0.5f + 0.5f * Vector2.one);
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x001CF21C File Offset: 0x001CD41C
		public static Vector3 UnpackNormal(float f)
		{
			Vector2 vector = Codec.UnpackSaturated(f);
			vector = vector * 2f - Vector2.one;
			Vector3 vector2 = new Vector3(vector.x, vector.y, 1f - Mathf.Abs(vector.x) - Mathf.Abs(vector.y));
			float num = Mathf.Clamp01(-vector2.z);
			vector2.x += ((vector2.x >= 0f) ? (-num) : num);
			vector2.y += ((vector2.y >= 0f) ? (-num) : num);
			return vector2.normalized;
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x00066B27 File Offset: 0x00064D27
		public static uint PackRgb(Color color)
		{
			return (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x00066B57 File Offset: 0x00064D57
		public static Color UnpackRgb(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f);
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x001CF2C4 File Offset: 0x001CD4C4
		public static uint PackRgba(Color color)
		{
			return (uint)(color.a * 255f) << 24 | (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x001CF310 File Offset: 0x001CD510
		public static Color UnpackRgba(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f, ((i & 4278190080U) >> 24) / 255f);
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00066B90 File Offset: 0x00064D90
		public static uint Pack8888(uint x, uint y, uint z, uint w)
		{
			return (x & 255U) << 24 | (y & 255U) << 16 | (z & 255U) << 8 | (w & 255U);
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x00066BB9 File Offset: 0x00064DB9
		public static void Unpack8888(uint i, out uint x, out uint y, out uint z, out uint w)
		{
			x = (i >> 24 & 255U);
			y = (i >> 16 & 255U);
			z = (i >> 8 & 255U);
			w = (i & 255U);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x001CF368 File Offset: 0x001CD568
		private static int IntReinterpret(float f)
		{
			return new Codec.IntFloat
			{
				FloatValue = f
			}.IntValue;
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x00066BE8 File Offset: 0x00064DE8
		public static int HashConcat(int hash, int i)
		{
			return (hash ^ i) * Codec.FnvPrime;
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x00066BF3 File Offset: 0x00064DF3
		public static int HashConcat(int hash, long i)
		{
			hash = Codec.HashConcat(hash, (int)(i & (long)((ulong)-1)));
			hash = Codec.HashConcat(hash, (int)(i >> 32));
			return hash;
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x00066C10 File Offset: 0x00064E10
		public static int HashConcat(int hash, float f)
		{
			return Codec.HashConcat(hash, Codec.IntReinterpret(f));
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x00066C1E File Offset: 0x00064E1E
		public static int HashConcat(int hash, bool b)
		{
			return Codec.HashConcat(hash, b ? 1 : 0);
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x001CF38C File Offset: 0x001CD58C
		public static int HashConcat(int hash, params int[] ints)
		{
			foreach (int i2 in ints)
			{
				hash = Codec.HashConcat(hash, i2);
			}
			return hash;
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x001CF3B8 File Offset: 0x001CD5B8
		public static int HashConcat(int hash, params float[] floats)
		{
			foreach (float f in floats)
			{
				hash = Codec.HashConcat(hash, f);
			}
			return hash;
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x00066C2D File Offset: 0x00064E2D
		public static int HashConcat(int hash, Vector2 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y
			});
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x00066C4D File Offset: 0x00064E4D
		public static int HashConcat(int hash, Vector3 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y,
				v.z
			});
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x00066C76 File Offset: 0x00064E76
		public static int HashConcat(int hash, Vector4 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y,
				v.z,
				v.w
			});
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x00066CA8 File Offset: 0x00064EA8
		public static int HashConcat(int hash, Quaternion q)
		{
			return Codec.HashConcat(hash, new float[]
			{
				q.x,
				q.y,
				q.z,
				q.w
			});
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x00066CDA File Offset: 0x00064EDA
		public static int HashConcat(int hash, Color c)
		{
			return Codec.HashConcat(hash, new float[]
			{
				c.r,
				c.g,
				c.b,
				c.a
			});
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x00066D0C File Offset: 0x00064F0C
		public static int HashConcat(int hash, Transform t)
		{
			return Codec.HashConcat(hash, t.GetHashCode());
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x00066D1A File Offset: 0x00064F1A
		public static int Hash(int i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x00066D27 File Offset: 0x00064F27
		public static int Hash(long i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x00066D34 File Offset: 0x00064F34
		public static int Hash(float f)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, f);
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x00066D41 File Offset: 0x00064F41
		public static int Hash(bool b)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, b);
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x00066D4E File Offset: 0x00064F4E
		public static int Hash(params int[] ints)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, ints);
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x00066D5B File Offset: 0x00064F5B
		public static int Hash(params float[] floats)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, floats);
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x00066D68 File Offset: 0x00064F68
		public static int Hash(Vector2 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x00066D75 File Offset: 0x00064F75
		public static int Hash(Vector3 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x00066D82 File Offset: 0x00064F82
		public static int Hash(Vector4 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x00066D8F File Offset: 0x00064F8F
		public static int Hash(Quaternion q)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, q);
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x00066D9C File Offset: 0x00064F9C
		public static int Hash(Color c)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, c);
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x001CF3E4 File Offset: 0x001CD5E4
		private static int HashTransformHierarchyRecurvsive(int hash, Transform t)
		{
			hash = Codec.HashConcat(hash, t);
			hash = Codec.HashConcat(hash, t.childCount);
			for (int i = 0; i < t.childCount; i++)
			{
				hash = Codec.HashTransformHierarchyRecurvsive(hash, t.GetChild(i));
			}
			return hash;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x00066DA9 File Offset: 0x00064FA9
		public static int HashTransformHierarchy(Transform t)
		{
			return Codec.HashTransformHierarchyRecurvsive(Codec.FnvDefaultBasis, t);
		}

		// Token: 0x040056AB RID: 22187
		public static readonly int FnvDefaultBasis = -2128831035;

		// Token: 0x040056AC RID: 22188
		public static readonly int FnvPrime = 16777619;

		// Token: 0x02000D12 RID: 3346
		[StructLayout(LayoutKind.Explicit)]
		private struct IntFloat
		{
			// Token: 0x040056AD RID: 22189
			[FieldOffset(0)]
			public int IntValue;

			// Token: 0x040056AE RID: 22190
			[FieldOffset(0)]
			public float FloatValue;
		}
	}
}
