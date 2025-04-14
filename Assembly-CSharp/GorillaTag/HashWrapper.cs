using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B8E RID: 2958
	[Serializable]
	public struct HashWrapper : IEquatable<int>
	{
		// Token: 0x06004AB6 RID: 19126 RVA: 0x00169A13 File Offset: 0x00167C13
		public override int GetHashCode()
		{
			return this.hashCode;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x00169A1B File Offset: 0x00167C1B
		public override bool Equals(object obj)
		{
			return this.hashCode.Equals(obj);
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x00169A29 File Offset: 0x00167C29
		public bool Equals(int i)
		{
			return this.hashCode.Equals(i);
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x00169A13 File Offset: 0x00167C13
		public static implicit operator int(in HashWrapper hash)
		{
			return hash.hashCode;
		}

		// Token: 0x04004C29 RID: 19497
		[SerializeField]
		private int hashCode;
	}
}
