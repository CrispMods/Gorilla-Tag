using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D5 RID: 1749
public class TaggedColliderTrigger : MonoBehaviour
{
	// Token: 0x06002B82 RID: 11138 RVA: 0x000D65E2 File Offset: 0x000D47E2
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(this.tag))
		{
			return;
		}
		if (this._sinceLastEnter.HasElapsed(this.enterHysteresis, true))
		{
			UnityEvent<Collider> unityEvent = this.onEnter;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(other);
		}
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x000D6618 File Offset: 0x000D4818
	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag(this.tag))
		{
			return;
		}
		if (this._sinceLastExit.HasElapsed(this.exitHysteresis, true))
		{
			UnityEvent<Collider> unityEvent = this.onExit;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(other);
		}
	}

	// Token: 0x040030BC RID: 12476
	public new UnityTag tag;

	// Token: 0x040030BD RID: 12477
	public UnityEvent<Collider> onEnter = new UnityEvent<Collider>();

	// Token: 0x040030BE RID: 12478
	public UnityEvent<Collider> onExit = new UnityEvent<Collider>();

	// Token: 0x040030BF RID: 12479
	public float enterHysteresis = 0.125f;

	// Token: 0x040030C0 RID: 12480
	public float exitHysteresis = 0.125f;

	// Token: 0x040030C1 RID: 12481
	private TimeSince _sinceLastEnter;

	// Token: 0x040030C2 RID: 12482
	private TimeSince _sinceLastExit;
}
