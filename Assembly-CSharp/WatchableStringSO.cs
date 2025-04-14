using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020C RID: 524
[CreateAssetMenu(fileName = "WatchableStringSO", menuName = "ScriptableObjects/WatchableStringSO")]
public class WatchableStringSO : ScriptableObject
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00041141 File Offset: 0x0003F341
	// (set) Token: 0x06000C30 RID: 3120 RVA: 0x00041149 File Offset: 0x0003F349
	private string _value { get; set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00041152 File Offset: 0x0003F352
	// (set) Token: 0x06000C32 RID: 3122 RVA: 0x00041160 File Offset: 0x0003F360
	public string Value
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
			foreach (Action<string> action in this.callbacks)
			{
				action(value);
			}
		}
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x000411C0 File Offset: 0x0003F3C0
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<string>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x000411F4 File Offset: 0x0003F3F4
	public void AddCallback(Action<string> callback, bool shouldCallbackNow = false)
	{
		this.EnsureInitialized();
		this.callbacks.Add(callback);
		if (shouldCallbackNow)
		{
			string value = this._value;
			foreach (Action<string> action in this.callbacks)
			{
				action(value);
			}
		}
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00041264 File Offset: 0x0003F464
	public void RemoveCallback(Action<string> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00041279 File Offset: 0x0003F479
	public override string ToString()
	{
		return this.Value;
	}

	// Token: 0x04000F6D RID: 3949
	[TextArea]
	public string InitialValue;

	// Token: 0x04000F6F RID: 3951
	private EnterPlayID enterPlayID;

	// Token: 0x04000F70 RID: 3952
	private List<Action<string>> callbacks;
}
