using System;
using UnityEngine;

// Token: 0x020006CC RID: 1740
[Serializable]
public class OptionalRef<T> where T : Object
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06002B13 RID: 11027 RVA: 0x000D5A05 File Offset: 0x000D3C05
	// (set) Token: 0x06002B14 RID: 11028 RVA: 0x000D5A0D File Offset: 0x000D3C0D
	public bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000D5A18 File Offset: 0x000D3C18
	// (set) Token: 0x06002B16 RID: 11030 RVA: 0x000D5A40 File Offset: 0x000D3C40
	public T Value
	{
		get
		{
			if (this)
			{
				return this._target;
			}
			return default(T);
		}
		set
		{
			this._target = (value ? value : default(T));
		}
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x000D5A6C File Offset: 0x000D3C6C
	public static implicit operator bool(OptionalRef<T> r)
	{
		if (r == null)
		{
			return false;
		}
		if (!r._enabled)
		{
			return false;
		}
		Object @object = r._target;
		return @object != null && @object;
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x000D5AA0 File Offset: 0x000D3CA0
	public static implicit operator T(OptionalRef<T> r)
	{
		if (r == null)
		{
			return default(T);
		}
		if (!r._enabled)
		{
			return default(T);
		}
		Object @object = r._target;
		if (@object == null)
		{
			return default(T);
		}
		if (!@object)
		{
			return default(T);
		}
		return @object as T;
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000D5B04 File Offset: 0x000D3D04
	public static implicit operator Object(OptionalRef<T> r)
	{
		if (r == null)
		{
			return null;
		}
		if (!r._enabled)
		{
			return null;
		}
		Object @object = r._target;
		if (@object == null)
		{
			return null;
		}
		if (!@object)
		{
			return null;
		}
		return @object;
	}

	// Token: 0x04003099 RID: 12441
	[SerializeField]
	private bool _enabled;

	// Token: 0x0400309A RID: 12442
	[SerializeField]
	private T _target;
}
