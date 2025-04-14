using System;
using System.Collections.Generic;

// Token: 0x0200020A RID: 522
public class Watchable<T>
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0004123C File Offset: 0x0003F43C
	// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00041244 File Offset: 0x0003F444
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

	// Token: 0x06000C25 RID: 3109 RVA: 0x000412A4 File Offset: 0x0003F4A4
	public Watchable()
	{
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000412B7 File Offset: 0x0003F4B7
	public Watchable(T initial)
	{
		this._value = initial;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000412D4 File Offset: 0x0003F4D4
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

	// Token: 0x06000C28 RID: 3112 RVA: 0x0004133C File Offset: 0x0003F53C
	public void RemoveCallback(Action<T> callback)
	{
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000F68 RID: 3944
	private T _value;

	// Token: 0x04000F69 RID: 3945
	private List<Action<T>> callbacks = new List<Action<T>>();
}
