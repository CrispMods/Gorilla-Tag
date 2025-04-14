using System;
using GorillaTag.Reactions;
using UnityEngine;

// Token: 0x0200089E RID: 2206
[RequireComponent(typeof(SpawnWorldEffects))]
public class SpawnWorldEffectsTrigger : MonoBehaviour
{
	// Token: 0x0600355E RID: 13662 RVA: 0x000FE232 File Offset: 0x000FC432
	private void OnEnable()
	{
		if (this.swe == null)
		{
			this.swe = base.GetComponent<SpawnWorldEffects>();
		}
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x000FE24E File Offset: 0x000FC44E
	private void OnTriggerEnter(Collider other)
	{
		this.spawnTime = Time.time;
		this.swe.RequestSpawn(base.transform.position);
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x000FE271 File Offset: 0x000FC471
	private void OnTriggerStay(Collider other)
	{
		if (Time.time - this.spawnTime < this.spawnCooldown)
		{
			return;
		}
		this.swe.RequestSpawn(base.transform.position);
		this.spawnTime = Time.time;
	}

	// Token: 0x040037BC RID: 14268
	private SpawnWorldEffects swe;

	// Token: 0x040037BD RID: 14269
	private float spawnTime;

	// Token: 0x040037BE RID: 14270
	[SerializeField]
	private float spawnCooldown = 1f;
}
