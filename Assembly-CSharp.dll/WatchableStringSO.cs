using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020C RID: 524
[CreateAssetMenu(fileName = "WatchableStringSO", menuName = "ScriptableObjects/WatchableStringSO")]
public class WatchableStringSO : ScriptableObject
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00037908 File Offset: 0x00035B08
	// (set) Token: 0x06000C32 RID: 3122 RVA: 0x00037910 File Offset: 0x00035B10
	private string _value { get; set; }

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00037919 File Offset: 0x00035B19
	// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0009CE70 File Offset: 0x0009B070
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

	// Token: 0x06000C35 RID: 3125 RVA: 0x00037927 File Offset: 0x00035B27
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<string>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0009CED0 File Offset: 0x0009B0D0
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

	// Token: 0x06000C37 RID: 3127 RVA: 0x00037958 File Offset: 0x00035B58
	public void RemoveCallback(Action<string> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0003796D File Offset: 0x00035B6D
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
