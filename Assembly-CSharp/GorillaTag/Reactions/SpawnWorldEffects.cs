using System;
using GorillaExtensions;
using GorillaNetworking;
using UnityEngine;

namespace GorillaTag.Reactions
{
	// Token: 0x02000BF8 RID: 3064
	public class SpawnWorldEffects : MonoBehaviour
	{
		// Token: 0x06004D75 RID: 19829 RVA: 0x001ABA10 File Offset: 0x001A9C10
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

		// Token: 0x06004D76 RID: 19830 RVA: 0x00062CB6 File Offset: 0x00060EB6
		public void RequestSpawn(Vector3 worldPosition)
		{
			this.RequestSpawn(worldPosition, Vector3.up);
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x001ABB14 File Offset: 0x001A9D14
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

		// Token: 0x04004F0A RID: 20234
		[Tooltip("The defaults are numbers for the flamethrower hair dryer.")]
		private readonly float _maxParticleHitReactionRate = 2f;

		// Token: 0x04004F0B RID: 20235
		[Tooltip("Must be in the global object pool and have a tag.")]
		[SerializeField]
		private GameObject _prefabToSpawn;

		// Token: 0x04004F0C RID: 20236
		private bool _hasPrefabToSpawn;

		// Token: 0x04004F0D RID: 20237
		private bool _isPrefabInPool;

		// Token: 0x04004F0E RID: 20238
		private double _lastCollisionTime;

		// Token: 0x04004F0F RID: 20239
		private SinglePool _pool;
	}
}
