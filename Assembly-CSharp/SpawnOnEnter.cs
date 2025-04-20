using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
public class SpawnOnEnter : MonoBehaviour
{
	// Token: 0x06000537 RID: 1335 RVA: 0x00033DA9 File Offset: 0x00031FA9
	public void OnTriggerEnter(Collider other)
	{
		if (Time.time > this.lastSpawnTime + this.cooldown)
		{
			this.lastSpawnTime = Time.time;
			ObjectPools.instance.Instantiate(this.prefab, other.transform.position);
		}
	}

	// Token: 0x04000614 RID: 1556
	public GameObject prefab;

	// Token: 0x04000615 RID: 1557
	public float cooldown = 0.1f;

	// Token: 0x04000616 RID: 1558
	private float lastSpawnTime;
}
