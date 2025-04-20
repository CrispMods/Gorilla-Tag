using System;
using UnityEngine;

// Token: 0x020008A6 RID: 2214
public class TriggerEventNotifier : MonoBehaviour
{
	// Token: 0x14000069 RID: 105
	// (add) Token: 0x060035C0 RID: 13760 RVA: 0x001438C4 File Offset: 0x00141AC4
	// (remove) Token: 0x060035C1 RID: 13761 RVA: 0x001438FC File Offset: 0x00141AFC
	public event TriggerEventNotifier.TriggerEvent TriggerEnterEvent;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x060035C2 RID: 13762 RVA: 0x00143934 File Offset: 0x00141B34
	// (remove) Token: 0x060035C3 RID: 13763 RVA: 0x0014396C File Offset: 0x00141B6C
	public event TriggerEventNotifier.TriggerEvent TriggerExitEvent;

	// Token: 0x060035C4 RID: 13764 RVA: 0x00053492 File Offset: 0x00051692
	private void OnTriggerEnter(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerEnterEvent = this.TriggerEnterEvent;
		if (triggerEnterEvent == null)
		{
			return;
		}
		triggerEnterEvent(this, other);
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x000534A6 File Offset: 0x000516A6
	private void OnTriggerExit(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerExitEvent = this.TriggerExitEvent;
		if (triggerExitEvent == null)
		{
			return;
		}
		triggerExitEvent(this, other);
	}

	// Token: 0x04003851 RID: 14417
	[HideInInspector]
	public int maskIndex;

	// Token: 0x020008A7 RID: 2215
	// (Invoke) Token: 0x060035C8 RID: 13768
	public delegate void TriggerEvent(TriggerEventNotifier notifier, Collider collider);
}
