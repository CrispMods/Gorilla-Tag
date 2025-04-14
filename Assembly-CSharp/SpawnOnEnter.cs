using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class SpawnOnEnter : MonoBehaviour
{
	// Token: 0x060004FB RID: 1275 RVA: 0x0001DAC4 File Offset: 0x0001BCC4
	public void OnTriggerEnter(Collider other)
	{
		if (Time.time > this.lastSpawnTime + this.cooldown)
		{
			this.lastSpawnTime = Time.time;
			ObjectPools.instance.Instantiate(this.prefab, other.transform.position);
		}
	}

	// Token: 0x040005D4 RID: 1492
	public GameObject prefab;

	// Token: 0x040005D5 RID: 1493
	public float cooldown = 0.1f;

	// Token: 0x040005D6 RID: 1494
	private float lastSpawnTime;
}
