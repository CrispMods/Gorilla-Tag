using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
[Serializable]
public struct GTOption<T>
{
	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00037774 File Offset: 0x00035974
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

	// Token: 0x06000AAF RID: 2735 RVA: 0x0003778B File Offset: 0x0003598B
	public GTOption(T defaultValue)
	{
		this.enabled = false;
		this.value = defaultValue;
		this.defaultValue = defaultValue;
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x000377A2 File Offset: 0x000359A2
	public void ResetValue()
	{
		this.value = this.defaultValue;
	}

	// Token: 0x04000CF6 RID: 3318
	[Tooltip("When checked, the filter is applied; when unchecked (default), it is ignored.")]
	[SerializeField]
	public bool enabled;

	// Token: 0x04000CF7 RID: 3319
	[SerializeField]
	public T value;

	// Token: 0x04000CF8 RID: 3320
	[NonSerialized]
	public readonly T defaultValue;
}
