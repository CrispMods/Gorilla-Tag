using System;
using UnityEngine;

// Token: 0x02000827 RID: 2087
[Serializable]
public struct AnimHashId
{
	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06003305 RID: 13061 RVA: 0x000F4219 File Offset: 0x000F2419
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003306 RID: 13062 RVA: 0x000F4221 File Offset: 0x000F2421
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x000F4229 File Offset: 0x000F2429
	public AnimHashId(string text)
	{
		this._text = text;
		this._hash = Animator.StringToHash(text);
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x000F4219 File Offset: 0x000F2419
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x000F4221 File Offset: 0x000F2421
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x000F4221 File Offset: 0x000F2421
	public static implicit operator int(AnimHashId h)
	{
		return h._hash;
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000F423E File Offset: 0x000F243E
	public static implicit operator AnimHashId(string s)
	{
		return new AnimHashId(s);
	}

	// Token: 0x0400367D RID: 13949
	[SerializeField]
	private string _text;

	// Token: 0x0400367E RID: 13950
	[NonSerialized]
	private int _hash;
}
