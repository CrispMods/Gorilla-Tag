using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class WatchableGenericSO<T> : ScriptableObject
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00041007 File Offset: 0x0003F207
	// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0004100F File Offset: 0x0003F20F
	private T _value { get; set; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00041018 File Offset: 0x0003F218
	// (set) Token: 0x06000C2A RID: 3114 RVA: 0x00041028 File Offset: 0x0003F228
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

	// Token: 0x06000C2B RID: 3115 RVA: 0x00041088 File Offset: 0x0003F288
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<T>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x000410BC File Offset: 0x0003F2BC
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

	// Token: 0x06000C2D RID: 3117 RVA: 0x0004112C File Offset: 0x0003F32C
	public void RemoveCallback(Action<T> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x04000F69 RID: 3945
	public T InitialValue;

	// Token: 0x04000F6B RID: 3947
	private EnterPlayID enterPlayID;

	// Token: 0x04000F6C RID: 3948
	private List<Action<T>> callbacks;
}
