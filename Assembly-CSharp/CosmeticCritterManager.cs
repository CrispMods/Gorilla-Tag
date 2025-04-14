using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class CosmeticCritterManager : NetworkSceneObject
{
	// Token: 0x06001F5C RID: 8028 RVA: 0x0009E39C File Offset: 0x0009C59C
	public void RegisterComponent(CosmeticCritterSpawner spawner)
	{
		this.critterSpawners.Add(spawner);
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x0009E3AB File Offset: 0x0009C5AB
	public void UnregisterComponent(CosmeticCritterSpawner spawner)
	{
		this.critterSpawners.Remove(spawner);
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x0009E3BA File Offset: 0x0009C5BA
	public void RegisterComponent(CosmeticCritterCatcher catcher)
	{
		if (catcher.isLocal)
		{
			this.localCritterCatchers.Add(catcher);
			return;
		}
		this.remoteCritterCatchers.Add(catcher);
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x0009E3DF File Offset: 0x0009C5DF
	public void UnregisterComponent(CosmeticCritterCatcher catcher)
	{
		this.localCritterCatchers.Remove(catcher);
		this.remoteCritterCatchers.Remove(catcher);
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x0009E3FC File Offset: 0x0009C5FC
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

	// Token: 0x06001F61 RID: 8033 RVA: 0x0009E45C File Offset: 0x0009C65C
	private void DestroyCritter(CosmeticCritter critter)
	{
		UnityEngine.Object.Destroy(critter.gameObject);
		if (critter.Spawner)
		{
			critter.Spawner.OnDespawn();
		}
		this.crittersBySeed.Remove(critter.Seed);
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x0009E494 File Offset: 0x0009C694
	private void Update()
	{
		if (PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom)
		{
			foreach (CosmeticCritterSpawner cosmeticCritterSpawner in this.critterSpawners)
			{
				if (cosmeticCritterSpawner.CanSpawn())
				{
					int num = Random.Range(0, int.MaxValue);
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

	// Token: 0x04002331 RID: 9009
	public static CosmeticCritterManager Instance;

	// Token: 0x04002332 RID: 9010
	private HashSet<CosmeticCritterSpawner> critterSpawners;

	// Token: 0x04002333 RID: 9011
	private HashSet<CosmeticCritterCatcher> localCritterCatchers;

	// Token: 0x04002334 RID: 9012
	private HashSet<CosmeticCritterCatcher> remoteCritterCatchers;

	// Token: 0x04002335 RID: 9013
	private Dictionary<int, CosmeticCritter> crittersBySeed;
}
