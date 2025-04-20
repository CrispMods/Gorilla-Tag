using System;
using UnityEngine;

// Token: 0x02000519 RID: 1305
public abstract class CosmeticCritterSpawner : CosmeticCritterHoldable
{
	// Token: 0x06001FBD RID: 8125 RVA: 0x000458B2 File Offset: 0x00043AB2
	public virtual bool CanSpawn()
	{
		return this.numCritters < this.maxCritters && Time.time > this.nextSpawnTime;
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000F02C0 File Offset: 0x000EE4C0
	public virtual bool TrySpawn(int seed, out CosmeticCritter critter)
	{
		if (this.numCritters >= this.maxCritters)
		{
			critter = null;
			return false;
		}
		critter = UnityEngine.Object.Instantiate<GameObject>(this.critterPrefab).GetComponent<CosmeticCritter>();
		critter.Init(seed, base.playerID, this);
		this.numCritters++;
		this.nextSpawnTime = Time.time + UnityEngine.Random.Range(this.spawnIntervalMinMax.x, this.spawnIntervalMinMax.y);
		return true;
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000458D1 File Offset: 0x00043AD1
	public virtual void OnDespawn()
	{
		this.numCritters--;
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000458E1 File Offset: 0x00043AE1
	protected virtual void OnEnable()
	{
		base.TrySetID();
		CosmeticCritterManager.Instance.RegisterComponent(this);
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000458F4 File Offset: 0x00043AF4
	protected virtual void OnDisable()
	{
		CosmeticCritterManager.Instance.UnregisterComponent(this);
	}

	// Token: 0x04002389 RID: 9097
	[SerializeField]
	private GameObject critterPrefab;

	// Token: 0x0400238A RID: 9098
	[SerializeField]
	private int maxCritters;

	// Token: 0x0400238B RID: 9099
	[SerializeField]
	private Vector2 spawnIntervalMinMax = new Vector2(2f, 5f);

	// Token: 0x0400238C RID: 9100
	private int numCritters;

	// Token: 0x0400238D RID: 9101
	private float nextSpawnTime;
}
