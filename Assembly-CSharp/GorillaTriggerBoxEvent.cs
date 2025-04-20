using System;
using UnityEngine.Events;

// Token: 0x02000486 RID: 1158
public class GorillaTriggerBoxEvent : GorillaTriggerBox
{
	// Token: 0x06001C2F RID: 7215 RVA: 0x0004361C File Offset: 0x0004181C
	public override void OnBoxTriggered()
	{
		if (this.onBoxTriggered != null)
		{
			this.onBoxTriggered.Invoke();
		}
	}

	// Token: 0x04001F38 RID: 7992
	public UnityEvent onBoxTriggered;
}
