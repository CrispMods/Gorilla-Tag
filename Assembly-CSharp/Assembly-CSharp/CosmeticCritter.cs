using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public abstract class CosmeticCritter : MonoBehaviour
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001F4A RID: 8010 RVA: 0x0009E61F File Offset: 0x0009C81F
	// (set) Token: 0x06001F4B RID: 8011 RVA: 0x0009E627 File Offset: 0x0009C827
	public int Seed { get; protected set; }

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001F4C RID: 8012 RVA: 0x0009E630 File Offset: 0x0009C830
	// (set) Token: 0x06001F4D RID: 8013 RVA: 0x0009E638 File Offset: 0x0009C838
	public int OwnerID { get; protected set; }

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001F4E RID: 8014 RVA: 0x0009E641 File Offset: 0x0009C841
	// (set) Token: 0x06001F4F RID: 8015 RVA: 0x0009E649 File Offset: 0x0009C849
	public CosmeticCritterSpawner Spawner { get; protected set; }

	// Token: 0x06001F50 RID: 8016 RVA: 0x0009E652 File Offset: 0x0009C852
	public virtual void Init(int seed, int ownerID, CosmeticCritterSpawner spawner)
	{
		this.Seed = seed;
		this.OwnerID = ownerID;
		this.Spawner = spawner;
		this.startTime = PhotonNetwork.Time;
	}

	// Token: 0x06001F51 RID: 8017
	public abstract void Tick();

	// Token: 0x06001F52 RID: 8018 RVA: 0x0009E674 File Offset: 0x0009C874
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
