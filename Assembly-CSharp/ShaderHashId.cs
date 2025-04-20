using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200088C RID: 2188
[Serializable]
public struct ShaderHashId : IEquatable<ShaderHashId>
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003505 RID: 13573 RVA: 0x00052F42 File Offset: 0x00051142
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003506 RID: 13574 RVA: 0x00052F4A File Offset: 0x0005114A
	public int hash
	{
		get
		{
			return this._hash;
		}
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x00052F52 File Offset: 0x00051152
	public ShaderHashId(string text)
	{
		this._text = text;
		this._hash = Shader.PropertyToID(text);
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x00052F42 File Offset: 0x00051142
	public override string ToString()
	{
		return this._text;
	}

	// Token: 0x06003509 RID: 13577 RVA: 0x00052F4A File Offset: 0x0005114A
	public override int GetHashCode()
	{
		return this._hash;
	}

	// Token: 0x0600350A RID: 13578 RVA: 0x00052F4A File Offset: 0x0005114A
	public static implicit operator int(ShaderHashId h)
	{
		return h._hash;
	}

	// Token: 0x0600350B RID: 13579 RVA: 0x00052F67 File Offset: 0x00051167
	public static implicit operator ShaderHashId(string s)
	{
		return new ShaderHashId(s);
	}

	// Token: 0x0600350C RID: 13580 RVA: 0x00052F6F File Offset: 0x0005116F
	public bool Equals(ShaderHashId other)
	{
		return this._hash == other._hash;
	}

	// Token: 0x0600350D RID: 13581 RVA: 0x0013F96C File Offset: 0x0013DB6C
	public override bool Equals(object obj)
	{
		if (obj is ShaderHashId)
		{
			ShaderHashId other = (ShaderHashId)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x00052F7F File Offset: 0x0005117F
	public static bool operator ==(ShaderHashId x, ShaderHashId y)
	{
		return x.Equals(y);
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x00052F89 File Offset: 0x00051189
	public static bool operator !=(ShaderHashId x, ShaderHashId y)
	{
		return !x.Equals(y);
	}

	// Token: 0x040037DF RID: 14303
	[FormerlySerializedAs("_hashText")]
	[SerializeField]
	private string _text;

	// Token: 0x040037E0 RID: 14304
	[NonSerialized]
	private int _hash;
}
