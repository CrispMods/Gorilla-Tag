using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class WatchableGenericSO<T> : ScriptableObject
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00038B63 File Offset: 0x00036D63
	// (set) Token: 0x06000C73 RID: 3187 RVA: 0x00038B6B File Offset: 0x00036D6B
	private T _value { get; set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00038B74 File Offset: 0x00036D74
	// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0009F62C File Offset: 0x0009D82C
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

	// Token: 0x06000C76 RID: 3190 RVA: 0x00038B82 File Offset: 0x00036D82
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<T>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0009F68C File Offset: 0x0009D88C
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

	// Token: 0x06000C78 RID: 3192 RVA: 0x00038BB3 File Offset: 0x00036DB3
	public void RemoveCallback(Action<T> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000FAF RID: 4015
	public T InitialValue;

	// Token: 0x04000FB1 RID: 4017
	private EnterPlayID enterPlayID;

	// Token: 0x04000FB2 RID: 4018
	private List<Action<T>> callbacks;
}
