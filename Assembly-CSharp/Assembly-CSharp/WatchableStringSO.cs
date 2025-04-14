using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020C RID: 524
[CreateAssetMenu(fileName = "WatchableStringSO", menuName = "ScriptableObjects/WatchableStringSO")]
public class WatchableStringSO : ScriptableObject
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00041485 File Offset: 0x0003F685
	// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0004148D File Offset: 0x0003F68D
	private string _value { get; set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00041496 File Offset: 0x0003F696
	// (set) Token: 0x06000C34 RID: 3124 RVA: 0x000414A4 File Offset: 0x0003F6A4
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

	// Token: 0x06000C35 RID: 3125 RVA: 0x00041504 File Offset: 0x0003F704
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<string>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00041538 File Offset: 0x0003F738
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

	// Token: 0x06000C37 RID: 3127 RVA: 0x000415A8 File Offset: 0x0003F7A8
	public void RemoveCallback(Action<string> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x000415BD File Offset: 0x0003F7BD
	public override string ToString()
	{
		return this.Value;
	}

	// Token: 0x04000F6E RID: 3950
	[TextArea]
	public string InitialValue;

	// Token: 0x04000F70 RID: 3952
	private EnterPlayID enterPlayID;

	// Token: 0x04000F71 RID: 3953
	private List<Action<string>> callbacks;
}
