using System;
using Critters.Scripts;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class ReleaseFoodWhenUpsideDown : MonoBehaviour
{
	// Token: 0x060002F4 RID: 756 RVA: 0x000124DD File Offset: 0x000106DD
	private void Awake()
	{
		this.latch = false;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000124E8 File Offset: 0x000106E8
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

	// Token: 0x0400038F RID: 911
	public CrittersFoodDispenser dispenser;

	// Token: 0x04000390 RID: 912
	public float angle = 30f;

	// Token: 0x04000391 RID: 913
	private bool latch;

	// Token: 0x04000392 RID: 914
	public Transform spawnPoint;

	// Token: 0x04000393 RID: 915
	public float maxFood;

	// Token: 0x04000394 RID: 916
	public float startingFood;

	// Token: 0x04000395 RID: 917
	public float startingSize;

	// Token: 0x04000396 RID: 918
	public int foodSubIndex;

	// Token: 0x04000397 RID: 919
	public float spawnDelay = 0.6f;

	// Token: 0x04000398 RID: 920
	private double nextSpawnTime;
}
