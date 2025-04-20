using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BBB RID: 3003
	[Serializable]
	public struct HashWrapper : IEquatable<int>
	{
		// Token: 0x06004C01 RID: 19457 RVA: 0x00061F2E File Offset: 0x0006012E
		public override int GetHashCode()
		{
			return this.hashCode;
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x00061F36 File Offset: 0x00060136
		public override bool Equals(object obj)
		{
			return this.hashCode.Equals(obj);
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00061F44 File Offset: 0x00060144
		public bool Equals(int i)
		{
			return this.hashCode.Equals(i);
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x00061F2E File Offset: 0x0006012E
		public static implicit operator int(in HashWrapper hash)
		{
			return hash.hashCode;
		}

		// Token: 0x04004D1F RID: 19743
		[SerializeField]
		private int hashCode;
	}
}
