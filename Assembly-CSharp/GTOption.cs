using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
[Serializable]
public struct GTOption<T>
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00038862 File Offset: 0x00036A62
	public T ResolvedValue
	{
		get
		{
			if (!this.enabled)
			{
				return this.defaultValue;
			}
			return this.value;
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00038879 File Offset: 0x00036A79
	public GTOption(T defaultValue)
	{
		this.enabled = false;
		this.value = defaultValue;
		this.defaultValue = defaultValue;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00038890 File Offset: 0x00036A90
	public void ResetValue()
	{
		this.value = this.defaultValue;
	}

	// Token: 0x04000CB0 RID: 3248
	[Tooltip("When checked, the filter is applied; when unchecked (default), it is ignored.")]
	[SerializeField]
	public bool enabled;

	// Token: 0x04000CB1 RID: 3249
	[SerializeField]
	public T value;

	// Token: 0x04000CB2 RID: 3250
	[NonSerialized]
	public readonly T defaultValue;
}
