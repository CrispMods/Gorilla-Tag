using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class DelayedDestroyCrittersPooledObject : MonoBehaviour
{
	// Token: 0x06000298 RID: 664 RVA: 0x000310A3 File Offset: 0x0002F2A3
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x000310D1 File Offset: 0x0002F2D1
	protected void LateUpdate()
	{
		if (Time.time >= this.timeToDie)
		{
			CrittersPool.Return(base.gameObject);
		}
	}

	// Token: 0x0400034C RID: 844
	public float destroyDelay = 1f;

	// Token: 0x0400034D RID: 845
	private float timeToDie = -1f;
}
