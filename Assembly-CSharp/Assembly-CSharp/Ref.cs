using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
[Serializable]
public class Ref<T> where T : class
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06002764 RID: 10084 RVA: 0x000C164B File Offset: 0x000BF84B
	// (set) Token: 0x06002765 RID: 10085 RVA: 0x000C1653 File Offset: 0x000BF853
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

	// Token: 0x06002766 RID: 10086 RVA: 0x000C1668 File Offset: 0x000BF868
	public static implicit operator bool(Ref<T> r)
	{
		Object @object = (r != null) ? r._target : null;
		return @object != null && @object != null;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x000C1690 File Offset: 0x000BF890
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

	// Token: 0x06002768 RID: 10088 RVA: 0x000C16D8 File Offset: 0x000BF8D8
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

	// Token: 0x04002B21 RID: 11041
	[SerializeField]
	private Object _target;
}
