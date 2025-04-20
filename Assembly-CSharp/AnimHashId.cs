using System;
using UnityEngine;

// Token: 0x02000841 RID: 2113
[Serializable]
public struct AnimHashId
{
	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000520AA File Offset: 0x000502AA
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000520B2 File Offset: 0x000502B2
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x000520BA File Offset: 0x000502BA
	public AnimHashId(string text)
	{
		this._text = text;
		this._hash = Animator.StringToHash(text);
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x000520AA File Offset: 0x000502AA
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x000520B2 File Offset: 0x000502B2
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x000520B2 File Offset: 0x000502B2
	public static implicit operator int(AnimHashId h)
	{
		return h._hash;
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x000520CF File Offset: 0x000502CF
	public static implicit operator AnimHashId(string s)
	{
		return new AnimHashId(s);
	}

	// Token: 0x04003739 RID: 14137
	[SerializeField]
	private string _text;

	// Token: 0x0400373A RID: 14138
	[NonSerialized]
	private int _hash;
}
