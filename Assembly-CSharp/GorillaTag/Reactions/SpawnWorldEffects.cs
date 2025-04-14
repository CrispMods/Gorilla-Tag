using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BCA RID: 3018
	public class SpawnWorldEffects : MonoBehaviour
	{
		// Token: 0x06004C29 RID: 19497 RVA: 0x00172FD8 File Offset: 0x001711D8
		protected void OnEnable()
		{
			if (GorillaComputer.instance == null)
			{
				Debug.LogError("SpawnWorldEffects: Disabling because GorillaComputer not found! Hierarchy path: " + base.transform.GetPath(), this);
				base.enabled = false;
				return;
			}
			if (this._prefabToSpawn != null && !this._isPrefabInPool)
			{
				if (this._prefabToSpawn.CompareTag("Untagged"))
				{
					Debug.LogError("SpawnWorldEffects: Disabling because Spawn Prefab has no tag! Hierarchy path: " + base.transform.GetPath(), this);
					base.enabled = false;
					return;
				}
				this._isPrefabInPool = ObjectPools.instance.DoesPoolExist(this._prefabToSpawn);
				if (!this._isPrefabInPool)
				{
					Debug.LogError("SpawnWorldEffects: Disabling because Spawn Prefab not in pool! Hierarchy path: " + base.transform.GetPath(), this);
					base.enabled = false;
					return;
				}
				this._pool = ObjectPools.instance.GetPoolByObjectType(this._prefabToSpawn);
			}
			this._hasPrefabToSpawn = (this._prefabToSpawn != null && this._isPrefabInPool);
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001730DC File Offset: 0x001712DC
		public void RequestSpawn(Vector3 worldPosition)
		{
			this.RequestSpawn(worldPosition, Vector3.up);
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x001730EC File Offset: 0x001712EC
		public void RequestSpawn(Vector3 worldPosition, Vector3 normal)
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
			if (this._hasPrefabToSpawn && this._isPrefabInPool && this._pool.GetInactiveCount() > 0)
			{
				FireManager.SpawnFire(this._pool, worldPosition, normal, base.transform.lossyScale.x);
			}
			this._lastCollisionTime = num;
		}

		// Token: 0x04004E14 RID: 19988
		[Tooltip("The defaults are numbers for the flamethrower hair dryer.")]
		private readonly float _maxParticleHitReactionRate = 2f;

		// Token: 0x04004E15 RID: 19989
		[Tooltip("Must be in the global object pool and have a tag.")]
		[SerializeField]
		private GameObject _prefabToSpawn;

		// Token: 0x04004E16 RID: 19990
		private bool _hasPrefabToSpawn;

		// Token: 0x04004E17 RID: 19991
		private bool _isPrefabInPool;

		// Token: 0x04004E18 RID: 19992
		private double _lastCollisionTime;

		// Token: 0x04004E19 RID: 19993
		private SinglePool _pool;
	}
}
