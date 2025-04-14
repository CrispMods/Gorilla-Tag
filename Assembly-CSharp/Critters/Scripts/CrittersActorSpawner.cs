using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C79 RID: 3193
	public class CrittersActorSpawner : MonoBehaviour
	{
		// Token: 0x060050A2 RID: 20642 RVA: 0x00188046 File Offset: 0x00186246
		private void Awake()
		{
			this.spawnPoint.OnSpawnChanged += this.HandleSpawnedActor;
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x00188060 File Offset: 0x00186260
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

		// Token: 0x060050A4 RID: 20644 RVA: 0x0018815A File Offset: 0x0018635A
		public void DoReset()
		{
			this.currentSpawnedObject = null;
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x00188163 File Offset: 0x00186363
		private void HandleSpawnedActor(CrittersActor spawnedActor)
		{
			this.currentSpawnedObject = spawnedActor;
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0018816C File Offset: 0x0018636C
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

		// Token: 0x060050A7 RID: 20647 RVA: 0x00188218 File Offset: 0x00186418
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

		// Token: 0x04005319 RID: 21273
		public CrittersActorSpawnerPoint spawnPoint;

		// Token: 0x0400531A RID: 21274
		public CrittersActor currentSpawnedObject;

		// Token: 0x0400531B RID: 21275
		public CrittersActor.CrittersActorType actorType;

		// Token: 0x0400531C RID: 21276
		public int subActorIndex = -1;

		// Token: 0x0400531D RID: 21277
		public Collider insideSpawnerCheck;

		// Token: 0x0400531E RID: 21278
		public int spawnDelay = 5;

		// Token: 0x0400531F RID: 21279
		public bool applyImpulseOnSpawn = true;

		// Token: 0x04005320 RID: 21280
		public bool attachSpawnedObjectToSpawnLocation;

		// Token: 0x04005321 RID: 21281
		private double nextSpawnTime;
	}
}
