using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000870 RID: 2160
[Serializable]
public struct ShaderHashId : IEquatable<ShaderHashId>
{
	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003439 RID: 13369 RVA: 0x000F8963 File Offset: 0x000F6B63
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x0600343A RID: 13370 RVA: 0x000F896B File Offset: 0x000F6B6B
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x000F8973 File Offset: 0x000F6B73
	public ShaderHashId(string text)
	{
		this._text = text;
		this._hash = Shader.PropertyToID(text);
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x000F8963 File Offset: 0x000F6B63
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x000F896B File Offset: 0x000F6B6B
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x0600343E RID: 13374 RVA: 0x000F896B File Offset: 0x000F6B6B
	public static implicit operator int(ShaderHashId h)
	{
		return h._hash;
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x000F8988 File Offset: 0x000F6B88
	public static implicit operator ShaderHashId(string s)
	{
		return new ShaderHashId(s);
	}

	// Token: 0x06003440 RID: 13376 RVA: 0x000F8990 File Offset: 0x000F6B90
	public bool Equals(ShaderHashId other)
	{
		return this._hash == other._hash;
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x000F89A0 File Offset: 0x000F6BA0
	public override bool Equals(object obj)
	{
		if (obj is ShaderHashId)
		{
			ShaderHashId other = (ShaderHashId)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x000F89C5 File Offset: 0x000F6BC5
	public static bool operator ==(ShaderHashId x, ShaderHashId y)
	{
		return x.Equals(y);
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x000F89CF File Offset: 0x000F6BCF
	public static bool operator !=(ShaderHashId x, ShaderHashId y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0400371F RID: 14111
	[FormerlySerializedAs("_hashText")]
	[SerializeField]
	private string _text;

	// Token: 0x04003720 RID: 14112
	[NonSerialized]
	private int _hash;
}
