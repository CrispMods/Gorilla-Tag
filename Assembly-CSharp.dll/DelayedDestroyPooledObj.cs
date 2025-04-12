using System;
using UnityEngine;

// Token: 0x02000847 RID: 2119
public class DelayedDestroyPooledObj : MonoBehaviour
{
	// Token: 0x0600339F RID: 13215 RVA: 0x000512B1 File Offset: 0x0004F4B1
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x000512DF File Offset: 0x0004F4DF
	protected void LateUpdate()
	{
		if (Time.time > this.timeToDie)
		{
			ObjectPools.instance.Destroy(base.gameObject);
		}
	}

	// Token: 0x040036F0 RID: 14064
	[Tooltip("Return to the object pool after this many seconds.")]
	public float destroyDelay;

	// Token: 0x040036F1 RID: 14065
	private float timeToDie = -1f;
}
