using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C7 RID: 2247
public class ThrowableBugBeaconActivation : MonoBehaviour
{
	// Token: 0x0600364E RID: 13902 RVA: 0x0010121C File Offset: 0x000FF41C
	private void Awake()
	{
		this.tbb = base.GetComponent<ThrowableBugBeacon>();
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x0010122A File Offset: 0x000FF42A
	private void OnEnable()
	{
		base.StartCoroutine(this.SendSignals());
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x00019935 File Offset: 0x00017B35
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x00101239 File Offset: 0x000FF439
	private IEnumerator SendSignals()
	{
		uint count = 0U;
		while (this.signalCount == 0U || count < this.signalCount)
		{
			yield return new WaitForSeconds(Random.Range(this.minCallTime, this.maxCallTime));
			switch (this.mode)
			{
			case ThrowableBugBeaconActivation.ActivationMode.CALL:
				this.tbb.Call();
				break;
			case ThrowableBugBeaconActivation.ActivationMode.DISMISS:
				this.tbb.Dismiss();
				break;
			case ThrowableBugBeaconActivation.ActivationMode.LOCK:
				this.tbb.Lock();
				break;
			}
			uint num = count;
			count = num + 1U;
		}
		yield break;
	}

	// Token: 0x04003898 RID: 14488
	[SerializeField]
	private float minCallTime = 1f;

	// Token: 0x04003899 RID: 14489
	[SerializeField]
	private float maxCallTime = 5f;

	// Token: 0x0400389A RID: 14490
	[SerializeField]
	private uint signalCount;

	// Token: 0x0400389B RID: 14491
	[SerializeField]
	private ThrowableBugBeaconActivation.ActivationMode mode;

	// Token: 0x0400389C RID: 14492
	private ThrowableBugBeacon tbb;

	// Token: 0x020008C8 RID: 2248
	private enum ActivationMode
	{
		// Token: 0x0400389E RID: 14494
		CALL,
		// Token: 0x0400389F RID: 14495
		DISMISS,
		// Token: 0x040038A0 RID: 14496
		LOCK
	}
}
