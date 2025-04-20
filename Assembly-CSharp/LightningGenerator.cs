using System;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class LightningGenerator : MonoBehaviour
{
	// Token: 0x060036BD RID: 14013 RVA: 0x00145318 File Offset: 0x00143518
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

	// Token: 0x060036BE RID: 14014 RVA: 0x000541A2 File Offset: 0x000523A2
	private void OnEnable()
	{
		LightningDispatcher.RequestLightningStrike += this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x000541B5 File Offset: 0x000523B5
	private void OnDisable()
	{
		LightningDispatcher.RequestLightningStrike -= this.LightningDispatcher_RequestLightningStrike;
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x000541C8 File Offset: 0x000523C8
	private LightningStrike LightningDispatcher_RequestLightningStrike(Vector3 t1, Vector3 t2)
	{
		this.index = (this.index + 1) % this.strikes.Length;
		return this.strikes[this.index];
	}

	// Token: 0x040038DE RID: 14558
	[SerializeField]
	private uint maxConcurrentStrikes = 10U;

	// Token: 0x040038DF RID: 14559
	[SerializeField]
	private LightningStrike prototype;

	// Token: 0x040038E0 RID: 14560
	private LightningStrike[] strikes;

	// Token: 0x040038E1 RID: 14561
	private int index;
}
