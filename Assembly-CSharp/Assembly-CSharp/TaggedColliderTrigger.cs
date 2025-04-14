using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D6 RID: 1750
public class TaggedColliderTrigger : MonoBehaviour
{
	// Token: 0x06002B8A RID: 11146 RVA: 0x000D6A62 File Offset: 0x000D4C62
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

	// Token: 0x06002B8B RID: 11147 RVA: 0x000D6A98 File Offset: 0x000D4C98
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

	// Token: 0x040030C2 RID: 12482
	public new UnityTag tag;

	// Token: 0x040030C3 RID: 12483
	public UnityEvent<Collider> onEnter = new UnityEvent<Collider>();

	// Token: 0x040030C4 RID: 12484
	public UnityEvent<Collider> onExit = new UnityEvent<Collider>();

	// Token: 0x040030C5 RID: 12485
	public float enterHysteresis = 0.125f;

	// Token: 0x040030C6 RID: 12486
	public float exitHysteresis = 0.125f;

	// Token: 0x040030C7 RID: 12487
	private TimeSince _sinceLastEnter;

	// Token: 0x040030C8 RID: 12488
	private TimeSince _sinceLastExit;
}
