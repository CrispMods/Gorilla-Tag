using System;
using UnityEngine.Events;

// Token: 0x0200047A RID: 1146
public class GorillaTriggerBoxEvent : GorillaTriggerBox
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x00087CEF File Offset: 0x00085EEF
	public override void OnBoxTriggered()
	{
		if (this.onBoxTriggered != null)
		{
			this.onBoxTriggered.Invoke();
		}
	}

	// Token: 0x04001EE9 RID: 7913
	public UnityEvent onBoxTriggered;
}
