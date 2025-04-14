using System;
using UnityEngine;

// Token: 0x02000844 RID: 2116
public class DelayedDestroyPooledObj : MonoBehaviour
{
	// Token: 0x06003393 RID: 13203 RVA: 0x000F64F3 File Offset: 0x000F46F3
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x000F6521 File Offset: 0x000F4721
	protected void LateUpdate()
	{
		if (Time.time > this.timeToDie)
		{
			ObjectPools.instance.Destroy(base.gameObject);
		}
	}

	// Token: 0x040036DE RID: 14046
	[Tooltip("Return to the object pool after this many seconds.")]
	public float destroyDelay;

	// Token: 0x040036DF RID: 14047
	private float timeToDie = -1f;
}
