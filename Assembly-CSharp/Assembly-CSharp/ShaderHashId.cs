using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000873 RID: 2163
[Serializable]
public struct ShaderHashId : IEquatable<ShaderHashId>
{
	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06003445 RID: 13381 RVA: 0x000F8F2B File Offset: 0x000F712B
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x000F8F33 File Offset: 0x000F7133
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x000F8F3B File Offset: 0x000F713B
	public ShaderHashId(string text)
	{
		this._text = text;
		this._hash = Shader.PropertyToID(text);
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x000F8F2B File Offset: 0x000F712B
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x000F8F33 File Offset: 0x000F7133
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x000F8F33 File Offset: 0x000F7133
	public static implicit operator int(ShaderHashId h)
	{
		return h._hash;
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x000F8F50 File Offset: 0x000F7150
	public static implicit operator ShaderHashId(string s)
	{
		return new ShaderHashId(s);
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x000F8F58 File Offset: 0x000F7158
	public bool Equals(ShaderHashId other)
	{
		return this._hash == other._hash;
	}

	// Token: 0x0600344D RID: 13389 RVA: 0x000F8F68 File Offset: 0x000F7168
	public override bool Equals(object obj)
	{
		if (obj is ShaderHashId)
		{
			ShaderHashId other = (ShaderHashId)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x000F8F8D File Offset: 0x000F718D
	public static bool operator ==(ShaderHashId x, ShaderHashId y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x000F8F97 File Offset: 0x000F7197
	public static bool operator !=(ShaderHashId x, ShaderHashId y)
	{
		return !x.Equals(y);
	}

	// Token: 0x04003731 RID: 14129
	[FormerlySerializedAs("_hashText")]
	[SerializeField]
	private string _text;

	// Token: 0x04003732 RID: 14130
	[NonSerialized]
	private int _hash;
}
