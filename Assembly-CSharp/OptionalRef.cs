using System;
using UnityEngine;

// Token: 0x020006CB RID: 1739
[Serializable]
public class OptionalRef<T> where T : Object
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000D5585 File Offset: 0x000D3785
	// (set) Token: 0x06002B0C RID: 11020 RVA: 0x000D558D File Offset: 0x000D378D
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

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06002B0D RID: 11021 RVA: 0x000D5598 File Offset: 0x000D3798
	// (set) Token: 0x06002B0E RID: 11022 RVA: 0x000D55C0 File Offset: 0x000D37C0
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

	// Token: 0x06002B0F RID: 11023 RVA: 0x000D55EC File Offset: 0x000D37EC
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

	// Token: 0x06002B10 RID: 11024 RVA: 0x000D5620 File Offset: 0x000D3820
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

	// Token: 0x06002B11 RID: 11025 RVA: 0x000D5684 File Offset: 0x000D3884
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

	// Token: 0x04003093 RID: 12435
	[SerializeField]
	private bool _enabled;

	// Token: 0x04003094 RID: 12436
	[SerializeField]
	private T _target;
}
