using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE3 RID: 3299
	public class Codec
	{
		// Token: 0x06005303 RID: 21251 RVA: 0x00199DA8 File Offset: 0x00197FA8
		public static float PackSaturated(float a, float b)
		{
			a = Mathf.Floor(a * 4095f);
			b = Mathf.Floor(b * 4095f);
			return a * 4096f + b;
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x00199DCF File Offset: 0x00197FCF
		public static float PackSaturated(Vector2 v)
		{
			return Codec.PackSaturated(v.x, v.y);
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x00199DE2 File Offset: 0x00197FE2
		public static Vector2 UnpackSaturated(float f)
		{
			return new Vector2(Mathf.Floor(f / 4096f), Mathf.Repeat(f, 4096f)) / 4095f;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x00199E0C File Offset: 0x0019800C
		public static Vector2 OctWrap(Vector2 v)
		{
			return (Vector2.one - new Vector2(Mathf.Abs(v.y), Mathf.Abs(v.x))) * new Vector2(Mathf.Sign(v.x), Mathf.Sign(v.y));
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x00199E60 File Offset: 0x00198060
		public static float PackNormal(Vector3 n)
		{
			n /= Mathf.Abs(n.x) + Mathf.Abs(n.y) + Mathf.Abs(n.z);
			return Codec.PackSaturated(((n.z >= 0f) ? new Vector2(n.x, n.y) : Codec.OctWrap(new Vector2(n.x, n.y))) * 0.5f + 0.5f * Vector2.one);
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x00199EF4 File Offset: 0x001980F4
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

		// Token: 0x06005309 RID: 21257 RVA: 0x00199F9C File Offset: 0x0019819C
		public static uint PackRgb(Color color)
		{
			return (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x00199FCC File Offset: 0x001981CC
		public static Color UnpackRgb(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f);
		}

		// Token: 0x0600530B RID: 21259 RVA: 0x0019A008 File Offset: 0x00198208
		public static uint PackRgba(Color color)
		{
			return (uint)(color.a * 255f) << 24 | (uint)(color.b * 255f) << 16 | (uint)(color.g * 255f) << 8 | (uint)(color.r * 255f);
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x0019A054 File Offset: 0x00198254
		public static Color UnpackRgba(uint i)
		{
			return new Color((i & 255U) / 255f, ((i & 65280U) >> 8) / 255f, ((i & 16711680U) >> 16) / 255f, ((i & 4278190080U) >> 24) / 255f);
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x0019A0AA File Offset: 0x001982AA
		public static uint Pack8888(uint x, uint y, uint z, uint w)
		{
			return (x & 255U) << 24 | (y & 255U) << 16 | (z & 255U) << 8 | (w & 255U);
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x0019A0D3 File Offset: 0x001982D3
		public static void Unpack8888(uint i, out uint x, out uint y, out uint z, out uint w)
		{
			x = (i >> 24 & 255U);
			y = (i >> 16 & 255U);
			z = (i >> 8 & 255U);
			w = (i & 255U);
		}

		// Token: 0x0600530F RID: 21263 RVA: 0x0019A104 File Offset: 0x00198304
		private static int IntReinterpret(float f)
		{
			return new Codec.IntFloat
			{
				FloatValue = f
			}.IntValue;
		}

		// Token: 0x06005310 RID: 21264 RVA: 0x0019A127 File Offset: 0x00198327
		public static int HashConcat(int hash, int i)
		{
			return (hash ^ i) * Codec.FnvPrime;
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x0019A132 File Offset: 0x00198332
		public static int HashConcat(int hash, long i)
		{
			hash = Codec.HashConcat(hash, (int)(i & (long)((ulong)-1)));
			hash = Codec.HashConcat(hash, (int)(i >> 32));
			return hash;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x0019A14F File Offset: 0x0019834F
		public static int HashConcat(int hash, float f)
		{
			return Codec.HashConcat(hash, Codec.IntReinterpret(f));
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x0019A15D File Offset: 0x0019835D
		public static int HashConcat(int hash, bool b)
		{
			return Codec.HashConcat(hash, b ? 1 : 0);
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x0019A16C File Offset: 0x0019836C
		public static int HashConcat(int hash, params int[] ints)
		{
			foreach (int i2 in ints)
			{
				hash = Codec.HashConcat(hash, i2);
			}
			return hash;
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x0019A198 File Offset: 0x00198398
		public static int HashConcat(int hash, params float[] floats)
		{
			foreach (float f in floats)
			{
				hash = Codec.HashConcat(hash, f);
			}
			return hash;
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x0019A1C3 File Offset: 0x001983C3
		public static int HashConcat(int hash, Vector2 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y
			});
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x0019A1E3 File Offset: 0x001983E3
		public static int HashConcat(int hash, Vector3 v)
		{
			return Codec.HashConcat(hash, new float[]
			{
				v.x,
				v.y,
				v.z
			});
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x0019A20C File Offset: 0x0019840C
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

		// Token: 0x06005319 RID: 21273 RVA: 0x0019A23E File Offset: 0x0019843E
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

		// Token: 0x0600531A RID: 21274 RVA: 0x0019A270 File Offset: 0x00198470
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

		// Token: 0x0600531B RID: 21275 RVA: 0x0019A2A2 File Offset: 0x001984A2
		public static int HashConcat(int hash, Transform t)
		{
			return Codec.HashConcat(hash, t.GetHashCode());
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x0019A2B0 File Offset: 0x001984B0
		public static int Hash(int i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x0019A2BD File Offset: 0x001984BD
		public static int Hash(long i)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, i);
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x0019A2CA File Offset: 0x001984CA
		public static int Hash(float f)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, f);
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x0019A2D7 File Offset: 0x001984D7
		public static int Hash(bool b)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, b);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x0019A2E4 File Offset: 0x001984E4
		public static int Hash(params int[] ints)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, ints);
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x0019A2F1 File Offset: 0x001984F1
		public static int Hash(params float[] floats)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, floats);
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x0019A2FE File Offset: 0x001984FE
		public static int Hash(Vector2 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x0019A30B File Offset: 0x0019850B
		public static int Hash(Vector3 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x0019A318 File Offset: 0x00198518
		public static int Hash(Vector4 v)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, v);
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x0019A325 File Offset: 0x00198525
		public static int Hash(Quaternion q)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, q);
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x0019A332 File Offset: 0x00198532
		public static int Hash(Color c)
		{
			return Codec.HashConcat(Codec.FnvDefaultBasis, c);
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x0019A340 File Offset: 0x00198540
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

		// Token: 0x06005328 RID: 21288 RVA: 0x0019A385 File Offset: 0x00198585
		public static int HashTransformHierarchy(Transform t)
		{
			return Codec.HashTransformHierarchyRecurvsive(Codec.FnvDefaultBasis, t);
		}

		// Token: 0x040055B1 RID: 21937
		public static readonly int FnvDefaultBasis = -2128831035;

		// Token: 0x040055B2 RID: 21938
		public static readonly int FnvPrime = 16777619;

		// Token: 0x02000CE4 RID: 3300
		[StructLayout(LayoutKind.Explicit)]
		private struct IntFloat
		{
			// Token: 0x040055B3 RID: 21939
			[FieldOffset(0)]
			public int IntValue;

			// Token: 0x040055B4 RID: 21940
			[FieldOffset(0)]
			public float FloatValue;
		}
	}
}
