using System;
using UnityEngine;

// Token: 0x020006A3 RID: 1699
[Serializable]
public struct AnimStateHash
{
	// Token: 0x06002A49 RID: 10825 RVA: 0x000D32F0 File Offset: 0x000D14F0
	public static implicit operator AnimStateHash(string s)
	{
		return new AnimStateHash
		{
			_hash = Animator.StringToHash(s)
		};
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000D3313 File Offset: 0x000D1513
	public static implicit operator int(AnimStateHash ash)
	{
		return ash._hash;
	}

	// Token: 0x04002FCB RID: 12235
	[SerializeField]
	private int _hash;
}
