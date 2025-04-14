using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public abstract class CosmeticCritter : MonoBehaviour
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001F47 RID: 8007 RVA: 0x0009E29B File Offset: 0x0009C49B
	// (set) Token: 0x06001F48 RID: 8008 RVA: 0x0009E2A3 File Offset: 0x0009C4A3
	public int Seed { get; protected set; }

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0009E2AC File Offset: 0x0009C4AC
	// (set) Token: 0x06001F4A RID: 8010 RVA: 0x0009E2B4 File Offset: 0x0009C4B4
	public int OwnerID { get; protected set; }

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001F4B RID: 8011 RVA: 0x0009E2BD File Offset: 0x0009C4BD
	// (set) Token: 0x06001F4C RID: 8012 RVA: 0x0009E2C5 File Offset: 0x0009C4C5
	public CosmeticCritterSpawner Spawner { get; protected set; }

	// Token: 0x06001F4D RID: 8013 RVA: 0x0009E2CE File Offset: 0x0009C4CE
	public virtual void Init(int seed, int ownerID, CosmeticCritterSpawner spawner)
	{
		this.Seed = seed;
		this.OwnerID = ownerID;
		this.Spawner = spawner;
		this.startTime = PhotonNetwork.Time;
	}

	// Token: 0x06001F4E RID: 8014
	public abstract void Tick();

	// Token: 0x06001F4F RID: 8015 RVA: 0x0009E2F0 File Offset: 0x0009C4F0
	public virtual bool Expired()
	{
		return this.startTime + (double)this.lifetime < PhotonNetwork.Time;
	}

	// Token: 0x0400232C RID: 9004
	[SerializeField]
	protected float lifetime;

	// Token: 0x0400232D RID: 9005
	protected double startTime;
}
