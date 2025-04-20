using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class CosmeticCritterSpawnerButterflyNet : CosmeticCritterSpawner
{
	// Token: 0x060003EB RID: 1003 RVA: 0x00032F33 File Offset: 0x00031133
	public override bool CanSpawn()
	{
		return base.CanSpawn();
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0007A6C0 File Offset: 0x000788C0
	public override bool TrySpawn(int seed, out CosmeticCritter critter)
	{
		if (!base.TrySpawn(seed, out critter))
		{
			return false;
		}
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(critter.Seed);
		((CosmeticCritterButterfly)critter).SetStartPos(base.transform.position + UnityEngine.Random.onUnitSphere * this.spawnRadius);
		UnityEngine.Random.state = state;
		return true;
	}

	// Token: 0x0400046C RID: 1132
	[SerializeField]
	private float spawnRadius = 1f;
}
