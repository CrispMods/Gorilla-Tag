using System;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public abstract class CosmeticCritterSpawner : CosmeticCritterHoldable
{
	// Token: 0x06001F67 RID: 8039 RVA: 0x0009E958 File Offset: 0x0009CB58
	public virtual bool CanSpawn()
	{
		return this.numCritters < this.maxCritters && Time.time > this.nextSpawnTime;
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x0009E978 File Offset: 0x0009CB78
	public virtual bool TrySpawn(int seed, out CosmeticCritter critter)
	{
		if (this.numCritters >= this.maxCritters)
		{
			critter = null;
			return false;
		}
		critter = Object.Instantiate<GameObject>(this.critterPrefab).GetComponent<CosmeticCritter>();
		critter.Init(seed, base.playerID, this);
		this.numCritters++;
		this.nextSpawnTime = Time.time + Random.Range(this.spawnIntervalMinMax.x, this.spawnIntervalMinMax.y);
		return true;
	}

	// Token: 0x06001F69 RID: 8041 RVA: 0x0009E9EF File Offset: 0x0009CBEF
	public virtual void OnDespawn()
	{
		this.numCritters--;
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x0009E9FF File Offset: 0x0009CBFF
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x0009EA12 File Offset: 0x0009CC12
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}

	// Token: 0x04002337 RID: 9015
	[SerializeField]
	private GameObject critterPrefab;

	// Token: 0x04002338 RID: 9016
	[SerializeField]
	private int maxCritters;

	// Token: 0x04002339 RID: 9017
	[SerializeField]
	private Vector2 spawnIntervalMinMax = new Vector2(2f, 5f);

	// Token: 0x0400233A RID: 9018
	private int numCritters;

	// Token: 0x0400233B RID: 9019
	private float nextSpawnTime;
}
