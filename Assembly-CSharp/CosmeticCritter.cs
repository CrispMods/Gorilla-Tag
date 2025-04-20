using System;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public abstract class CosmeticCritter : MonoBehaviour
{
	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x00045766 File Offset: 0x00043966
	// (set) Token: 0x06001FA1 RID: 8097 RVA: 0x0004576E File Offset: 0x0004396E
	public int Seed { get; protected set; }

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x00045777 File Offset: 0x00043977
	// (set) Token: 0x06001FA3 RID: 8099 RVA: 0x0004577F File Offset: 0x0004397F
	public int OwnerID { get; protected set; }

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x00045788 File Offset: 0x00043988
	// (set) Token: 0x06001FA5 RID: 8101 RVA: 0x00045790 File Offset: 0x00043990
	public CosmeticCritterSpawner Spawner { get; protected set; }

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00045799 File Offset: 0x00043999
	public virtual void Init(int seed, int ownerID, CosmeticCritterSpawner spawner)
	{
		this.Seed = seed;
		this.OwnerID = ownerID;
		this.Spawner = spawner;
		this.startTime = PhotonNetwork.Time;
	}

	// Token: 0x06001FA7 RID: 8103
	public abstract void Tick();

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000457BB File Offset: 0x000439BB
	public virtual bool Expired()
	{
		return this.startTime + (double)this.lifetime < PhotonNetwork.Time;
	}

	// Token: 0x0400237F RID: 9087
	[SerializeField]
	protected float lifetime;

	// Token: 0x04002380 RID: 9088
	protected double startTime;
}
