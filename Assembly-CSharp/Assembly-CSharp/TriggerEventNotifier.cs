using System;
using UnityEngine;

// Token: 0x0200088D RID: 2189
public class TriggerEventNotifier : MonoBehaviour
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06003500 RID: 13568 RVA: 0x000FD4D4 File Offset: 0x000FB6D4
	// (remove) Token: 0x06003501 RID: 13569 RVA: 0x000FD50C File Offset: 0x000FB70C
	public event TriggerEventNotifier.TriggerEvent TriggerEnterEvent;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06003502 RID: 13570 RVA: 0x000FD544 File Offset: 0x000FB744
	// (remove) Token: 0x06003503 RID: 13571 RVA: 0x000FD57C File Offset: 0x000FB77C
	public event TriggerEventNotifier.TriggerEvent TriggerExitEvent;

	// Token: 0x06003504 RID: 13572 RVA: 0x000FD5B1 File Offset: 0x000FB7B1
	private void OnTriggerEnter(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerEnterEvent = this.TriggerEnterEvent;
		if (triggerEnterEvent == null)
		{
			return;
		}
		triggerEnterEvent(this, other);
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x000FD5C5 File Offset: 0x000FB7C5
	private void OnTriggerExit(Collider other)
	{
		TriggerEventNotifier.TriggerEvent triggerExitEvent = this.TriggerExitEvent;
		if (triggerExitEvent == null)
		{
			return;
		}
		triggerExitEvent(this, other);
	}

	// Token: 0x040037A3 RID: 14243
	[HideInInspector]
	public int maskIndex;

	// Token: 0x0200088E RID: 2190
	// (Invoke) Token: 0x06003508 RID: 13576
	public delegate void TriggerEvent(TriggerEventNotifier notifier, Collider collider);
}
