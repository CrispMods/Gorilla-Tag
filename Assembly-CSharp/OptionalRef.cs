using System;
using UnityEngine;

// Token: 0x020006E0 RID: 1760
[Serializable]
public class OptionalRef<T> where T : UnityEngine.Object
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002BA1 RID: 11169 RVA: 0x0004D8B7 File Offset: 0x0004BAB7
	// (set) Token: 0x06002BA2 RID: 11170 RVA: 0x0004D8BF File Offset: 0x0004BABF
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

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002BA3 RID: 11171 RVA: 0x001210F8 File Offset: 0x0011F2F8
	// (set) Token: 0x06002BA4 RID: 11172 RVA: 0x00121120 File Offset: 0x0011F320
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

	// Token: 0x06002BA5 RID: 11173 RVA: 0x0012114C File Offset: 0x0011F34C
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
		UnityEngine.Object @object = r._target;
		return @object != null && @object;
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x00121180 File Offset: 0x0011F380
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
		UnityEngine.Object @object = r._target;
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

	// Token: 0x06002BA7 RID: 11175 RVA: 0x001211E4 File Offset: 0x0011F3E4
	public static implicit operator UnityEngine.Object(OptionalRef<T> r)
	{
		if (r == null)
		{
			return null;
		}
		if (!r._enabled)
		{
			return null;
		}
		UnityEngine.Object @object = r._target;
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

	// Token: 0x04003130 RID: 12592
	[SerializeField]
	private bool _enabled;

	// Token: 0x04003131 RID: 12593
	[SerializeField]
	private T _target;
}
