using System;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[Serializable]
public struct AnimHashId
{
	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003311 RID: 13073 RVA: 0x000F47E1 File Offset: 0x000F29E1
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x06003312 RID: 13074 RVA: 0x000F47E9 File Offset: 0x000F29E9
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x000F47F1 File Offset: 0x000F29F1
	public AnimHashId(string text)
	{
		this._text = text;
		this._hash = Animator.StringToHash(text);
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x000F47E1 File Offset: 0x000F29E1
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x000F47E9 File Offset: 0x000F29E9
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000F47E9 File Offset: 0x000F29E9
	public static implicit operator int(AnimHashId h)
	{
		return h._hash;
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x000F4806 File Offset: 0x000F2A06
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
