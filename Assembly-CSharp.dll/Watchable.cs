using System;
using System.Collections.Generic;

// Token: 0x0200020A RID: 522
public class Watchable<T>
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0003785F File Offset: 0x00035A5F
	// (set) Token: 0x06000C24 RID: 3108 RVA: 0x0009CCD8 File Offset: 0x0009AED8
	public T value
	{
		get
		{
			return this._value;
		}
		set
		{
			T value2 = this._value;
			this._value = value;
			foreach (Action<T> action in this.callbacks)
			{
				action(value);
			}
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00037867 File Offset: 0x00035A67
	public Watchable()
	{
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0003787A File Offset: 0x00035A7A
	public Watchable(T initial)
	{
		this._value = initial;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0009CD38 File Offset: 0x0009AF38
	public void AddCallback(Action<T> callback, bool shouldCallbackNow = false)
	{
		this.callbacks.Add(callback);
		if (shouldCallbackNow)
		{
			foreach (Action<T> action in this.callbacks)
			{
				action(this._value);
			}
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x00037894 File Offset: 0x00035A94
	public void RemoveCallback(Action<T> callback)
	{
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000F68 RID: 3944
	private T _value;

	// Token: 0x04000F69 RID: 3945
	private List<Action<T>> callbacks = new List<Action<T>>();
}
