using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
[Serializable]
public struct GTOption<T>
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000364B4 File Offset: 0x000346B4
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

	// Token: 0x06000A65 RID: 2661 RVA: 0x000364CB File Offset: 0x000346CB
	public GTOption(T defaultValue)
	{
		this.enabled = false;
		this.value = defaultValue;
		this.defaultValue = defaultValue;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000364E2 File Offset: 0x000346E2
	public void ResetValue()
	{
		this.value = this.defaultValue;
	}

	// Token: 0x04000CB1 RID: 3249
	[Tooltip("When checked, the filter is applied; when unchecked (default), it is ignored.")]
	[SerializeField]
	public bool enabled;

	// Token: 0x04000CB2 RID: 3250
	[SerializeField]
	public T value;

	// Token: 0x04000CB3 RID: 3251
	[NonSerialized]
	public readonly T defaultValue;
}
