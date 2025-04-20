using System;
using GorillaTag.Reactions;
using UnityEngine;

// Token: 0x020008BA RID: 2234
[RequireComponent(typeof(SpawnWorldEffects))]
public class SpawnWorldEffectsTrigger : MonoBehaviour
{
	// Token: 0x06003626 RID: 13862 RVA: 0x00053A1F File Offset: 0x00051C1F
	private void OnEnable()
	{
		if (this.swe == null)
		{
			this.swe = base.GetComponent<SpawnWorldEffects>();
		}
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x00053A3B File Offset: 0x00051C3B
	private void OnTriggerEnter(Collider other)
	{
		this.spawnTime = Time.time;
		this.swe.RequestSpawn(base.transform.position);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x00053A5E File Offset: 0x00051C5E
	private void OnTriggerStay(Collider other)
	{
		if (Time.time - this.spawnTime < this.spawnCooldown)
		{
			return;
		}
		this.swe.RequestSpawn(base.transform.position);
		this.spawnTime = Time.time;
	}

	// Token: 0x0400387D RID: 14461
	private SpawnWorldEffects swe;

	// Token: 0x0400387E RID: 14462
	private float spawnTime;

	// Token: 0x0400387F RID: 14463
	[SerializeField]
	private float spawnCooldown = 1f;
}
