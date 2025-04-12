using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class WatchableGenericSO<T> : ScriptableObject
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000C29 RID: 3113 RVA: 0x000378A3 File Offset: 0x00035AA3
	// (set) Token: 0x06000C2A RID: 3114 RVA: 0x000378AB File Offset: 0x00035AAB
	private T _value { get; set; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000C2B RID: 3115 RVA: 0x000378B4 File Offset: 0x00035AB4
	// (set) Token: 0x06000C2C RID: 3116 RVA: 0x0009CDA0 File Offset: 0x0009AFA0
	public T Value
	{
		get
		{
			this.EnsureInitialized();
			return this._value;
		}
		set
		{
			this.EnsureInitialized();
			this._value = value;
			foreach (Action<T> action in this.callbacks)
			{
				action(value);
			}
		}
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000378C2 File Offset: 0x00035AC2
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<T>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x0009CE00 File Offset: 0x0009B000
	public void AddCallback(Action<T> callback, bool shouldCallbackNow = false)
	{
		this.EnsureInitialized();
		this.callbacks.Add(callback);
		if (shouldCallbackNow)
		{
			T value = this._value;
			foreach (Action<T> action in this.callbacks)
			{
				action(value);
			}
		}
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000378F3 File Offset: 0x00035AF3
	public void RemoveCallback(Action<T> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000F6A RID: 3946
	public T InitialValue;

	// Token: 0x04000F6C RID: 3948
	private EnterPlayID enterPlayID;

	// Token: 0x04000F6D RID: 3949
	private List<Action<T>> callbacks;
}
