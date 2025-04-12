using System;
using UnityEngine.Events;

// Token: 0x0200047A RID: 1146
public class GorillaTriggerBoxEvent : GorillaTriggerBox
{
	// Token: 0x06001BDE RID: 7134 RVA: 0x000422E3 File Offset: 0x000404E3
	public override void OnBoxTriggered()
	{
		if (this.onBoxTriggered != null)
		{
			this.onBoxTriggered.Invoke();
		}
	}

	// Token: 0x04001EEA RID: 7914
	public UnityEvent onBoxTriggered;
}
