using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class SpawnOnEnter : MonoBehaviour
{
	// Token: 0x060004FD RID: 1277 RVA: 0x00032BA2 File Offset: 0x00030DA2
	public void OnTriggerEnter(Collider other)
	{
		if (Time.time > this.lastSpawnTime + this.cooldown)
		{
			this.lastSpawnTime = Time.time;
			ObjectPools.instance.Instantiate(this.prefab, other.transform.position);
		}
	}

	// Token: 0x040005D5 RID: 1493
	public GameObject prefab;

	// Token: 0x040005D6 RID: 1494
	public float cooldown = 0.1f;

	// Token: 0x040005D7 RID: 1495
	private float lastSpawnTime;
}
