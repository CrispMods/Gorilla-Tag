using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class ThrowableBugBeaconActivation : MonoBehaviour
{
	// Token: 0x0600365A RID: 13914 RVA: 0x00053089 File Offset: 0x00051289
	private void Awake()
	{
		this.tbb = base.GetComponent<ThrowableBugBeacon>();
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00053097 File Offset: 0x00051297
	private void OnEnable()
	{
		base.StartCoroutine(this.SendSignals());
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x0003242F File Offset: 0x0003062F
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x000530A6 File Offset: 0x000512A6
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

	// Token: 0x040038AA RID: 14506
	[SerializeField]
	private float minCallTime = 1f;

	// Token: 0x040038AB RID: 14507
	[SerializeField]
	private float maxCallTime = 5f;

	// Token: 0x040038AC RID: 14508
	[SerializeField]
	private uint signalCount;

	// Token: 0x040038AD RID: 14509
	[SerializeField]
	private ThrowableBugBeaconActivation.ActivationMode mode;

	// Token: 0x040038AE RID: 14510
	private ThrowableBugBeacon tbb;

	// Token: 0x020008CB RID: 2251
	private enum ActivationMode
	{
		// Token: 0x040038B0 RID: 14512
		CALL,
		// Token: 0x040038B1 RID: 14513
		DISMISS,
		// Token: 0x040038B2 RID: 14514
		LOCK
	}
}
