using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000CAA RID: 3242
	public class CrittersActorSpawner : MonoBehaviour
	{
		// Token: 0x06005202 RID: 20994 RVA: 0x000652C7 File Offset: 0x000634C7
		private void Awake()
		{
			this.spawnPoint.OnSpawnChanged += this.HandleSpawnedActor;
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x000652E0 File Offset: 0x000634E0
		private void OnEnable()
		{
			if (!CrittersManager.instance.actorSpawners.Contains(this))
			{
				CrittersManager.instance.actorSpawners.Add(this);
			}
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x00065308 File Offset: 0x00063508
		private void OnDisable()
		{
			if (CrittersManager.instance.actorSpawners.Contains(this))
			{
				CrittersManager.instance.actorSpawners.Remove(this);
			}
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x001BF260 File Offset: 0x001BD460
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

		// Token: 0x06005206 RID: 20998 RVA: 0x00065331 File Offset: 0x00063531
		public void DoReset()
		{
			this.currentSpawnedObject = null;
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x0006533A File Offset: 0x0006353A
		private void HandleSpawnedActor(CrittersActor spawnedActor)
		{
			this.currentSpawnedObject = spawnedActor;
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x001BF35C File Offset: 0x001BD55C
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

		// Token: 0x06005209 RID: 21001 RVA: 0x001BF408 File Offset: 0x001BD608
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

		// Token: 0x04005425 RID: 21541
		public CrittersActorSpawnerPoint spawnPoint;

		// Token: 0x04005426 RID: 21542
		public CrittersActor currentSpawnedObject;

		// Token: 0x04005427 RID: 21543
		public CrittersActor.CrittersActorType actorType;

		// Token: 0x04005428 RID: 21544
		public int subActorIndex = -1;

		// Token: 0x04005429 RID: 21545
		public Collider insideSpawnerCheck;

		// Token: 0x0400542A RID: 21546
		public int spawnDelay = 5;

		// Token: 0x0400542B RID: 21547
		public bool applyImpulseOnSpawn = true;

		// Token: 0x0400542C RID: 21548
		public bool attachSpawnedObjectToSpawnLocation;

		// Token: 0x0400542D RID: 21549
		private double nextSpawnTime;
	}
}
