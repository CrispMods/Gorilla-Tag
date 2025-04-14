using System;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
[Serializable]
public struct AnimStateHash
{
	// Token: 0x06002A41 RID: 10817 RVA: 0x000D2E70 File Offset: 0x000D1070
	public static implicit operator AnimStateHash(string s)
	{
		return new AnimStateHash
		{
			_hash = Animator.StringToHash(s)
		};
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000D2E93 File Offset: 0x000D1093
	public static implicit operator int(AnimStateHash ash)
	{
		return ash._hash;
	}

	// Token: 0x04002FC5 RID: 12229
	[SerializeField]
	private int _hash;
}
