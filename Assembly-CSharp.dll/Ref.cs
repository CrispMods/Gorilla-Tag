using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
[Serializable]
public class Ref<T> where T : class
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06002764 RID: 10084 RVA: 0x00049F64 File Offset: 0x00048164
	// (set) Token: 0x06002765 RID: 10085 RVA: 0x00049F6C File Offset: 0x0004816C
	public T AsT
	{
		get
		{
			return this;
		}
		set
		{
			this._target = (value as UnityEngine.Object);
		}
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x0010A794 File Offset: 0x00108994
	public static implicit operator bool(Ref<T> r)
	{
		UnityEngine.Object @object = (r != null) ? r._target : null;
		return @object != null && @object != null;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x0010A7BC File Offset: 0x001089BC
	public static implicit operator T(Ref<T> r)
	{
		UnityEngine.Object @object = (r != null) ? r._target : null;
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

	// Token: 0x06002768 RID: 10088 RVA: 0x0010A804 File Offset: 0x00108A04
	public static implicit operator UnityEngine.Object(Ref<T> r)
	{
		UnityEngine.Object @object = (r != null) ? r._target : null;
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

	// Token: 0x04002B21 RID: 11041
	[SerializeField]
	private UnityEngine.Object _target;
}
