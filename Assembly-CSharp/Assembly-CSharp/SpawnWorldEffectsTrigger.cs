using System;
using GorillaTag.Reactions;
using UnityEngine;

// Token: 0x020008A1 RID: 2209
[RequireComponent(typeof(SpawnWorldEffects))]
public class SpawnWorldEffectsTrigger : MonoBehaviour
{
	// Token: 0x0600356A RID: 13674 RVA: 0x000FE7FA File Offset: 0x000FC9FA
	private void OnEnable()
	{
		if (this.swe == null)
		{
			this.swe = base.GetComponent<SpawnWorldEffects>();
		}
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x000FE816 File Offset: 0x000FCA16
	private void OnTriggerEnter(Collider other)
	{
		this.spawnTime = Time.time;
		this.swe.RequestSpawn(base.transform.position);
	}

	// Token: 0x0600356C RID: 13676 RVA: 0x000FE839 File Offset: 0x000FCA39
	private void OnTriggerStay(Collider other)
	{
		if (Time.time - this.spawnTime < this.spawnCooldown)
		{
			return;
		}
		this.swe.RequestSpawn(base.transform.position);
		this.spawnTime = Time.time;
	}

	// Token: 0x040037CE RID: 14286
	private SpawnWorldEffects swe;

	// Token: 0x040037CF RID: 14287
	private float spawnTime;

	// Token: 0x040037D0 RID: 14288
	[SerializeField]
	private float spawnCooldown = 1f;
}
