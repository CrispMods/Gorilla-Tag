using System;
using UnityEngine;

// Token: 0x0200085E RID: 2142
public class DelayedDestroyPooledObj : MonoBehaviour
{
	// Token: 0x0600344E RID: 13390 RVA: 0x000526BF File Offset: 0x000508BF
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x000526ED File Offset: 0x000508ED
	protected void LateUpdate()
	{
		if (Time.time > this.timeToDie)
		{
			ObjectPools.instance.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400379A RID: 14234
	[Tooltip("Return to the object pool after this many seconds.")]
	public float destroyDelay;

	// Token: 0x0400379B RID: 14235
	private float timeToDie = -1f;
}
