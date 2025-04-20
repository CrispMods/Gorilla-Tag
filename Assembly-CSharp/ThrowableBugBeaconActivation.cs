using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class ThrowableBugBeaconActivation : MonoBehaviour
{
	// Token: 0x06003716 RID: 14102 RVA: 0x000545A6 File Offset: 0x000527A6
	private void Awake()
	{
		this.tbb = base.GetComponent<ThrowableBugBeacon>();
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x000545B4 File Offset: 0x000527B4
	private void OnEnable()
	{
		base.StartCoroutine(this.SendSignals());
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x00033636 File Offset: 0x00031836
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x000545C3 File Offset: 0x000527C3
	private IEnumerator SendSignals()
	{
		uint count = 0U;
		while (this.signalCount == 0U || count < this.signalCount)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(this.minCallTime, this.maxCallTime));
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

	// Token: 0x04003959 RID: 14681
	[SerializeField]
	private float minCallTime = 1f;

	// Token: 0x0400395A RID: 14682
	[SerializeField]
	private float maxCallTime = 5f;

	// Token: 0x0400395B RID: 14683
	[SerializeField]
	private uint signalCount;

	// Token: 0x0400395C RID: 14684
	[SerializeField]
	private ThrowableBugBeaconActivation.ActivationMode mode;

	// Token: 0x0400395D RID: 14685
	private ThrowableBugBeacon tbb;

	// Token: 0x020008E4 RID: 2276
	private enum ActivationMode
	{
		// Token: 0x0400395F RID: 14687
		CALL,
		// Token: 0x04003960 RID: 14688
		DISMISS,
		// Token: 0x04003961 RID: 14689
		LOCK
	}
}
