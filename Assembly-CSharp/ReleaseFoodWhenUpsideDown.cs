using System;
using Critters.Scripts;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class ReleaseFoodWhenUpsideDown : MonoBehaviour
{
	// Token: 0x06000325 RID: 805 RVA: 0x000326FE File Offset: 0x000308FE
	private void Awake()
	{
		this.latch = false;
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00076C98 File Offset: 0x00074E98
	private void Update()
	{
		if (!CrittersManager.instance.LocalAuthority())
		{
			return;
		}
		if (!this.dispenser.heldByPlayer)
		{
			return;
		}
		if (Vector3.Angle(base.transform.up, Vector3.down) < this.angle)
		{
			if (this.latch)
			{
				return;
			}
			this.latch = true;
			if (this.nextSpawnTime > (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time)))
			{
				return;
			}
			this.nextSpawnTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time)) + (double)this.spawnDelay;
			CrittersActor crittersActor = CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.Food, this.foodSubIndex);
			if (!crittersActor.IsNull())
			{
				CrittersFood crittersFood = (CrittersFood)crittersActor;
				crittersFood.MoveActor(this.spawnPoint.position, this.spawnPoint.rotation, false, true, true);
				crittersFood.SetImpulseVelocity(Vector3.zero, Vector3.zero);
				crittersFood.SpawnData(this.maxFood, this.startingFood, this.startingSize);
				return;
			}
		}
		else
		{
			this.latch = false;
		}
	}

	// Token: 0x040003C3 RID: 963
	public CrittersFoodDispenser dispenser;

	// Token: 0x040003C4 RID: 964
	public float angle = 30f;

	// Token: 0x040003C5 RID: 965
	private bool latch;

	// Token: 0x040003C6 RID: 966
	public Transform spawnPoint;

	// Token: 0x040003C7 RID: 967
	public float maxFood;

	// Token: 0x040003C8 RID: 968
	public float startingFood;

	// Token: 0x040003C9 RID: 969
	public float startingSize;

	// Token: 0x040003CA RID: 970
	public int foodSubIndex;

	// Token: 0x040003CB RID: 971
	public float spawnDelay = 0.6f;

	// Token: 0x040003CC RID: 972
	private double nextSpawnTime;
}
