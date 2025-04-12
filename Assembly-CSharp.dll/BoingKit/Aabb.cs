using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE8 RID: 3304
	public struct Aabb
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x00065369 File Offset: 0x00063569
		// (set) Token: 0x0600532D RID: 21293 RVA: 0x00065376 File Offset: 0x00063576
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
		// (get) Token: 0x0600532E RID: 21294 RVA: 0x00065384 File Offset: 0x00063584
		// (set) Token: 0x0600532F RID: 21295 RVA: 0x00065391 File Offset: 0x00063591
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
		// (get) Token: 0x06005330 RID: 21296 RVA: 0x0006539F File Offset: 0x0006359F
		// (set) Token: 0x06005331 RID: 21297 RVA: 0x000653AC File Offset: 0x000635AC
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
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x000653BA File Offset: 0x000635BA
		// (set) Token: 0x06005333 RID: 21299 RVA: 0x000653C7 File Offset: 0x000635C7
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
		// (get) Token: 0x06005334 RID: 21300 RVA: 0x000653D5 File Offset: 0x000635D5
		// (set) Token: 0x06005335 RID: 21301 RVA: 0x000653E2 File Offset: 0x000635E2
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
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x000653F0 File Offset: 0x000635F0
		// (set) Token: 0x06005337 RID: 21303 RVA: 0x000653FD File Offset: 0x000635FD
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
		// (get) Token: 0x06005338 RID: 21304 RVA: 0x0006540B File Offset: 0x0006360B
		public Vector3 Center
		{
			get
			{
				return 0.5f * (this.Min + this.Max);
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x06005339 RID: 21305 RVA: 0x001C7348 File Offset: 0x001C5548
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
		// (get) Token: 0x0600533A RID: 21306 RVA: 0x00065428 File Offset: 0x00063628
		public static Aabb Empty
		{
			get
			{
				return new Aabb(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), new Vector3(float.MinValue, float.MinValue, float.MinValue));
			}
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x001C73B0 File Offset: 0x001C55B0
		public static Aabb FromPoint(Vector3 p)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(p);
			return empty;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x001C73CC File Offset: 0x001C55CC
		public static Aabb FromPoints(Vector3 a, Vector3 b)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(a);
			empty.Include(b);
			return empty;
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x00065457 File Offset: 0x00063657
		public Aabb(Vector3 min, Vector3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x001C73F0 File Offset: 0x001C55F0
		public void Include(Vector3 p)
		{
			this.MinX = Mathf.Min(this.MinX, p.x);
			this.MinY = Mathf.Min(this.MinY, p.y);
			this.MinZ = Mathf.Min(this.MinZ, p.z);
			this.MaxX = Mathf.Max(this.MaxX, p.x);
			this.MaxY = Mathf.Max(this.MaxY, p.y);
			this.MaxZ = Mathf.Max(this.MaxZ, p.z);
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x001C7488 File Offset: 0x001C5688
		public bool Contains(Vector3 p)
		{
			return this.MinX <= p.x && this.MinY <= p.y && this.MinZ <= p.z && this.MaxX >= p.x && this.MaxY >= p.y && this.MaxZ >= p.z;
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x00065467 File Offset: 0x00063667
		public bool ContainsX(Vector3 p)
		{
			return this.MinX <= p.x && this.MaxX >= p.x;
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0006548A File Offset: 0x0006368A
		public bool ContainsY(Vector3 p)
		{
			return this.MinY <= p.y && this.MaxY >= p.y;
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x000654AD File Offset: 0x000636AD
		public bool ContainsZ(Vector3 p)
		{
			return this.MinZ <= p.z && this.MaxZ >= p.z;
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x001C74F0 File Offset: 0x001C56F0
		public bool Intersects(Aabb rhs)
		{
			return this.MinX <= rhs.MaxX && this.MinY <= rhs.MaxY && this.MinZ <= rhs.MaxZ && this.MaxX >= rhs.MinX && this.MaxY >= rhs.MinY && this.MaxZ >= rhs.MinZ;
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x001C755C File Offset: 0x001C575C
		public bool Intersects(ref BoingEffector.Params effector)
		{
			if (!effector.Bits.IsBitSet(0))
			{
				return this.Intersects(Aabb.FromPoint(effector.CurrPosition).Expand(effector.Radius));
			}
			return this.Intersects(Aabb.FromPoints(effector.PrevPosition, effector.CurrPosition).Expand(effector.Radius));
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001C75BC File Offset: 0x001C57BC
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
