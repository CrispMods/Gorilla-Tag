using System;
using System.Collections.Generic;

// Token: 0x02000215 RID: 533
public class Watchable<T>
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00038B1F File Offset: 0x00036D1F
	// (set) Token: 0x06000C6D RID: 3181 RVA: 0x0009F564 File Offset: 0x0009D764
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

	// Token: 0x06000C6E RID: 3182 RVA: 0x00038B27 File Offset: 0x00036D27
	public Watchable()
	{
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00038B3A File Offset: 0x00036D3A
	public Watchable(T initial)
	{
		this._value = initial;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0009F5C4 File Offset: 0x0009D7C4
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

	// Token: 0x06000C71 RID: 3185 RVA: 0x00038B54 File Offset: 0x00036D54
	public void RemoveCallback(Action<T> callback)
	{
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000FAD RID: 4013
	private T _value;

	// Token: 0x04000FAE RID: 4014
	private List<Action<T>> callbacks = new List<Action<T>>();
}
