﻿using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BF0 RID: 3056
	public class FlameThrowerParticleCollisionHandler : MonoBehaviour
	{
		// Token: 0x06004D5F RID: 19807 RVA: 0x001AAD74 File Offset: 0x001A8F74
		protected void OnEnable()
		{
			if (GorillaComputer.instance == null)
			{
				Debug.LogError("FlameThrowerParticleCollisionHandler: Disabling because GorillaComputer not found! Hierarchy path: " + base.transform.GetPath(), this);
				base.enabled = false;
				return;
			}
			if (this._prefabToSpawn != null && !this._isPrefabInPool)
			{
				if (this._prefabToSpawn.CompareTag("Untagged"))
				{
					Debug.LogError("FlameThrowerParticleCollisionHandler: Disabling because Spawn Prefab has no tag! Hierarchy path: " + base.transform.GetPath(), this);
					base.enabled = false;
					return;
				}
				this._isPrefabInPool = ObjectPools.instance.DoesPoolExist(this._prefabToSpawn);
				if (!this._isPrefabInPool)
				{
					Debug.LogError("FlameThrowerParticleCollisionHandler: Disabling because Spawn Prefab not in pool! Hierarchy path: " + base.transform.GetPath(), this);
					base.enabled = false;
					return;
				}
				this._pool = ObjectPools.instance.GetPoolByObjectType(this._prefabToSpawn);
			}
			this._hasPrefabToSpawn = (this._prefabToSpawn != null && this._isPrefabInPool);
			if (this._particleSystem == null)
			{
				this._particleSystem = base.GetComponent<ParticleSystem>();
			}
			if (this._particleSystem == null)
			{
				Debug.LogError("FlameThrowerParticleCollisionHandler: Disabling because could not find ParticleSystem! Hierarchy path: " + base.transform.GetPath(), this);
				base.enabled = false;
				return;
			}
			if (this._collisionEvents == null)
			{
				this._collisionEvents = new List<ParticleCollisionEvent>(this._particleSystem.main.maxParticles);
			}
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x001AAEEC File Offset: 0x001A90EC
		protected void OnParticleCollision(GameObject other)
		{
			if (this._maxParticleHitReactionRate < 1E-05f || !FireManager.hasInstance)
			{
				return;
			}
			double num = GTTime.TimeAsDouble();
			if ((float)(num - this._lastCollisionTime) < 1f / this._maxParticleHitReactionRate)
			{
				return;
			}
			if (this._particleSystem.GetCollisionEvents(other, this._collisionEvents) <= 0)
			{
				return;
			}
			if (this._hasPrefabToSpawn && this._isPrefabInPool && this._pool.GetInactiveCount() > 0)
			{
				ParticleCollisionEvent particleCollisionEvent = this._collisionEvents[0];
				FireManager.SpawnFire(this._pool, particleCollisionEvent.intersection, particleCollisionEvent.normal, base.transform.lossyScale.x);
			}
			if (this._extinguishAmount > 0f)
			{
				FireManager.Extinguish(other, this._extinguishAmount);
			}
			this._lastCollisionTime = num;
		}

		// Token: 0x04004ECF RID: 20175
		[Tooltip("The defaults are numbers for the flamethrower hair dryer.")]
		private readonly float _maxParticleHitReactionRate = 2f;

		// Token: 0x04004ED0 RID: 20176
		[Tooltip("Must be in the global object pool and have a tag.")]
		[SerializeField]
		private GameObject _prefabToSpawn;

		// Token: 0x04004ED1 RID: 20177
		[Tooltip("How much to extinguish any hit fire by.")]
		[SerializeField]
		private float _extinguishAmount;

		// Token: 0x04004ED2 RID: 20178
		private ParticleSystem _particleSystem;

		// Token: 0x04004ED3 RID: 20179
		private List<ParticleCollisionEvent> _collisionEvents;

		// Token: 0x04004ED4 RID: 20180
		private bool _hasPrefabToSpawn;

		// Token: 0x04004ED5 RID: 20181
		private bool _isPrefabInPool;

		// Token: 0x04004ED6 RID: 20182
		private double _lastCollisionTime;

		// Token: 0x04004ED7 RID: 20183
		private SinglePool _pool;
	}
}
