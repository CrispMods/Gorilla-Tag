using System;
using UnityEngine;

namespace GorillaTagScripts.Builder
{
	// Token: 0x020009FA RID: 2554
	public class BuilderParticleSpawner : MonoBehaviour
	{
		// Token: 0x06003FD8 RID: 16344 RVA: 0x0012ED40 File Offset: 0x0012CF40
		private void Start()
		{
			this.spawnTrigger.onTriggerFirstEntered += this.OnEnter;
			this.spawnTrigger.onTriggerLastExited += this.OnExit;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0012ED70 File Offset: 0x0012CF70
		private void OnDestroy()
		{
			if (this.spawnTrigger != null)
			{
				this.spawnTrigger.onTriggerFirstEntered -= this.OnEnter;
				this.spawnTrigger.onTriggerLastExited -= this.OnExit;
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0012EDB0 File Offset: 0x0012CFB0
		public void TrySpawning()
		{
			if (Time.time > this.lastSpawnTime + this.cooldown)
			{
				this.lastSpawnTime = Time.time;
				ObjectPools.instance.Instantiate(this.prefab, this.spawnLocation.position, this.spawnLocation.rotation);
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0012EE03 File Offset: 0x0012D003
		private void OnEnter()
		{
			if (this.spawnOnEnter)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x0012EE13 File Offset: 0x0012D013
		private void OnExit()
		{
			if (this.spawnOnExit)
			{
				this.TrySpawning();
			}
		}

		// Token: 0x040040DD RID: 16605
		public GameObject prefab;

		// Token: 0x040040DE RID: 16606
		public float cooldown = 0.1f;

		// Token: 0x040040DF RID: 16607
		private float lastSpawnTime;

		// Token: 0x040040E0 RID: 16608
		[SerializeField]
		private BuilderSmallMonkeTrigger spawnTrigger;

		// Token: 0x040040E1 RID: 16609
		[SerializeField]
		private bool spawnOnEnter = true;

		// Token: 0x040040E2 RID: 16610
		[SerializeField]
		private bool spawnOnExit;

		// Token: 0x040040E3 RID: 16611
		[SerializeField]
		private Transform spawnLocation;
	}
}
