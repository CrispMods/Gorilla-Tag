using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000518 RID: 1304
public class CosmeticCritterManager : NetworkSceneObject
{
	// Token: 0x06001FB5 RID: 8117 RVA: 0x0004581C File Offset: 0x00043A1C
	public void RegisterComponent(CosmeticCritterSpawner spawner)
	{
		this.critterSpawners.Add(spawner);
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x0004582B File Offset: 0x00043A2B
	public void UnregisterComponent(CosmeticCritterSpawner spawner)
	{
		this.critterSpawners.Remove(spawner);
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x0004583A File Offset: 0x00043A3A
	public void RegisterComponent(CosmeticCritterCatcher catcher)
	{
		if (catcher.isLocal)
		{
			this.localCritterCatchers.Add(catcher);
			return;
		}
		this.remoteCritterCatchers.Add(catcher);
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x0004585F File Offset: 0x00043A5F
	public void UnregisterComponent(CosmeticCritterCatcher catcher)
	{
		this.localCritterCatchers.Remove(catcher);
		this.remoteCritterCatchers.Remove(catcher);
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000F0120 File Offset: 0x000EE320
	private void Awake()
	{
		if (CosmeticCritterManager.Instance != null && CosmeticCritterManager.Instance != this)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		CosmeticCritterManager.Instance = this;
		this.critterSpawners = new HashSet<CosmeticCritterSpawner>();
		this.localCritterCatchers = new HashSet<CosmeticCritterCatcher>();
		this.remoteCritterCatchers = new HashSet<CosmeticCritterCatcher>();
		this.crittersBySeed = new Dictionary<int, CosmeticCritter>();
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x0004587B File Offset: 0x00043A7B
	private void DestroyCritter(CosmeticCritter critter)
	{
		UnityEngine.Object.Destroy(critter.gameObject);
		if (critter.Spawner)
		{
			critter.Spawner.OnDespawn();
		}
		this.crittersBySeed.Remove(critter.Seed);
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000F0180 File Offset: 0x000EE380
	private void Update()
	{
		if (PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom)
		{
			foreach (CosmeticCritterSpawner cosmeticCritterSpawner in this.critterSpawners)
			{
				if (cosmeticCritterSpawner.CanSpawn())
				{
					int num = UnityEngine.Random.Range(0, int.MaxValue);
					CosmeticCritter value;
					if (cosmeticCritterSpawner.TrySpawn(num, out value))
					{
						this.crittersBySeed.Add(num, value);
					}
				}
			}
		}
		foreach (CosmeticCritter cosmeticCritter in this.crittersBySeed.Values.ToList<CosmeticCritter>())
		{
			if (cosmeticCritter.Expired())
			{
				this.DestroyCritter(cosmeticCritter);
			}
			else
			{
				cosmeticCritter.Tick();
				foreach (CosmeticCritterCatcher cosmeticCritterCatcher in this.localCritterCatchers)
				{
					if (cosmeticCritterCatcher.TryToCatch(cosmeticCritter))
					{
						cosmeticCritterCatcher.Catch(cosmeticCritter);
						this.DestroyCritter(cosmeticCritter);
					}
				}
			}
		}
	}

	// Token: 0x04002384 RID: 9092
	public static CosmeticCritterManager Instance;

	// Token: 0x04002385 RID: 9093
	private HashSet<CosmeticCritterSpawner> critterSpawners;

	// Token: 0x04002386 RID: 9094
	private HashSet<CosmeticCritterCatcher> localCritterCatchers;

	// Token: 0x04002387 RID: 9095
	private HashSet<CosmeticCritterCatcher> remoteCritterCatchers;

	// Token: 0x04002388 RID: 9096
	private Dictionary<int, CosmeticCritter> crittersBySeed;
}
