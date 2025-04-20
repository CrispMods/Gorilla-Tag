using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006EA RID: 1770
public class TaggedColliderTrigger : MonoBehaviour
{
	// Token: 0x06002C18 RID: 11288 RVA: 0x0004DE40 File Offset: 0x0004C040
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

	// Token: 0x06002C19 RID: 11289 RVA: 0x0004DE76 File Offset: 0x0004C076
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

	// Token: 0x04003159 RID: 12633
	public new UnityTag tag;

	// Token: 0x0400315A RID: 12634
	public UnityEvent<Collider> onEnter = new UnityEvent<Collider>();

	// Token: 0x0400315B RID: 12635
	public UnityEvent<Collider> onExit = new UnityEvent<Collider>();

	// Token: 0x0400315C RID: 12636
	public float enterHysteresis = 0.125f;

	// Token: 0x0400315D RID: 12637
	public float exitHysteresis = 0.125f;

	// Token: 0x0400315E RID: 12638
	private TimeSince _sinceLastEnter;

	// Token: 0x0400315F RID: 12639
	private TimeSince _sinceLastExit;
}
