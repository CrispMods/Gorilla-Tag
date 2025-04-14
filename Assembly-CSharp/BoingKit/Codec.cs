using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE0 RID: 3296
	public class Codec
	{
		// Token: 0x060052F7 RID: 21239 RVA: 0x001997E0 File Offset: 0x001979E0
		public static float PackSaturated(float a, float b)
		{
			a = Mathf.Floor(a * 4095f);
			b = Mathf.Floor(b * 4095f);
			return a * 4096f + b;
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x00199807 File Offset: 0x00197A07
		public static float PackSaturated(Vector2 v)
		{
			return Codec.PackSaturated(v.x, v.y);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0019981A File Offset: 0x00197A1A
		public static Vector2 UnpackSaturated(float f)
		{
			return new Vector2(Mathf.Floor(f / 4096f), Mathf.Repeat(f, 4096f)) / 4095f;
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x00199844 File Offset: 0x00197A44
		public static Vector2 OctWrap(Vector2 v)
		{
			return (Vector2.one - new Vector2(Mathf.Abs(v.y), Mathf.Abs(v.x))) * new Vector2(Mathf.Sign(v.x), Mathf.Sign(v.y));
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x00199898 File Offset: 0x00197A98
		public static float PackNormal(Vector3 n)
		{
			n /= Mathf.Abs(n.x) + Mathf.Abs(n.y) + Mathf.Abs(n.z);
			return Codec.PackSaturated(((n.z >= 0f) ? new Vector2(n.x, n.y) : Codec.OctWrap(new Vector2(n.x, n.y))) * 0.5f + 0.5f * Vector2.one);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0019992C File Offset: 0x00197B2C
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

		// Token: 0x060052FD RID: 21245 RVA: 0x001999D4 File Offset: 0x00197BD4
		public static uint PackRgb(Color color)
		{
			return (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x00199A04 File Offset: 0x00197C04
		public static Color UnpackRgb(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f);
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x00199A40 File Offset: 0x00197C40
		public static uint PackRgba(Color color)
		{
			return (uint)(color.a * 255f) << 24 | (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x00199A8C File Offset: 0x00197C8C
		public static Color UnpackRgba(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f, ((i & 4278190080U) >> 24) / 255f);
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x00199AE2 File Offset: 0x00197CE2
		public static uint Pack8888(uint x, uint y, uint z, uint w)
		{
			return (x & 255U) << 24 | (y & 255U) << 16 | (z & 255U) << 8 | (w & 255U);
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x00199B0B File Offset: 0x00197D0B
		public static void Unpack8888(uint i, out uint x, out uint y, out uint z, out uint w)
		{
			x = (i >> 24 & 255U);
			y = (i >> 16 & 255U);
			z = (i >> 8 & 255U);
			w = (i & 255U);
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x00199B3C File Offset: 0x00197D3C
		private static int IntReinterpret(float f)
		{
			return new Codec.IntFloat
			{
				FloatValue = f
			}.IntValue;
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x00199B5F File Offset: 0x00197D5F
		public static int HashConcat(int hash, int i)
		{
			return (hash ^ i) * Codec.FnvPrime;
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x00199B6A File Offset: 0x00197D6A
		public static int HashConcat(int hash, long i)
		{
			hash = Codec.HashConcat(hash, (int)(i & (long)((ulong)-1)));
			hash = Codec.HashConcat(hash, (int)(i >> 32));
			return hash;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x00199B87 File Offset: 0x00197D87
		public static int HashConcat(int hash, float f)
		{
			return Codec.HashConcat(hash, Codec.IntReinterpret(f));
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x00199B95 File Offset: 0x00197D95
		public static int HashConcat(int hash, bool b)
		{
			return Codec.HashConcat(hash, b ? 1 : 0);
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x00199BA4 File Offset: 0x00197DA4
		public static int HashConcat(int hash, params int[] ints)
		{
			foreach (int i2 in ints)
			{
				hash = Codec.HashConcat(hash, i2);
			}
			return hash;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00199BD0 File Offset: 0x00197DD0
		public static int HashConcat(int hash, params float[] floats)
		{
			foreach (float f in floats)
			{
				hash = Codec.HashConcat(hash, f);
			}
			return hash;
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00199BFB File Offset: 0x00197DFB
		public static int HashConcat(int hash, Vector2 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y
			});
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x00199C1B File Offset: 0x00197E1B
		public static int HashConcat(int hash, Vector3 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y,
				v.z
			});
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x00199C44 File Offset: 0x00197E44
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

		// Token: 0x0600530D RID: 21261 RVA: 0x00199C76 File Offset: 0x00197E76
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

		// Token: 0x0600530E RID: 21262 RVA: 0x00199CA8 File Offset: 0x00197EA8
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

		// Token: 0x0600530F RID: 21263 RVA: 0x00199CDA File Offset: 0x00197EDA
		public static int HashConcat(int hash, Transform t)
		{
			return Codec.HashConcat(hash, t.GetHashCode());
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x00199CE8 File Offset: 0x00197EE8
		public static int Hash(int i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x00199CF5 File Offset: 0x00197EF5
		public static int Hash(long i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x00199D02 File Offset: 0x00197F02
		public static int Hash(float f)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, f);
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x00199D0F File Offset: 0x00197F0F
		public static int Hash(bool b)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, b);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x00199D1C File Offset: 0x00197F1C
		public static int Hash(params int[] ints)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, ints);
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x00199D29 File Offset: 0x00197F29
		public static int Hash(params float[] floats)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, floats);
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x00199D36 File Offset: 0x00197F36
		public static int Hash(Vector2 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x00199D43 File Offset: 0x00197F43
		public static int Hash(Vector3 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x00199D50 File Offset: 0x00197F50
		public static int Hash(Vector4 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x00199D5D File Offset: 0x00197F5D
		public static int Hash(Quaternion q)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, q);
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x00199D6A File Offset: 0x00197F6A
		public static int Hash(Color c)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, c);
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x00199D78 File Offset: 0x00197F78
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

		// Token: 0x0600531C RID: 21276 RVA: 0x00199DBD File Offset: 0x00197FBD
		public static int HashTransformHierarchy(Transform t)
		{
			return Codec.HashTransformHierarchyRecurvsive(Codec.FnvDefaultBasis, t);
		}

		// Token: 0x0400559F RID: 21919
		public static readonly int FnvDefaultBasis = -2128831035;

		// Token: 0x040055A0 RID: 21920
		public static readonly int FnvPrime = 16777619;

		// Token: 0x02000CE1 RID: 3297
		[StructLayout(LayoutKind.Explicit)]
		private struct IntFloat
		{
			// Token: 0x040055A1 RID: 21921
			[FieldOffset(0)]
			public int IntValue;

			// Token: 0x040055A2 RID: 21922
			[FieldOffset(0)]
			public float FloatValue;
		}
	}
}
