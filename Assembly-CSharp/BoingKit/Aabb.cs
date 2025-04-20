using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D16 RID: 3350
	public struct Aabb
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06005482 RID: 21634 RVA: 0x00066DDF File Offset: 0x00064FDF
		// (set) Token: 0x06005483 RID: 21635 RVA: 0x00066DEC File Offset: 0x00064FEC
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

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06005484 RID: 21636 RVA: 0x00066DFA File Offset: 0x00064FFA
		// (set) Token: 0x06005485 RID: 21637 RVA: 0x00066E07 File Offset: 0x00065007
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

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06005486 RID: 21638 RVA: 0x00066E15 File Offset: 0x00065015
		// (set) Token: 0x06005487 RID: 21639 RVA: 0x00066E22 File Offset: 0x00065022
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

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06005488 RID: 21640 RVA: 0x00066E30 File Offset: 0x00065030
		// (set) Token: 0x06005489 RID: 21641 RVA: 0x00066E3D File Offset: 0x0006503D
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

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600548A RID: 21642 RVA: 0x00066E4B File Offset: 0x0006504B
		// (set) Token: 0x0600548B RID: 21643 RVA: 0x00066E58 File Offset: 0x00065058
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

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x0600548C RID: 21644 RVA: 0x00066E66 File Offset: 0x00065066
		// (set) Token: 0x0600548D RID: 21645 RVA: 0x00066E73 File Offset: 0x00065073
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

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600548E RID: 21646 RVA: 0x00066E81 File Offset: 0x00065081
		public Vector3 Center
		{
			get
			{
				return 0.5f * (this.Min + this.Max);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x001CF42C File Offset: 0x001CD62C
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

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06005490 RID: 21648 RVA: 0x00066E9E File Offset: 0x0006509E
		public static Aabb Empty
		{
			get
			{
				return new Aabb(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue), new Vector3(float.MinValue, float.MinValue, float.MinValue));
			}
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x001CF494 File Offset: 0x001CD694
		public static Aabb FromPoint(Vector3 p)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(p);
			return empty;
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x001CF4B0 File Offset: 0x001CD6B0
		public static Aabb FromPoints(Vector3 a, Vector3 b)
		{
			Aabb empty = Aabb.Empty;
			empty.Include(a);
			empty.Include(b);
			return empty;
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x00066ECD File Offset: 0x000650CD
		public Aabb(Vector3 min, Vector3 max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x001CF4D4 File Offset: 0x001CD6D4
		public void Include(Vector3 p)
		{
			this.MinX = Mathf.Min(this.MinX, p.x);
			this.MinY = Mathf.Min(this.MinY, p.y);
			this.MinZ = Mathf.Min(this.MinZ, p.z);
			this.MaxX = Mathf.Max(this.MaxX, p.x);
			this.MaxY = Mathf.Max(this.MaxY, p.y);
			this.MaxZ = Mathf.Max(this.MaxZ, p.z);
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x001CF56C File Offset: 0x001CD76C
		public bool Contains(Vector3 p)
		{
			return this.MinX <= p.x && this.MinY <= p.y && this.MinZ <= p.z && this.MaxX >= p.x && this.MaxY >= p.y && this.MaxZ >= p.z;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x00066EDD File Offset: 0x000650DD
		public bool ContainsX(Vector3 p)
		{
			return this.MinX <= p.x && this.MaxX >= p.x;
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x00066F00 File Offset: 0x00065100
		public bool ContainsY(Vector3 p)
		{
			return this.MinY <= p.y && this.MaxY >= p.y;
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x00066F23 File Offset: 0x00065123
		public bool ContainsZ(Vector3 p)
		{
			return this.MinZ <= p.z && this.MaxZ >= p.z;
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x001CF5D4 File Offset: 0x001CD7D4
		public bool Intersects(Aabb rhs)
		{
			return this.MinX <= rhs.MaxX && this.MinY <= rhs.MaxY && this.MinZ <= rhs.MaxZ && this.MaxX >= rhs.MinX && this.MaxY >= rhs.MinY && this.MaxZ >= rhs.MinZ;
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x001CF640 File Offset: 0x001CD840
		public bool Intersects(ref BoingEffector.Params effector)
		{
			if (!effector.Bits.IsBitSet(0))
			{
				return this.Intersects(Aabb.FromPoint(effector.CurrPosition).Expand(effector.Radius));
			}
			return this.Intersects(Aabb.FromPoints(effector.PrevPosition, effector.CurrPosition).Expand(effector.Radius));
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x001CF6A0 File Offset: 0x001CD8A0
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

		// Token: 0x040056B8 RID: 22200
		public Vector3 Min;

		// Token: 0x040056B9 RID: 22201
		public Vector3 Max;
	}
}
