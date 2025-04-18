﻿using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B91 RID: 2961
	[Serializable]
	public struct HashWrapper : IEquatable<int>
	{
		// Token: 0x06004AC2 RID: 19138 RVA: 0x000604F6 File Offset: 0x0005E6F6
		public override int GetHashCode()
		{
			return this.hashCode;
		}

		// Token: 0x06004AC3 RID: 19139 RVA: 0x000604FE File Offset: 0x0005E6FE
		public override bool Equals(object obj)
		{
			return this.hashCode.Equals(obj);
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x0006050C File Offset: 0x0005E70C
		public bool Equals(int i)
		{
			return this.hashCode.Equals(i);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x000604F6 File Offset: 0x0005E6F6
		public static implicit operator int(in HashWrapper hash)
		{
			return hash.hashCode;
		}

		// Token: 0x04004C3B RID: 19515
		[SerializeField]
		private int hashCode;
	}
}
