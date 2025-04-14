using System;
using System.Collections.Generic;

// Token: 0x0200020A RID: 522
public class Watchable<T>
{
	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00040EF8 File Offset: 0x0003F0F8
	// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00040F00 File Offset: 0x0003F100
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

	// Token: 0x06000C23 RID: 3107 RVA: 0x00040F60 File Offset: 0x0003F160
	public Watchable()
	{
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00040F73 File Offset: 0x0003F173
	public Watchable(T initial)
	{
		this._value = initial;
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00040F90 File Offset: 0x0003F190
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

	// Token: 0x06000C26 RID: 3110 RVA: 0x00040FF8 File Offset: 0x0003F1F8
	public void RemoveCallback(Action<T> callback)
	{
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000F67 RID: 3943
	private T _value;

	// Token: 0x04000F68 RID: 3944
	private List<Action<T>> callbacks = new List<Action<T>>();
}
