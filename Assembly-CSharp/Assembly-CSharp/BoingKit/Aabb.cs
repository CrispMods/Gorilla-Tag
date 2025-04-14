using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE8 RID: 3304
	public struct Aabb
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x0019A3BB File Offset: 0x001985BB
		// (set) Token: 0x0600532D RID: 21293 RVA: 0x0019A3C8 File Offset: 0x001985C8
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

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x0019A3D6 File Offset: 0x001985D6
		// (set) Token: 0x0600532F RID: 21295 RVA: 0x0019A3E3 File Offset: 0x001985E3
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

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06005330 RID: 21296 RVA: 0x0019A3F1 File Offset: 0x001985F1
		// (set) Token: 0x06005331 RID: 21297 RVA: 0x0019A3FE File Offset: 0x001985FE
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

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x0019A40C File Offset: 0x0019860C
		// (set) Token: 0x06005333 RID: 21299 RVA: 0x0019A419 File Offset: 0x00198619
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

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06005334 RID: 21300 RVA: 0x0019A427 File Offset: 0x00198627
		// (set) Token: 0x06005335 RID: 21301 RVA: 0x0019A434 File Offset: 0x00198634
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

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x0019A442 File Offset: 0x00198642
		// (set) Token: 0x06005337 RID: 21303 RVA: 0x0019A44F File Offset: 0x0019864F
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

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x06005338 RID: 21304 RVA: 0x0019A45D File Offset: 0x0019865D
		public Vector3 Center
		{
			get
			{
				return 0.5f * (this.Min + this.Max);
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06005339 RID: 21305 RVA: 0x0019A47C File Offset: 0x0019867C
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

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x0600533A RID: 21306 RVA: 0x0019A4E1 File Offset: 0x001986E1
		public static Aabb Empty
		{
			get
			{
				return new Aabb(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), new Vector3(float.MinValue, float.MinValue, float.MinValue));
			}
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x0019A510 File Offset: 0x00198710
		public static Aabb FromPoint(Vector3 p)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(p);
			return empty;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0019A52C File Offset: 0x0019872C
		public static Aabb FromPoints(Vector3 a, Vector3 b)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(a);
			empty.Include(b);
			return empty;
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x0019A550 File Offset: 0x00198750
		public Aabb(Vector3 min, Vector3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x0019A560 File Offset: 0x00198760
		public void Include(Vector3 p)
		{
			this.MinX = Mathf.Min(this.MinX, p.x);
			this.MinY = Mathf.Min(this.MinY, p.y);
			this.MinZ = Mathf.Min(this.MinZ, p.z);
			this.MaxX = Mathf.Max(this.MaxX, p.x);
			this.MaxY = Mathf.Max(this.MaxY, p.y);
			this.MaxZ = Mathf.Max(this.MaxZ, p.z);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x0019A5F8 File Offset: 0x001987F8
		public bool Contains(Vector3 p)
		{
			return this.MinX <= p.x && this.MinY <= p.y && this.MinZ <= p.z && this.MaxX >= p.x && this.MaxY >= p.y && this.MaxZ >= p.z;
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0019A65E File Offset: 0x0019885E
		public bool ContainsX(Vector3 p)
		{
			return this.MinX <= p.x && this.MaxX >= p.x;
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0019A681 File Offset: 0x00198881
		public bool ContainsY(Vector3 p)
		{
			return this.MinY <= p.y && this.MaxY >= p.y;
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x0019A6A4 File Offset: 0x001988A4
		public bool ContainsZ(Vector3 p)
		{
			return this.MinZ <= p.z && this.MaxZ >= p.z;
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x0019A6C8 File Offset: 0x001988C8
		public bool Intersects(Aabb rhs)
		{
			return this.MinX <= rhs.MaxX && this.MinY <= rhs.MaxY && this.MinZ <= rhs.MaxZ && this.MaxX >= rhs.MinX && this.MaxY >= rhs.MinY && this.MaxZ >= rhs.MinZ;
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x0019A734 File Offset: 0x00198934
		public bool Intersects(ref BoingEffector.Params effector)
		{
			if (!effector.Bits.IsBitSet(0))
			{
				return this.Intersects(Aabb.FromPoint(effector.CurrPosition).Expand(effector.Radius));
			}
			return this.Intersects(Aabb.FromPoints(effector.PrevPosition, effector.CurrPosition).Expand(effector.Radius));
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x0019A794 File Offset: 0x00198994
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

		// Token: 0x040055BE RID: 21950
		public Vector3 Min;

		// Token: 0x040055BF RID: 21951
		public Vector3 Max;
	}
}
