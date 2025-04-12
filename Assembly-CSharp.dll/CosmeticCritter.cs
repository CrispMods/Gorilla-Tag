using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public abstract class CosmeticCritter : MonoBehaviour
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001F4A RID: 8010 RVA: 0x000443C7 File Offset: 0x000425C7
	// (set) Token: 0x06001F4B RID: 8011 RVA: 0x000443CF File Offset: 0x000425CF
	public int Seed { get; protected set; }

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000443D8 File Offset: 0x000425D8
	// (set) Token: 0x06001F4D RID: 8013 RVA: 0x000443E0 File Offset: 0x000425E0
	public int OwnerID { get; protected set; }

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000443E9 File Offset: 0x000425E9
	// (set) Token: 0x06001F4F RID: 8015 RVA: 0x000443F1 File Offset: 0x000425F1
	public CosmeticCritterSpawner Spawner { get; protected set; }

	// Token: 0x06001F50 RID: 8016 RVA: 0x000443FA File Offset: 0x000425FA
	public virtual void Init(int seed, int ownerID, CosmeticCritterSpawner spawner)
	{
		this.Seed = seed;
		this.OwnerID = ownerID;
		this.Spawner = spawner;
		this.startTime = PhotonNetwork.Time;
	}

	// Token: 0x06001F51 RID: 8017
	public abstract void Tick();

	// Token: 0x06001F52 RID: 8018 RVA: 0x0004441C File Offset: 0x0004261C
	public virtual bool Expired()
	{
		return this.startTime + (double)this.lifetime < PhotonNetwork.Time;
	}

	// Token: 0x0400232D RID: 9005
	[SerializeField]
	protected float lifetime;

	// Token: 0x0400232E RID: 9006
	protected double startTime;
}
