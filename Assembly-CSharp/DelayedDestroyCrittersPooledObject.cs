using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class DelayedDestroyCrittersPooledObject : MonoBehaviour
{
	// Token: 0x06000296 RID: 662 RVA: 0x00010F8F File Offset: 0x0000F18F
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00010FBD File Offset: 0x0000F1BD
	protected void LateUpdate()
	{
		if (Time.time >= this.timeToDie)
		{
			CrittersPool.Return(base.gameObject);
		}
	}

	// Token: 0x0400034B RID: 843
	public float destroyDelay = 1f;

	// Token: 0x0400034C RID: 844
	private float timeToDie = -1f;
}
