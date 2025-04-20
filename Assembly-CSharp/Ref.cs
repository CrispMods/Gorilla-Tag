using System;
using UnityEngine;

// Token: 0x0200060C RID: 1548
[Serializable]
public class Ref<T> where T : class
{
	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06002687 RID: 9863 RVA: 0x0004A4F9 File Offset: 0x000486F9
	// (set) Token: 0x06002688 RID: 9864 RVA: 0x0004A501 File Offset: 0x00048701
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

	// Token: 0x06002689 RID: 9865 RVA: 0x00108B80 File Offset: 0x00106D80
	public static implicit operator bool(Ref<T> r)
	{
		UnityEngine.Object @object = (r != null) ? r._target : null;
		return @object != null && @object != null;
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x00108BA8 File Offset: 0x00106DA8
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

	// Token: 0x0600268B RID: 9867 RVA: 0x00108BF0 File Offset: 0x00106DF0
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

	// Token: 0x04002A81 RID: 10881
	[SerializeField]
	private UnityEngine.Object _target;
}
