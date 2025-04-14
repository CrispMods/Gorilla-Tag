using System;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
public class LightningGenerator : MonoBehaviour
{
	// Token: 0x060035F5 RID: 13813 RVA: 0x000FF79C File Offset: 0x000FD99C
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
				this.strikes[i] = Object.Instantiate<LightningStrike>(this.prototype, base.transform);
			}
			this.strikes[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x000FF80C File Offset: 0x000FDA0C
	private void OnEnable()
	{
		LightningDispatcher.RequestLightningStrike += this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x000FF81F File Offset: 0x000FDA1F
	private void OnDisable()
	{
		LightningDispatcher.RequestLightningStrike -= this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x000FF832 File Offset: 0x000FDA32
	private LightningStrike LightningDispatcher_RequestLightningStrike(Vector3 t1, Vector3 t2)
	{
		this.index = (this.index + 1) % this.strikes.Length;
		return this.strikes[this.index];
	}

	// Token: 0x0400381D RID: 14365
	[SerializeField]
	private uint maxConcurrentStrikes = 10U;

	// Token: 0x0400381E RID: 14366
	[SerializeField]
	private LightningStrike prototype;

	// Token: 0x0400381F RID: 14367
	private LightningStrike[] strikes;

	// Token: 0x04003820 RID: 14368
	private int index;
}
