using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7C RID: 3196
	public class CrittersActorSpawner : MonoBehaviour
	{
		// Token: 0x060050AE RID: 20654 RVA: 0x0018860E File Offset: 0x0018680E
		private void Awake()
		{
			this.spawnPoint.OnSpawnChanged += this.HandleSpawnedActor;
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x00188628 File Offset: 0x00186828
		public void ProcessLocal()
		{
			if (!CrittersManager.instance.LocalAuthority())
			{
				return;
			}
			if (this.nextSpawnTime <= (double)Time.time)
			{
				this.nextSpawnTime = (double)(Time.time + (float)this.spawnDelay);
				if (this.currentSpawnedObject == null || !this.currentSpawnedObject.isEnabled)
				{
					this.SpawnActor();
				}
			}
			if (this.currentSpawnedObject.IsNotNull())
			{
				if (!this.currentSpawnedObject.isEnabled)
				{
					this.currentSpawnedObject = null;
					this.spawnPoint.SetSpawnedActor(null);
					return;
				}
				if (!this.insideSpawnerCheck.bounds.Contains(this.currentSpawnedObject.transform.position))
				{
					this.currentSpawnedObject.RemoveDespawnBlock();
					this.currentSpawnedObject = null;
					this.spawnPoint.SetSpawnedActor(null);
					return;
				}
				if (!this.VerifySpawnAttached())
				{
					this.currentSpawnedObject.RemoveDespawnBlock();
					this.currentSpawnedObject = null;
					this.spawnPoint.SetSpawnedActor(null);
				}
			}
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x00188722 File Offset: 0x00186922
		public void DoReset()
		{
			this.currentSpawnedObject = null;
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x0018872B File Offset: 0x0018692B
		private void HandleSpawnedActor(CrittersActor spawnedActor)
		{
			this.currentSpawnedObject = spawnedActor;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x00188734 File Offset: 0x00186934
		private void SpawnActor()
		{
			CrittersActor crittersActor = CrittersManager.instance.SpawnActor(this.actorType, this.subActorIndex);
			this.spawnPoint.SetSpawnedActor(crittersActor);
			if (crittersActor.IsNull())
			{
				return;
			}
			if (this.attachSpawnedObjectToSpawnLocation)
			{
				crittersActor.GrabbedBy(this.spawnPoint, true, default(Quaternion), default(Vector3), false);
				return;
			}
			crittersActor.MoveActor(this.spawnPoint.transform.position, this.spawnPoint.transform.rotation, false, true, true);
			crittersActor.rb.velocity = Vector3.zero;
			if (this.applyImpulseOnSpawn)
			{
				crittersActor.SetImpulse();
			}
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001887E0 File Offset: 0x001869E0
		private bool VerifySpawnAttached()
		{
			if (this.attachSpawnedObjectToSpawnLocation)
			{
				CrittersActor crittersActor;
				CrittersManager.instance.actorById.TryGetValue(this.currentSpawnedObject.parentActorId, out crittersActor);
				if (crittersActor.IsNull() || crittersActor != this.spawnPoint)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400532B RID: 21291
		public CrittersActorSpawnerPoint spawnPoint;

		// Token: 0x0400532C RID: 21292
		public CrittersActor currentSpawnedObject;

		// Token: 0x0400532D RID: 21293
		public CrittersActor.CrittersActorType actorType;

		// Token: 0x0400532E RID: 21294
		public int subActorIndex = -1;

		// Token: 0x0400532F RID: 21295
		public Collider insideSpawnerCheck;

		// Token: 0x04005330 RID: 21296
		public int spawnDelay = 5;

		// Token: 0x04005331 RID: 21297
		public bool applyImpulseOnSpawn = true;

		// Token: 0x04005332 RID: 21298
		public bool attachSpawnedObjectToSpawnLocation;

		// Token: 0x04005333 RID: 21299
		private double nextSpawnTime;
	}
}
