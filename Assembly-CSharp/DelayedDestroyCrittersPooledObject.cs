using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class DelayedDestroyCrittersPooledObject : MonoBehaviour
{
	// Token: 0x060002C4 RID: 708 RVA: 0x0003220D File Offset: 0x0003040D
	protected void OnEnable()
	{
		if (ObjectPools.instance == null || !ObjectPools.instance.initialized)
		{
			return;
		}
		this.timeToDie = Time.time + this.destroyDelay;
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0003223B File Offset: 0x0003043B
	protected void LateUpdate()
	{
		if (Time.time >= this.timeToDie)
		{
			CrittersPool.Return(base.gameObject);
		}
	}

	// Token: 0x0400037D RID: 893
	public float destroyDelay = 1f;

	// Token: 0x0400037E RID: 894
	private float timeToDie = -1f;
}
