using System;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public abstract class CosmeticCritterSpawner : CosmeticCritterHoldable
{
	// Token: 0x06001F64 RID: 8036 RVA: 0x0009E5D4 File Offset: 0x0009C7D4
	public virtual bool CanSpawn()
	{
		return this.numCritters < this.maxCritters && Time.time > this.nextSpawnTime;
	}

	// Token: 0x06001F65 RID: 8037 RVA: 0x0009E5F4 File Offset: 0x0009C7F4
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

	// Token: 0x06001F66 RID: 8038 RVA: 0x0009E66B File Offset: 0x0009C86B
	public virtual void OnDespawn()
	{
		this.numCritters--;
	}

	// Token: 0x06001F67 RID: 8039 RVA: 0x0009E67B File Offset: 0x0009C87B
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001F68 RID: 8040 RVA: 0x0009E68E File Offset: 0x0009C88E
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}

	// Token: 0x04002336 RID: 9014
	[SerializeField]
	private GameObject critterPrefab;

	// Token: 0x04002337 RID: 9015
	[SerializeField]
	private int maxCritters;

	// Token: 0x04002338 RID: 9016
	[SerializeField]
	private Vector2 spawnIntervalMinMax = new Vector2(2f, 5f);

	// Token: 0x04002339 RID: 9017
	private int numCritters;

	// Token: 0x0400233A RID: 9018
	private float nextSpawnTime;
}
