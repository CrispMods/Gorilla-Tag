using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000217 RID: 535
[CreateAssetMenu(fileName = "WatchableStringSO", menuName = "ScriptableObjects/WatchableStringSO")]
public class WatchableStringSO : ScriptableObject
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00038BC8 File Offset: 0x00036DC8
	// (set) Token: 0x06000C7B RID: 3195 RVA: 0x00038BD0 File Offset: 0x00036DD0
	private string _value { get; set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00038BD9 File Offset: 0x00036DD9
	// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0009F6FC File Offset: 0x0009D8FC
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

	// Token: 0x06000C7E RID: 3198 RVA: 0x00038BE7 File Offset: 0x00036DE7
	private void EnsureInitialized()
	{
		if (!this.enterPlayID.IsCurrent)
		{
			this._value = this.InitialValue;
			this.callbacks = new List<Action<string>>();
			this.enterPlayID = EnterPlayID.GetCurrent();
		}
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0009F75C File Offset: 0x0009D95C
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

	// Token: 0x06000C80 RID: 3200 RVA: 0x00038C18 File Offset: 0x00036E18
	public void RemoveCallback(Action<string> callback)
	{
		this.EnsureInitialized();
		this.callbacks.Remove(callback);
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00038C2D File Offset: 0x00036E2D
	public override string ToString()
	{
		return this.Value;
	}

	// Token: 0x04000FB3 RID: 4019
	[TextArea]
	public string InitialValue;

	// Token: 0x04000FB5 RID: 4021
	private EnterPlayID enterPlayID;

	// Token: 0x04000FB6 RID: 4022
	private List<Action<string>> callbacks;
}
