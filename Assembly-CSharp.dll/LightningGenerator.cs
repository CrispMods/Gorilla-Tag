using System;
using UnityEngine;

// Token: 0x020008BA RID: 2234
public class LightningGenerator : MonoBehaviour
{
	// Token: 0x06003601 RID: 13825 RVA: 0x0013FD58 File Offset: 0x0013DF58
	private void Awake()
	{
		this.strikes = new LightningStrike[this.maxConcurrentStrikes];
		for (int i = 0; i < this.strikes.Length; i++)
		{
			if (i == 0)
			{
				this.strikes[i] = this.prototype;
			}
			else
			{
				this.strikes[i] = UnityEngine.Object.Instantiate<LightningStrike>(this.prototype, base.transform);
			}
			this.strikes[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x00052C85 File Offset: 0x00050E85
	private void OnEnable()
	{
		LightningDispatcher.RequestLightningStrike += this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x00052C98 File Offset: 0x00050E98
	private void OnDisable()
	{
		LightningDispatcher.RequestLightningStrike -= this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x00052CAB File Offset: 0x00050EAB
	private LightningStrike LightningDispatcher_RequestLightningStrike(Vector3 t1, Vector3 t2)
	{
		this.index = (this.index + 1) % this.strikes.Length;
		return this.strikes[this.index];
	}

	// Token: 0x0400382F RID: 14383
	[SerializeField]
	private uint maxConcurrentStrikes = 10U;

	// Token: 0x04003830 RID: 14384
	[SerializeField]
	private LightningStrike prototype;

	// Token: 0x04003831 RID: 14385
	private LightningStrike[] strikes;

	// Token: 0x04003832 RID: 14386
	private int index;
}
