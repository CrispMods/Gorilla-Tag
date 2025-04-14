using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE5 RID: 3301
	public struct Aabb
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06005320 RID: 21280 RVA: 0x00199DF3 File Offset: 0x00197FF3
		// (set) Token: 0x06005321 RID: 21281 RVA: 0x00199E00 File Offset: 0x00198000
		public float MinX
		{
			get
			{
				return this.Min.x;
			}
			set
			{
				this.Min.x = value;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06005322 RID: 21282 RVA: 0x00199E0E File Offset: 0x0019800E
		// (set) Token: 0x06005323 RID: 21283 RVA: 0x00199E1B File Offset: 0x0019801B
		public float MinY
		{
			get
			{
				return this.Min.y;
			}
			set
			{
				this.Min.y = value;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06005324 RID: 21284 RVA: 0x00199E29 File Offset: 0x00198029
		// (set) Token: 0x06005325 RID: 21285 RVA: 0x00199E36 File Offset: 0x00198036
		public float MinZ
		{
			get
			{
				return this.Min.z;
			}
			set
			{
				this.Min.z = value;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x00199E44 File Offset: 0x00198044
		// (set) Token: 0x06005327 RID: 21287 RVA: 0x00199E51 File Offset: 0x00198051
		public float MaxX
		{
			get
			{
				return this.Max.x;
			}
			set
			{
				this.Max.x = value;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06005328 RID: 21288 RVA: 0x00199E5F File Offset: 0x0019805F
		// (set) Token: 0x06005329 RID: 21289 RVA: 0x00199E6C File Offset: 0x0019806C
		public float MaxY
		{
			get
			{
				return this.Max.y;
			}
			set
			{
				this.Max.y = value;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x0600532A RID: 21290 RVA: 0x00199E7A File Offset: 0x0019807A
		// (set) Token: 0x0600532B RID: 21291 RVA: 0x00199E87 File Offset: 0x00198087
		public float MaxZ
		{
			get
			{
				return this.Max.z;
			}
			set
			{
				this.Max.z = value;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x00199E95 File Offset: 0x00198095
		public Vector3 Center
		{
			get
			{
				return 0.5f * (this.Min + this.Max);
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x00199EB4 File Offset: 0x001980B4
		public Vector3 Size
		{
			get
			{
				Vector3 vector = this.Max - this.Min;
				vector.x = Mathf.Max(0f, vector.x);
				vector.y = Mathf.Max(0f, vector.y);
				vector.z = Mathf.Max(0f, vector.z);
				return vector;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x00199F19 File Offset: 0x00198119
		public static Aabb Empty
		{
			get
			{
				return new Aabb(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), new Vector3(float.MinValue, float.MinValue, float.MinValue));
			}
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x00199F48 File Offset: 0x00198148
		public static Aabb FromPoint(Vector3 p)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(p);
			return empty;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x00199F64 File Offset: 0x00198164
		public static Aabb FromPoints(Vector3 a, Vector3 b)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(a);
			empty.Include(b);
			return empty;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x00199F88 File Offset: 0x00198188
		public Aabb(Vector3 min, Vector3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x00199F98 File Offset: 0x00198198
		public void Include(Vector3 p)
		{
			this.MinX = Mathf.Min(this.MinX, p.x);
			this.MinY = Mathf.Min(this.MinY, p.y);
			this.MinZ = Mathf.Min(this.MinZ, p.z);
			this.MaxX = Mathf.Max(this.MaxX, p.x);
			this.MaxY = Mathf.Max(this.MaxY, p.y);
			this.MaxZ = Mathf.Max(this.MaxZ, p.z);
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x0019A030 File Offset: 0x00198230
		public bool Contains(Vector3 p)
		{
			return this.MinX <= p.x && this.MinY <= p.y && this.MinZ <= p.z && this.MaxX >= p.x && this.MaxY >= p.y && this.MaxZ >= p.z;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x0019A096 File Offset: 0x00198296
		public bool ContainsX(Vector3 p)
		{
			return this.MinX <= p.x && this.MaxX >= p.x;
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x0019A0B9 File Offset: 0x001982B9
		public bool ContainsY(Vector3 p)
		{
			return this.MinY <= p.y && this.MaxY >= p.y;
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x0019A0DC File Offset: 0x001982DC
		public bool ContainsZ(Vector3 p)
		{
			return this.MinZ <= p.z && this.MaxZ >= p.z;
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x0019A100 File Offset: 0x00198300
		public bool Intersects(Aabb rhs)
		{
			return this.MinX <= rhs.MaxX && this.MinY <= rhs.MaxY && this.MinZ <= rhs.MaxZ && this.MaxX >= rhs.MinX && this.MaxY >= rhs.MinY && this.MaxZ >= rhs.MinZ;
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x0019A16C File Offset: 0x0019836C
		public bool Intersects(ref BoingEffector.Params effector)
		{
			if (!effector.Bits.IsBitSet(0))
			{
				return this.Intersects(Aabb.FromPoint(effector.CurrPosition).Expand(effector.Radius));
			}
			return this.Intersects(Aabb.FromPoints(effector.PrevPosition, effector.CurrPosition).Expand(effector.Radius));
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x0019A1CC File Offset: 0x001983CC
		public Aabb Expand(float amount)
		{
			this.MinX -= amount;
			this.MinY -= amount;
			this.MinZ -= amount;
			this.MaxX += amount;
			this.MaxY += amount;
			this.MaxZ += amount;
			return this;
		}

		// Token: 0x040055AC RID: 21932
		public Vector3 Min;

		// Token: 0x040055AD RID: 21933
		public Vector3 Max;
	}
}
