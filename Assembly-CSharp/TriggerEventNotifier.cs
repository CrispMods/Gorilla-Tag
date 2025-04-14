using System;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class TriggerEventNotifier : MonoBehaviour
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x060034F4 RID: 13556 RVA: 0x000FCF0C File Offset: 0x000FB10C
	// (remove) Token: 0x060034F5 RID: 13557 RVA: 0x000FCF44 File Offset: 0x000FB144
	public event TriggerEventNotifier.TriggerEvent TriggerEnterEvent;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x060034F6 RID: 13558 RVA: 0x000FCF7C File Offset: 0x000FB17C
	// (remove) Token: 0x060034F7 RID: 13559 RVA: 0x000FCFB4 File Offset: 0x000FB1B4
	public event TriggerEventNotifier.TriggerEvent TriggerExitEvent;

	// Token: 0x060034F8 RID: 13560 RVA: 0x000FCFE9 File Offset: 0x000FB1E9
	private void OnTriggerEnter(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerEnterEvent = this.TriggerEnterEvent;
		if (triggerEnterEvent == null)
		{
			return;
		}
		triggerEnterEvent(this, other);
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x000FCFFD File Offset: 0x000FB1FD
	private void OnTriggerExit(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerExitEvent = this.TriggerExitEvent;
		if (triggerExitEvent == null)
		{
			return;
		}
		triggerExitEvent(this, other);
	}

	// Token: 0x04003791 RID: 14225
	[HideInInspector]
	public int maskIndex;

	// Token: 0x0200088B RID: 2187
	// (Invoke) Token: 0x060034FC RID: 13564
	public delegate void TriggerEvent(TriggerEventNotifier notifier, Collider collider);
}
