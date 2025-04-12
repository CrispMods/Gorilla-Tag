using System;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[Serializable]
public struct AnimHashId
{
	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003311 RID: 13073 RVA: 0x00050C9C File Offset: 0x0004EE9C
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003312 RID: 13074 RVA: 0x00050CA4 File Offset: 0x0004EEA4
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x00050CAC File Offset: 0x0004EEAC
	public AnimHashId(string text)
	{
		this._text = text;
		this._hash = Animator.StringToHash(text);
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x00050C9C File Offset: 0x0004EE9C
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x00050CA4 File Offset: 0x0004EEA4
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x00050CA4 File Offset: 0x0004EEA4
	public static implicit operator int(AnimHashId h)
	{
		return h._hash;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x00050CC1 File Offset: 0x0004EEC1
	public static implicit operator AnimHashId(string s)
	{
		return new AnimHashId(s);
	}

	// Token: 0x0400368F RID: 13967
	[SerializeField]
	private string _text;

	// Token: 0x04003690 RID: 13968
	[NonSerialized]
	private int _hash;
}
