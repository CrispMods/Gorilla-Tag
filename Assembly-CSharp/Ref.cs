using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
[Serializable]
public class Ref<T> where T : class
{
	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x0600275C RID: 10076 RVA: 0x000C11CB File Offset: 0x000BF3CB
	// (set) Token: 0x0600275D RID: 10077 RVA: 0x000C11D3 File Offset: 0x000BF3D3
	public T AsT
	{
		get
		{
			return this;
		}
		set
		{
			this._target = (value as Object);
		}
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x000C11E8 File Offset: 0x000BF3E8
	public static implicit operator bool(Ref<T> r)
	{
		Object @object = (r != null) ? r._target : null;
		return @object != null && @object != null;
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x000C1210 File Offset: 0x000BF410
	public static implicit operator T(Ref<T> r)
	{
		Object @object = (r != null) ? r._target : null;
		if (@object == null)
		{
			return default(T);
		}
		if (@object == null)
		{
			return default(T);
		}
		return @object as T;
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000C1258 File Offset: 0x000BF458
	public static implicit operator Object(Ref<T> r)
	{
		Object @object = (r != null) ? r._target : null;
		if (@object == null)
		{
			return null;
		}
		if (@object == null)
		{
			return null;
		}
		return @object;
	}

	// Token: 0x04002B1B RID: 11035
	[SerializeField]
	private Object _target;
}
