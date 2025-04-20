using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
[Serializable]
public struct AnimStateHash
{
	// Token: 0x06002AD7 RID: 10967 RVA: 0x0011F450 File Offset: 0x0011D650
	public static implicit operator AnimStateHash(string s)
	{
		return new AnimStateHash
		{
			_hash = Animator.StringToHash(s)
		};
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x0004CE40 File Offset: 0x0004B040
	public static implicit operator int(AnimStateHash ash)
	{
		return ash._hash;
	}

	// Token: 0x04003062 RID: 12386
	[SerializeField]
	private int _hash;
}
